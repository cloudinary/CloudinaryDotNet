namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// TextAnnotation contains a structured representation of OCR extracted text.
    /// </summary>
    [DataContract]
    public class TextAnnotation
    {
        /// <summary>
        /// Gets or sets the detected locale of the text.
        /// </summary>
        [DataMember(Name = "locale")]
        public string Locale { get; set; }

        /// <summary>
        /// Gets or sets a description listing the entirety of the detected text content, with a newline character (\n) separating
        /// groups of text.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the outer bounding polygon for the detected image annotation.
        /// </summary>
        [DataMember(Name="boundingPoly")]
        public BoundingBlock BoundingPoly { get; set; }
    }
}