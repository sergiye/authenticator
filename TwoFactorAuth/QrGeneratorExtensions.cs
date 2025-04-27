using System;
using System.IO;
using QRCoder;

namespace TwoFactorAuth {
  
  public static class QrGeneratorEx {

    public static string GenerateQrCode(string issuer, string accountTitle, string accountSecretKey, int qrPixelsPerModule = 4) {

      var provisionUrl = QrGenerator.GetProvisionUrl(issuer, accountTitle, accountSecretKey);
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
  }
}