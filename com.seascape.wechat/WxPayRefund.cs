using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seascape.tools;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.IO;
using System.Xml;

namespace com.seascape.wechat
{
    public class WxPayRefund
    {
        /*
        公众账号ID	appid	是	String(32)	wx8888888888888888	微信分配的公众账号ID（企业号corpid即为此appId）
        商户号	mch_id	是	String(32)	1900000109	微信支付分配的商户号
        设备号	device_info	否	String(32)	013467007045764	终端设备号
        随机字符串	nonce_str	是	String(32)	5K8264ILTKCH16CQ2502SI8ZNMTM67VS	随机字符串，不长于32位。推荐随机数生成算法
        签名	sign	是	String(32)	C380BEC2BFD727A4B6845133519F3AD6	签名，详见签名生成算法
        微信订单号	transaction_id	是	String(28)	1217752501201407033233368018	微信订单号
        商户订单号	out_trade_no	是	String(32)	1217752501201407033233368018	商户系统内部的订单号,transaction_id、out_trade_no二选一，如果同时存在优先级：transaction_id> out_trade_no
        商户退款单号	out_refund_no	是	String(32)	1217752501201407033233368018	商户系统内部的退款单号，商户系统内部唯一，同一退款单号多次请求只退一笔
        总金额	total_fee	是	Int	100	订单总金额，单位为分，只能为整数，详见支付金额
        退款金额	refund_fee	是	Int	100	退款总金额，订单总金额，单位为分，只能为整数，详见支付金额
        货币种类	refund_fee_type	否	String(8)	CNY	货币类型，符合ISO 4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型
        操作员	op_user_id	是	String(32)	1900000109	操作员帐号, 默认为商户号
        */
        public int id { get; set; }
        public string appid { get; set; }
        public string mch_id { get; set; }
        public string nonce_str { get; set; }
        public string sign { get; set; }
        public string transaction_id { get; set; }
        public string out_trade_no { get; set; }
        public string out_refund_no { get; set; }
        public int total_fee { get; set; }
        public int refund_fee { get; set; }
        public string op_user_id { get; set; }

        public int enable { get; set; }
        public DateTime addOn { get; set; }
        public string OrderNo { get; set; }
        public string errMsg { get; set; }
        public DateTime refundOn { get; set; }
        public string KeyValue { get; set; }

        public string WxRefund(WxPayRefund w,out string r)
        {
            string Rsa = "";

            string[] Arr = { "appid=" + w.appid, "mch_id=" + w.mch_id, "nonce_str=" + w.nonce_str, "transaction_id=" + w.transaction_id, "out_trade_no=" + w.out_trade_no, "out_refund_no=" + w.out_refund_no, "total_fee=" + w.total_fee, "refund_fee=" + w.refund_fee, "op_user_id=" + w.op_user_id};
            Array.Sort(Arr);
            string Temp = string.Join("&", Arr);
            Temp = Temp + "&key=" + w.KeyValue;
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(Temp));
            var sb_ = new StringBuilder();
            foreach (byte b in bs)
            {
                sb_.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            w.sign = sb_.ToString().ToUpper();

            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<appid>"+w.appid+"</appid>");
            sb.Append("<mch_id>"+w.mch_id+"</mch_id>");
            sb.Append("<nonce_str>" + w.nonce_str + "</nonce_str>");
            sb.Append("<op_user_id>"+w.mch_id+"</op_user_id>");
            sb.Append("<out_refund_no>" + w.out_refund_no + "</out_refund_no>");
            sb.Append("<out_trade_no>" + w.out_trade_no + "</out_trade_no>");
            sb.Append("<refund_fee>" + w.refund_fee + "</refund_fee>");
            sb.Append("<total_fee>" + w.total_fee + "</total_fee>");
            sb.Append("<transaction_id>" + w.transaction_id + "</transaction_id>");
            sb.Append("<sign>"+w.sign+"</sign>");
            sb.Append("</xml>");

            string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
            string cert = BasicTool.GetConfigPara("certFile");// "E:/2015/微信/技术/WeinXin/bin/cert/apiclient_cert.p12"; 
            string password = w.mch_id;
            Rsa = new WxTool().webRequest(url, sb.ToString(), cert, password);
            string res = "";
            try
            {
                XmlDocument Xml = new XmlDocument();
                Xml.LoadXml(Rsa);
                res = Rsa;
                string return_code = Xml.DocumentElement.SelectSingleNode("return_code").InnerText;
                if (return_code == "SUCCESS")
                {
                    Rsa = "0";
                }
                else
                {
                    try
                    {
                        Rsa = Xml.DocumentElement.SelectSingleNode("return_msg").InnerText;
                    }
                    catch
                    {
                        Rsa = "ERR";
                    }
                }
                
                
            }
            catch {
                Rsa = "Catch";
            }
            r = res;
            return Rsa;
        }
    }
}
