using System.Windows.Forms;

namespace Authenticator {
  /// <summary>
  /// Interface for secret textbox that cannot be copied
  /// </summary>
  public interface ISecretTextBox {
    bool SecretMode { get; set; }
    int SpaceOut { get; set; }
    HorizontalAlignment TextAlign { get; set; }
  }
}