using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace TellMeWYS
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class EnableCorsAttribute : ActionFilterAttribute
    {
        protected string _Origins;

        // protected string _Headers;

        protected IEnumerable<string> _Methods;

        public EnableCorsAttribute(string origins = "*", string headers = "*", string methods = "*")
        {
            this._Origins = origins;
            // this._Headers = headers;
            if (headers != "*") throw new NotImplementedException("Can only specified * to headres argument at this version.");

            this._Methods = methods == "*" ? Enumerable.Empty<string>() :
                methods
                    .Split(',')
                    .Select(_ => _.Trim().ToUpper())
                    .Where(_ => !string.IsNullOrEmpty(_))
                    .ToArray();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (_Methods.Any())
            {
                var httpMethod = filterContext.HttpContext.Request.HttpMethod.ToUpper();
                if (_Methods.Contains(httpMethod) == false)
                {
                    filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.MethodNotAllowed);
                }
            }
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            if (filterContext.HttpContext.Request.Headers.AllKeys.Contains("Origin"))
            {
                filterContext.HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", this._Origins);
            }
        }
    }
}