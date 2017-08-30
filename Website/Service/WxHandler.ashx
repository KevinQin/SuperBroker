<%@ WebHandler Language="C#" Class="WxHandler" %>

using System;
using System.Web;
using System.IO;
using System.Text;
using com.seascape.wechat;
using com.superbroker.data;
using com.superbroker.model;

public class WxHandler : IHttpHandler {

    HttpContext context = null;
    string postStr = "";
    //微信公众号相关
    public static string appid = com.seascape.tools.BasicTool.GetConfigPara("appid");
    public static string secret = com.seascape.tools.BasicTool.GetConfigPara("secret");

    //默认代金券信息
    public static int voucherFee = 120;
    public static string voucherS = "R";
    public static string source = "0000";
    public static int isVoucher = 0;
    public static string BaseUrl = "http://b.seascapeapp.cn/";
    public static string Token = "seascaoe_weixin_superbroker";

    public void ProcessRequest(HttpContext param_context)
    {
        context = param_context;
        source =  string.IsNullOrEmpty(context.Request["source"]) ? "0000" : context.Request["source"].ToString();
        WriteLog("source:-------" + source);
        OperOpenID(source,param_context);
        //new voucher().SendVoucher(9431, 8647);
        //return;
        //WriteLog("before valid \n");
        //valid();//用于验证       
        /*var echostr = context.Request["echoStr"].ToString();
        if (!string.IsNullOrEmpty(echostr))
        {
            if (checkSignature())
            {
                context.Response.Write(echostr);
                context.Response.End();//推送...不然微信平台无法验证token                
            }
        } */
        //WriteLog("after valid, before post \n");
        if (context.Request.HttpMethod.ToLower() == "post")
        {
            System.IO.Stream s = context.Request.InputStream;
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            postStr = System.Text.Encoding.UTF8.GetString(b);
            //WriteLog("SrcText:" + postStr);
            if (!string.IsNullOrEmpty(postStr))
            {
                responseMsg(postStr, param_context);
            }
        }
    }

    public void valid()
    {
        var echostr = context.Request["echoStr"].ToString();
        if (checkSignature() && !string.IsNullOrEmpty(echostr))
        {
            context.Response.Write(echostr);
            context.Response.End();//推送...不然微信平台无法验证token
        }
    }

