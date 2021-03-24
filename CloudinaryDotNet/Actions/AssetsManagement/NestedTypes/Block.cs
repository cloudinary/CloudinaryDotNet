namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Logical element on the page.
    /// </summary>
    [DataContract]
    public abstract class Block
    {
        /// <summary>
        /// Gets or sets additional information detected on the page.
        /// </summary>
        [DataMember(Name = "property")]
        public PageProperty Property { get; set; }

        /// <summary>
        /// Gets or sets the bounding box for the block.
        /// The vertices are in the order of top-left, top-right, bottom-right, bottom-left.
        /// </summary>
        [DataMember(Name = "boundingBox")]
        public BoundingBlock BoundingBox { get; set; }
    }
}