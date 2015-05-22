using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Results of image uploading
    /// </summary>
    //[DataContract]
    public class ImageUploadResult : RawUploadResult
    {
        /// <summary>
        /// Image width
        /// </summary>
        [JsonProperty(PropertyName = "width")]
        public int Width { get; protected set; }

        /// <summary>
        /// Image height
        /// </summary>
        [JsonProperty(PropertyName = "height")]
        public int Height { get; protected set; }

        /// <summary>
        /// Image format
        /// </summary>
        [JsonProperty(PropertyName = "format")]
        public string Format { get; protected set; }

        [JsonProperty(PropertyName = "exif")]
        public Dictionary<string, string> Exif { get; protected set; }

        [JsonProperty(PropertyName = "image_metadata")]
        public Dictionary<string, string> Metadata { get; protected set; }

        [JsonProperty(PropertyName = "faces")]
        public int[][] Faces { get; protected set; }

        [JsonProperty(PropertyName = "colors")]
        public string[][] Colors { get; protected set; }

        [JsonProperty(PropertyName = "phash")]
        public string Phash { get; protected set; }

        [JsonProperty(PropertyName = "delete_token")]
        public string DeleteToken { get; protected set; }

        [JsonProperty(PropertyName = "info")]
        public Info Info { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal new static ImageUploadResult Parse(HttpWebResponse response)
        {
            return Parse<ImageUploadResult>(response);
        }
    }
}
