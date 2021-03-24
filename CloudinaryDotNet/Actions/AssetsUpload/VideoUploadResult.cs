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
}
