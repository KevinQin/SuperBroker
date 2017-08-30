using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using com.seascape.tools;

namespace com.seascape.wechat
{
    public class Common
    {
        public static string appid = "";//com.seascape.tools.BasicTool.GetConfigPara("appid");// "wxa5664d5c79e7d6c9";
        public static string secret = "";//com.seascape.tools.BasicTool.GetConfigPara("secret");//"1f115453bef00428173733eeef3b174a";
        public static string access_token;

        public Common(string _appid,string _secret) {
            appid = _appid;
            secret = _secret;
        }

        /*
        grant_type  是  获取access_token填写client_credential  
        appid  是  第三方用户唯一凭证  
        secret  是  第三方用户唯一凭证密钥，即appsecret  
        */
        /// <summary>
        /// 获取access_token
        /// </summary>
        /// <returns></returns>
        public string Get_Access_Token()
        {
            string access_token_Rsa = "";
            //if (access_token == null || access_token.Length == 0)
            {
                string GetUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret;
                try
                {
                    access_token_Rsa = com.seascape.tools.BasicTool.webRequest(GetUrl);
                    JsonData j = JsonMapper.ToObject(access_token_Rsa);
                    access_token_Rsa = j["access_token"].ToString();
                    access_token = access_token_Rsa;
                }
                catch (Exception e)
                {
                    access_token_Rsa = "err:" + e.ToString();
                }
            }
            return access_token_Rsa;
        }

        /// <summary>
        /// 获取临时票据--已有access_token
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public string Get_jsapi_ticket(string access_token,out string r)
        {
            string jsapi_ticket = "";
            string GetUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + access_token + "&type=jsapi";
            try
            {
                string JsonStr = com.seascape.tools.BasicTool.webRequest(GetUrl);
                r = JsonStr;
                JsonData jd = JsonMapper.ToObject(JsonStr);
                if (jd["errcode"].ToString() == "0")
                {
                    jsapi_ticket = jd["ticket"].ToString();
                }
            }
            catch (Exception e)
            {
                r = e.Message;
            }
            return jsapi_ticket;
        }

        
        /// <summary>
        /// JSSDK-通过config接口注入权限验证配置-字符串分割格式 v
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static WxConfig Get_Config_(string url, string jsapiTicket,out string TempS)
        {
            WxConfig Config = new WxConfig();
            Random r = new Random();
            int nonce = r.Next(10000, 99999);
            DateTime t = new DateTime(1970, 1, 1);  //得到1970年的时间戳
            long timestamp = (DateTime.UtcNow.Ticks - t.Ticks) / 10000000;  //注意这里有时区问题，用now就要减掉8个小时
            string jsapi_ticket = jsapiTicket;// new Common().Get_jsapi_ticket();
            string[] Arr = { "jsapi_ticket=" + jsapi_ticket, "timestamp=" + timestamp.ToString(), "noncestr=" + nonce.ToString(), "url=" + url };
            Array.Sort(Arr);
            string TempStr = string.Join("&", Arr);
            TempS = TempStr;
            string signature = OperSha1(TempStr);
            Config.signature = signature;
            Config.timestamp = timestamp;
            Config.nonce = nonce;
            Config.appid = appid;
            //Config = "{signature:'" + signature + "',timestamp:'" + timestamp + "',nonce:'" + nonce + "',appid:'" + appid+"'}";
            return Config;
        }

        public class WxConfig
        {
            public string signature { get; set; }
            public long timestamp { get; set; }
            public int nonce { get; set; }
            public string appid { get; set; }
        }
        
        /// <summary>
        /// 进行SHA1加密
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        public static string OperSha1(string strContent)
        {
            Encoding _encoding = Encoding.Default;
            byte[] bb = _encoding.GetBytes(strContent);

            byte[] result;

            SHA1 sha = new SHA1CryptoServiceProvider();
            result = sha.ComputeHash(bb);
            string strResult = BitConverter.ToString(result);
            strResult = strResult.Replace("-", "");
            strResult = strResult.ToLower();
            return strResult;
        }

        /// <summary>
        /// 提交POST信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string webRequest(string url, string data)
        {
            string html = null;
            WebRequest req = WebRequest.Create(url);
            req.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            req.ContentType = "application/json";
            req.ContentLength = byteArray.Length;
            Stream dataStream = req.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse res = req.GetResponse();

            Stream receiveStream = res.GetResponseStream();
            Encoding encode = Encoding.UTF8;
            StreamReader sr = new StreamReader(receiveStream, encode);
            char[] readbuffer = new char[256];
            int n = sr.Read(readbuffer, 0, 256);
            while (n > 0)
            {
                string str = new string(readbuffer, 0, n);
                html += str;
                n = sr.Read(readbuffer, 0, 256);
            }
            return html;
        }

        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <returns></returns>
        public string GetQR_Code(string access_token_, string FilePath, int SceneID)
        {
            string OrCode = "";
            string GetUrl = " https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + access_token_;
            try
            {
                string ticket = "";
                string body = "{\"action_name\": \"QR_LIMIT_SCENE\",\"action_info\": {\"scene\": {\"scene_id\": " + SceneID + "}}}";
                string JsonStr = webRequest(GetUrl, body);
                JsonData jd = JsonMapper.ToObject(JsonStr);
                if (jd["ticket"].ToString() != "")
                {
                    ticket = jd["ticket"].ToString();
                }

                if (ticket.Length > 0)
                {
                    GetUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + ticket;
                    OrCode = BasicTool.webRequestForFile(GetUrl, FilePath);
                }
            }
            catch (Exception ex)
            {
            }
            return OrCode;
        }

        /// <summary>
        /// 网页获取用户信息--获取临时access_token
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static access_token_UserInfo Get_access_token_UserInfo(string Code)
        {
            access_token_UserInfo access_token_UserInfo = new access_token_UserInfo();
            JsonData jdT = null;
            string GetUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
            GetUrl = string.Format(GetUrl, appid, secret, Code);
            try
            {
                string JsonStr = com.seascape.tools.BasicTool.webRequest(GetUrl);
                JsonData jd = JsonMapper.ToObject(JsonStr);
                jdT = jd;
                if (!string.IsNullOrEmpty(jd["openid"].ToString()))
                {
                    access_token_UserInfo.openid = jd["openid"].ToString();
                    access_token_UserInfo.access_token = jd["access_token"].ToString();
                }
                else
                {
                    access_token_UserInfo.openid = "";
                    access_token_UserInfo.access_token = "";
                    access_token_UserInfo.errcode = jd["errcode"].ToString();
                }
            }
            catch (Exception e)
            {
                access_token_UserInfo.openid = "";
                access_token_UserInfo.access_token = "";
                try
                {
                    if (jdT != null && jdT["errcode"].ToString().Length > 0)
                    {
                        access_token_UserInfo.errcode = jdT["errcode"].ToString();
                    }
                    else
                    {
                        access_token_UserInfo.errcode = "ERR";
                    }
                }
                catch
                {
                }
            }
            return access_token_UserInfo;
        }

    }

    public class access_token_UserInfo
    {
        public string openid { get; set; }
        public string access_token { get; set; }
        public string errcode { get; set; }
    }
}
