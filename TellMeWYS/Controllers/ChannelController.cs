using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using TellMeWYS.Models;

namespace TellMeWYS.Controllers
{
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

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(Guid id, Object model)
        {
            // TODO: Enable CORS
            return View();
        }

        [AllowAnonymous]
        public ActionResult JS(Guid id)
        {
            // TODO: return Cached content result.
            return View();
        }

        [HttpGet]
        public ActionResult CreateChannel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateChannel(Channel model)
        {
            return RedirectToAction("List");
        }

        public ActionResult Settings(Guid id)
        {
            return View();
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
            return View();
        }

        [HttpPost]
        public ActionResult AddMember(Guid id, Account model)
        {
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
