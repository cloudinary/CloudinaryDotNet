using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parsed result of deletion derived resources.
    /// </summary>
    [DataContract]
    public class DelDerivedResResult : BaseResult
    {
        /// <summary>
        /// The list of media assets requested for deletion, with the status of each asset 
        /// (deleted unless there was an issue).
        /// </summary>
        [DataMember(Name = "deleted")]
        public Dictionary<string, string> Deleted { get; protected set; }
        
    }
}
