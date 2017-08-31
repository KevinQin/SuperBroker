using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace com.seascape.wechat
{
    public class BaseMsg
    {
        /// <summary>
        /// 发送者标识
        /// </summary>
        public string FromUser { get; set; }
        /// <summary>
        /// 消息表示。普通消息时，为msgid，事件消息时，为事件的创建时间
        /// </summary>
        public string MsgFlag { get; set; }
        /// <summary>
        /// 添加到队列的时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
    /// <summary>
    /// 微信接入参数
    /// </summary>
    public class EnterParam
    {
        /// <summary>
        /// 是否加密
        /// </summary>
        public bool IsAes { get; set; }
        /// <summary>
        /// 接入token
        /// </summary>
        public string token { get; set; }
        /// <summary>
        ///微信appid
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 加密密钥
        /// </summary>
        public string EncodingAESKey { get; set; }
    }

    public abstract class BaseMessage
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public MsgType MsgType { get; set; }

        public virtual void ResponseNull(HttpContext c)
        {
           c.Response.Write("");
        }

        public virtual string ResText(string content)
        {
            StringBuilder resxml = new StringBuilder(string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime>", FromUserName, ToUserName, Common.ConvertDateTimeInt(DateTime.Now)));
            resxml.AppendFormat("<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[{0}]]></Content><FuncFlag>0</FuncFlag></xml>", content);
            return resxml.ToString();
        }
        /// <summary>
        /// 回复消息(音乐)
        /// </summary>
        public string ResMusic(Music mu,string baseurl)
        {
            StringBuilder resxml = new StringBuilder(string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime>", FromUserName, ToUserName, Common.ConvertDateTimeInt(DateTime.Now)));
            resxml.Append(" <MsgType><![CDATA[music]]></MsgType>");
            resxml.AppendFormat("<Music><Title><![CDATA[{0}]]></Title><Description><![CDATA[{1}]]></Description>", mu.Title, mu.Description);
            resxml.AppendFormat("<MusicUrl><![CDATA[http://{0}{1}]]></MusicUrl><HQMusicUrl><![CDATA[http://{2}{3}]]></HQMusicUrl></Music><FuncFlag>0</FuncFlag></xml>", baseurl, mu.MusicUrl, baseurl, mu.HQMusicUrl);
            return resxml.ToString();
        }
        public string ResVideo(Video v)
        {
            StringBuilder resxml = new StringBuilder(string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime>", FromUserName, ToUserName, Common.ConvertDateTimeInt(DateTime.Now)));
            resxml.Append(" <MsgType><![CDATA[video]]></MsgType>");
            resxml.AppendFormat("<Video><MediaId><![CDATA[{0}]]></MediaId>", v.media_id);
            resxml.AppendFormat("<Title><![CDATA[{0}]]></Title>", v.title);
            resxml.AppendFormat("<Description><![CDATA[{0}]]></Description></Video></xml>", v.description);
            return resxml.ToString();
        }

        /// <summary>
        /// 回复消息(图片)
        /// </summary>
        public string ResPicture(Picture pic, string domain)
        {
            StringBuilder resxml = new StringBuilder(string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime>", FromUserName, ToUserName, Common.ConvertDateTimeInt(DateTime.Now)));
            resxml.Append(" <MsgType><![CDATA[image]]></MsgType>");
            resxml.AppendFormat("<PicUrl><![CDATA[{0}]]></PicUrl></xml>", domain + pic.PictureUrl);
            return resxml.ToString();
        }

        /// <summary>
        /// 回复消息（图文列表）
        /// </summary>
        /// <param name="art"></param>
        public string ResArticles(List<Articles> art,string baseurl)
        {
            StringBuilder resxml = new StringBuilder(string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime>", FromUserName, ToUserName, Common.ConvertDateTimeInt(DateTime.Now)));
            resxml.AppendFormat("<MsgType><![CDATA[news]]></MsgType><ArticleCount>{0}</ArticleCount><Articles>", art.Count);
            for (int i = 0; i < art.Count; i++)
            {
                resxml.AppendFormat("<item><Title><![CDATA[{0}]]></Title>  <Description><![CDATA[{1}]]></Description>", art[i].Title, art[i].Description);
                resxml.AppendFormat("<PicUrl><![CDATA[{0}]]></PicUrl><Url><![CDATA[{1}]]></Url></item>", art[i].PicUrl.Contains("http://") ? art[i].PicUrl : "http://" + baseurl + art[i].PicUrl, art[i].Url.Contains("http://") ? art[i].Url : "http://" +baseurl + art[i].Url);
            }
            resxml.Append("</Articles><FuncFlag>0</FuncFlag></xml>");
            return resxml.ToString();
        }

        /// <summary>
        /// 多客服转发
        /// </summary>
        public string ResDKF()
        {
            StringBuilder resxml = new StringBuilder();
            resxml.AppendFormat("<xml><ToUserName><![CDATA[{0}]]></ToUserName>", FromUserName);
            resxml.AppendFormat("<FromUserName><![CDATA[{0}]]></FromUserName><CreateTime>{1}</CreateTime>", ToUserName, CreateTime);
            resxml.AppendFormat("<MsgType><![CDATA[transfer_customer_service]]></MsgType></xml>");
            return resxml.ToString();
        }
        /// <summary>
        /// 多客服转发如果指定的客服没有接入能力(不在线、没有开启自动接入或者自动接入已满)，该用户会一直等待指定客服有接入能力后才会被接入，而不会被其他客服接待。建议在指定客服时，先查询客服的接入能力指定到有能力接入的客服，保证客户能够及时得到服务。
        /// </summary>
        /// <param name="KfAccount">多客服账号</param>
        public string ResDKF( string KfAccount)
        {
            StringBuilder resxml = new StringBuilder();
            resxml.AppendFormat("<xml><ToUserName><![CDATA[{0}]]></ToUserName>", FromUserName);
            resxml.AppendFormat("<FromUserName><![CDATA[{0}]]></FromUserName><CreateTime>{1}</CreateTime>", ToUserName, CreateTime);
            resxml.AppendFormat("<MsgType><![CDATA[transfer_customer_service]]></MsgType><TransInfo><KfAccount>{0}</KfAccount></TransInfo></xml>", KfAccount);
            return resxml.ToString();
        }
    }

    public class TextMessage : BaseMessage
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public string MsgId { get; set; }
    }

    public class ImgMessage : BaseMessage
    {
        /// <summary>
        /// 图片路径
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// 媒体ID
        /// </summary>
        public string MediaId { get; set; }
    }

    public class VoiceMessage : BaseMessage
    {
        /// <summary>
        /// 缩略图ID
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// 格式
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// 媒体ID
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 语音识别结果
        /// </summary>
        public string Recognition { get; set; }
    }

    public class VideoMessage : BaseMessage
    {
        /// <summary>
        /// 缩略图ID
        /// </summary>
        public string ThumbMediaId { get; set; }
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// 媒体ID
        /// </summary>
        public string MediaId { get; set; }

    }

    public class LinkMessage : BaseMessage
    {
        /// <summary>
        /// 缩略图ID
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }
    }

    public class LocationMessage : BaseMessage
    {
        /// <summary>
        /// 经度
        /// </summary>
        public string LocationX { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string LocationY { get; set; }
        /// <summary>
        /// 缩放
        /// </summary>
        public string Scale { get; set; }
        /// <summary>
        /// 消息ID
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// 位置信息
        /// </summary>
        public string Lable { get; set; }
    }

    public class EventMessage:BaseMessage {
        public EVENT Event{ get;set; }
    }

    public class SubEventMessage : EventMessage
    {
        private string _eventkey;
        /// <summary>
        /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值（已去掉前缀，可以直接使用）
        /// </summary>
        public string EventKey
        {
            get { return _eventkey; }
            set { _eventkey = value.Replace("qrscene_", ""); }
        }
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }
    }

    /// <summary>
    /// 扫描带参数的二维码实体
    /// </summary>
    public class ScanEventMessage : EventMessage
    {

        /// <summary>
        /// 事件KEY值，是一个32位无符号整数，即创建二维码时的二维码scene_id
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }
    }

    /// <summary>
    /// 上报地理位置实体
    /// </summary>
    public class LocationEventMessage : EventMessage
    {
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 地理位置精度
        /// </summary>
        public string Precision { get; set; }
    }

    /// <summary>
    /// 普通菜单事件，包括click和view
    /// </summary>
    public class NormalMenuEventMessage : EventMessage
    {
        /// <summary>
        /// MENU ID
        /// </summary>
        public string MenuId { get; set; }
        /// <summary>
        /// 事件KEY值，设置的跳转URL
        /// </summary>
        public string EventKey { get; set; }
    }

    /// <summary>
    /// 菜单扫描事件
    /// </summary>
    public class ScanMenuEventMessage : EventMessage
    {
        /// <summary>
        /// 事件KEY值
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 扫码类型。qrcode是二维码，其他的是条码
        /// </summary>
        public string ScanType { get; set; }
        /// <summary>
        /// 扫描结果
        /// </summary>
        public string ScanResult { get; set; }
    }

    /// <summary>
    /// 消息类型枚举
    /// </summary>
    public enum MsgType
    {
        /// <summary>
        ///文本类型
        /// </summary>
        TEXT,
        /// <summary>
        /// 图片类型
        /// </summary>
        IMAGE,
        /// <summary>
        /// 语音类型
        /// </summary>
        VOICE,
        /// <summary>
        /// 视频类型
        /// </summary>
        VIDEO,
        /// <summary>
        /// 地理位置类型
        /// </summary>
        LOCATION,
        /// <summary>
        /// 链接类型
        /// </summary>
        LINK,
        /// <summary>
        /// 事件类型
        /// </summary>
        EVENT
    }

    /*
文本消息
<xml>
 <ToUserName><![CDATA[toUser]]></ToUserName>
 <FromUserName><![CDATA[fromUser]]></FromUserName> 
 <CreateTime>1348831860</CreateTime>
 <MsgType><![CDATA[text]]></MsgType>
 <Content><![CDATA[this is a test]]></Content>
 <MsgId>1234567890123456</MsgId>
 </xml>
 
 图片消息        
 <xml>
 <ToUserName><![CDATA[toUser]]></ToUserName>
 <FromUserName><![CDATA[fromUser]]></FromUserName>
 <CreateTime>1348831860</CreateTime>
 <MsgType><![CDATA[image]]></MsgType>
 <PicUrl><![CDATA[this is a url]]></PicUrl>
 <MediaId><![CDATA[media_id]]></MediaId>
 <MsgId>1234567890123456</MsgId>
 </xml> 
 
语音消息
<xml>
<ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[fromUser]]></FromUserName>
<CreateTime>1357290913</CreateTime>
<MsgType><![CDATA[voice]]></MsgType>
<MediaId><![CDATA[media_id]]></MediaId>
<Format><![CDATA[Format]]></Format>
<MsgId>1234567890123456</MsgId>
</xml>

 视频消息
<xml>
<ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[fromUser]]></FromUserName>
<CreateTime>1357290913</CreateTime>
<MsgType><![CDATA[video]]></MsgType>
<MediaId><![CDATA[media_id]]></MediaId>
<ThumbMediaId><![CDATA[thumb_media_id]]></ThumbMediaId>
<MsgId>1234567890123456</MsgId>
</xml>

地理位置消息
<xml>
<ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[fromUser]]></FromUserName>
<CreateTime>1351776360</CreateTime>
<MsgType><![CDATA[location]]></MsgType>
<Location_X>23.134521</Location_X>
<Location_Y>113.358803</Location_Y>
<Scale>20</Scale>
<Label><![CDATA[位置信息]]></Label>
<MsgId>1234567890123456</MsgId>
</xml>

链接消息
<xml>
<ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[fromUser]]></FromUserName>
<CreateTime>1351776360</CreateTime>
<MsgType><![CDATA[link]]></MsgType>
<Title><![CDATA[公众平台官网链接]]></Title>
<Description><![CDATA[公众平台官网链接]]></Description>
<Url><![CDATA[url]]></Url>
<MsgId>1234567890123456</MsgId>
</xml>
*/
    /// <summary>
    /// 事件类型枚举
    /// </summary>
    public enum EVENT
    {
        /// <summary>
        /// 非事件类型
        /// </summary>
        NOEVENT,
        /// <summary>
        /// 订阅
        /// </summary>
        SUBSCRIBE,
        /// <summary>
        /// 取消订阅
        /// </summary>
        UNSUBSCRIBE,
        /// <summary>
        /// 扫描带参数的二维码
        /// </summary>
        SCAN,
        /// <summary>
        /// 地理位置
        /// </summary>
        LOCATION,
        /// <summary>
        /// 单击按钮
        /// </summary>
        CLICK,
        /// <summary>
        /// 链接按钮
        /// </summary>
        VIEW,
        /// <summary>
        /// 扫码推事件
        /// </summary>
        SCANCODE_PUSH,
        /// <summary>
        /// 扫码推事件且弹出“消息接收中”提示框
        /// </summary>
        SCANCODE_WAITMSG,
        /// <summary>
        /// 弹出系统拍照发图
        /// </summary>
        PIC_SYSPHOTO,
        /// <summary>
        /// 弹出拍照或者相册发图
        /// </summary>
        PIC_PHOTO_OR_ALBUM,
        /// <summary>
        /// 弹出微信相册发图器
        /// </summary>
        PIC_WEIXIN,
        /// <summary>
        /// 弹出地理位置选择器
        /// </summary>
        LOCATION_SELECT,
        /// <summary>
        /// 模板消息推送
        /// </summary>
        TEMPLATESENDJOBFINISH
    }
    /*
关注/取消关注事件
<xml>
<ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[FromUser]]></FromUserName>
<CreateTime>123456789</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[subscribe]]></Event>
</xml>

扫描带参数二维码事件
用户已关注时的事件推送
<xml>
<ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[FromUser]]></FromUserName>
<CreateTime>123456789</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[SCAN]]></Event>
<EventKey><![CDATA[SCENE_VALUE]]></EventKey>
<Ticket><![CDATA[TICKET]]></Ticket>
</xml>

上报地理位置事件
<xml>
<ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[fromUser]]></FromUserName>
<CreateTime>123456789</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[LOCATION]]></Event>
<Latitude>23.137466</Latitude>
<Longitude>113.352425</Longitude>
<Precision>119.385040</Precision>
</xml>

click事件推送的xml数据包：
<xml>
<ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[FromUser]]></FromUserName>
<CreateTime>123456789</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[CLICK]]></Event>
<EventKey><![CDATA[EVENTKEY]]></EventKey>
</xml>
scancode事件的xml数据包如下：
<xml><ToUserName><![CDATA[ToUserName]]></ToUserName>
<FromUserName><![CDATA[FromUserName]]></FromUserName>
<CreateTime>1419265698</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[scancode_push]]></Event>
<EventKey><![CDATA[EventKey]]></EventKey>
<ScanCodeInfo><ScanType><![CDATA[qrcode]]></ScanType>
<ScanResult><![CDATA[http://weixin.qq.com/r/JEy5oRLE0U_urVbC9xk2]]></ScanResult>
</ScanCodeInfo>
</xml>
*/

    public enum MediaType
    {
        /// <summary>
        /// 图片（image）: 1M，支持JPG格式
        /// </summary>
        image,
        /// <summary>
        /// 语音（voice）：2M，播放长度不超过60s，支持AMR\MP3格式
        /// </summary>
        voice,
        /// <summary>
        /// 视频（video）：10MB，支持MP4格式
        /// </summary>
        video,
        /// <summary>
        /// 缩略图（thumb）：64KB，支持JPG格式
        /// </summary>
        thumb
    }

    public class Music
    {
        #region 属性
        /// <summary>
        /// 音乐链接
        /// </summary>
        public string MusicUrl { get; set; }
        /// <summary>
        /// 高质量音乐链接，WIFI环境优先使用该链接播放音乐
        /// </summary>
        public string HQMusicUrl { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        #endregion
    }

    public class Video
    {
        public string title { get; set; }
        public string media_id { get; set; }
        public string description { get; set; }
    }

    public class Picture
    {
        public string media_id { get; set; }
        public string PictureUrl { get; set; }
    }

    public class Articles
    {
        #region 属性
        /// <summary>
        /// 图文消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 图文消息描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 图片链接，支持JPG、PNG格式，较好的效果为大图640*320，小图80*80。
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 点击图文消息跳转链接
        /// </summary>
        public string Url { get; set; }
        #endregion
    }

}

