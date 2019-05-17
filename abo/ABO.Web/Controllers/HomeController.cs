using ABO.Core;
using ABO.Web.Framework;
using ABO.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABO.Web.Controllers
{
    public class HomeController : WebControllerBase
    {
        private const string HELP_FILE_NAME = "HDSD dành cho {0}.docx";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult DownloadHelp(string type)
        {
            string downloadedFileName = string.Format(HELP_FILE_NAME, type);
            string downloadedFilePath = Server.MapPath("~/App_Data/HelpFiles/" + downloadedFileName);
            
            if (!System.IO.File.Exists(downloadedFilePath))
                return PageNotFound();

            return File(downloadedFilePath, FileContentType.WORD, downloadedFileName);
        }
    }
}
