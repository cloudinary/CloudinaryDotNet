using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class UpdateResourceAccessModeResult : BaseResult
    {
        [DataMember(Name = "updated")]
        public List<object> Updated { get; protected set; }

        [DataMember(Name = "failed")]
        public List<object> Failed { get; protected set; }
        
    }
}
