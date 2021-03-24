namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Status of the asset moderation process.
    /// </summary>
    public enum ModerationStatus
    {
        /// <summary>
        /// Moderation is in process.
        /// </summary>
        [EnumMember(Value = "pending")]
        Pending,

        /// <summary>
        /// Asset was rejected by moderation service.
        /// </summary>
        [EnumMember(Value = "rejected")]
        Rejected,

        /// <summary>
        /// Asset approved.
        /// </summary>
        [EnumMember(Value = "approved")]
        Approved,

        /// <summary>
        /// Moderation result was manually overridden.
        /// </summary>
        [EnumMember(Value = "overridden")]
        Overridden,
    }

    /// <summary>
    /// Possible types of resources to store on cloudinary.
    /// </summary>
    public enum ResourceType
    {
        /// <summary>
        /// Images in various formats (jpg, png, etc.)
        /// </summary>
        [EnumMember(Value = "image")]
        Image,

        /// <summary>
        /// Any files (text, binary)
        /// </summary>
        [EnumMember(Value = "raw")]
        Raw,

        /// <summary>
        /// Video files in various formats (mp4, etc.)
        /// </summary>
        [EnumMember(Value = "video")]
        Video,
    }

    /// <summary>
    /// Represents a result of HTTP API call. This is an abstract class.
    /// </summary>
    [DataContract]
    public abstract class BaseResult
    {
        // protected static Dictionary<Type, DataContractJsonSerializer> m_serializers = new Dictionary<Type, DataContractJsonSerializer>();
        private JToken rawJson;

        /// <summary>
        /// Gets or sets hTTP status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets raw JSON as received from the server.
        /// </summary>
        public JToken JsonObj
        {
            get
            {
                return rawJson;
            }

            internal set
            {
                rawJson = value;
                SetValues(value);
            }
        }

        /// <summary>
        /// Gets or sets description of server-side error (if one has occurred).
        /// </summary>
        [DataMember(Name = "error")]
        public Error Error { get; set; }

        /// <summary>
        /// Gets or sets current limit of API requests until <see cref="Reset"/>.
        /// </summary>
        public long Limit { get; set; }

        /// <summary>
        /// Gets or sets remaining amount of requests until <see cref="Reset"/>.
        /// </summary>
        public long Remaining { get; set; }

        /// <summary>
        /// Gets or sets time of next reset of limits.
        /// </summary>
        public DateTime Reset { get; set; }

        /// <summary>
        /// Populates additional token fields.
        /// </summary>
        /// <param name="source">JSON token received from the server.</param>
        internal virtual void SetValues(JToken source)
        {
        }
    }
}
