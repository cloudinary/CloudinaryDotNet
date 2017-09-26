using System;
using System.Net;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class UsageResult : BaseResult
    {
        [DataMember(Name = "plan")]
        public string Plan { get; protected set; }

        [DataMember(Name = "last_updated")]
        public DateTime LastUpdated { get; protected set; }

        [DataMember(Name = "objects")]
        public Usage Objects { get; protected set; }

        [DataMember(Name = "bandwidth")]
        public Usage Bandwidth { get; protected set; }

        [DataMember(Name = "storage")]
        public Usage Storage { get; protected set; }

        [DataMember(Name = "requests")]
        public int Requests { get; protected set; }

        [DataMember(Name = "resources")]
        public int Resources { get; protected set; }

        [DataMember(Name = "derived_resources")]
        public int DerivedResources { get; protected set; }
        
    }

    [DataContract]
    public class Usage
    {
        [DataMember(Name = "usage")]
        public long Used { get; protected set; }

        [DataMember(Name = "limit")]
        public long Limit { get; protected set; }

        [DataMember(Name = "used_percent")]
        public float UsedPercent { get; protected set; }
    }
}
