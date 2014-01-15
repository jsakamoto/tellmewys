using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace TellMeWYS
{
    public class LamdaResult : ActionResult
    {
        private Action<ControllerContext> _CallBack;

        public LamdaResult(Action<ControllerContext> callback)
        {
            _CallBack = callback;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            _CallBack(context);
        }
    }
}
