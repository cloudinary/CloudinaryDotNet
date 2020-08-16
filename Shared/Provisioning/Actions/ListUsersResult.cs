namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed result of the users listing request.
    /// </summary>
    [DataContract]
    public class ListUsersResult : BaseResult
    {
        /// <summary>
        /// Gets or sets a list of the users matching the request conditions.
        /// </summary>
        [DataMember(Name = "users")]
        public UserResult[] Users { get; set; }
    }
}
