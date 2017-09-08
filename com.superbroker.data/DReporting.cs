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
    public class DReporting  : DbCenter, IDB<Reporting>
    {
        public bool Add(out int Id, Reporting t)
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

        public bool Update(Reporting t)
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

        public List<ReportingSimpleView> Get(string BrokerNo, int state, int pageno,  out int pageCount) {
            List<ReportingSimpleView> list = new List<ReportingSimpleView>();
            string _sql = "select count(id) from " + Reporting.TABLENAME + " where brokerno='" + BrokerNo + "'";
            string sql= "select ReportNo,Fee,TotalPrice,r.AddOn,ReportOn,ProtectedOn,ArriveOn,DisableOn,PayFeeOn,State,c.Name,c.Mobile,c.Gender,(select Name from "+ Builder.TABLENAME +" where BuilderNo = r.BuilderNo) as BuilderName  from "+ Reporting.TABLENAME +" as r,"+ Custom.TABLENAME +" as c where r.CustomNo = c.CustomNo and BrokerNo = '"+ BrokerNo +"'";
            if (state >= 0) {
                _sql += " and state = " + state;
                sql += " and state = " + state;
            }
            string orderby = "addon";
            if (state == 0 || state == 1 || state == 2)
            {
                orderby = "ReportOn";
            }
            else if (state == 3 || state == 4 || state == 5 || state == 6) {
                orderby = "ArriveOn";
            }
            else if (state ==9)
            {
                orderby = "PayFeeOn";
            }
            sql += " order by "+ orderby +" desc";
            int recordCount = helper.GetOne(_sql).ToInt();
            pageCount = pageno / PAGE_SIZE + (recordCount % PAGE_SIZE == 0 ? 0 : 1);           
            if (pageno > pageCount) { pageno = pageCount; }
            if (pageno <= 0) { pageno = 1; }
            sql += " limit " + (pageno - 1) * PAGE_SIZE + "," + PAGE_SIZE;
            using (DataTable dt = helper.GetDataTable(sql))
            {
                foreach (DataRow r in dt.Rows)
                {
                    ReportingSimpleView reporting = new ReportingSimpleView()
                    {                       
                        ReportNo = r["ReportNo"].ToString(),                       
                        Fee = r["Fee"].ToInt(),                       
                        TotalPrice = r["TotalPrice"].ToInt(),
                        ArriveOn = r["ArriveOn"].ToDateTime(),                       
                        DisableOn = r["DisableOn"].ToDateTime(),                        
                        PayFeeOn = r["PayFeeOn"].ToDateTime(),
                        ProtectedOn = r["ProtectedOn"].ToDateTime(),
                        ReportOn = r["ReportOn"].ToDateTime(),                       
                        State = r["State"].ToInt16(),
                        BuilderName=r["BuilderName"].ToString(),
                        CustomMobile=r["Mobile"].ToString(),
                        CustomName=r["Name"].ToString(),
                        Gender=r["Gender"].ToInt16()
                    };
                    list.Add(reporting);
                }
            }
            return list;
        }

        public List<Reporting> Get(int pageno, out int pageCount,string CustomName, string CustomMobile, string BuilderName, string BrokerNo, string ReportNo, DateTime begin, DateTime end, int state=99)
        {
            List<Reporting> list = new List<Reporting>();
            string sql = "select * from " + Reporting.TABLENAME + " where 1=1 ";
            string _sql = "select count(id) from " + Reporting.TABLENAME + " where 1=1 ";
            if (!string.IsNullOrEmpty(CustomName) || !string.IsNullOrEmpty(CustomMobile)) { sql += " and CustomNo in ( select CustomNo from "+ Custom.TABLENAME + " where (tel like '%" + CustomMobile + "%' or mobile like '%" + CustomMobile +"%' or  name like '%" + CustomName + "%'))"; }
            if (!string.IsNullOrEmpty(BuilderName)) { sql += " and BuilderNo in ( select BuilderNo from "+ Builder.TABLENAME +" where name like '%" + BuilderName + "%')"; }
            if (!string.IsNullOrEmpty(BrokerNo)) { sql += " and BrokerNo in ( select BrokerNo from " + Broker.TABLENAME + " where name like '%" + BuilderName + "%')"; }
            if (!string.IsNullOrEmpty(ReportNo)) { sql += " and ReportNo like '%" + ReportNo + "%'"; }
            if (state >= 0) {
                if (state == 99)
                {
                    sql += " and (state=1 or state=3 or state=4 or state=5 or state=7 or state=8 or state=9 or state=11) ";
                }
                else
                {
                    sql += " and state=" + state;
                    if (state == 0 || state == 1)
                    {//备案失败+成功
                        if (begin > DEF_DATE) { sql += " and ReportOn>='" + begin.Format() + "'"; }
                        if (end > DEF_DATE) { sql += " and ReportOn<='" + begin.Format() + "'"; }
                    }
                    else if (state == 2)
                    {
                        //已过备案保护期
                        if (begin > DEF_DATE) { sql += " and ProtectedOn>='" + begin.Format() + "'"; }
                        if (end > DEF_DATE) { sql += " and ProtectedOn<='" + begin.Format() + "'"; }
                    }
                    else if (state == 3 || state == 5)
                    {
                        //到案场 + 到案场未预订，需要跟进
                        if (begin > DEF_DATE) { sql += " and ArriveOn>='" + begin.Format() + "'"; }
                        if (end > DEF_DATE) { sql += " and ArriveOn<='" + begin.Format() + "'"; }
                    }
                    else if (state == 4)
                    {
                        //预订
                        if (begin > DEF_DATE) { sql += " and DownPaymentOn>='" + begin.Format() + "'"; }
                        if (end > DEF_DATE) { sql += " and DownPaymentOn<='" + begin.Format() + "'"; }
                    }
                    else if (state == 6)
                    {
                        //到场未预订,已过保护期
                        if (begin > DEF_DATE) { sql += " and DisableOn>='" + begin.Format() + "'"; }
                        if (end > DEF_DATE) { sql += " and DisableOn<='" + begin.Format() + "'"; }
                    }
                    else if (state == 7)
                    {
                        //到签约/首付
                        if (begin > DEF_DATE) { sql += " and SignedOn>='" + begin.Format() + "'"; }
                        if (end > DEF_DATE) { sql += " and SignedOn<='" + begin.Format() + "'"; }
                    }
                    else if (state == 8)
                    {
                        //佣金已到平台，未发放给经纪人
                        if (begin > DEF_DATE) { sql += " and DebitedOn>='" + begin.Format() + "'"; }
                        if (end > DEF_DATE) { sql += " and DebitedOn<='" + begin.Format() + "'"; }
                    }
                    else if (state == 9)
                    {
                        //佣金已发放
                        if (begin > DEF_DATE) { sql += " and PayFeeOn>='" + begin.Format() + "'"; }
                        if (end > DEF_DATE) { sql += " and PayFeeOn<='" + begin.Format() + "'"; }
                    }
                    else if (state == 10)
                    {
                        //用户退房
                        if (begin > DEF_DATE) { sql += " and FailOn>='" + begin.Format() + "'"; }
                        if (end > DEF_DATE) { sql += " and FailOn<='" + begin.Format() + "'"; }
                    }
                    else if (state == 11)
                    {
                        //退还佣金到平台
                        if (begin > DEF_DATE) { sql += " and ReturnFeeOn>='" + begin.Format() + "'"; }
                        if (end > DEF_DATE) { sql += " and ReturnFeeOn<='" + begin.Format() + "'"; }
                    }
                    else if (state == 12)
                    {
                        //退还佣金至开发商
                        if (begin > DEF_DATE) { sql += " and BackFeeOn>='" + begin.Format() + "'"; }
                        if (end > DEF_DATE) { sql += " and BackFeeOn<='" + begin.Format() + "'"; }
                    }
                }
            }            
            
            int recordCount = helper.GetOne(_sql).ToInt();
            pageCount = pageno / PAGE_SIZE + (recordCount % PAGE_SIZE == 0 ? 0 : 1);
            if (pageno <= 0) { pageno = 1; }
            if (pageno > pageCount) { pageno = pageCount; }
            sql += " limit " + (pageno - 1) * PAGE_SIZE + "," + PAGE_SIZE;
            using (DataTable dt = helper.GetDataTable(sql))
            {
                foreach (DataRow r in dt.Rows)
                {
                    Reporting reporting = new Reporting()
                    {
                        CustomNo = r["CustomNo"].ToString(),
                        AddOn = r["AddOn"].ToDateTime(),
                        ReportNo = r["ReportNo"].ToString(),
                        BrokerNo = r["BrokerNo"].ToString(),
                        Id = r["Id"].ToInt(),
                        BuilderNo = r["BuilderNo"].ToString(),
                        RoomNo = r["RoomNo"].ToString(),
                        HouseNo = r["HouseNo"].ToString(),
                        Fee = r["Fee"].ToInt(),
                         FeeType=r["FeeType"].ToInt16(),
                        TotalPrice = r["TotalPrice"].ToInt(),
                        ArriveOn = r["ArriveOn"].ToDateTime(),
                        BackFeeOn = r["BackFeeOn"].ToDateTime(),
                        DebitedOn = r["DebitedOn"].ToDateTime(),
                        DisableOn = r["DisableOn"].ToDateTime(),
                        DownPaymentOn = r["DownPaymentOn"].ToDateTime(),
                        PayFeeOn = r["PayFeeOn"].ToDateTime(),
                        ProtectedOn = r["ProtectedOn"].ToDateTime(),
                        ReportOn = r["ReportOn"].ToDateTime(),
                        FailOn=r["FailOn"].ToDateTime(),
                        ReturnFeeOn = r["ReturnFeeOn"].ToDateTime(),
                        SignedOn = r["SignedOn"].ToDateTime(),
                        State = r["State"].ToInt16()
                    };
                    list.Add(reporting);
                }
            }
            return list;
        }

        public Reporting Get(int id, string ReportNo = null)
        {
            Reporting report = null;
            string sql = "select* from " + Reporting.TABLENAME;
            if (id > 0) { sql += " where id=" + id; }
            if (!string.IsNullOrEmpty(ReportNo)) { sql += " where ReportNo='" + ReportNo + "'"; }
            using (DataTable dt = helper.GetDataTable(sql))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    report = new Reporting()
                    {
                        CustomNo = r["CustomNo"].ToString(),
                        AddOn = r["AddOn"].ToDateTime(),
                        ReportNo = r["ReportNo"].ToString(),
                        BrokerNo = r["BrokerNo"].ToString(),
                        Id = r["Id"].ToInt(),
                        BuilderNo = r["BuilderNo"].ToString(),
                        RoomNo = r["RoomNo"].ToString(),
                        HouseNo = r["HouseNo"].ToString(),
                        Fee = r["Fee"].ToInt(),
                        FeeType = r["FeeType"].ToInt16(),
                        TotalPrice = r["TotalPrice"].ToInt(),
                        ArriveOn = r["ArriveOn"].ToDateTime(),
                        BackFeeOn = r["BackFeeOn"].ToDateTime(),
                        DebitedOn = r["DebitedOn"].ToDateTime(),
                        DisableOn = r["DisableOn"].ToDateTime(),
                        DownPaymentOn = r["DownPaymentOn"].ToDateTime(),
                        PayFeeOn = r["PayFeeOn"].ToDateTime(),
                        ProtectedOn = r["ProtectedOn"].ToDateTime(),
                        ReportOn = r["ReportOn"].ToDateTime(),
                        FailOn = r["FailOn"].ToDateTime(),
                        ReturnFeeOn = r["ReturnFeeOn"].ToDateTime(),
                        SignedOn = r["SignedOn"].ToDateTime(),
                        State = r["State"].ToInt16()
                    };
                }
            }
            return report;
        }
    }
}
