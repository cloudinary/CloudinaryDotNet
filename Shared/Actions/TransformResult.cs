using System;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parsed response after transformation manipulation.
    /// </summary>
    [DataContract]
    public class TransformResult : BaseResult
    {
        /// <summary>
        /// Result message.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; protected set; }
        
    }
}
