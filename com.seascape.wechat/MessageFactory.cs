using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace com.seascape.wechat
{
    public class MessageFactory
    {
        private static List<BaseMsg> _queue;
        public static BaseMessage CreateMessage(string xml) {
            if (_queue == null)
            {
                _queue = new List<BaseMsg>();
            }
            else if(_queue.Count>=50) {
                _queue = _queue.Where(q => { return q.CreateTime.AddSeconds(20) > DateTime.Now; }).ToList();
            }           
            XElement xmlDoc = XElement.Parse(xml);
            var msgType = xmlDoc.Element("MsgType").Value.ToUpper();
            var FromUserName = xmlDoc.Element("FromUserName").Value;
            var MsgId = "0";
            try
            {
                MsgId = xmlDoc.Element("MsgId").Value;
            }
            catch { }
            var CreateTime = xmlDoc.Element("CreateTime").Value;
            //ToUserName = postObj.GetElementsByTagName("ToUserName").Item(0).InnerText;
            MsgType type = (MsgType)Enum.Parse(typeof(MsgType), msgType);
            Common.WriteLog("MsgType=" + type.ToString());
            if (type != MsgType.EVENT)
            {
                if (_queue.FirstOrDefault(m => { return m.MsgFlag == MsgId; }) == null)
                {
                    _queue.Add(new BaseMsg { CreateTime = DateTime.Now, FromUser = FromUserName, MsgFlag = MsgId });
                }
                else
                {
                    return null;
                }
            }
            else {
                if (_queue.FirstOrDefault(m => { return m.MsgFlag == CreateTime; }) == null)
                {
                    _queue.Add(new BaseMsg { CreateTime = DateTime.Now, FromUser = FromUserName, MsgFlag = CreateTime });
                }
                else {
                    return null;
                }
            }
            Common.WriteLog("queue=" +_queue.Count);
            switch (type) {
                case MsgType.TEXT:
                    return Common.ConvertObj<TextMessage>(xml);
                case MsgType.IMAGE:
                    return Common.ConvertObj<ImgMessage>(xml);
                case MsgType.VIDEO:
                    return Common.ConvertObj<VideoMessage>(xml);
                case MsgType.VOICE:
                    return Common.ConvertObj<VoiceMessage>(xml);
                case MsgType.LINK:
                    return Common.ConvertObj<LinkMessage>(xml);
                case MsgType.LOCATION:
                    return Common.ConvertObj<LocationMessage>(xml);
                case MsgType.EVENT:
                    var strEvent = xmlDoc.Element("Event").Value.ToUpper();
                    var eventtype = (EVENT)Enum.Parse(typeof(EVENT), strEvent);
                    Common.WriteLog("EventType=" + strEvent);
                    switch (eventtype) {
                        case EVENT.CLICK:
                            return Common.ConvertObj<NormalMenuEventMessage>(xml);
                        case EVENT.VIEW:
                            return Common.ConvertObj<NormalMenuEventMessage>(xml);
                        case EVENT.LOCATION:
                            return Common.ConvertObj<LocationEventMessage>(xml);
                        //case EVENT.LOCATION_SELECT:
                        //    return Common.ConvertObj<LocationMenuEventMessage>(xml);
                        case EVENT.SCAN:
                            return Common.ConvertObj<ScanEventMessage>(xml);
                        case EVENT.SUBSCRIBE:
                            return Common.ConvertObj<SubEventMessage>(xml);
                        case EVENT.UNSUBSCRIBE:
                            return Common.ConvertObj<SubEventMessage>(xml);
                        case EVENT.SCANCODE_WAITMSG:
                            return Common.ConvertObj<ScanMenuEventMessage>(xml);
                        default:
                            return Common.ConvertObj<EventMessage>(xml);
                    }
                default:
                    return Common.ConvertObj<BaseMessage>(xml);
            }
        }
    }    
}
