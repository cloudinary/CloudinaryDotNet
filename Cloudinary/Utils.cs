namespace CloudinaryDotNet
{
    using System.Web;

    /// <summary>
    /// Implement platform specific functions.
    /// </summary>
    internal static partial class Utils
    {
        /// <summary>
        /// Converts a string into a Unicode string.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <returns>Unicode string.</returns>
        internal static string Encode(string value)
        {
            return HttpUtility.UrlEncodeUnicode(value);
        }
    }
}
