namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Description of the offensive content category.
    /// </summary>
    [DataContract]
    public class ModerationLabel
    {
        /// <summary>
        /// Amazon Rekognition assigns a moderation confidence score (0 - 100) indicating the chances that an image
        /// belongs to an offensive content category.
        /// </summary>
        [DataMember(Name = "confidence")]
        public float Confidence;

        /// <summary>
        /// Name of the offensive content category.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name;

        /// <summary>
        /// Name of the parent offensive content category.
        /// </summary>
        [DataMember(Name = "parent_name")]
        public string ParentName;
    }
}