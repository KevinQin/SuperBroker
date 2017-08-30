using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;

namespace com.seascape.wechat
{
    public class WxCard
    {
        public string code { get; set; }
        public string card_id { get; set; }

        /// <summary>
        /// 获取用户卡券列表
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public List<WxCard> GetCardList(string openId, string access_token,string card_id,out string cardList)
        {
            List<WxCard> lw = new List<WxCard>();
            string url = "https://api.weixin.qq.com/card/user/getcardlist?access_token=" + access_token;
            string data = "{\"openid\": \"" + openId + "\",\"card_id\": \"" + card_id + "\"}";
            string cards = new WxTool().webRequest(url, data);
            cardList = cards;
            WxCardSrc wc = new WxCardSrc();
            try
            {
                wc = LitJson.JsonMapper.ToObject<WxCardSrc>(cards);
                if (wc != null)
                {
                    if (wc.card_list.Count > 0)
                    {
                        lw = wc.card_list;
                    }
                }

            }
            catch
            {

            }
            return lw;
        }

        /// <summary>
        /// 核销卡券
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="card_id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public string DestroyCode(string access_token, string card_id, string code)
        {
            string Rsa = "ERR";
            code = DeCode(access_token, code);
            if (code.Length > 0)
            {
                string url = "https://api.weixin.qq.com/card/code/consume?access_token=" + access_token;
                string data = data = "{\"code\": \"" + code + "\",\"card_id\": \"" + card_id + "\"}";
                string JsonStr = new WxTool().webRequest(url, data);
                try
                {
                    JsonData jd = JsonMapper.ToObject(JsonStr);
                    if (jd["errcode"].ToString() == "0")
                    {
                        Rsa = "sucess";
                    }
                    else
                    {
                        Rsa = jd["errcode"].ToString() + "][" + jd["errmsg"].ToString();
                    }
                }
                catch
                {

                }
            }
            return Rsa;
        }

        public string DeCode(string access_token, string code)
        {
            string Rsa = "";
            string url = "https://api.weixin.qq.com/card/code/decrypt?access_token=" + access_token;
            string data = "{\"encrypt_code\": \"" + code + "\"}"; //data = "{\"code\": \"" + code + "\",\"card_id\": \"" + card_id + "\"}";
            string JsonStr = new WxTool().webRequest(url, data);
            try
            {
                JsonData jd = JsonMapper.ToObject(JsonStr);
                if (jd["errcode"].ToString() == "0")
                {
                    Rsa = jd["code"].ToString();
                }
            }
            catch
            {

            }
            return Rsa;
        }

        /// <summary>
        /// 微信卡包获取api_ticket
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public string Get_api_ticket(string access_token, out string r)
        {
            string jsapi_ticket = "";
            string GetUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token="+access_token+"&type=wx_card";
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
        /// 签名
        /// </summary>
        /// <param name="code"></param>
        /// <param name="card_id"></param>
        /// <param name="api_ticket"></param>
        /// <param name="timestamps"></param>
        /// <returns></returns>
        public string Get_Signature(string appid,string nonce_str, string card_id, string api_ticket, out long timestamps)
        {
            DateTime t = new DateTime(1970, 1, 1);  //得到1970年的时间戳
            long timestamp = (DateTime.UtcNow.Ticks - t.Ticks) / 10000000;  //注意这里有时区问题，用now就要减掉8个小时
            string[] Arr = { timestamp.ToString(), nonce_str, card_id, api_ticket, appid };
            Array.Sort(Arr);
            string TempStr = string.Join("", Arr);
            string signature = Common.OperSha1(TempStr);
            timestamps = timestamp;
            return signature;
        }
    }

    public class WxCardSrc
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public List<WxCard> card_list { get; set; }
    }
}
