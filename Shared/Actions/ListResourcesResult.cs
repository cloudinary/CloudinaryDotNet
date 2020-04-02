namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Parsed result of the resources listing request.
    /// </summary>
    [DataContract]
    public class ListResourcesResult : BaseResult
    {
        /// <summary>
        /// Gets or sets list of the assets matching the request conditions.
        /// </summary>
        [DataMember(Name = "resources")]
        public Resource[] Resources { get; protected set; }

        /// <summary>
        /// Gets or sets when a listing request has more results to return than <see cref="ListResourcesParams.MaxResults"/>,
        /// the <see cref="NextCursor"/> value is returned as part of the response. You can then specify this value as
        /// the <see cref="ListResourcesParams.NextCursor"/> parameter of the following listing request.
        /// </summary>
        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; protected set; }
    }

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
        public string ResourceType { get; protected set; }

        /// <summary>
        /// Gets or sets the storage type: upload, private, authenticated, facebook, twitter, gplus, instagram_name, gravatar,
        /// youtube, hulu, vimeo, animoto, worldstarhiphop or dailymotion.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; protected set; }

        /// <summary>
        /// Gets or sets the UTC date and time when the asset was originally uploaded in ISO8601 syntax: [yyyy-mm-dd]T[hh:mm:ss]Z.
        /// </summary>
        [DataMember(Name = "created_at")]
        public string Created { get; protected set; }

        /// <summary>
        /// Gets or sets the width of the media asset in pixels.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        /// <summary>
        /// Gets or sets the height of the media asset in pixels.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        /// <summary>
        /// Gets or sets the list of tags currently assigned to the media asset.
        /// </summary>
        [DataMember(Name = "tags")]
        public string[] Tags { get; protected set; }

        /// <summary>
        /// Gets or sets whether the asset is backed up to secondary storage.
        /// </summary>
        [DataMember(Name = "backup")]
        public bool? Backup { get; protected set; }

        /// <summary>
        /// Gets or sets the current moderation status and details if any.
        /// </summary>
        [DataMember(Name = "moderation_status")]
        public ModerationStatus? ModerationStatus { get; protected set; }

        /// <summary>
        /// Gets or sets the key-value pairs of general textual context metadata attached to the media asset.
        /// </summary>
        [DataMember(Name = "context")]
        public JToken Context { get; protected set; }

        /// <summary>
        /// Gets the Fully Qualified Public ID.
        /// </summary>
        public string FullyQualifiedPublicId => $"{ResourceType}/{Type}/{PublicId}";
    }
}
