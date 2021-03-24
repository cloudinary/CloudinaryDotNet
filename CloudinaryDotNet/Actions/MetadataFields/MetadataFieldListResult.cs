namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Result of metadata fields listing.
    /// </summary>
    [DataContract]
    public class MetadataFieldListResult : BaseResult
    {
        /// <summary>
        ///  Gets or sets a list of basic details of listed metadata fields.
        /// </summary>
        [DataMember(Name = "metadata_fields")]
        public IEnumerable<MetadataFieldResult> MetadataFields { get; set; }
    }
}