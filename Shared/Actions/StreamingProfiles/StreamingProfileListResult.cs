namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Result of listing of streaming profiles.
    /// </summary>
    [DataContract]
    public class StreamingProfileListResult : BaseResult
    {
        /// <summary>
        /// Gets or sets list of basic details of the streaming profiles.
        /// </summary>
        [DataMember(Name = "data")]
        public IEnumerable<StreamingProfileBaseData> Data { get; set; }
    }
}