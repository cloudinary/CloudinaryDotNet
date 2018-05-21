﻿using System.Text;
using System.Web;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Implement platform specific functions
    /// </summary>
    internal static partial class Utils
    {

        internal static string EncodedUrl(string url)
        {
            return HttpUtility.UrlEncode(url, Encoding.UTF8);
        }

        internal static string Encode(string value)
        {
            return HttpUtility.UrlEncodeUnicode(value);
        }
    }
}
