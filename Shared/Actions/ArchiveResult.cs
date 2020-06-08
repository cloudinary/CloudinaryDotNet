namespace CloudinaryDotNet.Actions
{
    using System;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Parsed result of creating the archive.
    /// </summary>
    public class ArchiveResult : BaseResult
    {
        /// <summary>
        /// The type of file. Possible values: image, raw, video.
        /// </summary>
        protected string m_resourceType;

        /// <summary>
        /// Gets the URL for accessing the created archive.
        /// </summary>
        public string Url { get; protected set; }

        /// <summary>
        /// Gets the HTTPS URL for securely accessing the created archive.
        /// </summary>
        public string SecureUrl { get; protected set; }

        /// <summary>
        /// Gets publicId of the created archive.
        /// </summary>
        public string PublicId { get; protected set; }

        /// <summary>
        /// Gets size of the created archive (in bytes).
        /// </summary>
        public long Bytes { get; protected set; }

        /// <summary>
        /// Gets count of files in the archive.
        /// </summary>
        public int FileCount { get; private set; }

        /// <summary>
        /// Version of uploaded asset.
        /// </summary>
        public string Version { get; protected set; }

        /// <summary>
        /// The signature for verifying the response is a valid response from Cloudinary.
        /// </summary>
        public string Signature { get; protected set; }

        /// <summary>
        /// The type of resource. Possible values: image, raw, video.
        /// </summary>
        public ResourceType ResourceType
        {
            get { return Api.ParseCloudinaryParam<ResourceType>(m_resourceType); }
        }

        /// <summary>
        /// Date when the asset was uploaded.
        /// </summary>
        public DateTime CreatedAt { get; protected set; }

        /// <summary>
        /// The storage type: upload, private, authenticated, facebook, twitter, gplus, instagram_name, gravatar,
        /// youtube, hulu, vimeo, animoto, worldstarhiphop or dailymotion.
        /// </summary>
        public string Type { get; protected set; }

        /// <summary>
        /// Used to determine whether two versions of an asset are identical.
        /// </summary>
        public string Etag { get; protected set; }

        /// <summary>
        /// Indicates if a placeholder (default image) is currently used instead of displaying the image (due to moderation).
        /// </summary>
        public bool Placeholder { get; protected set; }

        /// <summary>
        /// The accessibility mode of the media asset: public, or authenticated.
        /// </summary>
        public string AccessMode { get; protected set; }

        /// <summary>
        /// The total number of assets.
        /// </summary>
        public int ResourceCount { get; protected set; }

        /// <summary>
        /// A list of tag names assigned to resource.
        /// </summary>
        public string[] Tags { get; protected set; }

        /// <summary>
        /// Overrides corresponding method of <see cref="BaseResult"/> class.
        /// Populates additional token fields.
        /// </summary>
        /// <param name="source">JSON token received from the server.</param>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            Url = source.Value<string>("url");
            SecureUrl = source.Value<string>("secure_url");
            PublicId = source.Value<string>("public_id");
            Bytes = source.Value<long>("bytes");
            FileCount = source.Value<int>("file_count");
            Version = source.Value<string>("version");
            Signature = source.Value<string>("signature");
            m_resourceType = source.Value<string>("resource_type");
            CreatedAt = source.Value<DateTime>("created_at");
            Type = source.Value<string>("type");
            Etag = source.Value<string>("etag");
            Placeholder = source.Value<bool>("placeholder");
            AccessMode = source.Value<string>("access_mode");
            ResourceCount = source.Value<int>("resource_count");
            Tags = source["tags"].ToObject<string[]>();
        }
    }
}
