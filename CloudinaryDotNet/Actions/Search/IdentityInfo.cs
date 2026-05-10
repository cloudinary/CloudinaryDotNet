namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents identity data of the user who created or uploaded an asset.
    /// </summary>
    [DataContract]
    public class IdentityInfo
    {
        /// <summary>
        /// Gets or sets identity access key.
        /// </summary>
        [DataMember(Name = "access_key")]
        public string AccessKey { get; set; }

        /// <summary>
        /// Gets or sets the custom identifier of the user (e.g. email address).
        /// </summary>
        [DataMember(Name = "custom_id")]
        public string CustomId { get; set; }

        /// <summary>
        /// Gets or sets the external identifier of the user.
        /// </summary>
        [DataMember(Name = "external_id")]
        public string ExternalId { get; set; }
    }
}