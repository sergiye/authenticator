namespace Authenticator {

  public class GoogleAuthenticator : Authenticator {
  
    public void Enroll(string b32Key) {
      SecretKey = Base32.GetInstance().Decode(b32Key);
    }
  }
  
  public class MicrosoftAuthenticator : GoogleAuthenticator {
  }
  
  public class OktaVerifyAuthenticator : GoogleAuthenticator {
  }

}