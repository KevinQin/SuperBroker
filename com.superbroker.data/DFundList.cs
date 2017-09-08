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
    public class DFundList : DbCenter, IDB<FundList>
    {
        public bool Add(out int Id, FundList t)
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

        public bool Update(FundList t)
        {            
            return false;
        }

        public List<FundList> Get(string ReportNo, string From, string To, string subject, int type, int pageno, DateTime begin, DateTime end, out int pageCount)
        {
            List<FundList> list = new List<FundList>();
            string sql = "select * from " + FundList.TABLENAME + " where 1=1 ";
            string _sql = "select count(id) from " + FundList.TABLENAME + " where 1=1 ";
            if (!string.IsNullOrEmpty(ReportNo)) { sql += " and (ReportNo like '%" + ReportNo + "%')"; }
            if (!string.IsNullOrEmpty(From)) { sql += " and (`From` like '%" + From + "%')"; }
            if (!string.IsNullOrEmpty(To)) { sql += " and (`To` like '%" + To + "%')"; }
            if (!string.IsNullOrEmpty(subject)) { sql += " and (`subject` like '%" + subject + "%')"; }
            if (type>=0) { sql += " and type=" + type ; }
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
                    FundList fund = new FundList()
                    {
                        ReportNo = r["ReportNo"].ToString(),
                        AddOn = r["AddOn"].ToDateTime(),
                        Price = r["Price"].ToInt(),
                        Type = r["Type"].ToInt16(),
                        Id = r["Id"].ToInt(),
                        Memo = r["Memo"].ToString(),
                        Subject = r["Subject"].ToString(),
                        From = r["From"].ToString(),
                        To = r["To"].ToString()
                    };
                    list.Add(fund);
                }
            }
            return list;
        }
    }
}
