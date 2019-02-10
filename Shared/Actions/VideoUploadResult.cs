using System;
using System.Net;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parsed response after upload of the video resource.
    /// </summary>
    [DataContract]
    public class VideoUploadResult : RawUploadResult
    {
        /// <summary>
        /// Width of the video asset.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; protected set; }

        /// <summary>
        /// Height of the video asset.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        /// <summary>
        /// The detailed information about the video.
        /// </summary>
        [DataMember(Name = "video")]
        public Video Video { get; protected set; }

        /// <summary>
        /// The detailed information about the audio.
        /// </summary>
        [DataMember(Name = "audio")]
        public Audio Audio { get; protected set; }

        /// <summary>
        /// Frame rate of the video.
        /// </summary>
        [DataMember(Name = "frame_rate")]
        public double FrameRate { get; protected set; }

        /// <summary>
        /// Bit rate of the video.
        /// </summary>
        [DataMember(Name = "bit_rate")]
        public int BitRate { get; protected set; }

        /// <summary>
        /// Duration of the video.
        /// </summary>
        [DataMember(Name = "duration")]
        public double Duration { get; protected set; }
        
    }

    /// <summary>
    /// Additional metadata for video.
    /// </summary>
    [DataContract]
    public class Video
    {
        /// <summary>
        /// Type of video format.
        /// </summary>
        [DataMember(Name = "pix_format")]
        public string Format { get; protected set; }

        /// <summary>
        /// The video codec applied.
        /// </summary>
        [DataMember(Name = "codec")]
        public string Codec { get; protected set; }

        /// <summary>
        /// Video level.
        /// </summary>
        [DataMember(Name = "level")]
        public int? Level { get; protected set; }

        /// <summary>
        /// Bitrate of the video. This parameter controls the number of bits used to represent the video data. 
        /// </summary>
        [DataMember(Name = "bit_rate")]
        public int? BitRate { get; protected set; }
    }

    /// <summary>
    /// Additional metadata for audio.
    /// </summary>
    [DataContract]
    public class Audio
    {
        /// <summary>
        /// The audiocodec applied.
        /// </summary>
        [DataMember(Name = "codec")]
        public string Codec { get; protected set; }

        /// <summary>
        /// Bitrate of the audio. This parameter controls the number of bits used to represent the audio data. 
        /// </summary>
        [DataMember(Name = "bit_rate")]
        public int? BitRate { get; protected set; }

        /// <summary>
        /// Audio sampling frequency. Represents an integer value in Hz.
        /// </summary>
        [DataMember(Name = "frequency")]
        public int? Frequency { get; protected set; }

        /// <summary>
        /// Audio channel.
        /// </summary>
        [DataMember(Name = "channels")]
        public int? Channels { get; protected set; }

        /// <summary>
        /// Audio channel layout.
        /// </summary>
        [DataMember(Name = "channel_layout")]
        public string ChannelLayout { get; protected set; }
    }
}
