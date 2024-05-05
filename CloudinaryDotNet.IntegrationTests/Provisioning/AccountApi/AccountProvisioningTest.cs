using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTests.Provisioning.AccountApi
{
    public class AccountProvisioningTest : ProvisioningIntegrationTestBase
    {
        [Test, RetryWithDelay]
        public void TestUpdateSubAccount()
         {
            var newName = GetCloudName();
            var updateSubAccountParams = new UpdateSubAccountParams(m_cloudId1)
            {
                CloudName = newName
            };

            var updateResult = AccountProvisioning.UpdateSubAccount(updateSubAccountParams);

            Assert.AreEqual(HttpStatusCode.OK, updateResult.StatusCode, updateResult.Error?.Message);

            var result = AccountProvisioning.SubAccount(m_cloudId1);

            Assert.AreEqual(newName, result.CloudName);
        }

        [Test, RetryWithDelay]
        public void TestGetAllSubAccounts()
        {
            var listSubAccountsParams = new ListSubAccountsParams
            {
                Enabled = true
            };

            var result = AccountProvisioning.SubAccounts(listSubAccountsParams);

            Assert.NotNull(result.SubAccounts.FirstOrDefault(subAccount => subAccount.Id == m_cloudId1));
        }

        [Test, RetryWithDelay]
        public void TestGetSpecificSubAccount()
        {
            var result = AccountProvisioning.SubAccount(m_cloudId1);

            Assert.AreEqual(m_cloudId1, result.Id);
        }

        [Test, RetryWithDelay]
        public void TestUpdateUser()
        {
            var newEmailAddress = $"updated+{m_timestampSuffix}@cloudinary.com";
            const string newName = "updated";
            var updateUserParams = new UpdateUserParams(m_userId1)
            {
                Email = newEmailAddress,
                Name = newName

            };

            var updateUserResult = AccountProvisioning.UpdateUser(updateUserParams);
            Assert.AreEqual(newName, updateUserResult.Name);
            Assert.AreEqual(newEmailAddress, updateUserResult.Email);

            var getUserResult = AccountProvisioning.User(m_userId1);
            Assert.AreEqual(m_userId1, getUserResult.Id);
            Assert.AreEqual(newEmailAddress, getUserResult.Email);

            var listUsersResult = AccountProvisioning.Users(new ListUsersParams());
            var foundUser = listUsersResult.Users.FirstOrDefault(user => user.Id == m_userId1);
            Assert.NotNull(foundUser);
            Assert.AreEqual(newEmailAddress, foundUser.Email);
        }

        [Test, RetryWithDelay]
        public void TestGetUsersInAListOfUserIds()
        {
            var listUsersParams = new ListUsersParams
            {
                UserIds = new List<string> {m_userId1}
            };

            var result = AccountProvisioning.Users(listUsersParams);

            Assert.AreEqual(1, result.Users.Length);
        }

        [Test, RetryWithDelay]
        public void TestGetPendingUsers()
        {
            var listUsersParams = new ListUsersParams
            {
                Pending = true,
                UserIds = new List<string> { m_userId1 }
            };

            var result = AccountProvisioning.Users(listUsersParams);

            Assert.AreEqual(1, result.Users.Length);
        }

        [Test, RetryWithDelay]
        public void TestGetNonPendingUsers()
        {
            var listUsersParams = new ListUsersParams
            {
                Pending = false,
                UserIds = new List<string> { m_userId1 }
            };

            var result = AccountProvisioning.Users(listUsersParams);

            Assert.AreEqual(0, result.Users.Length);
        }

        [Test, RetryWithDelay]
        public void TestGetPendingAndNonPendingUsers()
        {
            var listUsersParams = new ListUsersParams
            {
                Pending = null,
                UserIds = new List<string> { m_userId1 }
            };

            var result = AccountProvisioning.Users(listUsersParams);

            Assert.AreEqual(1, result.Users.Length);
        }

        [Test, RetryWithDelay]
        public void TestGetUsersByPrefix()
        {
            var listUsersParams1 = new ListUsersParams
            {
                Pending = true,
                Prefix = m_userName2.Substring(0, m_userName2.Length - 1)
            };
            var result1 = AccountProvisioning.Users(listUsersParams1);
            Assert.AreEqual(1, result1.Users.Length);

            var listUsersParams2 = new ListUsersParams
            {
                Pending = true,
                Prefix = $"{m_userName2}zzz"
            };
            var result2 = AccountProvisioning.Users(listUsersParams2);
            Assert.AreEqual(0, result2.Users.Length);
        }

        [Test, RetryWithDelay]
        public void TestGetUsersBySubAccountId()
        {
            var listUsersParams = new ListUsersParams
            {
                Pending = true,
                Prefix = m_userName2,
                SubAccountId = m_cloudId1
            };

            var result = AccountProvisioning.Users(listUsersParams);

            Assert.AreEqual(1, result.Users.Length);
        }

        [Test, RetryWithDelay]
        public void TestGetUsersByNonexistentSubAccountIdError()
        {
            var randomId = new Random().Next(100000).ToString();
            var listUsersParams = new ListUsersParams
            {
                Pending = true,
                SubAccountId = randomId
            };

            var result = AccountProvisioning.Users(listUsersParams);

            Assert.True(result.Error.Message.StartsWith("Cannot find sub account"));
        }

        [Test, RetryWithDelay]
        public void TestUpdateUserGroup()
        {
            var newName = $"new-test-name_{m_timestampSuffix}";

            var updateUserGroupParams = new UpdateUserGroupParams(m_groupId, newName);

            var updateResult = AccountProvisioning.UpdateUserGroup(updateUserGroupParams);
            Assert.AreEqual(m_groupId, updateResult.GroupId);

            var getGroupResult = AccountProvisioning.UserGroup(m_groupId);
            Assert.AreEqual(newName, getGroupResult.Name);
        }

        [Test, RetryWithDelay]
        public void TestAddAndRemoveUserFromGroup()
        {
            var addUserResult = AccountProvisioning.AddUserToGroup(m_groupId, m_userId1);
            Assert.AreEqual(1, addUserResult.Users.Length);

            var listUsersResult = AccountProvisioning.UsersGroupUsers(m_groupId);
            Assert.AreEqual(1, listUsersResult.Users.Length);

            var removeUserResult = AccountProvisioning.RemoveUserFromGroup(m_groupId, m_userId1);
            Assert.AreEqual(0, removeUserResult.Users.Length);
        }

        [Test, RetryWithDelay]
        public void TestUserGroupsInAccount()
        {
            var result = AccountProvisioning.UserGroups();

            var foundGroup = result.UserGroups.FirstOrDefault(group => group.GroupId == m_groupId);

            Assert.NotNull(foundGroup);
        }

        [Test, RetryWithDelay]
        public void TestListAccessKeys()
        {
            var result = AccountProvisioning.ListAccessKeys(new ListAccessKeysParams(m_cloudId1));

            Assert.LessOrEqual(1, result.Total);
            Assert.LessOrEqual(1, result.AccessKeys.Length);
        }

        [Test, RetryWithDelay]
        public void TestGenerateAccessKey()
        {
            var result = AccountProvisioning.GenerateAccessKey(new GenerateAccessKeyParams(m_cloudId1)
            {
                Name = "test_key",
                Enabled = false,
            });

            Assert.AreEqual("test_key", result.Name);
            Assert.AreEqual(false, result.Enabled);
        }

        [Test, RetryWithDelay]
        public void TestUpdateAccessKey()
        {
            var accessKey = AccountProvisioning.ListAccessKeys(new ListAccessKeysParams(m_cloudId1)).AccessKeys.FirstOrDefault();

            Assert.NotNull(accessKey);
            Assert.IsNotEmpty(accessKey.ApiKey);

            var result = AccountProvisioning.UpdateAccessKey(new UpdateAccessKeyParams(m_cloudId1, accessKey.ApiKey)
            {
                Name = "updated_key",
                Enabled = true,
            });

            Assert.AreEqual("updated_key", result.Name);
            Assert.AreEqual(true, result.Enabled);
        }

        [Test, RetryWithDelay]
        public void TestDeleteAccessKey()
        {
            var testKeyToDelete = GetTaggedRandomValue() + "_test_key_to_delete_by_name";
            var accessKey = AccountProvisioning.GenerateAccessKey(new GenerateAccessKeyParams(m_cloudId1)
            {
                Name = testKeyToDelete
            });

            Assert.NotNull(accessKey);
            Assert.IsNotEmpty(accessKey.ApiKey);
            Assert.AreEqual(testKeyToDelete, accessKey.Name);

            var result = AccountProvisioning.DeleteAccessKey(new DelAccessKeyParams(m_cloudId1)
            {
                Name = accessKey.Name
            });

            Assert.AreEqual("ok", result.Message);
        }
    }
}
