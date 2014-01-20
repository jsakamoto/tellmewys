using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TellMeWYS.Models;

namespace TellMeWYS
{
    public class AuthorizeChannelAttribute : FilterAttribute, IAuthorizationFilter
    {
        public bool OwnerOnly { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            filterContext.Result = AuthorizeCore(filterContext);
        }

        private ActionResult AuthorizeCore(AuthorizationContext filterContext)
        {
            var channelIdStr = filterContext.RouteData.Values["id"].ToString();
            var channelIdGuid = default(Guid);
            if (Guid.TryParse(channelIdStr, out channelIdGuid) == false) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var db = TellMeWYSDB.Default(filterContext.HttpContext);
            var channel = db.Channels.Find(channelIdGuid);
            if (channel == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var account = filterContext.HttpContext.Account();
            var member = channel.ChannelMembers.FirstOrDefault(_ => _.AccountId == account.Id);
            if (member == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            if (this.OwnerOnly && member.IsOwner == false) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return null;
        }
    }
}