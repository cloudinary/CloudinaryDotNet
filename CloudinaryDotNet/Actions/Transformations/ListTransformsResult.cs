namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed list of transformations details.
    /// </summary>
    [DataContract]
    public class ListTransformsResult : BaseResult
    {
        /// <summary>
        /// Gets or sets a listing of transformations specified in your account.
        /// </summary>
        [DataMember(Name = "transformations")]
        public TransformDesc[] Transformations { get; set; }

        /// <summary>
        /// Gets or sets a value for a situation when a listing request has more results to return than <see cref="ListTransformsParams.MaxResults"/>,
        /// the <see cref="NextCursor"/> value is returned as part of the response. You can then specify this value as
        /// the <see cref="ListTransformsParams.NextCursor"/> parameter of the following listing request.
        /// </summary>
        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; set; }
    }
}
