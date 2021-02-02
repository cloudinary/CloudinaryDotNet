using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.Provisioning.AccountApi
{
    public class AccountProvisioningTest : ProvisioningIntegrationTestBase
    {
        [Test]
        public void TestUpdateSubAccount()
        {
            var newName = $"some-name-{Guid.NewGuid().GetHashCode()}";
            var updateSubAccountParams = new UpdateSubAccountParams(m_cloudId1)
            {
                CloudName = newName
            };

            var updateResult = AccountProvisioning.UpdateSubAccount(updateSubAccountParams);
            
            Assert.AreEqual(HttpStatusCode.OK, updateResult.StatusCode);
            
            var result = AccountProvisioning.SubAccount(m_cloudId1);

            Assert.AreEqual(newName, result.CloudName);
        }

        [Test]
        public void TestGetAllSubAccounts()
        {
            var listSubAccountsParams = new ListSubAccountsParams
            {
                Enabled = true
            };

            var result = AccountProvisioning.SubAccounts(listSubAccountsParams);

            Assert.NotNull(result.SubAccounts.FirstOrDefault(subAccount => subAccount.Id == m_cloudId1));
        }

        [Test]
        public void TestGetSpecificSubAccount()
        {
            var result = AccountProvisioning.SubAccount(m_cloudId1);

            Assert.AreEqual(m_cloudId1, result.Id);
        }

        [Test]
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

        [Test]
        public void TestGetUsersInAListOfUserIds()
        {
            var listUsersParams = new ListUsersParams
            {
                UserIds = new List<string> {m_userId1}
            };

            var result = AccountProvisioning.Users(listUsersParams);

            Assert.AreEqual(1, result.Users.Length);
        }

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
        public void TestUpdateUserGroup()
        {
            var newName = $"new-test-name_{m_timestampSuffix}";

            var updateUserGroupParams = new UpdateUserGroupParams(m_groupId, newName);

            var updateResult = AccountProvisioning.UpdateUserGroup(updateUserGroupParams);
            Assert.AreEqual(m_groupId, updateResult.GroupId);

            var getGroupResult = AccountProvisioning.UserGroup(m_groupId);
            Assert.AreEqual(newName, getGroupResult.Name);
        }

        [Test]
        public void TestAddAndRemoveUserFromGroup()
        {
            var addUserResult = AccountProvisioning.AddUserToGroup(m_groupId, m_userId1);
            Assert.AreEqual(1, addUserResult.Users.Length);

            var listUsersResult = AccountProvisioning.UsersGroupUsers(m_groupId);
            Assert.AreEqual(1, listUsersResult.Users.Length);

            var removeUserResult = AccountProvisioning.RemoveUserFromGroup(m_groupId, m_userId1);
            Assert.AreEqual(0, removeUserResult.Users.Length);
        }

        [Test]
        public void TestUserGroupsInAccount()
        {
            var result = AccountProvisioning.UserGroups();

            var foundGroup = result.UserGroups.FirstOrDefault(group => group.GroupId == m_groupId);

            Assert.NotNull(foundGroup);
        }
    }
}
