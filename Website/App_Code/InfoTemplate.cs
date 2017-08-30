using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.seascape.wechat;

/// <summary>
/// 消息模板消息
/// </summary>
public class InfoTemplate:BasicTemplate
{
    public TM_Item keyword3 { get; set; }
    public TM_Item keyword4 { get; set; }
}

public class BasicTemplate {
    public TM_Item first { get; set; }
    public TM_Item keyword2 { get; set; }
    public TM_Item keyword1 { get; set; }
    public TM_Item remark { get; set; }
}