using System.IO;
using System.Net;
using System.Runtime.Serialization;
//using System.Runtime.Serialization.Json;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Results of tags management
    /// </summary>
    //[DataContract]
    public class SpriteResult : BaseResult
    {
        [JsonProperty(PropertyName = "css_url")]
        public Uri CssUri { get; protected set; }

        [JsonProperty(PropertyName = "secure_css_url")]
        public Uri SecureCssUri { get; protected set; }

        [JsonProperty(PropertyName = "image_url")]
        public Uri ImageUri { get; protected set; }

        [JsonProperty(PropertyName = "json_url")]
        public Uri JsonUri { get; protected set; }

        [JsonProperty(PropertyName = "public_id")]
        public string PublicId { get; protected set; }

        [JsonProperty(PropertyName = "version")]
        public string Version { get; protected set; }

        [JsonProperty(PropertyName = "image_infos")]
        public Dictionary<string, ImageInfo> ImageInfos { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static SpriteResult Parse(HttpWebResponse response)
        {
            return Parse<SpriteResult>(response);
        }
    }

    //[DataContract]
    public class ImageInfo
    {
        [JsonProperty(PropertyName = "width")]
        public int Width { get; protected set; }

        [JsonProperty(PropertyName = "height")]
        public int Height { get; protected set; }

        [JsonProperty(PropertyName = "x")]
        public int X { get; protected set; }

        [JsonProperty(PropertyName = "y")]
        public int Y { get; protected set; }
    }
}
