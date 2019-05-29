using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parsed result of folder deletion.
    /// </summary>
    [DataContract]
    public class DeleteFolderResult : BaseResult
    {
        /// <summary>
        /// The list of media assets requested for deletion, with the status of each asset (deleted unless there was an issue).
        /// </summary>
        [DataMember(Name = "deleted")]
        public List<string> Deleted { get; protected set; }
    }
}
