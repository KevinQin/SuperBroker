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
    public class DBroker : DbCenter, IDB<Broker>
    {
        public bool Add(out int Id, Broker t)
        {
            SqlObject sql = new SqlObject(SqlObjectType.Insert, t, DB_TYPE);
            t.Password = PasswordEncode(t.Password);
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

        public bool Update(Broker t)
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

        public bool ChangeState(string BrokerNo, string WorkNo,string info, int state) {
            SqlObject sql = new SqlObject(SqlObjectType.Update, Broker.TABLENAME, DB_TYPE);
            sql.Where = " workno='" + BrokerNo + "'";
            sql.AddField("State", state, SqlFieldType.Int);
            sql.AddField("CheckOn", DateTime.Now, SqlFieldType.DateTime);
            sql.AddField("CheckWorkNo", WorkNo, SqlFieldType.String);
            sql.AddField("CheckInfo", info, SqlFieldType.String);
            return helper.ExecuteSqlNoResult(sql);
        }

        public List<Broker> Get(string workno, string name, string mobile, string area, int state, int pageno, DateTime begin, DateTime end,out int pageCount) {
            List<Broker> list = new List<Broker>();
            string sql = "select * from " + Broker.TABLENAME+" where 1=1 ";
            string _sql = "select count(id) from " + Broker.TABLENAME + " where 1=1 ";
            if (!string.IsNullOrEmpty(workno)) { sql += " and (workno like '%" + workno +"%')"; }
            if (!string.IsNullOrEmpty(name)) { sql += " and (name like '%" + name + "%')"; }
            if (!string.IsNullOrEmpty(mobile)) { sql += " and (name like '%" + mobile + "%' or tel like '%"+ mobile +"%')"; }
            if (!string.IsNullOrEmpty(area)) { sql += " and (area like '%" + area + "%')"; }
            if (state>=0) { sql += " and state=" + area ; }
            if (begin > DEF_DATE) { sql += " and addon>='"+ begin.Format() +"'"; }
            if (end > DEF_DATE) { sql += " and addon<='" + begin.Format() + "'"; }            
            int recordCount = helper.GetOne(_sql).ToInt();
            pageCount = pageno / PAGE_SIZE + (pageno % PAGE_SIZE == 0 ? 0 : 1);
            if (pageno <= 0) { pageno = 1; }
            if (pageno > pageCount) { pageno = pageCount; }
            sql += " limit "+ (pageno-1)*PAGE_SIZE +"," + PAGE_SIZE;
            using (DataTable dt = helper.GetDataTable(sql))
            {
                foreach (DataRow r in dt.Rows)
                {
                    Broker broker = new Broker()
                    {
                        AccountName = r["AccountName"].ToString(),
                        AddOn = r["AddOn"].ToDateTime(),
                        Address = r["Address"].ToString(),
                        Area = r["area"].ToString(),
                        AvatarMediaId = r["AvatarMediaId"].ToString(),
                        BankCardNo = r["BankCardNo"].ToString(),
                        BankInfo = r["BankInfo"].ToString(),
                        CheckInfo = r["CheckInfo"].ToString(),
                        CheckOn = r["CheckOn"].ToDateTime(),
                        CheckWorkNo = r["CheckWorkNo"].ToString(),
                        Company = r["Company"].ToString(),
                        FeePeer = r["FeePeer"].ToInt(),
                        Gender = r["Gender"].ToInt16(),
                        Id = r["Id"].ToInt(),
                        Memo = r["Memo"].ToString(),
                        Mobile = r["Mobile"].ToString(),
                        Name = r["Name"].ToString(),
                        OpenId = r["OpenId"].ToString(),
                        Password = r["Password"].ToString(),
                        State = r["State"].ToInt16(),
                        Tel = r["Tel"].ToString(),
                        Trade = r["Trade"].ToString(),
                        UnionId = r["UnionId"].ToString(),
                        UptimeOn = r["UptimeOn"].ToDateTime(),
                        WorkNo = r["WorkNo"].ToString()
                    };
                    list.Add(broker);
                }
            }
            return list;
        }

        public Broker Login(string openid, string account, string pwd,out int ErrCode) {
            Broker broker = Get(0,openid);
            ErrCode = 0;
            if (broker != null)
            {
                if (broker.Mobile == account || broker.WorkNo == account)
                {
                    if (broker.State == 1)
                    {

                        if (broker.Password == PasswordEncode(pwd))
                        {
                            ErrCode = 1;
                        }
                        else
                        {
                            ErrCode = 11;
                        }
                    }
                    else {
                        ErrCode = broker.State;
                    }
                }
                else
                {
                    ErrCode = 12;
                }
            }
            else {
                ErrCode = 13;
            }
            return broker;
        }

        public Broker Get(int id=0, string openid="", string workno=null) {
            Broker broker = null;
            string sql= "select* from "+ Broker.TABLENAME;
            if (id > 0) { sql += " where id=" + id; }
            if (!string.IsNullOrEmpty(workno)) { sql += " where workno='" + workno + "'"; }
            if (!string.IsNullOrEmpty(openid)) { sql += " where openid='" + openid + "'"; }
            using (DataTable dt = helper.GetDataTable(sql)) {
                if (dt.Rows.Count > 0) {
                    DataRow r = dt.Rows[0];
                    broker = new Broker() {
                        AccountName=r["AccountName"].ToString(),
                        AddOn= r["AddOn"].ToDateTime(),
                        Address=r["Address"].ToString(),
                        Area=r["area"].ToString(), 
                        AvatarMediaId=r["AvatarMediaId"].ToString(),
                        BankCardNo=r["BankCardNo"].ToString(),
                        BankInfo=r["BankInfo"].ToString(),
                        CheckInfo=r["CheckInfo"].ToString(),
                        CheckOn=r["CheckOn"].ToDateTime(),
                        CheckWorkNo=r["CheckWorkNo"].ToString(),
                        Company=r["Company"].ToString(),
                        FeePeer=r["FeePeer"].ToInt(),
                        Gender=r["Gender"].ToInt16(),
                        Id=r["Id"].ToInt(),
                        Memo=r["Memo"].ToString(), 
                        Mobile=r["Mobile"].ToString(),
                        Name=r["Name"].ToString(),
                        OpenId=r["OpenId"].ToString(),
                        Password=r["Password"].ToString(),
                        State=r["State"].ToInt16(),
                        Tel=r["Tel"].ToString(),
                        Trade=r["Trade"].ToString(),
                        UnionId=r["UnionId"].ToString(),
                        UptimeOn=r["UptimeOn"].ToDateTime(),
                        WorkNo=r["WorkNo"].ToString()
                    };
                }
            }
            return broker;
        }
    }
}
