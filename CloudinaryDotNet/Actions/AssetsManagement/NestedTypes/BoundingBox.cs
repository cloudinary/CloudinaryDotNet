namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Identifies the bounding box around the face.
    /// </summary>
    [DataContract]
    public class BoundingBox
    {
        /// <summary>
        /// Gets or sets top left point of the bounding box.
        /// </summary>
        [DataMember(Name = "tl")]
        public Point TopLeft { get; set; }

        /// <summary>
        /// Gets or sets size of the bounding box.
        /// </summary>
        [DataMember(Name = "size")]
        public Size Size { get; set; }
    }
}