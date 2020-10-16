namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Data returned by OCR plugin.
    /// </summary>
    [DataContract]
    public class AdvOcrData
    {
        /// <summary>
        /// Gets or sets annotations of the recognized text.
        /// </summary>
        [DataMember(Name = "textAnnotations")]
        public List<TextAnnotation> TextAnnotations { get; set; }

        /// <summary>
        /// Gets or sets this annotation provides the structural hierarchy for the OCR detected text.
        /// If present, text (OCR) detection or document (OCR) text detection has completed successfully.
        /// </summary>
        [DataMember(Name = "fullTextAnnotation")]
        public FullTextAnnotation FullTextAnnotation { get; set; }
    }
}