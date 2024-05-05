namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Result of deleting a key.
    /// </summary>
    [DataContract]
    public class DelAccessKeyResult : BaseResult
    {
        /// <summary>
        /// Gets or sets an API message.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
