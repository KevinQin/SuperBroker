using com.seascape.db;
using com.seascape.tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.superbroker.data
{
    public abstract class DbCenter
    {
        public const string APPID = "seascape_super_broker_20170828";
        public const string SALT = "k*v%ei`!hx~sg77o`uP-^162859?";
        public const com.seascape.db.DBTYPE DB_TYPE = com.seascape.db.DBTYPE.MySql;
        public static DateTime DEF_DATE = new DateTime(2000, 1, 1);

        public DbHelper helper
        {
            get { return DataConnectionManager.GetHelper(); }
        }

        public const int PAGE_SIZE = 15;

        /// <summary>
        /// 检测表是否存在
        /// </summary>
        /// <param name="Table">表名</param>
        /// <returns></returns>
        public bool ExistsTable(string Table)
        {
            return Convert.ToInt16(helper.GetOne("SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME='" + Table + "'")) > 0;
        }

        public static string PasswordEncode(string pwd)
        {
            return BasicTool.MD5(APPID + BasicTool.MD5(pwd) + SALT);
        }

        public static string GetUserCode(string beforeWord = "")
        {
            string ys = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string ms = "ABCDEFGHIJKL";
            string ds = "abcdefghijklmnopqrstuvwxyzABCDE";
            string hs = "abcdefghijklABCDEFGHIJKL";
            string Ms = ys.Substring(0, 60);
            string Ss = Ms;
            DateTime dt = DateTime.Now;
            string rs = new Random(dt.Millisecond).Next(100000, 999999).ToString();
            string rcode = ys.Substring(dt.Year - 2017, 1) + ms.Substring(dt.Month - 1, 1) + ds.Substring(dt.Day - 1, 1);
            rcode += hs.Substring(dt.Hour, 1) + Ms.Substring(dt.Minute, 1) + Ss.Substring(dt.Second, 1);
            rcode += "_" + rs;
            return rcode;
        }

        public static DateTime Now()
        {
            return DateTime.Now;
        }        
    }

    public interface IDB<T>
    {
        bool Add(out int Id, T t);
        bool Update(T t);
    }

    public static class SelfExtClass {

        public static int ToInt(this string t) {
            int id;
            int.TryParse(t, out id);
            return id;
        }

        public static DateTime ToDateTime(this string t) {
            return Convert.ToDateTime(t);
        }

        public static String Format(this DateTime dt) {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static bool ToBool(this int t) {
            return t == 1;
        }

        public static DateTime ToDateTime(this object t)
        {
            return Convert.ToDateTime(t);
        }

        public static int ToInt(this object t)
        {
            int id;
            int.TryParse(t.ToString(), out id);
            return id;
        }

        public static Int16 ToInt16(this object t) {
            Int16 id;
            Int16.TryParse(t.ToString(), out id);
            return id;
        }

        public static double ToDouble(this object t)
        {
            Double id;
            double.TryParse(t.ToString(), out id);
            return id;
        }

    }
}
