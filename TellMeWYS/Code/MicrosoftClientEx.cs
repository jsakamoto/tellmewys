using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using DotNetOpenAuth.AspNet.Clients;
using Newtonsoft.Json.Linq;

namespace TellMeWYS
{
    public class GetUserDataEventArgs : EventArgs
    {
        public string SourceJsonString { get; protected set; }

        public IDictionary<string, string> UserData { get; protected set; }

        public GetUserDataEventArgs(string jsonStr, IDictionary<string,string> userData)
        {
            this.SourceJsonString = jsonStr;
            this.UserData = userData;
        }
    }

    public class MicrosoftClientEx : MicrosoftClient
    {
        public static readonly string[] UriRfc3986CharsToEscape = new string[] { "!", "*", "'", "(", ")" };

        public static string EscapeUriDataStringRfc3986(string value)
        {
            var stringBuilder = new StringBuilder(Uri.EscapeDataString(value));
            for (int i = 0; i < UriRfc3986CharsToEscape.Length; i++)
            {
                stringBuilder.Replace(UriRfc3986CharsToEscape[i], Uri.HexEscape(UriRfc3986CharsToEscape[i][0]));
            }
            return stringBuilder.ToString();
        }

        public event EventHandler<GetUserDataEventArgs> PreGetuserData;

        public MicrosoftClientEx(string appId, string appSecret, params string[] requestedScopes)
            : base("Microsoft", appId, appSecret, requestedScopes)
        {
        }

        protected override IDictionary<string, string> GetUserData(string accessToken)
        {
            var userData = new Dictionary<string, string>();
            
            var address = "https://apis.live.net/v5.0/me?access_token=" + EscapeUriDataStringRfc3986(accessToken);
            var userDataJsonStr = new WebClient { Encoding = Encoding.UTF8 }
                .DownloadString(address);
            
            var args = new GetUserDataEventArgs(userDataJsonStr, userData);
            if (PreGetuserData != null) PreGetuserData(this, args);
            
            var dynamicUserData = JObject.Parse(userDataJsonStr);

            Action<string, string, JObject> set = null;
            set = (dstkey, srckey, data) =>
            {
                if (userData.ContainsKey(dstkey)) return;

                var srckeys = srckey.Split('.');
                var srckeyHead = srckeys.First();
                var srcKeyTails = srckeys.Skip(1).Join(".");
                var value = default(JToken);
                if (data.TryGetValue(srckeyHead, out value))
                {
                    if (srcKeyTails == "")
                    {
                        var sval = (string)value;
                        if (sval != null) userData.Add(dstkey, sval);
                    }
                    else set(dstkey, srcKeyTails, (JObject)value);
                }
            };

            set("id", "id", dynamicUserData);
            set("username", "name", dynamicUserData);
            set("name", "name", dynamicUserData);
            set("link", "link", dynamicUserData);
            set("gender", "gender", dynamicUserData);
            set("firstname", "first_name", dynamicUserData);
            set("lastname", "last_name", dynamicUserData);
            set("account", "emails.account", dynamicUserData);
            set("email", "emails.preferred", dynamicUserData);

            return userData;
        }
    }
}