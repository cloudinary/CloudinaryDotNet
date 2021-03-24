namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// A single symbol representation.
    /// </summary>
    [DataContract]
    public class Symbol : Block
    {
        /// <summary>
        /// Gets or sets the actual UTF-8 representation of the symbol.
        /// </summary>
        [DataMember(Name = "text")]
        public string Text { get; set; }
    }
}