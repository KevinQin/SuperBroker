using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seascape.tools;
using System.Xml;

namespace com.seascape.wechat
{
    /// <summary>
    /// 企业给用户付款 
    /// </summary>
    public class WxPayForCop
    {
        //公众账号appid	mch_appid	是	wx8888888888888888	String	商户appid
        public string mch_appid { get; set; }
        //商户号	mchid	是	1900000109	String(32)	微信支付分配的商户号
        public string mchid { get; set; }
        //子商户号	sub_mch_id	否	1900000109	String(32)	微信支付分配的子商户号，受理模式下必填
        //设备号	device_info	否	1.3467E+13	String(32)	微信支付分配的终端设备号
        //随机字符串	nonce_str	是	5K8264ILTKCH16CQ2502SI8ZNMTM67VS	String(32)	随机字符串，不长于32位
        public string nonce_str { get; set; }
        //签名	sign	是	C380BEC2BFD727A4B6845133519F3AD6	String(32)	签名，详见签名算法
        public string sign { get; set; }
        //商户订单号	partner_trade_no	是	1.00001E+25	String	商户订单号，需保持唯一性
        public string partner_trade_no { get; set; }
        //用户openid	openid	是	oxTWIuGaIt6gTKsQRLau2M0yL16E	String	商户appid下，某用户的openid
        public string openid { get; set; }
        //校验用户姓名选项	check_name	是	OPTION_CHECK	String	NO_CHECK：不校验真实姓名 
        public string check_name { get; set; }
        //FORCE_CHECK：强校验真实姓名（未实名认证的用户会校验失败，无法转账） 
        //OPTION_CHECK：针对已实名认证的用户才校验真实姓名（未实名认证用户不校验，可以转账成功）
        //收款用户姓名	re_user_name	可选	马花花	String	收款用户真实姓名。 
        //如果check_name设置为FORCE_CHECK或OPTION_CHECK，则必填用户真实姓名
        //金额	amount	是	10099	Uint64_t	企业付款金额，单位为分
        public int amount { get; set; }
        //企业付款描述信息	desc	是	理赔	String	企业付款操作说明信息。必填。
        public string desc { get; set; }
        //Ip地址	spbill_create_ip	是	192.168.0.1	String(32)	调用接口的机器Ip地址
        public string spbill_create_ip { get; set; }

        /// <summary>
        /// 商户密钥
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 企业向用户付款
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        public bool PayToOpenId(WxPayForCop w)
        {
            bool Result = false;
            string KeyValue = w.key;
            w.key = "";
            string[] Arr = new WxTool().ConvertToArr(w);
            sign = new WxTool().Sign_Pay(Arr, KeyValue);

            StringBuilder s = new StringBuilder();
            s.Append("<xml>");
            s.Append("<mch_appid>"+w.mch_appid+"</mch_appid>");
            s.Append("<mchid>"+w.mchid+"</mchid>");
            s.Append("<nonce_str>"+w.nonce_str+"</nonce_str>");
            s.Append("<partner_trade_no>"+w.partner_trade_no+"</partner_trade_no>");
            s.Append("<openid>"+w.openid+"</openid>");
            s.Append("<check_name>NO_CHECK</check_name>");
            //s.Append("<check_name>OPTION_CHECK</check_name>");
            //s.Append("<re_user_name>张三</re_user_name>");
            s.Append("<amount>"+w.amount+"</amount>");
            s.Append("<desc>"+w.desc+"</desc>");
            s.Append("<spbill_create_ip>"+w.spbill_create_ip+"</spbill_create_ip>");
            s.Append("<sign>"+sign+"</sign>");
            s.Append("</xml>");

            string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";
            string cert = BasicTool.GetConfigPara("certFile");// "E:/2015/微信/技术/WeinXin/bin/cert/apiclient_cert.p12"; 
            string password = w.mchid;
            string Rsa = new WxTool().webRequest(url, s.ToString(), cert, password);
            try
            {
                XmlDocument Xml = new XmlDocument();
                Xml.LoadXml(Rsa);

                string return_code = Xml.DocumentElement.SelectSingleNode("return_code").InnerText;
                if (return_code == "SUCCESS")
                {
                    string result_code = Rsa = Xml.DocumentElement.SelectSingleNode("result_code").InnerText;
                    if (result_code == "SUCCESS")
                    {
                        Result = true;
                    }
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
            catch
            {
                Rsa = "Catch";
            }
            return Result;
        }
    }
}
