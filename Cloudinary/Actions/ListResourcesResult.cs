using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    //[DataContract]
    public class ListResourcesResult : BaseResult
    {
        [JsonProperty(PropertyName = "resources")]
        public Resource[] Resources { get; protected set; }

        [JsonProperty(PropertyName = "next_cursor")]
        public string NextCursor { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static ListResourcesResult Parse(HttpWebResponse response)
        {
            return Parse<ListResourcesResult>(response);
        }
    }

    //[DataContract]
    public class Resource : UploadResult
    {
        [JsonProperty(PropertyName = "format")]
        public string Format { get; protected set; }

        [JsonProperty(PropertyName = "resource_type")]
        public string ResourceType { get; protected set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; protected set; }

        [JsonProperty(PropertyName = "created_at")]
        public string Created { get; protected set; }

        [JsonProperty(PropertyName = "bytes")]
        public long Length { get; protected set; }

        [JsonProperty(PropertyName = "width")]
        public int Width { get; protected set; }

        [JsonProperty(PropertyName = "height")]
        public int Height { get; protected set; }

        [JsonProperty(PropertyName = "tags")]
        public string[] Tags { get; protected set; }

        [JsonProperty(PropertyName = "backup")]
        public bool? Backup { get; protected set; }

        [JsonProperty(PropertyName = "moderation_status")]
        public ModerationStatus? ModerationStatus { get; protected set; }

        [JsonProperty(PropertyName = "context")]
        public JToken Context { get; protected set; }
    }
}
