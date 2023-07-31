namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Search response with information matching the search criteria.
    /// </summary>
    public class SearchResultBase : BaseResult
    {
        /// <summary>
        /// Gets or sets the total count of assets matching the search criteria.
        /// </summary>
        [DataMember(Name = "total_count")]
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the time taken to process the request.
        /// </summary>
        [DataMember(Name = "time")]
        public long Time { get; set; }

        /// <summary>
        /// Gets or sets when a search request has more results to return than max_results, the next_cursor value is returned as
        /// part of the response.
        /// </summary>
        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; set; }
    }
}
