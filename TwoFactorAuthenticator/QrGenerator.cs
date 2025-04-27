using System;
using System.Linq;
using System.Text;
using System.IO;
using QRCoder;
using System.Net.Http;
using System.Threading.Tasks;

namespace TwoFactorAuth {
  
  public static class QrGenerator {

    //nuget package QRCoder required
    public static string GenerateQrCode(string issuer, string accountTitle, string accountSecretKey, int qrPixelsPerModule = 4) {

      var provisionUrl = GetProvisionUrl(issuer, accountTitle, accountSecretKey);
      var qrCodeUrl = "";
      try {
        using (var qrGenerator = new QRCodeGenerator())
        using (var qrCodeData = qrGenerator.CreateQrCode(provisionUrl, QRCodeGenerator.ECCLevel.Q))
        using (var qrCode = new QRCode(qrCodeData))
        using (var qrCodeImage = qrCode.GetGraphic(qrPixelsPerModule))
        using (var ms = new MemoryStream()) {
          qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
          qrCodeUrl = $"data:image/png;base64,{Convert.ToBase64String(ms.ToArray())}";
        }
      }
      catch (TypeInitializationException e) {
        if (e.InnerException != null
            && e.InnerException.GetType() == typeof(DllNotFoundException)
            && e.InnerException.Message.Contains("libgdiplus")) {
          throw new Exception(
            "It looks like libgdiplus has not been installed - see" +
            " https://github.com/codebude/QRCoder/issues/227",
            e);
        }
      }
      catch (System.Runtime.InteropServices.ExternalException e) {
        if (e.Message.Contains("GDI+") && qrPixelsPerModule > 10) {
          throw new Exception(
            $"There was a problem generating a QR code. The value of {nameof(qrPixelsPerModule)}" +
            " should be set to a value of 10 or less for optimal results.",
            e);
        }
      }
      return qrCodeUrl;
    }

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

    private static string GetProvisionUrl(string issuer, string accountTitle, string encodedSecretKey) {
      
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