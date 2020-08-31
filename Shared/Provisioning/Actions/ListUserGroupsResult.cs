namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed result of the user groups listing request.
    /// </summary>
    [DataContract]
    public class ListUserGroupsResult : BaseResult
    {
        /// <summary>
        /// Gets or sets list of the user groups.
        /// </summary>
        [DataMember(Name = "user_groups")]
        public UserGroupResult[] UserGroups { get; set; }
    }
}
