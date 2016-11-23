using System;
using System.Security.Cryptography;
using System.Text;

namespace Web.Services.Common
{
    public static class Common
    {
        public static string EncryptMD5(string encryptString)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(encryptString));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }
        }
    }
}
