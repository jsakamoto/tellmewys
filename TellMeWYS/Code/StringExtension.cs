using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TellMeWYS
{
    public static class StringExtension
    {
        public static string Bind(this string self, params object[] args)
        {
            return string.Format(self, args);
        }

        public static int ToInt(this string self)
        {
            return int.Parse(self);
        }

        public static string Join(this IEnumerable<string> self, string separator = "")
        {
            return string.Join(separator, self);
        }

        public static string ToHashString<T>(this string self, string salt = null) where T : HashAlgorithm
        {
            var toHash = (salt ?? "") + self;
            return HashAlgorithm.Create(typeof(T).Name)
                .ComputeHash(Encoding.UTF8.GetBytes(toHash))
                .Select(b => "{0:x2}".Bind(b))
                .Join();
        }

        public static T AsJsonTo<T>(this string self)
        {
            return JsonConvert.DeserializeObject<T>(self);
        }

        public static string ToBase64String(this byte[] self)
        {
            return Convert.ToBase64String(self);
        }

        public static string ToJson(this object self)
        {
            return JsonConvert.SerializeObject(self);
        }
    }
}