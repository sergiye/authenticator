using System;
using QRCoder;

namespace TwoFactorAuth {

  public static class QrGeneratorEx {

    // PngByteQRCode renders without System.Drawing, which QRCoder does not use (and QRCode does not exist) on net6+ targets
    public static string GenerateQrCode(string issuer, string accountTitle, string accountSecretKey, int qrPixelsPerModule = 4) {

      var provisionUrl = QrGenerator.GetProvisionUrl(issuer, accountTitle, accountSecretKey);
      using (var qrGenerator = new QRCodeGenerator())
      using (var qrCodeData = qrGenerator.CreateQrCode(provisionUrl, QRCodeGenerator.ECCLevel.Q))
      using (var qrCode = new PngByteQRCode(qrCodeData)) {
        var png = qrCode.GetGraphic(qrPixelsPerModule);
        return $"data:image/png;base64,{Convert.ToBase64String(png)}";
      }
    }
  }
}
