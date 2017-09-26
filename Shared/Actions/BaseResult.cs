using System;
using System.Net;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Possible moderation statuses
    /// </summary>
    public enum ModerationStatus
    {
        [EnumMember(Value = "pending")]
        Pending,
        [EnumMember(Value = "rejected")]
        Rejected,
        [EnumMember(Value = "approved")]
        Approved,
        [EnumMember(Value = "overridden")]
        Overridden
    }

    /// <summary>
    /// Possible types of resources to store on cloudinary
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
        Video
    }

    /// <summary>
    /// Results of uploading
    /// </summary>
    [DataContract]
    public abstract class UploadResult : BaseResult
    {
        /// <summary>
        /// Public ID of uploaded resource
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; protected set; }

        /// <summary>
        /// Version of uploaded resource
        /// </summary>
        [DataMember(Name = "version")]
        public string Version { get; protected set; }

        /// <summary>
        /// URL of uploaded resource
        /// </summary>
        [DataMember(Name = "url")]
        public Uri Uri { get; protected set; }

        /// <summary>
        /// Secured URL of uploaded resource
        /// </summary>
        [DataMember(Name = "secure_url")]
        public Uri SecureUri { get; protected set; }

        /// <summary>
        /// Resource length in bytes
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Length { get; protected set; }

        /// <summary>
        /// Resource format
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; protected set; }
    }

    /// <summary>
    /// Represents basic result of HTTP request
    /// </summary>
    [DataContract]
    public abstract class BaseResult
    {
        //protected static Dictionary<Type, DataContractJsonSerializer> m_serializers = new Dictionary<Type, DataContractJsonSerializer>();

        /// <summary>
        /// HTTP status code
        /// </summary>
        public HttpStatusCode StatusCode { get; internal set; }
        private JToken RawJson;
        /// <summary>
        /// Raw JSON as received from server
        /// </summary>
        public JToken JsonObj
        {
            get { return RawJson; }
            internal set
            {
                RawJson = value;
                SetValues(value);
            }
        }

        /// <summary>
        /// Description of server-side error (if one has occured)
        /// </summary>
        [DataMember(Name = "error")]
        public Error Error { get; internal set; }

        /// <summary>
        /// Gets current limit of API requests until <see cref="Reset"/>.
        /// </summary>
        public long Limit { get; internal set; }

        /// <summary>
        /// Gets remaining amount of requests until <see cref="Reset"/>.
        /// </summary>
        public long Remaining { get; internal set; }

        /// <summary>
        /// Gets time of next reset of limits.
        /// </summary>
        public DateTime Reset { get; internal set; }

        internal virtual void SetValues(JToken source)
        {
            
        }
    }

    /// <summary>
    /// Represents server-side error
    /// </summary>
    [DataContract]
    public class Error
    {
        /// <summary>
        /// Error description
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; protected set; }
    }

    [DataContract]
    public class Moderation
    {
        [DataMember(Name = "status")]
        public ModerationStatus Status;

        [DataMember(Name = "kind")]
        public string Kind;

        [DataMember(Name = "response")]
        public string Response;

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt;
    }
}
