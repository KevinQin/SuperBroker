using com.seascape.db;
using com.superbroker.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace com.superbroker.data
{
    public class DSysLog : DbCenter, IDB<SysLog>
    {

        public List<SysLog> GetLog()
        {
            List<SysLog> list = new List<SysLog>();
            using (DataTable dt = helper.GetDataTable("select id,workno,msg,params,rawurl,level,tag,addon from " + SysLog.TABLENAME + " order by id desc limit 0,200"))
            {
                foreach (DataRow r in dt.Rows)
                {
                    SysLog log = new SysLog()
                    {
                        AddOn = Convert.ToDateTime(r["addon"]),
                        Id = Convert.ToInt32(r["id"]),
                        Level = Convert.ToInt16(r["level"]),
                        Msg = r["msg"].ToString(),
                        Params = r["params"].ToString(),
                        RawUrl = r["RawUrl"].ToString(),
                        Tag = r["tag"].ToString(),
                        WorkNo=r["WorkNo"].ToString()
                    };

                    list.Add(log);
                }
            }

            return list;
        }

        public static void Dolog(string tag, string log, Level l, HttpContext c)
        {
            try
            {
                SysLog slog = new SysLog()
                {
                    AddOn = DateTime.Now,
                    WorkNo = "0000",
                    Msg = log,
                    Params = getParams(c),
                    RawUrl = c.Request.RawUrl,
                    Level = Convert.ToInt16(l),
                    Tag = tag
                };
                int logId = 0;
                new DSysLog().Add(out logId, slog);
            }
            catch (Exception e) { }
        }

        public bool Add(out int Id, SysLog log)
        {
            Id = 0;
            SqlObject sql = new SqlObject(SqlObjectType.Insert, log, DB_TYPE);
            if (sql.AddAllField())
            {
                Id = helper.InsertToDb(sql.ToString() + ";");
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string getParams(HttpContext c)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in c.Request.QueryString.AllKeys)
            {
                sb.Append(key + "=" + c.Request.QueryString[key] + "&");
            }

            foreach (string key in c.Request.Form.AllKeys)
            {
                sb.Append(key + "=" + c.Request.Form[key] + "&");
            }

            return sb.ToString();
        }


        public bool Update(SysLog t)
        {
            return false;
        }
    }
}
