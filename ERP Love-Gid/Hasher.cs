using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
namespace ERP_Love_Gid
{
    static class PasswordHasher
    {
        /// <summary>
        /// Получение хеша из строки
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string getMd5Hash(string input)
        {

            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();


            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));


            StringBuilder sBuilder = new StringBuilder();


            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }


            return sBuilder.ToString();
        }

        /// <summary>
        /// сравнение хешей
        /// </summary>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool verifyMd5Hash(string input, string hash)
        {

            string hashOfInput = getMd5Hash(input);


            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return (0 == comparer.Compare(hashOfInput, hash));

        }
    }

}