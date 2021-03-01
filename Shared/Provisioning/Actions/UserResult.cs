namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

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
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user role.
        /// </summary>
        [DataMember(Name = "role")]
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether account is pending or not. Should always be true for new accounts,
        /// and becomes false upon setting a password.
        /// </summary>
        [DataMember(Name = "pending")]
        public bool Pending { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether account is enabled or not. Should always be true for new accounts.
        /// When false, user should not be able to login to the management console.
        /// </summary>
        [DataMember(Name = "enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a list of sub-accounts to which the user has access.
        /// </summary>
        [JsonConverter(typeof(SafeArrayConverter))]
        [DataMember(Name = "sub_account_ids")]
        public string[] SubAccountIds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has access to all sub-accounts or not.
        /// </summary>
        [DataMember(Name = " all_sub_accounts")]
        public bool AllSubAccounts { get; set; }

        /// <summary>
        /// Gets or sets date when the user was created.
        /// </summary>
        [DataMember(Name = "created_at")]
        public DateTime? CreatedAt { get; set; }
    }
}
