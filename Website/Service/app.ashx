<%@ WebHandler Language="C#" Class="AppHandler" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Web.SessionState;
using System.Text;
using System.Text.RegularExpressions;
using LitJson;
using com.seascape.tools;
using com.superbroker.data;
using com.superbroker.model;
using com.seascape.wechat;


public class AppHandler:IHttpHandler,IRequiresSessionState {
    //微信公众号相关
    static string APPID = com.seascape.tools.BasicTool.GetConfigPara("appid");
    static string SECRET = com.seascape.tools.BasicTool.GetConfigPara("secret");
    //微信支付商户
    static string mch_id = com.seascape.tools.BasicTool.GetConfigPara("mch_id");
    //微信支付接收返回地址
    static string notify_url = com.seascape.tools.BasicTool.GetConfigPara("notify_url");
    //微信支付提交服务器IP
    static string client_ip = com.seascape.tools.BasicTool.GetConfigPara("client_ip");
    //微信支付密钥
    static string keyValue = com.seascape.tools.BasicTool.GetConfigPara("keyValue");
    static string HjKeyValue = "Seascape.Fast.Fix";
    //默认代金券信息
    static int voucherFee = 10;
    static string voucherS = "J";
    //微信卡券配置信息
    static string wxcardConfig = "pR7BJwMJKGp4Aun2O7OaMOb9nBTM@10";
    //系统相关
    static int perPage = 15;
    static string DefaultWrokNo = "100";
    static string _BASE_URL = com.seascape.tools.BasicTool.GetConfigPara("baseurl");
    static string MsgUrl = "http://"+_BASE_URL+"/service/app.ashx";
    static string BaseUrl = "http://"+_BASE_URL+"/";
    static Response response;
    HttpContext _c;
    static Admin admin = null;

    public void ProcessRequest(HttpContext c) {
        response = new Response();
        _c = c;
        int F = string.IsNullOrEmpty(c.Request["fn"]) ? 0 : Convert.ToInt16(c.Request["fn"]);
        c.Response.ContentType = "text/plain";
        c.Response.Write(GetResult(F, c));
    }
    /// <summary>
    /// 功能导航
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public string GetResult(int f, HttpContext c)
    {
        string Result = response.Fail("System Exception[" + f + "]", 1);
        Log.D("[A]Request-[FN:" + f + "]", c);
        try
        {
            switch (f)
            {
                case 1:
                    Result = GetOpenId(c);//获取OPENID
                    break;
                case 2:
                    Result = GetMember(c);//获取会员信息
                    break;
                case 3:
                    Result = CreateCode(c);//用户二维码生成
                    break;
                case 4:
                    Result = InitMenu(c);//初始化菜单
                    break;
                case 5:
                    Result = GetPayInfo_(c);//获取支付相关信息
                    break;
                case 6:
                    Result = WxConfig(c);//微信Config
                    break;
                case 7:
                    Result = GetWxCardInfo(c);//获取微信卡包接口
                    break;
                case 8:
                    Result = DestoryWxCard(c);//核销卡券
                    break;
                case 9:
                    Result = GetLocation(c);//获取地理位置
                    break;
                case 100:
                    Result = "";
                    break;
                case 999:
                    Result = Test(c);
                    break;
                default:
                    break;
            }
        }
        catch (Exception e)
        {
            Result = e.Message.ToString();
        }
        Log.D("[A]Result-" + f + ":" + Result.ToString(), c);
        return ReplaceTableName(Result);
    }

    public string Test(HttpContext c)
    {
        string xml = "<xml><ToUserName><![CDATA[gh_1ead2193ad3d]]></ToUserName><FromUserName><![CDATA[o3MRawEmK5OK-ringNnTyiNPA6uM]]></FromUserName><CreateTime>1504150583</CreateTime><MsgType><![CDATA[event]]></MsgType><Event><![CDATA[VIEW]]></Event><EventKey><![CDATA[https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx9a6c6b62bc80e7d8&redirect_uri=http%3a%2f%2fb.seascapeapp.cn%2fapp%2fregister.aspx&response_type=code&scope=snsapi_userinfo&state=i#wechat_redirect]]></EventKey><MenuId>413650345</MenuId></xml>";
        BaseMessage msg= Common.ConvertObj<EventMessage>(xml);
        Log.F(msg.FromUserName,c);
        return response.Success(DateTime.Now.Format());
    }

