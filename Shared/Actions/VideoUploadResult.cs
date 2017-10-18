using System;
using System.Net;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class VideoUploadResult : RawUploadResult
    {
        /// <summary>
        /// Video width
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        /// <summary>
        /// Video height
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        /// <summary>
        /// Video information.
        /// </summary>
        [DataMember(Name = "video")]
        public Video Video { get; protected set; }

        /// <summary>
        /// Audio information.
        /// </summary>
        [DataMember(Name = "audio")]
        public Audio Audio { get; protected set; }

        /// <summary>
        /// Frame rate.
        /// </summary>
        [DataMember(Name = "frame_rate")]
        public double FrameRate { get; protected set; }

        /// <summary>
        /// Bit rate.
        /// </summary>
        [DataMember(Name = "bit_rate")]
        public int BitRate { get; protected set; }

        /// <summary>
        /// Duration.
        /// </summary>
        [DataMember(Name = "duration")]
        public double Duration { get; protected set; }
        
    }

    [DataContract]
    public class Video
    {
        [DataMember(Name = "pix_format")]
        public string Format { get; protected set; }

        [DataMember(Name = "codec")]
        public string Codec { get; protected set; }

        [DataMember(Name = "level")]
        public int? Level { get; protected set; }

        [DataMember(Name = "bit_rate")]
        public int? BitRate { get; protected set; }
    }

    [DataContract]
    public class Audio
    {
        [DataMember(Name = "codec")]
        public string Codec { get; protected set; }

        [DataMember(Name = "bit_rate")]
        public int? BitRate { get; protected set; }

        [DataMember(Name = "frequency")]
        public int? Frequency { get; protected set; }

        [DataMember(Name = "channels")]
        public int? Channels { get; protected set; }

        [DataMember(Name = "channel_layout")]
        public string ChannelLayout { get; protected set; }
    }
}
