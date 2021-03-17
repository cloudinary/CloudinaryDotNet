namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed result of folder deletion.
    /// </summary>
    [DataContract]
    public class DeleteFolderResult : BaseResult
    {
        /// <summary>
        /// Gets or sets the list of media assets requested for deletion, with the status of each asset (deleted unless there was an issue).
        /// </summary>
        [DataMember(Name = "deleted")]
        public List<string> Deleted { get; set; }
    }
}
