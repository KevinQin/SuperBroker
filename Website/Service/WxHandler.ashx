<%@ WebHandler Language="C#" Class="WxHandler" %>

using System;
using System.Web;
using System.IO;
using System.Text;
using System.Linq;
using com.seascape.wechat;
using com.superbroker.data;
using com.superbroker.model;

public class WxHandler : IHttpHandler {

    HttpContext _c = null;
    string postStr = "";
    //微信公众号相关
    static string APPID = com.seascape.tools.BasicTool.GetConfigPara("appid");
    static string SECRET = com.seascape.tools.BasicTool.GetConfigPara("secret");
    static string TOKEN = com.seascape.tools.BasicTool.GetConfigPara("token");
    static string ENCODINGAESKEY=com.seascape.tools.BasicTool.GetConfigPara("aeskey");
    static bool ISAES = false;
    //接收参数
    string verifySig="", verifyMsgSig = "", verifyTimeStamp = "", verifyNonce = "", verifyEchoStr = "",verifyEncryptType="";
    string source = "0";
    int QRCode = 0;
    static string logFile = "";
    //默认代金券信息
    static int voucherFee = 120;
    static string voucherS = "R";
    static int isVoucher = 0;
    static string BaseUrl = "http://"+ com.seascape.tools.BasicTool.GetConfigPara("baseurl") + "/";

    public void ProcessRequest(HttpContext c)
    {
        _c = c;
        Log.F(c.Request.RawUrl,c);
        source =  string.IsNullOrEmpty(c.Request["source"]) ? "0" : c.Request["source"].ToString();
        verifySig = c.Request["signature"];
        verifyTimeStamp = c.Request["timestamp"];
        verifyNonce = c.Request["nonce"];
        verifyEchoStr = c.Request["echostr"];
        try
        {//明文模式下没有该参数
            verifyMsgSig = c.Request["msg_signature"];
            verifyEncryptType = c.Request["encrypt_type"];
        }catch{ }
        var method = c.Request.HttpMethod.ToUpper();
        if (c.Request.HttpMethod.ToUpper() == "POST")
        {
            System.IO.Stream s = c.Request.InputStream;
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            postStr = System.Text.Encoding.UTF8.GetString(b);
            if (!string.IsNullOrEmpty(postStr))
            {
                ResponseMsg(postStr, c);
            }
        }
        else if (c.Request.HttpMethod.ToUpper() == "GET") {
            ValidUrl();
        }
    }

    public void ResponseWrite(string msg) {
        if (ISAES) {
            var wxcpt = new MsgCrypt(TOKEN, ENCODINGAESKEY, APPID);
            var data = "";
            wxcpt.EncryptMsg(msg, Common.ConvertDateTimeInt(DateTime.Now).ToString(),verifyNonce, ref data);
            msg = data;
        }
        _c.Response.Write(msg);
        _c.Response.End();
    }

    public void ResponseMsg(string postStr,HttpContext c)
    {
        //是否需要解密
        var data="";
        if (verifyEncryptType == "aes") {
            ISAES = true;
            var ret = new MsgCrypt(TOKEN, ENCODINGAESKEY, APPID);
            int r = ret.DecryptMsg(verifyMsgSig, verifyTimeStamp, verifyNonce, postStr, ref data);
            if (r != 0) {
                Log.F("消息解密失败",c);
                return;
            }
            postStr = data;
        }
        Log.F("responseMsg:-------" + postStr,c);
        try
        {
            BaseMessage msg = MessageFactory.CreateMessage(postStr);
            if (msg.MsgType == MsgType.TEXT)
            {
                ResponseWrite(msg.ResText("您好，您的信息已经收到"));
            }
            else if (msg.MsgType == MsgType.IMAGE)
            {
                ResponseWrite(msg.ResPicture(new Picture() { PictureUrl = "/app/images/1001.jpg" }, BaseUrl));
            }
            else if (msg.MsgType == MsgType.EVENT)
            {
                EventMessage emsg = (EventMessage)msg;
                string openId = emsg.FromUserName;
                if (emsg.Event == EVENT.SUBSCRIBE)
                {
                    try
                    {
                        QRCode = Convert.ToInt32(((SubEventMessage)emsg).EventKey);
                    }
                    catch { QRCode = 0; }
                    CheckMember(openId);
                    ResponseWrite(msg.ResText("感谢您的关注"));
                }
                else if (emsg.Event == EVENT.SCAN)
                {
                    try
                    {
                        QRCode = Convert.ToInt32(((ScanEventMessage)emsg).EventKey);
                    }
                    catch { QRCode = 0; }
                    CheckMember(openId);
                    ResponseWrite(msg.ResText("欢迎再次回来"));
                }
                else if (emsg.Event == EVENT.UNSUBSCRIBE) {
                    MemberUnFllow(openId);
                }
            }
        }
        catch (Exception ex) {
            Log.F(ex.Message,c);
        }
    }

