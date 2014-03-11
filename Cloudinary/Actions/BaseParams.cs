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

        protected void AddParam(SortedDictionary<string, object> dict, string key, string value)
        {
            if (!String.IsNullOrEmpty(value))
                dict.Add(key, value);
        }

        protected void AddParam(SortedDictionary<string, object> dict, string key, float value)
        {
            dict.Add(key, value.ToString(CultureInfo.InvariantCulture));
        }

        protected void AddParam(SortedDictionary<string, object> dict, string key, IEnumerable<string> value)
        {
            if (value != null)
                dict.Add(key, value);
        }

        protected void AddParam(SortedDictionary<string, object> dict, string key, bool value)
        {
            dict.Add(key, value ? "true" : "false");
        }
    }
}
