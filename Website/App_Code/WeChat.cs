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

    /// <summary>
    /// 得到用户IP
    /// </summary>
    /// <param name="r">Request对象</param>
    /// <returns></returns>
    public static string GetIp(HttpRequest r)
    {
        string Ip = string.Empty;
        if (r.ServerVariables["HTTP_VIA"] != null)
        {
            if (r.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
            {
                if (r.ServerVariables["HTTP_CLIENT_IP"] != null)
                    Ip = r.ServerVariables["HTTP_CLIENT_IP"].ToString();
                else
                    if (r.ServerVariables["REMOTE_ADDR"] != null)
                    Ip = r.ServerVariables["REMOTE_ADDR"].ToString();
                else
                    Ip = "0.0.0.0";
            }
            else
                Ip = r.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        }
        else if (r.ServerVariables["REMOTE_ADDR"] != null)
        {
            Ip = r.ServerVariables["REMOTE_ADDR"].ToString();
        }
        else
        {
            Ip = "0.0.0.0";
        }
        return Ip;
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

    /// <summary>
    /// 获取全局Access_Token
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>

    public static string GetAccessToken(HttpContext _c)
    {
        string APPID = com.seascape.tools.BasicTool.GetConfigPara("appid");
        string SECRET = com.seascape.tools.BasicTool.GetConfigPara("secret");
        string Access_Token = "";
        bool isFail = string.IsNullOrEmpty(_c.Request["isFail"]) ? true : true;
        if (isFail || string.IsNullOrEmpty(_c.Cache["Global_Access_Token"].ToString()))
        {
            com.seascape.wechat.AccessToken accessToken = new com.seascape.wechat.Common(APPID, SECRET).GetAccessToken();
            _c.Cache.Add("Global_Access_Token", accessToken.token + "^" + accessToken.expirestime.Ticks, null, accessToken.expirestime, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
            Access_Token = accessToken.token;
        }
        else
        {
            string[] tokenp = _c.Cache["Global_Access_Token"].ToString().Split('^');
            long expticket = Convert.ToInt64(tokenp[1]);
            if (expticket < DateTime.Now.Ticks)
            {
                _c.Cache["Global_Access_Token"] = "";
                Access_Token = GetAccessToken(_c);
            }
            else {
                Access_Token = tokenp[0];
            }
        }
        return Access_Token;
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

    /// <summary>
    /// 输入流转字符串
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public string ConvertStream(HttpContext c)
    {
        System.IO.Stream sm = c.Request.InputStream;//获取post数据
        int len = (int)sm.Length;//post数据的长度
        byte[] inputByts = new byte[len];//存储post数据
        sm.Read(inputByts, 0, len);//将post数据写入数组
        sm.Close();//关闭流
        string data = System.Text.Encoding.GetEncoding("utf-8").GetString(inputByts);//转换为unicode字符串  
        return data;
    }
}