using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class DelDerivedResResult : BaseResult
    {
        [DataMember(Name = "deleted")]
        public Dictionary<string, string> Deleted { get; protected set; }
        
    }
}
