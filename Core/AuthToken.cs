using CloudinaryShared.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Cloudinary.NetCoreShared
{
    public class AuthToken : AuthTokenBase
    {
        public AuthToken() : base() { }

        public AuthToken(string key) : base(key) { }

        
        protected override string EncodedUrl(string url)
        {
            return System.Net.WebUtility.UrlEncode(url);
        }
    }
}
