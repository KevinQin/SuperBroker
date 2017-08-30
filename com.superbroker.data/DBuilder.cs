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
    public class DBuilder : DbCenter, IDB<Builder>
    {
        public bool Add(out int Id, Builder t)
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

        public bool Update(Builder t)
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

        public List<Builder> Get( out int pageCount,string BuilderNo, string name, string brand, string provider, string hottel, string province, string city,string district, string managerno, int pageno, DateTime begin, DateTime end, int pricelower=0, int priceupper=0)
        {
            List<Builder> list = new List<Builder>();
            string sql = "select * from " + Builder.TABLENAME + " where 1=1 ";
            string _sql = "select count(id) from " + Builder.TABLENAME + " where 1=1 ";
            if (!string.IsNullOrEmpty(BuilderNo)) { sql += " and (BuilderNo like '%" + BuilderNo + "%')"; }
            if (!string.IsNullOrEmpty(name)) { sql += " and (name like '%" + name + "%')"; }
            if (!string.IsNullOrEmpty(brand)) { sql += " and (brand like '%" + brand + "%')"; }
            if (!string.IsNullOrEmpty(hottel)) { sql += " and (hottel like '%" + hottel + "%')"; }
            if (!string.IsNullOrEmpty(provider)) { sql += " and (provider like '%" + provider + "%')"; }
            if (!string.IsNullOrEmpty(province)) { sql += " and (province like '%" + province + "%')"; }
            if (!string.IsNullOrEmpty(city)) { sql += " and (city like '%" + city + "%')"; }
            if (!string.IsNullOrEmpty(district)) { sql += " and (district like '%" + district + "%')"; }
            if (!string.IsNullOrEmpty(managerno)) { sql += " and (managerno like '%" + managerno + "%')"; }
            if (pricelower > 0) { sql += " and price>=" + pricelower; }
            if (priceupper > 0 && priceupper>=pricelower) { sql += " and price<=" + priceupper; }
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
                    Builder builder = new Builder()
                    {
                        BuilderNo = r["BuilderNo"].ToString(),
                        AddOn = r["AddOn"].ToDateTime(),
                        Name = r["Name"].ToString(),
                        Id = r["Id"].ToInt(),
                        Address = r["Address"].ToString(),
                        Area = r["Area"].ToInt(),
                        Brand = r["Brand"].ToString(),
                        Tel = r["Tel"].ToString(),
                        City = r["City"].ToString(),
                        Contact = r["Contact"].ToString(),
                        D3Url = r["D3Url"].ToString(),
                        Description = r["Description"].ToString(),
                        District = r["District"].ToString(),
                        FeePeer = r["FeePeer"].ToInt(),
                        FeeType = r["FeeType"].ToInt16(),
                        HotTel = r["HotTel"].ToString(),
                        Latitude = r["Latitude"].ToDouble(),
                        Longitude = r["Longitude"].ToDouble(),
                        ManagerNo = r["ManagerNo"].ToString(),
                        MapUrl = r["MapUrl"].ToString(),
                        OffOn = r["OffOn"].ToDateTime(),
                        Price = r["Price"].ToInt(),
                        Provider = r["Provider"].ToString(),
                        Province = r["Province"].ToString(),
                        Slogen = r["Slogen"].ToString(),
                        VRUrl = r["VRUrl"].ToString()
                    };
                    list.Add(builder);
                }
            }
            return list;
        }

        public Builder Get(int id, string BuilderNo = null)
        {
            Builder builder = null;
            string sql = "select* from " + Builder.TABLENAME;
            if (id > 0) { sql += " where id=" + id; }
            if (!string.IsNullOrEmpty(BuilderNo)) { sql += " where BuilderNo='" + BuilderNo + "'"; }
            using (DataTable dt = helper.GetDataTable(sql))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    builder = new Builder()
                    {
                        BuilderNo = r["BuilderNo"].ToString(),
                        AddOn = r["AddOn"].ToDateTime(),
                        Name = r["Name"].ToString(),
                        Id = r["Id"].ToInt(),
                        Address = r["Address"].ToString(),
                        Area = r["Area"].ToInt(),
                        Brand = r["Brand"].ToString(),
                        Tel = r["Tel"].ToString(),
                        City = r["City"].ToString(),
                        Contact = r["Contact"].ToString(),
                        D3Url = r["D3Url"].ToString(),
                        Description = r["Description"].ToString(),
                        District = r["District"].ToString(),
                        FeePeer = r["FeePeer"].ToInt(),
                        FeeType = r["FeeType"].ToInt16(),
                        HotTel = r["HotTel"].ToString(),
                        Latitude = r["Latitude"].ToDouble(),
                        Longitude = r["Longitude"].ToDouble(),
                        ManagerNo = r["ManagerNo"].ToString(),
                        MapUrl = r["MapUrl"].ToString(),
                        OffOn = r["OffOn"].ToDateTime(),
                        Price = r["Price"].ToInt(),
                        Provider = r["Provider"].ToString(),
                        Province = r["Province"].ToString(),
                        Slogen = r["Slogen"].ToString(),
                        VRUrl = r["VRUrl"].ToString()
                    };
                }
            }
            return builder;
        }

    }
}
