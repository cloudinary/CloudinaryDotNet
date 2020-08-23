using System.Net;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet.Provisioning;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.Provisioning
{
    [TestFixture]
    public class ProvisioningIntegrationTestBase
    {
        protected long m_timestampSuffix;
        protected string m_userName;
        protected string m_userEmail;
        protected string m_cloudId;
        protected string m_userId;
        protected string m_groupId;
        protected readonly Role m_userRole = Role.Billing;

        protected AccountProvisioning AccountProvisioning;

        [OneTimeSetUp]
        public void Initialize()
        {
            AccountProvisioning = new AccountProvisioning();

            if (AccountProvisioning.ProvisioningApi.ProvisioningApiAccount.AccountId == null)
            {
                Assert.Ignore( "Provisioning API is not configured, skipping tests." );
            }

            // Create a sub account(sub cloud)
            m_timestampSuffix = Utils.UnixTimeNowSeconds();
            var subAccountName = $"jutaname{m_timestampSuffix}";
            var createSubAccountParams = new CreateSubAccountParams(subAccountName)
            {
                CloudName = subAccountName
            };
            var createSubAccountResult = AccountProvisioning.CreateSubAccountAsync(createSubAccountParams)
                .GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, createSubAccountResult.StatusCode);

            m_cloudId = createSubAccountResult.Id;

            // Create a user
            m_userName = $"SDK TEST {m_timestampSuffix}";
            m_userEmail = $"sdk-test+{m_timestampSuffix}@cloudinary.com";
            var createUserParams = new CreateUserParams(m_userName, m_userEmail, m_userRole);
            var createUserResult = AccountProvisioning.CreateUserAsync(createUserParams).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, createUserResult.StatusCode);
            
            m_userId = createUserResult.Id;

            // Create a user group
            var userGroupName = $"test-group-{m_timestampSuffix}";
            var createUserGroupParams = new CreateUserGroupParams(userGroupName);
            var createUserGroupResult = AccountProvisioning.CreateUserGroupAsync(createUserGroupParams).GetAwaiter().GetResult();
            
            Assert.AreEqual(HttpStatusCode.OK, createUserGroupResult.StatusCode);
            
            m_groupId = createUserGroupResult.GroupId;
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            AccountProvisioning.DeleteSubAccount(m_cloudId);
            AccountProvisioning.DeleteUser(m_userId);
            AccountProvisioning.DeleteUserGroup(m_groupId);
        }
    }
}
