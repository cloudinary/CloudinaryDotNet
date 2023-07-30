using System.Collections.Generic;
using System.Globalization;
using System.Net;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet.Provisioning;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTests.Provisioning
{
    [TestFixture]
    public class ProvisioningIntegrationTestBase
    {
        protected long m_timestampSuffix;
        protected string m_userName1;
        protected string m_userName2;
        protected string m_userEmail1;
        protected string m_userEmail2;
        protected string m_cloudId1;
        protected string m_cloudId2;
        protected string m_userId1;
        protected string m_userId2;
        protected string m_groupId;
        protected readonly Role m_userRole = Role.Billing;

        protected AccountProvisioning AccountProvisioning;

        protected bool Skipped;

        [OneTimeSetUp]
        public void Initialize()
        {
            AccountProvisioning = new AccountProvisioning();

            if (AccountProvisioning.ProvisioningApi.ProvisioningApiAccount.AccountId == null)
            {
                Assert.Ignore( "Provisioning API is not configured, skipping tests." );
                Skipped = true;
            }

            // Create a sub account(sub cloud)
            m_timestampSuffix = Utils.UnixTimeNowSeconds();

            var subAccount1Name = $"jutaname{m_timestampSuffix}";
            var subAccount2Name = $"jutaname2{m_timestampSuffix}";

            m_cloudId1 = CreateSubAccount(subAccount1Name);
            m_cloudId2 = CreateSubAccount(subAccount2Name);

            // Create users
            m_userName1 = $"SDK TEST {m_timestampSuffix}";
            m_userEmail1 = $"sdk-test+{m_timestampSuffix}@cloudinary.com";
            m_userId1 = CreateUser(m_userName1, m_userEmail1);

            m_userName2 = $"SDK TEST 2 {m_timestampSuffix}";
            m_userEmail2 = $"sdk-test2+{m_timestampSuffix}@cloudinary.com";
            m_userId2 = CreateUser(m_userName2, m_userEmail2);

            // Create a user group
            var userGroupName = $"test-group-{m_timestampSuffix}";
            var createUserGroupParams = new CreateUserGroupParams(userGroupName);
            var createUserGroupResult = AccountProvisioning.CreateUserGroupAsync(createUserGroupParams).GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, createUserGroupResult.StatusCode, createUserGroupResult.Error?.Message);

            m_groupId = createUserGroupResult.GroupId;
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            if (Skipped)
            {
                return;
            }
            AccountProvisioning.DeleteSubAccount(m_cloudId1);
            AccountProvisioning.DeleteSubAccount(m_cloudId2);
            AccountProvisioning.DeleteUser(m_userId1);
            AccountProvisioning.DeleteUser(m_userId2);
            AccountProvisioning.DeleteUserGroup(m_groupId);
        }

        private string CreateUser(string userName, string userEmail)
        {
            var createUserParams = new CreateUserParams(userName, userEmail, m_userRole)
            {
                SubAccountIds = new List<string> { m_cloudId1 }
            };
            var createUserResult = AccountProvisioning.CreateUserAsync(createUserParams).GetAwaiter().GetResult();
            Assert.AreEqual(HttpStatusCode.OK, createUserResult.StatusCode, createUserResult?.Error?.Message);
            Assert.AreEqual(1, createUserResult.SubAccountIds.Length);
            Assert.AreEqual(m_cloudId1, createUserResult.SubAccountIds[0]);

            return createUserResult.Id;
        }

        private string CreateSubAccount(string subAccountName)
        {
            var createSubAccountParams = new CreateSubAccountParams(subAccountName)
            {
                CloudName = subAccountName
            };
            var createSubAccountResult = AccountProvisioning.CreateSubAccountAsync(createSubAccountParams)
                .GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, createSubAccountResult.StatusCode, createSubAccountResult?.Error?.Message);

            return createSubAccountResult.Id;
        }

        protected static string GetCloudName()
        {
            return $"dotnetsdk{IntegrationTestBase.GetTaggedRandomValue()}"
                .Replace("_","").ToLower(CultureInfo.InvariantCulture);
        }
    }
}
