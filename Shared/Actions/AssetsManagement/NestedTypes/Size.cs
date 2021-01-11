namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// A size of the block, represented by width and height.
    /// </summary>
    [DataContract]
    public class Size
    {
        /// <summary>
        /// Gets or sets width of the block.
        /// </summary>
        [DataMember(Name = "width")]
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets width of the block.
        /// </summary>
        [DataMember(Name = "height")]
        public double Height { get; set; }
    }
}