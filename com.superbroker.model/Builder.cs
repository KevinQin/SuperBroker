using System;
using System.Collections.Generic;
using System.Text;

namespace com.superbroker.model
{
    public class SimpleBuilderView {
        /// <summary>
        /// 楼盘编号
        /// </summary>
        public string BuilderNo { get; set; }
        /// <summary>
        /// 楼盘名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 口号
        /// </summary>
        public string Slogen { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 头图
        /// </summary>
        public string HeadImg { get; set; }
    }

    /// <summary>
    /// 楼盘
    /// </summary>
    public class Builder : ModelBase
    {
        public new const String TABLENAME = "s_builder";
        /// <summary>
        /// 楼盘编号
        /// </summary>
        public string BuilderNo { get; set; }
        /// <summary>
        /// 楼盘名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 口号
        /// </summary>
        public string Slogen { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 开发商/品牌
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// 提供商名称
        /// </summary>
        public string Provider { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 楼盘电话
        /// </summary>
        public string HotTel { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 楼盘负责人工号
        /// </summary>
        public string ManagerNo { get; set; }
        /// <summary>
        /// 楼盘面积
        /// </summary>
        public int Area { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// 地址Url
        /// </summary>
        public string MapUrl { get; set; }
        /// <summary>
        /// VR
        /// </summary>
        public string VRUrl { get; set; }
        /// <summary>
        /// 3D
        /// </summary>
        public string D3Url{get;set;}
        /// <summary>
        /// 分佣比例/数额
        /// </summary>
        public int FeePeer { get; set; }
        /// <summary>
        /// 分佣类型 0按比例 1固定金额
        /// </summary>
        public int FeeType { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 下架时间
        /// </summary>
        public DateTime OffOn { get; set; }
    }
}
