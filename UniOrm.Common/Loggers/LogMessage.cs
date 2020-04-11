using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm
{
    public class LogMessage
    {
        public LogLevelType Type { get; set; }
        public string LoggerName { get; set; }
        public string Msg { get; set; }
        public object[] Args { get; set; }



    }


}
