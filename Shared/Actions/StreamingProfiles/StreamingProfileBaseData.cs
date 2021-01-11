namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Basic details of the streaming profile.
    /// </summary>
    [DataContract]
    public class StreamingProfileBaseData
    {
        /// <summary>
        /// Gets or sets the identification name of the new streaming profile.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a descriptive name for the profile.
        /// </summary>
        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether streaming profile is defined.
        /// </summary>
        [DataMember(Name = "predefined")]
        public bool Predefined { get; set; }
    }
}