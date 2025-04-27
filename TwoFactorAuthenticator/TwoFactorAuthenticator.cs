using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TwoFactorAuth {

  public class TwoFactorAuthenticator {
    
    private int userId;
    private string secretKey;

    private static readonly DateTime epoch =
      new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private TimeSpan DefaultClockDriftTolerance { get; set; }

    public TwoFactorAuthenticator(int userId) {
      DefaultClockDriftTolerance = TimeSpan.FromMinutes(2);

      UserId = userId;
    }

    public int UserId {
      get => userId;
      set {
        userId = value;
        secretKey = GetSecretKey(userId);
      }
    }

    public string ManualSecretKey {
      get {
        var key = secretKey.ToLower().Trim('=');
        string result = string.Empty;
        var idx = 0;
        while (idx < key.Length) {
          if (idx % 4 == 0)
            result += " ";
          result += key[idx];
          idx++;
        }
        return result;
      }
      set {
        secretKey = value;
      }
    }

    public static string GetSecretKey(int userId) {
      string tmps = (userId + 10000).ToString() + (userId + 67000).GetHashCode().ToString("X");
      var accountSecretKeyBytes = Encoding.ASCII.GetBytes(tmps);
      var accountSecretKey = Base32Encoder.ToBase32(accountSecretKeyBytes);
      return accountSecretKey;
    }

    public string GenerateHashedCode(long iterationNumber, int digits = 6) {
      return GenerateHashedCode(
        Base32Encoder.FromBase32(secretKey),
        iterationNumber,
        digits);
    }

    private string GenerateHashedCode(byte[] key, long iterationNumber, int digits = 6) {
      var counter = BitConverter.GetBytes(iterationNumber);

      if (BitConverter.IsLittleEndian)
        Array.Reverse(counter);

      var hmac = new HMACSHA1(key);
      var hash = hmac.ComputeHash(counter);
      var offset = hash[hash.Length - 1] & 0xf;

      // Convert the 4 bytes into an integer, ignoring the sign.
      var binary =
        ((hash[offset] & 0x7f) << 24)
        | (hash[offset + 1] << 16)
        | (hash[offset + 2] << 8)
        | hash[offset + 3];

      var password = binary % (int)Math.Pow(10, digits);
      return password.ToString(new string('0', digits));
    }

    private long GetCurrentCounter() => GetCurrentCounter(DateTime.UtcNow, epoch, 30);

    private long GetCurrentCounter(DateTime now, DateTime epoch, int timeStep) =>
      (long)(now - epoch).TotalSeconds / timeStep;

    public bool ValidateTwoFactorPin(string twoFactorCodeFromClient) {
      return ValidateTwoFactorPin(twoFactorCodeFromClient, DefaultClockDriftTolerance);
    }

    public bool ValidateTwoFactorPin(string twoFactorCodeFromClient, TimeSpan timeTolerance) {
      if (!string.IsNullOrEmpty(twoFactorCodeFromClient))
        twoFactorCodeFromClient = twoFactorCodeFromClient.Replace(" ", "");
      return GetCurrentPins(timeTolerance).Any(c => c == twoFactorCodeFromClient);
    }

    public string GetCurrentPin() =>
      GenerateHashedCode(GetCurrentCounter());

    public string GetCurrentPin(DateTime now) =>
      GenerateHashedCode(GetCurrentCounter(now, epoch, 30));

    public string[] GetCurrentPins() =>
      GetCurrentPins(DefaultClockDriftTolerance);

    public string[] GetCurrentPins(TimeSpan timeTolerance) {
      var codes = new List<string>();
      var iterationCounter = GetCurrentCounter();
      var iterationOffset = 0;

      if (timeTolerance.TotalSeconds > 30) {
        iterationOffset = Convert.ToInt32(timeTolerance.TotalSeconds / 30.00);
      }

      var iterationStart = iterationCounter - iterationOffset;
      var iterationEnd = iterationCounter + iterationOffset;

      for (var counter = iterationStart; counter <= iterationEnd; counter++) {
        codes.Add(GenerateHashedCode(counter));
      }

      return codes.ToArray();
    }

    public string GenerateQrCode(string issuer, string accountTitle) {
      return QrGenerator.GenerateQrCode(issuer, accountTitle, secretKey);
    }

    public Task<byte[]> GenerateQrCodeByGoogle(string issuer, string accountTitle) {
      return QrGenerator.GenerateQrCodeByGoogle(issuer, accountTitle, secretKey);
    }
  }
}
