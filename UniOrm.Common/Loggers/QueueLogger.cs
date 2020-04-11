using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniOrm
{
    public class QueueLogger : ILogger
    {
        private Queue<LogMessage> m_msgs;

        private Task m_task;

        public QueueLogger()
        {
            if (m_msgs == null)
            {
                m_signal = new AutoResetEvent(false);
                m_msgs = new Queue<LogMessage>();
                m_task = Task.Factory.StartNew(() => work());
            }
        }

        private AutoResetEvent m_signal;
        private bool m_Start = true;

        private void work()
        {
            while (m_Start)
            {
                m_signal.WaitOne();

                while (m_msgs.Count > 0)
                {
                    ExecueMsgs();
                    if (m_msgs.Count == 0)
                    {
                        m_signal.Reset();
                    }
                }
            }
            ExecueMsgs();
        }

        private void ExecueMsgs()
        {
            int msgCount = m_msgs.Count;
            for (int i = 0; i < msgCount; i++)
            {
                LogMessage msg = null;
                lock (m_msgs)
                {
                    msg = m_msgs.Dequeue();
                }
                if (msg != null)
                {
                    ExecLog(msg);
                }
            }
        }

        private void AddLog(LogMessage msg)
        {
            if (msg != null)
            {
                lock (m_msgs)
                {
                    m_msgs.Enqueue(msg);
                    m_signal.Set();
                }
            }
        }

        private void ExecLog(LogMessage msg)
        {
            switch (msg.Type)
            {
                case LogLevelType.Error:
                    Logger.LogError(msg.LoggerName, msg.Msg, msg.Args);
                    break;
                case LogLevelType.Debug:
                    Logger.LogDebug(msg.LoggerName, msg.Msg, msg.Args);
                    break;
                case LogLevelType.Fatal:
                    Logger.LogFatal(msg.LoggerName, msg.Msg, msg.Args);
                    break;
                case LogLevelType.Warn:
                    Logger.LogWarn(msg.LoggerName, msg.Msg, msg.Args);

                    break;
                case LogLevelType.Info:
                    Logger.LogInfo(msg.LoggerName, msg.Msg, msg.Args);
                    break;
            }

        }


        // 记录严重出错信息
        public void LogFatal(string loggerName, string msg, params object[] args)
        {
            AddLog(new LogMessage { Type = LogLevelType.Fatal, LoggerName = loggerName, Msg = msg, Args = args });
        }

        // 记录出错信息
        public void LogError(string loggerName, string msg, params object[] args)
        {
            AddLog(new LogMessage { Type = LogLevelType.Error, LoggerName = loggerName, Msg = msg, Args = args });
        }

        // 记录警告信息
        public void LogWarn(string loggerName, string msg, params object[] args)
        {
            AddLog(new LogMessage { Type = LogLevelType.Warn, LoggerName = loggerName, Msg = msg, Args = args });
        }

        // 记录普通信息
        public void LogInfo(string loggerName, string msg, params object[] args)
        {
            AddLog(new LogMessage { Type = LogLevelType.Info, LoggerName = loggerName, Msg = msg, Args = args });
        }

        // 记录调试信息
        public void LogDebug(string loggerName, string msg, params object[] args)
        {
            AddLog(new LogMessage { Type = LogLevelType.Debug, LoggerName = loggerName, Msg = msg, Args = args });
        }


        public void Dispose()
        {
            m_Start = false;
            m_task.Wait();
            if (m_task.IsCompleted)
            {
                m_task.Dispose();
            }
        }

    }
}
