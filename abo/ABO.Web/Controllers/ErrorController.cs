using ABO.Web.Framework;
using ABO.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABO.Web.Controllers
{
    public class ErrorController : WebControllerBase
    {
        public ActionResult Index()
        {
            return PageError();
        }

        public ActionResult NotFound()
        {
            return PageNotFound();
        }

        public ActionResult Forbidden()
        {
            return PageForbidden();
        }
    }
}
