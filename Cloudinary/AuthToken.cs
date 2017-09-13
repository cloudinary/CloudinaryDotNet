using CloudinaryShared.Core;
using Cloudinary.NetCoreShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CloudinaryDotNet
{
    public class AuthToken : AuthTokenBase
    {
        public AuthToken() : base() { }

        public AuthToken(string key) : base(key) { }

        protected override string EscapeToLower(string url)
        {
            string escaped = string.Empty;
            string encodedUrl = string.Empty;

            encodedUrl = HttpUtility.UrlEncode(url, Encoding.UTF8);
            StringBuilder sb = new StringBuilder(encodedUrl);
            string result = sb.ToString();
            string regex = "%..";
            Regex r = new Regex(regex, RegexOptions.Compiled);
            foreach (Match ItemMatch in r.Matches(sb.ToString()))
            {
                string buf = sb.ToString().Substring(ItemMatch.Index, ItemMatch.Length).ToLower();
                sb.Remove(ItemMatch.Index, ItemMatch.Length);
                sb.Insert(ItemMatch.Index, buf);
            }

            return sb.ToString();
        }
    }
}
