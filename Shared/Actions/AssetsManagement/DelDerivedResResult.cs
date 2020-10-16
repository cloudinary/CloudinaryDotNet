namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed result of deletion derived resources.
    /// </summary>
    [DataContract]
    public class DelDerivedResResult : BaseResult
    {
        /// <summary>
        /// Gets or sets the list of media assets requested for deletion, with the status of each asset
        /// (deleted unless there was an issue).
        /// </summary>
        [DataMember(Name = "deleted")]
        public Dictionary<string, string> Deleted { get; set; }
    }
}
