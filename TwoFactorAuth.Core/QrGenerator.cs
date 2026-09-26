using System;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace TwoFactorAuth {
  
  public static class QrGenerator {

    [Obsolete("The secret key is sent to a third-party service, and the Google Chart QR API is deprecated. Use QrGeneratorEx.GenerateQrCode instead.")]
    public static string GetQrCodeLink(string issuer, string accountTitle, string encodedSecretKey, int width = 300, int height = 300) {
      var provisionUrl = GetProvisionUrl(issuer, accountTitle, encodedSecretKey);
      var chartUrl = $"https://chart.apis.google.com/chart?cht=qr&chs={width}x{height}&chl={Uri.EscapeDataString(provisionUrl)}";
      return chartUrl;
    }

    [Obsolete("The secret key is sent to a third-party service, and the Google Chart QR API is deprecated. Use QrGeneratorEx.GenerateQrCode instead.")]
    public static async Task<byte[]> GenerateQrCodeByGoogle(string issuer, string accountTitle, string encodedSecretKey, int width = 300, int height = 300) {
      var chartUrl = GetQrCodeLink(issuer, accountTitle, encodedSecretKey, width, height);

      using (var client = new HttpClient()) {
        return await client.GetByteArrayAsync(chartUrl).ConfigureAwait(false);
      }
    }

    public static string GetProvisionUrl(string issuer, string accountTitle, string encodedSecretKey) {
      
      if (string.IsNullOrWhiteSpace(accountTitle))
        throw new NotSupportedException("Empty Account Title is not supported.");
      //https://github.com/google/google-authenticator/wiki/Key-Uri-Format
      accountTitle = RemoveWhitespace(Uri.EscapeDataString(accountTitle));
      var provisionUrl = $"otpauth://totp/{accountTitle}?secret={NormalizeSecretKey(encodedSecretKey).Trim('=')}";
      if (!string.IsNullOrWhiteSpace(issuer))
        provisionUrl += $"&issuer={UrlEncode(issuer)}";
      return provisionUrl;
    }

    private static string UrlEncode(string value) {
      var result = new StringBuilder();
      var validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
      foreach (var symbol in value) {
        if (!validChars.Contains(symbol))
          result.AppendFormat("%{0:X2}", (int)symbol);
        else
          result.Append(symbol);
      }
      return result.Replace(" ", "%20").ToString();
    }

    public static string NormalizeSecretKey(string key) => new string(key.Where(c => !char.IsWhiteSpace(c) && c != '-').ToArray()).ToUpperInvariant();

    public static string RemoveWhitespace(string str) => new string(str.Where(c => !char.IsWhiteSpace(c)).ToArray());
  }
}