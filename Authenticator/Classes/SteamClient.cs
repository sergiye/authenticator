﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Authenticator {
  public class SteamClient : IDisposable {
    private const string COMMUNITY_DOMAIN = "steamcommunity.com";

    private const string COMMUNITY_BASE = "https://" + COMMUNITY_DOMAIN;
    private static string webapiBase = "https://api.steampowered.com";
    private static string apiGetwgtoken = webapiBase + "/IMobileAuthService/GetWGToken/v0001";
    private static string apiLogoff = webapiBase + "/ISteamWebUserPresenceOAuth/Logoff/v0001";

    private const string USERAGENT =
      "Mozilla/5.0 (Linux; U; Android 4.1.1; en-us; Google Nexus 4 - 4.1.1 - API 16 - 768x1280 Build/JRO03S) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30";

    private static Regex tradesRegex = new Regex("\"mobileconf_list_entry\"(.*?)>(.*?)\"mobileconf_list_entry_sep\"",
      RegexOptions.Singleline | RegexOptions.IgnoreCase);

    private static Regex tradeConfidRegex =
      new Regex(@"data-confid\s*=\s*""([^""]+)""", RegexOptions.Singleline | RegexOptions.IgnoreCase);

    private static Regex tradeKeyRegex =
      new Regex(@"data-key\s*=\s*""([^""]+)""", RegexOptions.Singleline | RegexOptions.IgnoreCase);

    private static Regex tradePlayerRegex = new Regex("\"mobileconf_list_entry_icon\"(.*?)src=\"([^\"]+)\"",
      RegexOptions.Singleline | RegexOptions.IgnoreCase);

    private static Regex tradeDetailsRegex =
      new Regex(
        "\"mobileconf_list_entry_description\".*?<div>([^<]*)</div>[^<]*<div>([^<]*)</div>[^<]*<div>([^<]*)</div>[^<]*</div>",
        RegexOptions.Singleline | RegexOptions.IgnoreCase);

    private const int DEFAULT_CONFIRMATIONPOLLER_RETRIES = 3;

    public const int CONFIRMATION_EVENT_DELAY = 1000;

    public enum PollerAction {
      None = 0,
      Notify = 1,
      AutoConfirm = 2,
      SilentAutoConfirm = 3
    }

    public class ConfirmationPoller {
      public int Duration;

      public PollerAction Action;

      public List<string> Ids;

      public override string ToString() {
        if (Duration == 0) {
          return "null";
        }

        var props = new List<string>();

        props.Add("\"duration\":" + Duration);
        props.Add("\"action\":" + (int) Action);
        if (Ids != null) {
          props.Add("\"ids\":[" +
                    (Ids.Count != 0 ? "\"" + string.Join("\",\"", Ids.ToArray()) + "\"" : string.Empty) + "]");
        }

        return "{" + string.Join(",", props.ToArray()) + "}";
      }

      public static ConfirmationPoller FromJson(JToken tokens) {
        if (tokens == null)
          return null;

        var poller = new ConfirmationPoller();

        var token = tokens.SelectToken("duration");
        if (token != null) {
          poller.Duration = token.Value<int>();
        }

        token = tokens.SelectToken("action");
        if (token != null) {
          poller.Action = (PollerAction) token.Value<int>();
        }

        token = tokens.SelectToken("ids");
        if (token != null) {
          poller.Ids = token.ToObject<List<string>>();
        }

        return (poller.Duration != 0 ? poller : null);
      }
    }

    public class Confirmation {
      public string Id;
      public string Key;
      public bool Offline;
      public bool IsNew;
      public string Image;
      public string Details;
      public string Traded;
      public string When;
    }

    public class SteamSession {
      public string SteamId;
      public CookieContainer Cookies;
      public string OAuthToken;
      public string UmqId;
      public int MessageId;
      public ConfirmationPoller Confirmations;

      public SteamSession() {
        Clear();
      }

      public SteamSession(string json) : this() {
        if (string.IsNullOrEmpty(json) == false) {
          try {
            FromJson(json);
          }
          catch (Exception) {
            // invalid json
          }
        }
      }

      public void Clear() {
        OAuthToken = null;
        UmqId = null;
        Cookies = new CookieContainer();
        Confirmations = null;
      }

      public override string ToString() {
        return "{\"steamid\":\"" + (SteamId ?? string.Empty) + "\","
               + "\"cookies\":\"" + Cookies.GetCookieHeader(new Uri(COMMUNITY_BASE + "/")) + "\","
               + "\"oauthtoken\":\"" + (OAuthToken ?? string.Empty) + "\","
               // + "\"umqid\":\"" + (this.UmqId ?? string.Empty) + "\","
               + "\"confs\":" + (Confirmations != null ? Confirmations.ToString() : "null")
               + "}";
      }

      private void FromJson(string json) {
        var tokens = JObject.Parse(json);
        var token = tokens.SelectToken("steamid");
        if (token != null) {
          SteamId = token.Value<string>();
        }

        token = tokens.SelectToken("cookies");
        if (token != null) {
          Cookies = new CookieContainer();

          // Net3.5 has a bug that prepends "." to domain, e.g. ".steamcommunity.com"
          var uri = new Uri(COMMUNITY_BASE + "/");
          var match = Regex.Match(token.Value<string>(), @"([^=]+)=([^;]*);?", RegexOptions.Singleline);
          while (match.Success) {
            Cookies.Add(uri, new Cookie(match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim()));
            match = match.NextMatch();
          }
        }

        token = tokens.SelectToken("oauthtoken");
        if (token != null) {
          OAuthToken = token.Value<string>();
        }

        //token = tokens.SelectToken("umqid");
        //if (token != null)
        //{
        //	this.UmqId = token.Value<string>();
        //}
        token = tokens.SelectToken("confs");
        if (token != null) {
          Confirmations = ConfirmationPoller.FromJson(token);
        }
      }
    }

    public bool InvalidLogin;
    public bool RequiresCaptcha;
    public string CaptchaId;
    public string CaptchaUrl;
    public bool Requires2Fa;
    public bool RequiresEmailAuth;
    public string EmailDomain;
    public string Error;

    public SteamSession Session;
    public SteamAuthenticator Authenticator;

    private string confirmationsHtml;
    private string confirmationsQuery;
    private CancellationTokenSource pollerCancellation;
    public int ConfirmationPollerRetries = DEFAULT_CONFIRMATIONPOLLER_RETRIES;

    public SteamClient(SteamAuthenticator auth, string session = null) {
      Authenticator = auth;
      Session = new SteamSession(session);

      if (Session.Confirmations != null) {
        if (IsLoggedIn() == false) {
          Session.Confirmations = null;
        }
        else {
          Task.Factory.StartNew(() => {
            Refresh();
            PollConfirmations(Session.Confirmations);
          });
        }
      }
    }

    ~SteamClient() {
      Dispose(false);
    }

    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
      if (disposing) {
        // clear resources
      }

      if (pollerCancellation != null) {
        pollerCancellation.Cancel();
        pollerCancellation = null;
      }
    }

    public void Clear() {
      InvalidLogin = false;
      RequiresCaptcha = false;
      CaptchaId = null;
      CaptchaUrl = null;
      RequiresEmailAuth = false;
      EmailDomain = null;
      Requires2Fa = false;
      Error = null;

      Session.Clear();
    }

    public bool IsLoggedIn() {
      return (Session != null && string.IsNullOrEmpty(Session.OAuthToken) == false);
    }

    public bool Login(string username, string password, string captchaId = null, string captchaText = null) {
      // clear error
      Error = null;

      var data = new NameValueCollection();
      string response;

      if (IsLoggedIn() == false) {
        // get session
        if (Session.Cookies.Count == 0) {
          // .Net3.5 has a bug in CookieContainer that prepends a "." to the domain, i.e. ".steamcommunity.com"
          var cookieuri = new Uri(COMMUNITY_BASE + "/");
          Session.Cookies.Add(cookieuri, new Cookie("mobileClientVersion", "3067969+%282.1.3%29"));
          Session.Cookies.Add(cookieuri, new Cookie("mobileClient", "android"));
          Session.Cookies.Add(cookieuri, new Cookie("steamid", ""));
          Session.Cookies.Add(cookieuri, new Cookie("steamLogin", ""));
          Session.Cookies.Add(cookieuri, new Cookie("Steam_Language", "english"));
          Session.Cookies.Add(cookieuri, new Cookie("dob", ""));

          var headers = new NameValueCollection();
          headers.Add("X-Requested-With", "com.valvesoftware.android.steam.community");

          response = GetString(
            COMMUNITY_BASE +
            "/mobilelogin?oauth_client_id=DE45CD61&oauth_scope=read_profile%20write_profile%20read_client%20write_client",
            "GET", null, headers);
        }

        // Steam strips any non-ascii chars from username and password
        username = Regex.Replace(username, @"[^\u0000-\u007F]", string.Empty);
        password = Regex.Replace(password, @"[^\u0000-\u007F]", string.Empty);

        // get the user's RSA key
        data.Add("username", username);
        response = GetString(COMMUNITY_BASE + "/mobilelogin/getrsakey", "POST", data);
        var rsaresponse = JObject.Parse(response);
        if (rsaresponse.SelectToken("success").Value<bool>() != true) {
          InvalidLogin = true;
          Error = "Unknown username";
          return false;
        }

        // encrypt password with RSA key
        // var random = new RNGCryptoServiceProvider();
        string encryptedPassword64;
        using (var rsa = new RSACryptoServiceProvider()) {
          var passwordBytes = Encoding.ASCII.GetBytes(password);
          var p = rsa.ExportParameters(false);
          p.Exponent = StringToByteArray(rsaresponse.SelectToken("publickey_exp").Value<string>());
          p.Modulus = StringToByteArray(rsaresponse.SelectToken("publickey_mod").Value<string>());
          rsa.ImportParameters(p);
          var encryptedPassword = rsa.Encrypt(passwordBytes, false);
          encryptedPassword64 = Convert.ToBase64String(encryptedPassword);
        }

        // login request
        data = new NameValueCollection();
        data.Add("password", encryptedPassword64);
        data.Add("username", username);
        data.Add("twofactorcode", Authenticator.CurrentCode);
        //data.Add("emailauth", string.Empty);
        data.Add("loginfriendlyname", "#login_emailauth_friendlyname_mobile");
        data.Add("captchagid", (string.IsNullOrEmpty(captchaId) == false ? captchaId : "-1"));
        data.Add("captcha_text", (string.IsNullOrEmpty(captchaText) == false ? captchaText : "enter above characters"));
        //data.Add("emailsteamid", (string.IsNullOrEmpty(emailcode) == false ? this.SteamId ?? string.Empty : string.Empty));
        data.Add("rsatimestamp", rsaresponse.SelectToken("timestamp").Value<string>());
        data.Add("remember_login", "false");
        data.Add("oauth_client_id", "DE45CD61");
        data.Add("oauth_scope", "read_profile write_profile read_client write_client");
        data.Add("donotache",
          new DateTime().ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
            .TotalMilliseconds.ToString());
        response = GetString(COMMUNITY_BASE + "/mobilelogin/dologin/", "POST", data);
        var loginresponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

        if (loginresponse.ContainsKey("emailsteamid")) {
          Session.SteamId = loginresponse["emailsteamid"] as string;
        }

        InvalidLogin = false;
        RequiresCaptcha = false;
        CaptchaId = null;
        CaptchaUrl = null;
        RequiresEmailAuth = false;
        EmailDomain = null;
        Requires2Fa = false;

        if (loginresponse.ContainsKey("login_complete") == false || (bool) loginresponse["login_complete"] == false ||
            loginresponse.ContainsKey("oauth") == false) {
          InvalidLogin = true;

          // require captcha
          if (loginresponse.ContainsKey("captcha_needed") && (bool) loginresponse["captcha_needed"]) {
            RequiresCaptcha = true;
            CaptchaId = (string) loginresponse["captcha_gid"];
            CaptchaUrl = COMMUNITY_BASE + "/public/captcha.php?gid=" + CaptchaId;
          }

          // require email auth
          if (loginresponse.ContainsKey("emailauth_needed") && (bool) loginresponse["emailauth_needed"]) {
            if (loginresponse.ContainsKey("emaildomain")) {
              var emaildomain = (string) loginresponse["emaildomain"];
              if (string.IsNullOrEmpty(emaildomain) == false) {
                EmailDomain = emaildomain;
              }
            }

            RequiresEmailAuth = true;
          }

          // require email auth
          if (loginresponse.ContainsKey("requires_twofactor") && (bool) loginresponse["requires_twofactor"]) {
            Requires2Fa = true;
          }

          if (loginresponse.ContainsKey("message")) {
            Error = (string) loginresponse["message"];
          }

          return false;
        }

        // get the OAuth token
        var oauth = (string) loginresponse["oauth"];
        var oauthjson = JObject.Parse(oauth);
        Session.OAuthToken = oauthjson.SelectToken("oauth_token").Value<string>();
        if (oauthjson.SelectToken("steamid") != null) {
          Session.SteamId = oauthjson.SelectToken("steamid").Value<string>();
        }

        //// perform UMQ login
        //data.Clear();
        //data.Add("access_token", this.Session.OAuthToken);
        //response = GetString(API_LOGON, "POST", data);
        //loginresponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
        //if (loginresponse.ContainsKey("umqid") == true)
        //{
        //	this.Session.UmqId = (string)loginresponse["umqid"];
        //	if (loginresponse.ContainsKey("message") == true)
        //	{
        //		this.Session.MessageId = Convert.ToInt32(loginresponse["message"]);
        //	}
        //}
      }

      return true;
    }

    public void Logout() {
      if (string.IsNullOrEmpty(Session.OAuthToken) == false) {
        PollConfirmationsStop();

        if (string.IsNullOrEmpty(Session.UmqId) == false) {
          var data = new NameValueCollection();
          data.Add("access_token", Session.OAuthToken);
          data.Add("umqid", Session.UmqId);
          GetString(apiLogoff, "POST", data);
        }
      }

      Clear();
    }

    public bool Refresh() {
      try {
        var data = new NameValueCollection {{"access_token", Session.OAuthToken}};
        var response = GetString(apiGetwgtoken, "POST", data);
        if (string.IsNullOrEmpty(response)) {
          return false;
        }

        var json = JObject.Parse(response);
        var token = json.SelectToken("response.token");
        if (token == null) {
          return false;
        }

        var cookieuri = new Uri(COMMUNITY_BASE + "/");
        Session.Cookies.Add(cookieuri, new Cookie("steamLogin", Session.SteamId + "||" + token.Value<string>()));

        token = json.SelectToken("response.token_secure");
        if (token == null) {
          return false;
        }

        Session.Cookies.Add(cookieuri, new Cookie("steamLoginSecure", Session.SteamId + "||" + token.Value<string>()));

        // perform UMQ login
        //response = GetString(API_LOGON, "POST", data);
        //var loginresponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
        //if (loginresponse.ContainsKey("umqid") == true)
        //{
        //	this.Session.UmqId = (string)loginresponse["umqid"];
        //	if (loginresponse.ContainsKey("message") == true)
        //	{
        //		this.Session.MessageId = Convert.ToInt32(loginresponse["message"]);
        //	}
        //}

        return true;
      }
      catch (Exception) {
        return false;
      }
    }

    public delegate void ConfirmationDelegate(object sender, Confirmation newconfirmation, PollerAction action);

    public delegate void ConfirmationErrorDelegate(object sender, string message, PollerAction action, Exception ex);

    public event ConfirmationDelegate ConfirmationEvent;

    public event ConfirmationErrorDelegate ConfirmationErrorEvent;

    public static Task Delay(int milliseconds, CancellationToken cancel) {
      var tcs = new TaskCompletionSource<object>();
      cancel.Register(() => tcs.TrySetCanceled());
      Timer timer = new Timer(_ => tcs.TrySetResult(null));
      timer.Change(milliseconds, -1);
      return tcs.Task;
    }

    protected void PollConfirmationsStop() {
      // kill any existing poller
      if (pollerCancellation != null) {
        pollerCancellation.Cancel();
        pollerCancellation = null;
      }

      Session.Confirmations = null;
    }

    public void PollConfirmations(ConfirmationPoller poller) {
      PollConfirmationsStop();

      if (poller == null || poller.Duration <= 0) {
        return;
      }

      if (Session.Confirmations == null) {
        Session.Confirmations = new ConfirmationPoller();
      }

      Session.Confirmations = poller;

      pollerCancellation = new CancellationTokenSource();
      var token = pollerCancellation.Token;
      Task.Factory.StartNew(() => PollConfirmations(token), token, TaskCreationOptions.LongRunning,
        TaskScheduler.Default);
    }

    public async void PollConfirmations(CancellationToken cancel) {
      //lock (this.Session.Confirmations)
      //{
      //	if (this.Session.Confirmations.Ids == null)
      //	{
      //		try
      //		{
      //			// this will update the session
      //			GetConfirmations();
      //		}
      //		catch (InvalidSteamRequestException)
      //		{
      //			// ignore in case of Steam timeout
      //		}
      //	}
      //}

      try {
        var retryCount = 0;
        while (!cancel.IsCancellationRequested && Session.Confirmations != null) {
          try {
            //List<string> currentIds;
            //lock (this.Session.Confirmations)
            //{
            //	currentIds = this.Session.Confirmations.Ids;
            //}

            var confs = GetConfirmations();

            // check for new ids
            //List<string> newIds;
            //if (currentIds == null)
            //{
            //	newIds = confs.Select(t => t.Id).ToList();
            //}
            //else
            //{
            //	newIds = confs.Select(t => t.Id).Except(currentIds).ToList();
            //}

            // fire events if subscriber
            if (ConfirmationEvent != null /* && newIds.Count() != 0 */) {
              var rand = new Random();
              foreach (var conf in confs) {
                if (cancel.IsCancellationRequested) {
                  break;
                }

                var start = DateTime.Now;

                ConfirmationEvent(this, conf, Session.Confirmations.Action);

                // Issue#339: add a delay for any autoconfs or notifications
                var delay = CONFIRMATION_EVENT_DELAY +
                            rand.Next(CONFIRMATION_EVENT_DELAY / 2); // delay is 100%-150% of CONFIRMATION_EVENT_DELAY
                var duration = (int) DateTime.Now.Subtract(start).TotalMilliseconds;
                if (delay > duration) {
                  Thread.Sleep(delay - duration);
                }
              }
            }

            retryCount = 0;
          }
          catch (TaskCanceledException) {
            throw;
          }
          catch (Exception ex) {
            retryCount++;
            if (retryCount >= ConfirmationPollerRetries) {
              ConfirmationErrorEvent?.Invoke(this, "Failed to read confirmations", Session.Confirmations.Action, ex);
            }
            else {
              // try and reset the session
              try {
                Refresh();
              }
              catch (Exception) {
                // ignored
              }
            }
          }

          if (Session.Confirmations != null) {
            await Delay(this.Session.Confirmations.Duration * 60 * 1000, cancel);
          }
        }
      }
      catch (TaskCanceledException) {
      }
    }

    public List<Confirmation> GetConfirmations() {
      var servertime = (global::Authenticator.Authenticator.CurrentTime + Authenticator.ServerTimeDiff) / 1000L;

      var jids = JObject.Parse(Authenticator.SteamData).SelectToken("identity_secret");
      var ids = (jids != null ? jids.Value<string>() : string.Empty);

      var timehash = CreateTimeHash(servertime, "conf", ids);

      var data = new NameValueCollection();
      data.Add("p", Authenticator.DeviceId);
      data.Add("a", Session.SteamId);
      data.Add("k", timehash);
      data.Add("t", servertime.ToString());
      data.Add("m", "android");
      data.Add("tag", "conf");

      var html = GetString(COMMUNITY_BASE + "/mobileconf/conf", "GET", data);

      // save last html for confirmations details
      confirmationsHtml = html;
      confirmationsQuery = string.Join("&",
        Array.ConvertAll(data.AllKeys,
          key => String.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(data[key]))));

      var trades = new List<Confirmation>();

      // extract the trades
      var match = tradesRegex.Match(html);
      while (match.Success) {
        var tradeIds = match.Groups[1].Value;

        var trade = new Confirmation();

        var innerMatch = tradeConfidRegex.Match(tradeIds);
        if (innerMatch.Success) {
          trade.Id = innerMatch.Groups[1].Value;
        }

        innerMatch = tradeKeyRegex.Match(tradeIds);
        if (innerMatch.Success) {
          trade.Key = innerMatch.Groups[1].Value;
        }

        var traded = match.Groups[2].Value;

        innerMatch = tradePlayerRegex.Match(traded);
        if (innerMatch.Success) {
          if (innerMatch.Groups[1].Value.IndexOf("offline") != -1) {
            trade.Offline = true;
          }

          trade.Image = innerMatch.Groups[2].Value;
        }

        innerMatch = tradeDetailsRegex.Match(traded);
        if (innerMatch.Success) {
          trade.Details = innerMatch.Groups[1].Value;
          trade.Traded = innerMatch.Groups[2].Value;
          trade.When = innerMatch.Groups[3].Value;
        }

        trades.Add(trade);

        match = match.NextMatch();
      }

      if (Session.Confirmations != null) {
        lock (Session.Confirmations) {
          if (Session.Confirmations.Ids == null) {
            Session.Confirmations.Ids = new List<string>();
          }

          foreach (var conf in trades) {
            conf.IsNew = (Session.Confirmations.Ids.Contains(conf.Id) == false);
            if (conf.IsNew) {
              Session.Confirmations.Ids.Add(conf.Id);
            }
          }

          var newIds = trades.Select(t => t.Id).ToList();
          foreach (var confId in Session.Confirmations.Ids.ToList()) {
            if (newIds.Contains(confId) == false) {
              Session.Confirmations.Ids.Remove(confId);
            }
          }
        }
      }

      return trades;
    }

    public string GetConfirmationDetails(Confirmation trade) {
      // build details URL
      var url = COMMUNITY_BASE + "/mobileconf/details/" + trade.Id + "?" + confirmationsQuery;

      var response = GetString(url);
      if (response.IndexOf("success") == -1) {
        throw new InvalidSteamRequestException("Invalid request from steam: " + response);
      }

      if (JObject.Parse(response).SelectToken("success").Value<bool>()) {
        var html = JObject.Parse(response).SelectToken("html").Value<string>();

        var detailsRegex = new Regex(@"(.*<body[^>]*>\s*<div\s+class=""[^""]+"">).*(</div>.*?</body>\s*</html>)",
          RegexOptions.Singleline | RegexOptions.IgnoreCase);
        var match = detailsRegex.Match(confirmationsHtml);
        if (match.Success) {
          return match.Groups[1].Value + html + match.Groups[2].Value;
        }
      }

      return "<html><head></head><body><p>Cannot load trade confirmation details</p></body></html>";
    }

    public bool ConfirmTrade(string id, string key, bool accept) {
      if (string.IsNullOrEmpty(Session.OAuthToken)) {
        return false;
      }

      var servertime = (global::Authenticator.Authenticator.CurrentTime + Authenticator.ServerTimeDiff) / 1000L;

      var jids = JObject.Parse(Authenticator.SteamData).SelectToken("identity_secret");
      var ids = (jids != null ? jids.Value<string>() : string.Empty);
      var timehash = CreateTimeHash(servertime, "conf", ids);

      var data = new NameValueCollection();
      data.Add("op", accept ? "allow" : "cancel");
      data.Add("p", Authenticator.DeviceId);
      data.Add("a", Session.SteamId);
      data.Add("k", timehash);
      data.Add("t", servertime.ToString());
      data.Add("m", "android");
      data.Add("tag", "conf");
      //
      data.Add("cid", id);
      data.Add("ck", key);

      try {
        var response = GetString(COMMUNITY_BASE + "/mobileconf/ajaxop", "GET", data);
        if (string.IsNullOrEmpty(response)) {
          Error = "Blank response";
          return false;
        }

        var success = JObject.Parse(response).SelectToken("success");
        if (success == null || success.Value<bool>() == false) {
          Error = "Failed";
          return false;
        }

        if (Session.Confirmations != null) {
          lock (Session.Confirmations) {
            if (Session.Confirmations.Ids.Contains(id)) {
              Session.Confirmations.Ids.Remove(id);
            }
          }
        }

        return true;
      }
      catch (InvalidSteamRequestException ex) {
#if DEBUG
        Error = ex.Message + Environment.NewLine + ex.StackTrace;
#else
				this.Error = ex.Message;
#endif
        return false;
      }
    }

    private static string CreateTimeHash(long time, string tag, string secret) {
      var b64Secret = Convert.FromBase64String(secret);

      var bufferSize = 8;
      if (string.IsNullOrEmpty(tag) == false) {
        bufferSize += Math.Min(32, tag.Length);
      }

      var buffer = new byte[bufferSize];

      var timeArray = BitConverter.GetBytes(time);
      if (BitConverter.IsLittleEndian) {
        Array.Reverse(timeArray);
      }

      Array.Copy(timeArray, buffer, 8);
      if (string.IsNullOrEmpty(tag) == false) {
        Array.Copy(Encoding.UTF8.GetBytes(tag), 0, buffer, 8, bufferSize - 8);
      }

      var hmac = new HMACSHA1(b64Secret, true);
      var hash = hmac.ComputeHash(buffer);

      return Convert.ToBase64String(hash, Base64FormattingOptions.None);
    }

    #region Web Request

    public string GetString(string url, string method = null, NameValueCollection formdata = null,
      NameValueCollection headers = null) {
      var data = Request(url, method ?? "GET", formdata, headers);
      if (data == null || data.Length == 0)
        return string.Empty;
      return Encoding.UTF8.GetString(data);
    }

    protected byte[] Request(string url, string method, NameValueCollection data, NameValueCollection headers) {
      // ensure only one request per account at a time
      lock (this) {
        // create form-encoded data for query or body
        var query = (data == null
          ? string.Empty
          : string.Join("&",
            Array.ConvertAll(data.AllKeys,
              key => String.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(data[key])))));
        if (string.Compare(method, "GET", true) == 0) {
          url += (url.IndexOf("?") == -1 ? "?" : "&") + query;
        }

        // call the server
        var request = (HttpWebRequest) WebRequest.Create(url);
        request.Method = method;
        request.Accept = "text/javascript, text/html, application/xml, text/xml, */*";
        request.ServicePoint.Expect100Continue = false;
        request.UserAgent = USERAGENT;
        request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
        request.Referer = COMMUNITY_BASE;
        if (headers != null) {
          request.Headers.Add(headers);
        }

        request.CookieContainer = Session.Cookies;

        if (string.Compare(method, "POST", true) == 0) {
          request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
          request.ContentLength = query.Length;

          var requestStream = new StreamWriter(request.GetRequestStream());
          requestStream.Write(query);
          requestStream.Close();
        }

        try {
          using (var response = (HttpWebResponse) request.GetResponse()) {
            LogRequest(method, url, request.CookieContainer, data,
              response.StatusCode + " " + response.StatusDescription);

            // OK?
            if (response.StatusCode != HttpStatusCode.OK) {
              throw new InvalidSteamRequestException(string.Format("{0}: {1}", (int) response.StatusCode,
                response.StatusDescription));
            }

            // load the response
            using (var ms = new MemoryStream()) {
              var buffer = new byte[4096];
              int read;
              while ((read = response.GetResponseStream().Read(buffer, 0, 4096)) > 0) {
                ms.Write(buffer, 0, read);
              }

              var responsedata = ms.ToArray();

              LogRequest(method, url, request.CookieContainer, data,
                responsedata.Length != 0
                  ? Encoding.UTF8.GetString(responsedata)
                  : string.Empty);

              return responsedata;
            }
          }
        }
        catch (Exception ex) {
          LogException(method, url, request.CookieContainer, data, ex);

          if (ex is WebException exception && exception.Response != null &&
              ((HttpWebResponse) exception.Response).StatusCode == HttpStatusCode.Forbidden) {
            throw new UnauthorisedSteamRequestException(exception);
          }

          throw new InvalidSteamRequestException(ex.Message, ex);
        }
      }
    }

    private static void LogException(string method, string url, CookieContainer cookies, NameValueCollection request,
      Exception ex) {
      var data = new StringBuilder();
      if (cookies != null) {
        foreach (Cookie cookie in cookies.GetCookies(new Uri(url))) {
          if (data.Length == 0) {
            data.Append("Cookies:");
          }
          else {
            data.Append("&");
          }

          data.Append(cookie.Name + "=" + cookie.Value);
        }

        data.Append(" ");
      }

      if (request != null) {
        foreach (var key in request.AllKeys) {
          if (data.Length == 0) {
            data.Append("Req:");
          }
          else {
            data.Append("&");
          }

          data.Append(key + "=" + request[key]);
        }

        data.Append(" ");
      }

      //Logger.Error(ex, "{0}\t{1}\t{2}", method, url, data.ToString());
    }

    private static void LogRequest(string method, string url, CookieContainer cookies, NameValueCollection request,
      string response) {
      var data = new StringBuilder();
      if (cookies != null) {
        foreach (Cookie cookie in cookies.GetCookies(new Uri(url))) {
          if (data.Length == 0) {
            data.Append("Cookies:");
          }
          else {
            data.Append("&");
          }

          data.Append(cookie.Name + "=" + cookie.Value);
        }

        data.Append(" ");
      }

      if (request != null) {
        foreach (var key in request.AllKeys) {
          if (data.Length == 0) {
            data.Append("Req:");
          }
          else {
            data.Append("&");
          }

          data.Append(key + "=" + request[key]);
        }

        data.Append(" ");
      }

      //Logger.Info("{0}\t{1}\t{2}\t{3}", method, url, data.ToString(), (response != null ? response.Replace("\n", "\\n").Replace("\r", "") : string.Empty));
    }

    #endregion

    private static byte[] StringToByteArray(string hex) {
      var len = hex.Length;
      var bytes = new byte[len / 2];
      for (var i = 0; i < len; i += 2) {
        bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
      }

      return bytes;
    }

    public class InvalidSteamRequestException : ApplicationException {
      public InvalidSteamRequestException(string msg = null, Exception ex = null) : base(msg, ex) {
      }
    }

    public class UnauthorisedSteamRequestException : InvalidSteamRequestException {
      public UnauthorisedSteamRequestException(Exception ex = null) : base("Unauthorised", ex) {
      }
    }
  }
}