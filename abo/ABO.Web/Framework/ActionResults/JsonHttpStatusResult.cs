using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ABO.Web.Framework.ActionResults
{
    public class JsonHttpStatusResult: JsonResult
    {
        private readonly HttpStatusCode _httpStatus;

        public JsonHttpStatusResult(object data, HttpStatusCode httpStatus = HttpStatusCode.OK)
        {
            Data = data;
            _httpStatus = httpStatus;
        }

        public JsonHttpStatusResult(HttpStatusCode httpStatus)
        {
            _httpStatus = httpStatus;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.RequestContext.HttpContext.Response.StatusCode = (int) _httpStatus;
            base.ExecuteResult(context);
        }
    }
}