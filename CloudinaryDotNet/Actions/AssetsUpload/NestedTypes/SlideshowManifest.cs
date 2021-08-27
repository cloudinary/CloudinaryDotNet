namespace CloudinaryDotNet.Actions
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a manifest for slideshow creation.
    /// </summary>
    public class SlideshowManifest
    {
        /// <summary>
        /// Gets or sets the width of the slideshow in pixels.
        /// </summary>
        [JsonProperty(PropertyName = "w")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the slideshow in pixels.
        /// </summary>
        [JsonProperty(PropertyName = "h")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the duration of the slideshow in seconds. Server Default: 10.
        /// </summary>
        [JsonProperty(PropertyName = "du", NullValueHandling=NullValueHandling.Ignore)]
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets the frames per second of the slideshow. Server Default: 20.
        /// </summary>
        [JsonProperty(PropertyName = "fps", NullValueHandling=NullValueHandling.Ignore)]
        public int Fps { get; set; }

        /// <summary>
        /// Gets or sets the slideshow settings.
        /// </summary>
        [JsonProperty(PropertyName = "vars")]
        public Slideshow Variables { get; set; }
    }
}