    /// <summary>
    /// 不再关注
    /// </summary>
    /// <param name="openId"></param>
    /// <returns></returns>
    public bool MemberUnFllow(string openId) {
        bool isSuccess = false;
        Log.F("UnFllow Member:" + openId,_c);
        if (!string.IsNullOrEmpty(openId))
        {
            Member m = new DMember().Get(openId);
            if (m != null) {
                m.IsFllow = false;
                isSuccess= new DMember().Update(m);
            }
            Log.F("UnFllow Member:" + isSuccess,_c);
        }
        return isSuccess;
    }

    /// <summary>
    /// 检测用户是否已存储至数据库，并自动存储
    /// </summary>
    /// <param name="openId"></param>
    /// <returns></returns>
    public bool CheckMember(string openId) {
        bool IsNewMember = false;
        Log.F("Check Member:" + openId,_c);
        if (!string.IsNullOrEmpty(openId)) {
            Member m= new DMember().Get(openId);
            if (m == null)
            {
                Log.F("Check Member 01:new member", _c);
                IsNewMember = true;
                string result = "";
                UserInfo ui = UserInfo.getUserInfoByGlobal(WeChat.GetAccessToken(_c), openId, out result);
                Log.F("[" + openId + "]GetUserInfoByGlobal=" + result, _c);
                if (ui != null && !string.IsNullOrEmpty(ui.nickname))
                {
                    Member member = new Member()
                    {
                        AddOn = DateTime.Now,
                        Country = ui.country,
                        Province = ui.province,
                        City = ui.city,
                        Gender = ui.sex,
                        Memo = "",
                        Mobile = "",
                        Name = "",
                        NickName = ui.nickname,
                        OpenId = openId,
                        UnionId = "",
                        IsFllow = true,
                        QrCode = QRCode,
                        Source = source,
                        PhotoUrl = ui.headimgurl
                    };
                    int Id = 0;
                    bool isSuccess= new DMember().Add(out Id, member);
                    Log.F("Check Member-Add Member:uid["+ isSuccess +":"+ Id +"]", _c);
                }
            }
            else {
                m.IsFllow = true;
                new DMember().Update(m);
                Log.F("Check Member 01:old member",_c);
            }
        }
        return IsNewMember;
    }

    public bool ValidUrl()
    {
        if (CheckSignature())
        {
            if (!string.IsNullOrEmpty(verifyEchoStr))
            {
                ResponseWrite(verifyEchoStr);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 验证微信签名
    /// </summary>
    /// * 将token、timestamp、nonce三个参数进行字典序排序
    /// * 将三个参数字符串拼接成一个字符串进行sha1加密
    /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
    /// <returns></returns>
    public bool CheckSignature()
    {
        string[] ArrTmp = new[]{ TOKEN, verifyTimeStamp, verifyNonce }.OrderBy(z=>z).ToArray();
        string tmpStr = string.Join("", ArrTmp);
        tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
        tmpStr = tmpStr.ToLower();
        Log.F(tmpStr,_c);
        if (tmpStr == verifySig)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 发送模板消息
    /// </summary>
    /// <param name="o"></param>
    /// <param name="key"></param>
    /// <param name="openid"></param>
    /// <param name="url"></param>
    public void SendTemplateMessage(HttpContext c, object o, string key, string openid, string url, string MsgContent, string source)
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
            new DTemplateMsg().Add(out id, tmp);
        }
        catch { }
        new TMessage().Send_TemplateMsg(t, WeChat.GetAccessToken(c));
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}