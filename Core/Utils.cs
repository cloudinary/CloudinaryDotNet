namespace CloudinaryDotNet
{
    using System.Text.Encodings.Web;

    /// <summary>
    /// Implement platform specific functions.
    /// </summary>
    internal static partial class Utils
    {
        internal static string Encode(string value)
        {
            return UrlEncoder.Default.Encode(value);
        }
    }
}
