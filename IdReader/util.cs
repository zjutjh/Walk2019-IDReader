using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IdReader
{
    public static class Util
    {

        public static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.ASCII.GetBytes(myString);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }

            return byte2String.ToLower();
        }

        public static bool TestMode = false;

        public static string testUrl = "https://wx.idevlab.cn/user/verify";
        private static string _serverUrl = "https://walk.zjutjh.com/user/verify";

        public static string serverUrl
        {
            get
            {
                if (TestMode)
                {
                    return testUrl;
                }
                return _serverUrl;
            }
        }
    }
}
