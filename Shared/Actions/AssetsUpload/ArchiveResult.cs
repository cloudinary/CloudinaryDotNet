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
        /// Gets or sets the URL for accessing the created archive.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the HTTPS URL for securely accessing the created archive.
        /// </summary>
        public string SecureUrl { get; set; }

        /// <summary>
        /// Gets or sets publicId of the created archive.
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// Gets or sets size of the created archive (in bytes).
        /// </summary>
        public long Bytes { get; set; }

        /// <summary>
        /// Gets or sets count of files in the archive.
        /// </summary>
        public int FileCount { get; set; }

        /// <summary>
        /// Gets or sets version of uploaded asset.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the signature for verifying the response is a valid response from Cloudinary.
        /// </summary>
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
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the storage type: upload, private, authenticated, facebook, twitter, gplus, instagram_name, gravatar,
        /// youtube, hulu, vimeo, animoto, worldstarhiphop or dailymotion.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets etag used to determine whether two versions of an asset are identical.
        /// </summary>
        public string Etag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a placeholder (default image) is currently used instead of displaying the image (due to moderation).
        /// </summary>
        public bool Placeholder { get; set; }

        /// <summary>
        /// Gets or sets the accessibility mode of the media asset: public, or authenticated.
        /// </summary>
        public string AccessMode { get; set; }

        /// <summary>
        /// Gets or sets the total number of assets.
        /// </summary>
        public int ResourceCount { get; set; }

        /// <summary>
        /// Gets or sets a list of tag names assigned to resource.
        /// </summary>
        public string[] Tags { get; set; }

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

            var tags = source["tags"];
            if (tags != null)
            {
                Tags = tags.ToObject<string[]>();
            }
        }
    }
}
