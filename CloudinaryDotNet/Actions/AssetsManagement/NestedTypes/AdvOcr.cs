namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// Details of executing an ADV_OCR engine.
    /// </summary>
    [DataContract]
    [JsonConverter(typeof(AdvOcrDataConverter))]
    public class AdvOcr
    {
        /// <summary>
        /// Gets or sets the status of the OCR operation.
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets data returned by OCR plugin when successful.
        /// This will be null if the operation failed and ErrorMessage contains the error.
        /// </summary>
        [DataMember(Name = "data")]
        public List<AdvOcrData> Data { get; set; }

        /// <summary>
        /// Gets or sets the error message when the OCR operation fails.
        /// This property captures the string value of 'data' field when status is "failed".
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
