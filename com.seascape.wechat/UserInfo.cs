using com.seascape.tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.seascape.wechat
{
    public class UserInfo
    {
        static UserInfo userinfo;

        public static UserInfo getUserInfo(string access_token,string openid)
        {
            userinfo = new UserInfo();
            try
            {
                string requestUrl = "https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openid + "&lang=zh_CN";
                string webRequet = BasicTool.webRequest(requestUrl);
                userinfo = LitJson.JsonMapper.ToObject<UserInfo>(webRequet);
                userinfo.requestUrl = requestUrl;
                return userinfo;
            }
            catch (Exception ex)
            {
                userinfo.requestUrl = ex.Message;
                return userinfo;
            }
        }

        public static UserInfo getUserInfoByCode(string access_token, string openid,out string Result)
        {
            userinfo = new UserInfo();
            string webResult = "";
            try
            {
                string requestUrl = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + access_token + "&openid=" + openid + "&lang=zh_CN";
                requestUrl = "https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openid + "&lang=zh_CN";
                webResult = BasicTool.webRequest(requestUrl);
                Result = webResult;
                //webResult = "{\"subscribe\":1,\"openid\":\"ozsQauNthXLaMneze4wIdMIwJrYo\",\"nickname\":\"Kevin\",\"sex\":1,\"language\":\"zh_CN\",\"city\":\"太原\",\"province\":\"山西\",\"country\":\"中国\",\"headimgurl\":\"http://wx.qlogo.cn/mmopen/Kwg1Hs1pPD3R7Lia3JMC25AXEsI9X9ziagBdwD6ia9VXgnBt7o2NN4bV8C7ibkZZO92hH2KibT8oLW9dAyn3cQHNAbQ/0\",\"subscribe_time\":1432608497,\"remark\":\"\",\"groupid\":0}";
                userinfo = LitJson.JsonMapper.ToObject<UserInfo>(webResult);
                userinfo.requestUrl = requestUrl;
                return userinfo;
            }
            catch (Exception ex)
            {
                userinfo.requestUrl = ex.Message;
                Result = ex.Message;
                return userinfo;
            }
        }

        public static UserInfo getUserInfoByGlobal(string access_token, string openid, out string Result)
        {
            userinfo = new UserInfo();
            string webResult = "";
            try
            {
                string requestUrl = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + access_token + "&openid=" + openid + "&lang=zh_CN";
                //requestUrl = "https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openid + "&lang=zh_CN";
                webResult = BasicTool.webRequest(requestUrl);
                if (webResult.IndexOf("privilege") == -1)
                {
                    webResult = webResult.Replace("}", ",\"privilege\":[]}");
                }
                if (webResult.IndexOf("tagid_list") == -1)
                {
                    webResult = webResult.Replace("}", ",\"tagid_list\":[]}");
                }
                Result = webResult;
                //webResult = "{\"subscribe\":1,\"openid\":\"ozsQauNthXLaMneze4wIdMIwJrYo\",\"nickname\":\"Kevin\",\"sex\":1,\"language\":\"zh_CN\",\"city\":\"太原\",\"province\":\"山西\",\"country\":\"中国\",\"headimgurl\":\"http://wx.qlogo.cn/mmopen/Kwg1Hs1pPD3R7Lia3JMC25AXEsI9X9ziagBdwD6ia9VXgnBt7o2NN4bV8C7ibkZZO92hH2KibT8oLW9dAyn3cQHNAbQ/0\",\"subscribe_time\":1432608497,\"remark\":\"\",\"groupid\":0}";
                //webResult = "{"subscribe":1,"openid":"oqJ_Zsn5uihUeg_yOGSxWXi9GnHs","nickname":"猫姐","sex":2,"language":"zh_CN","city":"太原","province":"山西","country":"中国","headimgurl":"http://wx.qlogo.cn/mmopen/yWVyZ16008jQ35tpEPHr3l8SdEl2rNzZ5OofahQjxn88wqeT9fzZic9evjTD13jfXTDUN9SZUz1To34OhGTDqF50ZLIlSeaHI/0","subscribe_time":1434442632,"remark":"","groupid":0}
                //webResult = "{"openid":"oqJ_Zsn5uihUeg_yOGSxWXi9GnHs","nickname":"猫姐","sex":2,"language":"zh_CN","city":"太原","province":"山西","country":"中国","headimgurl":"http://wx.qlogo.cn/mmopen/yWVyZ16008jQ35tpEPHr3l8SdEl2rNzZ5OofahQjxn88wqeT9fzZic9evjTD13jfXTDUN9SZUz1To34OhGTDqF50ZLIlSeaHI/0","subscribe_time":1434442632,"remark":"","groupid":0}
                //webResult = "{"openid":"oqJ_Zsn5uihUeg_yOGSxWXi9GnHs","nickname":"猫姐","sex":2,"language":"zh_CN","city":"太原","province":"山西","country":"中国","headimgurl":"http://wx.qlogo.cn/mmopen/yWVyZ16008jQ35tpEPHr3l8SdEl2rNzZ5OofahQjxn88wqeT9fzZic9evjTD13jfXTDUN9SZUz1To34OhGTDqF50ZLIlSeaHI/0","privilege":[]}
                
                userinfo = LitJson.JsonMapper.ToObject<UserInfo>(webResult);
                userinfo.requestUrl = requestUrl;
                return userinfo;
            }
            catch (Exception ex)
            {
                userinfo.requestUrl = ex.Message;
                Result = ex.Message + webResult;
                return userinfo;
            }
        }

        string[] _privilege = new string[]{""};

        public int subscribe { get; set; }
        public string openid { get; set; }
        public string nickname { get; set; }
        public int sex { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public string[] privilege { get { return _privilege; } set { _privilege = value; } }
        public string[] tagid_list { get { return _privilege; } set { _privilege = value; } }
        public string unionid { get; set; }
        public string requestUrl { get; set; }
        public string language { get; set; }
        public Int32 subscribe_time {get;set;}
        public string remark {get;set;}
        public int groupid { get; set; }
    }
}
