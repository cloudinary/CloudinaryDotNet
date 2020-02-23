namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using CloudinaryDotNet.Core;

    /// <summary>
    /// Parameters for request to cloudinary.
    /// </summary>
    public abstract class BaseParams
    {
        /// <summary>
        /// The dictionary with custom parameters.
        /// </summary>
        private SortedDictionary<string, object> customParams = new SortedDictionary<string, object>();

        /// <summary>
        /// Validate object model.
        /// </summary>
        public abstract void Check();

        /// <summary>
        /// Make a shallow copy of parameters.
        /// </summary>
        /// <returns>The shallow copy of parameters.</returns>
        public virtual BaseParams Copy()
        {
            return (BaseParams)MemberwiseClone();
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>The dictionary of parameters in cloudinary notation.</returns>
        public virtual SortedDictionary<string, object> ToParamsDictionary()
        {
            var dictionary = new SortedDictionary<string, object>(customParams);
            AddParamsToDictionary(dictionary);
            return dictionary;
        }

        /// <summary>
        /// Allow passing ad-hoc parameters in each method (mainly to allow work-around solutions until a fix is published).
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void AddCustomParam(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                customParams.Add(key, value);
            }
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected virtual void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
        }

        /// <summary>
        /// Add parameter to the dictionary.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected static void AddParam(SortedDictionary<string, object> dict, string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                dict.Add(key, value);
            }
        }

        /// <summary>
        /// Add parameter to the dictionary.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected static void AddParam(SortedDictionary<string, object> dict, string key, DateTime value)
        {
            if (value != DateTime.MinValue)
            {
                dict.Add(key, value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            }
        }

        /// <summary>
        /// Add parameter to the dictionary.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected static void AddParam(SortedDictionary<string, object> dict, string key, float value)
        {
            dict.Add(key, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Add parameter to the dictionary.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected static void AddParam(SortedDictionary<string, object> dict, string key, IEnumerable<string> value)
        {
            if (value != null)
            {
                dict.Add(key, value);
            }
        }

        /// <summary>
        /// Add parameter to the dictionary.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected static void AddParam(SortedDictionary<string, object> dict, string key, bool value)
        {
            dict.Add(key, value ? "true" : "false");
        }

        /// <summary>
        /// Add parameter to the dictionary.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected static void AddParam(SortedDictionary<string, object> dict, string key, bool? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            AddParam(dict, key, value.Value);
        }

        /// <summary>
        /// Add a coordinate object (plain string or Rectangle or List of Rectangles or FaceCoordinates)
        /// to the dictionary.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="coordObj">The value.</param>
        protected static void AddCoordinates(SortedDictionary<string, object> dict, string key, object coordObj)
        {
            if (coordObj == null)
            {
                return;
            }

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

        /// <summary>
        /// Validate that an object's property is specified.
        /// </summary>
        /// <param name="propertyExpr">Function that gets object's property value.</param>
        protected static void ShouldBeSpecified(Expression<Func<object>> propertyExpr)
        {
            var propertyValue = propertyExpr.Compile()();
            if (propertyValue == null)
            {
                throw new ArgumentException($"{GetPropertyName(propertyExpr.Body)} must be specified");
            }
        }

        /// <summary>
        /// Validate that an object's property is not specified.
        /// </summary>
        /// <param name="propertyExpr">Expression that gets object's property value.</param>
        protected static void ShouldNotBeSpecified(Expression<Func<object>> propertyExpr)
        {
            var propertyValue = propertyExpr.Compile()();
            if (propertyValue != null)
            {
                throw new ArgumentException($"{GetPropertyName(propertyExpr.Body)} must not be specified");
            }
        }

        /// <summary>
        /// Validate that an object's property is not empty collection.
        /// </summary>
        /// <param name="propertyExpr">Expression that gets object's property value.</param>
        /// <typeparam name="TP">Collection item type.</typeparam>
        protected static void ShouldNotBeEmpty<TP>(Expression<Func<List<TP>>> propertyExpr)
        {
            var propertyValue = propertyExpr.Compile()();
            if (propertyValue == null || !propertyValue.Any())
            {
                throw new ArgumentException($"{GetPropertyName(propertyExpr.Body)} must not be empty");
            }
        }

        /// <summary>
        /// Validate that an object's property is not empty string.
        /// </summary>
        /// <param name="propertyExpr">Expression that gets object's property value.</param>
        protected static void ShouldNotBeEmpty(Expression<Func<string>> propertyExpr)
        {
            var propertyValue = propertyExpr.Compile()();
            if (string.IsNullOrEmpty(propertyValue))
            {
                throw new ArgumentException($"{GetPropertyName(propertyExpr.Body)} must not be empty");
            }
        }

        private static string GetPropertyName(Expression propertyExpr)
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
