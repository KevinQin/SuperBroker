using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.superbroker.model
{
    public class BrokerLog:ModelBase
    {
        public new static string TABLENAME = "s_BrokerLog";
        /// <summary>
        /// 经纪人
        /// </summary>
        public string BrokerNo { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string WorkNo { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Info { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }
    }
}
