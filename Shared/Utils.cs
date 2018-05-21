using System;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Implement generic utility functions
    /// </summary>
    internal static partial class Utils
    {
        internal static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts DateTime to Unix epoch time in seconds
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Epoch time in seconds</returns>
        internal static long ToUnixTimeSeconds(DateTime date)
        {
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds);
        }

        /// <summary>
        /// Converts Unix epoch time in seconds to DateTime
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns>Datetime</returns>
        public static DateTime FromUnixTimeSeconds(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }

        /// <summary>
        /// Unix epoch time now in seconds
        /// </summary>
        /// <returns>total seconds since epoch</returns>
        internal static long UnixTimeNowSeconds()
        {
            return ToUnixTimeSeconds(DateTime.UtcNow);
        }
    }
}
