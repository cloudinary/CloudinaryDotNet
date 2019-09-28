namespace CloudinaryDotNet
{
    using System.Text.Encodings.Web;

    /// <summary>
    /// Implement platform specific functions.
    /// </summary>
    internal static partial class Utils
    {
        /// <summary>
        /// Encodes the supplied string and returns the encoded text as a new string.
        /// </summary>
        /// <param name="value">String to encode.</param>
        /// <returns>Encoded string.</returns>
        internal static string Encode(string value)
        {
            return UrlEncoder.Default.Encode(value);
        }
    }
}
