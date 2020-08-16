using System.Collections.Generic;
using System.Linq;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.Provisioning.AccountApi
{
    public class AccountProvisioningTest : ProvisioningIntegrationTestBase
    {
        [Test]
        public void TestUpdateSubAccount()
        {
            const string newName = "new-test-name";
            var updateSubAccountParams = new UpdateSubAccountParams(m_cloudId)
            {
                CloudName = newName

            };
            AccountProvisioning.UpdateSubAccount(updateSubAccountParams);

            var result = AccountProvisioning.SubAccount(m_cloudId);

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

            Assert.NotNull(result.SubAccounts.FirstOrDefault(subAccount => subAccount.Id == m_cloudId));
        }

        [Test]
        public void TestGetSpecificSubAccount()
        {
            var result = AccountProvisioning.SubAccount(m_cloudId);

            Assert.AreEqual(m_cloudId, result.Id);
        }

        [Test]
        public void TestUpdateUser()
        {
            var newEmailAddress = $"updated+{m_timestampSuffix}@cloudinary.com";
            const string newName = "updated";
            var updateUserParams = new UpdateUserParams(m_userId)
            {
                Email = newEmailAddress,
                Name = newName
            };

            var updateUserResult = AccountProvisioning.UpdateUser(updateUserParams);
            Assert.AreEqual(newName, updateUserResult.Name);
            Assert.AreEqual(newEmailAddress, updateUserResult.Email);

            var getUserResult = AccountProvisioning.User(m_userId);
            Assert.AreEqual(m_userId, getUserResult.Id);
            Assert.AreEqual(newEmailAddress, getUserResult.Email);

            var listUsersResult = AccountProvisioning.Users(new ListUsersParams());
            var foundUser = listUsersResult.Users.FirstOrDefault(user => user.Id == m_userId);
            Assert.NotNull(foundUser);
            Assert.AreEqual(newEmailAddress, foundUser.Email);
        }

        [Test]
        public void TestGetUsersInAListOfUserIds()
        {
            var listUsersParams = new ListUsersParams
            {
                UserIds = new List<string> {m_userId}
            };

            var result = AccountProvisioning.Users(listUsersParams);

            Assert.AreEqual(1, result.Users.Length);
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
            var addUserResult = AccountProvisioning.AddUserToGroup(m_groupId, m_userId);
            Assert.AreEqual(1, addUserResult.Users.Length);

            var listUsersResult = AccountProvisioning.UsersGroupUsers(m_groupId);
            Assert.AreEqual(1, listUsersResult.Users.Length);

            var removeUserResult = AccountProvisioning.RemoveUserFromGroup(m_groupId, m_userId);
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
