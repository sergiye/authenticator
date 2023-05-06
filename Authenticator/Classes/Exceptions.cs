using System;

namespace Authenticator {
  public class AuthenticatorException : ApplicationException {
    public AuthenticatorException() {
    }

    public AuthenticatorException(string msg)
      : base(msg) {
    }

    public AuthenticatorException(string msg, Exception ex)
      : base(msg, ex) {
    }
  }

  public class InvalidHmacAlgorithmException : AuthenticatorException {
  }

  public class InvalidEnrollResponseException : AuthenticatorException {
    public InvalidEnrollResponseException(string msg = null, Exception ex = null) : base(msg, ex) {
    }
  }

  public class InvalidSyncResponseException : AuthenticatorException {
    public InvalidSyncResponseException(string msg) : base(msg) {
    }
  }

  public class EncryptedSecretDataException : AuthenticatorException {
  }

  public class BadPasswordException : AuthenticatorException {
    public BadPasswordException(string msg = null, Exception ex = null) : base(msg, ex) {
    }
  }

  public class InvalidRestoreResponseException : AuthenticatorException {
    public InvalidRestoreResponseException(string msg) : base(msg) {
    }
  }

  public class InvalidRestoreCodeException : InvalidRestoreResponseException {
    public InvalidRestoreCodeException(string msg) : base(msg) {
    }
  }

  public class InvalidEncryptionException : AuthenticatorException {
    public InvalidEncryptionException(string plain, string password, string encrypted, string decrypted) {
      Plain = plain;
      Password = password;
      Encrypted = encrypted;
      Decrypted = decrypted;
    }

    public string Plain { get; set; }
    public string Password { get; set; }
    public string Encrypted { get; set; }
    public string Decrypted { get; set; }
  }
  
  public class ImportException : ApplicationException {
    public ImportException() {
    }

    public ImportException(string message) : base(message) {
    }

    public ImportException(string message, Exception ex) : base(message, ex) {
    }
  }
}