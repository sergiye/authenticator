using System;

namespace Authenticator {
  /// <summary>
  /// Exception class wrapper for importing error
  /// </summary>
  public class ImportException : ApplicationException {
    public ImportException() {
    }

    public ImportException(string message) : base(message) {
    }

    public ImportException(string message, Exception ex) : base(message, ex) {
    }
  }
}