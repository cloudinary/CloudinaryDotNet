namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Restored resource information.
    /// </summary>
    [DataContract]
    public class RestoredResource
    {
        /// <summary>
        /// The type of file. Possible values: image, raw, video. Default: image.
        /// </summary>
        [DataMember(Name = "resource_type")]
        protected string m_resourceType;

        /// <summary>
        /// Gets or sets publicId of the created archive.
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; set; }

        /// <summary>
        /// Gets or sets version of uploaded asset.
        /// </summary>
        [DataMember(Name = "version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the signature for verifying the response is a valid response from Cloudinary.
        /// </summary>
        [DataMember(Name = "signature")]
        public string Signature { get; set; }

        /// <summary>
        /// Gets or sets parameter "width" of the resource. Not relevant for raw files.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets parameter "height" of the resource. Not relevant for raw files.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets asset format.
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; set; }

        /// <summary>
        /// Gets get the cloudinary resource type.
        /// </summary>
        public ResourceType ResourceType
        {
            get { return Api.ParseCloudinaryParam<ResourceType>(m_resourceType); }
        }

        /// <summary>
        /// Gets or sets the UTC date and time when the asset was originally uploaded in ISO8601 syntax: [yyyy-mm-dd]T[hh:mm:ss]Z.
        /// </summary>
        [DataMember(Name = "created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets a list of tag names assigned to resource.
        /// </summary>
        [DataMember(Name = "tags")]
        public string[] Tags { get; set; }

        /// <summary>
        /// Gets or sets the size of the media asset in bytes.
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Bytes { get; set; }

        /// <summary>
        /// Gets or sets the storage type: upload, private, authenticated, facebook, twitter, gplus, instagram_name, gravatar,
        /// youtube, hulu, vimeo, animoto, worldstarhiphop or dailymotion.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a placeholder (default image) is currently used instead of displaying the image (due to moderation).
        /// </summary>
        [DataMember(Name = "placeholder")]
        public bool Placeholder { get; set; }

        /// <summary>
        /// Gets or sets HTTP URL of asset backup.
        /// </summary>
        [DataMember(Name = "backup_url")]
        public string BackupUrl { get; set; }

        /// <summary>
        /// Gets or sets the accessibility mode of the media asset: public, or authenticated.
        /// </summary>
        [DataMember(Name = "access_mode")]
        public string AccessMode { get; set; }
    }
}