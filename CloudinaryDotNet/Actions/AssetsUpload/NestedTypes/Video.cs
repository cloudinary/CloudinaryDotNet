namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

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
}