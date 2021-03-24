namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Additional information detected on the page.
    /// </summary>
    [DataContract]
    public class PageProperty
    {
        /// <summary>
        /// Gets or sets a list of detected languages together with confidence.
        /// </summary>
        [DataMember(Name = "detectedLanguages")]
        public List<DetectedLanguage> DetectedLanguages { get; set; }
    }
}