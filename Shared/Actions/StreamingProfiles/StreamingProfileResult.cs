namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Detailed information about streaming profile.
    /// </summary>
    [DataContract]
    public class StreamingProfileResult : BaseResult
    {
        /// <summary>
        /// Gets or sets an API message.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets details of the streaming profile.
        /// </summary>
        [DataMember(Name = "data")]
        public StreamingProfileData Data { get; set; }
    }
}
