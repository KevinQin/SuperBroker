using System;
using System.Collections.Generic;
using System.Text;

namespace com.superbroker.model
{
    /// <summary>
    /// 系统日志
    /// </summary>
    public class SysLog:ModelBase
    {
        public new const String TABLENAME = "s_syslog";

        public string WorkNo { get; set; }
        public String Msg { get; set; }
        public String Params { get; set; }
        public String RawUrl { get; set; }
        public int Level { get; set; }
        public String Tag { get; set; }
    }

    /// <summary>
    /// 日志级别
    /// </summary>
    public enum Level
    {
        Info = 0, Debug = 1, Warning = 2, Error = 3
    }
}
