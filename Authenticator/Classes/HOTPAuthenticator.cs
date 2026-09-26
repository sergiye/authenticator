using System;

namespace Authenticator {

  public class HotpAuthenticator : Authenticator {
    #region Authenticator data

    public long Counter { get; set; }

    #endregion

    public HotpAuthenticator() {
    }

    public HotpAuthenticator(int digits) : base(digits) {
    }

    public override string SecretData {
      get => base.SecretData + "|" + Counter; //this is the key |  serial | deviceid
      set {
        // extract key + counter
        if (string.IsNullOrEmpty(value) == false) {
          var parts = value.Split('|');
          base.SecretData = value;
          Counter = (parts.Length > 1 ? long.Parse(parts[1]) : 0);
        }
        else {
          base.SecretData = null;
        }
      }
    }

    public void Enroll(string b32Key, long counter = 0) {
      SecretKey = Base32.GetInstance().Decode(b32Key);
      Counter = counter;
    }

    protected override string CalculateCode(bool sync = false, long counter = -1) {
      if (sync) {
        if (counter == -1) {
          throw new ArgumentException("counter must be >= 0");
        }

        // set as previous because we increment
        Counter = counter - 1;
      }

      Counter++;
      return CalculateCode(Counter);
    }
  }
}
