namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Details of the asset last updated time.
    /// </summary>
    [DataContract]
    public class LastUpdated
    {
        /// <summary>
        /// Gets or sets the time of the last update of access control.
        /// </summary>
        [DataMember(Name = "access_control_updated_at")]
        public DateTime AccessControlUpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the time of the last update of context.
        /// </summary>
        [DataMember(Name = "context_updated_at")]
        public DateTime ContextUpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the time of the last update of metadata.
        /// </summary>
        [DataMember(Name = "metadata_updated_at")]
        public DateTime MetadataUpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the time of the last update of Public ID.
        /// </summary>
        [DataMember(Name = "public_id_updated_at")]
        public DateTime PublicIdUpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the time of the last update of tags.
        /// </summary>
        [DataMember(Name = "tags_updated_at")]
        public DateTime TagsUpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the time of the last update of the asset.
        /// </summary>
        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
