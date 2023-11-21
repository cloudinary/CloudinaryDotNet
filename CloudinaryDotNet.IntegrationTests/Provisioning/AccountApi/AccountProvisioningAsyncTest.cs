using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTests.Provisioning.AccountApi
{
    public class AccountProvisioningAsyncTest : ProvisioningIntegrationTestBase
    {
        [Test, RetryWithDelay]
        public async Task TestUpdateSubAccount()
        {
            var newName = GetCloudName();
            var updateSubAccountParams = new UpdateSubAccountParams(m_cloudId1)
            {
                CloudName = newName
            };

            var updateResult = await AccountProvisioning.UpdateSubAccountAsync(updateSubAccountParams);

            Assert.AreEqual(HttpStatusCode.OK, updateResult.StatusCode, updateResult.Error?.Message);

            var result = await AccountProvisioning.SubAccountAsync(m_cloudId1);

            Assert.AreEqual(newName, result.CloudName);
        }

        [Test, RetryWithDelay]
        public async Task TestGetAllSubAccounts()
        {
            var listSubAccountsParams = new ListSubAccountsParams
            {
                Enabled = true
            };

            var result = await AccountProvisioning.SubAccountsAsync(listSubAccountsParams);

            Assert.NotNull(result.SubAccounts.FirstOrDefault(subAccount => subAccount.Id == m_cloudId1));
        }

        [Test, RetryWithDelay]
        public async Task TestGetSpecificSubAccount()
        {
            var result = await  AccountProvisioning.SubAccountAsync(m_cloudId1);

            Assert.AreEqual(m_cloudId1, result.Id);
        }

        [Test, RetryWithDelay]
        public async Task TestUpdateUser()
        {
            var newEmailAddress = $"updated-async+{m_timestampSuffix}@cloudinary.com";
            const string newName = "updated-async";
            var updateUserParams = new UpdateUserParams(m_userId1)
            {
                Email = newEmailAddress,
                Name = newName,
                SubAccountIds = new List<string> {m_cloudId1, m_cloudId2}
            };

            var updateUserResult = await  AccountProvisioning.UpdateUserAsync(updateUserParams);
            Assert.AreEqual(newName, updateUserResult.Name);
            Assert.AreEqual(newEmailAddress, updateUserResult.Email);
            Assert.AreEqual(2, updateUserResult.SubAccountIds.Length);
            Assert.That( new[] {m_cloudId1, m_cloudId2}, Is.EquivalentTo( updateUserResult.SubAccountIds ) );

            var getUserResult = await AccountProvisioning.UserAsync(m_userId1);
            Assert.AreEqual(m_userId1, getUserResult.Id);
            Assert.AreEqual(newEmailAddress, getUserResult.Email);

            var listUsersResult = await AccountProvisioning.UsersAsync(new ListUsersParams());
            var foundUser = listUsersResult.Users.FirstOrDefault(user => user.Id == m_userId1);
            Assert.NotNull(foundUser);
            Assert.AreEqual(newEmailAddress, foundUser.Email);
        }

        [Test, RetryWithDelay]
        public async Task TestGetUsersInAListOfUserIds()
        {
            var listUsersParams = new ListUsersParams
            {
                UserIds = new List<string> {m_userId1}
            };

            var result = await AccountProvisioning.UsersAsync(listUsersParams);

            Assert.AreEqual(1, result.Users.Length);
        }

        [Test, RetryWithDelay]
        public async Task TestUpdateUserGroup()
        {
            var newName = $"new-test-name-async_{m_timestampSuffix}";

            var updateUserGroupParams = new UpdateUserGroupParams(m_groupId, newName);

            var updateResult = await AccountProvisioning.UpdateUserGroupAsync(updateUserGroupParams);
            Assert.AreEqual(m_groupId, updateResult.GroupId);

            var getGroupResult = await AccountProvisioning.UserGroupAsync(m_groupId);
            Assert.AreEqual(newName, getGroupResult.Name);
        }

        [Test, RetryWithDelay]
        public async Task TestAddAndRemoveUserFromGroup()
        {
            var addUserResult = await AccountProvisioning.AddUserToGroupAsync(m_groupId, m_userId1);
            Assert.AreEqual(1, addUserResult.Users.Length);

            var listUsersResult = await AccountProvisioning.UsersGroupUsersAsync(m_groupId);
            Assert.AreEqual(1, listUsersResult.Users.Length);

            var removeUserResult = await AccountProvisioning.RemoveUserFromGroupAsync(m_groupId, m_userId1);
            Assert.AreEqual(0, removeUserResult.Users.Length);
        }

        [Test, RetryWithDelay]
        public async Task TestUserGroupsInAccount()
        {
            var result = await AccountProvisioning.UserGroupsAsync();

            var foundGroup = result.UserGroups.FirstOrDefault(group => group.GroupId == m_groupId);

            Assert.NotNull(foundGroup);
        }

        [Test, RetryWithDelay]
        public async Task TestListAccessKeys()
        {
            var result = await AccountProvisioning.ListAccessKeysAsync(new ListAccessKeysParams(m_cloudId1));

            Assert.LessOrEqual(1, result.Total);
            Assert.LessOrEqual(1, result.AccessKeys.Length);
        }

        [Test, RetryWithDelay]
        public async Task TestGenerateAccessKey()
        {
            var result = await AccountProvisioning.GenerateAccessKeyAsync(new GenerateAccessKeyParams(m_cloudId1)
            {
                Name = "test_key",
                Enabled = false,
            });

            Assert.AreEqual("test_key", result.Name);
            Assert.AreEqual(false, result.Enabled);
        }

        [Test, RetryWithDelay]
        public async Task TestUpdateAccessKey()
        {
            var accessKeyResult = await AccountProvisioning.ListAccessKeysAsync(new ListAccessKeysParams(m_cloudId1));
            var accessKey = accessKeyResult.AccessKeys.FirstOrDefault();

            Assert.NotNull(accessKey);

            var result = await AccountProvisioning.UpdateAccessKeyAsync(new UpdateAccessKeyParams(m_cloudId1, accessKey.ApiKey)
            {
                Name = "updated_key",
                Enabled = true,
            });

            Assert.AreEqual("updated_key", result.Name);
            Assert.AreEqual(true, result.Enabled);
        }
    }
}
