using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UniOrm
{
    public class SyncWriteLogger : ILogger
    {
        #region ILogger Members

        public void Dispose()
        {

        }

        public void LogDebug(string loggerName, string msg, params object[] args)
        {
            Logger.LogDebug(loggerName, msg, args);
        }

        public void LogError(string loggerName, string msg, params object[] args)
        {
            Logger.LogError(loggerName, msg, args);
        }

        public void LogFatal(string loggerName, string msg, params object[] args)
        {
            Logger.LogFatal(loggerName, msg, args);
        }

        public void LogInfo(string loggerName, string msg, params object[] args)
        {
            Logger.LogInfo(loggerName, msg, args);
        }

        public void LogWarn(string loggerName, string msg, params object[] args)
        {
            Logger.LogWarn(loggerName, msg, args);
        }

        #endregion
    }
}
