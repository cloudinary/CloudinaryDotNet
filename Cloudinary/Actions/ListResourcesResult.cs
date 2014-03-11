using Newtonsoft.Json.Linq;
using System.Net;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class ListResourcesResult : BaseResult
    {
        [DataMember(Name = "resources")]
        public Resource[] Resources { get; protected set; }

        [DataMember(Name = "next_cursor")]
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

    [DataContract]
    public class Resource : UploadResult
    {
        [DataMember(Name = "format")]
        public string Format { get; protected set; }

        [DataMember(Name = "resource_type")]
        public string ResourceType { get; protected set; }

        [DataMember(Name = "type")]
        public string Type { get; protected set; }

        [DataMember(Name = "created_at")]
        public string Created { get; protected set; }

        [DataMember(Name = "bytes")]
        public long Length { get; protected set; }

        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        [DataMember(Name = "tags")]
        public string[] Tags { get; protected set; }

        [DataMember(Name = "backup")]
        public bool? Backup { get; protected set; }

        [DataMember(Name = "moderation_status")]
        public ModerationStatus? ModerationStatus { get; protected set; }

        [DataMember(Name = "context")]
        public JToken Context { get; protected set; }
    }
}
