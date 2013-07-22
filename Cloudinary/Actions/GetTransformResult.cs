using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class GetTransformResult : BaseResult
    {
        [DataMember(Name = "name")]
        public string Name { get; protected set; }

        [DataMember(Name = "allowed_for_strict")]
        public bool Strict { get; protected set; }

        [DataMember(Name = "used")]
        public bool Used { get; protected set; }

        [DataMember(Name = "info")]
        public Dictionary<string, object>[] Info { get; protected set; }

        [DataMember(Name = "derived")]
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

    [DataContract]
    public class TransformDerived
    {
        [DataMember(Name = "public_id")]
        public string PublicId { get; protected set; }

        [DataMember(Name = "resource_type")]
        public string m_resourceType;

        public ResourceType ResourceType
        {
            get { return Api.ParseCloudinaryParam<ResourceType>(m_resourceType); }
        }

        [DataMember(Name = "type")]
        public string Type { get; protected set; }

        [DataMember(Name = "format")]
        public string Format { get; protected set; }

        [DataMember(Name = "url")]
        public string Url { get; protected set; }

        [DataMember(Name = "secure_url")]
        public string SecureUrl { get; protected set; }

        [DataMember(Name = "bytes")]
        public long Length { get; protected set; }

        [DataMember(Name = "id")]
        public string Id { get; protected set; }
    }
}
