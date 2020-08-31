namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed response after upload of the video resource.
    /// </summary>
    [DataContract]
    public class VideoUploadResult : RawUploadResult
    {
        /// <summary>
        /// Gets or sets width of the video asset.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets height of the video asset.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the detailed information about the video.
        /// </summary>
        [DataMember(Name = "video")]
        public Video Video { get; set; }

        /// <summary>
        /// Gets or sets the detailed information about the audio.
        /// </summary>
        [DataMember(Name = "audio")]
        public Audio Audio { get; set; }

        /// <summary>
        /// Gets or sets frame rate of the video.
        /// </summary>
        [DataMember(Name = "frame_rate")]
        public double FrameRate { get; set; }

        /// <summary>
        /// Gets or sets bit rate of the video.
        /// </summary>
        [DataMember(Name = "bit_rate")]
        public int BitRate { get; set; }

        /// <summary>
        /// Gets or sets duration of the video.
        /// </summary>
        [DataMember(Name = "duration")]
        public double Duration { get; set; }

        /// <summary>
        /// Gets or sets the number of page(s) or layers in a multi-page or multi-layer file (PDF, animated GIF, TIFF, PSD).
        /// </summary>
        [DataMember(Name = "pages")]
        public int Pages { get; set; }

        /// <summary>
        /// Gets or sets details of cinemagraph analysis for the video.
        /// </summary>
        [DataMember(Name = "cinemagraph_analysis")]
        public CinemagraphAnalysis CinemagraphAnalysis { get; set; }

        /// <summary>
        /// Gets or sets a key-value pairs of context associated with the resource.
        /// </summary>
        [DataMember(Name = "context")]
        public Dictionary<string, string> Context { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether source video has audio.
        /// </summary>
        [DataMember(Name = "is_audio")]
        public bool IsAudio { get; set; }

        /// <summary>
        /// Gets or sets determine whether source video has rotation.
        /// </summary>
        [DataMember(Name = "rotation")]
        public int Rotation { get; set; }

        /// <summary>
        /// Gets or sets amount of nb frames in a source video.
        /// </summary>
        [DataMember(Name = "nb_frames")]
        public int NbFrames { get; set; }
    }

    /// <summary>
    /// Additional metadata for video.
    /// </summary>
    [DataContract]
    public class Video
    {
        /// <summary>
        /// Gets or sets type of video format.
        /// </summary>
        [DataMember(Name = "pix_format")]
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the video codec applied.
        /// </summary>
        [DataMember(Name = "codec")]
        public string Codec { get; set; }

        /// <summary>
        /// Gets or sets video level.
        /// </summary>
        [DataMember(Name = "level")]
        public int? Level { get; set; }

        /// <summary>
        /// Gets or sets bitrate of the video. This parameter controls the number of bits used to represent the video data.
        /// </summary>
        [DataMember(Name = "bit_rate")]
        public int? BitRate { get; set; }

        /// <summary>
        /// Gets or sets applied profile name.
        /// </summary>
        [DataMember(Name = "profile")]
        public string Profile { get; set; }
    }

    /// <summary>
    /// Additional metadata for audio.
    /// </summary>
    [DataContract]
    public class Audio
    {
        /// <summary>
        /// Gets or sets the audiocodec applied.
        /// </summary>
        [DataMember(Name = "codec")]
        public string Codec { get; set; }

        /// <summary>
        /// Gets or sets bitrate of the audio. This parameter controls the number of bits used to represent the audio data.
        /// </summary>
        [DataMember(Name = "bit_rate")]
        public int? BitRate { get; set; }

        /// <summary>
        /// Gets or sets audio sampling frequency. Represents an integer value in Hz.
        /// </summary>
        [DataMember(Name = "frequency")]
        public int? Frequency { get; set; }

        /// <summary>
        /// Gets or sets audio channel.
        /// </summary>
        [DataMember(Name = "channels")]
        public int? Channels { get; set; }

        /// <summary>
        /// Gets or sets audio channel layout.
        /// </summary>
        [DataMember(Name = "channel_layout")]
        public string ChannelLayout { get; set; }
    }
}
