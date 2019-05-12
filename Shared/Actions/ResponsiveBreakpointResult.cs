using Newtonsoft.Json;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parsed results of uploading image with Responsive breakpoints.
    /// </summary>
    public class ResponsiveBreakpointList
    {
        /// <summary>
        /// Array of responsive breakpoints found.
        /// </summary>
        [JsonProperty("breakpoints")]
        public List<Breakpoint> Breakpoints { get; set; }

        /// <summary>
        /// The transformation applied to the image before finding the best breakpoints.
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
        /// Width of the image. 
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; set; }

        /// <summary>
        /// Width of the image. 
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; set; }

        /// <summary>
        /// Size of the image. 
        /// </summary>
        [JsonProperty("bytes")]
        public long Bytes { get; set; }

        /// <summary>
        /// The URL for accessing the media asset.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// The HTTPS URL for securely accessing the media asset.
        /// </summary>
        [JsonProperty("secure_url")]
        public string SecureUrl { get; set; }
    }
}
