using System;
using System.Collections.Generic;
using System.Text;

namespace com.superbroker.model
{

    /// <summary>
    /// 管理员
    /// </summary>
    public class Admin:ModelBase
    {
        public new const String TABLENAME = "s_admin";

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string WorkNo { get; set; }
        /// <summary>
        /// Openid
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public Role RoleId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enable { get; set; }
    }

    public enum Role
    {
        Admin=100,Builder=101,Fund=102,Editer=103,Bussiness=104, Broker=199
    }
}
