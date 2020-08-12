namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Results of context management.
    /// </summary>
    [DataContract]
    public class MetadataUpdateResult : BaseResult
    {
        /// <summary>
        /// Gets or sets public IDs of affected assets.
        /// </summary>
        [DataMember(Name = "public_ids")]
        public string[] PublicIds { get; set; }
    }
}
