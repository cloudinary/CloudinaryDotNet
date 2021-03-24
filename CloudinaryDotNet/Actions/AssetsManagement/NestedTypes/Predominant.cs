namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The predominant colors in the image according to both a Google palette and a Cloudinary palette.
    /// </summary>
    [DataContract]
    public class Predominant
    {
        /// <summary>
        /// Gets or sets google palette details.
        /// </summary>
        [DataMember(Name = "google")]
        public object[][] Google { get; set; }

        /// <summary>
        /// Gets or sets cloudinary palette details.
        /// </summary>
        [DataMember(Name = "cloudinary")]
        public object[][] Cloudinary { get; set; }
    }
}