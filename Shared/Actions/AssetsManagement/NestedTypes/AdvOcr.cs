namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Details of executing an ADV_OCR engine.
    /// </summary>
    [DataContract]
    public class AdvOcr
    {
        /// <summary>
        /// Gets or sets the status of the OCR operation.
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets data returned by OCR plugin.
        /// </summary>
        [DataMember(Name = "data")]
        public List<AdvOcrData> Data { get; set; }
    }
}