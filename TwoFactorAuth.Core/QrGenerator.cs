using System;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace TwoFactorAuth {
  
  public static class QrGenerator {

    public static string GetQrCodeLink(string issuer, string accountTitle, string encodedSecretKey, int width = 300, int height = 300) {
      var provisionUrl = GetProvisionUrl(issuer, accountTitle, encodedSecretKey);
      var chartUrl = $"https://chart.apis.google.com/chart?cht=qr&chs={width}x{height}&chl={provisionUrl}";
      return chartUrl;
    }

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
      accountTitle = RemoveWhitespace(Uri.EscapeUriString(accountTitle));
      var provisionUrl = $"otpauth://totp/{accountTitle}?secret={encodedSecretKey.Trim('=')}";
      if (!string.IsNullOrWhiteSpace(issuer))
        provisionUrl += $"&issuer={UrlEncode(issuer)}";
      return provisionUrl;
    }

    private static string UrlEncode(string value) {
      var result = new StringBuilder();
      var validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
      foreach (var symbol in value) {
        if (validChars.IndexOf(symbol) == -1)
          result.AppendFormat("%{0:X2}", (int)symbol);
        else
          result.Append(symbol);
      }
      return result.Replace(" ", "%20").ToString();
    }

    public static string RemoveWhitespace(string str) => new string(str.Where(c => !char.IsWhiteSpace(c)).ToArray());

  }
}