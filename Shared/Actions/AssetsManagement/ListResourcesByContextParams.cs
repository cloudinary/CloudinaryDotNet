namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Allows to list resources by context metadata keys and values.
    /// </summary>
    public class ListResourcesByContextParams : ListResourcesParams
    {
        /// <summary>
        /// Gets or sets if only resources with the given key should be returned. Required.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets whether should only return resources with this given value for the context key.
        /// When not provided, return all resources for which the context key exists. Optional.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (string.IsNullOrEmpty(Key))
            {
                throw new InvalidOperationException("Key must be set to list resources by context.");
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = base.ToParamsDictionary();

            AddParam(dict, "key", Key);
            AddParam(dict, "value", Value);

            return dict;
        }
    }
}
