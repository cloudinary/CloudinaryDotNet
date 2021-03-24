namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Detected page from OCR.
    /// </summary>
    [DataContract]
    public class Page
    {
        /// <summary>
        /// Gets or sets additional information detected on the page.
        /// </summary>
        [DataMember(Name = "property")]
        public PageProperty Property { get; set; }

        /// <summary>
        /// Gets or sets page width. For PDFs the unit is points. For images (including TIFFs) the unit is pixels.
        /// </summary>
        [DataMember(Name = "width")]
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets page height. For PDFs the unit is points. For images (including TIFFs) the unit is pixels.
        /// </summary>
        [DataMember(Name = "height")]
        public int? Height { get; set; }

        /// <summary>
        /// Gets or sets list of text blocks on this page.
        /// </summary>
        [DataMember(Name = "blocks")]
        public List<TextBlock> Blocks { get; set; }
    }
}