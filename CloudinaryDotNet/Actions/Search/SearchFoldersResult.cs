namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Search response with information about the folders matching the search criteria.
    /// </summary>
    [DataContract]
    public class SearchFoldersResult : SearchResultBase
    {
        /// <summary>
        /// Gets or sets the details of each of the folders found.
        /// </summary>
        [DataMember(Name = "folders")]
        public List<SearchFolder> Folders { get; set; }
    }
}
