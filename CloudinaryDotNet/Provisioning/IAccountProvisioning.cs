namespace CloudinaryDotNet.Provisioning
{
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;

    /// <summary>
    /// Account Provisioning Interface.
    /// </summary>
    public interface IAccountProvisioning
    {
        /// <summary>
        /// Retrieves the details of the specified sub-account.
        /// </summary>
        /// <param name="subAccountId">The ID of the sub-account.</param>
        /// <returns>Parsed information about sub-account.</returns>
        SubAccountResult SubAccount(string subAccountId);

        /// <summary>
        /// Retrieves the details of the specified sub-account asynchronously.
        /// </summary>
        /// <param name="subAccountId">The ID of the sub-account.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about sub-account.</returns>
        Task<SubAccountResult> SubAccountAsync(string subAccountId, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Lists sub-accounts.
        /// </summary>
        /// <param name="parameters">Parameters to list sub-accounts.</param>
        /// <returns>Parsed information about sub-accounts.</returns>
        ListSubAccountsResult SubAccounts(ListSubAccountsParams parameters);

        /// <summary>
        /// Lists sub-accounts asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to list sub-accounts.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about sub-accounts.</returns>
        Task<ListSubAccountsResult> SubAccountsAsync(ListSubAccountsParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Creates a new sub-account. Any users that have access to all sub-accounts
        /// will also automatically have access to the new sub-account.
        /// </summary>
        /// <param name="parameters">Parameters to create sub-account.</param>
        /// <returns>Parsed information about created sub-account.</returns>
        SubAccountResult CreateSubAccount(CreateSubAccountParams parameters);

        /// <summary>
        /// Creates a new sub-account asynchronously. Any users that have access to all sub-accounts
        /// will also automatically have access to the new sub-account.
        /// </summary>
        /// <param name="parameters">Parameters to create sub-account.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about created sub-account.</returns>
        Task<SubAccountResult> CreateSubAccountAsync(CreateSubAccountParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Updates the specified details of the sub-account.
        /// </summary>
        /// <param name="parameters">Parameters to update sub-account.</param>
        /// <returns>Parsed information about updated sub-account.</returns>
        SubAccountResult UpdateSubAccount(UpdateSubAccountParams parameters);

        /// <summary>
        /// Updates the specified details of the sub-account asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to update sub-account.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about updated sub-account.</returns>
        Task<SubAccountResult> UpdateSubAccountAsync(UpdateSubAccountParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Deletes the specified sub-account. Supported only for accounts with fewer than 1000 assets.
        /// </summary>
        /// <param name="subAccountId">The ID of the sub-account to delete.</param>
        /// <returns>Parsed information about deleted sub-account.</returns>
        DelSubAccountResult DeleteSubAccount(string subAccountId);

        /// <summary>
        /// Deletes the specified sub-account asynchronously. Supported only for accounts with fewer than 1000 assets.
        /// </summary>
        /// <param name="subAccountId">The ID of the sub-account to delete.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about deleted sub-account.</returns>
        Task<DelSubAccountResult> DeleteSubAccountAsync(string subAccountId, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Returns the user with the specified ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>Parsed information about sub-account.</returns>
        UserResult User(string userId);

        /// <summary>
        /// Returns the user with the specified ID asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about sub-account.</returns>
        Task<UserResult> UserAsync(string userId, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Lists users in the account.
        /// </summary>
        /// <param name="parameters">Parameters to list users.</param>
        /// <returns>Parsed information about users.</returns>
        ListUsersResult Users(ListUsersParams parameters);

        /// <summary>
        /// Lists users in the account asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to list users.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about users.</returns>
        Task<ListUsersResult> UsersAsync(ListUsersParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Creates a new user in the account.
        /// </summary>
        /// <param name="parameters">Parameters to create user.</param>
        /// <returns>Parsed information about created user.</returns>
        UserResult CreateUser(CreateUserParams parameters);

        /// <summary>
        /// Creates a new user in the account asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to create user.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about created user.</returns>
        Task<UserResult> CreateUserAsync(CreateUserParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Updates the details of the specified user.
        /// </summary>
        /// <param name="parameters">Parameters to update user.</param>
        /// <returns>Parsed information about updated user.</returns>
        UserResult UpdateUser(UpdateUserParams parameters);

        /// <summary>
        /// Updates the details of the specified user asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to update user.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about updated user.</returns>
        Task<UserResult> UpdateUserAsync(UpdateUserParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Deletes an existing user.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>Parsed information about deleted user.</returns>
        DelUserResult DeleteUser(string userId);

        /// <summary>
        /// Deletes an existing user asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about deleted user.</returns>
        Task<DelUserResult> DeleteUserAsync(string userId, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Creates a new user group.
        /// </summary>
        /// <param name="parameters">Parameters to create user group.</param>
        /// <returns>Parsed information about created user group.</returns>
        UserGroupResult CreateUserGroup(CreateUserGroupParams parameters);

        /// <summary>
        /// Creates a new user group asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to create user group.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about created user group.</returns>
        Task<UserGroupResult> CreateUserGroupAsync(CreateUserGroupParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Updates the specified user group.
        /// </summary>
        /// <param name="parameters">Parameters to update user group.</param>
        /// <returns>Parsed information about updated user group.</returns>
        UserGroupResult UpdateUserGroup(UpdateUserGroupParams parameters);

        /// <summary>
        /// Updates the specified user group asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to update user group.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about updated user group.</returns>
        Task<UserGroupResult> UpdateUserGroupAsync(UpdateUserGroupParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Deletes the user group with the specified ID.
        /// </summary>
        /// <param name="groupId">The ID of the user group to delete.</param>
        /// <returns>Parsed information about deleted user group.</returns>
        DelUserGroupResult DeleteUserGroup(string groupId);

        /// <summary>
        /// Deletes the user group with the specified ID asynchronously.
        /// </summary>
        /// <param name="groupId">The ID of the user group to delete.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about deleted user group.</returns>
        Task<DelUserGroupResult> DeleteUserGroupAsync(string groupId, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Retrieves the details of the specified user group.
        /// </summary>
        /// <param name="groupId">The ID of the user group to retrieve.</param>
        /// <returns>Parsed information about user group.</returns>
        UserGroupResult UserGroup(string groupId);

        /// <summary>
        /// Retrieves the details of the specified user group asynchronously.
        /// </summary>
        /// <param name="groupId">The ID of the user group to retrieve.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about user group.</returns>
        Task<UserGroupResult> UserGroupAsync(string groupId, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Lists user groups in the account.
        /// </summary>
        /// <returns>Parsed information about user groups.</returns>
        ListUserGroupsResult UserGroups();

        /// <summary>
        /// Lists user groups in the account asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about user groups.</returns>
        Task<ListUserGroupsResult> UserGroupsAsync(CancellationToken? cancellationToken = null);

        /// <summary>
        /// Adds a user to a group with the specified ID.
        /// </summary>
        /// <param name="groupId">The ID of the user group.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>Parsed information about updated user group.</returns>
        ListUsersResult AddUserToGroup(string groupId, string userId);

        /// <summary>
        /// Adds a user to a group with the specified ID asynchronously.
        /// </summary>
        /// <param name="groupId">The ID of the user group.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about updated user group.</returns>
        Task<ListUsersResult> AddUserToGroupAsync(string groupId, string userId, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Removes a user from a group with the specified ID.
        /// </summary>
        /// <param name="groupId">The ID of the user group.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>Parsed information about updated user group.</returns>
        ListUsersResult RemoveUserFromGroup(string groupId, string userId);

        /// <summary>
        /// Removes a user from a group with the specified ID asynchronously.
        /// </summary>
        /// <param name="groupId">The ID of the user group.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about updated user group.</returns>
        Task<ListUsersResult> RemoveUserFromGroupAsync(string groupId, string userId, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Lists users in the specified user group.
        /// </summary>
        /// <param name="groupId">The ID of the user group.</param>
        /// <returns>Parsed information about users.</returns>
        ListUsersResult UsersGroupUsers(string groupId);

        /// <summary>
        /// Lists users in the specified user group asynchronously.
        /// </summary>
        /// <param name="groupId">The ID of the user group.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about users.</returns>
        Task<ListUsersResult> UsersGroupUsersAsync(string groupId, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Lists access keys.
        /// </summary>
        /// <param name="parameters">Parameters to list access keys.</param>
        /// <returns>Parsed information about access keys.</returns>
        ListAccessKeysResult ListAccessKeys(ListAccessKeysParams parameters);

        /// <summary>
        /// Lists access keys asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to list access keys.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about access keys.</returns>
        Task<ListAccessKeysResult> ListAccessKeysAsync(ListAccessKeysParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Generates access key.
        /// </summary>
        /// <param name="parameters">Parameters to generate access key.</param>
        /// <returns>Parsed information about generated access key.</returns>
        AccessKeyResult GenerateAccessKey(GenerateAccessKeyParams parameters);

        /// <summary>
        /// Generates access key asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to generate access key.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about generated access key.</returns>
        Task<AccessKeyResult> GenerateAccessKeyAsync(GenerateAccessKeyParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Updates access key.
        /// </summary>
        /// <param name="parameters">Parameters to update access key.</param>
        /// <returns>Parsed information about updated access key.</returns>
        AccessKeyResult UpdateAccessKey(UpdateAccessKeyParams parameters);

        /// <summary>
        /// Updates access key asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to update access key.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about updated access key.</returns>
        Task<AccessKeyResult> UpdateAccessKeyAsync(UpdateAccessKeyParams parameters, CancellationToken? cancellationToken = null);
    }
}
