using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class UpdateTransformResult : BaseResult
    {
        [DataMember(Name = "name")]
        public string Name { get; protected set; }

        [DataMember(Name = "allowed_for_strict")]
        public bool Strict { get; protected set; }

        [DataMember(Name = "used")]
        public bool Used { get; protected set; }

        [DataMember(Name = "info")]
        public Dictionary<string, string>[] Info { get; protected set; }

        [DataMember(Name = "derived")]
        public TransformDerived[] Derived { get; protected set; }
        
    }
}
