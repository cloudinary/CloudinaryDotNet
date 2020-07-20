namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed list of tags.
    /// </summary>
    [DataContract]
    public class ListTagsResult : BaseResult
    {
        /// <summary>
        /// Gets or sets the list of tags currently assigned to the media asset.
        /// </summary>
        [DataMember(Name = "tags")]
        public string[] Tags { get; set; }

        /// <summary>
        /// Gets or sets a value for a situation when a listing request has more results to return than <see cref="ListTagsParams.MaxResults"/>, the
        /// <see cref="NextCursor"/> value is returned as part of the response. You can then specify this value as the
        /// <see cref="ListTagsParams.NextCursor"/> parameter of the following listing request.
        /// </summary>
        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; set; }
    }
}
