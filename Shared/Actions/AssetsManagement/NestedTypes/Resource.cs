namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Details of a single asset.
    /// </summary>
    [DataContract]
    public class Resource : UploadResult
    {
        /// <summary>
        /// Gets or sets the type of file. Possible values: image, raw, video.
        /// </summary>
        [DataMember(Name = "resource_type")]
        public string ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the storage type: upload, private, authenticated, facebook, twitter, gplus, instagram_name, gravatar,
        /// youtube, hulu, vimeo, animoto, worldstarhiphop or dailymotion.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the UTC date and time when the asset was originally uploaded in ISO8601 syntax: [yyyy-mm-dd]T[hh:mm:ss]Z.
        /// </summary>
        [Obsolete("Property Created is deprecated, please use CreatedAt instead")]
        public string Created
        {
            get { return CreatedAt; }
            set { CreatedAt = value; }
        }

        /// <summary>
        /// Gets or sets the UTC date and time when the asset was originally uploaded in ISO8601 syntax: [yyyy-mm-dd]T[hh:mm:ss]Z.
        /// </summary>
        [DataMember(Name = "created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the width of the media asset in pixels.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the media asset in pixels.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the list of tags currently assigned to the media asset.
        /// </summary>
        [DataMember(Name = "tags")]
        public string[] Tags { get; set; }

        /// <summary>
        /// Gets or sets whether the asset is backed up to secondary storage.
        /// </summary>
        [DataMember(Name = "backup")]
        public bool? Backup { get; set; }

        /// <summary>
        /// Gets or sets the current moderation status and details if any.
        /// </summary>
        [DataMember(Name = "moderation_status")]
        public ModerationStatus? ModerationStatus { get; set; }

        /// <summary>
        /// Gets or sets the key-value pairs of general textual context metadata attached to the media asset.
        /// </summary>
        [DataMember(Name = "context")]
        public JToken Context { get; set; }

        /// <summary>
        /// Gets the Fully Qualified Public ID.
        /// </summary>
        public string FullyQualifiedPublicId => $"{ResourceType}/{Type}/{PublicId}";

        /// <summary>
        /// Gets or sets the accessibility mode of the media asset: public, or authenticated.
        /// </summary>
        [DataMember(Name = "access_mode")]
        public string AccessMode { get; set; }
    }
}