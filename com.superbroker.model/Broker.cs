using System;
using System.Collections.Generic;
using System.Text;

namespace com.superbroker.model
{
    /// <summary>
    /// 经纪人
    /// </summary>
    public class Broker : ModelBase
    {
        public new const String TABLENAME = "s_broker";
        /// <summary>
        /// OpenId
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// UnionId
        /// </summary>
        public string UnionId { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string WorkNo { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 备用电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string AvatarMediaId { get; set; }
        /// <summary>
        /// 所在区域
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// 所属单位
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 所在行业
        /// </summary>
        public string Trade { get; set; }
        /// <summary>
        /// 银行卡信息
        /// </summary>
        public string BankCardNo { get; set; }
        /// <summary>
        /// 开户行信息
        /// </summary>
        public string BankInfo { get; set; }
        /// <summary>
        /// 持卡人姓名
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 返佣比例
        /// </summary>
        public int FeePeer { get; set; }
        /// <summary>
        /// 当前状态 0未审核 1通过 2暂停 9拒绝 
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime CheckOn { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string CheckWorkNo { get; set; }
        /// <summary>
        /// 审核意见
        /// </summary>
        public string CheckInfo { get; set; }
        /// <summary>
        /// 上线时间
        /// </summary>
        public DateTime UptimeOn { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
    }
}
