using System.Security.Cryptography;
using System.Text;

namespace IdReader
{
    public static class Util
    {

        public static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var fromData = Encoding.UTF8.GetBytes(myString);
            var targetData = md5.ComputeHash(fromData);
            string byte2String = "";

            foreach (var item in targetData)
                byte2String += item.ToString("x2");

            return byte2String.ToLower();
        }

        public static bool TestMode = false;

        public static string testUrl = "https://wx.idevlab.cn/user/verify";
        private static string _serverUrl = "https://walk.zjutjh.com/user/verify";

        public static string ServerUrl
        {
            get
            {
                if (TestMode)
                    return testUrl;
                return _serverUrl;
            }
        }
    }
}
