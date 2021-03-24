namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Search response with information about the assets matching the search criteria.
    /// </summary>
    [DataContract]
    public class SearchResult : BaseResult
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
        /// Gets or sets the details of each of the assets (resources) found.
        /// </summary>
        [DataMember(Name = "resources")]
        public List<SearchResource> Resources { get; set; }

        /// <summary>
        /// Gets or sets when a search request has more results to return than max_results, the next_cursor value is returned as
        /// part of the response.
        /// </summary>
        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; set; }

        /// <summary>
        /// Gets or sets counts of assets, grouped by specified parameters.
        /// </summary>
        [DataMember(Name = "aggregations")]
        public Dictionary<string, Dictionary<string, int>> Aggregations { get; set; }
    }
}
