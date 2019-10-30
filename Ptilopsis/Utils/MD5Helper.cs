using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Ptilopsis.Utils
{
    public class MD5Helper
    {
        public static string getMd5Hash(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return null;
            }
            var benchStr = input.Trim();
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(benchStr));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
