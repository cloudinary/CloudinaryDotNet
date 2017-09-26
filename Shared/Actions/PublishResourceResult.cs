using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class PublishResourceResult : BaseResult
    {
        [DataMember(Name = "published")]
        public List<object> Published { get; protected set; }

        [DataMember(Name = "failed")]
        public List<object> Failed { get; protected set; }
        
    }
}
