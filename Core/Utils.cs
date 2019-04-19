using System.Text.Encodings.Web;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Implement platform specific functions
    /// </summary>
    internal static partial class Utils
    {
        internal static string Encode(string value)
        {
            return UrlEncoder.Default.Encode(value);
        }
    }
}
