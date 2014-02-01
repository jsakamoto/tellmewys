using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Web.WebPages.OAuth;
using TellMeWYS.Models;

namespace TellMeWYS.Controllers
{
    public class AccountController : Controller
    {
        public Func<TellMeWYSDB> DB { get; set; }

        public AccountController()
        {
            this.DB = () => TellMeWYSDB.Default(this.HttpContext);
        }

        public ActionResult SignIn()
        {
            return View(OAuthWebSecurity.RegisteredClientData);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/");
        }

        [HttpPost]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
#if DEBUG
            if (provider == "guest")
            {
                return ExternalLoginCore(
                    provider,
                    providerUserId: "guest",
                    accountName: "Guest");
            }
#endif
            return new LamdaResult(_ =>
            {
                OAuthWebSecurity.RequestAuthentication(
                    provider,
                    Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            });
        }

        [HttpGet]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            var result = OAuthWebSecurity.VerifyAuthentication(
                Url.Action("ExternalLoginCallback",
                new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return Redirect("~/");
            }

            return ExternalLoginCore(
                result.Provider,
                result.ProviderUserId,
                // terrible hack...
                result.Provider != "github" ? result.ExtraData["email"] : result.ExtraData["login"]);
        }

        internal ActionResult ExternalLoginCore(string provider, string providerUserId, string accountName)
        {
            var user = new Account
            {
                UniqueIdInProvider =
                    (providerUserId + "@" + provider)
                    .ToHashString<MD5>(AppSettings.Salt),
                ProviderName = provider,
                AccountName = accountName
            };

            var db = this.DB();
            var account = db.Accounts.FirstOrDefault(_ => _.ProviderName == user.ProviderName && _.AccountName == user.AccountName);
            if (account == null)
            {
                db.Accounts.Add(user);
            }
            else if (account.UniqueIdInProvider == "")
            {
                account.UniqueIdInProvider = user.UniqueIdInProvider;
            }
            db.SaveChanges();

            this.HttpContext.SetAuthCookie(user.UniqueIdInProvider, user.AccountName);

            return RedirectToAction("List", "Channel");
        }
    }
}
