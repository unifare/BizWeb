/*
 * ************
 * file:	    Logger.cs
 * creator:	    Cai Huan(huan.cai@philips.com)
 * date:	    2014-03
 * description:	Implement to log leveled runtime information
 * ************
 */

using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using log4net.Repository;

namespace UniOrm
{
    public static class Logger
    {
        private static ILoggerRepository loggerRepository;

        public static ILoggerRepository LoggerRepository { get; private set; }
        public static ILog Log { get; private set; } 
        static Logger()
        {
            LoggerRepository = CreateLoggerRepository();
            LoadLog4NetConfig();

            //log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo("Logger.conf"));
        }
        /// <summary>
        /// 创建日志仓储实例
        /// </summary>
        /// <returns></returns>
        private static ILoggerRepository CreateLoggerRepository()
        {
            loggerRepository = loggerRepository ?? LogManager.CreateRepository("GlobalExceptionHandler"); // 单例
            return loggerRepository;
        }

        /// <summary>
        /// 加载log4net配置
        /// </summary>
        private static void LoadLog4NetConfig()
        {
            // 配置log4net
            log4net.Config.XmlConfigurator.Configure(loggerRepository, new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "/Logger.conf"));

            //// 创建log实例
            //Log = LogManager.GetLogger(loggerRepository.Name, AppDomain.CurrentDomain.FriendlyName);

            //Log.Info("已加载日志配置");
        }
         
        // 记录严重出错信息
        public static void LogFatal(string loggerName, string msg, params object[] args)
        {
            ILog logger = GetLogger(loggerName);
            if (logger != null)
            {
                logger.FatalFormat(msg, args);
            }
        }

        // 记录出错信息
        public static void LogError(string loggerName, string msg, params object[] args)
        {
            ILog logger = GetLogger(loggerName);
            if (logger != null)
            {
                logger.ErrorFormat(msg, args);
            }
        }

        public static void LogError(string loggerName, string funtionName, Exception ex)
        {
            ILog logger = GetLogger(loggerName);
            if (logger != null)
            {
                var exContent = LoggerHelper.GetExceptionString(ex);
                var msg = string.Format("{0} -> #Exception# {1}", funtionName, exContent);
                logger.ErrorFormat(msg, null);
            }
        }

        // 记录警告信息
        public static void LogWarn(string loggerName, string msg, params object[] args)
        {
            ILog logger = GetLogger(loggerName);
            if (logger != null)
            {
                logger.WarnFormat(msg, args);
            }
        }

        // 记录普通信息
        public static void LogInfo(string loggerName, string msg, params object[] args)
        {
            ILog logger = GetLogger(loggerName);
            if (logger != null)
            {
                logger.InfoFormat(msg, args);
            }
        }

        // 记录调试信息
        public static void LogDebug(string loggerName, string msg, params object[] args)
        {
            ILog logger = GetLogger(loggerName);
            if (logger != null)
            {
                logger.DebugFormat(msg, args);
            }
        }

        #region Members with condition judging

        // 记录严重出错信息
        public static void LogFatalIf(bool condition, string loggerName, string msg, params object[] args)
        {
            if (condition)
            {
                LogFatal(loggerName, msg, args);
            }
        }

        // 记录出错信息
        public static void LogErrorIf(bool condition, string loggerName, string msg, params object[] args)
        {
            if (condition)
            {
                LogError(loggerName, msg, args);
            }
        }

        // 记录警告信息
        public static void LogWarnIf(bool condition, string loggerName, string msg, params object[] args)
        {
            if (condition)
            {
                LogWarn(loggerName, msg, args);
            }
        }

        // 记录普通信息
        public static void LogInfoIf(bool condition, string loggerName, string msg, params object[] args)
        {
            if (condition)
            {
                LogInfo(loggerName, msg, args);
            }
        }

        // 记录调试信息
        public static void LogDebugIf(bool condition, string loggerName, string msg, params object[] args)
        {
            if (condition)
            {
                LogDebug(loggerName, msg, args);
            }
        }

        #endregion Members with condition judging

        public static  void  ShowdownLogger()
        {
            if( s_loggers!=null)
            {
                foreach(var logname in s_loggers.Keys)
                {
                    s_loggers[logname].Logger.Repository.Shutdown();
                }
            }
        }

        private static ILog GetLogger(string loggerName)
        {
            ILog logger = null;
            if (s_loggers != null)
            {
                lock (s_loggers)
                {
                    if (!s_loggers.ContainsKey(loggerName))
                    {
                        logger = LogManager.GetLogger(loggerRepository.Name, loggerName);
                        s_loggers.Add(loggerName, logger);
                    }
                    else
                    {
                        logger = s_loggers[loggerName];
                    }
                }
            }
            return logger;
        }

        private static readonly Dictionary<string, ILog> s_loggers = new Dictionary<string, ILog>();
    }
}
