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
    public class DRoom: DbCenter, IDB<Room>
    {
        public bool Add(out int Id, Room t)
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

        public bool Update(Room t)
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

        public List<Room> Get(out int pageCount, int pageno, string BuilderNo, string name, DateTime begin, DateTime end, int AreaUpper=0, int AreaLower=0, int PriceLower=0, int PriceUpper=0, int NumLower=0, int NumUpper=0)
        {
            List<Room> list = new List<Room>();
            string sql = "select * from " + Room.TABLENAME + " where 1=1 ";
            string _sql = "select count(id) from " + Room.TABLENAME + " where 1=1 ";
            if (!string.IsNullOrEmpty(BuilderNo)) { sql += " and (BuilderNo like '%" + BuilderNo + "%')"; }
            if (!string.IsNullOrEmpty(name)) { sql += " and (name like '%" + name + "%')"; }
            if (PriceLower > 0) { sql += " and price>=" + PriceLower; }
            if (PriceUpper > 0 && PriceUpper >= PriceLower) { sql += " and price<=" + PriceUpper; }
            if (AreaLower > 0) { sql += " and price>=" + AreaLower; }
            if (AreaUpper > 0 && AreaUpper >= AreaLower) { sql += " and area<=" + AreaUpper; }
            if (NumLower > 0) { sql += " and num>=" + NumLower; }
            if (NumUpper > 0 && NumUpper >= NumLower) { sql += " and num<=" + NumUpper; }
            if (begin > DEF_DATE) { sql += " and addon>='" + begin.Format() + "'"; }
            if (end > DEF_DATE) { sql += " and addon<='" + begin.Format() + "'"; }
            int recordCount = helper.GetOne(_sql).ToInt();
            pageCount = pageno / PAGE_SIZE + (recordCount % PAGE_SIZE == 0 ? 0 : 1);
            if (pageno <= 0) { pageno = 1; }
            if (pageno > pageCount) { pageno = pageCount; }
            sql += " limit " + (pageno - 1) * PAGE_SIZE + "," + PAGE_SIZE;
            using (DataTable dt = helper.GetDataTable(sql))
            {
                foreach (DataRow r in dt.Rows)
                {
                    Room room = new Room()
                    {
                        BuilderNo = r["BuilderNo"].ToString(),
                        AddOn = r["AddOn"].ToDateTime(),
                        Name = r["Name"].ToString(),
                        RoomNo = r["RoomNo"].ToString(),
                        Id = r["Id"].ToInt(),
                        Area = r["Area"].ToDouble(),
                        Price = r["Price"].ToInt16(),
                        ImgUrl = r["ImgUrl"].ToString(),
                        Description = r["Description"].ToString(),
                        VRUrl = r["VrUrl"].ToString(),
                        D3Url = r["D3Url"].ToString(),
                        Num = r["Num"].ToInt()
                    };
                    list.Add(room);
                }
            }
            return list;
        }

        public Room Get(int id, string RoomNo = null)
        {
            Room room = null;
            string sql = "select* from " + Room.TABLENAME;
            if (id > 0) { sql += " where id=" + id; }
            if (!string.IsNullOrEmpty(RoomNo)) { sql += " where RoomNo='" + RoomNo + "'"; }
            using (DataTable dt = helper.GetDataTable(sql))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    room = new Room()
                    {
                        BuilderNo = r["BuilderNo"].ToString(),
                        AddOn = r["AddOn"].ToDateTime(),
                        Name = r["Name"].ToString(),
                        RoomNo = r["RoomNo"].ToString(),
                        Id = r["Id"].ToInt(),
                        Area = r["Area"].ToDouble(),
                        Price = r["Price"].ToInt16(),
                        ImgUrl = r["ImgUrl"].ToString(),
                        Description = r["Description"].ToString(),
                        VRUrl = r["VrUrl"].ToString(),
                        D3Url = r["D3Url"].ToString(),
                        Num = r["Num"].ToInt()
                    };
                }
            }
            return room;
        }
    }
}
