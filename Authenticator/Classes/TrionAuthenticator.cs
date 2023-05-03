using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace Authenticator {

  public sealed class TrionAuthenticator : Authenticator {
  
    private const int MODEL_SIZE = 15;
    private const string MODEL_CHARS = "1234567890ABCDEF";
    private const int CODE_DIGITS = 8;
    private const string TRION_ISSUER = "Trion";

    private static string enrollUrl = "https://rift.trionworlds.com/external/create-device-key";
    private static string syncUrl = "https://auth.trionworlds.com/time";
    private static string securityquestionsUrl = "https://rift.trionworlds.com/external/get-account-security-questions.action";
    private static string restoreUrl = "https://rift.trionworlds.com/external/retrieve-device-key.action";

    private const int SYNC_ERROR_MINUTES = 60;
    private static DateTime lastSyncError = DateTime.MinValue;

    #region Authenticator data

    public override string SecretData {
      get {
        // this is the key |  serial | deviceid
        return base.SecretData + "|" + ByteArrayToString(Encoding.UTF8.GetBytes(Serial)) + "|" +
               ByteArrayToString(Encoding.UTF8.GetBytes(DeviceId));
      }
      set {
        // extract key + serial + deviceid
        if (string.IsNullOrEmpty(value) == false) {
          var parts = value.Split('|');
          if (parts.Length == 3 && parts[1].IndexOf("-") != -1) {
            // alpha 3.0.2 version
            SecretKey = StringToByteArray(parts[0]);
            Serial = parts[1];
            DeviceId = parts[2];
          }
          else if (parts.Length == 4) {
            // alpha 3.0.6 version
            SecretKey = StringToByteArray(parts[0]);
            Serial = (parts.Length > 2 ? Encoding.UTF8.GetString(StringToByteArray(parts[2])) : null);
            DeviceId = (parts.Length > 3 ? Encoding.UTF8.GetString(StringToByteArray(parts[3])) : null);
          }
          else {
            base.SecretData = value;
            Serial = (parts.Length > 1 ? Encoding.UTF8.GetString(StringToByteArray(parts[1])) : null);
            DeviceId = (parts.Length > 2 ? Encoding.UTF8.GetString(StringToByteArray(parts[2])) : null);
          }
        }
        else {
          SecretKey = null;
          Serial = null;
          DeviceId = null;
        }
      }
    }

    public string Serial { get; set; }

    public string DeviceId { get; set; }

    #endregion

    public TrionAuthenticator() : base(CODE_DIGITS) {
      Issuer = TRION_ISSUER;
    }

    public void Enroll() {
      // generate model name
      var deviceId = GeneralRandomModel();

      var postdata = "deviceId=" + deviceId;

      // call the enroll server
      var request = (HttpWebRequest) WebRequest.Create(enrollUrl);
      request.Method = "POST";
      request.ContentType = "application/x-www-form-urlencoded";
      request.ContentLength = postdata.Length;
      var requestStream = new StreamWriter(request.GetRequestStream());
      requestStream.Write(postdata);
      requestStream.Close();
      string responseData;
      using (var response = (HttpWebResponse) request.GetResponse()) {
        // OK?
        if (response.StatusCode != HttpStatusCode.OK) {
          throw new InvalidEnrollResponseException(string.Format("{0}: {1}", (int) response.StatusCode,
            response.StatusDescription));
        }

        // load the response
        using (var responseStream = new StreamReader(response.GetResponseStream())) {
          responseData = responseStream.ReadToEnd();
        }
      }

      // return data:
      // <DeviceKey>
      //	<DeviceId />
      //	<SerialKey />
      //	<SecretKey />
      //	<ErrorCode /> only exists if an error
      // </DeviceKey>
      var doc = new XmlDocument();
      doc.LoadXml(responseData);
      var node = doc.SelectSingleNode("//ErrorCode");
      if (node != null && string.IsNullOrEmpty(node.InnerText) == false) {
        // an error occured
        throw new InvalidEnrollResponseException(node.InnerText);
      }

      // get the secret key
      SecretKey = Encoding.UTF8.GetBytes(doc.SelectSingleNode("//SecretKey").InnerText);

      // get the serial number
      var serial = doc.SelectSingleNode("//SerialKey").InnerText;
      Serial = Regex.Replace(serial, @"(.{4})", "$1 ").Trim().Replace(" ", "-");

      // save the device
      DeviceId = doc.SelectSingleNode("//DeviceId").InnerText;
    }

#if DEBUG
    public void Enroll(bool testmode) {
      if (!testmode) {
        Enroll();
      }
      else {
        //string responseData = "<DeviceKey><DeviceId>zarTM0v5ko0BwrOYQV1HhsE4Q0stqgbF</DeviceId><SerialKey>FJP7H9DG3T67</SerialKey><SecretKey>DP7FFJZKLG6ZNCJTNNMT</SecretKey></DeviceKey>";
        var responseData =
          "<DeviceKey><DeviceId>19897B57952648559364352F7FE9B8A8</DeviceId><SerialKey>HM3ZMQ233FPZ</SerialKey><SecretKey>6MYNGRGYX7XZQNL6T2M6</SecretKey></DeviceKey>";

        //	<DeviceId />
        //	<SerialKey />
        //	<SecretKey />
        //	<ErrorCode /> only exists if an error
        // </DeviceKey>
        var doc = new XmlDocument();
        doc.LoadXml(responseData);
        var node = doc.SelectSingleNode("//ErrorCode");
        if (node != null && string.IsNullOrEmpty(node.InnerText) == false) {
          // an error occured
          throw new InvalidEnrollResponseException(node.InnerText);
        }

        // get the secret key
        SecretKey = Encoding.UTF8.GetBytes(doc.SelectSingleNode("//SecretKey").InnerText);

        // get the serial number
        var serial = doc.SelectSingleNode("//SerialKey").InnerText;
        Serial = Regex.Replace(serial, @"(.{4})", "$1 ").Trim().Replace(" ", "-");

        DeviceId = doc.SelectSingleNode("//DeviceId").InnerText;
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
        var request = (HttpWebRequest) WebRequest.Create(syncUrl);
        request.Method = "GET";

        // get response
        string responseData;
        using (var response = (HttpWebResponse) request.GetResponse()) {
          // OK?
          if (response.StatusCode != HttpStatusCode.OK) {
            throw new ApplicationException(string.Format("{0}: {1}", (int) response.StatusCode,
              response.StatusDescription));
          }

          // load the response
          using (var responseStream = new StreamReader(response.GetResponseStream())) {
            responseData = responseStream.ReadToEnd();
          }
        }

        // return data is string version of time in milliseconds since epoch

        // get the difference between the server time and our current time
        var serverTimeDiff = long.Parse(responseData) - CurrentTime;

        // update the Data object
        ServerTimeDiff = serverTimeDiff;
        LastServerTime = DateTime.Now.Ticks;

        // clear any sync error
        lastSyncError = DateTime.MinValue;
      }
      catch (Exception) {
        // don't retry for a while after error
        lastSyncError = DateTime.Now;
      }
    }

    protected override string CalculateCode(bool resyncTime = false, long interval = -1) {
      // sync time if required
      if (resyncTime || ServerTimeDiff == 0) {
        if (interval > 0) {
          ServerTimeDiff = interval * Period * 1000L - CurrentTime;
        }
        else {
          Sync();
        }
      }

      var hmac = new HMac(new Sha1Digest());
      hmac.Init(new KeyParameter(SecretKey));

      var codeIntervalArray = BitConverter.GetBytes(CodeInterval);
      if (BitConverter.IsLittleEndian) {
        Array.Reverse(codeIntervalArray);
      }

      hmac.BlockUpdate(codeIntervalArray, 0, codeIntervalArray.Length);

      var mac = new byte[hmac.GetMacSize()];
      hmac.DoFinal(mac, 0);

      // the last 4 bits of the mac say where the code starts (e.g. if last 4 bit are 1100, we start at byte 12)
      var start = mac[19] & 0x0f;

      // extract those 4 bytes
      var bytes = new byte[4];
      Array.Copy(mac, start, bytes, 0, 4);
      if (BitConverter.IsLittleEndian) {
        Array.Reverse(bytes);
      }

      // this is where Trion is broken and their version uses all 32bits
      //uint fullcode = BitConverter.ToUInt32(bytes, 0) & 0x7fffffff;
      var fullcode = BitConverter.ToUInt32(bytes, 0);

      // we use the last 8 digits of this code in radix 10
      var codemask = (uint) Math.Pow(10, CodeDigits);
      var format = new string('0', CodeDigits);
      var code = (fullcode % codemask).ToString(format);
      // New glyph authenticator now uses 6, but takes the first 6 of 8 rather the proper last 6, so again we override the standard implementation
      code = code.Substring(0, 6);

      return code;
    }

    public static void SecurityQuestions(string email, string password, out string question1, out string question2) {
      var postdata = "emailAddress=" + email + "&password=" + password;

      // call the enroll server
      var request = (HttpWebRequest) WebRequest.Create(securityquestionsUrl);
      request.Method = "POST";
      request.ContentType = "application/x-www-form-urlencoded";
      request.ContentLength = postdata.Length;
      var requestStream = new StreamWriter(request.GetRequestStream());
      requestStream.Write(postdata);
      requestStream.Close();
      string responseData;
      using (var response = (HttpWebResponse) request.GetResponse()) {
        // OK?
        if (response.StatusCode != HttpStatusCode.OK) {
          throw new InvalidRestoreResponseException($"{(int) response.StatusCode}: {response.StatusDescription}");
        }

        // load the response
        using (var responseStream = new StreamReader(response.GetResponseStream())) {
          responseData = responseStream.ReadToEnd();
        }
      }

      // return data:
      // <SecurityQuestions>
      //	<EmailAddress />
      //	<FirstQuestion />
      //	<SecondQuestion />
      //	<ErrorCode /> only exists if an error
      // </SecurityQuestions>
      var doc = new XmlDocument();
      doc.LoadXml(responseData);
      var node = doc.SelectSingleNode("//ErrorCode");
      if (node != null && string.IsNullOrEmpty(node.InnerText) == false) {
        // an error occured
        throw new InvalidRestoreResponseException(node.InnerText);
      }

      // get the questions
      question1 = doc.SelectSingleNode("//FirstQuestion").InnerText;
      question2 = doc.SelectSingleNode("//SecondQuestion").InnerText;
    }

    public void Restore(string email, string password, string deviceId, string answer1, string answer2) {
      var postdata = "emailAddress=" + email
                                     + "&password=" + password
                                     + "&deviceId=" + deviceId
                                     + "&securityAnswer=" + answer1
                                     + "&secondSecurityAnswer=" + answer2;

      // call the enroll server
      var request = (HttpWebRequest) WebRequest.Create(restoreUrl);
      request.Method = "POST";
      request.ContentType = "application/x-www-form-urlencoded";
      request.ContentLength = postdata.Length;
      var requestStream = new StreamWriter(request.GetRequestStream());
      requestStream.Write(postdata);
      requestStream.Close();
      string responseData;
      using (var response = (HttpWebResponse) request.GetResponse()) {
        // OK?
        if (response.StatusCode != HttpStatusCode.OK) {
          throw new InvalidRestoreResponseException($"{(int) response.StatusCode}: {response.StatusDescription}");
        }

        // load the response
        using (var responseStream = new StreamReader(response.GetResponseStream())) {
          responseData = responseStream.ReadToEnd();
        }
      }

      // return data:
      // <DeviceKey>
      //	<DeviceId />
      //	<SerialKey />
      //	<SecretKey />
      //	<ErrorCode /> only exists if an error
      // </DeviceKey>
      var doc = new XmlDocument();
      doc.LoadXml(responseData);
      var node = doc.SelectSingleNode("//ErrorCode");
      if (node != null && string.IsNullOrEmpty(node.InnerText) == false) {
        // an error occured
        throw new InvalidRestoreResponseException(node.InnerText);
      }

      // get the secret key
      SecretKey = Encoding.UTF8.GetBytes(doc.SelectSingleNode("//SecretKey").InnerText);

      // get the serial number
      var serial = doc.SelectSingleNode("//SerialKey").InnerText;
      Serial = Regex.Replace(serial, @"(.{4})", "$1 ").Trim().Replace(" ", "-");

      // save the device
      DeviceId = doc.SelectSingleNode("//DeviceId").InnerText;
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
  }
}