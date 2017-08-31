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
        /// UnionId
        /// </summary>
        public string UnionId { get; set; }
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
        /// 国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }    
        /// <summary>
        /// 微信昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 关注二维码编码
        /// </summary>
        public int QrCode { get; set; }
        /// <summary>
        /// 是否已关注
        /// </summary>
        public bool IsFllow { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
    }
}
