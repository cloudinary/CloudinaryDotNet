namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The coordinates of a single region contained in an image that is subsequently used for cropping the image using
    /// the custom gravity mode.
    /// </summary>
    [DataContract]
    public class Coordinates
    {
        /// <summary>
        /// Gets or sets a list of custom coordinates.
        /// </summary>
        [DataMember(Name = "custom")]
        public int[][] Custom { get; set; }

        /// <summary>
        /// Gets or sets a list of coordinates of detected faces.
        /// </summary>
        [DataMember(Name = "faces")]
        public int[][] Faces { get; set; }
    }
}