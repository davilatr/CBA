using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CBA.Web
{
    public static class CriptoHelper
    {
        public static string HashMD5(string val)
        {
            var bytes = Encoding.ASCII.GetBytes(val);
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(bytes);

            var retorno = string.Empty;

            for (int i = 0; i < hash.Length; i++)
                retorno += hash[i].ToString("x2");
            

            return retorno;
        }
    }
}