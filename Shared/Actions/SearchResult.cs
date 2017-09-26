using System;
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
        [DataMember(Name = "public_id")]
        public string PublicId { get; protected set; }

        [DataMember(Name = "created_at")]
        public string Created { get; protected set; }

        [DataMember(Name = "format")]
        public string Format { get; protected set; }

        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        [DataMember(Name = "bytes")]
        public long Length { get; protected set; }
    }
}
