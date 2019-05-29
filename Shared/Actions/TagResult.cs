using System;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Results of tags management.
    /// </summary>
    [DataContract]
    public class TagResult : BaseResult
    {
        /// <summary>
        /// A list of public IDs (up to 1000) of affected assets.
        /// </summary>
        [DataMember(Name = "public_ids")]
        public string[] PublicIds { get; protected set; }
        
    }
}
