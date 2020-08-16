namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Detailed information about user group.
    /// </summary>
    [DataContract]
    public class UserGroupResult : BaseResult
    {
        /// <summary>
        /// Gets or sets the ID of the user group.
        /// </summary>
        [DataMember(Name = "id")]
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user group.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets users of the group.
        /// </summary>
        [DataMember(Name = "users")]
        public string[] Users { get; set; }
    }
}
