namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Results of generating an image of a given textual string.
    /// </summary>
    [DataContract]
    public class TextResult : BaseResult
    {
        /// <summary>
        /// Gets or sets parameter "width" of the asset.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        /// <summary>
        /// Gets or sets parameter "height" of the asset.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        /// <summary>
        /// Public ID assigned to the asset.
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; protected set; }

        /// <summary>
        /// Version of uploaded asset.
        /// </summary>
        [DataMember(Name = "version")]
        public string Version { get; protected set; }

        /// <summary>
        /// The signature for verifying the response is a valid response from Cloudinary.
        /// </summary>
        [DataMember(Name = "signature")]
        public string Signature { get; protected set; }

        /// <summary>
        /// Asset format.
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; protected set; }

        /// <summary>
        /// The type of file. Possible values: image, raw, video.
        /// </summary>
        [DataMember(Name = "resource_type")]
        protected string m_resourceType;

        /// <summary>
        /// The type of resource. Possible values: image, raw, video.
        /// </summary>
        public ResourceType ResourceType
        {
            get { return Api.ParseCloudinaryParam<ResourceType>(m_resourceType); }
        }

        /// <summary>
        /// Date when the resource was created.
        /// </summary>
        [DataMember(Name = "created_at")]
        public string CreatedAt { get; protected set; }

        /// <summary>
        /// A list of tag names assigned to resource.
        /// </summary>
        [DataMember(Name = "tags")]
        public string[] Tags { get; protected set; }

        /// <summary>
        /// Size of the created archive (in bytes).
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Bytes { get; protected set; }

        /// <summary>
        /// The storage type: upload, private, authenticated, facebook, twitter, gplus, instagram_name, gravatar,
        /// youtube, hulu, vimeo, animoto, worldstarhiphop or dailymotion.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; protected set; }

        /// <summary>
        /// Indicates if a placeholder (default image) is currently used instead of displaying the image (due to moderation).
        /// </summary>
        [DataMember(Name = "placeholder")]
        public bool Placeholder { get; protected set; }

        /// <summary>
        /// The URL for accessing the created archive.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; protected set; }

        /// <summary>
        /// The HTTPS URL for securely accessing the created archive.
        /// </summary>
        [DataMember(Name = "secure_url")]
        public string SecureUrl { get; protected set; }

        /// <summary>
        /// The accessibility mode of the media asset: public, or authenticated.
        /// </summary>
        [DataMember(Name = "access_mode")]
        public string AccessMode { get; protected set; }
    }
}
