namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Detailed information about sub-account.
    /// </summary>
    [DataContract]
    public class SubAccountResult : BaseResult
    {
        /// <summary>
        /// Gets or sets an auto-generated unique identifier of the sub-account.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; protected set; }

        /// <summary>
        /// Gets or sets the name of the cloud, provided by user or auto-generated.
        /// </summary>
        [DataMember(Name = "cloud_name")]
        public string CloudName { get; protected set; }

        /// <summary>
        /// Gets or sets the name of the sub-account.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether sub-accounts should always be created as enabled.
        /// Disabled accounts cannot perform any new API operations,
        /// i.e.no image upload, no new transformations.
        /// </summary>
        [DataMember(Name = "enabled")]
        public bool Enabled { get; protected set; }

        /// <summary>
        /// Gets or sets list of ​enabled​ access keys for this account, sorted by ascending order
        /// of creation.On creation there will only be one.
        /// </summary>
        [DataMember(Name = "api_access_keys")]
        public ApiAccessKey[] ApiAccessKeys { get; set; }

        /// <summary>
        /// Gets or sets date when the sub-account was created.
        /// </summary>
        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; protected set; }

        /// <summary>
        /// Gets or sets a list of ​enabled​ access keys for this account, sorted by ascending order
        /// of creation.On creation there will only be one.
        /// </summary>
        [DataMember(Name = "custom_attributes")]
        public StringDictionary CustomAttributes { get; set; }
    }

    /// <summary>
    /// Access key, enabled for the account.
    /// </summary>
    [DataContract]
    public class ApiAccessKey
    {
        /// <summary>
        /// Gets or sets account API key.
        /// </summary>
        [DataMember(Name = "key")]
        public string Key { get; protected set; }

        /// <summary>
        /// Gets or sets the account API secret.
        /// </summary>
        [DataMember(Name = "secret")]
        public string Secret { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether account is enabled or not.
        /// </summary>
        [DataMember(Name = "enabled")]
        public bool Enabled { get; protected set; }
    }

    /// <summary>
    /// Parsed result of the sub-accounts listing request.
    /// </summary>
    [DataContract]
    public class ListSubAccountsResult : BaseResult
    {
        /// <summary>
        /// Gets or sets list of the sub-accounts matching the request conditions.
        /// </summary>
        [DataMember(Name = "sub_accounts")]
        public SubAccountResult[] SubAccounts { get; protected set; }
    }

    /// <summary>
    /// Result of removing a sub-account.
    /// </summary>
    [DataContract]
    public class DelSubAccountResult : BaseResult
    {
        /// <summary>
        /// Gets or sets an API message.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; protected set; }
    }

    /// <summary>
    /// Detailed information about user.
    /// </summary>
    [DataContract]
    public class UserResult : BaseResult
    {
        /// <summary>
        /// Gets or sets an auto-generated unique identifier of the user.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; protected set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; protected set; }

        /// <summary>
        /// Gets or sets the user role.
        /// </summary>
        [DataMember(Name = "role")]
        public string Role { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether account is pending or not. Should always be true for new accounts,
        /// and becomes false upon setting a password.
        /// </summary>
        [DataMember(Name = "pending")]
        public bool Pending { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether account is enabled or not. Should always be true for new accounts.
        /// When false, user should not be able to login to the management console.
        /// </summary>
        [DataMember(Name = "enabled")]
        public bool Enabled { get; protected set; }

        /// <summary>
        /// Gets or sets a list of sub-accounts to which the user has access.
        /// </summary>
        [DataMember(Name = "sub_account_ids")]
        public string[] SubAccountIds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has access to all sub-accounts or not.
        /// </summary>
        [DataMember(Name = " all_sub_accounts")]
        public bool AllSubAccounts { get; protected set; }

        /// <summary>
        /// Gets or sets date when the user was created.
        /// </summary>
        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; protected set; }
    }

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
        public UserResult[] Users { get; protected set; }
    }

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
        public string Message { get; protected set; }
    }

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
        public string GroupId { get; protected set; }

        /// <summary>
        /// Gets or sets the name of the user group.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets users of the group.
        /// </summary>
        [DataMember(Name = "users")]
        public string[] Users { get; protected set; }
    }

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
        public UserGroupResult[] UserGroups { get; protected set; }
    }

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
        public bool Ok { get; protected set; }
    }
}