    public bool checkSignature()
    {
        var signature = context.Request["signature"].ToString();
        var timestamp = context.Request["timestamp"].ToString();
        var nonce = context.Request["nonce"].ToString();      
        string[] ArrTmp = { Token, timestamp, nonce };
        Array.Sort(ArrTmp);//字典排序
        string tmpStr = string.Join("", ArrTmp);
        tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
        tmpStr = tmpStr.ToLower();
        if (tmpStr == signature)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string GetSha1(System.Collections.Generic.List<string> codelist)
    {
        codelist.Sort();
        var combostr = string.Empty;
        for (int i = 0; i < codelist.Count; i++)
        {
            combostr += codelist[i];
        }
        return EncryptToSHA1(combostr);
    }

    public string EncryptToSHA1(string str)
    {
        System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
        byte[] str1 = System.Text.Encoding.UTF8.GetBytes(str);
        byte[] str2 = sha1.ComputeHash(str1);
        sha1.Clear();
        (sha1 as IDisposable).Dispose();
        return Convert.ToBase64String(str2);
    }

    public void responseMsg(string postStr,HttpContext c)
    {
        System.Xml.XmlDocument postObj = new System.Xml.XmlDocument();
        postObj.LoadXml(postStr);
        WriteLog("responseMsg:-------" + postStr);
        //WriteLog("XmlObj:-------" + postObj);
        string FromUserName = "";
        string ToUserName = "";
        string Content = "";
        string VoucherContent = "";
        string MsgType = "";
        string srcDesp = "";
        int MenuKey = 0;

        string FContent = "";
        string FOpenId = "";
        string FNickName = "";
        string kefu = "test1@test";
        try
        {
            FromUserName = postObj.GetElementsByTagName("FromUserName").Item(0).InnerText;
            ToUserName = postObj.GetElementsByTagName("ToUserName").Item(0).InnerText;
            MsgType = postObj.GetElementsByTagName("MsgType").Item(0).InnerText;
            MsgType = MsgType.ToLower();
        }
        catch(Exception ex)
        {
            WriteLog("Exception" + ex.ToString());
        }
        WriteLog("MsgType:-------" + MsgType);
        switch (MsgType)
        {
            case "text"://文本消息
                Content = "";
                break;
            case "event"://事件
                string Event = postObj.GetElementsByTagName("Event").Item(0).InnerText;
                WriteLog("EventStr" + Event);
                Event = Event.ToLower();
                int QrCode = 0;
                switch (Event)
                {
                    case "subscribe"://关注
                        try
                        {
                            QrCode = Convert.ToInt32(postObj.GetElementsByTagName("EventKey").Item(0).InnerText.Replace("qrscene_", ""));//是否获取二维码
                        }
                        catch
                        {
                            QrCode = 0;
                        }
                        WriteLog("关注来源" + QrCode);
                        Member u = new DMember().Get(FromUserName);
                        Content = "感谢关注全民经纪人平台~";
                        //if(source=="0000")
                        //{
                        Content = "感谢关注全民经纪人平台，From：000";
                        //}
                        if (u == null)
                        {
                            string r = "";
                            com.seascape.wechat.UserInfo ui = com.seascape.wechat.UserInfo.getUserInfoByGlobal(Get_Access_Token(c), FromUserName, out r);
                            Log.D("r"+r,c);
                            if (ui != null && !string.IsNullOrEmpty(ui.nickname))
                            {
                                u = new Member();
                                u.NickName = ui.nickname.Replace("'", "").Replace(@"""", "");
                                u.PhotoUrl = ui.headimgurl;
                                u.Area = ui.country + "|" + ui.province + "|" + ui.city;
                                u.Gender = ui.sex;
                                u.OpenId = ui.openid;
                                u.Mobile = "";
                                u.Name = "";
                                u.AddOn = DateTime.Now;
                                int uid = 0;
                                new DMember().Add(out uid,u);
                                /*
                                if (uid > 0 && isVoucher == 0)
                                {                                   
                                    //送120元代金券--关注人

                                    int VoucherCount = new _Voucher().GetVoucherCount(uid, QrCode);

                                    if (VoucherCount == 0)
                                    {
                                        new _Voucher().SendVoucher(uid, QrCode);
                                        voucherFee = 120;

                                        if (QrCode > 2010)
                                        {
                                            user fuser = new _User().GetUser("", "", QrCode);
                                            if (fuser != null)
                                            {
                                                Content = "飞行出行，10年专注！\n你的好友" + fuser.nickName + "送您的“山西出行”120元代金礼券，已经存入您的私人飞行账户。\n一键生成并分享专属您的2016年度飞行里程图，<a href='https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appid + "&redirect_uri=http%3a%2f%2fhjhk.edmp.cc%2factive%2fa_start.html&response_type=code&scope=snsapi_userinfo&state=00000" + appid + "#wechat_redirect'>点击这里</a>。 \n将这份幸运分享下去，邀请好友关注，立即为TA送出120元飞行代金礼券。\n您更有机会赢取5G超大全国流量包！\n点击“人气排行榜”随时查看排名更新，12月27日活动马上截止。\n送礼券拼人气抢大包，和朋友圈一起躁起来吧！";
                                                string pm = new _User().getUserPM(QrCode);
                                                if (pm.IndexOf("|") > -1)
                                                {
                                                    FContent = fuser.nickName + "，您的好友[" + u.nickName + "]已经收到您送出的“山西出行”120元代金礼券。\n截止目前，一共有" + pm.Split('|')[0] + "个好友通过您的分享码获得总额" + (Convert.ToInt32(pm.Split('|')[0]) * 120) + "元的飞行代金礼券，\n目前您在“人气排行榜”中排" + pm.Split('|')[1] + "名。\n冲进前5名，即可赢取5G超大全国流量包！\n12月27日活动马上截止。\n送礼券拼人气抢大包，让朋友圈继续躁起来吧！";
                                                    FOpenId = fuser.openId;
                                                    FNickName = u.nickName;
                                                }
                                            }
                                        }
                                    }
                               }*/
                            }
                        }
                        break;
                    case "scan"://已关注的扫描二维码
                        Content = "亲，欢迎再次回到全民经纪人";
                        try
                        {
                            QrCode = Convert.ToInt32(postObj.GetElementsByTagName("EventKey").Item(0).InnerText.Replace("SCENE_", ""));//是否获取二维码
                        }
                        catch
                        {
                            QrCode = 0;
                        }
                        //有来源码并且来源码不是老用户
                        /*
                        if (isVoucher == 0)
                        {
                            user u_ = new _User().GetUser(FromUserName, "", 0);
                            if (u_ != null)
                            {
                                int VoucherCount = new _Voucher().GetVoucherCount(u_.id, QrCode);
                                if (VoucherCount == 0)
                                {
                                    new _Voucher().SendVoucher(u_.id, QrCode);
                                    Content = "飞行出行，10年专注！" + srcDesp + voucherFee + "元代金券,已经存入您的私人飞行账户。";
                                }
                            }

                        }        */
                        break;
                    case "click":
                        string key = postObj.GetElementsByTagName("EventKey").Item(0).InnerText.Replace("qrscene_", "");//是否获取二维码
                        WriteLog("click" + key);
                        if (key == "SXCX_GJJP")
                        {
                            MenuKey = 1;
                            kefu = "kf2001@gh_e67ca5c82957";
                        }
                        if (key == "SXCX_JDYD")
                        {
                            MenuKey = 2;
                            kefu = "kf2002@gh_e67ca5c82957";
                        }
                        Content = "正在为您接通客服，请稍后，您也可以直接拨打客服电话4006660000";
                        if (key == "SXCX_BUILD")
                        {
                            Content = "栏目正在建设中......";
                        }
                        if (key == "SXCX_GYLMK")
                        {
                            Content = "海景航空和中国工商银行联合发行海景工银联名信用卡，老客户办理热线0351-8777777";
                        }
                        break;
                }
                break;
        }
        if (MsgType == "text")
        {
            var textpl_ = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName>" +
                         "<FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>" +
                         "<CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[transfer_customer_service]]></MsgType>" +
                         "</xml> ";
            WriteLog("客服消息" + textpl_);
            context.Response.Write(textpl_);
            context.Response.End();
            //context.Response.Write("<xml><ToUserName><![CDATA[" + ToUserName + "]]></ToUserName><FromUserName><![CDATA[" + FromUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[transfer_customer_service]]></MsgType></xml>");
            //context.Response.End();
        }
        if (MsgType == "event" && MenuKey > 0)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + Get_Access_Token(c);
            string content = "{\"touser\":\"" + FromUserName + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"" + Content + "\"}}";
            WriteLog(new WxTool().webRequest(url, content));

            var textpl_ = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName>" +
             "<FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>" +
             "<CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[transfer_customer_service]]></MsgType>" +
             "<TransInfo><KfAccount><![CDATA[" + kefu + "]]></KfAccount></TransInfo>" +
             "</xml> ";
            WriteLog("客服消息" + textpl_);
            context.Response.Write(textpl_);
            context.Response.End();
        }
        var time = DateTime.Now;
        var textpl = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName>" +
                     "<FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>" +
                     "<CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType>" +
                     "<Content><![CDATA[" + Content + "]]></Content><FuncFlag>0</FuncFlag></xml> ";
        WriteLog("客服消息" + textpl);
        context.Response.Write(textpl);
        //下发代金券提醒
        if (VoucherContent.Length > 0)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + Get_Access_Token(c);
            string content = "{\"touser\":\"" + FromUserName + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"" + VoucherContent + "\"}}";
            WriteLog(new WxTool().webRequest(url,content));
        }
        context.Response.End();
    }

    private DateTime UnixTimeToTime(string timeStamp)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }

    private int ConvertDateTimeInt(System.DateTime time)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        return (int)(time - startTime).TotalSeconds;
    }

    private void WriteLog(string strMemo)
    {
        string filename = "D:\\worker\\superbroker\\log\\"+DateTime.Now.ToString("yyMMdd")+".txt";
        strMemo = "[" + DateTime.Now.ToString("MM-dd HH:mm:ss") + "]" + strMemo;
        System.IO.StreamWriter sr = null;
        try
        {
            if (!System.IO.File.Exists(filename))
            {
                sr = System.IO.File.CreateText(filename);
            }
            else
            {
                sr = System.IO.File.AppendText(filename);
            }
            sr.WriteLine(strMemo);
        }
        catch
        {
        }
        finally
        {
            if (sr != null)
                sr.Close();
        }
    }

    /// <summary>
    /// 发送模板消息
    /// </summary>
    /// <param name="o"></param>
    /// <param name="key"></param>
    /// <param name="openid"></param>
    /// <param name="url"></param>
    public void SendTemplateMessage(HttpContext c, object o, string key, string openid, string url, string MsgContent,string source)
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
                MsgBody = LitJson.JsonMapper.ToJson(o),
                MsgUrl = url,
                OpenId = openid,
                AddOn = DateTime.Now,
                MsgContent = MsgContent,
                OrderNo = ""
            };
            int id = -1;
            new DTemplateMsg().Add(out id,tmp);
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
            Access_Token = new Common(appid, secret).Get_Access_Token();
            c.Cache.Add("Global_Access_Token", Access_Token, null, System.DateTime.UtcNow.AddMinutes(100), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
        }
        else
        {
            Access_Token = c.Cache["Global_Access_Token"].ToString();
        }
        return Access_Token;
    }

    public void OperOpenID(string source, HttpContext c)
    {
        appid = com.seascape.tools.BasicTool.GetConfigPara("appid");
        secret = com.seascape.tools.BasicTool.GetConfigPara("secret");
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}