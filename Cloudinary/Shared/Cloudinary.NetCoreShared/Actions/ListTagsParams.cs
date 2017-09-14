using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    public class ListTagsParams : BaseParams
    {
        public ListTagsParams()
        {
            NextCursor = String.Empty;
            Prefix = String.Empty;
        }

        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Find all tags that start with the given prefix.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Max number of tags to return. Default=10. Maximum=500.  
        /// </summary>
        public int MaxResults { get; set; }

        public string NextCursor { get; set; }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            // ok
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            if (MaxResults > 0)
                AddParam(dict, "max_results", MaxResults.ToString());

            AddParam(dict, "next_cursor", NextCursor);
            AddParam(dict, "prefix", Prefix);

            return dict;
        }
    }
}
