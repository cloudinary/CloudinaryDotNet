namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Implement generic utility functions.
    /// </summary>
    internal static class Utils
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
                @"^((ftp|https?|s3|gs):.*)|data:([\w-]+/[\w-]+(\+[\w-]+)?)?(;[\w-]+=[\w-]+)*;base64,([a-zA-Z0-9/+\n=]+)");
        }

        /// <summary>
        /// Encodes the supplied URL string as a new string.
        /// </summary>
        /// <param name="value">String to encode.</param>
        /// <returns>Encoded string.</returns>
        internal static string Encode(string value)
        {
            return UrlEncoder.Default.Encode(value);
        }

        /// <summary>
        /// Encode string to URL-safe Base64 string.
        /// </summary>
        /// <param name="s"> String to encode.</param>
        /// <returns>An URL-safe Base64-encoded string.</returns>
        internal static string EncodeUrlSafe(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            return EncodeUrlSafe(bytes);
        }

        /// <summary>
        /// Encode bytes to URL-safe Base64 string.
        /// </summary>
        /// <param name="bytes"> Byte array to encode.</param>
        /// <returns>An URL-safe Base64-encoded string.</returns>
        internal static string EncodeUrlSafe(byte[] bytes)
        {
            return Convert.ToBase64String(bytes).Replace('+', '-').Replace('/', '_');
        }

        /// <summary>
        /// Computes the hash value for the specified string, using default hashing algorithm.
        /// </summary>
        /// <param name="s"> The input to compute the hash code for.</param>
        /// <param name="signatureAlgorithm">Type of hashing algorithm to use for the hash code computation.</param>
        /// <returns>The computed hash code.</returns>
        [SuppressMessage("Security", "CA5350:DoNotUseWeakCryptographicAlgorithms", Justification = "Reviewed.")]
        internal static byte[] ComputeHash(string s, SignatureAlgorithm signatureAlgorithm = SignatureAlgorithm.SHA1)
        {
            if (signatureAlgorithm == SignatureAlgorithm.SHA256)
            {
                using (var sha256 = SHA256.Create())
                {
                    return sha256.ComputeHash(Encoding.UTF8.GetBytes(s));
                }
            }

            using (var sha1 = SHA1.Create())
            {
                return sha1.ComputeHash(Encoding.UTF8.GetBytes(s));
            }
        }

        /// <summary>
        /// Compute hash and convert the result to HEX string.
        /// </summary>
        /// <param name="s"> String to calculate a hash for.</param>
        /// <param name="signatureAlgorithm">Type of hashing algorithm to use for the hash code computation.</param>
        /// <returns>A HEX string that represents the result of hashing.</returns>
        internal static string ComputeHexHash(string s, SignatureAlgorithm signatureAlgorithm = SignatureAlgorithm.SHA1)
        {
            var bytesHash = ComputeHash(s, signatureAlgorithm);
            var signature = new StringBuilder();
            foreach (var b in bytesHash)
            {
                signature.Append(b.ToString("x2", CultureInfo.InvariantCulture));
            }

            return signature.ToString();
        }

        /// <summary>
        /// Prepare HTTP headers that denote that request content is encoded in JSON format.
        /// </summary>
        /// <returns>Map of HTTP headers.</returns>
        internal static Dictionary<string, string> PrepareJsonHeaders()
        {
            var extraHeaders = new Dictionary<string, string>
            {
                {
                    Constants.HEADER_CONTENT_TYPE,
                    Constants.CONTENT_TYPE_APPLICATION_JSON
                },
            };

            return extraHeaders;
        }

        /// <summary>
        /// Validate that an object's property is specified.
        /// </summary>
        /// <param name="propertyExpr">Function that gets object's property value.</param>
        internal static void ShouldBeSpecified(Expression<Func<object>> propertyExpr)
        {
            CheckProperty(propertyExpr, val => val == null, "must be specified");
        }

        /// <summary>
        /// Validate that an object's property is specified.
        /// </summary>
        /// <param name="propertyExpr">Function that gets object's property value.</param>
        /// <typeparam name="T">Value type.</typeparam>
        internal static void ShouldBeSpecified<T>(Expression<Func<T?>> propertyExpr)
            where T : struct
        {
            CheckProperty(propertyExpr, val => !val.HasValue, "must be specified");
        }

        /// <summary>
        /// Validate that an object's property is not specified.
        /// </summary>
        /// <param name="propertyExpr">Expression that gets object's property value.</param>
        internal static void ShouldNotBeSpecified(Expression<Func<object>> propertyExpr)
        {
            CheckProperty(propertyExpr, val => val != null, "must not be specified");
        }

        /// <summary>
        /// Validate that an object's property is not empty string.
        /// </summary>
        /// <param name="propertyExpr">Expression that gets object's property value.</param>
        /// <param name="message">General part of the validation exception message.</param>
        internal static void ShouldNotBeEmpty(Expression<Func<string>> propertyExpr, string message = "must not be empty")
        {
            CheckProperty(propertyExpr, string.IsNullOrEmpty, message);
        }

        /// <summary>
        /// Validate that an object's property is not empty collection.
        /// </summary>
        /// <param name="propertyExpr">Expression that gets object's property value.</param>
        /// <typeparam name="TP">Collection item type.</typeparam>
        internal static void ShouldNotBeEmpty<TP>(Expression<Func<List<TP>>> propertyExpr)
        {
            var propertyValue = propertyExpr.Compile()();
            if (propertyValue == null || !propertyValue.Any())
            {
                throw new ArgumentException($"{GetPropertyName(propertyExpr.Body)} must not be empty");
            }
        }

        private static void CheckProperty<T>(Expression<Func<T>> propertyExpr, Func<T, bool> condition, string message = null)
        {
            var propertyValue = propertyExpr.Compile()();
            if (condition.Invoke(propertyValue))
            {
                var errorMessage = string.IsNullOrEmpty(message)
                    ? $"{GetPropertyName(propertyExpr.Body)}"
                    : $"{GetPropertyName(propertyExpr.Body)} {message}";
                throw new ArgumentException(errorMessage);
            }
        }

        private static string GetPropertyName(System.Linq.Expressions.Expression propertyExpr)
        {
            switch (propertyExpr)
            {
                case MemberExpression memberExpression:
                    return memberExpression.Member.Name;

                case UnaryExpression unaryExpression:
                    {
                        var operandExpr = (MemberExpression)unaryExpression.Operand;
                        return operandExpr.Member.Name;
                    }

                default:
                    return string.Empty;
            }
        }
    }
}