    public string GetOrderNo()
    {
        return string.Format("{0}{1:00}", DateTime.Now.ToString("yyMMddHHmmss"), new Random().Next(99));
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

    private string ReplaceTableName(string str)
    {
        Regex reg = new Regex(",\\\"TABLENAME\\\":\\\"\\w*\\\"");
        str = reg.Replace(str, "");
        return str;
    }

    #region About Wechat
    public string GetLocation(HttpContext c) {
        double latitude = Convert.ToDouble(c.Request["latitude"]);
        double longitude = Convert.ToDouble(c.Request["longitude"]);
        return GetFormattedAddress(latitude, longitude);
    }

    private string GetFormattedAddress(double latitude, double longitude)
    {
        try
        {
            string url = "http://maps.google.cn/maps/api/geocode/json?latlng={0},{1}&sensor=true&language=zh-CN";
            string json = BasicTool.webRequest(string.Format(url, latitude, longitude));
            LitJson.JsonData allData = LitJson.JsonMapper.ToObject(json);
            LitJson.JsonData resultData = allData["results"];
            LitJson.JsonData firstData = resultData[0];
            string formatAddress = firstData["formatted_address"].ToString();
            formatAddress = formatAddress.Split(' ')[0].Replace("中国", "").Trim();
            int len = firstData["address_components"].Count;
            //string Address = firstData["address_components"][0]["long_name"].ToString();
            string prov = firstData["address_components"][len-3]["long_name"].ToString();
            string city = firstData["address_components"][len-4]["long_name"].ToString();
            string dist = firstData["address_components"][len-5]["long_name"].ToString();
            string Address = formatAddress.Replace(prov, "").Replace(city, "").Replace(dist, "");
            if (formatAddress.ToUpper().IndexOf("unnamed") > -1) {
                Address = "";
                if (city != "") { Address += city; }
                if (dist != "") { Address += dist; }
                if (Address == "") { Address = prov; }
            }
            object obj = new { Full = formatAddress, A=Address, P = prov, C = city, D = dist };
            return response.Success(obj);
        }
        catch (Exception ex)
        {
            return response.Fail("can not get location");
        }
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
    /// <summary>
    /// 通过Code获取OpenID
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public string GetOpenId(HttpContext c)
    {
        string code = string.IsNullOrEmpty(c.Request["code"]) ? "" : c.Request["code"].ToString();
        string source = string.IsNullOrEmpty(c.Request["source"]) ? "" : c.Request["source"].ToString();
        Log.D("code:"+code,_c);
        Common.appid = APPID;
        Common.secret = SECRET;
        access_token_UserInfo at = Common.Get_access_token_UserInfo(code);
        if (!string.IsNullOrEmpty(at.openid))
        {
            Log.D("openid:" + at.openid, _c);
            //------------------写用户------------------
            Member u = new DMember().Get(at.openid);
            if (u != null)
            {
            }
            else
            {
                u = new Member();
                string r = "";
                UserInfo ui =UserInfo.getUserInfoByCode(at.access_token, at.openid, out r);
                Log.D("r:" + r, _c);
                if (ui != null && !string.IsNullOrEmpty(ui.nickname))
                {
                    source = source.Replace("i", "1");
                    u.NickName = ui.nickname.Replace("'", "").Replace(@"""", "");
                    u.PhotoUrl = ui.headimgurl;
                    u.Area = ui.country + "|" + ui.province + "|" + ui.city;
                    u.Gender = ui.sex;
                    u.OpenId = ui.openid;
                    u.Mobile = "";
                    u.Name = "";
                    u.AddOn = DateTime.Now;
                    u.Memo = "";
                    int id = 0;
                    new DMember().Add(out id, u);
                    u.Id = id;
                }
            }
            //------------------------------------------

            return response.Success(at.openid);
        }
        else
        {
            Log.D("openid:" + at.errcode.ToString(), _c);
            return response.Fail(at.errcode.ToString(),1);
        }
    }


    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public string GetMember(HttpContext c)
    {
        string openId = string.IsNullOrEmpty(c.Request["openId"]) ? "" : c.Request["openId"].ToString();
        string source = string.IsNullOrEmpty(c.Request["source"]) ? "" : c.Request["source"].ToString();
        if (openId.Length > 0)
        {
            Member u = new DMember().Get(openId);
            if (u != null)
            {
                return response.Success(u);
            }
            else
            {
                u = new Member();
                string r = "";
                UserInfo ui = UserInfo.getUserInfoByGlobal(Get_Access_Token(c), openId, out r);
                Log.D("r:"+ r,_c);
                if (ui != null && !string.IsNullOrEmpty(ui.nickname))
                {
                    u.NickName = ui.nickname.Replace("'", "").Replace(@"""", "");
                    u.PhotoUrl = ui.headimgurl;
                    u.Area = ui.country + "|" + ui.province + "|" + ui.city;
                    u.Gender = ui.sex;
                    u.OpenId = ui.openid;
                    u.Mobile = "";
                    u.Name = "";
                    u.AddOn = DateTime.Now;
                    u.Memo = "";
                    int Id = 0;
                    new DMember().Add(out Id,u);
                    u.Id = Id;
                    return response.Success(u);
                }
                else
                {
                    return response.Fail("get member info error", 1);
                }
            }
        }
        return response.Fail("open id length is too short", 2);
    }



    /// <summary>
    /// 微信认证
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public string WxConfig(HttpContext c)
    {
        Common.WxConfig w = InitJsApi(c);
        if (w != null)
        {
            return w.signature + "|" + w.timestamp + "|" + w.nonce + "|" + w.appid;
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 用户二维码生成
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public string CreateCode(HttpContext c)
    {
        int uid = string.IsNullOrEmpty(c.Request["uid"]) ? 0 : Convert.ToInt16(c.Request["uid"]);
        if (uid > 0)
        {
            string FilePath = c.Server.MapPath("/QrCode/user/");
            BasicTool.CheckFolder(FilePath,true);
            FilePath+= uid + ".jpg";
            int SceneID = uid;
            string Result = new Common(APPID, SECRET).GetQR_Code(FilePath, SceneID);
            Log.D("推荐有礼分享成功["+ uid +"]", _c);
            return response.Success("success");
        }
        return response.Fail("error", 1);
    }


    /// <summary>
    /// 发送模板消息
    /// </summary>
    /// <param name="o"></param>
    /// <param name="key"></param>
    /// <param name="openid"></param>
    /// <param name="url"></param>
    public void SendTemplateMessage(HttpContext c, object o, string key, string openid, string url, string MsgContent,string orderno="")
    {
        TMessage t = new TMessage
        {
            touser = openid,
            data = o,
            template_id = key,
            url = url,
            topcolor = ""
        };
        try
        {
            TemplateMsg tmp = new TemplateMsg
            {
                MsgId = key,
                MsgBody = JsonMapper.ToJson(o),
                MsgUrl = url,
                OpenId = openid,
                AddOn = DateTime.Now,
                MsgContent = MsgContent,
                OrderNo = orderno
            };
            int Id = 0;
            new DTemplateMsg().Add(out Id, tmp);
        }
        catch { }
        new TMessage().Send_TemplateMsg(t, Get_Access_Token(c));
    }

    /// <summary>
    /// 获取全局Access_Token
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public string Get_Access_Token(HttpContext c)
    {
        string Access_Token = "";
        bool isFail = string.IsNullOrEmpty(c.Request["isFail"]) ? true : true;
        if (isFail || string.IsNullOrEmpty(c.Cache["Global_Access_Token"].ToString()))
        {
            AccessToken accessToken = new Common(APPID, SECRET).GetAccessToken();
            c.Cache.Add("Global_Access_Token", accessToken.token, null, accessToken.expirestime, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
        }
        else
        {
            Access_Token = c.Cache["Global_Access_Token"].ToString();
        }
        return Access_Token;
    }


    /// <summary>
    /// 获取支付相关信息--字符串
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public string GetPayInfo_(HttpContext c)
    {
        return "";
    }

    /// <summary>
    /// 处理支付
    /// </summary>
    /// <param name="OrderNo"></param>
    /// <param name="prepayID"></param>
    /// <returns></returns>
    public WxPay.WxPayConfig OperPay(string OrderNo, string prepayID)
    {
        WxPay.WxPayConfig wp = null;
        return wp;
    }


    /// <summary>
    /// 发送菜单
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public string InitMenu(HttpContext c)
    {
        string errmsg = "ERR";
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("    \"button\": [");
            sb.Append("        {");
            sb.Append("            \"type\": \"view\", ");
            sb.Append("            \"name\": \"加入\", ");
            sb.Append("            \"url\": \"https://open.weixin.qq.com/connect/oauth2/authorize?appid="+ APPID +"&redirect_uri=http%3a%2f%2f"+ _BASE_URL +"%2fapp%2fregister.aspx&response_type=code&scope=snsapi_userinfo&state=i#wechat_redirect\"");
            sb.Append("        }, ");
            sb.Append("        {");
            sb.Append("            \"type\": \"view\", ");
            sb.Append("            \"name\": \"备案\", ");
            sb.Append("            \"url\": \"https://open.weixin.qq.com/connect/oauth2/authorize?appid="+ APPID +"&redirect_uri=http%3a%2f%2f"+ _BASE_URL +"%2fapp%2freport.aspx&response_type=code&scope=snsapi_userinfo&state=i#wechat_redirect\"");
            sb.Append("        }, ");
            sb.Append("        {");
            sb.Append("           \"name\": \"我的\", ");
            sb.Append("            \"sub_button\": [");
            sb.Append("                {");
            sb.Append("                    \"type\": \"view\", ");
            sb.Append("                    \"name\": \"个人中心\", ");
            sb.Append("                    \"url\": \"https://open.weixin.qq.com/connect/oauth2/authorize?appid="+ APPID +"&redirect_uri=http%3a%2f%2f"+ _BASE_URL +"%2fapp%2fuc.aspx&response_type=code&scope=snsapi_userinfo&state=i#wechat_redirect\"");
            sb.Append("                }, ");
            sb.Append("                {");
            sb.Append("                    \"type\": \"view\", ");
            sb.Append("                    \"name\": \"我的备案\", ");
            sb.Append("                    \"url\": \"https://open.weixin.qq.com/connect/oauth2/authorize?appid="+ APPID +"&redirect_uri=http%3a%2f%2f"+ _BASE_URL +"%2fapp%2freport.aspx&response_type=code&scope=snsapi_userinfo&state=i#wechat_redirect\"");
            sb.Append("                }, ");
            sb.Append("                {");
            sb.Append("                    \"type\": \"view\", ");
            sb.Append("                    \"name\": \"我的佣金\", ");
            sb.Append("                    \"url\": \"https://open.weixin.qq.com/connect/oauth2/authorize?appid="+ APPID +"&redirect_uri=http%3a%2f%2f"+ _BASE_URL +"%2fapp%2ffee.aspx&response_type=code&scope=snsapi_userinfo&state=i#wechat_redirect\"");
            sb.Append("                }, ");
            sb.Append("                {");
            sb.Append("                    \"type\": \"view\", ");
            sb.Append("                    \"name\": \"消息中心\", ");
            sb.Append("                     \"url\": \"https://open.weixin.qq.com/connect/oauth2/authorize?appid="+ APPID +"&redirect_uri=http%3a%2f%2f"+ _BASE_URL +"%2fapp%2fnotify.aspx&response_type=code&scope=snsapi_userinfo&state=i#wechat_redirect\"");
            sb.Append("                }");
            sb.Append("            ]");
            sb.Append("        }");
            sb.Append("    ]");
            sb.Append("}");
            string ACCESS_TOKEN = Get_Access_Token(c);
            string GetUrl = " https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + ACCESS_TOKEN;
            string JsonStr = new Common(APPID,SECRET).webRequest(GetUrl, sb.ToString());
            JsonData jd = JsonMapper.ToObject(JsonStr);
            if (jd["errcode"].ToString() == "0")
            {
                return "{Return:0,Msg:'OK'}";
            }
            else
            {
                try
                {
                    errmsg = "[" + jd["errcode"].ToString() + "]" + jd["errmsg"].ToString();
                }
                catch { }
            }
        }
        return "{Return:1,Msg:'" + errmsg + "'}";
    }


    /// <summary>
    /// 初始化微信JS
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public Common.WxConfig InitJsApi(HttpContext c)
    {
        try
        {
            string Urls = c.Request["Url"] == null ? "" : c.Request["Url"].ToString();
            bool isFail = string.IsNullOrEmpty(c.Request["isFail"]) ? false : true;
            if (Urls.IndexOf("#") != -1)
            {
                Urls = Urls.Split('#')[0].ToString();
            }
            Urls = Urls.Replace("%26", "&");
            Urls = Urls.Replace("|", "%7C");
            string jsApi_ticket = "";
            string Access_Token = "";
            if (isFail || c.Cache["Para_JsApiTicket"] == null || c.Cache["Para_JsApiTicket"].ToString().Length == 0)
            {
                Access_Token = Get_Access_Token(c);
                //获取网页调用临时票据
                string r = "";
                jsApi_ticket = new Common(APPID, SECRET).Get_jsapi_ticket(Access_Token, out r);
                Log.D("r:" + r, c);
                Log.D("jsApi_ticket:" + jsApi_ticket, c);
                c.Cache.Add("Para_JsApiTicket", jsApi_ticket, null, System.DateTime.UtcNow.AddMinutes(100), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
            }
            else
            {
                jsApi_ticket = c.Cache["Para_JsApiTicket"].ToString();
            }


            string TempS = "";
            Log.D("isFail-Urls:" + isFail.ToString() + Urls, c);
            Common.WxConfig w = Common.Get_Config_(Urls, jsApi_ticket, out TempS);
            Log.D("TempS:" + TempS, c);
            return w;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 调用微信卡包接口
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public string GetWxCardInfo(HttpContext c)
    {
        string nonce_str = new Random().Next(100000, 999999).ToString();
        string card_id = "";// "pR7BJwD6sYq-5TAhAMQd2U4Ko-9g";
        string r = "";
        string api_ticket = new WxCard().Get_api_ticket(Get_Access_Token(c), out r);
        if (api_ticket.Length > 0)
        {
            long timestamp = 0;
            string sign = new WxCard().Get_Signature(APPID, nonce_str, card_id, api_ticket, out timestamp);
            if (sign.Length > 0)
            {
                return card_id + "|" + timestamp + "|" + nonce_str + "|" + sign + "|" + wxcardConfig;
            }
        }
        return "";
    }

    /// <summary>
    /// 核销卡券
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public string DestoryWxCard(HttpContext c)
    {
        string card_id = string.IsNullOrEmpty(c.Request["cardid"]) ? "" : c.Request["cardid"].ToString();
        string code = string.IsNullOrEmpty(c.Request["cardno"]) ? "" : c.Request["cardno"].ToString();
        Log.D("DestoryWxCard-card_id:" + card_id + ",code:" + code,c);
        return response.Success(new WxCard().DestroyCode(Get_Access_Token(c), card_id, code));
    }

    /// <summary>
    /// 获取微信卡券金额
    /// </summary>
    /// <param name="Card_ID"></param>
    /// <returns></returns>
    public int GetWxCardFee(string Card_ID)
    {
        string[] w = (wxcardConfig + ",").Split(',');
        foreach (string ws in w)
        {
            if (ws.Length > 0)
            {
                if (ws.Split('@')[0] == Card_ID)
                {
                    return Convert.ToInt16(ws.Split('@')[1]);
                }
            }
        }
        return 0;
    }
    #endregion
}