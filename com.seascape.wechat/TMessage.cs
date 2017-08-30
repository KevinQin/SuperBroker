using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;

namespace com.seascape.wechat
{
    public class TMessage
    {
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public string Send_TemplateMsg(TMessage t,string access_token)
        {
            string TemplateMsgRsa = "";
            string GetUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + access_token;
            try
            {
                string data = JsonMapper.ToJson(t);
                //string JsonStr = com.seascape.tools.BasicTool.webRequestForJson(GetUrl,data,null);
                string JsonStr = new WxTool().webRequest(GetUrl, data);
                JsonData jd = JsonMapper.ToObject(JsonStr);
                if (jd["errcode"].ToString() == "0")
                {
                    TemplateMsgRsa = "OK";
                }
                else
                {
                    TemplateMsgRsa = jd["errmsg"].ToString();
                }
            }
            catch (Exception e)
            {

            }
            return  TemplateMsgRsa;
        }

        /// <summary>
        /// 接收用户的OpenID
        /// </summary>
        public string touser { get; set; }
        /// <summary>
        /// 模板消息ID
        /// </summary>
        public string template_id { get; set; }
        /// <summary>
        /// 模板消息连接地址
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 消息头部底色
        /// </summary>
        public string topcolor { get; set; }
        /// <summary>
        /// 消息体对象
        /// </summary>
        public object data { get; set; }
    }
    

    public class TM_Item
    {
        public string value { get; set; }

        private string _color = "#000";
        public string color { get { return _color; } set { _color=value; } }
    }
}
