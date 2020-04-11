using log4net.Appender;
using log4net.Core;
using System.Diagnostics;

namespace UniOrm
{
    public class VsDebugAppender : DebugAppender
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            Debug.Write(RenderLoggingEvent(loggingEvent));
            if (!ImmediateFlush)
            {
                return;
            }
            Debug.Flush();
        }
    }
}
