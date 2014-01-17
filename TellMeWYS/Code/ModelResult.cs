using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace TellMeWYS
{
    public class ModelResult<T> : ActionResult
    {
        public T Model { get; protected set; }

        public ModelResult(T model)
        {
            this.Model = model;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            throw new NotImplementedException();
        }
    }
}
