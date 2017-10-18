using System.Text;
using System.Web;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Implement platform specific functions
    /// </summary>
    internal static class Utils
    {
        
        internal static string EncodedUrl(string url)
        {
            return System.Web.HttpUtility.UrlEncode(url, Encoding.UTF8);
        }

        internal static string Encode(string value)
        {
            return HttpUtility.UrlEncodeUnicode(value);
        }
    }
}