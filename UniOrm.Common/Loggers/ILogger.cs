using System;
namespace UniOrm
{
    public interface ILogger
    {
        void Dispose();
        void LogDebug(string loggerName, string msg, params object[] args);
        void LogError(string loggerName, string msg, params object[] args);
        void LogFatal(string loggerName, string msg, params object[] args);
        void LogInfo(string loggerName, string msg, params object[] args);
        void LogWarn(string loggerName, string msg, params object[] args);
    }
}
