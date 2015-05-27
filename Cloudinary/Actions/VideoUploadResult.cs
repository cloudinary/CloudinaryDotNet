using Newtonsoft.Json;
using System.Net;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    //[DataContract]
    public class VideoUploadResult : RawUploadResult
    {
        /// <summary>
        /// Video width
        /// </summary>
        [JsonProperty(PropertyName = "width")]
        public int Width { get; protected set; }

        /// <summary>
        /// Video height
        /// </summary>
        [JsonProperty(PropertyName = "height")]
        public int Height { get; protected set; }

        /// <summary>
        /// File format
        /// </summary>
        [JsonProperty(PropertyName = "format")]
        public string Format { get; protected set; }

        /// <summary>
        /// Video information.
        /// </summary>
        [JsonProperty(PropertyName = "video")]
        public Video Video { get; protected set; }

        /// <summary>
        /// Audio information.
        /// </summary>
        [JsonProperty(PropertyName = "audio")]
        public Audio Audio { get; protected set; }

        /// <summary>
        /// Frame rate.
        /// </summary>
        [JsonProperty(PropertyName = "frame_rate")]
        public double FrameRate { get; protected set; }

        /// <summary>
        /// Bit rate.
        /// </summary>
        [JsonProperty(PropertyName = "bit_rate")]
        public int BitRate { get; protected set; }

        /// <summary>
        /// Duration.
        /// </summary>
        [JsonProperty(PropertyName = "duration")]
        public double Duration { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static new VideoUploadResult Parse(HttpWebResponse response)
        {
            return Parse<VideoUploadResult>(response);
        }
    }

    //[DataContract]
    public class Video
    {
        [JsonProperty(PropertyName = "pix_format")]
        public string Format { get; protected set; }

        [JsonProperty(PropertyName = "codec")]
        public string Codec { get; protected set; }

        [JsonProperty(PropertyName = "level")]
        public int Level { get; protected set; }

        [JsonProperty(PropertyName = "bit_rate")]
        public int BitRate { get; protected set; }
    }

    //[DataContract]
    public class Audio
    {
        [JsonProperty(PropertyName = "codec")]
        public string Codec { get; protected set; }

        [JsonProperty(PropertyName = "bit_rate")]
        public int BitRate { get; protected set; }

        [JsonProperty(PropertyName = "frequency")]
        public int Frequency { get; protected set; }

        [JsonProperty(PropertyName = "channels")]
        public int Channels { get; protected set; }

        [JsonProperty(PropertyName = "channel_layout")]
        public string ChannelLayout { get; protected set; }
    }
}
