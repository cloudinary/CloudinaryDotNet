namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Result of removing a user group.
    /// </summary>
    [DataContract]
    public class DelUserGroupResult : BaseResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether an API message.
        /// </summary>
        [DataMember(Name = "ok")]
        public bool Ok { get; set; }
    }
}
