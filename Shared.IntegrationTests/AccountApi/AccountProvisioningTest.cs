using System.Collections.Generic;
using System.Linq;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.AccountApi
{
    public class AccountProvisioningTest : IntegrationTestBase
    {
        private long m_timestampSuffix;
        private string m_userName;
        private string m_userEmail;
        private string m_cloudId;
        private string m_userId;
        private string m_groupId;
        private readonly Role m_userRole = Role.Billing;

        [OneTimeSetUp]
        public override void Initialize()
        {
            base.Initialize();

            // Create a sub account(sub cloud)
            m_timestampSuffix = Utils.UnixTimeNowSeconds();
            var subAccountName = $"jutaname{m_timestampSuffix}";
            var createSubAccountParams = new CreateSubAccountParams(subAccountName)
            {
                CloudName = subAccountName
            };
            var createSubAccountResult = m_cloudinary.CreateSubAccount(createSubAccountParams);
            m_cloudId = createSubAccountResult.Id;

            // Create a user
            m_userName = $"SDK TEST {m_timestampSuffix}";
            m_userEmail = $"sdk-test+{m_timestampSuffix}@cloudinary.com";
            var createUserParams = new CreateUserParams(m_userName, m_userEmail, m_userRole);
            var createUserResult = m_cloudinary.CreateUser(createUserParams);
            m_userId = createUserResult.Id;

            // Create a user group
            var userGroupName = $"test-group-{m_timestampSuffix}";
            var createUserGroupParams = new CreateUserGroupParams(userGroupName);
            var createUserGroupResult = m_cloudinary.CreateUserGroup(createUserGroupParams);
            m_groupId = createUserGroupResult.GroupId;
        }

        [OneTimeTearDown]
        public override void Cleanup()
        {
            base.Cleanup();

            m_cloudinary.DeleteSubAccount(m_cloudId);
            m_cloudinary.DeleteUser(m_userId);
            m_cloudinary.DeleteUserGroup(m_groupId);
        }

        [Test]
        public void TestUpdateSubAccount()
        {
            const string newName = "new-test-name";
            var updateSubAccountParams = new UpdateSubAccountParams(m_cloudId)
            {
                CloudName = newName

            };
            m_cloudinary.UpdateSubAccount(updateSubAccountParams);

            var result = m_cloudinary.SubAccount(m_cloudId);

            Assert.AreEqual(newName, result.CloudName);
        }

        [Test]
        public void TestGetAllSubAccounts()
        {
            var listSubAccountsParams = new ListSubAccountsParams
            {
                Enabled = true
            };

            var result = m_cloudinary.SubAccounts(listSubAccountsParams);

            Assert.NotNull(result.SubAccounts.FirstOrDefault(subAccount => subAccount.Id == m_cloudId));
        }

        [Test]
        public void TestGetSpecificSubAccount()
        {
            var result = m_cloudinary.SubAccount(m_cloudId);

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

            var updateUserResult = m_cloudinary.UpdateUser(updateUserParams);
            Assert.AreEqual(newName, updateUserResult.Name);
            Assert.AreEqual(newEmailAddress, updateUserResult.Email);

            var getUserResult = m_cloudinary.User(m_userId);
            Assert.AreEqual(m_userId, getUserResult.Id);
            Assert.AreEqual(newEmailAddress, getUserResult.Email);

            var listUsersResult = m_cloudinary.Users(new ListUsersParams());
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

            var result = m_cloudinary.Users(listUsersParams);

            Assert.AreEqual(1, result.Users.Length);
        }

        [Test]
        public void TestUpdateUserGroup()
        {
            var newName = $"new-test-name_{m_timestampSuffix}";

            var updateUserGroupParams = new UpdateUserGroupParams(m_groupId, newName);

            var updateResult = m_cloudinary.UpdateUserGroup(updateUserGroupParams);
            Assert.AreEqual(m_groupId, updateResult.GroupId);

            var getGroupResult = m_cloudinary.UserGroup(m_groupId);
            Assert.AreEqual(newName, getGroupResult.Name);
        }

        [Test]
        public void TestAddAndRemoveUserFromGroup()
        {
            var addUserResult = m_cloudinary.AddUserToGroup(m_groupId, m_userId);
            Assert.AreEqual(1, addUserResult.Users.Length);

            var listUsersResult = m_cloudinary.UsersGroupUsers(m_groupId);
            Assert.AreEqual(1, listUsersResult.Users.Length);

            var removeUserResult = m_cloudinary.RemoveUserFromGroup(m_groupId, m_userId);
            Assert.AreEqual(0, removeUserResult.Users.Length);
        }

        [Test]
        public void TestUserGroupsInAccount()
        {
            var result = m_cloudinary.UserGroups();

            var foundGroup = result.UserGroups.FirstOrDefault(group => group.GroupId == m_groupId);

            Assert.NotNull(foundGroup);
        }
    }
}
