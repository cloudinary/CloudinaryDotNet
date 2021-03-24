namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Details of an image in the sprite.
    /// </summary>
    [DataContract]
    public class ImageInfo
    {
        /// <summary>
        /// Gets or sets width of the image.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets width of the image.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets x-coordinate of the image in sprite.
        /// </summary>
        [DataMember(Name = "x")]
        public int X { get; set; }

        /// <summary>
        /// Gets or sets y-coordinate of the image in sprite.
        /// </summary>
        [DataMember(Name = "y")]
        public int Y { get; set; }
    }
}