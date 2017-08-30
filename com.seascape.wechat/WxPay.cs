using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Xml;

namespace com.seascape.wechat
{
    public class WxPay
    {
        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 随机字符串不长于32位
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 商品描述-商品或支付单简要描述
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 商品详情[否]
        /// </summary>
        public string detail { get; set; }
        /// <summary>
        /// 附加数据，在查询API和支付通知中原样返回，该字段主要用于商户携带订单的自定义数据 [否]
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// 商户订单号-商户系统内部的订单号,32个字符内、可包含字母, 其他说明见商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 总金额--订单总金额，只能为整数，详见支付金额
        /// </summary>
        public int total_fee { get; set; }
        /// <summary>
        /// 终端IP--APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP。
        /// </summary>
        public string spbill_create_ip { get; set; }
        /// <summary>
        /// 通知地址-接收微信支付异步通知回调地址
        /// </summary>
        public string notify_url { get; set; }
        /// <summary>
        /// 交易类型--取值如下：JSAPI，NATIVE，APP，WAP,详细说明见参数规
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// 用户标识--trade_type=JSAPI，此参数必传，用户在商户appid下的唯一标识。下单前需要调用【网页授权获取用户信息】接口获取到用户的Openid。      
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 商户密钥
        /// </summary>
        public string KeyValue { get; set; }

        /// <summary>
        /// 微信支付统一下单接口--prepay_id
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        public string Get_prepay_id(WxPay w,out string errMsg)
        {
            string prepay_id = "";
            string KeyValue = w.KeyValue;
            w.KeyValue = "";
            string[] Arr = new WxTool().ConvertToArr(w);
            sign = new WxTool().Sign_Pay(Arr,KeyValue);

            StringBuilder s = new StringBuilder();
            s.Append("<xml>");
            s.Append("   <appid>" + w.appid + "</appid>");
            s.Append("   <attach>" + w.attach + "</attach>");
            s.Append("   <body>" + w.body + "</body>");
            s.Append("   <detail>" + w.detail + "</detail>");
            s.Append("   <mch_id>" + w.mch_id + "</mch_id>");
            s.Append("   <nonce_str>" + w.nonce_str + "</nonce_str>");
            s.Append("   <notify_url>" + w.notify_url + "</notify_url>");
            s.Append("   <openid>" + w.openid + "</openid>");
            s.Append("   <out_trade_no>" + w.out_trade_no + "</out_trade_no>");
            s.Append("   <spbill_create_ip>" + w.spbill_create_ip + "</spbill_create_ip>");
            s.Append("   <total_fee>" + w.total_fee + "</total_fee>");
            s.Append("   <trade_type>JSAPI</trade_type>");
            s.Append("   <sign>" + sign + "</sign>");
            s.Append("</xml>");

            string GetUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            string Temp = new WxTool().webRequest(GetUrl, s.ToString());
            errMsg = Temp;
            XmlDocument Xml = new XmlDocument();
            Xml.LoadXml(Temp);

            string return_code = Xml.DocumentElement.SelectSingleNode("return_code").InnerText;
            if (return_code == "SUCCESS")
            {
                try
                {
                    prepay_id = Xml.DocumentElement.SelectSingleNode("prepay_id").InnerText;
                }
                catch
                {
                    prepay_id = "";
                }
            }

            return prepay_id;
        }

        /// <summary>
        /// 生成JSAPI支付签名
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public WxPayConfig Get_Config_Pay(string package, string appid, string KeyValue)
        {
            try
            {
                Random r = new Random();
                int nonce = r.Next(10000, 99999);
                DateTime t = new DateTime(1970, 1, 1);  //得到1970年的时间戳
                long timestamp = (DateTime.UtcNow.Ticks - t.Ticks) / 10000000;  //注意这里有时区问题，用now就要减掉8个小时
                string[] Arr = { "signType=MD5", "appId=" + appid, "package=" + package, "timeStamp=" + timestamp.ToString(), "nonceStr=" + nonce.ToString() };
                string signature = new WxTool().Sign_Pay(Arr, KeyValue);
                //Config = "{\"signature\":" + signature + ",\"timestamp\":" + timestamp + ",\"nonce\":" + nonce + ",\"appid\":" + appid + "}";
                WxPayConfig Config = new WxPayConfig { appid = appid, signature = signature, nonce = nonce, package = package, timestamp = timestamp };
                return Config;
            }
            catch
            {
                return null;
            }
        }

        public class WxPayConfig
        {
            public string signature { get; set; }
            public long timestamp { get; set; }
            public int nonce { get; set; }
            public string appid { get; set; }
            public string package { get; set; }
        }
    }
}
