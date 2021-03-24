namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Result of removing a user.
    /// </summary>
    [DataContract]
    public class DelUserResult : BaseResult
    {
        /// <summary>
        /// Gets or sets an API message.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
