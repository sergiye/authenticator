﻿namespace Authenticator {

  public class RegisteredAuthenticator {
  
    public enum AuthenticatorTypes {
      None = 0,
      BattleNet,
      Google,
      GuildWars,
      Trion,
      Microsoft,
      RFC6238_TIME_COUNTER,
      OktaVerify
    }

    public string Name;
    public AuthenticatorTypes AuthenticatorType;
    public string Icon;
  }
}