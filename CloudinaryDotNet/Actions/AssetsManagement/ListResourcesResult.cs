namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed result of the resources listing request.
    /// </summary>
    [DataContract]
    public class ListResourcesResult : BaseResult
    {
        /// <summary>
        /// Gets or sets list of the assets matching the request conditions.
        /// </summary>
        [DataMember(Name = "resources")]
        public Resource[] Resources { get; set; }

        /// <summary>
        /// Gets or sets when a listing request has more results to return than <see cref="ListResourcesParams.MaxResults"/>,
        /// the <see cref="NextCursor"/> value is returned as part of the response. You can then specify this value as
        /// the <see cref="ListResourcesParams.NextCursor"/> parameter of the following listing request.
        /// </summary>
        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; set; }
    }
}
