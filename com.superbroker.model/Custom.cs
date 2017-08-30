using System;
using System.Collections.Generic;
using System.Text;

namespace com.superbroker.model
{
    /// <summary>
    /// 客户信息
    /// </summary>
    public class Custom:ModelBase
    {
        public new const String TABLENAME = "s_custom";
        public string CustomNo { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// UnionId
        /// </summary>
        public string UnionId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }

    }
}
