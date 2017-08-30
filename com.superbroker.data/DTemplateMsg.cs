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
    /// <summary>
    /// 消息
    /// </summary>
    public class DTemplateMsg : DbCenter, IDB<TemplateMsg>
    {

        public bool Add(out int Id, TemplateMsg message)
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


        public bool Update(TemplateMsg message)
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

        public TemplateMsg Get(int id)
        {
            string sql = "select * from " + TemplateMsg.TABLENAME + " where id= " + id;
            TemplateMsg c = null;
            using (DataTable dt = helper.GetDataTable(sql))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    c = new TemplateMsg();
                    c.Id = r["Id"].ToInt();
                    c.Enable =r["Enable"].ToInt16() == 1;
                    c.MsgBody = r["MsgBody"].ToString();
                    c.MsgContent = r["MsgContent"].ToString();
                    c.MsgId = r["MsgId"].ToString();
                    c.MsgUrl = r["MsgUrl"].ToString();
                    c.OpenId = r["OpenId"].ToString();
                    c.AddOn = r["SendOn"].ToDateTime();
                    c.ToUser = r["ToUser"].ToString();
                    c.OrderNo = r["OrderNo"].ToString();
                }
            }
            return c;
        }

        public bool Delete(int id)
        {
            return helper.ExecuteSqlNoResult("delete from " + TemplateMsg.TABLENAME + " where id=" + id);
        }

        public List<TemplateMsg> GetMsgList(out int pageCount, string key, int pageNum = 1)
        {
            List<TemplateMsg> list = new List<TemplateMsg>();
            pageCount = 1;
            string sql = "select count(id) from " + TemplateMsg.TABLENAME;
            string where = " where 1=1 ";
            if (key != "") { where += " and Uid in (select id from " + Member.TABLENAME + " where name='" + key + "' or nickname='" + key + "' or mobile='" + key + "')"; }
            int recordCount = Convert.ToInt32(helper.GetOne(sql + where));
            pageCount = pageCount / PAGE_SIZE + pageCount % PAGE_SIZE == 0 ? 0 : 1;

            sql = "select * from " + TemplateMsg.TABLENAME + where + " order by id desc limit " + (pageNum - 1) * PAGE_SIZE + "," + PAGE_SIZE;
            using (DataTable dt = helper.GetDataTable(sql))
            {
                foreach (DataRow r in dt.Rows)
                {
                    TemplateMsg c = new TemplateMsg();
                    c.Id = r["Id"].ToInt();
                    c.Enable = r["Enable"].ToInt16() == 1;
                    c.MsgBody = r["MsgBody"].ToString();
                    c.MsgContent = r["MsgContent"].ToString();
                    c.MsgId = r["MsgId"].ToString();
                    c.MsgUrl = r["MsgUrl"].ToString();
                    c.OpenId = r["OpenId"].ToString();
                    c.AddOn = r["SendOn"].ToDateTime();
                    c.ToUser = r["ToUser"].ToString();
                    c.OrderNo = r["OrderNo"].ToString();
                    list.Add(c);
                }
            }
            return list;
        }
    }
}
