using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parsed result of the resources listing request.
    /// </summary>
    [DataContract]
    public class ListResourcesResult : BaseResult
    {
        /// <summary>
        /// List of the assets matching the request conditions.
        /// </summary>
        [DataMember(Name = "resources")]
        public Resource[] Resources { get; protected set; }

        /// <summary>
        /// When a listing request has more results to return than <see cref="ListResourcesParams.MaxResults"/>, 
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
        /// The type of file. Possible values: image, raw, video.
        /// </summary>
        [DataMember(Name = "resource_type")]
        public string ResourceType { get; protected set; }

        /// <summary>
        /// The storage type: upload, private, authenticated, facebook, twitter, gplus, instagram_name, gravatar,
        /// youtube, hulu, vimeo, animoto, worldstarhiphop or dailymotion. 
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; protected set; }

        /// <summary>
        /// The UTC date and time when the asset was originally uploaded in ISO8601 syntax: [yyyy-mm-dd]T[hh:mm:ss]Z.
        /// </summary>
        [DataMember(Name = "created_at")]
        public string Created { get; protected set; }

        /// <summary>
        /// The width of the media asset in pixels. 
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        /// <summary>
        /// The height of the media asset in pixels.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        /// <summary>
        /// The list of tags currently assigned to the media asset.
        /// </summary>
        [DataMember(Name = "tags")]
        public string[] Tags { get; protected set; }

        /// <summary>
        /// Indicates whether the asset is backed up to secondary storage.
        /// </summary>
        [DataMember(Name = "backup")]
        public bool? Backup { get; protected set; }

        /// <summary>
        /// The current moderation status and details if any.
        /// </summary>
        [DataMember(Name = "moderation_status")]
        public ModerationStatus? ModerationStatus { get; protected set; }

        /// <summary>
        /// The key-value pairs of general textual context metadata attached to the media asset.
        /// </summary>
        [DataMember(Name = "context")]
        public JToken Context { get; protected set; }
    }
}
