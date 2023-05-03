using System;
using System.Net;

namespace Authenticator {

  public class GoogleAuthenticator : Authenticator {
  
    private const int CODE_DIGITS = 6;
    private const int SYNC_ERROR_MINUTES = 5;
    private const string TIME_SYNC_URL = "http://www.google.com";
    private static DateTime lastSyncError = DateTime.MinValue;

    #region Authenticator data

    public string Serial => Base32.GetInstance().Encode(SecretKey);

    #endregion

    public GoogleAuthenticator() : base(CODE_DIGITS) {
    }

    public void Enroll(string b32Key) {
      SecretKey = Base32.GetInstance().Decode(b32Key);
      Sync();
    }

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
        // we use the Header response field from a request to www.google.come
        var request = (HttpWebRequest) WebRequest.Create(TIME_SYNC_URL);
        request.Method = "GET";
        request.ContentType = "text/html";
        request.Timeout = 5000;
        // get response
        using (var response = (HttpWebResponse) request.GetResponse()) {
          // OK?
          if (response.StatusCode != HttpStatusCode.OK) {
            throw new ApplicationException(string.Format("{0}: {1}", (int) response.StatusCode,
              response.StatusDescription));
          }

          var headerdate = response.Headers["Date"];
          if (string.IsNullOrEmpty(headerdate) == false) {
            if (DateTime.TryParse(headerdate, out var dt)) {
              // get as ms since epoch
              var dtms = Convert.ToInt64((dt.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds);

              // get the difference between the server time and our current time
              var serverTimeDiff = dtms - CurrentTime;

              // update the Data object
              ServerTimeDiff = serverTimeDiff;
              LastServerTime = DateTime.Now.Ticks;
            }
          }

          // clear any sync error
          lastSyncError = DateTime.MinValue;
        }
      }
      catch (WebException) {
        // don't retry for a while after error
        lastSyncError = DateTime.Now;

        // set to zero to force reset
        ServerTimeDiff = 0;
      }
    }
  }
  
  public class MicrosoftAuthenticator : GoogleAuthenticator {
  }
  
  public class GuildWarsAuthenticator : GoogleAuthenticator {
  }

  public class OktaVerifyAuthenticator : GoogleAuthenticator {
  }

}