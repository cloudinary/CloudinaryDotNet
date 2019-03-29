using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class SearchResult : BaseResult
    {
        [DataMember(Name = "total_count")]
        public int TotalCount { get; protected set; }

        [DataMember(Name = "time")]
        public long Time { get; protected set; }

        [DataMember(Name = "resources")]
        public List<SearchResource> Resources { get; protected set; }

        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; protected set; }
        
    }

    [DataContract]
    public class SearchResource
    {
        [DataMember(Name = "resource_type")]
        protected string m_resourceType;

        [DataMember(Name = "public_id")]
        public string PublicId { get; protected set; }

        [DataMember(Name = "folder")]
        public string Folder { get; protected set; }

        [DataMember(Name = "filename")]
        public string FileName { get; protected set; }

        [DataMember(Name = "format")]
        public string Format { get; protected set; }

        [DataMember(Name = "version")]
        public string Version { get; protected set; }

        public ResourceType ResourceType
        {
            get { return Api.ParseCloudinaryParam<ResourceType>(m_resourceType); }
        }

        [DataMember(Name = "type")]
        public string Type { get; protected set; }

        [DataMember(Name = "created_at")]
        public string Created { get; protected set; }

        [DataMember(Name = "uploaded_at")]
        public string Uploaded { get; protected set; }

        [DataMember(Name = "bytes")]
        public int Length { get; protected set; }

        [DataMember(Name = "backup_bytes")]
        public int BackupBytes { get; protected set; }

        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        [DataMember(Name = "aspect_ratio")]
        public double AspectRatio { get; protected set; }

        [DataMember(Name = "pixels")]
        public int Pixels { get; protected set; }

        [DataMember(Name = "tags")]
        public string[] Tags { get; protected set; }

        [DataMember(Name = "context")]
        public JToken Context { get; protected set; }

        [DataMember(Name = "image_analysis")]
        public JToken ImageAnalysis { get; protected set; }

        [DataMember(Name = "image_metadata")]
        public Dictionary<string, string> Metadata { get; protected set; }

        [DataMember(Name = "url")]
        public string Url { get; protected set; }

        [DataMember(Name = "secure_url")]
        public string SecureUrl { get; protected set; }

        [DataMember(Name = "status")]
        public string Status { get; protected set; }

        [DataMember(Name = "access_mode")]
        public string AccessMode { get; protected set; }

    }
}
