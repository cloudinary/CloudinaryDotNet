using System;
using System.Collections.Generic;
using System.Globalization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of request to cloudinary
    /// </summary>
    public abstract class BaseParams
    {
        /// <summary>
        /// Validate object model
        /// </summary>
        public abstract void Check();

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns></returns>
        public abstract SortedDictionary<string, object> ToParamsDictionary();

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected void AddParam(SortedDictionary<string, object> dict, string key, string value)
        {
            if (!String.IsNullOrEmpty(value))
                dict.Add(key, value);
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected void AddParam(SortedDictionary<string, object> dict, string key, DateTime value)
        {
            if (value != DateTime.MinValue)
                dict.Add(key, value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected void AddParam(SortedDictionary<string, object> dict, string key, float value)
        {
            dict.Add(key, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected void AddParam(SortedDictionary<string, object> dict, string key, IEnumerable<string> value)
        {
            if (value != null)
                dict.Add(key, value);
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected void AddParam(SortedDictionary<string, object> dict, string key, bool value)
        {
            dict.Add(key, value ? "true" : "false");
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected void AddParam(SortedDictionary<string, object> dict, string key, bool? value)
        {
            if (!value.HasValue) return;

            AddParam(dict, key, value.Value);
        }
    }
}
