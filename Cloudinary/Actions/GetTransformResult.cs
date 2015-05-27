using System.IO;
using System.Net;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    //[DataContract]
    public class GetTransformResult : BaseResult
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; protected set; }

        [JsonProperty(PropertyName = "allowed_for_strict")]
        public bool Strict { get; protected set; }

        [JsonProperty(PropertyName = "used")]
        public bool Used { get; protected set; }

        [JsonProperty(PropertyName = "info")]
        public Dictionary<string, object>[] Info { get; protected set; }

        [JsonProperty(PropertyName = "derived")]
        public TransformDerived[] Derived { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static GetTransformResult Parse(HttpWebResponse response)
        {
            return Parse<GetTransformResult>(response);
        }
    }

    //[DataContract]
    public class TransformDerived
    {
        [JsonProperty(PropertyName = "public_id")]
        public string PublicId { get; protected set; }

        [JsonProperty(PropertyName = "resource_type")]
        public string m_resourceType;

        [JsonIgnore]
        public ResourceType ResourceType
        {
            get { return Api.ParseCloudinaryParam<ResourceType>(m_resourceType); }
        }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; protected set; }

        [JsonProperty(PropertyName = "format")]
        public string Format { get; protected set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; protected set; }

        [JsonProperty(PropertyName = "secure_url")]
        public string SecureUrl { get; protected set; }

        [JsonProperty(PropertyName = "bytes")]
        public long Length { get; protected set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; protected set; }
    }
}
