namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed result of the visual search request.
    /// </summary>
    [DataContract]
    public class VisualSearchResult : BaseResult
    {
        /// <summary>
        /// Gets or sets list of the assets matching the request conditions.
        /// </summary>
        [DataMember(Name = "resources")]
        public List<Resource> Resources { get; set; }

        /// <summary>
        /// Gets or sets the total count of assets matching the search criteria.
        /// </summary>
        [DataMember(Name = "total_count")]
        public int TotalCount { get; set; }
    }
}
