using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Possible moderation statuses
    /// </summary>
    public enum ModerationStatus
    {
        [Description("pending")]
        Pending,
        [Description("rejected")]
        Rejected,
        [Description("approved")]
        Approved,
        [Description("overridden")]
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
        [Description("image")]
        Image,
        /// <summary>
        /// Any files (text, binary)
        /// </summary>
        [Description("raw")]
        Raw,
        /// <summary>
        /// Video files in various formats (mp4, etc.)
        /// </summary>
        [Description("video")]
        Video
    }

    // types to manipulate with generated images
    public enum SpecialImageType
    {
        /// <summary>
        /// Text images
        /// </summary>
        [Description("Text")]
        Text,
        /// <summary>
        /// Sprite images
        /// </summary>
        [Description("Sprite")]
        Sprite,
        /// <summary>
        /// Standard image
        /// </summary>
        None
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
        public HttpStatusCode StatusCode { get; protected set; }

        /// <summary>
        /// Raw JSON as received from server
        /// </summary>
        public JToken JsonObj { get; protected set; }

        /// <summary>
        /// Description of server-side error (if one has occured)
        /// </summary>
        [DataMember(Name = "error")]
        public Error Error { get; protected set; }

        /// <summary>
        /// Gets current limit of API requests until <see cref="Reset"/>.
        /// </summary>
        public long Limit { get; protected set; }

        /// <summary>
        /// Gets remaining amount of requests until <see cref="Reset"/>.
        /// </summary>
        public long Remaining { get; protected set; }

        /// <summary>
        /// Gets time of next reset of limits.
        /// </summary>
        public DateTime Reset { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static T Parse<T>(HttpWebResponse response) where T : BaseResult, new()
        {
            if (response == null)
                throw new ArgumentNullException("response");

            T result = new T();

            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                string s = reader.ReadToEnd();

#if DEBUG
                Console.WriteLine(String.Format("RESPONSE ({0}):", typeof(T).Name));
                Console.WriteLine(s);
#endif

                result = JsonConvert.DeserializeObject<T>(s);
                result.JsonObj = JToken.Parse(s);
            }

            if (response.Headers != null)
                foreach (var header in response.Headers.AllKeys)
                {
                    if (header.StartsWith("X-FeatureRateLimit"))
                    {
                        long l;
                        DateTime t;

                        if (header.EndsWith("Limit") && long.TryParse(response.Headers[header], out l))
                            result.Limit = l;

                        if (header.EndsWith("Remaining") && long.TryParse(response.Headers[header], out l))
                            result.Remaining = l;

                        if (header.EndsWith("Reset") && DateTime.TryParse(response.Headers[header], out t))
                            result.Reset = t;
                    }
                }

            result.StatusCode = response.StatusCode;

            return result;
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
