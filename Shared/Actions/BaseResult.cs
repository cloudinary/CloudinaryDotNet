namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json;
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
    /// Results of uploading the asset.
    /// </summary>
    [DataContract]
    public abstract class UploadResult : BaseResult
    {
        /// <summary>
        /// Public ID of uploaded asset.
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; protected set; }

        /// <summary>
        /// Version of uploaded asset.
        /// </summary>
        [DataMember(Name = "version")]
        public string Version { get; protected set; }

        /// <summary>
        /// The URL for accessing the uploaded asset.
        /// </summary>
        [DataMember(Name = "url")]
        public Uri Uri { get; protected set; }

        /// <summary>
        /// The HTTPS URL for securely accessing the uploaded asset.
        /// </summary>
        [DataMember(Name = "secure_url")]
        public Uri SecureUri { get; protected set; }

        /// <summary>
        /// Resource length in bytes.
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Length { get; protected set; }

        /// <summary>
        /// Asset format.
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; protected set; }

        /// <inheritdoc/>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            Format = source.ReadValueAsSnakeCase<string>(nameof(Format));
            PublicId = source.ReadValueAsSnakeCase<string>(nameof(PublicId));
            Version = source.ReadValueAsSnakeCase<string>(nameof(Version));
            Length = source.ReadValue<long>("bytes");
            Uri = source.ReadValue<Uri>("url");
            SecureUri = source.ReadValue<Uri>("secure_url");
        }
    }

    /// <summary>
    /// Represents a result of HTTP API call. This is an abstract class.
    /// </summary>
    [DataContract]
    [JsonObject(MemberSerialization.OptOut)]
    public abstract class BaseResult
    {
        // protected static Dictionary<Type, DataContractJsonSerializer> m_serializers = new Dictionary<Type, DataContractJsonSerializer>();
        private JToken rawJson;

        /// <summary>
        /// HTTP status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; internal set; }

        /// <summary>
        /// Raw JSON as received from the server.
        /// </summary>
        [JsonIgnore]
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
        /// Description of server-side error (if one has occurred).
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

        /// <summary>
        /// Populates additional token fields.
        /// </summary>
        /// <param name="source">JSON token received from the server.</param>
        internal virtual void SetValues(JToken source)
        {
            Error = source.ReadObject(nameof(Error).ToLower(), _ => new Error(_));
            StatusCode = source.ReadValueAsSnakeCase<HttpStatusCode>(nameof(StatusCode));
            Remaining = source.ReadValueAsSnakeCase<long>(nameof(Remaining));
            Limit = source.ReadValueAsSnakeCase<long>(nameof(Limit));
            Reset = source.ReadValueAsSnakeCase<DateTime>(nameof(Reset));
        }
    }

    /// <summary>
    /// Represents a server-side error.
    /// </summary>
    [DataContract]
    public class Error
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Error"/> class.
        /// </summary>
        public Error()
        {
        }

        /// <summary>
        /// Error description.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Error"/> class.
        /// </summary>
        /// <param name="source">JSON Token.</param>
        internal Error(JToken source)
        {
            this.Message = source.ReadValueAsSnakeCase<string>(nameof(Message));
        }
    }

    /// <summary>
    /// The data received from image moderation service.
    /// </summary>
    [DataContract]
    public class Moderation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Moderation"/> class.
        /// </summary>
        public Moderation()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Moderation"/> class.
        /// </summary>
        /// <param name="source">JSON Token.</param>
        internal Moderation(JToken source)
        {
            Status = source.ReadValueAsSnakeCase<ModerationStatus>(nameof(Status));
            Kind = source.ReadValueAsSnakeCase<string>(nameof(Kind));
            UpdatedAt = source.ReadValueAsSnakeCase<DateTime>(nameof(UpdatedAt));
            Response = source.ReadObject(nameof(Response).ToCamelCase(), _ => new ModerationResponse(_));
        }

        /// <summary>
        /// Moderation status of assets.
        /// </summary>
        [DataMember(Name = "status")]
        public ModerationStatus Status;

        /// <summary>
        /// Type of image moderation service: "manual", "webpurify", "aws_rek", or "metascan".
        /// </summary>
        [DataMember(Name = "kind")]
        public string Kind;

        /// <summary>
        /// Result of the request for moderation.
        /// </summary>
        [DataMember(Name = "response")]
        [JsonConverter(typeof(ModerationResponseConverter))]
        public ModerationResponse Response;

        /// <summary>
        /// Date of the moderation status update.
        /// </summary>
        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt;
    }

    /// <summary>
    /// Result of the request for moderation.
    /// </summary>
    [DataContract]
    public class ModerationResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModerationResponse"/> class.
        /// </summary>
        public ModerationResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModerationResponse"/> class.
        /// </summary>
        /// <param name="source">JSON Token.</param>
        internal ModerationResponse(JToken source)
        {
            ModerationLabels =
                source
                    .ReadList(nameof(ModerationLabels).ToSnakeCase(), _ => new ModerationLabel(_))
                    .ToArray();
        }

        /// <summary>
        /// Detected offensive content categories.
        /// </summary>
        [DataMember(Name = "moderation_labels")]
        public ModerationLabel[] ModerationLabels;
    }

    /// <summary>
    /// Description of the offensive content category.
    /// </summary>
    [DataContract]
    public class ModerationLabel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModerationLabel"/> class.
        /// </summary>
        public ModerationLabel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModerationLabel"/> class.
        /// </summary>
        /// <param name="source">JSON Token.</param>
        internal ModerationLabel(JToken source)
        {
            Confidence = source.ReadValueAsSnakeCase<float>(nameof(Confidence));
            Name = source.ReadValueAsSnakeCase<string>(nameof(Name));
            ParentName = source.ReadValueAsSnakeCase<string>(nameof(ParentName));
        }

        /// <summary>
        /// Amazon Rekognition assigns a moderation confidence score (0 - 100) indicating the chances that an image
        /// belongs to an offensive content category.
        /// </summary>
        [DataMember(Name = "confidence")]
        public float Confidence;

        /// <summary>
        /// Name of the offensive content category.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name;

        /// <summary>
        /// Name of the parent offensive content category.
        /// </summary>
        [DataMember(Name = "parent_name")]
        public string ParentName;
    }

    /// <summary>
    /// Custom JSON converter to handle responses from moderation plugins properly.
    /// </summary>
    public class ModerationResponseConverter : JsonConverter
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="ModerationResponseConverter"/> can write JSON.
        /// </summary>
        public override bool CanWrite => false;

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            return reader.TokenType == JsonToken.StartObject
                ? serializer.Deserialize(reader, objectType)
                : null;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>True if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType) => true;

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="existingValue">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because of using just for Deserialization");
        }
    }
}
