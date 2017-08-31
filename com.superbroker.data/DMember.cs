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
    public class DMember : DbCenter, IDB<Member>
    {
        public bool Add(out int Id, Member t)
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

        public bool Update(Member t)
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

        public Member Get(string openid)
        {
            string sql = "select * from " + Member.TABLENAME + " where openid='" + openid + "'";
            Member c = null;
            using (DataTable dt = helper.GetDataTable(sql))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    c = new Member();
                    c.Id = r["Id"].ToInt();
                    c.Name = r["Name"].ToString();
                    c.Mobile = r["Mobile"].ToString();
                    c.AddOn = r["AddOn"].ToDateTime();
                    c.Province = r["Province"].ToString();
                    c.Country = r["Country"].ToString();
                    c.City = r["city"].ToString();
                    c.Source = r["Source"].ToString();
                    c.QrCode = r["QrCode"].ToInt();
                    c.IsFllow = r["IsFllow"].ToInt16() == 1;
                    c.Gender = r["Gender"].ToInt16();
                    c.NickName = r["NickName"].ToString();
                    c.OpenId = r["OpenId"].ToString();
                    c.UnionId = r["UnionId"].ToString();
                    c.PhotoUrl = r["PhotoUrl"].ToString();
                    c.Memo = r["Memo"].ToString();
                }
            }
            return c;
        }

        public Member Get(int id)
        {
            string sql = "select * from " + Member.TABLENAME + " where id= " + id;
            Member c = null;
            using (DataTable dt = helper.GetDataTable(sql))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    c = new Member();
                    c.Id = r["Id"].ToInt();
                    c.Name = r["Name"].ToString();
                    c.Mobile = r["Mobile"].ToString();
                    c.AddOn = r["AddOn"].ToDateTime();
                    c.Province = r["Province"].ToString();
                    c.Country = r["Country"].ToString();
                    c.Source = r["Source"].ToString();
                    c.QrCode = r["QrCode"].ToInt();
                    c.IsFllow = r["IsFllow"].ToInt16() == 1;
                    c.City = r["city"].ToString();
                    c.Gender = r["Gender"].ToInt16();                  
                    c.NickName = r["NickName"].ToString();
                    c.OpenId = r["OpenId"].ToString();
                    c.UnionId = r["UnionId"].ToString();
                    c.PhotoUrl = r["PhotoUrl"].ToString();
                    c.Memo = r["Memo"].ToString();
                }
            }
            return c;
        }

        public bool Delete(int id)
        {
            return helper.ExecuteSqlNoResult("delete from " + Member.TABLENAME + " where id=" + id);
        }
        
        public List<Member> Get(out int pageCount, string name = "", string mobile = "", int aid = -1, int pageNum = 1)
        {
            List<Member> list = new List<Member>();
            pageCount = 1;
            string sql = "select count(id) from " + Member.TABLENAME;
            string where = " where 1=1 ";
            if (!string.IsNullOrEmpty(name)) { where += " and name like '%" + name + "%' "; }
            if (!string.IsNullOrEmpty(mobile)) { where += " and mobile like '%" + mobile + "%' "; }
            if (aid >= 0) { where += " and aid=" + aid; }
            int recordCount = Convert.ToInt32(helper.GetOne(sql + where));
            pageCount = pageCount / PAGE_SIZE + pageCount % PAGE_SIZE == 0 ? 0 : 1;

            sql = "select * from " +Member.TABLENAME + where + " order by id desc limit " + (pageNum - 1) * PAGE_SIZE + "," + PAGE_SIZE;
            using (DataTable dt = helper.GetDataTable(sql))
            {
                foreach (DataRow r in dt.Rows)
                {
                    Member c = new Member();
                    c.Id = r["Id"].ToInt();
                    c.Name = r["Name"].ToString();
                    c.Mobile = r["Mobile"].ToString();
                    c.AddOn = r["AddOn"].ToDateTime();
                    c.Province = r["Province"].ToString();
                    c.Source = r["Source"].ToString();
                    c.QrCode = r["QrCode"].ToInt();
                    c.IsFllow = r["IsFllow"].ToInt16() == 1;
                    c.Country = r["Country"].ToString();
                    c.City = r["city"].ToString();
                    c.Gender = r["Gender"].ToInt16();
                    c.NickName = r["NickName"].ToString();
                    c.OpenId = r["OpenId"].ToString();
                    c.UnionId = r["UnionId"].ToString();
                    c.PhotoUrl = r["PhotoUrl"].ToString();
                    c.Memo = r["Memo"].ToString();
                    list.Add(c);
                }
            }
            return list;
        }

    }
}
