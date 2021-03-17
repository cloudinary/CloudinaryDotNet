namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// A text element on the page.
    /// </summary>
    [DataContract]
    public class TextBlock : Block
    {
        /// <summary>
        /// Gets or sets list of paragraphs in this block.
        /// </summary>
        [DataMember(Name = "paragraphs")]
        public List<Paragraph> Paragraphs { get; set; }

        /// <summary>
        /// Gets or sets detected block type (text, image etc) for this block.
        /// </summary>
        [DataMember(Name = "blockType")]
        public string BlockType { get; set; }
    }
}