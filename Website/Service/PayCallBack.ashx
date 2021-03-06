﻿<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;
using LitJson;
using com.seascape.tools;
using com.seascape.wechat;
using com.superbroker.data;
using com.superbroker.model;


public class Handler : IHttpHandler
{
    public static string HjKeyValue = "Seascape.Fast.Fix";
    public static string APPID = com.seascape.tools.BasicTool.GetConfigPara("appid");
    public static string SECRET = com.seascape.tools.BasicTool.GetConfigPara("secret");    
    //微信支付商户
    public static string mch_id = com.seascape.tools.BasicTool.GetConfigPara("mch_id");
    //微信支付提交服务器IP
    public static string client_ip = com.seascape.tools.BasicTool.GetConfigPara("client_ip");
    //合作伙伴默认每单提取金额
    public static int partnerRedNum = 20*100;

    public static string MsgUrl = "http://hjhk.edmp.cc/service/Handler.ashx";
    
    public void ProcessRequest(HttpContext c)
    {
        string Result = "";
        Log.D("PayCallBack:-------",c);
        Result = OperPayResult(c);//处理支付结果
        Log.D("Result:"+Result,c);
        c.Response.ContentType = "text/plain";
        c.Response.Write(Result);
    }

    
    /// <summary>
    /// 处理支付返回结果
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public string OperPayResult(HttpContext c)
    {
        return "";
    }
      

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}