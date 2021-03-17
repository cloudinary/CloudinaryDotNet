namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Structural unit of text representing a number of words in certain order.
    /// </summary>
    [DataContract]
    public class Paragraph : Block
    {
        /// <summary>
        /// Gets or sets list of words in this paragraph.
        /// </summary>
        [DataMember(Name = "words")]
        public List<Word> Words { get; set; }

        /// <summary>
        /// Gets or sets the actual text representation of this paragraph.
        /// </summary>
        [DataMember(Name = "text")]
        public string Text { get; set; }
    }
}