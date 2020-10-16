namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Parsed results of uploading image with Responsive breakpoints.
    /// </summary>
    public class ResponsiveBreakpointList
    {
        /// <summary>
        /// Gets or sets array of responsive breakpoints found.
        /// </summary>
        [JsonProperty("breakpoints")]
        public List<Breakpoint> Breakpoints { get; set; }

        /// <summary>
        /// Gets or sets the transformation applied to the image before finding the best breakpoints.
        /// </summary>
        [JsonProperty("transformation")]
        public string Transformation { get; set; }
    }
}
