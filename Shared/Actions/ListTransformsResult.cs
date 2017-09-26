using System;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class ListTransformsResult : BaseResult
    {
        [DataMember(Name = "transformations")]
        public TransformDesc[] Transformations { get; protected set; }

        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; protected set; }
        
    }

    [DataContract]
    public class TransformDesc
    {
        [DataMember(Name = "name")]
        public string Name { get; protected set; }

        [DataMember(Name = "allowed_for_strict")]
        public bool Strict { get; protected set; }

        [DataMember(Name = "used")]
        public bool Used { get; protected set; }
    }
}
