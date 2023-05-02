using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace Authenticator {
  public abstract class Authenticator : ICloneable {
    private const int SALT_LENGTH = 8;
    private const int PBKDF2_ITERATIONS = 2000;
    private const int PBKDF2_KEYSIZE = 256;
    private static string encryptionHeader = ByteArrayToString(Encoding.UTF8.GetBytes("SergiyeAuthenticator"));
    public const int DEFAULT_CODE_DIGITS = 6;
    public const int DEFAULT_PERIOD = 30;

    public enum PasswordTypes {
      None = 0,
      Explicit = 1,
      User = 2,
      Machine = 4,
    }

    public enum HmacTypes {
      SHA1 = 0,
      SHA256 = 1,
      SHA512 = 2
    }

    public const HmacTypes DEFAULT_HMAC_TYPE = HmacTypes.SHA1;

    #region Authenticator data

    public byte[] SecretKey { get; set; }
    public long ServerTimeDiff { get; set; }
    public long LastServerTime { get; set; }
    public PasswordTypes PasswordType { get; private set; }
    protected string Password { get; set; }
    protected byte[] SecretHash { get; private set; }
    public bool RequiresPassword { get; private set; }
    protected string EncryptedData { get; private set; }
    public int CodeDigits { get; set; }
    public HmacTypes HmacType { get; set; }
    public int Period { get; set; }
    public virtual string Issuer { get; set; }

    public virtual string SecretData {
      get => ByteArrayToString(SecretKey) + "\t" + CodeDigits + "\t" + HmacType + "\t" + Period;
      set {
        if (string.IsNullOrEmpty(value) == false) {
          var parts = value.Split('|')[0].Split('\t');
          SecretKey = StringToByteArray(parts[0]);
          if (parts.Length > 1) {
            if (int.TryParse(parts[1], out var digits)) {
              CodeDigits = digits;
            }
          }

          if (parts.Length > 2) {
            HmacType = (HmacTypes) Enum.Parse(typeof(HmacTypes), parts[2]);
          }

          if (parts.Length > 3) {
            if (int.TryParse(parts[3], out var period)) {
              Period = period;
            }
          }
        }
        else {
          SecretKey = null;
        }
      }
    }

    public long ServerTime => CurrentTime + ServerTimeDiff;

    public long CodeInterval =>
      // calculate the code interval; the server's time div 30,000
      (CurrentTime + ServerTimeDiff) / ((long) Period * 1000L);

    public string CurrentCode {
      get {
        if (SecretKey == null && EncryptedData != null) {
          throw new EncryptedSecretDataException();
        }
        return CalculateCode(false);
      }
    }

    #endregion

    static Authenticator() {
      // Issue#71: remove the default .net expect header, which can cause issues (http://stackoverflow.com/questions/566437/)
      System.Net.ServicePointManager.Expect100Continue = false;
    }

    public Authenticator(int codeDigits = DEFAULT_CODE_DIGITS, HmacTypes hmacType = HmacTypes.SHA1,
      int period = DEFAULT_PERIOD) {
      CodeDigits = codeDigits;
      HmacType = hmacType;
      Period = period;
    }

    protected virtual string CalculateCode(bool resync = false, long interval = -1) {
      // sync time if required
      if (resync || ServerTimeDiff == 0) {
        if (interval > 0) {
          ServerTimeDiff = (interval * ((long) Period * 1000L)) - CurrentTime;
        }
        else {
          Sync();
        }
      }

      HMac hmac;
      switch (HmacType) {
        case HmacTypes.SHA1:
          hmac = new HMac(new Sha1Digest());
          break;
        case HmacTypes.SHA256:
          hmac = new HMac(new Sha256Digest());
          break;
        case HmacTypes.SHA512:
          hmac = new HMac(new Sha512Digest());
          break;
        default:
          throw new InvalidHmacAlgorithmException();
      }

      hmac.Init(new KeyParameter(SecretKey));

      var codeIntervalArray = BitConverter.GetBytes(CodeInterval);
      if (BitConverter.IsLittleEndian) {
        Array.Reverse(codeIntervalArray);
      }

      hmac.BlockUpdate(codeIntervalArray, 0, codeIntervalArray.Length);

      var mac = new byte[hmac.GetMacSize()];
      hmac.DoFinal(mac, 0);

      // the last 4 bits of the mac say where the code starts (e.g. if last 4 bit are 1100, we start at byte 12)
      var start = mac.Last() & 0x0f;

      // extract those 4 bytes
      var bytes = new byte[4];
      Array.Copy(mac, start, bytes, 0, 4);
      if (BitConverter.IsLittleEndian) {
        Array.Reverse(bytes);
      }

      var fullcode = BitConverter.ToUInt32(bytes, 0) & 0x7fffffff;

      // we use the last 8 digits of this code in radix 10
      var codemask = (uint) Math.Pow(10, CodeDigits);
      var format = new string('0', CodeDigits);
      var code = (fullcode % codemask).ToString(format);

      return code;
    }

    public abstract void Sync();

    #region Load / Save

    public static Authenticator ReadXmlv2(XmlReader reader, string password = null) {
      Authenticator authenticator = null;
      var authenticatorType = reader.GetAttribute("type");
      if (string.IsNullOrEmpty(authenticatorType) == false) {
        authenticatorType = authenticatorType.Replace("WindowsAuthenticator.", "Authenticator.");
        var type = System.Reflection.Assembly.GetExecutingAssembly().GetType(authenticatorType, false, true);
        authenticator = Activator.CreateInstance(type) as Authenticator;
      }

      if (authenticator == null) {
        authenticator = new BattleNetAuthenticator();
      }

      reader.MoveToContent();
      if (reader.IsEmptyElement) {
        reader.Read();
        return null;
      }

      reader.Read();
      while (reader.EOF == false) {
        if (reader.IsStartElement()) {
          switch (reader.Name) {
            case "servertimediff":
              authenticator.ServerTimeDiff = reader.ReadElementContentAsLong();
              break;

            //case "restorecodeverified":
            //	authenticator.RestoreCodeVerified = reader.ReadElementContentAsBoolean();
            //	break;

            case "secretdata":
              var encrypted = reader.GetAttribute("encrypted");
              var data = reader.ReadElementContentAsString();

              var passwordType = DecodePasswordTypes(encrypted);

              if (passwordType != PasswordTypes.None) {
                // this is an old version so there is no hash
                data = DecryptSequence(data, passwordType, password);
              }

              authenticator.PasswordType = PasswordTypes.None;
              authenticator.SecretData = data;

              break;

            default:
              if (authenticator.ReadExtraXml(reader, reader.Name) == false) {
                reader.Skip();
              }

              break;
          }
        }
        else {
          reader.Read();
          break;
        }
      }

      return authenticator;
    }

    public virtual bool ReadExtraXml(XmlReader reader, string name) {
      return false;
    }

    public static PasswordTypes DecodePasswordTypes(string passwordTypes) {
      var passwordType = PasswordTypes.None;
      if (string.IsNullOrEmpty(passwordTypes)) {
        return passwordType;
      }

      var types = passwordTypes.ToCharArray();
      for (var i = types.Length - 1; i >= 0; i--) {
        var type = types[i];
        switch (type) {
          case 'u':
            passwordType |= PasswordTypes.User;
            break;
          case 'm':
            passwordType |= PasswordTypes.Machine;
            break;
          case 'y':
            passwordType |= PasswordTypes.Explicit;
            break;
        }
      }

      return passwordType;
    }

    public static string EncodePasswordTypes(PasswordTypes passwordType) {
      var encryptedTypes = new StringBuilder();
      if ((passwordType & PasswordTypes.Explicit) != 0) {
        encryptedTypes.Append("y");
      }

      if ((passwordType & PasswordTypes.User) != 0) {
        encryptedTypes.Append("u");
      }

      if ((passwordType & PasswordTypes.Machine) != 0) {
        encryptedTypes.Append("m");
      }

      return encryptedTypes.ToString();
    }

    public void SetEncryption(PasswordTypes passwordType, string password = null) {
      // check if still encrpyted
      if (RequiresPassword) {
        // have to decrypt to be able to re-encrypt
        throw new EncryptedSecretDataException();
      }

      if (passwordType == PasswordTypes.None) {
        RequiresPassword = false;
        EncryptedData = null;
        PasswordType = passwordType;
      }
      else {
        using (var ms = new MemoryStream()) {
          // get the plain version
          var settings = new XmlWriterSettings();
          settings.Indent = true;
          settings.Encoding = Encoding.UTF8;
          using (var encryptedwriter = XmlWriter.Create(ms, settings)) {
            var encrpytedData = EncryptedData;
            var savedpasswordType = PasswordType;
            try {
              PasswordType = PasswordTypes.None;
              EncryptedData = null;
              WriteToWriter(encryptedwriter);
            }
            finally {
              PasswordType = savedpasswordType;
              EncryptedData = encrpytedData;
            }
          }

          var data = ByteArrayToString(ms.ToArray());

          // update secret hash
          using (var sha1 = SHA1.Create()) {
            SecretHash = sha1.ComputeHash(Encoding.UTF8.GetBytes(SecretData));
          }

          // encrypt
          EncryptedData = EncryptSequence(data, passwordType, password);
          PasswordType = passwordType;
          if (PasswordType == PasswordTypes.Explicit) {
            SecretData = null;
            RequiresPassword = true;
          }
        }
      }
    }

    public void Protect() {
      if (PasswordType != PasswordTypes.None) {
        // check if the data has changed
        //if (this.SecretData != null)
        //{
        //	using (SHA1 sha1 = SHA1.Create())
        //	{
        //		byte[] secretHash = sha1.ComputeHash(Encoding.UTF8.GetBytes(this.SecretData));
        //		if (this.SecretHash == null || secretHash.SequenceEqual(this.SecretHash) == false)
        //		{
        //			// we need to encrypt changed secret data
        //			SetEncryption(this.PasswordType, this.Password);
        //		}
        //	}
        //}

        SecretData = null;
        RequiresPassword = true;
        Password = null;
      }
    }

    public bool Unprotect(string password) {
      var passwordType = PasswordType;
      if (passwordType == PasswordTypes.None) {
        throw new InvalidOperationException("Cannot Unprotect a non-encrypted authenticator");
      }

      // decrypt
      var changed = false;
      try {
        var data = DecryptSequence(EncryptedData, PasswordType, password);
        using (var ms = new MemoryStream(StringToByteArray(data))) {
          var reader = XmlReader.Create(ms);
          changed = ReadXml(reader, password) || changed;
        }

        RequiresPassword = false;
        // calculate hash of current secretdata
        using (var sha1 = SHA1.Create()) {
          SecretHash = sha1.ComputeHash(Encoding.UTF8.GetBytes(SecretData));
        }

        // keep the password until we reprotect in case data changes
        Password = password;

        if (changed) {
          // we need to encrypt changed secret data
          using (var ms = new MemoryStream()) {
            // get the plain version
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;
            using (var encryptedwriter = XmlWriter.Create(ms, settings)) {
              WriteToWriter(encryptedwriter);
            }

            var encrypteddata = ByteArrayToString(ms.ToArray());

            // update secret hash
            using (var sha1 = SHA1.Create()) {
              SecretHash = sha1.ComputeHash(Encoding.UTF8.GetBytes(SecretData));
            }

            // encrypt
            EncryptedData = EncryptSequence(encrypteddata, passwordType, password);
          }
        }

        return changed;
      }
      catch (EncryptedSecretDataException) {
        RequiresPassword = true;
        throw;
      }
      finally {
        PasswordType = passwordType;
      }
    }

    public bool ReadXml(XmlReader reader, string password = null) {
      // decode the password type
      var encrypted = reader.GetAttribute("encrypted");
      var passwordType = DecodePasswordTypes(encrypted);
      PasswordType = passwordType;

      if (passwordType != PasswordTypes.None) {
        // read the encrypted text from the node
        EncryptedData = reader.ReadElementContentAsString();
        return Unprotect(password);

        //// decrypt
        //try
        //{
        //	string data = Authenticator.DecryptSequence(this.EncryptedData, passwordType, password);
        //	using (MemoryStream ms = new MemoryStream(Authenticator.StringToByteArray(data)))
        //	{
        //		reader = XmlReader.Create(ms);
        //		this.ReadXml(reader, password);
        //	}
        //}
        //catch (EncryptedSecretDataException)
        //{
        //	this.RequiresPassword = true;
        //	throw;
        //}
        //finally
        //{
        //	this.PasswordType = passwordType;
        //}
      }

      reader.MoveToContent();
      if (reader.IsEmptyElement) {
        reader.Read();
        return false;
      }

      reader.Read();
      while (reader.EOF == false) {
        if (reader.IsStartElement()) {
          switch (reader.Name) {
            case "lastservertime":
              LastServerTime = reader.ReadElementContentAsLong();
              break;

            case "servertimediff":
              ServerTimeDiff = reader.ReadElementContentAsLong();
              break;

            case "secretdata":
              SecretData = reader.ReadElementContentAsString();
              break;

            default:
              if (ReadExtraXml(reader, reader.Name) == false) {
                reader.Skip();
              }

              break;
          }
        }
        else {
          reader.Read();
          break;
        }
      }

      // check if we need to sync, or if it's been a day
      if (this is HotpAuthenticator) {
        // no time sync
        return true;
      }
      else if (ServerTimeDiff == 0 || LastServerTime == 0 || LastServerTime < DateTime.Now.AddHours(-24).Ticks) {
        Sync();
        return true;
      }
      else {
        return false;
      }
    }

    public void WriteToWriter(XmlWriter writer) {
      writer.WriteStartElement("authenticatordata");
      //writer.WriteAttributeString("type", this.GetType().FullName);
      var encrypted = EncodePasswordTypes(PasswordType);
      if (string.IsNullOrEmpty(encrypted) == false) {
        writer.WriteAttributeString("encrypted", encrypted);
      }

      if (PasswordType != PasswordTypes.None) {
        writer.WriteRaw(EncryptedData);
      }
      else {
        writer.WriteStartElement("servertimediff");
        writer.WriteString(ServerTimeDiff.ToString());
        writer.WriteEndElement();
        //
        writer.WriteStartElement("lastservertime");
        writer.WriteString(LastServerTime.ToString());
        writer.WriteEndElement();
        //
        writer.WriteStartElement("secretdata");
        writer.WriteString(SecretData);
        writer.WriteEndElement();

        WriteExtraXml(writer);
      }

      writer.WriteEndElement();
    }

    protected virtual void WriteExtraXml(XmlWriter writer) {
    }

    #endregion

    #region Utility functions

    protected internal static byte[] CreateOneTimePad(int length) {
      // There is a MITM vulnerability from using the standard Random call
      // see https://docs.google.com/document/edit?id=1pf-YCgUnxR4duE8tr-xulE3rJ1Hw-Bm5aMk5tNOGU3E&hl=en
      // in http://code.google.com/p/winauth/issues/detail?id=2
      // so we switch out to use RNGCryptoServiceProvider instead of Random

      var random = new RNGCryptoServiceProvider();

      var randomblock = new byte[length];

      var sha1 = SHA1.Create();
      var i = 0;
      do {
        var hashBlock = new byte[128];
        random.GetBytes(hashBlock);

        var key = sha1.ComputeHash(hashBlock, 0, hashBlock.Length);
        if (key.Length >= randomblock.Length) {
          Array.Copy(key, 0, randomblock, i, randomblock.Length);
          break;
        }

        Array.Copy(key, 0, randomblock, i, key.Length);
        i += key.Length;
      } while (true);

      return randomblock;
    }

    public static long CurrentTime => Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds);

    public static byte[] StringToByteArray(string hex) {
      var len = hex.Length;
      var bytes = new byte[len / 2];
      for (var i = 0; i < len; i += 2) {
        bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
      }

      return bytes;
    }

    public static string ByteArrayToString(byte[] bytes) {
      // Use BitConverter, but it sticks dashes in the string
      return BitConverter.ToString(bytes).Replace("-", string.Empty);
    }

    public static string DecryptSequence(string data, PasswordTypes encryptedTypes, string password,
      bool decode = false) {
      // check for encrpytion header
      if (data.Length < encryptionHeader.Length || data.IndexOf(encryptionHeader) != 0) {
        return DecryptSequenceNoHash(data, encryptedTypes, password, decode);
      }

      // extract salt and hash
      //using (var sha = new SHA256Managed())
      using (var sha = SafeHasher("SHA256")) {
        // jump header
        var datastart = encryptionHeader.Length;
        var salt = data.Substring(datastart, Math.Min(SALT_LENGTH * 2, data.Length - datastart));
        datastart += salt.Length;
        var hash = data.Substring(datastart, Math.Min(sha.HashSize / 8 * 2, data.Length - datastart));
        datastart += hash.Length;
        data = data.Substring(datastart);

        data = DecryptSequenceNoHash(data, encryptedTypes, password);

        // check the hash
        var compareplain = StringToByteArray(salt + data);
        var comparehash = ByteArrayToString(sha.ComputeHash(compareplain));
        if (string.Compare(comparehash, hash) != 0) {
          throw new BadPasswordException();
        }
      }

      return data;
    }

    private static string DecryptSequenceNoHash(string data, PasswordTypes encryptedTypes, string password,
      bool decode = false) {
      try {
        // reverse order they were encrypted
        if ((encryptedTypes & PasswordTypes.Machine) != 0) {
          // we are going to decrypt with the Windows local machine key
          var cipher = StringToByteArray(data);
          var plain = ProtectedData.Unprotect(cipher, null, DataProtectionScope.LocalMachine);
          if (decode) {
            data = Encoding.UTF8.GetString(plain, 0, plain.Length);
          }
          else {
            data = ByteArrayToString(plain);
          }
        }

        if ((encryptedTypes & PasswordTypes.User) != 0) {
          // we are going to decrypt with the Windows User account key
          var cipher = StringToByteArray(data);
          var plain = ProtectedData.Unprotect(cipher, null, DataProtectionScope.CurrentUser);
          if (decode) {
            data = Encoding.UTF8.GetString(plain, 0, plain.Length);
          }
          else {
            data = ByteArrayToString(plain);
          }
        }

        if ((encryptedTypes & PasswordTypes.Explicit) != 0) {
          // we use an explicit password to encrypt data
          if (string.IsNullOrEmpty(password)) {
            throw new EncryptedSecretDataException();
          }

          data = Decrypt(data, password, true);
          if (decode) {
            var plain = StringToByteArray(data);
            data = Encoding.UTF8.GetString(plain, 0, plain.Length);
          }
        }
      }
      catch (EncryptedSecretDataException) {
        throw;
      }
      catch (BadYubiKeyException) {
        throw;
      }
      catch (Exception ex) {
        throw new BadPasswordException(ex.Message, ex);
      }

      return data;
    }

    public static HashAlgorithm SafeHasher(string name) {
      try {
        if (name == "SHA512") {
          return SHA512.Create();
        }

        if (name == "SHA256") {
          return SHA256.Create();
        }

        if (name == "MD5") {
          return MD5.Create();
        }

        return SHA1.Create();
      }
      catch (Exception) {
        // FIPS only allows SHA1 before Windows 10
        return SHA1.Create();
      }
    }

    public static string EncryptSequence(string data, PasswordTypes passwordType, string password) {
      // get hash of original
      var random = new RNGCryptoServiceProvider();
      var saltbytes = new byte[SALT_LENGTH];
      random.GetBytes(saltbytes);
      var salt = ByteArrayToString(saltbytes);

      string hash;
      //using (var sha = new SHA256Managed())
      using (var sha = SafeHasher("SHA256")) {
        var plain = StringToByteArray(salt + data);
        hash = ByteArrayToString(sha.ComputeHash(plain));
      }

      if ((passwordType & PasswordTypes.Explicit) != 0) {
        var encrypted = Encrypt(data, password);

        // test the encryption
        var decrypted = Decrypt(encrypted, password, true);
        if (string.Compare(data, decrypted) != 0) {
          throw new InvalidEncryptionException(data, password, encrypted, decrypted);
        }

        data = encrypted;
      }

      if ((passwordType & PasswordTypes.User) != 0) {
        // we encrypt the data using the Windows User account key
        var plain = StringToByteArray(data);
        var cipher = ProtectedData.Protect(plain, null, DataProtectionScope.CurrentUser);
        data = ByteArrayToString(cipher);
      }

      if ((passwordType & PasswordTypes.Machine) != 0) {
        // we encrypt the data using the Local Machine account key
        var plain = StringToByteArray(data);
        var cipher = ProtectedData.Protect(plain, null, DataProtectionScope.LocalMachine);
        data = ByteArrayToString(cipher);
      }

      // prepend the salt + hash
      return encryptionHeader + salt + hash + data;
    }

    public static string Encrypt(string plain, string password) {
      var passwordBytes = Encoding.UTF8.GetBytes(password);

      // build a new salt
      var rg = new RNGCryptoServiceProvider();
      var saltbytes = new byte[SALT_LENGTH];
      rg.GetBytes(saltbytes);
      var salt = ByteArrayToString(saltbytes);

      // build our PBKDF2 key
      var kg = new Rfc2898DeriveBytes(passwordBytes, saltbytes, PBKDF2_ITERATIONS);
      var key = kg.GetBytes(PBKDF2_KEYSIZE);

      return salt + Encrypt(plain, key);
    }

    public static string Encrypt(string plain, byte[] key) {
      var inBytes = StringToByteArray(plain);

      // get our cipher
      BufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new BlowfishEngine(), new ISO10126d2Padding());
      cipher.Init(true, new KeyParameter(key));

      // encrypt data
      var osize = cipher.GetOutputSize(inBytes.Length);
      var outBytes = new byte[osize];
      var olen = cipher.ProcessBytes(inBytes, 0, inBytes.Length, outBytes, 0);
      olen += cipher.DoFinal(outBytes, olen);
      if (olen < osize) {
        var t = new byte[olen];
        Array.Copy(outBytes, 0, t, 0, olen);
        outBytes = t;
      }

      // return encoded byte->hex string
      return ByteArrayToString(outBytes);
    }

    public static string Decrypt(string data, string password, bool pbkdf2) {
      byte[] key;
      var saltBytes = StringToByteArray(data.Substring(0, SALT_LENGTH * 2));

      if (pbkdf2) {
        // extract the salt from the data
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        // build our PBKDF2 key
        var kg = new Rfc2898DeriveBytes(passwordBytes, saltBytes, PBKDF2_ITERATIONS);
        key = kg.GetBytes(PBKDF2_KEYSIZE);
      }
      else {
        // extract the salt from the data
        var passwordBytes = Encoding.Default.GetBytes(password);
        key = new byte[saltBytes.Length + passwordBytes.Length];
        Array.Copy(saltBytes, key, saltBytes.Length);
        Array.Copy(passwordBytes, 0, key, saltBytes.Length, passwordBytes.Length);
        // build out combined key
        var md5 = MD5.Create();
        key = md5.ComputeHash(key);
      }

      // extract the actual data to be decrypted
      var inBytes = StringToByteArray(data.Substring(SALT_LENGTH * 2));

      // get cipher
      BufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new BlowfishEngine(), new ISO10126d2Padding());
      cipher.Init(false, new KeyParameter(key));

      // decrypt the data
      var osize = cipher.GetOutputSize(inBytes.Length);
      var outBytes = new byte[osize];
      try {
        var olen = cipher.ProcessBytes(inBytes, 0, inBytes.Length, outBytes, 0);
        olen += cipher.DoFinal(outBytes, olen);
        if (olen < osize) {
          var t = new byte[olen];
          Array.Copy(outBytes, 0, t, 0, olen);
          outBytes = t;
        }
      }
      catch (Exception) {
        // an exception is due to bad password
        throw new BadPasswordException();
      }

      // return encoded string
      return ByteArrayToString(outBytes);
    }

    public static string Decrypt(string data, byte[] key) {
      // the actual data to be decrypted
      var inBytes = StringToByteArray(data);

      // get cipher
      BufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new BlowfishEngine(), new ISO10126d2Padding());
      cipher.Init(false, new KeyParameter(key));

      // decrypt the data
      var osize = cipher.GetOutputSize(inBytes.Length);
      var outBytes = new byte[osize];
      try {
        var olen = cipher.ProcessBytes(inBytes, 0, inBytes.Length, outBytes, 0);
        olen += cipher.DoFinal(outBytes, olen);
        if (olen < osize) {
          var t = new byte[olen];
          Array.Copy(outBytes, 0, t, 0, olen);
          outBytes = t;
        }
      }
      catch (Exception) {
        // an exception is due to bad password
        throw new BadPasswordException();
      }

      // return encoded string
      return ByteArrayToString(outBytes);
    }

    #endregion

    #region ICloneable

    public object Clone() {
      // we only need to do shallow copy
      return MemberwiseClone();
    }

    #endregion
  }
}