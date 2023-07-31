namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The details of the folder found.
    /// </summary>
    [DataContract]
    public class SearchFolder
    {
        /// <summary>
        /// Gets or sets the name of the folder.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name;

        /// <summary>
        /// Gets or sets the path of the folder.
        /// </summary>
        [DataMember(Name = "path")]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets date when the folder was created.
        /// </summary>
        [DataMember(Name = "created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the eternal id of the folder.
        /// </summary>
        [DataMember(Name = "external_id")]
        public string ExternalId { get; set; }
    }
}
