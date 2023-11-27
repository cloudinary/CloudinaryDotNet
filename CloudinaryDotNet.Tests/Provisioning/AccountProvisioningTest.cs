using CloudinaryDotNet.Provisioning;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests.Provisioning
{
    public class AccountProvisioningTest
    {
        private const string AccountId = "7e57acc00-0123-4567-890a-bcdef0123456";
        private const string ProvisioningApiKey = "01234567890123456789";
        private const string ProvisioningApiSecret = "S3CR3T!";
        private readonly string accountUrl = $"account://{ProvisioningApiKey}:{ProvisioningApiSecret}@{AccountId}";

        [Test]
        public void TestProvisioningApiAccountFromUrl()
        {
            var acc = new ProvisioningApiAccount(accountUrl);

            AssertAccount(acc);
        }

        [Test]
        public void TestProvisioningApiAccountFromParams()
        {
            var acc = new ProvisioningApiAccount(AccountId, ProvisioningApiKey, ProvisioningApiSecret);

            AssertAccount(acc);
        }

        [Test]
        public void TestProvisioningApiAccountSetters()
        {
            var acc = new ProvisioningApiAccount
            {
                AccountId = AccountId,
                ProvisioningApiKey = ProvisioningApiKey,
                ProvisioningApiSecret = ProvisioningApiSecret
            };

            AssertAccount(acc);
        }

        [Test]
        public void TestAccountProvisioningFromUrl()
        {
            var ap = new AccountProvisioning(accountUrl);

            AssertAccount(ap.ProvisioningApi.ProvisioningApiAccount);
        }

        [Test]
        public void TestAccountProvisioningFromParams()
        {
            var ap = new AccountProvisioning(AccountId, ProvisioningApiKey, ProvisioningApiSecret);

            AssertAccount(ap.ProvisioningApi.ProvisioningApiAccount);
        }

        [Test]
        public void TestAccountProvisioningFromProvisioningApiAccount()
        {

            var acc = new ProvisioningApiAccount(accountUrl);

            var ap = new AccountProvisioning(acc);

            AssertAccount(ap.ProvisioningApi.ProvisioningApiAccount);
        }

        private void AssertAccount(ProvisioningApiAccount account)
        {
            Assert.AreEqual(AccountId, account.AccountId);
            Assert.AreEqual(ProvisioningApiKey, account.ProvisioningApiKey);
            Assert.AreEqual(ProvisioningApiSecret, account.ProvisioningApiSecret);
        }
    }
}
