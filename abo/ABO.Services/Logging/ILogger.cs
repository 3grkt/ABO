
using ABO.Core;
using System;
namespace ABO.Services.Logging
{
    public interface ILogger
    {
        void WriteLog(string message, LogLevel level, Exception ex = null, object data = null);
        void WriteLog(string message, Exception ex = null, object data = null);
        void Error(string message, Exception ex = null);
        void Info(string message);
        void Warn(string message);
        void Debug(string message);
    }
}
