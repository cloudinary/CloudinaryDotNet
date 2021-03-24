namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// A word representation.
    /// </summary>
    [DataContract]
    public class Word : Block
    {
        /// <summary>
        /// Gets or sets list of symbols in the word. The order of the symbols follows the natural reading order.
        /// </summary>
        [DataMember(Name = "symbols")]
        public List<Symbol> Symbols { get; set; }
    }
}