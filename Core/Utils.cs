using System.Text.Encodings.Web;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Implement platform specific functions
    /// </summary>
    internal static class Utils
    {
        internal static string EncodedUrl(string url)
        {
            return System.Net.WebUtility.UrlEncode(url);
        }

        internal static string Encode(string value)
        {
            return UrlEncoder.Default.Encode(value);
        }
    }
}