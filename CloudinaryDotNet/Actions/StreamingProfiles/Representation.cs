namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// Details of the transformation parameters for the representation.
    /// </summary>
    [DataContract]
    public class Representation
    {
        /// <summary>
        /// Specifies the transformation parameters for the representation.
        /// All video transformation parameters except video_sampling are supported.
        /// Common transformation parameters for representations include: width, height
        /// (or aspect_ratio), bit_rate, video_codec, audio_codec, sample_rate (or fps), etc.
        /// </summary>
        [DataMember(Name = "transformation")]
        [JsonConverter(typeof(RepresentationsConverter))]
        public Transformation Transformation;
    }
}
