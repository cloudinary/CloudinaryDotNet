namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

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