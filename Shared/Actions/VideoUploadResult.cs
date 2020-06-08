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
        public int Width { get; protected set; }

        /// <summary>
        /// Gets or sets height of the video asset.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; protected set; }

        /// <summary>
        /// Gets or sets the detailed information about the video.
        /// </summary>
        [DataMember(Name = "video")]
        public Video Video { get; protected set; }

        /// <summary>
        /// Gets or sets the detailed information about the audio.
        /// </summary>
        [DataMember(Name = "audio")]
        public Audio Audio { get; protected set; }

        /// <summary>
        /// Gets or sets frame rate of the video.
        /// </summary>
        [DataMember(Name = "frame_rate")]
        public double FrameRate { get; protected set; }

        /// <summary>
        /// Gets or sets bit rate of the video.
        /// </summary>
        [DataMember(Name = "bit_rate")]
        public int BitRate { get; protected set; }

        /// <summary>
        /// Gets or sets duration of the video.
        /// </summary>
        [DataMember(Name = "duration")]
        public double Duration { get; protected set; }

        /// <summary>
        /// The number of page(s) or layers in a multi-page or multi-layer file (PDF, animated GIF, TIFF, PSD).
        /// </summary>
        [DataMember(Name = "pages")]
        public int Pages { get; protected set; }

        /// <summary>
        /// Gets or sets details of cinemagraph analysis for the video.
        /// </summary>
        [DataMember(Name = "cinemagraph_analysis")]
        public CinemagraphAnalysis CinemagraphAnalysis { get; protected set; }

        /// <summary>
        /// A key-value pairs of context associated with the resource.
        /// </summary>
        [DataMember(Name = "context")]
        public Dictionary<string, string> Context { get; protected set; }

        /// <summary>
        /// Determine whether source video has audio.
        /// </summary>
        [DataMember(Name = "is_audio")]
        public bool IsAudio { get; protected set; }

        /// <summary>
        /// Determine whether source video has rotation.
        /// </summary>
        [DataMember(Name = "rotation")]
        public int Rotation { get; protected set; }

        /// <summary>
        /// Amount of nb frames in a source video.
        /// </summary>
        [DataMember(Name = "nb_frames")]
        public int NbFrames { get; protected set; }
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
        public string Format { get; protected set; }

        /// <summary>
        /// Gets or sets the video codec applied.
        /// </summary>
        [DataMember(Name = "codec")]
        public string Codec { get; protected set; }

        /// <summary>
        /// Gets or sets video level.
        /// </summary>
        [DataMember(Name = "level")]
        public int? Level { get; protected set; }

        /// <summary>
        /// Gets or sets bitrate of the video. This parameter controls the number of bits used to represent the video data.
        /// </summary>
        [DataMember(Name = "bit_rate")]
        public int? BitRate { get; protected set; }

        /// <summary>
        /// Applied profile name.
        /// </summary>
        [DataMember(Name = "profile")]
        public string Profile { get; protected set; }
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
        public string Codec { get; protected set; }

        /// <summary>
        /// Gets or sets bitrate of the audio. This parameter controls the number of bits used to represent the audio data.
        /// </summary>
        [DataMember(Name = "bit_rate")]
        public int? BitRate { get; protected set; }

        /// <summary>
        /// Gets or sets audio sampling frequency. Represents an integer value in Hz.
        /// </summary>
        [DataMember(Name = "frequency")]
        public int? Frequency { get; protected set; }

        /// <summary>
        /// Gets or sets audio channel.
        /// </summary>
        [DataMember(Name = "channels")]
        public int? Channels { get; protected set; }

        /// <summary>
        /// Gets or sets audio channel layout.
        /// </summary>
        [DataMember(Name = "channel_layout")]
        public string ChannelLayout { get; protected set; }
    }
}
