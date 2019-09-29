namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Implement generic utility functions.
    /// </summary>
    internal static partial class Utils
    {
        /// <summary>Represents the Unix time starting point.</summary>
        internal static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts Unix epoch time in seconds to DateTime.
        /// </summary>
        /// <param name="unixTime">The epoch time to be converted.</param>
        /// <returns>Datetime.</returns>
        public static DateTime FromUnixTimeSeconds(long unixTime)
        {
            return Epoch.AddSeconds(unixTime);
        }

        /// <summary>
        /// Converts DateTime to Unix epoch time in seconds.
        /// </summary>
        /// <param name="date">The date to be converted.</param>
        /// <returns>Epoch time in seconds.</returns>
        internal static long ToUnixTimeSeconds(DateTime date)
        {
            return Convert.ToInt64((date.ToUniversalTime() - Epoch).TotalSeconds);
        }

        /// <summary>
        /// Unix epoch time now in seconds.
        /// </summary>
        /// <returns>total seconds since epoch.</returns>
        internal static long UnixTimeNowSeconds()
        {
            return ToUnixTimeSeconds(DateTime.UtcNow);
        }

        /// <summary>
        /// Concatenates items using provided separator, escaping separator character in each item.
        /// </summary>
        /// <param name="separator">The string to use as a separator.</param>
        /// <param name="items">IEnumerable to join.</param>
        /// <returns>The safely joined string.</returns>
        internal static string SafeJoin(string separator, IEnumerable<string> items)
        {
            return string.Join(separator, items.Select(item => Regex.Replace(item, $"([{separator}])", "\\$1")));
        }

        /// <summary>
        /// Based on file path, determines if the file is hosted remotely.
        /// </summary>
        /// <param name="filePath"> Path to the file.</param>
        /// <returns>True if the file is remote; otherwise, false.</returns>
        internal static bool IsRemoteFile(string filePath)
        {
            return Regex.IsMatch(
                filePath,
                @"^((ftp|https?|s3|gs):.*)|data:([\w-]+/[\w-]+)?(;[\w-]+=[\w-]+)*;base64,([a-zA-Z0-9/+\n=]+)");
        }

        /// <summary>
        /// Encode string to URL-safe Base64 string.
        /// </summary>
        /// <param name="s"> String to encode.</param>
        internal static string EncodeUrlSafe(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            return EncodeUrlSafe(bytes);
        }

        /// <summary>
        /// Encode bytes to URL-safe Base64 string.
        /// </summary>
        /// <param name="bytes"> Byte array to encode.</param>
        internal static string EncodeUrlSafe(byte[] bytes)
        {
            return Convert.ToBase64String(bytes).Replace('+', '-').Replace('/', '_');
        }

        /// <summary>
        /// Computes the hash value for the specified string.
        /// </summary>
        /// <param name="s"> The input to compute the hash code for.</param>
        /// <returns>The computed hash code.</returns>
        internal static byte[] ComputeHash(string s)
        {
            using (var sha1 = SHA1.Create())
            {
                return sha1.ComputeHash(Encoding.UTF8.GetBytes(s));
            }
        }

        /// <summary>
        /// Compute hash and convert the result to HEX string.
        /// </summary>
        /// <param name="s"> String to calculate a hash for.</param>
        internal static string ComputeHexHash(string s)
        {
            var bytesHash = ComputeHash(s);
            var signature = new StringBuilder();
            foreach (var b in bytesHash)
            {
                signature.Append(b.ToString("x2"));
            }

            return signature.ToString();
        }
    }
}
