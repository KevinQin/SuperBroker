using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using com.seascape.tools;

/// <summary>
/// WeChat 的摘要说明
/// </summary>
public class WeChat
{
    static string BASE_URL = "http://r.4009990351.com/";

	public WeChat()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public static void InitConifg(Page page)
    {
        string urlPath = page.Request.Url.AbsoluteUri;
        if (urlPath.IndexOf("#") > -1)
        {
            urlPath = urlPath.Split('#')[0].Replace("?", "%3f");
        }
        urlPath = urlPath.Replace("&", "%26");
        string Url = BASE_URL+ "/service/app.ashx?fn=6&Url=" + urlPath;
        HttpContext c = new HttpContext(page.Request, page.Response);
        Log.D(Url, c, "WX[InitConifg]");
        string Para = BasicTool.webRequest(Url);
        Log.D(Para, c, "WX[InitConifg]-Return");
        Object wxObj = new { appid = "", timestamp = "", nonce = "", signature = ""};
        if (Para.Split('|').Length == 4)
        {
            string[] Paras = Para.Split('|');
            wxObj = new { appId = Paras[3].ToString(), timestamp = Paras[1].ToString(), nonceStr = Paras[2].ToString(), signature = Paras[0].ToString() };
        }
        if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "jsmode"))
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "jsmode", "<script>var WxConfigInfo=" + LitJson.JsonMapper.ToJson(wxObj) + ";</script>");
        }
    }

    public static void InitPayConfig(Page page,string orderno)
    {
        string strPayConfig = BasicTool.webRequest(BASE_URL + "/service/Handler.ashx?fn=5&orderno=" + orderno);
        Object PayConfig = new { appid = "", timestamp = "", nonce = "", signature = "", package = "" };
        if (strPayConfig.Split('|').Length == 5)
        {
            string[] payPar = strPayConfig.Split('|');
            PayConfig = new { appid = payPar[0].ToString(), timestamp = payPar[4].ToString(), nonce = payPar[1].ToString(), signature = payPar[3].ToString(), package = payPar[2].ToString() };
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "jspaymode"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "jspaymode", "<script>var WxPayConfigInfo=" + LitJson.JsonMapper.ToJson(PayConfig) + ";</script>");
            }
        }
        else
        {
            if (orderno.StartsWith("B"))
            {
                page.Response.Redirect("balance.html");
            }
            else
            {
                page.Response.Redirect("orderdetail.html?orderno=" + orderno);
            }
        }        
    }

    public static void InitCard(Page page)
    {
        string strCardConfig = BasicTool.webRequest(BASE_URL + "/service/app.ashx?fn=7");
        Object CardConfig = new {  timestamp = "", nonce = "", cardSign = "", priceList = new List<object>() };
        if (strCardConfig.Split('|').Length == 5)
        {
            string[] cardPar = strCardConfig.Split('|');
            string[] cardpriceList = cardPar[4].Split(',');
            List<object> dict = new List<object>();
            foreach (string p in cardpriceList)
            {
                string [] ps = p.Split('@');
                dict.Add(new{key=ps[0],value=Convert.ToInt32(ps[1])});
            }
            CardConfig = new { timestamp = cardPar[1].ToString(), nonce = cardPar[2].ToString(), cardSign = cardPar[3].ToString(),priceList=dict };
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "jscardmode"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "jscardmode", "<script>var WxCardConfigInfo=" + LitJson.JsonMapper.ToJson(CardConfig) + ";</script>");
            }
        }
        else
        {
            page.Response.Redirect("uc.html");
        }
    }
}