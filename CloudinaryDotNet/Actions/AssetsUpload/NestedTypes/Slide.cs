namespace CloudinaryDotNet.Actions
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents settings of a single slide.
    /// Is a part of Slideshow.
    /// </summary>
    public class Slide
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Slide"/> class.
        /// </summary>
        /// <param name="media">The media.</param>
        public Slide(string media)
        {
            Media = media;
        }

        /// <summary>
        /// Gets or sets the media.
        /// Specify images as i:[public_id]. Specify videos as v:[public_id].
        /// </summary>
        [JsonProperty(PropertyName = "media")]
        public string Media { get; set; }

        /// <summary>
        /// Gets or sets the slide type. Set to video when using a video for a slide.
        /// For example: media_v:my-public-id;type_s:video. Server Default: image.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the transition to use from the individual slide to the next. Server Default: CrossZoom.
        /// </summary>
        [JsonProperty(PropertyName = "transition_s")]
        public string Transition { get; set; }

        /// <summary>
        /// Gets or sets the slide duration in milliseconds. Server Default: 3000.
        /// </summary>
        [JsonProperty(PropertyName = "sdur")]
        public int SlideDuration { get; set; }

        /// <summary>
        /// Gets or sets the transition duration in milliseconds. Server Default: 1000.
        /// </summary>
        [JsonProperty(PropertyName = "tdur")]
        public int TransitionDuration { get; set; }
    }
}
