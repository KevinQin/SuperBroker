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
    public class DHelp:DbCenter,IDB<Help>
    {
        public bool Add(out int Id, Help message)
        {
            SqlObject sql = new SqlObject(SqlObjectType.Insert, message, DB_TYPE);
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


        public bool Update(Help message)
        {
            SqlObject sql = new SqlObject(SqlObjectType.Update, message, DB_TYPE);
            sql.Where = " id=" + message.Id;
            if (sql.AddAllField())
            {
                return helper.ExecuteSqlNoResult(sql);
            }
            else
            {
                return false;
            }
        }

        public Help Get(int id)
        {
            string sql = "select * from " + Help.TABLENAME + " where id= " + id;
            Help c = null;
            using (DataTable dt = helper.GetDataTable(sql))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    c = new Help();
                    c.Id = r["Id"].ToInt();
                    c.Title = r["Title"].ToString();
                    c.Content = r["Content"].ToString();
                    c.Sender = r["Sender"].ToString();
                    c.AddOn = r["SendOn"].ToDateTime();
                }
            }
            return c;
        }

        public bool Delete(int id)
        {
            return helper.ExecuteSqlNoResult("delete from " + Help.TABLENAME + " where id=" + id);
        }

        public List<Help> Get()
        {
            List<Help> list = new List<Help>();
            string sql = "select * from " + Help.TABLENAME + " order by id desc";
            using (DataTable dt = helper.GetDataTable(sql))
            {
                foreach (DataRow r in dt.Rows)
                {
                    Help c = new Help();
                    c.Id = r["Id"].ToInt();
                    c.Title = r["Title"].ToString();
                    c.Content = r["Content"].ToString();
                    c.Sender = r["Sender"].ToString();
                    c.AddOn = r["AddOn"].ToDateTime();
                    list.Add(c);
                }
            }
            return list;
        }
    }
}
