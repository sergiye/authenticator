using System;
using System.Collections.Generic;

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

  public class InvalidTradesResponseException : AuthenticatorException {
    public InvalidTradesResponseException(string msg = null, Exception ex = null) : base(msg, ex) {
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

  public class BadYubiKeyException : BadPasswordException {
    public BadYubiKeyException(string msg = null, Exception ex = null) : base(msg, ex) {
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

  public class InvalidSecretDataException : AuthenticatorException {
    public InvalidSecretDataException(Exception inner, string password, string encType, List<string> decrypted)
      : base("Error decoding Secret Data", inner) {
      Password = password;
      EncType = encType;
      Decrypted = decrypted;
    }

    public string Password { get; set; }
    public string EncType { get; set; }
    public List<string> Decrypted { get; set; }
  }
}