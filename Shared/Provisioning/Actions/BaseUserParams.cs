namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Base parameters for user modification requests.
    /// </summary>
    public abstract class BaseUserParams : BaseParams
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a unique email address, which serves as the login name and notification address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the role to assign to the user.
        /// </summary>
        public Role? Role { get; set; }

        /// <summary>
        /// Gets or sets the list of sub-account IDs that this user can access.
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
}
