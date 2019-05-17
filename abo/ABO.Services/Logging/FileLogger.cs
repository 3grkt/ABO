using ABO.Core;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ABO.Services.Logging
{
    public class FileLogger : ILogger
    {
        //private static readonly ILog _log = LogManager.GetLogger("AOBLogger");

        private static Lazy<ILog> _lazyObject = new Lazy<ILog>(() =>
        {
            //log4net.Config.XmlConfigurator.Configure(new FileInfo(HttpContext.Current.Server.MapPath("~/web.config")));
            log4net.Config.XmlConfigurator.Configure();
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": log4net config was loaded.");

            return LogManager.GetLogger("ABOLogger");
        });

        protected static ILog Logger
        {
            get { return _lazyObject.Value; }
        }

        public void Debug(string message)
        {
            WriteLog(message, LogLevel.Debug);
        }

        public void Error(string message, Exception ex)
        {
            WriteLog(message, ex);
        }

        public void Info(string message)
        {
            WriteLog(message, LogLevel.Info);
        }

        public void Warn(string message)
        {
            WriteLog(message, LogLevel.Warning);
        }

        public void WriteLog(string message, Exception ex = null, object data = null)
        {
            if (ex != null)
                WriteLog(message, LogLevel.Error, ex, data);
            else
                WriteLog(message, LogLevel.Debug, ex, data);
        }

        public void WriteLog(string message, LogLevel level, Exception ex = null, object data = null)
        {
            // TODO: extend log capabitility (e.g. logLevel, logType)
            // TODO: add working user
            switch (level)
            {
                case LogLevel.Debug:
                    Logger.Debug(message);
                    break;
                case LogLevel.Info:
                    Logger.Info(message);
                    break;
                case LogLevel.Warning:
                    Logger.Warn(message);
                    break;
                case LogLevel.Error:
                    Logger.Error(message, ex);
                    break;
            }
        }

    }
}
