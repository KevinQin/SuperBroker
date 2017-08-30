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
    public class DBrokerLog : DbCenter, IDB<BrokerLog>
    {
        public bool Add(out int Id, BrokerLog t)
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

        public bool Update(BrokerLog t)
        {
            return false;
        }

        public List<BrokerLog> Get(string BrokerNo,string workno)
        {
            List<BrokerLog> list = new List<BrokerLog>();
            string sql = "select * from " + BrokerLog.TABLENAME + " where 1=1 ";            
            if (!string.IsNullOrEmpty(BrokerNo)) { sql += " and (BrokerNo like '%" + BrokerNo + "%')"; }
            if (!string.IsNullOrEmpty(workno)) { sql += " and (workno like '%" + workno + "%')"; }
            using (DataTable dt = helper.GetDataTable(sql))
            {
                foreach (DataRow r in dt.Rows)
                {
                    BrokerLog log = new BrokerLog()
                    {
                       
                        AddOn = r["AddOn"].ToDateTime(),
                        BrokerNo = r["BrokerNo"].ToString(),
                        WorkNo = r["WorkNo"].ToString(),
                        Info = r["Info"].ToString(),                      
                        Id = r["Id"].ToInt(),                       
                        State = r["State"].ToInt16()                        
                    };
                    list.Add(log);
                }
            }
            return list;
        }
    }
}
