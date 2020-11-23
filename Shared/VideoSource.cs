namespace CloudinaryDotNet
{
    /// <summary>
    /// Source for video tag.
    /// </summary>
    public class VideoSource
    {
        /// <summary>
        /// Gets or sets one of the HTML5 video tag MIME types: video/mp4, video/webm, video/ogg.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a single value, or a comma-separated list of values identifying the codec(s) that should be used to
        /// generate the video. The codec definition can include additional properties,separated with a dot.
        /// For example, codecs="avc1.42E01E,mp4a.40.2".
        /// </summary>
        public string[] Codecs { get; set; }

        /// <summary>
        /// Gets or sets transformation, applied to the <see cref="Type"/> in video tag.
        /// </summary>
        public Transformation Transformation { get; set; }
    }
}