using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Result of resource publishing.
    /// </summary>
    [DataContract]
    public class PublishResourceResult : BaseResult
    {
        /// <summary>
        /// List of details of published resources.
        /// </summary>
        [DataMember(Name = "published")]
        public List<object> Published { get; protected set; }

        /// <summary>
        /// List of details of the resources with failed publish.
        /// </summary>
        [DataMember(Name = "failed")]
        public List<object> Failed { get; protected set; }
        
    }
}
