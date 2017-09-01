using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using com.seascape.tools;

/// <summary>
/// Util 的摘要说明
/// </summary>
public class Util
{
    public static string MyEncodeInputString(string inputString)
    {
        //要替换的敏感字
        string SqlStr = @"and|or|exec|execute|insert|select|delete|update|alter|create|drop|count|\*|chr|char|asc|mid|substring|master|truncate|declare|xp_cmdshell|restore|backup|net +user|net +localgroup +administrators";
        try
        {
            if ((inputString != null) && (inputString != String.Empty))
            {
                string str_Regex = @"\b(" + SqlStr + @")\b";

                Regex Regex = new Regex(str_Regex, RegexOptions.IgnoreCase);
                MatchCollection matches = Regex.Matches(inputString);
                for (int i = 0; i < matches.Count; i++)
                    inputString = inputString.Replace(matches[i].Value, "[" + matches[i].Value + "]");

            }
        }
        catch
        {
            return "";
        }
        return inputString;
    }

    public static string MyDecodeOutputString(string outputstring)
    {
        //要替换的敏感字
        string SqlStr = @"and|or|exec|execute|insert|select|delete|update|alter|create|drop|count|\*|chr|char|asc|mid|substring|master|truncate|declare|xp_cmdshell|restore|backup|net +user|net +localgroup +administrators";
        try
        {
            if ((outputstring != null) && (outputstring != String.Empty))
            {
                string str_Regex = @"\[\b(" + SqlStr + @")\b\]";
                Regex Regex = new Regex(str_Regex, RegexOptions.IgnoreCase);
                MatchCollection matches = Regex.Matches(outputstring);
                for (int i = 0; i < matches.Count; i++)
                    outputstring = outputstring.Replace(matches[i].Value, matches[i].Value.Substring(1, matches[i].Value.Length - 2));
            }
        }
        catch
        {
            return "";
        }
        return outputstring;
    }

    /// <summary>
    /// 得到用户IP
    /// </summary>
    /// <param name="r">Request对象</param>
    /// <returns></returns>
    public static string GetIp(HttpRequest r)
    {
        string Ip = string.Empty;
        if (r.ServerVariables["HTTP_VIA"] != null)
        {
            if (r.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
            {
                if (r.ServerVariables["HTTP_CLIENT_IP"] != null)
                    Ip = r.ServerVariables["HTTP_CLIENT_IP"].ToString();
                else
                    if (r.ServerVariables["REMOTE_ADDR"] != null)
                    Ip = r.ServerVariables["REMOTE_ADDR"].ToString();
                else
                    Ip = "0.0.0.0";
            }
            else
                Ip = r.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        }
        else if (r.ServerVariables["REMOTE_ADDR"] != null)
        {
            Ip = r.ServerVariables["REMOTE_ADDR"].ToString();
        }
        else
        {
            Ip = "0.0.0.0";
        }
        return Ip;
    }

    public static string GetFormattedAddress(double latitude, double longitude)
    {
        try
        {
            string url = "http://maps.google.cn/maps/api/geocode/json?latlng={0},{1}&sensor=true&language=zh-CN";
            string json = BasicTool.webRequest(string.Format(url, latitude, longitude));
            LitJson.JsonData allData = LitJson.JsonMapper.ToObject(json);
            LitJson.JsonData resultData = allData["results"];
            LitJson.JsonData firstData = resultData[0];
            string formatAddress = firstData["formatted_address"].ToString();
            formatAddress = formatAddress.Split(' ')[0].Replace("中国", "").Trim();
            int len = firstData["address_components"].Count;
            //string Address = firstData["address_components"][0]["long_name"].ToString();
            string prov = firstData["address_components"][len - 3]["long_name"].ToString();
            string city = firstData["address_components"][len - 4]["long_name"].ToString();
            string dist = firstData["address_components"][len - 5]["long_name"].ToString();
            string Address = formatAddress.Replace(prov, "").Replace(city, "").Replace(dist, "");
            if (formatAddress.ToUpper().IndexOf("unnamed") > -1)
            {
                Address = "";
                if (city != "") { Address += city; }
                if (dist != "") { Address += dist; }
                if (Address == "") { Address = prov; }
            }
            object obj = new { Full = formatAddress, A = Address, P = prov, C = city, D = dist };
            return new com.seascape.tools.Response().Success(obj);
        }
        catch (Exception ex)
        {
            return new com.seascape.tools.Response().Fail("can not get location");
        }
    }
}

public static class SelfExtClass
{

    public static int ToInt(this string t)
    {
        int id=0;
        int.TryParse(t, out id);
        return id;
    }

    public static DateTime ToDateTime(this string t)
    {
        return Convert.ToDateTime(t);
    }

    public static String Format(this DateTime dt)
    {
        return dt.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static bool ToBool(this int t)
    {
        return t == 1;
    }

    public static DateTime ToDateTime(this object t)
    {
        return Convert.ToDateTime(t);
    }

    public static int ToInt(this object t)
    {
        int id=0;
        int.TryParse(t.ToString(), out id);
        return id;
    }

    public static Int16 ToInt16(this object t)
    {
        Int16 id=0;
        Int16.TryParse(t.ToString(), out id);
        return id;
    }

    public static double ToDouble(this object t)
    {
        Double id=0;
        double.TryParse(t.ToString(), out id);
        return id;
    }

    public static string xRequest(this HttpContext hc, string param)
    {
        if (hc.Request[param] == null)
        {
            return "";
        }
        else
        {
            return Util.MyEncodeInputString(hc.Request[param].ToString());
        }
    }

    public static string xRequestEncode(this HttpContext hc, string param)
    {
        if (hc.Request[param] == null)
        {
            return "";
        }
        else
        {
            return Util.MyEncodeInputString(hc.Server.UrlDecode(hc.Request[param].ToString()));
        }
    }
}

