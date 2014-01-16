using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using TellMeWYS.Models;

namespace TellMeWYS
{
    public static class Authentication
    {
        public class AuthTicketUserData
        {
            public string AccountName { get; set; }

            public AuthTicketUserData(string accountName)
            {
                this.AccountName = accountName;
            }
        }

        public static void SetAuthCookie(this HttpContextBase context, string userName, string accountName)
        {
            var authTicketUserData = new AuthTicketUserData(accountName).ToJson();

            var encryptedAuthCookie = FormsAuthentication.GetAuthCookie(userName, createPersistentCookie: false);
            var authTicketTemplate = FormsAuthentication.Decrypt(encryptedAuthCookie.Value);
            var authTicket = new FormsAuthenticationTicket(
                version: authTicketTemplate.Version,
                name: authTicketTemplate.Name,
                issueDate: authTicketTemplate.IssueDate,
                expiration: authTicketTemplate.Expiration,
                isPersistent: authTicketTemplate.IsPersistent,
                userData: authTicketUserData,
                cookiePath: authTicketTemplate.CookiePath
            );
            encryptedAuthCookie.Value = FormsAuthentication.Encrypt(authTicket);
            context.Response.Cookies.Add(encryptedAuthCookie);
        }

        public static string AccountName(this IPrincipal user)
        {
            if (user.Identity.IsAuthenticated == false) return "";

            var formsIdentity = user.Identity as FormsIdentity;
            if (formsIdentity == null) return "";
            var authUserData = formsIdentity.Ticket.UserData.AsJsonTo<AuthTicketUserData>();
            return authUserData.AccountName;
        }

        public static Account Account(this HttpContextBase context)
        {
            var db = TellMeWYSDB.Default(context);
            var account = db.Accounts.FirstOrDefault(_ => _.UniqueIdInProvider == context.User.Identity.Name);
            return account;
        }
    }
}