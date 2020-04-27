namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// Parameters of list sub-accounts request.
    /// </summary>
    public class ListSubAccountsParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListSubAccountsParams"/> class.
        /// </summary>
        public ListSubAccountsParams()
        {
            Ids = new List<string>();
        }

        /// <summary>
        /// Optional. Whether to return enabled sub-accounts only (true) or disabled accounts (false).
        /// Default: all accounts are returned(both enabled and disabled).
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        /// Optional. List of up to 100 sub-account IDs. When provided, other parameters are ignored.
        /// </summary>
        public List<string> Ids { get; set; }

        /// <summary>
        /// Optional. Returns accounts where the name begins with the specified case-insensitive string.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            // ok
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            if (Enabled.HasValue)
            {
                AddParam(dict, "enabled", Enabled.Value);
            }

            if (Ids != null && Ids.Any())
            {
                AddParam(dict, "ids", Ids);
            }

            if (!string.IsNullOrEmpty(Prefix))
            {
                AddParam(dict, "prefix", Prefix);
            }
        }
    }

    /// <summary>
    /// Base parameters for sub-account modification requests.
    /// </summary>
    public abstract class BaseSubAccountParams : BaseParams
    {
        /// <summary>
        /// The display name as shown in the management console.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Optional. A case-insensitive cloud name comprised of alphanumeric and underscore characters.
        /// Generates an error if the specified cloud name is not unique across all Cloudinary accounts.
        /// Note: Once created, the name can only be changed for accounts with fewer than 1000 assets.
        /// </summary>
        public string CloudName { get; set; }

        /// <summary>
        /// Optional. Any custom attributes you want to associate with the sub-account, as a map/hash of key/value pairs.
        /// </summary>
        public StringDictionary CustomAttributes { get; set; }

        /// <summary>
        /// Optional. Whether the sub-account is enabled. Default: true.
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            if (Enabled.HasValue)
            {
                AddParam(dict, "enabled", Enabled.Value);
            }

            if (!string.IsNullOrEmpty(Name))
            {
                AddParam(dict, "name", Name);
            }

            if (!string.IsNullOrEmpty(CloudName))
            {
                AddParam(dict, "cloud_name", CloudName);
            }

            if (CustomAttributes != null)
            {
                dict.Add("custom_attributes", Utils.SafeJoin("|", CustomAttributes.SafePairs));
            }
        }
    }

    /// <summary>
    /// Parameters of create sub-account request.
    /// </summary>
    public class CreateSubAccountParams : BaseSubAccountParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSubAccountParams"/> class.
        /// </summary>
        /// <param name="subAccountName">The name of the sub-account.</param>
        public CreateSubAccountParams(string subAccountName)
        {
            Name = subAccountName;
            Enabled = true;
        }

        /// <summary>
        /// Optional. The ID of another sub-account, from which to copy all of
        /// the following  settings:Size limits, Timed limits, and Flags.
        /// </summary>
        public string BaseSubAccountId { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            Utils.ShouldNotBeEmpty(() => Name);
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            base.AddParamsToDictionary(dict);
            if (!string.IsNullOrEmpty(BaseSubAccountId))
            {
                AddParam(dict, "base_sub_account_id", BaseSubAccountId);
            }
        }
    }

    /// <summary>
    /// Parameters of update sub-account request.
    /// </summary>
    public class UpdateSubAccountParams : BaseSubAccountParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSubAccountParams"/> class.
        /// </summary>
        /// <param name="subAccountId">The ID of the sub-account.</param>
        public UpdateSubAccountParams(string subAccountId)
        {
            SubAccountId = subAccountId;
        }

        /// <summary>
        /// The ID of the sub-account.
        /// </summary>
        public string SubAccountId { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            Utils.ShouldNotBeEmpty(() => SubAccountId);
        }
    }

    /// <summary>
    /// Parameters of list users request.
    /// </summary>
    public class ListUsersParams : BaseParams
    {
        /// <summary>
        /// Optional. Whether to only return pending users. Default: false (all users).
        /// </summary>
        public bool? Pending { get; set; }

        /// <summary>
        /// Optional. A list of up to 100 user IDs. When provided, other parameters are ignored.
        /// </summary>
        public List<string> UserIds { get; set; }

        /// <summary>
        /// Optional. Returns users where the name or email address begins
        /// with the specified case-insensitive string.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Optional. Only returns users who have access to the specified account.
        /// </summary>
        public string SubAccountId { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            // ok
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            if (Pending.HasValue)
            {
                AddParam(dict, "pending", Pending.Value);
            }

            if (UserIds != null && UserIds.Any())
            {
                AddParam(dict, "ids", UserIds);
            }

            if (!string.IsNullOrEmpty(Prefix))
            {
                AddParam(dict, "prefix", Prefix);
            }

            if (!string.IsNullOrEmpty(SubAccountId))
            {
                AddParam(dict, "sub_account_id", SubAccountId);
            }
        }
    }

    /// <summary>
    /// Base parameters for user modification requests.
    /// </summary>
    public abstract class BaseUserParams : BaseParams
    {
        /// <summary>
        /// The name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A unique email address, which serves as the login name and notification address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The role to assign to the user.
        /// </summary>
        public Role? Role { get; set; }

        /// <summary>
        /// Optional. The list of sub-account IDs that this user can access.
        /// Note: This parameter is ignored if the role is specified as master_admin.
        /// </summary>
        public List<string> SubAccountIds { get; set; }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                AddParam(dict, "name", Name);
            }

            if (!string.IsNullOrEmpty(Email))
            {
                AddParam(dict, "email", Email);
            }

            if (Role.HasValue)
            {
                AddParam(dict, "role", Api.GetCloudinaryParam(Role.Value));
            }

            if (SubAccountIds != null && SubAccountIds.Any())
            {
                AddParam(dict, "sub_account_ids", SubAccountIds);
            }
        }
    }

    /// <summary>
    /// Parameters of create user request.
    /// </summary>
    public class CreateUserParams : BaseUserParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserParams"/> class.
        /// </summary>
        /// <param name="name">Name of the user.</param>
        /// <param name="email">Email of the user.</param>
        /// <param name="role">The role to assign to the user.</param>
        public CreateUserParams(string name, string email, Role role)
        {
            Name = name;
            Email = email;
            Role = role;
        }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            Utils.ShouldNotBeEmpty(() => Name);
            Utils.ShouldNotBeEmpty(() => Email);
            Utils.ShouldBeSpecified(() => Role);
        }
    }

    /// <summary>
    /// Parameters of update user request.
    /// </summary>
    public class UpdateUserParams : BaseUserParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserParams"/> class.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        public UpdateUserParams(string userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// The ID of the user.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            Utils.ShouldNotBeEmpty(() => UserId);
        }
    }

    /// <summary>
    /// Base parameters for user groups modification requests.
    /// </summary>
    public abstract class BaseUserGroupParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseUserGroupParams"/> class.
        /// </summary>
        /// <param name="name">The name of the user group.</param>
        protected BaseUserGroupParams(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of the user group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            Utils.ShouldNotBeEmpty(() => Name);
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            AddParam(dict, "name", Name);
        }
    }

    /// <summary>
    /// Parameters of create user group request.
    /// </summary>
    public class CreateUserGroupParams : BaseUserGroupParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserGroupParams"/> class.
        /// </summary>
        /// <param name="name">The name of the user group.</param>
        public CreateUserGroupParams(string name)
            : base(name)
        {
        }
    }

    /// <summary>
    /// Parameters of update user group request.
    /// </summary>
    public class UpdateUserGroupParams : BaseUserGroupParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserGroupParams"/> class.
        /// </summary>
        /// <param name="userGroupId">The ID of the user group to update.</param>
        /// <param name="name">The name of the user group to update.</param>
        public UpdateUserGroupParams(string userGroupId, string name)
        : base(name)
        {
            UserGroupId = userGroupId;
        }

        /// <summary>
        /// The ID of the user.
        /// </summary>
        public string UserGroupId { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            base.Check();
            Utils.ShouldNotBeEmpty(() => UserGroupId);
        }
    }

    /// <summary>
    /// Possible roles of a user.
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// Has access to all elements of the Cloudinary console, including user
        /// and account management, billing details and purchase/upgrade options,
        /// and full permissions to use all Cloudinary functionality.
        /// </summary>
        [EnumMember(Value = "master_admin")]
        MaserAdmin,

        /// <summary>
        /// Same as a master admin, except that they do not have access to
        /// account management, billing details and purchase/upgrade options.
        /// </summary>
        [EnumMember(Value = "admin")]
        Admin,

        /// <summary>
        /// Can access only billing-related areas of the Cloudinary management console,
        /// including the Billing tab (for paid accounts), usage reports, and purchase/upgrade options.
        /// </summary>
        [EnumMember(Value = "billing")]
        Billing,

        /// <summary>
        /// Same as an Admin, except they do not have access to the List of users area of the User Settings.
        /// </summary>
        [EnumMember(Value = "technical_admin")]
        TechnicalAdmin,

        /// <summary>
        /// Can access only reporting details in the Cloudinary console,
        /// including those in the Dashboard and in the Reports tabs.
        /// </summary>
        [EnumMember(Value = "reports")]
        Reports,

        /// <summary>
        /// Full read-write access to all areas of the Cloudinary console that are related to asset management.
        /// </summary>
        [EnumMember(Value = "media_library_admin")]
        MediaLibraryAdmin,

        /// <summary>
        /// Can access only the Media Library area of the console. The specific read, write,
        /// and other access permissions that a user has within the Media Library are controlled
        /// by the user groups that the user belongs to and the folders that are shared with those user groups.
        /// </summary>
        [EnumMember(Value = "media_library_user")]
        MediaLibraryUser,
    }
}
