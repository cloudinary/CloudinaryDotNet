namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Details of asset version.
    /// </summary>
    [DataContract]
    public class AssetVersion
    {
        /// <summary>
        /// Gets or sets asset version identifier.
        /// </summary>
        [DataMember(Name = "version_id")]
        public string VersionId { get; set; }

        /// <summary>
        /// Gets or sets asset version number.
        /// </summary>
        [DataMember(Name = "version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets asset size in bytes.
        /// </summary>
        [DataMember(Name = "size")]
        public string Size { get; set; }

        /// <summary>
        /// Gets or sets time when version created.
        /// </summary>
        [DataMember(Name = "time")]
        public DateTime Time { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether asset version can be restored.
        /// </summary>
        [DataMember(Name = "restorable")]
        public bool Restorable { get; set; }

        /// <summary>
        /// Gets or sets asset version url.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}
