using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of the request for a list of transformation.
    /// </summary>
    public class ListTransformsParams : BaseParams
    {
        /// <summary>
        /// Instantiates the <see cref="ListTransformsParams"/> object.
        /// </summary>
        public ListTransformsParams()
        {
            NextCursor = String.Empty;
        }

        /// <summary>
        /// Max number of transformations to return. Default=10. Maximum=500.
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// Return named transformations or no.
        /// </summary>
        public bool? Named { get; set; }

        /// <summary>
        /// When a listing request has more results to return than <see cref="MaxResults"/>, 
        /// the <see cref="ListTransformsResult.NextCursor"/> value is returned as part of the response. You can then
        /// specify this value as the <see cref="NextCursor"/> parameter of the following listing request. 
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
            if(Named.HasValue)
                AddParam(dict, "named", Named.Value.ToString());
            if(!string.IsNullOrWhiteSpace(NextCursor))
                AddParam(dict, "next_cursor", NextCursor);

            return dict;
        }
    }
}
