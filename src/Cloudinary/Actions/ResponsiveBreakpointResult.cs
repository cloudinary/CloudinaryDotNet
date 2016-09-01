using Newtonsoft.Json;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Results of uploading image with Responsive breakpoints feature
    /// </summary>
    public class ResponsiveBreakpointList
    {
        [JsonProperty("breakpoints")]
        public List<Breakpoint> Breakpoints { get; set; }
    }
    
    /// <summary>
    /// Responsive image breakpoint
    /// </summary>
    [JsonObject]
    public class Breakpoint
    {
        /// <summary>
        /// Image width
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; set; }

        /// <summary>
        /// Image height
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; set; }

        /// <summary>
        /// Bytes
        /// </summary>
        [JsonProperty("bytes")]
        public long Bytes { get; set; }

        /// <summary>
        /// Url of the image
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Secure Url of the image
        /// </summary>
        [JsonProperty("secure_url")]
        public string SecureUrl { get; set; }
    }
}
