using System;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parsed list of tags.
    /// </summary>
    [DataContract]
    public class ListTagsResult : BaseResult
    {
        /// <summary>
        /// The list of tags currently assigned to the media asset.
        /// </summary>
        [DataMember(Name = "tags")]
        public string[] Tags { get; protected set; }

        /// <summary>
        /// When a listing request has more results to return than <see cref="ListTagsParams.MaxResults"/>, the
        /// <see cref="NextCursor"/> value is returned as part of the response. You can then specify this value as the
        /// <see cref="ListTagsParams.NextCursor"/> parameter of the following listing request.
        /// </summary>
        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; protected set; }
    }
}
