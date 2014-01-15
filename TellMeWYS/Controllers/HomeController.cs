using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TellMeWYS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var viewName = this.Request.IsAuthenticated ? "Index_Authenticated" : "Index";
            return View(viewName);
        }
    }
}
