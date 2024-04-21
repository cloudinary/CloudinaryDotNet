namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Payload structure for analysis results.
    /// </summary>
    [DataContract]
    public class AnalysisPayload
    {
        /// <summary>
        /// Gets or sets the entity analyzed.
        /// </summary>
        [DataMember(Name = "entity")]
        public string Entity { get; set; }

        /// <summary>
        /// Gets or sets the analysis data as a dictionary of key-value pairs.
        /// </summary>
        [DataMember(Name = "analysis")]
        public JToken Analysis { get; set; }
    }
}
