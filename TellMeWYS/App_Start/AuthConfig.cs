using System;
using System.Reflection;
using DotNetOpenAuth.AspNet.Clients;
using Microsoft.Web.WebPages.OAuth;

namespace TellMeWYS
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            // Use MicrosoftClientEx class instead of MicrosoftClient, because to retrieve e-mail address.
            var microsoftClient = new MicrosoftClientEx(
                appId: AppSettings.MicrosoftAccount_ClientId,
                appSecret: AppSettings.MicrosoftAccount_ClientSecret,
                requestedScopes: new[] { "wl.emails" });
            OAuthWebSecurity.RegisterClient(microsoftClient, microsoftClient.ProviderName, null);

            // Hack: Fix unmatch of google client provider name and display name upper/lower case, 
            // but ProviderName property is read only, so I hacked this by reflection.
            var googleClient = new GoogleOpenIdClient();
            typeof(OpenIdClient)
                .InvokeMember("providerName", BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Instance, null, googleClient,
                new object[] { "Google" });
            OAuthWebSecurity.RegisterClient(googleClient, googleClient.ProviderName, null);
        }
    }
}
