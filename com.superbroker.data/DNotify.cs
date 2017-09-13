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
    public class DNotify:DbCenter,IDB<Notify>
    {
        public bool Add(out int Id, Notify message)
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


        public bool Update(Notify message)
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

        public Notify Get(int id)
        {
            string sql = "select * from " + Notify.TABLENAME + " where id= " + id;
            Notify c = null;
            using (DataTable dt = helper.GetDataTable(sql))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    c = new Notify();
                    c.Id = r["Id"].ToInt();                   
                    c.Title = r["Title"].ToString();
                    c.Content = r["Content"].ToString();
                    c.Sender = r["Sender"].ToString();
                    c.Recver = r["Recver"].ToString();                   
                    c.AddOn = r["SendOn"].ToDateTime();
                }
            }
            return c;
        }

        public bool Delete(int id)
        {
            return helper.ExecuteSqlNoResult("delete from " + Notify.TABLENAME + " where id=" + id);
        }

        public List<Notify> Get(out int pageCount, string recver, int pageNum = 1)
        {
            List<Notify> list = new List<Notify>();
            pageCount = 1;
            string sql = "select count(id) from " + Notify.TABLENAME;
            string where = " where 1=1 ";
            if (recver != "") { where += " and (Recver='*' or Recver like '%"+ recver +"%')"; }
            int recordCount = Convert.ToInt32(helper.GetOne(sql + where));
            pageCount = pageCount / PAGE_SIZE + recordCount % PAGE_SIZE == 0 ? 0 : 1;
            sql = "select * from " + Notify.TABLENAME + where + " order by id desc limit " + (pageNum - 1) * PAGE_SIZE + "," + PAGE_SIZE;
            using (DataTable dt = helper.GetDataTable(sql))
            {
                foreach (DataRow r in dt.Rows)
                {
                    Notify c = new Notify();
                    c.Id = r["Id"].ToInt();
                    c.Title = r["Title"].ToString();
                    c.Content = r["Content"].ToString();
                    c.Sender = r["Sender"].ToString();
                    c.Recver = r["Recver"].ToString();
                    c.AddOn = r["AddOn"].ToDateTime();
                    list.Add(c);
                }
            }
            return list;
        }
    }
}
