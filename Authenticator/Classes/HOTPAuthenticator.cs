using System;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace Authenticator {

  public class HotpAuthenticator : Authenticator {
    #region Authenticator data

    public long Counter { get; set; }

    #endregion

    public HotpAuthenticator() : base(DEFAULT_CODE_DIGITS) {
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
          Counter = 0;
        }
      }
    }

    public void Enroll(string b32Key, long counter = 0) {
      SecretKey = Base32.GetInstance().Decode(b32Key);
      Counter = counter;
    }

    public override void Sync() {
    }

    protected override string CalculateCode(bool sync = false, long counter = -1) {
      if (sync) {
        if (counter == -1) {
          throw new ArgumentException("counter must be >= 0");
        }

        // set as previous because we increment
        Counter = counter - 1;
      }

      var hmac = new HMac(new Sha1Digest());
      hmac.Init(new KeyParameter(SecretKey));

      // increment counter
      Counter++;

      var codeIntervalArray = BitConverter.GetBytes(Counter);
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

      var fullcode = BitConverter.ToUInt32(bytes, 0) & 0x7fffffff;

      // we use the last 8 digits of this code in radix 10
      var codemask = (uint) Math.Pow(10, CodeDigits);
      var format = new string('0', CodeDigits);
      var code = (fullcode % codemask).ToString(format);

      return code;
    }
  }
}