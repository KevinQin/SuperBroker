using System;
using System.Collections.Generic;
using System.Text;

namespace com.superbroker.model
{
    /// <summary>
    /// 户型
    /// </summary>
    public class Room:ModelBase
    {
        public new const String TABLENAME = "s_room";
        /// <summary>
        /// 所属楼盘
        /// </summary>
        public string BuilderNo { get; set; }
        public string RoomNo { get; set; }
        /// <summary>
        /// 户型名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public double Area { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 户型图
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// VR
        /// </summary>
        public string VRUrl { get; set; }
        /// <summary>
        /// 3D
        /// </summary>
        public string D3Url { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
    }
}
