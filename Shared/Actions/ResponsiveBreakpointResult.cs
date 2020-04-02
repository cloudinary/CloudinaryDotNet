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

    /// <summary>
    /// Settings of the responsive breakpoints(images) found.
    /// </summary>
    [JsonObject]
    public class Breakpoint
    {
        /// <summary>
        /// Gets or sets width of the image.
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets width of the image.
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets size of the image.
        /// </summary>
        [JsonProperty("bytes")]
        public long Bytes { get; set; }

        /// <summary>
        /// Gets or sets the URL for accessing the media asset.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the HTTPS URL for securely accessing the media asset.
        /// </summary>
        [JsonProperty("secure_url")]
        public string SecureUrl { get; set; }
    }
}
