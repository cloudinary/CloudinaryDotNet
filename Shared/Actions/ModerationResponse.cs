namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Result of the request for moderation.
    /// </summary>
    [DataContract]
    public class ModerationResponse
    {
        /// <summary>
        /// Detected offensive content categories.
        /// </summary>
        [DataMember(Name = "moderation_labels")]
        public ModerationLabel[] ModerationLabels;
    }
}