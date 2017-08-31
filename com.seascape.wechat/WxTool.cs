using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using LitJson;
using com.seascape.tools;
using System.Security.Cryptography.X509Certificates;

namespace com.seascape.wechat
{
    public class WxTool
    {
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
        /// 对象转数组
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public string[] ConvertToArr(object o)
        {
            StringBuilder sb = new StringBuilder();
            foreach (System.Reflection.PropertyInfo p in o.GetType().GetProperties())
            {
                if (p.GetValue(o, null) != null && p.GetValue(o, null).ToString().Length > 0)
                {
                    sb.Append(p.Name + "=" + p.GetValue(o, null) + ",");
                }
            }
            return sb.ToString().Substring(0, sb.ToString().Length - 1).Split(',');
        }

        /// <summary>
        /// 微信MD5
        /// </summary>
        /// <param name="SrcS"></param>
        /// <returns></returns>
        public static string WxMd5(string SrcS)
        {
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(SrcS));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToUpper();
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
        /// 微信支付统一签名
        /// </summary>
        /// <param name="Arr"></param>
        /// <returns></returns>
        public string Sign_Pay(string[] Arr, string KeyValue)
        {
            Array.Sort(Arr);
            string Temp = string.Join("&", Arr);
            Temp = Temp + "&key=" + KeyValue;
            return WxTool.WxMd5(Temp);
        }

        /// 微信支付提交POST信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string webRequest(string url, string data, string cert, string password)
        {
            string html = null;
            X509Certificate2 cer = new X509Certificate2(cert, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);//线上发        布需要添加
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.ClientCertificates.Add(cer);
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
        /// 下载多媒体文件
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="localPath"></param>
        /// <returns></returns>
        public string downloadMedia(string sid, string localPath, string access_token)
        {
            try
            {
                return BasicTool.webRequestForFile(string.Format("http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", access_token, sid), localPath);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
}
