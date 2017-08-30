using System;
using System.Collections.Generic;
using System.Text;

namespace com.superbroker.model
{
    /// <summary>
    /// 申报日志
    /// </summary>
    public class ReportLog:ModelBase
    {
        public new const String TABLENAME = "s_reportlog";
        /// <summary>
        /// 申报单
        /// </summary>
        public string ReportNo { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string WorkNo { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
    }
}
