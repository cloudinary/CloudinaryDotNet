namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed result of creating the archive.
    /// </summary>
    public class ArchiveResult : BaseResult
    {
        /// <summary>
        /// The type of file. Possible values: image, raw, video.
        /// </summary>
        [DataMember(Name = "resource_type")]
        protected string m_resourceType;

        /// <summary>
        /// Gets or sets the URL for accessing the created archive.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the HTTPS URL for securely accessing the created archive.
        /// </summary>
        [DataMember(Name = "secure_url")]
        public string SecureUrl { get; set; }

        /// <summary>
        /// Gets or sets publicId of the created archive.
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; set; }

        /// <summary>
        /// Gets or sets size of the created archive (in bytes).
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Bytes { get; set; }

        /// <summary>
        /// Gets or sets count of files in the archive.
        /// </summary>
        [DataMember(Name = "file_count")]
        public int FileCount { get; set; }

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
        /// Gets the type of resource. Possible values: image, raw, video.
        /// </summary>
        public ResourceType ResourceType
        {
            get { return Api.ParseCloudinaryParam<ResourceType>(m_resourceType); }
        }

        /// <summary>
        /// Gets or sets date when the asset was uploaded.
        /// </summary>
        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the storage type: upload, private, authenticated, facebook, twitter, gplus, instagram_name, gravatar,
        /// youtube, hulu, vimeo, animoto, worldstarhiphop or dailymotion.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets etag used to determine whether two versions of an asset are identical.
        /// </summary>
        [DataMember(Name = "etag")]
        public string Etag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a placeholder (default image) is currently used instead of displaying the image (due to moderation).
        /// </summary>
        [DataMember(Name = "placeholder")]
        public bool Placeholder { get; set; }

        /// <summary>
        /// Gets or sets the accessibility mode of the media asset: public, or authenticated.
        /// </summary>
        [DataMember(Name = "access_mode")]
        public string AccessMode { get; set; }

        /// <summary>
        /// Gets or sets the total number of assets.
        /// </summary>
        [DataMember(Name = "resource_count")]
        public int ResourceCount { get; set; }

        /// <summary>
        /// Gets or sets a list of tag names assigned to resource.
        /// </summary>
        [DataMember(Name = "tags")]
        public string[] Tags { get; set; }
    }
}
