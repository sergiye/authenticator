using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace Authenticator {
  public class BattleNetAuthenticator : Authenticator {
    private const int MODEL_SIZE = 16;
    private const string MODEL_CHARS = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567890";
    private const int RESPONSE_BUFFER_SIZE = 64;
    private const int ENROLL_RESPONSE_SIZE = 45;
    private const int SYNC_RESPONSE_SIZE = 8;
    private const int RESTOREINIT_BUFFER_SIZE = 32;
    private const int RESTOREVALIDATE_BUFFER_SIZE = 20;
    private const int CODE_DIGITS = 8;

    private const string ENROLL_MODULUS =
      "955e4bd989f3917d2f15544a7e0504eb9d7bb66b6f8a2fe470e453c779200e5e" +
      "3ad2e43a02d06c4adbd8d328f1a426b83658e88bfd949b2af4eaf30054673a14" +
      "19a250fa4cc1278d12855b5b25818d162c6e6ee2ab4a350d401d78f6ddb99711" +
      "e72626b48bd8b5b0b7f3acf9ea3c9e0005fee59e19136cdb7c83f2ab8b0a2a99";

    private const string ENROLL_EXPONENT = "0101";
    private const int SYNC_ERROR_MINUTES = 5;
    private const string BATTLENET_ISSUER = "BattleNet";

    private static string regionUs = "US";
    private static string regionEu = "EU";
    private static string regionKr = "KR";
    private static string regionCn = "CN";

    public static Dictionary<string, string> MobileUrls = new Dictionary<string, string> {
      {regionUs, "http://mobile-service.blizzard.com"},
      {regionEu, "http://mobile-service.blizzard.com"},
      {regionKr, "http://mobile-service.blizzard.com"},
      {regionCn, "http://mobile-service.battlenet.com.cn"}
    };

    private static string enrollPath = "/enrollment/enroll2.htm";
    private static string syncPath = "/enrollment/time.htm";
    private static string restorePath = "/enrollment/initiatePaperRestore.htm";
    private static string restorevalidatePath = "/enrollment/validatePaperRestore.htm";

    private static List<string> euCountries = new List<string> {
      "AL", "AD", "AM", "AT", "AZ", "BY", "BE", "BA", "BG", "HR",
      "CY", "CZ", "DK", "EE", "FI", "FR", "GE", "DE", "GR", "HU",
      "IS", "IE", "IT", "KV", "XK", "LV", "LI", "LT", "LU", "MK",
      "MT", "MD", "MC", "ME", "NL", "NO", "PL", "PT", "RO", "RU",
      "SM", "RS", "SK", "ES", "SE", "CH", "TR", "UA", "UK", "GB",
      "VA"
    };

    private static List<string> krCountries = new List<string> {
      "KR", "KP", "TW", "HK", "MO"
    };

    private static string geoipurl = "http://geoiplookup.wikimedia.org";

    private static DateTime lastSyncError = DateTime.MinValue;

    #region Authenticator data

    public string Region => (string.IsNullOrEmpty(Serial) == false ? Serial.Substring(0, 2) : string.Empty);

    public string Serial { get; set; }

    public override string SecretData {
      get =>
        // for Battle.net, this is the key + serial
        base.SecretData + "|" + ByteArrayToString(Encoding.UTF8.GetBytes(Serial));
      set {
        // for Battle.net, extract key + serial
        if (string.IsNullOrEmpty(value) == false) {
          var parts = value.Split('|');
          if (parts.Length <= 1) {
            // old Auth2 version with secretdata + serial
            SecretKey = StringToByteArray(value.Substring(0, 40));
            Serial = Encoding.UTF8.GetString(StringToByteArray(value.Substring(40)));
          }
          else if (parts.Length == 3) // alpha 3.0.6
          {
            // secret|script|serial
            base.SecretData = value;
            Serial = (parts.Length > 2 ? Encoding.UTF8.GetString(StringToByteArray(parts[2])) : null);
          }
          else {
            // secret|serial
            base.SecretData = value;
            Serial = (parts.Length > 1 ? Encoding.UTF8.GetString(StringToByteArray(parts[1])) : null);
          }
        }
        else {
          SecretKey = null;
          Serial = null;
        }
      }
    }

    public bool RestoreCodeVerified { get; set; }

    public string RestoreCode => BuildRestoreCode();

    #endregion

    public BattleNetAuthenticator()
      : base(CODE_DIGITS) {
      Issuer = BATTLENET_ISSUER;
    }

    public void Enroll() {
      // default to US
      var region = regionUs;
      var country = regionUs;

      // Battle.net does a GEO IP lookup anyway so there is no need to pass the region
      // however China has its own URL so we must still do our own GEO IP lookup to find the country
      var georequest = (HttpWebRequest) WebRequest.Create(geoipurl);
      georequest.Method = "GET";
      georequest.ContentType = "application/json";
      georequest.Timeout = 10000;
      // get response
      string responseString = null;
      try {
        using (var georesponse = (HttpWebResponse) georequest.GetResponse()) {
          // OK?
          if (georesponse.StatusCode == HttpStatusCode.OK) {
            using (var ms = new MemoryStream()) {
              using (var bs = georesponse.GetResponseStream()) {
                var temp = new byte[RESPONSE_BUFFER_SIZE];
                int read;
                while ((read = bs.Read(temp, 0, RESPONSE_BUFFER_SIZE)) != 0) {
                  ms.Write(temp, 0, read);
                }

                responseString = Encoding.UTF8.GetString(ms.ToArray());
              }
            }
          }
        }
      }
      catch (Exception) {
      }

      if (string.IsNullOrEmpty(responseString) == false) {
        // not worth a full json parser, just regex it
        var match = Regex.Match(responseString, ".*\"country\":\"([^\"]*)\".*", RegexOptions.IgnoreCase);
        if (match.Success) {
          // match the correct region
          country = match.Groups[1].Value.ToUpper();

          if (euCountries.Contains(country)) {
            region = regionEu;
          }
          else if (krCountries.Contains(country)) {
            region = regionKr;
          }
          else if (country == regionCn) {
            region = regionCn;
          }
          else {
            region = regionUs;
          }
        }
      }

      // allow override of country for CN using US from app.config
      var config = new System.Configuration.AppSettingsReader();
      try {
        var configcountry = config.GetValue("BattleNetAuthenticator.Country", typeof(string)) as string;
        if (string.IsNullOrEmpty(configcountry) == false) {
          country = configcountry;
        }
      }
      catch (InvalidOperationException) {
      }

      try {
        var configregion = config.GetValue("BattleNetAuthenticator.Region", typeof(string)) as string;
        if (string.IsNullOrEmpty(configregion) == false) {
          region = configregion;
        }
      }
      catch (InvalidOperationException) {
      }

      // generate byte array of data:
      //  00 byte[20] one-time key used to decrypt data when returned;
      //  20 byte[2] country code, e.g. US, GB, FR, KR, etc
      //  22 byte[16] model string for this device;
      //	38 END
      var data = new byte[38];
      var oneTimePad = CreateOneTimePad(20);
      Array.Copy(oneTimePad, data, oneTimePad.Length);
      // add country
      var countrydata = Encoding.UTF8.GetBytes(country);
      Array.Copy(countrydata, 0, data, 20, Math.Min(countrydata.Length, 2));
      // add model name
      var model = Encoding.UTF8.GetBytes(GeneralRandomModel());
      Array.Copy(model, 0, data, 22, Math.Min(model.Length, 16));

      // encrypt the data with BMA public key
      var rsa = new RsaEngine();
      rsa.Init(true,
        new RsaKeyParameters(false, new Org.BouncyCastle.Math.BigInteger(ENROLL_MODULUS, 16),
          new Org.BouncyCastle.Math.BigInteger(ENROLL_EXPONENT, 16)));
      var encrypted = rsa.ProcessBlock(data, 0, data.Length);

      // call the enroll server
      byte[] responseData;
      try {
        var request = (HttpWebRequest) WebRequest.Create(GetMobileUrl(region) + enrollPath);
        request.Method = "POST";
        request.ContentType = "application/octet-stream";
        request.ContentLength = encrypted.Length;
        request.Timeout = 10000;
        var requestStream = request.GetRequestStream();
        requestStream.Write(encrypted, 0, encrypted.Length);
        requestStream.Close();
        using (var response = (HttpWebResponse) request.GetResponse()) {
          // OK?
          if (response.StatusCode != HttpStatusCode.OK) {
            throw new InvalidEnrollResponseException($"{(int) response.StatusCode}: {response.StatusDescription}");
          }

          // load back the buffer - should only be a byte[45]
          using (var ms = new MemoryStream()) {
            //using (BufferedStream bs = new BufferedStream(response.GetResponseStream()))
            using (var bs = response.GetResponseStream()) {
              var temp = new byte[RESPONSE_BUFFER_SIZE];
              int read;
              while ((read = bs.Read(temp, 0, RESPONSE_BUFFER_SIZE)) != 0) {
                ms.Write(temp, 0, read);
              }

              responseData = ms.ToArray();

              // check it is correct size
              if (responseData.Length != ENROLL_RESPONSE_SIZE) {
                throw new InvalidEnrollResponseException($"Invalid response data size (expected 45 got {responseData.Length})");
              }
            }
          }
        }
      }
      catch (Exception ex) {
        throw new InvalidEnrollResponseException("Cannot contact Battle.net servers: " + ex.Message);
      }
      // return data:
      // 00-07 server time (Big Endian)
      // 08-24 serial number (17)
      // 25-44 secret key encrpyted with our pad
      // 45 END

      // extract the server time
      var serverTime = new byte[8];
      Array.Copy(responseData, serverTime, 8);
      if (BitConverter.IsLittleEndian) {
        Array.Reverse(serverTime);
      }

      // get the difference between the server time and our current time
      ServerTimeDiff = BitConverter.ToInt64(serverTime, 0) - CurrentTime;

      // get the secret key
      var secretKey = new byte[20];
      Array.Copy(responseData, 25, secretKey, 0, 20);
      // decrypt the initdata with a simple xor with our key
      for (var i = oneTimePad.Length - 1; i >= 0; i--) {
        secretKey[i] ^= oneTimePad[i];
      }

      SecretKey = secretKey;

      // get the serial number
      Serial = Encoding.Default.GetString(responseData, 8, 17);
    }

#if DEBUG
    public void Enroll(bool testmode) {
      if (!testmode) {
        Enroll();
      }
      else {
        ServerTimeDiff = 0;
        SecretKey = new byte[20] {
          0x7B, 0x0B, 0xFA, 0x82, 0x30, 0xE5, 0x44, 0x24, 0xAB, 0x51, 0x77, 0x7D, 0xAD, 0xBF, 0xD5, 0x37, 0x41, 0x43,
          0xE3, 0xB0
        };
        Serial = "US-1306-2525-4376"; // Restore: CR24KPKF51
      }
    }
#endif


    public override void Sync() {
      // check if data is protected
      if (SecretKey == null && EncryptedData != null) {
        throw new EncryptedSecretDataException();
      }

      // don't retry for 5 minutes
      if (lastSyncError >= DateTime.Now.AddMinutes(0 - SYNC_ERROR_MINUTES)) {
        return;
      }

      try {
        // create a connection to time sync server
        var request = (HttpWebRequest) WebRequest.Create(GetMobileUrl(Region) + syncPath);
        request.Method = "GET";
        request.Timeout = 5000;

        // get response
        byte[] responseData = null;
        using (var response = (HttpWebResponse) request.GetResponse()) {
          // OK?
          if (response.StatusCode != HttpStatusCode.OK) {
            throw new ApplicationException(string.Format("{0}: {1}", (int) response.StatusCode,
              response.StatusDescription));
          }

          // load back the buffer - should only be a byte[8]
          using (var ms = new MemoryStream()) {
            // using (BufferedStream bs = new BufferedStream(response.GetResponseStream()))
            using (var bs = response.GetResponseStream()) {
              var temp = new byte[RESPONSE_BUFFER_SIZE];
              int read;
              while ((read = bs.Read(temp, 0, RESPONSE_BUFFER_SIZE)) != 0) {
                ms.Write(temp, 0, read);
              }

              responseData = ms.ToArray();

              // check it is correct size
              if (responseData.Length != SYNC_RESPONSE_SIZE) {
                throw new InvalidSyncResponseException(string.Format(
                  "Invalid response data size (expected " + SYNC_RESPONSE_SIZE + " got {0}", responseData.Length));
              }
            }
          }
        }

        // return data:
        // 00-07 server time (Big Endian)

        // extract the server time
        if (BitConverter.IsLittleEndian) {
          Array.Reverse(responseData);
        }

        // get the difference between the server time and our current time
        var serverTimeDiff = BitConverter.ToInt64(responseData, 0) - CurrentTime;

        // update the Data object
        ServerTimeDiff = serverTimeDiff;
        LastServerTime = DateTime.Now.Ticks;

        // clear any sync error
        lastSyncError = DateTime.MinValue;
      }
      catch (WebException) {
        // don't retry for a while after error
        lastSyncError = DateTime.Now;

        // set to zero to force reset
        ServerTimeDiff = 0;
      }
    }

    public void Restore(string serial, string restoreCode) {
      // get the serial data
      var serialBytes = Encoding.UTF8.GetBytes(serial.ToUpper().Replace("-", string.Empty));

      // send the request to the server to get our challenge
      var request = (HttpWebRequest) WebRequest.Create(GetMobileUrl(serial) + restorePath);
      request.Method = "POST";
      request.ContentType = "application/octet-stream";
      request.ContentLength = serialBytes.Length;
      var requestStream = request.GetRequestStream();
      requestStream.Write(serialBytes, 0, serialBytes.Length);
      requestStream.Close();
      byte[] challenge = null;
      try {
        using (var response = (HttpWebResponse) request.GetResponse()) {
          // OK?
          if (response.StatusCode != HttpStatusCode.OK) {
            throw new InvalidRestoreResponseException(string.Format("{0}: {1}", (int) response.StatusCode,
              response.StatusDescription));
          }

          // load back the buffer - should only be a byte[32]
          using (var ms = new MemoryStream()) {
            using (var bs = response.GetResponseStream()) {
              var temp = new byte[RESPONSE_BUFFER_SIZE];
              int read;
              while ((read = bs.Read(temp, 0, RESPONSE_BUFFER_SIZE)) != 0) {
                ms.Write(temp, 0, read);
              }

              challenge = ms.ToArray();

              // check it is correct size
              if (challenge.Length != RESTOREINIT_BUFFER_SIZE) {
                throw new InvalidRestoreResponseException(
                  string.Format("Invalid response data size (expected 32 got {0})", challenge.Length));
              }
            }
          }
        }
      }
      catch (WebException we) {
        var code = (int) ((HttpWebResponse) we.Response).StatusCode;
        if (code >= 500 && code < 600) {
          throw new InvalidRestoreResponseException(
            string.Format("No response from server ({0}). Perhaps maintainence?", code));
        }
        else {
          throw new InvalidRestoreResponseException(string.Format("Error communicating with server: {0} - {1}", code,
            ((HttpWebResponse) we.Response).StatusDescription));
        }
      }

      // only take the first 10 bytes of the restore code and encode to byte taking count of the missing chars
      var restoreCodeBytes = new byte[10];
      var arrayOfChar = restoreCode.ToUpper().ToCharArray();
      for (var i = 0; i < 10; i++) {
        restoreCodeBytes[i] = ConvertRestoreCodeCharToByte(arrayOfChar[i]);
      }

      // build the response to the challenge
      var hmac = new HMac(new Sha1Digest());
      hmac.Init(new KeyParameter(restoreCodeBytes));
      var hashdata = new byte[serialBytes.Length + challenge.Length];
      Array.Copy(serialBytes, 0, hashdata, 0, serialBytes.Length);
      Array.Copy(challenge, 0, hashdata, serialBytes.Length, challenge.Length);
      hmac.BlockUpdate(hashdata, 0, hashdata.Length);
      var hash = new byte[hmac.GetMacSize()];
      hmac.DoFinal(hash, 0);

      // create a random key
      var oneTimePad = CreateOneTimePad(20);

      // concatanate the hash and key
      var hashkey = new byte[hash.Length + oneTimePad.Length];
      Array.Copy(hash, 0, hashkey, 0, hash.Length);
      Array.Copy(oneTimePad, 0, hashkey, hash.Length, oneTimePad.Length);

      // encrypt the data with BMA public key
      var rsa = new RsaEngine();
      rsa.Init(true,
        new RsaKeyParameters(false, new Org.BouncyCastle.Math.BigInteger(ENROLL_MODULUS, 16),
          new Org.BouncyCastle.Math.BigInteger(ENROLL_EXPONENT, 16)));
      var encrypted = rsa.ProcessBlock(hashkey, 0, hashkey.Length);

      // prepend the serial to the encrypted data
      var postbytes = new byte[serialBytes.Length + encrypted.Length];
      Array.Copy(serialBytes, 0, postbytes, 0, serialBytes.Length);
      Array.Copy(encrypted, 0, postbytes, serialBytes.Length, encrypted.Length);

      // send the challenge response back to the server
      request = (HttpWebRequest) WebRequest.Create(GetMobileUrl(serial) + restorevalidatePath);
      request.Method = "POST";
      request.ContentType = "application/octet-stream";
      request.ContentLength = postbytes.Length;
      requestStream = request.GetRequestStream();
      requestStream.Write(postbytes, 0, postbytes.Length);
      requestStream.Close();
      byte[] secretKey = null;
      try {
        using (var response = (HttpWebResponse) request.GetResponse()) {
          // OK?
          if (response.StatusCode != HttpStatusCode.OK) {
            throw new InvalidRestoreResponseException(string.Format("{0}: {1}", (int) response.StatusCode,
              response.StatusDescription));
          }

          // load back the buffer - should only be a byte[32]
          using (var ms = new MemoryStream()) {
            using (var bs = response.GetResponseStream()) {
              var temp = new byte[RESPONSE_BUFFER_SIZE];
              int read;
              while ((read = bs.Read(temp, 0, RESPONSE_BUFFER_SIZE)) != 0) {
                ms.Write(temp, 0, read);
              }

              secretKey = ms.ToArray();

              // check it is correct size
              if (secretKey.Length != RESTOREVALIDATE_BUFFER_SIZE) {
                throw new InvalidRestoreResponseException(string.Format(
                  "Invalid response data size (expected " + RESTOREVALIDATE_BUFFER_SIZE + " got {0})",
                  secretKey.Length));
              }
            }
          }
        }
      }
      catch (WebException we) {
        var code = (int) ((HttpWebResponse) we.Response).StatusCode;
        if (code >= 500 && code < 600) {
          throw new InvalidRestoreResponseException(
            string.Format("No response from server ({0}). Perhaps maintainence?", code));
        }
        else if (code >= 600 && code < 700) {
          throw new InvalidRestoreCodeException("Invalid serial number or restore code.");
        }
        else {
          throw new InvalidRestoreResponseException(string.Format("Error communicating with server: {0} - {1}", code,
            ((HttpWebResponse) we.Response).StatusDescription));
        }
      }

      // xor the returned data key with our pad to get the actual secret key
      for (var i = oneTimePad.Length - 1; i >= 0; i--) {
        secretKey[i] ^= oneTimePad[i];
      }

      // set the authenticator data
      SecretKey = secretKey;
      if (serial.Length == 14) {
        Serial = serial.Substring(0, 2).ToUpper() + "-" + serial.Substring(2, 4) + "-" + serial.Substring(6, 4) + "-" +
                 serial.Substring(10, 4);
      }
      else {
        Serial = serial.ToUpper();
      }

      // restore code is ok
      RestoreCodeVerified = true;
      // sync the time
      ServerTimeDiff = 0L;
      Sync();
    }

    public override bool ReadExtraXml(XmlReader reader, string name) {
      switch (name) {
        case "restorecodeverified":
          RestoreCodeVerified = reader.ReadElementContentAsBoolean();
          return true;
        default:
          return false;
      }
    }

    protected override void WriteExtraXml(XmlWriter writer) {
      if (RestoreCodeVerified) {
        writer.WriteStartElement("restorecodeverified");
        writer.WriteString(bool.TrueString.ToLower());
        writer.WriteEndElement();
      }
    }

    private static string GetMobileUrl(string region) {
      var upperregion = region.ToUpper();
      if (upperregion.Length > 2) {
        upperregion = upperregion.Substring(0, 2);
      }

      if (MobileUrls.ContainsKey(upperregion)) {
        return MobileUrls[upperregion];
      }
      else {
        return MobileUrls[regionUs];
      }
    }

    protected string BuildRestoreCode() {
      // return if not set
      if (string.IsNullOrEmpty(Serial) || SecretKey == null) {
        return string.Empty;
      }

      // get byte array of serial
      var serialdata = Encoding.UTF8.GetBytes(Serial.ToUpper().Replace("-", string.Empty));
      var secretdata = SecretKey;

      // combine serial data and secret data
      var combined = new byte[serialdata.Length + secretdata.Length];
      Array.Copy(serialdata, 0, combined, 0, serialdata.Length);
      Array.Copy(secretdata, 0, combined, serialdata.Length, secretdata.Length);

      // create digest of combined data
      IDigest digest = new Sha1Digest();
      digest.BlockUpdate(combined, 0, combined.Length);
      var digestdata = new byte[digest.GetDigestSize()];
      digest.DoFinal(digestdata, 0);

      // take last 10 chars of hash and convert each byte to our encoded string that doesn't use I,L,O,S
      var code = new StringBuilder();
      var startpos = digestdata.Length - 10;
      for (var i = 0; i < 10; i++) {
        code.Append(ConvertRestoreCodeByteToChar(digestdata[startpos + i]));
      }

      return code.ToString();
    }

    private static string GeneralRandomModel() {
      // seed a new RNG
      var randomSeedGenerator = new RNGCryptoServiceProvider();
      var seedBuffer = new byte[4];
      randomSeedGenerator.GetBytes(seedBuffer);
      var random = new Random(BitConverter.ToInt32(seedBuffer, 0));

      // create a model string with available characters
      var model = new StringBuilder(MODEL_SIZE);
      for (var i = MODEL_SIZE; i > 0; i--) {
        model.Append(MODEL_CHARS[random.Next(MODEL_CHARS.Length)]);
      }

      return model.ToString();
    }

    #region Utility functions

    private static byte ConvertRestoreCodeCharToByte(char c) {
      if (c >= '0' && c <= '9')
        return (byte) (c - '0');

      var index = (byte) (c + 10 - 65);
      if (c >= 'I') {
        index--;
      }
      if (c >= 'L') {
        index--;
      }
      if (c >= 'O') {
        index--;
      }
      if (c >= 'S') {
        index--;
      }
      return index;
    }

    private static char ConvertRestoreCodeByteToChar(byte b) {
      var index = b & 0x1f;
      if (index <= 9)
        return (char) (index + 48);

      index = (index + 65) - 10;
      if (index >= 73) {
        index++;
      }
      if (index >= 76) {
        index++;
      }
      if (index >= 79) {
        index++;
      }
      if (index >= 83) {
        index++;
      }
      return (char) index;
    }

    #endregion
  }
}