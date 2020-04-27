using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.AccountApi
{
    public class AccountProvisioningAsyncTest : IntegrationTestBase
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
            var createSubAccountResult = m_cloudinary.CreateSubAccountAsync(createSubAccountParams)
                .GetAwaiter().GetResult();
            m_cloudId = createSubAccountResult.Id;

            // Create a user
            m_userName = $"SDK TEST {m_timestampSuffix}";
            m_userEmail = $"sdk-test+{m_timestampSuffix}@cloudinary.com";
            var createUserParams = new CreateUserParams(m_userName, m_userEmail, m_userRole);
            var createUserResult = m_cloudinary.CreateUserAsync(createUserParams).GetAwaiter().GetResult();
            m_userId = createUserResult.Id;

            // Create a user group
            var userGroupName = $"test-group-{m_timestampSuffix}";
            var createUserGroupParams = new CreateUserGroupParams(userGroupName);
            var createUserGroupResult = m_cloudinary.CreateUserGroupAsync(createUserGroupParams).GetAwaiter().GetResult();
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
        public async Task TestUpdateSubAccount()
        {
            const string newName = "new-test-name-async";
            var updateSubAccountParams = new UpdateSubAccountParams(m_cloudId)
            {
                CloudName = newName
            };
            await m_cloudinary.UpdateSubAccountAsync(updateSubAccountParams);

            var result = await m_cloudinary.SubAccountAsync(m_cloudId);

            Assert.AreEqual(newName, result.CloudName);
        }

        [Test]
        public async Task TestGetAllSubAccounts()
        {
            var listSubAccountsParams = new ListSubAccountsParams
            {
                Enabled = true
            };

            var result = await m_cloudinary.SubAccountsAsync(listSubAccountsParams);

            Assert.NotNull(result.SubAccounts.FirstOrDefault(subAccount => subAccount.Id == m_cloudId));
        }

        [Test]
        public async Task TestGetSpecificSubAccount()
        {
            var result = await  m_cloudinary.SubAccountAsync(m_cloudId);

            Assert.AreEqual(m_cloudId, result.Id);
        }

        [Test]
        public async Task TestUpdateUser()
        {
            var newEmailAddress = $"updated-async+{m_timestampSuffix}@cloudinary.com";
            const string newName = "updated-async";
            var updateUserParams = new UpdateUserParams(m_userId)
            {
                Email = newEmailAddress,
                Name = newName
            };

            var updateUserResult = await  m_cloudinary.UpdateUserAsync(updateUserParams);
            Assert.AreEqual(newName, updateUserResult.Name);
            Assert.AreEqual(newEmailAddress, updateUserResult.Email);

            var getUserResult = await m_cloudinary.UserAsync(m_userId);
            Assert.AreEqual(m_userId, getUserResult.Id);
            Assert.AreEqual(newEmailAddress, getUserResult.Email);

            var listUsersResult = await m_cloudinary.UsersAsync(new ListUsersParams());
            var foundUser = listUsersResult.Users.FirstOrDefault(user => user.Id == m_userId);
            Assert.NotNull(foundUser);
            Assert.AreEqual(newEmailAddress, foundUser.Email);
        }

        [Test]
        public async Task TestGetUsersInAListOfUserIds()
        {
            var listUsersParams = new ListUsersParams
            {
                UserIds = new List<string> {m_userId}
            };

            var result = await m_cloudinary.UsersAsync(listUsersParams);

            Assert.AreEqual(1, result.Users.Length);
        }

        [Test]
        public async Task TestUpdateUserGroup()
        {
            var newName = $"new-test-name-async_{m_timestampSuffix}";

            var updateUserGroupParams = new UpdateUserGroupParams(m_groupId, newName);

            var updateResult = await m_cloudinary.UpdateUserGroupAsync(updateUserGroupParams);
            Assert.AreEqual(m_groupId, updateResult.GroupId);

            var getGroupResult = await m_cloudinary.UserGroupAsync(m_groupId);
            Assert.AreEqual(newName, getGroupResult.Name);
        }

        [Test]
        public async Task TestAddAndRemoveUserFromGroup()
        {
            var addUserResult = await m_cloudinary.AddUserToGroupAsync(m_groupId, m_userId);
            Assert.AreEqual(1, addUserResult.Users.Length);

            var listUsersResult = await m_cloudinary.UsersGroupUsersAsync(m_groupId);
            Assert.AreEqual(1, listUsersResult.Users.Length);

            var removeUserResult = await m_cloudinary.RemoveUserFromGroupAsync(m_groupId, m_userId);
            Assert.AreEqual(0, removeUserResult.Users.Length);
        }

        [Test]
        public async Task TestUserGroupsInAccount()
        {
            var result = await m_cloudinary.UserGroupsAsync();

            var foundGroup = result.UserGroups.FirstOrDefault(group => group.GroupId == m_groupId);

            Assert.NotNull(foundGroup);
        }
    }
}
