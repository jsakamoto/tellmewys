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
    using System.IO;
    using System.Text;
    using Newtonsoft.Json.Linq;
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

        [HttpGet, AuthorizeChannel]
        public ActionResult Index(Guid id)
        {
            var channel = this.DB().Channels.Find(id);
            return View(channel);
        }

        [AllowAnonymous, EnableCors(methods: "POST")]
        public ActionResult Message()
        {
            var requestBody = default(dynamic);
            using (var sr = new StreamReader(this.Request.InputStream, Encoding.UTF8))
                requestBody = JObject.Parse(sr.ReadToEnd());
            var clientPort = (string)requestBody.clientPort;
            var url = (string)requestBody.url;

            var clientPortGuid = default(Guid);
            if (Guid.TryParse(clientPort, out clientPortGuid) == false) return HttpNotFound();

            var db = this.DB();
            var channel = db.Channels.FirstOrDefault(_ => _.ClientPort == clientPortGuid);
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
        public ActionResult JS(string clientPort, string ns)
        {
            var clientPortGuid = default(Guid);
            if (Guid.TryParse(clientPort, out clientPortGuid) == false) return HttpNotFound();
            if (this.DB().Channels.Any(_ => _.ClientPort == clientPortGuid) == false) return HttpNotFound();

            this.ViewBag.ClientPort = clientPort;
            this.ViewBag.NameSpace = ns ?? "window";
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

        [AuthorizeChannel(OwnerOnly = true)]
        public ActionResult Settings(Guid id)
        {
            var channel = this.DB().Channels.Find(id);
            return View(channel);
        }

        [HttpGet, AuthorizeChannel(OwnerOnly = true)]
        public ActionResult EditChannelName(Guid id)
        {
            var channel = this.DB().Channels.Find(id);
            return View(channel);
        }

        [HttpPost, AuthorizeChannel(OwnerOnly = true), ValidateAntiForgeryToken]
        public ActionResult EditChannelName(Guid id, Channel model)
        {
            if (ModelState.IsValid == false) return View(model);

            var channel = this.DB().Channels.Find(id);
            channel.Name = model.Name;
            this.DB().SaveChanges();

            return RedirectToAction("Settings", new { id });
        }

        [HttpGet, AuthorizeChannel(OwnerOnly = true)]
        public ActionResult AddMember(Guid id)
        {
            var model = new AddMemberViewModel();
            return View(model);
        }

        [HttpPost, AuthorizeChannel(OwnerOnly = true)]
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
                db.SaveChanges();
            }

            var channel = this.DB().Channels.Find(id);
            if (channel.ChannelMembers.Any(_ => _.AccountId == account.Id))
            {
                ModelState.AddModelError("Name", "This account is already member of this channel.");
                return View(model);
            }

            channel.ChannelMembers.Add(new ChannelMember
            {
                AccountId = account.Id,
                IsOwner = model.IsOwner
            });
            db.SaveChanges();

            return RedirectToAction("Settings", new { id });
        }

        [HttpPost, AuthorizeChannel(OwnerOnly = true)]
        public ActionResult RemoveMember(Guid id, Guid memberId)
        {
            if (this.Request.IsAjaxRequest() == false) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var channel = this.DB().Channels.Find(id);

            var member = channel.ChannelMembers.FirstOrDefault(_ => _.Id == memberId);
            if (member == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            // Dont remove myself!
            if (this.HttpContext.Account().Id == member.AccountId) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var db = this.DB();
            db.ChannelMembers.Remove(member);
            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        [HttpPost, AuthorizeChannel(OwnerOnly = true)]
        public ActionResult Delete(Guid id)
        {
            if (this.Request.IsAjaxRequest() == false) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var db = this.DB();
            var channel = db.Channels.Find(id);
            channel.ChannelMembers.ToList().ForEach(member => db.ChannelMembers.Remove(member));
            db.Channels.Remove(channel);
            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        [AuthorizeChannel]
        public ActionResult TestDrive(Guid id)
        {
            var db = this.DB();
            var channel = db.Channels.Find(id);

            ViewBag.ChannelId = id.ToString("N");
            ViewBag.ClientPort = channel.ClientPort.ToString("N");
            return View();
        }
    }
}
