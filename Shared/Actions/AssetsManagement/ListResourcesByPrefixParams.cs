namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Allow to filter resources by prefix.
    /// </summary>
    public class ListResourcesByPrefixParams : ListResourcesParams
    {
        /// <summary>
        /// Gets or sets find all resources that their public ID starts with the given prefix.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "prefix", Prefix);

            return dict;
        }
    }
}