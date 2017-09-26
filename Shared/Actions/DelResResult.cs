using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class DelResResult : BaseResult
    {
        [DataMember(Name = "deleted")]
        public Dictionary<string, string> Deleted { get; protected set; }

        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; protected set; }

        [DataMember(Name = "partial")]
        public bool Partial { get; protected set; }
        
    }
}
