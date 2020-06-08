namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Parsed results of resources restore.
    /// </summary>
    [DataContract]
    public class RestoreResult : BaseResult
    {
        /// <summary>
        /// The type of file. Possible values: image, raw, video. Default: image.
        /// </summary>
        [DataMember(Name = "resource_type")]
        protected string m_resourceType;

        /// <summary>
        /// Gets the cloudinary resource type.
        /// </summary>
        public ResourceType ResourceType
        {
            get { return Api.ParseCloudinaryParam<ResourceType>(m_resourceType); }
        }

        /// <summary>
        /// Collection of restored resources.
        /// </summary>
        public Dictionary<string, RestoredResource> RestoredResources { get; protected set; }

        /// <summary>
        /// Overrides corresponding method of <see cref="BaseResult"/> class.
        /// Populates additional token fields.
        /// </summary>
        /// <param name="source">JSON token received from the server.</param>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            if (RestoredResources == null)
            {
                RestoredResources = new Dictionary<string, RestoredResource>();
            }

            if (source != null)
            {
                // parsing message
                foreach (var resource in source.Children())
                {
                    var tagName = resource.ToObject<JProperty>().Name;
                    var restoredResourceAsObject = resource.ToObject<JProperty>().Value.ToObject<RestoredResource>();

                    RestoredResources.Add(tagName, restoredResourceAsObject);
                }
            }
        }
    }

    /// <summary>
    /// Restored resource information.
    /// </summary>
    [DataContract]
    public class RestoredResource
    {
        /// <summary>
        /// PublicId of the created archive.
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
        /// Parameter "width" of the resource. Not relevant for raw files.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        /// <summary>
        /// Parameter "height" of the resource. Not relevant for raw files.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        /// <summary>
        /// Asset format.
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; protected set; }

        /// <summary>
        /// The type of file. Possible values: image, raw, video. Default: image.
        /// </summary>
        [DataMember(Name = "resource_type")]
        protected string m_resourceType;

        /// <summary>
        /// Get the cloudinary resource type.
        /// </summary>
        public ResourceType ResourceType
        {
            get { return Api.ParseCloudinaryParam<ResourceType>(m_resourceType); }
        }

        /// <summary>
        /// The UTC date and time when the asset was originally uploaded in ISO8601 syntax: [yyyy-mm-dd]T[hh:mm:ss]Z.
        /// </summary>
        [DataMember(Name = "created_at")]
        public string CreatedAt { get; protected set; }

        /// <summary>
        /// A list of tag names assigned to resource.
        /// </summary>
        [DataMember(Name = "tags")]
        public string[] Tags { get; protected set; }

        /// <summary>
        /// The size of the media asset in bytes.
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
        /// HTTP URL of asset backup.
        /// </summary>
        [DataMember(Name = "backup_url")]
        public string BackupUrl { get; protected set; }

        /// <summary>
        /// The accessibility mode of the media asset: public, or authenticated.
        /// </summary>
        [DataMember(Name = "access_mode")]
        public string AccessMode { get; protected set; }
    }
}
