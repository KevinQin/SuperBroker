using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.superbroker.data;
using com.superbroker.model;

/// <summary>
/// Log 日志类
/// </summary>
public class Log
{

    public static void D(string info, HttpContext c, string Tag="")
    {        
         DSysLog.Dolog(Tag, info, Level.Debug, c);
    }

    public static void E(string info, HttpContext c, string Tag = "")
    {
        DSysLog.Dolog(Tag, info, Level.Error, c);
    }

    public static void I(string info, HttpContext c, string Tag = "")
    {
        DSysLog.Dolog(Tag, info, Level.Info, c);
    }

    public static void W(string info, HttpContext c, string Tag = "")
    {
        DSysLog.Dolog(Tag, info, Level.Warning, c);
    }
}