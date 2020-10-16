namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The results of the advanced image analysis.
    /// </summary>
    [DataContract]
    public class ImageAnalysis
    {
        /// <summary>
        /// Gets or sets amount of faces the image contains.
        /// </summary>
        [DataMember(Name = "face_count")]
        public int FaceCount { get; set; }

        /// <summary>
        /// Gets or sets a list of coordinates of detected faces.
        /// </summary>
        [DataMember(Name = "faces")]
        public int[][] Faces { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the image only contains a single grayscale channel.
        /// </summary>
        [DataMember(Name = "grayscale")]
        public bool GrayScale { get; set; }

        /// <summary>
        /// Gets or sets the likelihood that the image is an illustration as opposed to a photograph.
        /// A value between 0 (photo) and 1.0 (illustration).
        /// </summary>
        [DataMember(Name = "illustration_score")]
        public double IllustrationScore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the image contains one or more colors with an alpha channel.
        /// </summary>
        [DataMember(Name = "transparent")]
        public bool Transparent { get; set; }

        /// <summary>
        /// Gets or sets a values to determine whether two versions of an analysis are identical.
        /// </summary>
        [DataMember(Name = "etag")]
        public string Etag { get; set; }

        /// <summary>
        /// Gets or sets the predominant colors uploaded image.
        /// </summary>
        [DataMember(Name = "colors")]
        public Dictionary<string, double> Colors { get; set; }
    }
}