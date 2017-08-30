using System;
using System.Collections.Generic;
using System.Text;

namespace com.superbroker.model
{
    /// <summary>
    /// 资金明细
    /// </summary>
    public class FundList:ModelBase
    {
        public new const String TABLENAME = "s_fundlist";
        /// <summary>
        /// 备案号
        /// </summary>
        public string ReportNo { get; set; }
        /// <summary>
        /// 金额 分
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 类型 0进平台 1出平台
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 资金用途
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 从经手人
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// 到经手人
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
    }
}
