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
    public class DAdmin : DbCenter, IDB<Admin>
    {
        public bool Add(out int Id, Admin t)
        {
            t.Password = PasswordEncode(t.Password);
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

        public bool Update(Admin t)
        {
            t.Password = PasswordEncode(t.Password);
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

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Admin Login(string account, string password)
        {
            string endcodePwd = PasswordEncode(password);
            string sql = "select * from " + Admin.TABLENAME + " where enable=1 and  workno='" + account + "' and password='" + endcodePwd + "' ";
            Admin admin = null;
            using (DataTable dt = helper.GetDataTable(sql))
            {
                if (dt != null && dt.Rows.Count == 1)
                {
                    DataRow r = dt.Rows[0];
                    admin = new Admin()
                    {
                        AddOn = DateTime.Parse(r["addon"].ToString()),
                        Department = r["Department"].ToString(),
                        Enable = int.Parse(r["enable"].ToString()) == 1,
                        Id = int.Parse(r["Id"].ToString()),
                        Memo = r["Memo"].ToString(),
                        WorkNo = r["WorkNo"].ToString(),
                        Password = "",
                        RoleId = int.Parse(r["roleId"].ToString()),
                        Name = r["Name"].ToString()
                    };
                }
            }
            return admin;
        }

        /// <summary>
        /// 判断是否有重名的用户
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool CheckOnlyOne(string account)
        {
            string sql = "select id from " + Admin.TABLENAME + " where WorkNo='" + account + "'";
            return helper.GetOne(sql) == null;
        }

        public string GetAdminWorkNo(int Id)
        {
            return helper.GetOne("select workno from " + Admin.TABLENAME + " where id=" + Id).ToString();
        }

        public bool GetAdminByWorkNo(string workno)
        {
            return Convert.ToInt16(helper.GetOne("select count(id) from " + Admin.TABLENAME + " where  workno='" + workno + "'")) == 1;
        }


        public bool ChangePwd(string pwd, int adminId)
        {
            string sql = "update " + Admin.TABLENAME + " set password='" + PasswordEncode(pwd) + "' where id=" + adminId;
            return helper.ExecuteSqlNoResult(sql);
        }

        public bool SetRole(int roleId, string workno)
        {
            return helper.ExecuteSqlNoResult("update " + Admin.TABLENAME + " set RoleId=" + roleId + " where workno='" + workno + "'");
        }

        public bool SetDepartment(string depStr, string workno)
        {
            return helper.ExecuteSqlNoResult("update " + Admin.TABLENAME + " set Department='" + depStr + "' where workno='" + workno + "'");
        }

        public bool SetEnable(bool enable, string workno)
        {
            return helper.ExecuteSqlNoResult("update " + Admin.TABLENAME + " set enable=" + (enable ? 1 : 0) + " where workno='" + workno + "'");
        }

        public List<Admin> GetList(int Role)
        {
            List<Admin> list = new List<Admin>();
            string sql = "select * from " + Admin.TABLENAME + " where enable=1 and RoleId=" + Role;
            using (DataTable dt = helper.GetDataTable(sql))
            {
                foreach (DataRow r in dt.Rows)
                {
                    Admin admin = new Admin()
                    {
                        AddOn = DateTime.Parse(r["addon"].ToString()),
                        Department = r["Department"].ToString(),
                        Enable = int.Parse(r["enable"].ToString()) == 1,
                        Id = int.Parse(r["Id"].ToString()),
                        Memo = r["Memo"].ToString(),
                        WorkNo = r["WorkNo"].ToString(),
                        Password = "",
                        RoleId = int.Parse(r["roleId"].ToString()),
                        Name=r["Name"].ToString()
                    };
                    list.Add(admin);
                }
            }
            return list;
        }
    }
}
