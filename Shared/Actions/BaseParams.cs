using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using CloudinaryDotNet.Core;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of request to cloudinary
    /// </summary>
    public abstract class BaseParams
    {
        /// <summary>
        /// The dictionary with custom parameters
        /// </summary>
        private SortedDictionary<string, object> CustomParams = new SortedDictionary<string, object>();

        /// <summary>
        /// Validate object model
        /// </summary>
        public abstract void Check();

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns></returns>
        public virtual SortedDictionary<string, object> ToParamsDictionary()
        {
            return new SortedDictionary<string, object>(CustomParams);
        }

        /// <summary>
        /// Allow passing ad-hoc parameters in each method (mainly to allow work-around solutions until a fix is published)
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        public void AddCustomParam(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                CustomParams.Add(key, value);
            }
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected void AddParam(SortedDictionary<string, object> dict, string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
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

        /// <summary>
        /// Adds a coordinate object (plain string or Rectangle or List of Rectangles or FaceCoordinates)
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="coordObj">The value.</param>
        protected void AddCoordinates(SortedDictionary<string, object> dict, string key, object coordObj)
        {
            if (coordObj == null) return;

            if (coordObj is Rectangle)
            {
                var rect = (Rectangle)coordObj;
                dict.Add(key, string.Format("{0},{1},{2},{3}", rect.X, rect.Y, rect.Width, rect.Height));
            }
            else if (coordObj is List<Rectangle>)
            {
                var list = (List<Rectangle>)coordObj;
                dict.Add(key, string.Join("|", list.Select(r => string.Format("{0},{1},{2},{3}", r.X, r.Y, r.Width, r.Height)).ToArray()));
            }
            else
            {
                dict.Add(key, coordObj.ToString());
            }
        }
    }
}
