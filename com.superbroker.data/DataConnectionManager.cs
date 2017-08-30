using System;
using System.Collections.Generic;
using System.Text;
using com.seascape.db;
using com.seascape.tools;

namespace com.superbroker.data
{
    /// <summary>
    /// 数据链接管理器
    /// </summary>
    public class DataConnectionManager
    {
        static List<DbHelper> HelperList = new List<DbHelper>();

        public static DbHelper GetHelper()
        {
            DbHelper helper = null;
            if (HelperList.Count < 5)
            {
                helper = new MySqlHelper(BasicTool.GetConnectionstring("dbConn"));
                HelperList.Add(helper);
            }
            else
            {
                Random r = new Random(DateTime.Now.Millisecond);
                helper = HelperList[r.Next(0, 4)];
            }
            return helper;
        }
    }
}
