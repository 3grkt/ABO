using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ABO.Web.Framework.ActionResults
{
    public class JsonErrorResult: JsonHttpStatusResult
    {
        public JsonErrorResult(string message, HttpStatusCode httpStatus):base(httpStatus)
        {
            Data = new {ErrorMessage = message};
        }
    }
}