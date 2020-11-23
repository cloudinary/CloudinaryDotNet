namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Details of the streaming profile.
    /// </summary>
    [DataContract]
    public class StreamingProfileData : StreamingProfileBaseData
    {
        /// <summary>
        /// Gets or sets a collection of Representations that defines a custom streaming profile.
        /// </summary>
        [DataMember(Name = "representations")]
        public List<Representation> Representations { get; set; }
    }
}