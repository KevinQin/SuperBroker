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
    public class DReportLog : DbCenter, IDB<ReportLog>
    {
        public bool Add(out int Id, ReportLog t)
        {
            SqlObject sql = new SqlObject(SqlObjectType.Insert, t, DB_TYPE);
            Id = 0;
            if (sql.AddAllField())
            {
                Id = helper.InsertToDb(sql.ToString());
                if (Id > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool Update(ReportLog t)
        {
            SqlObject sql = new SqlObject(SqlObjectType.Update, t, DB_TYPE);
            sql.Where = " id=" + t.Id;
            if (sql.AddAllField())
            {
                return helper.ExecuteSqlNoResult(sql);
            }
            else
            {
                return false;
            }
        }

        public List<ReportLog> Get(string reportno)
        {
            List<ReportLog> list = new List<ReportLog>();
            string sql = "select * from " + ReportLog.TABLENAME + " where 1=1 ";            
            sql += " and reportno='" + reportno + "')";            
            using (DataTable dt = helper.GetDataTable(sql))
            {
                foreach (DataRow r in dt.Rows)
                {
                    ReportLog log = new ReportLog()
                    {
                        ReportNo = r["ReportNo"].ToString(),
                        AddOn = r["AddOn"].ToDateTime(),
                        WorkNo = r["WorkNo"].ToString(),
                        Id = r["Id"].ToInt(),
                        Memo = r["Memo"].ToString(),
                        State = r["Mobile"].ToInt16()                      
                    };
                    list.Add(log);
                }
            }
            return list;
        }
    }
}
