using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of list tags request.
    /// </summary>
    public class ListTagsParams : BaseParams
    {
        /// <summary>
        /// Instantiates the <see cref="ListTagsParams"/> object.
        /// </summary>
        public ListTagsParams()
        {
            NextCursor = String.Empty;
            Prefix = String.Empty;
        }

        /// <summary>
        /// Optional. The type of file for which to retrieve the tags. Possible values: image, raw, video. 
        /// Default: image.
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Find all tags that start with the given prefix.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Max number of tags to return. Default=10. Maximum=500.  
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// When a listing request has more results to return than <see cref="MaxResults"/>, the 
        /// <see cref="ListTagsResult.NextCursor"/> value is returned as part of the response. You can then specify
        /// this value as the <see cref="NextCursor"/> parameter of the following listing request.
        /// </summary>
        public string NextCursor { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            // ok
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
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
