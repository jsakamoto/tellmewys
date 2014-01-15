using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TellMeWYS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ChannelList",
                url: "Channel/List",
                defaults: new { controller = "Channel", action = "List" }
            );

            routes.MapRoute(
                name: "Channel",
                url: "Channel/{id}/{action}/{id2}",
                defaults: new { controller = "Channel", action = "Index", id2 = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}