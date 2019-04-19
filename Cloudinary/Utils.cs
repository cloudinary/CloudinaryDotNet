using System.Text;
using System.Web;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Implement platform specific functions
    /// </summary>
    internal static partial class Utils
    {

        internal static string Encode(string value)
        {
            return HttpUtility.UrlEncodeUnicode(value);
        }
    }
}
