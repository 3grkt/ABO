using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABO.Web.Framework
{
    public class ForbiddenResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            new ViewResult { ViewName = "ErrorForbidden" }.ExecuteResult(context);
        }
    }
}