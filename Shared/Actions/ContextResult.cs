using System;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Results of tags management
    /// </summary>
    [DataContract]
    public class ContextResult : BaseResult
    {
        /// <summary>
        /// Public IDs of affected images
        /// </summary>
        [DataMember(Name = "public_ids")]
        public string[] PublicIds { get; protected set; }
    }
}
