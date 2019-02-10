using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parsed result of deletion resources.
    /// </summary>
    [DataContract]
    public class DelResResult : BaseResult
    {
        /// <summary>
        /// The list of media assets requested for deletion, with the status of each asset (deleted unless there was
        /// an issue).
        /// </summary>
        [DataMember(Name = "deleted")]
        public Dictionary<string, string> Deleted { get; protected set; }

        /// <summary>
        /// When a deletion request has more than 1000 resources to delete, the response includes the 
        /// <see cref="Partial"/> boolean parameter set to true, as well as a <see cref="NextCursor"/> value. You can
        /// then specify this returned <see cref="NextCursor"/> value as the <see cref="DelResParams.NextCursor"/> 
        /// parameter of the following deletion request.
        /// </summary>
        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; protected set; }

        /// <summary>
        /// Whether resources were partially deleted. Use it with the <see cref="NextCursor"/> property.
        /// </summary>
        [DataMember(Name = "partial")]
        public bool Partial { get; protected set; }
    }
}
