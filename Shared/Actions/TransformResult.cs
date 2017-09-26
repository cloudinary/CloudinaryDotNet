using System;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class TransformResult : BaseResult
    {
        [DataMember(Name = "message")]
        public string Message { get; protected set; }
        
    }
}
