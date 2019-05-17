using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABO.Web.Framework
{
    public class ErrorResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            new ViewResult { ViewName = "Error" }.ExecuteResult(context);
        }
    }
}