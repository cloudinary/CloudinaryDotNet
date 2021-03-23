using CloudinaryDotNet.Provisioning;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests.Provisioning
{
    public class ProvisioningApiAccountTest
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
            var acc = new ProvisioningApiAccount();

            acc.AccountId = AccountId;
            acc.ProvisioningApiKey = ProvisioningApiKey;
            acc.ProvisioningApiSecret = ProvisioningApiSecret;

            AssertAccount(acc);
        }

        private void AssertAccount(ProvisioningApiAccount account)
        {
            Assert.AreEqual(AccountId, account.AccountId);
            Assert.AreEqual(ProvisioningApiKey, account.ProvisioningApiKey);
            Assert.AreEqual(ProvisioningApiSecret, account.ProvisioningApiSecret);
        }
    }
}
