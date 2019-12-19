namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Extensions methods for JSON processing.
    /// </summary>
    internal static class JsonExtensions
    {
        /// <summary>
        /// Read value from JSON, converting property name to snake_case.
        /// </summary>
        /// <typeparam name="T">Type of value to read.</typeparam>
        /// <param name="token">JSON Token.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Value read from JSON.</returns>
        internal static T ReadValueAsSnakeCase<T>(this JToken token, string propertyName) => ReadValue<T>(token, propertyName.ToSnakeCase());

        /// <summary>
        /// Read value from JSON.
        /// </summary>
        /// <typeparam name="T">Type of value to read.</typeparam>
        /// <param name="token">JSON Token.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Value read from JSON.</returns>
        internal static T ReadValue<T>(this JToken token, string propertyName)
        {
            var jToken = token[propertyName];
            return jToken == null ? default(T) : jToken.ToObject<T>();
        }

        /// <summary>
        /// Converts string to snake_case.
        /// </summary>
        /// <param name="s">String to convert.</param>
        /// <returns>snake_cased_string.</returns>
        public static string ToSnakeCase(this string s) =>
            Regex.Replace(s, "[A-Z]", "_$0")
                .Substring(1)
                .ToLower();

        /// <summary>
        /// Converts string to camelCase.
        /// </summary>
        /// <param name="s">String to convert.</param>
        /// <returns>camelCasedString.</returns>
        public static string ToCamelCase(this string s) => char.ToLowerInvariant(s[0]) + s.Substring(1);

        /// <summary>
        /// Reads object from JSON using factory function.
        /// </summary>
        /// <param name="token">The JSON token.</param>
        /// <param name="propertyName">The name of JSON property to read from.</param>
        /// <param name="func">The factory function.</param>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <returns>Object read. If property is missin in JSON, returns default value.</returns>
        internal static T ReadObject<T>(this JToken token, string propertyName, Func<JToken, T> func)
        {
            var dataToken = token[propertyName];
            return dataToken != null ? func(dataToken) : default(T);
        }

        /// <summary>
        /// Fixes spelling of XxxUri args.
        /// </summary>
        /// <param name="propertyName">Property name that should be fixed.</param>
        /// <returns>Property name where 'Uri' is changed to 'Url'.</returns>
        internal static string FixUri(this string propertyName) => propertyName.Replace("Uri", "Url");

        /// <summary>
        /// Reads list from JSON.
        /// </summary>
        /// <param name="token">JSON token to read from.</param>
        /// <param name="propertyName">The property in JSON that should be read.</param>
        /// <param name="func">Factory fucnction to create elements of result list.</param>
        /// <typeparam name="T">The type of result list items.</typeparam>
        /// <returns>A typed list of items. If property is not present in JSON, returns an empty list.</returns>
        internal static List<T> ReadList<T>(this JToken token, string propertyName, Func<JToken, T> func)
        {
            var dataToken = token[propertyName];
            return dataToken == null ?
                Enumerable.Empty<T>().ToList()
                : dataToken.ToObject<List<JObject>>()
                    .Select(func)
                    .ToList();
        }
    }
}
