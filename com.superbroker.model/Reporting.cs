﻿using System;
using System.Collections.Generic;
using System.Text;

namespace com.superbroker.model
{
    /// <summary>
    /// 申报记录
    /// </summary>
    public class Reporting:ModelBase
    {
        public new const String TABLENAME = "s_reporting";
        /// <summary>
        /// 申报编号
        /// </summary>
        public string ReportNo { get; set; }
        /// <summary>
        /// 经纪人
        /// </summary>
        public string BrokerNo { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public string CustomNo { get; set; }
        /// <summary>
        /// 楼盘
        /// </summary>
        public string BuilderNo { get; set; }
        /// <summary>
        /// 户型
        /// </summary>
        public string RoomNo { get; set; }
        /// <summary>
        /// 房号 楼号-单元-房号 为销控做准备
        /// </summary>
        public string HouseNo { get; set; }
        /// <summary>
        /// 返佣比例
        /// </summary>
        public int Fee { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public int TotalPrice { get; set; }
        /// <summary>
        /// 备案时间
        /// </summary>
        public DateTime ReportOn { get; set; }
        /// <summary>
        /// 备案保护期 7天
        /// </summary>
        public DateTime ProtectedOn { get; set; }
        /// <summary>
        /// 到场时间
        /// </summary>
        public DateTime ArriveOn { get; set; }
        /// <summary>
        /// 到场保护期 30天
        /// </summary>
        public DateTime DisableOn { get; set; }
        /// <summary>
        /// 首付时间
        /// </summary>
        public DateTime DownPaymentOn { get; set; }
        /// <summary>
        /// 签约时间
        /// </summary>
        public DateTime SignedOn { get; set; }
        /// <summary>
        /// 平台扣佣时间
        /// </summary>
        public DateTime DebitedOn { get; set; }
        /// <summary>
        /// 返佣时间
        /// </summary>
        public DateTime PayFeeOn { get; set; }
        /// <summary>
        /// 退房时间
        /// </summary>
        public DateTime FailOn { get; set; }
        /// <summary>
        /// 退佣至平台时间
        /// </summary>
        public DateTime ReturnFeeOn { get; set; }
        /// <summary>
        /// 退还佣金至开发商时间
        /// </summary>
        public DateTime BackFeeOn { get; set; }
        /// <summary>
        /// 当前状态
        /// </summary>
        public int State { get; set; }
    }
}
