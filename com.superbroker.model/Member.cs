using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.superbroker.model
{
    public class Member:ModelBase
    {
        public new const String TABLENAME = "s_member";
     
        /// <summary>
        /// OpenId
        /// </summary>
        public string OpenId { get; set; }       
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string PhotoUrl { get; set; }
        /// <summary>
        /// 所在区域
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }    
        /// <summary>
        /// 微信昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
    }
}
