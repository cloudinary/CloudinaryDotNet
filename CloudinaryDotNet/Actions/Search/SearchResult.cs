namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Search response with information about the assets matching the search criteria.
    /// </summary>
    [DataContract]
    public class SearchResult : SearchResultBase
    {
        /// <summary>
        /// Gets or sets the details of each of the assets (resources) found.
        /// </summary>
        [DataMember(Name = "resources")]
        public List<SearchResource> Resources { get; set; }

        /// <summary>
        /// Gets or sets counts of assets, grouped by specified parameters.
        /// </summary>
        [DataMember(Name = "aggregations")]
        public Dictionary<string, Dictionary<string, int>> Aggregations { get; set; }
    }
}
