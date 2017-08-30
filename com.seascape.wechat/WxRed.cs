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
    /*
        随机字符串  nonce_str  是  5K8264ILTKCH16CQ2502SI8ZNMTM67VS  String(32)  随机字符串，不长于32位 
        签名  sign  是  C380BEC2BFD727A4B6845133519F3AD6  String(32)  详见签名生成算法 
        商户订单号  mch_billno  是  10000098201411111234567890  String(28)  商户订单号（每个订单号必须唯一） 组成： mch_id+yyyymmdd+10位一天内不能重复的数字。 接口根据商户订单号支持重入， 如出现超时可再调用。 
        商户号  mch_id  是  10000098  String(32)  微信支付分配的商户号 
        子商户号  sub_mch_id  否  10000090  String(32)  微信支付分配的子商户号，受理模式下必填 
        公众账号appid  wxappid  是  wx8888888888888888  String(32)  商户appid 
        提供方名称  nick_name  是  天虹百货  String(32)  提供方名称 
        商户名称  send_name  是  天虹百货  String(32)  红包发送者名称  
        用户openid re_openid  是  oxTWIuGaIt6gTKsQRLau2M0yL16E  String(32)  接受收红包的用户 用户在wxappid下的openid  
        付款金额  total_amount  是  1000  int  付款金额，单位分 
        最小红包金额  min_value  是  1000  int  最小红包金额，单位分 
        最大红包金额  max_value  是  1000  int  最大红包金额，单位分 （ 最小金额等于最大金额： min_value=max_value =total_amount） 
        红包发放总人数  total_num  是  1  int  红包发放总人数 total_num=1 
        红包祝福语  wishing  是  感谢您参加猜灯谜活动，祝您元宵节快乐！  String(128)  红包祝福语 
        Ip地址  client_ip  是  192.168.0.1  String(15)  调用接口的机器Ip地址 
        活动名称  act_name  是  猜灯谜抢红包活动  String(32)  活动名称  
        备注  remark  是  猜越多得越多，快来抢！  String(256)  备注信息 
        商户logo的url  logo_imgurl  否  https://wx.gtimg.com/mch/img/ico-logo.png  String(128)  商户logo的url 
        分享文案  share_content  否  快来参加猜灯谜活动  String(256)  分享文案 
        分享链接  share_url  否  http://www.qq.com  String(128)  分享链接 
        分享的图片  share_imgurl  否  https://wx.gtimg.com/mch/img/ico-logo.png  String(128)  分享的图片url 
    */
    public class WxRed
    {
        //public static string mchID = "1240117802";
        //public static string wxAppid = "wx2d9c722d8af82106";
        //public static string clientIP = BasicTool.GetConfigPara("client_ip");
        public static string KeyValue = BasicTool.GetConfigPara("KeyValue");
        /// <summary>
        /// 随机字符串32位
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 商户订单号 
        /// </summary>
        public string mch_billno { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 子商户号
        /// </summary>
        public string sub_mch_id { get; set; }
        /// <summary>
        /// 公众账号appid
        /// </summary>
        public string wxappid { get; set; }
        /// <summary>
        /// 提供方名称
        /// </summary>
        public string nick_name { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>
        public string send_name { get; set; }
        /// <summary>
        /// 用户openid
        /// </summary>
        public string re_openid { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public int total_amount { get; set; }
        /// <summary>
        /// 最小红包金额
        /// </summary>
        public int min_value { get; set; }
        /// <summary>
        /// 最大红包金额
        /// </summary>
        public int max_value { get; set; }
        /// <summary>
        /// 红包发放总人数
        /// </summary>
        public int total_num { get; set; }
        /// <summary>
        /// 红包祝福语
        /// </summary>
        public string wishing { get; set; }
        /// <summary>
        /// Ip地址
        /// </summary>
        public string client_ip { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        public string act_name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 商户logo的url
        /// </summary>
        public string logo_imgurl { get; set; }
        /// <summary>
        /// 分享文案
        /// </summary>
        public string share_content { get; set; }
        /// <summary>
        /// 分享链接
        /// </summary>
        public string share_url { get; set; }
        /// <summary>
        /// 分享的图片
        /// </summary>
        public string share_imgurl { get; set; }

        /// <summary>
        /// 发送红包
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="RedNum"></param>
        /// <returns></returns>
        public string SendRed(WxRed r,out string m)
        {
            string Rsa = "";
            m = "";
            r.act_name = "海景快修";
            r.logo_imgurl = "http://fix.4009990351.com/V2/images/logo.png";
            r.nick_name = "山西海景科技有限公司";
            r.remark = "手机上门维修";
            r.send_name = "海景快修";
            r.share_content = "手机上门维修";
            r.share_imgurl = "";
            r.share_url = "";
            r.sign = "";
            r.sub_mch_id = "";
            r.wishing = "合作共赢";
            r.nonce_str = BasicTool.MD5(r.mch_billno).ToUpper();

            string[] Arr = { "act_name=" + r.act_name, "client_ip=" + r.client_ip, "logo_imgurl=" + r.logo_imgurl, "max_value=" + r.max_value, "mch_billno=" + r.mch_billno, "mch_id=" + r.mch_id, "min_value=" + r.min_value, "nick_name=" + r.nick_name, "nonce_str=" + r.nonce_str, "re_openid=" + r.re_openid, "send_name=" + r.send_name, "remark=" + r.remark, "share_content=" + r.share_content, "total_amount=" + r.total_amount, "total_num=" + r.total_num, "wishing=" + r.wishing, "wxappid=" + r.wxappid };
            Array.Sort(Arr);
            string Temp = string.Join("&", Arr);
            Temp = Temp + "&key=" + KeyValue;
            //r.sign = BasicTool.MD5(Temp).ToUpper();
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(Temp));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            r.sign = sb.ToString().ToUpper();

            sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<sign>" + r.sign + "</sign>");
            sb.Append("<mch_billno>" + r.mch_billno + "</mch_billno>");
            sb.Append("<mch_id>" + r.mch_id + "</mch_id>");
            sb.Append("<wxappid>" + r.wxappid + "</wxappid>");
            sb.Append("<nick_name><![CDATA[" + r.nick_name + "]]></nick_name>");
            sb.Append("<send_name><![CDATA[" + r.send_name + "]]></send_name>");
            sb.Append("<re_openid>" + r.re_openid + "</re_openid>");
            sb.Append("<total_amount>" + r.total_amount + "</total_amount>");
            sb.Append("<min_value>" + r.min_value + "</min_value>");
            sb.Append("<max_value>" + r.max_value + "</max_value>");
            sb.Append("<total_num>" + r.total_num + "</total_num>");
            sb.Append("<wishing><![CDATA[" + r.wishing + "]]></wishing>");
            sb.Append("<client_ip>" + r.client_ip + "</client_ip>");
            sb.Append("<act_name><![CDATA[" + r.act_name + "]]></act_name>");
            sb.Append("<remark><![CDATA[" + r.remark + "]]></remark>");
            sb.Append("<logo_imgurl>" + r.logo_imgurl + "</logo_imgurl>");
            sb.Append("<share_content><![CDATA[" + r.share_content + "]]></share_content>");
            sb.Append("<share_url>" + r.share_url + "</share_url>");
            sb.Append("<share_imgurl>" + r.share_imgurl + "</share_imgurl>");
            sb.Append("<nonce_str>" + r.nonce_str + "</nonce_str>");
            sb.Append("</xml>");

            string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";
            string cert = BasicTool.GetConfigPara("certFile");// "E:/2015/微信/技术/WeinXin/bin/cert/apiclient_cert.p12"; 
            string password = r.mch_id;
            Rsa = new WxTool().webRequest(url, sb.ToString(), cert, password);
            m = Rsa;
            try
            {
                XmlDocument Xml = new XmlDocument();
                Xml.LoadXml(Rsa);

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
            //获取红包发送结果
            return Rsa;
        }

   }

    public class RedResp
    {
        /// <summary>
        /// 返回状态码SUCCESS/FAIL 此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断 
        /// </summary>
        public string return_code { get; set; }
        /// <summary>
        /// 返回信息，如非空，为错误原因 签名失败 参数格式校验错误 
        /// </summary>
        public string return_msg { get; set; }

        public string sign { get; set; }
        public string result_code { get; set; }
        public string err_code { get; set; }
        public string err_code_des { get; set; }

        public string mch_billno { get; set; }
        public string mch_id { get; set; }
        public string wxappid { get; set; }
        public string re_openid { get; set; }
        public int total_amount { get; set; }
    }
}
