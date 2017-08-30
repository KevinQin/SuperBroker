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
    public class DCustom : DbCenter, IDB<Custom>
    {
        public bool Add(out int Id, Custom t)
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

        public bool Update(Custom t)
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

        public List<Custom> Get(string CustomNo, string name, string mobile, int pageno, DateTime begin, DateTime end, out int pageCount)
        {
            List<Custom> list = new List<Custom>();
            string sql = "select * from " + Custom.TABLENAME + " where 1=1 ";
            string _sql = "select count(id) from " + Custom.TABLENAME + " where 1=1 ";
            if (!string.IsNullOrEmpty(CustomNo)) { sql += " and (CustomNo like '%" + CustomNo + "%')"; }
            if (!string.IsNullOrEmpty(name)) { sql += " and (name like '%" + name + "%')"; }
            if (!string.IsNullOrEmpty(mobile)) { sql += " and (name like '%" + mobile + "%' or tel like '%" + mobile + "%')"; }
            if (begin > DEF_DATE) { sql += " and addon>='" + begin.Format() + "'"; }
            if (end > DEF_DATE) { sql += " and addon<='" + begin.Format() + "'"; }
            int recordCount = helper.GetOne(_sql).ToInt();
            pageCount = pageno / PAGE_SIZE + (pageno % PAGE_SIZE == 0 ? 0 : 1);
            if (pageno <= 0) { pageno = 1; }
            if (pageno > pageCount) { pageno = pageCount; }
            sql += " limit " + (pageno - 1) * PAGE_SIZE + "," + PAGE_SIZE;
            using (DataTable dt = helper.GetDataTable(sql))
            {
                foreach (DataRow r in dt.Rows)
                {
                    Custom custom = new Custom()
                    {
                        CustomNo = r["CustomNo"].ToString(),
                        AddOn = r["AddOn"].ToDateTime(),
                        Name = r["Name"].ToString(),                       
                        Gender = r["Gender"].ToInt16(),
                        Id = r["Id"].ToInt(),
                        Memo = r["Memo"].ToString(),
                        Mobile = r["Mobile"].ToString(),
                        OpenId = r["OpenId"].ToString(),
                        Tel = r["Tel"].ToString(),
                        UnionId = r["UnionId"].ToString()
                    };
                    list.Add(custom);
                }
            }
            return list;
        }

        public Custom Get(int id, string CustomNo = null)
        {
            Custom custom = null;
            string sql = "select* from " + Custom.TABLENAME;
            if (id > 0) { sql += " where id=" + id; }
            if (!string.IsNullOrEmpty(CustomNo)) { sql += " where CustomNo='" + CustomNo + "'"; }
            using (DataTable dt = helper.GetDataTable(sql))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    custom = new Custom()
                    {
                        CustomNo = r["CustomNo"].ToString(),
                        AddOn = r["AddOn"].ToDateTime(),
                        Name = r["Name"].ToString(),
                        Gender = r["Gender"].ToInt16(),
                        Id = r["Id"].ToInt(),
                        Memo = r["Memo"].ToString(),
                        Mobile = r["Mobile"].ToString(),
                        OpenId = r["OpenId"].ToString(),
                        Tel = r["Tel"].ToString(),
                        UnionId = r["UnionId"].ToString()
                    };
                }
            }
            return custom;
        }
    }
}
