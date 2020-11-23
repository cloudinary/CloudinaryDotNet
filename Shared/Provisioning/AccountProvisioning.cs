namespace CloudinaryDotNet.Provisioning
{
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;

    /// <summary>
    /// Account provisioning.
    /// </summary>
    public class AccountProvisioning
    {
        /// <summary>
        /// Gets Provisioning API object that used by this instance.
        /// </summary>
        public ProvisioningApi ProvisioningApi { get; } = new ProvisioningApi();

        /// <summary>
        /// Retrieves the details of the specified sub-account.
        /// </summary>
        /// <param name="subAccountId">The ID of the sub-account.</param>
        /// <returns>Parsed information about sub-account.</returns>
        public SubAccountResult SubAccount(string subAccountId)
        {
            Utils.ShouldNotBeEmpty(() => subAccountId);
            var url = GetSubAccountsUrl(subAccountId);
            return CallAccountApi<SubAccountResult>(HttpMethod.GET, url);
        }

        /// <summary>
        /// Retrieves the details of the specified sub-account asynchronously.
        /// </summary>
        /// <param name="subAccountId">The ID of the sub-account.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about sub-account.</returns>
        public Task<SubAccountResult> SubAccountAsync(string subAccountId, CancellationToken? cancellationToken = null)
        {
            Utils.ShouldNotBeEmpty(() => subAccountId);
            var url = GetSubAccountsUrl(subAccountId);
            return CallAccountApiAsync<SubAccountResult>(HttpMethod.GET, url, cancellationToken);
        }

        /// <summary>
        /// Lists sub-accounts.
        /// </summary>
        /// <param name="parameters">Parameters to list sub-accounts.</param>
        /// <returns>Parsed information about sub-accounts.</returns>
        public ListSubAccountsResult SubAccounts(ListSubAccountsParams parameters)
        {
            var url = GetSubAccountsUrl();
            var urlBuilder = new UrlBuilder(url, parameters.ToParamsDictionary());
            return CallAccountApi<ListSubAccountsResult>(HttpMethod.GET, urlBuilder.ToString());
        }

        /// <summary>
        /// Lists sub-accounts asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to list sub-accounts.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about sub-accounts.</returns>
        public Task<ListSubAccountsResult> SubAccountsAsync(ListSubAccountsParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = GetSubAccountsUrl();
            var urlBuilder = new UrlBuilder(url, parameters.ToParamsDictionary());
            return CallAccountApiAsync<ListSubAccountsResult>(HttpMethod.GET, urlBuilder.ToString(), cancellationToken);
        }

        /// <summary>
        /// Creates a new sub-account. Any users that have access to all sub-accounts
        /// will also automatically have access to the new sub-account.
        /// </summary>
        /// <param name="parameters">Parameters to create sub-account.</param>
        /// <returns>Parsed information about created sub-account.</returns>
        public SubAccountResult CreateSubAccount(CreateSubAccountParams parameters)
        {
            var url = GetSubAccountsUrl();
            return CallAccountApi<SubAccountResult>(HttpMethod.POST, url, parameters);
        }

        /// <summary>
        /// Creates a new sub-account asynchronously. Any users that have access to all sub-accounts
        /// will also automatically have access to the new sub-account.
        /// </summary>
        /// <param name="parameters">Parameters to create sub-account.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about created sub-account.</returns>
        public Task<SubAccountResult> CreateSubAccountAsync(CreateSubAccountParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = GetSubAccountsUrl();
            return CallAccountApiAsync<SubAccountResult>(HttpMethod.POST, url, cancellationToken, parameters);
        }

        /// <summary>
        /// Updates the specified details of the sub-account.
        /// </summary>
        /// <param name="parameters">Parameters to update sub-account.</param>
        /// <returns>Parsed information about updated sub-account.</returns>
        public SubAccountResult UpdateSubAccount(UpdateSubAccountParams parameters)
        {
            var url = GetSubAccountsUrl(parameters.SubAccountId);
            return CallAccountApi<SubAccountResult>(HttpMethod.PUT, url, parameters);
        }

        /// <summary>
        /// Updates the specified details of the sub-account asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to update sub-account.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about updated sub-account.</returns>
        public Task<SubAccountResult> UpdateSubAccountAsync(UpdateSubAccountParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = GetSubAccountsUrl(parameters.SubAccountId);
            return CallAccountApiAsync<SubAccountResult>(HttpMethod.PUT, url, cancellationToken, parameters);
        }

        /// <summary>
        /// Deletes the specified sub-account. Supported only for accounts with fewer than 1000 assets.
        /// </summary>
        /// <param name="subAccountId">The ID of the sub-account to delete.</param>
        /// <returns>Parsed information about deleted sub-account.</returns>
        public DelSubAccountResult DeleteSubAccount(string subAccountId)
        {
            Utils.ShouldNotBeEmpty(() => subAccountId);
            var url = GetSubAccountsUrl(subAccountId);
            return CallAccountApi<DelSubAccountResult>(HttpMethod.DELETE, url);
        }

        /// <summary>
        /// Deletes the specified sub-account asynchronously. Supported only for accounts with fewer than 1000 assets.
        /// </summary>
        /// <param name="subAccountId">The ID of the sub-account to delete.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about deleted sub-account.</returns>
        public Task<DelSubAccountResult> DeleteSubAccountAsync(string subAccountId, CancellationToken? cancellationToken = null)
        {
            Utils.ShouldNotBeEmpty(() => subAccountId);
            var url = GetSubAccountsUrl(subAccountId);
            return CallAccountApiAsync<DelSubAccountResult>(HttpMethod.DELETE, url, cancellationToken);
        }

        /// <summary>
        /// Returns the user with the specified ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>Parsed information about sub-account.</returns>
        public UserResult User(string userId)
        {
            Utils.ShouldNotBeEmpty(() => userId);
            var url = GetUsersUrl(userId);
            return CallAccountApi<UserResult>(HttpMethod.GET, url);
        }

        /// <summary>
        /// Returns the user with the specified ID asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about sub-account.</returns>
        public Task<UserResult> UserAsync(string userId, CancellationToken? cancellationToken = null)
        {
            Utils.ShouldNotBeEmpty(() => userId);
            var url = GetUsersUrl(userId);
            return CallAccountApiAsync<UserResult>(HttpMethod.GET, url, cancellationToken);
        }

        /// <summary>
        /// Lists users in the account.
        /// </summary>
        /// <param name="parameters">Parameters to list users.</param>
        /// <returns>Parsed information about users.</returns>
        public ListUsersResult Users(ListUsersParams parameters)
        {
            var url = GetUsersUrl();
            var urlBuilder = new UrlBuilder(url, parameters.ToParamsDictionary());
            return CallAccountApi<ListUsersResult>(HttpMethod.GET, urlBuilder.ToString());
        }

        /// <summary>
        /// Lists users in the account asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to list users.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about users.</returns>
        public Task<ListUsersResult> UsersAsync(ListUsersParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = GetUsersUrl();
            var urlBuilder = new UrlBuilder(url, parameters.ToParamsDictionary());
            return CallAccountApiAsync<ListUsersResult>(HttpMethod.GET, urlBuilder.ToString(), cancellationToken);
        }

        /// <summary>
        /// Creates a new user in the account.
        /// </summary>
        /// <param name="parameters">Parameters to create user.</param>
        /// <returns>Parsed information about created user.</returns>
        public UserResult CreateUser(CreateUserParams parameters)
        {
            var url = GetUsersUrl();
            return CallAccountApi<UserResult>(HttpMethod.POST, url, parameters);
        }

        /// <summary>
        /// Creates a new user in the account asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to create user.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about created user.</returns>
        public Task<UserResult> CreateUserAsync(CreateUserParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = GetUsersUrl();
            return CallAccountApiAsync<UserResult>(HttpMethod.POST, url, cancellationToken, parameters);
        }

        /// <summary>
        /// Updates the details of the specified user.
        /// </summary>
        /// <param name="parameters">Parameters to update user.</param>
        /// <returns>Parsed information about updated user.</returns>
        public UserResult UpdateUser(UpdateUserParams parameters)
        {
            var url = GetUsersUrl(parameters.UserId);
            return CallAccountApi<UserResult>(HttpMethod.PUT, url, parameters);
        }

        /// <summary>
        /// Updates the details of the specified user asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to update user.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about updated user.</returns>
        public Task<UserResult> UpdateUserAsync(UpdateUserParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = GetUsersUrl(parameters.UserId);
            return CallAccountApiAsync<UserResult>(HttpMethod.PUT, url, cancellationToken, parameters);
        }

        /// <summary>
        /// Deletes an existing user.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>Parsed information about deleted user.</returns>
        public DelUserResult DeleteUser(string userId)
        {
            Utils.ShouldNotBeEmpty(() => userId);
            var url = GetUsersUrl(userId);
            return CallAccountApi<DelUserResult>(HttpMethod.DELETE, url);
        }

        /// <summary>
        /// Deletes an existing user asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about deleted user.</returns>
        public Task<DelUserResult> DeleteUserAsync(string userId, CancellationToken? cancellationToken = null)
        {
            Utils.ShouldNotBeEmpty(() => userId);
            var url = GetUsersUrl(userId);
            return CallAccountApiAsync<DelUserResult>(HttpMethod.DELETE, url, cancellationToken);
        }

        /// <summary>
        /// Creates a new user group.
        /// </summary>
        /// <param name="parameters">Parameters to create user group.</param>
        /// <returns>Parsed information about created user group.</returns>
        public UserGroupResult CreateUserGroup(CreateUserGroupParams parameters)
        {
            var url = GetUserGroupsUrl();
            return CallAccountApi<UserGroupResult>(HttpMethod.POST, url, parameters);
        }

        /// <summary>
        /// Creates a new user group asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to create user group.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about created user group.</returns>
        public Task<UserGroupResult> CreateUserGroupAsync(CreateUserGroupParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = GetUserGroupsUrl();
            return CallAccountApiAsync<UserGroupResult>(HttpMethod.POST, url, cancellationToken, parameters);
        }

        /// <summary>
        /// Updates the specified user group.
        /// </summary>
        /// <param name="parameters">Parameters to update user group.</param>
        /// <returns>Parsed information about updated user group.</returns>
        public UserGroupResult UpdateUserGroup(UpdateUserGroupParams parameters)
        {
            var url = GetUserGroupsUrl(parameters.UserGroupId);
            return CallAccountApi<UserGroupResult>(HttpMethod.PUT, url, parameters);
        }

        /// <summary>
        /// Updates the specified user group asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to update user group.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about updated user group.</returns>
        public Task<UserGroupResult> UpdateUserGroupAsync(UpdateUserGroupParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = GetUserGroupsUrl(parameters.UserGroupId);
            return CallAccountApiAsync<UserGroupResult>(HttpMethod.PUT, url, cancellationToken, parameters);
        }

        /// <summary>
        /// Deletes the user group with the specified ID.
        /// </summary>
        /// <param name="groupId">The ID of the user group to delete.</param>
        /// <returns>Parsed information about deleted user group.</returns>
        public DelUserGroupResult DeleteUserGroup(string groupId)
        {
            Utils.ShouldNotBeEmpty(() => groupId);
            var url = GetUserGroupsUrl(groupId);
            return CallAccountApi<DelUserGroupResult>(HttpMethod.DELETE, url);
        }

        /// <summary>
        /// Deletes the user group with the specified ID asynchronously.
        /// </summary>
        /// <param name="groupId">The ID of the user group to delete.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about deleted user group.</returns>
        public Task<DelUserGroupResult> DeleteUserGroupAsync(string groupId, CancellationToken? cancellationToken = null)
        {
            Utils.ShouldNotBeEmpty(() => groupId);
            var url = GetUserGroupsUrl(groupId);
            return CallAccountApiAsync<DelUserGroupResult>(HttpMethod.DELETE, url, cancellationToken);
        }

        /// <summary>
        /// Retrieves the details of the specified user group.
        /// </summary>
        /// <param name="groupId">The ID of the user group to retrieve.</param>
        /// <returns>Parsed information about user group.</returns>
        public UserGroupResult UserGroup(string groupId)
        {
            Utils.ShouldNotBeEmpty(() => groupId);
            var url = GetUserGroupsUrl(groupId);
            return CallAccountApi<UserGroupResult>(HttpMethod.GET, url);
        }

        /// <summary>
        /// Retrieves the details of the specified user group asynchronously.
        /// </summary>
        /// <param name="groupId">The ID of the user group to retrieve.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about user group.</returns>
        public Task<UserGroupResult> UserGroupAsync(string groupId, CancellationToken? cancellationToken = null)
        {
            Utils.ShouldNotBeEmpty(() => groupId);
            var url = GetUserGroupsUrl(groupId);
            return CallAccountApiAsync<UserGroupResult>(HttpMethod.GET, url, cancellationToken);
        }

        /// <summary>
        /// Lists user groups in the account.
        /// </summary>
        /// <returns>Parsed information about user groups.</returns>
        public ListUserGroupsResult UserGroups()
        {
            var url = GetUserGroupsUrl();
            return CallAccountApi<ListUserGroupsResult>(HttpMethod.GET, url);
        }

        /// <summary>
        /// Lists user groups in the account asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about user groups.</returns>
        public Task<ListUserGroupsResult> UserGroupsAsync(CancellationToken? cancellationToken = null)
        {
            var url = GetUserGroupsUrl();
            return CallAccountApiAsync<ListUserGroupsResult>(HttpMethod.GET, url, cancellationToken);
        }

        /// <summary>
        /// Adds a user to a group with the specified ID.
        /// </summary>
        /// <param name="groupId">The ID of the user group.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>Parsed information about updated user group.</returns>
        public ListUsersResult AddUserToGroup(string groupId, string userId)
        {
            return ChangeUserGroup(groupId, userId, HttpMethod.POST);
        }

        /// <summary>
        /// Adds a user to a group with the specified ID asynchronously.
        /// </summary>
        /// <param name="groupId">The ID of the user group.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about updated user group.</returns>
        public Task<ListUsersResult> AddUserToGroupAsync(string groupId, string userId, CancellationToken? cancellationToken = null)
        {
            return ChangeUserGroupAsync(groupId, userId, HttpMethod.POST, cancellationToken);
        }

        /// <summary>
        /// Removes a user from a group with the specified ID.
        /// </summary>
        /// <param name="groupId">The ID of the user group.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>Parsed information about updated user group.</returns>
        public ListUsersResult RemoveUserFromGroup(string groupId, string userId)
        {
            return ChangeUserGroup(groupId, userId, HttpMethod.DELETE);
        }

        /// <summary>
        /// Removes a user from a group with the specified ID asynchronously.
        /// </summary>
        /// <param name="groupId">The ID of the user group.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about updated user group.</returns>
        public Task<ListUsersResult> RemoveUserFromGroupAsync(string groupId, string userId, CancellationToken? cancellationToken = null)
        {
            return ChangeUserGroupAsync(groupId, userId, HttpMethod.DELETE, cancellationToken);
        }

        /// <summary>
        /// Lists users in the specified user group.
        /// </summary>
        /// <param name="groupId">The ID of the user group.</param>
        /// <returns>Parsed information about users.</returns>
        public ListUsersResult UsersGroupUsers(string groupId)
        {
            Utils.ShouldNotBeEmpty(() => groupId);
            var url = GetUserGroupsUrlForUsers(groupId);
            return CallAccountApi<ListUsersResult>(HttpMethod.GET, url);
        }

        /// <summary>
        /// Lists users in the specified user group asynchronously.
        /// </summary>
        /// <param name="groupId">The ID of the user group.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed information about users.</returns>
        public Task<ListUsersResult> UsersGroupUsersAsync(string groupId, CancellationToken? cancellationToken = null)
        {
            Utils.ShouldNotBeEmpty(() => groupId);
            var url = GetUserGroupsUrlForUsers(groupId);
            return CallAccountApiAsync<ListUsersResult>(HttpMethod.GET, url, cancellationToken);
        }

        private static string UrlWithOptionalParameter(Url baseUrl, string urlParameter)
        {
            if (!string.IsNullOrEmpty(urlParameter))
            {
                baseUrl.Add(urlParameter);
            }

            return baseUrl.BuildUrl();
        }

        private ListUsersResult ChangeUserGroup(string groupId, string userId, HttpMethod httpMethod)
        {
            Utils.ShouldNotBeEmpty(() => groupId);
            Utils.ShouldNotBeEmpty(() => userId);

            var url = GetUserGroupsUrlForUsers(groupId, userId);
            return CallAccountApi<ListUsersResult>(httpMethod, url);
        }

        private Task<ListUsersResult> ChangeUserGroupAsync(string groupId, string userId, HttpMethod httpMethod, CancellationToken? cancellationToken)
        {
            Utils.ShouldNotBeEmpty(() => groupId);
            Utils.ShouldNotBeEmpty(() => userId);

            var url = GetUserGroupsUrlForUsers(groupId, userId);
            return CallAccountApiAsync<ListUsersResult>(httpMethod, url, cancellationToken);
        }

        private T CallAccountApi<T>(HttpMethod httpMethod, string url, BaseParams parameters = null)
            where T : BaseResult, new()
        {
            if (httpMethod == HttpMethod.POST || httpMethod == HttpMethod.PUT)
            {
                return ProvisioningApi.CallAccountApi<T>(httpMethod, url, parameters, null, Utils.PrepareJsonHeaders());
            }

            return ProvisioningApi.CallAccountApi<T>(httpMethod, url, parameters, null);
        }

        private Task<T> CallAccountApiAsync<T>(
            HttpMethod httpMethod,
            string url,
            CancellationToken? cancellationToken,
            BaseParams parameters = null)
            where T : BaseResult, new()
        {
            if (httpMethod == HttpMethod.POST || httpMethod == HttpMethod.PUT)
            {
                return ProvisioningApi.CallAccountApiAsync<T>(httpMethod, url, parameters, null, Utils.PrepareJsonHeaders(), cancellationToken);
            }

            return ProvisioningApi.CallAccountApiAsync<T>(httpMethod, url, parameters, null, null, cancellationToken);
        }

        private string GetSubAccountsUrl(string subAccountId = null)
        {
            return BuildAccountApiUrl(Constants.SUB_ACCOUNTS, subAccountId);
        }

        private string GetUsersUrl(string userId = null)
        {
            return BuildAccountApiUrl(Constants.USERS, userId);
        }

        private string GetUserGroupsUrl(string groupId = null)
        {
            return BuildAccountApiUrl(Constants.USER_GROUPS, groupId);
        }

        private string GetUserGroupsUrlForUsers(string groupId, string userId = null)
        {
            var baseUrl = ProvisioningApi.AccountApiUrlV.Add(Constants.USER_GROUPS).Add(groupId).Add(Constants.USERS);
            return UrlWithOptionalParameter(baseUrl, userId);
        }

        private string BuildAccountApiUrl(string resourceName, string urlParameter)
        {
            var baseUrl = ProvisioningApi.AccountApiUrlV.Add(resourceName);
            return UrlWithOptionalParameter(baseUrl, urlParameter);
        }
    }
}
