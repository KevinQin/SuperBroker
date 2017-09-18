using com.seascape.db;
using com.superbroker.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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
            sql.AddField("AccountName", t.AccountName, SqlFieldType.String);
            sql.AddField("Address", t.Address, SqlFieldType.String);
            sql.AddField("Area", t.Area, SqlFieldType.String);
            sql.AddField("BankCardNo", t.BankCardNo, SqlFieldType.String);
            sql.AddField("BankInfo", t.BankInfo, SqlFieldType.String);
            sql.AddField("CheckInfo", t.CheckInfo, SqlFieldType.String);
            sql.AddField("CheckOn", t.CheckOn, SqlFieldType.DateTime);
            sql.AddField("CheckWorkNo", t.CheckWorkNo, SqlFieldType.String);
            sql.AddField("Company", t.Company, SqlFieldType.String);
            sql.AddField("FeePeer", t.FeePeer, SqlFieldType.Int);
            sql.AddField("Gender", t.Gender, SqlFieldType.Int);
            sql.AddField("Memo", t.Memo, SqlFieldType.String);
            sql.AddField("Mobile", t.Mobile, SqlFieldType.String);
            sql.AddField("Name", t.Name, SqlFieldType.String);
            sql.AddField("OpenId", t.OpenId, SqlFieldType.String);
            sql.AddField("State", t.State, SqlFieldType.Int);
            sql.AddField("Tel", t.Tel, SqlFieldType.String);
            sql.AddField("Trade", t.Trade, SqlFieldType.String);
            sql.AddField("UnionId", t.UnionId, SqlFieldType.String);
            sql.AddField("UptimeOn", t.UptimeOn, SqlFieldType.DateTime);
            sql.AddField("WorkNo", t.WorkNo, SqlFieldType.String);
            if (t.Password != "")
            {
                sql.AddField("Password", PasswordEncode(t.Password), SqlFieldType.String);
            }            
            return helper.ExecuteSqlNoResult(sql);           
        }

        public bool ChangeState(string BrokerNo,int Id, string WorkNo,string info, BrokerState state) {
            SqlObject sql = new SqlObject(SqlObjectType.Update, Broker.TABLENAME, DB_TYPE);
            sql.Where = " Id="+ Id +" and workno='" + BrokerNo + "'";
            sql.AddField("State", Convert.ToInt16(state), SqlFieldType.Int);
            sql.AddField("CheckOn", DateTime.Now, SqlFieldType.DateTime);
            sql.AddField("CheckWorkNo", WorkNo, SqlFieldType.String);
            sql.AddField("CheckInfo", info, SqlFieldType.String);
            return helper.ExecuteSqlNoResult(sql);
        }

        public int GetWorkNo() {
            object obj = helper.GetOne("select workno from " + Broker.TABLENAME + " order by workno desc limit 0,1");
            int result = 60001;
            if (obj != null) {
                result = Convert.ToInt32(obj) + 1;
            }
            return result;
        }

        public List<Broker> Get(string workno, string name, string mobile, string area, BrokerState state, int pageno, DateTime begin, DateTime end,out int pageCount) {
            List<Broker> list = new List<Broker>();
            string sql = "select * from " + Broker.TABLENAME+" where 1=1 ";
            string _sql = "select count(id) from " + Broker.TABLENAME + " where 1=1 ";
            if (!string.IsNullOrEmpty(workno)) { sql += " and (workno like '%" + workno +"%')"; }
            if (!string.IsNullOrEmpty(name)) { sql += " and (name like '%" + name + "%')"; }
            if (!string.IsNullOrEmpty(mobile)) { sql += " and (name like '%" + mobile + "%' or tel like '%"+ mobile +"%')"; }
            if (!string.IsNullOrEmpty(area)) { sql += " and (area like '%" + area + "%')"; }
            if (state>=0) { sql += " and state=" + Convert.ToInt16(state) ; }
            if (begin > DEF_DATE) { sql += " and addon>='"+ begin.Format() +"'"; }
            if (end > DEF_DATE) { sql += " and addon<='" + begin.Format() + "'"; }            
            int recordCount = helper.GetOne(_sql).ToInt();
            pageCount = pageno / PAGE_SIZE + (recordCount % PAGE_SIZE == 0 ? 0 : 1);
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
                        Password ="",
                        State =(BrokerState)Enum.Parse(typeof(BrokerState),r["State"].ToString()),
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
            Broker broker = Get(0,openid,"",true);
            ErrCode = 0;
            if (broker != null)
            {
                if (broker.Mobile == account || broker.WorkNo == account)
                {
                    if (broker.State ==  BrokerState.Checked)
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
                        ErrCode = Convert.ToInt16(broker.State);
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

        public Broker Get(int id=0, string openid="", string workno=null,bool withPassword=false) {
            Broker broker = null;
            if(id==0 && openid == "") { return null; }
            string sql= "select * from "+ Broker.TABLENAME +" where 1=1 ";
            if (id > 0) { sql += " and id=" + id; }
            if (!string.IsNullOrEmpty(workno)) { sql += " and workno='" + workno + "'"; }
            if (!string.IsNullOrEmpty(openid)) { sql += " and openid='" + openid + "'"; }
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
                        Password="",
                        State=(BrokerState)Enum.Parse(typeof(BrokerState), r["State"].ToString()),
                        Tel=r["Tel"].ToString(),
                        Trade=r["Trade"].ToString(),
                        UnionId=r["UnionId"].ToString(),
                        UptimeOn=r["UptimeOn"].ToDateTime(),
                        WorkNo=r["WorkNo"].ToString()
                    };
                    if (withPassword) { broker.Password = r["Password"].ToString(); }
                }
            }
            return broker;
        }
    }
}
