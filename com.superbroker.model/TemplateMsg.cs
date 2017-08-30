using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.superbroker.model
{
    public class TemplateMsg:ModelBase
    {
        public new const String TABLENAME = "s_template_msg";

        /// <summary>
        /// OpenId
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 消息体
        /// </summary>
        public string MsgBody { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string MsgContent { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string MsgUrl { get; set; }
        /// <summary>
        /// 消息ID
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 收件人ID
        /// </summary>
        public string ToUser { get; set; }
        /// <summary>
        /// 关联的订单号
        /// </summary>
        public string OrderNo { get; set; }
    }
}
