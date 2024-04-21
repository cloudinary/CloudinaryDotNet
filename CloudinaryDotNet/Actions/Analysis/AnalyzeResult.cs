namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed result of the Analyze operation.
    /// </summary>
    [DataContract]
    public class AnalyzeResult : BaseResult
    {
        /// <summary>
        /// Gets or sets the data payload of the analysis result.
        /// </summary>
        [DataMember(Name = "data")]
        public AnalysisPayload Data { get; set; }

        /// <summary>
        /// Gets or sets the request ID associated with the analysis operation.
        /// </summary>
        [DataMember(Name = "request_id")]
        public string RequestId { get; set; }
    }
}
