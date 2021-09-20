namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents settings of the slideshow.
    /// Is a part of SlideshowManifest.
    /// </summary>
    public class Slideshow
    {
        /// <summary>
        /// Gets or sets the transition to use for all slides. Server Default: CrossZoom.
        /// </summary>
        [JsonProperty(PropertyName = "transition_s")]
        public string Transition { get; set; }

        /// <summary>
        /// Gets or sets a single transformation to apply to all slides. Server Default: null.
        /// </summary>
        [JsonProperty(PropertyName = "transformation_s")]
        public string Transformation { get; set; }

        /// <summary>
        /// Gets or sets the duration for all slides in milliseconds. Server Default: 3000.
        /// </summary>
        [JsonProperty(PropertyName = "sdur")]
        public int SlideDuration { get; set; }

        /// <summary>
        /// Gets or sets the duration for all transitions in milliseconds. Server Default: 1000.
        /// </summary>
        [JsonProperty(PropertyName = "tdur")]
        public int TransitionDuration { get; set; }

        /// <summary>
        /// Gets or sets the duration for all transitions in milliseconds. Server Default: 1000.
        /// </summary>
        [JsonProperty(PropertyName = "slides")]
        public List<Slide> Slides { get; set; }
    }
}
