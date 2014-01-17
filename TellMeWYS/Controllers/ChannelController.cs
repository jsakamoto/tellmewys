using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using TellMeWYS.Hubs;
using TellMeWYS.Models;

namespace TellMeWYS.Controllers
{
    using TellMeWYS.Models.ViewModel;
    using SignalR = Microsoft.AspNet.SignalR;

    [Authorize]
    public class ChannelController : Controller
    {
        public Func<TellMeWYSDB> DB { get; set; }

        public ChannelController()
        {
            this.DB = () => TellMeWYSDB.Default(this.HttpContext);
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult List_ItemsPartial()
        {
            var db = this.DB();
            var account = this.HttpContext.Account();
            var channels = db.ChannelMembers//.Include("Channels")
                .Where(_ => _.AccountId == account.Id)
                .ToArray()
                .Select(_ => _.Channel);
            return PartialView(channels);
        }

        public ActionResult FindChannel(Guid channelId)
        {
            var db = this.DB();
            var channel = db.Channels.Find(channelId);
            if (channel == null) return HttpNotFound();

            var account = this.HttpContext.Account();
            if (channel.ChannelMembers.Any(_ => _.AccountId == account.Id) == false) return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return new ModelResult<Channel>(channel);
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            var result = this.FindChannel(id);
            if ((result is ModelResult<Channel>) == false) return result;
            var channel = (result as ModelResult<Channel>).Model;

            return View(channel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Message(string id, string url)
        {
            // TODO: Enable CORS

            var clientPortGuid = default(Guid);
            if (Guid.TryParse(id, out clientPortGuid) == false) return HttpNotFound();

            var db = this.DB();
            var channel = db.Channels.FirstOrDefault(_=>_.ClientPort == clientPortGuid);
            if (channel == null) return HttpNotFound();

            var isSafe = false;
            var exactUri = default(Uri);
            if (Uri.TryCreate(url, UriKind.Absolute, out exactUri) == true)
            {
                isSafe = (exactUri.Scheme == "http" || exactUri.Scheme == "https");
            }

            var channelHubContext = SignalR.GlobalHost.ConnectionManager.GetHubContext<ChannelHub>();
            channelHubContext.Clients.Group(channel.Id.ToString("N")).SendUrl(url, isSafe);

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        [AllowAnonymous]
        public ActionResult JS(Guid id)
        {
            // TODO: return Cached content result.
            return View();
        }

        [HttpPost]
        public ActionResult Create()
        {
            if (this.Request.IsAjaxRequest() == false) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var account = this.HttpContext.Account();
            var newChannel = new Channel();
            newChannel.ChannelMembers.Add(new ChannelMember
            {
                AccountId = account.Id,
                IsOwner = true
            });

            var db = this.DB();
            db.Channels.Add(newChannel);
            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        public ActionResult Settings(Guid id)
        {
            var result = this.FindChannel(id);
            if ((result is ModelResult<Channel>) == false) return result;
            var channel = (result as ModelResult<Channel>).Model;

            return View(channel);
        }

        [HttpGet]
        public ActionResult EditChannelName(Guid id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditChannelName(Guid id, Channel model)
        {
            return RedirectToAction("Settings", new { id });
        }

        [HttpGet]
        public ActionResult AddMember(Guid id)
        {
            var model = new AddMemberViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddMember(Guid id, AddMemberViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            var db = this.DB();
            var account = db.Accounts.FirstOrDefault(_ => _.AccountName == model.Name && _.ProviderName == model.Provider);
            if (account == null)
            {
                account = new Account
                {
                    AccountName = model.Name,
                    ProviderName = model.Provider
                };
                db.Accounts.Add(account);
            }
            
            var result = this.FindChannel(id);
            if (result is ModelResult<Channel> == false) return result;
            var channel = (result as ModelResult<Channel>).Model;

            channel.ChannelMembers.Add(new ChannelMember { 
                AccountId = account.Id,
                IsOwner = model.IsOwner
            });
            db.SaveChanges();

            return RedirectToAction("Settings", new { id });
        }

        [HttpGet]
        public ActionResult EditMember(Guid id, Guid id2)
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditMember(Guid id, Guid id2, Account model)
        {
            return RedirectToAction("Settings", new { id });
        }

        [HttpPost]
        public ActionResult DeleteUser(Guid id, Guid id2)
        {
            if (this.Request.IsAjaxRequest() == false) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
