using System;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTests.AdminApi
{
    public class OAuthTest: IntegrationTestBase
    {
        private const string FAKE_OAUTH_TOKEN = "MTQ0NjJkZmQ5OTM2NDE1ZTZjNGZmZjI4";
        private static string m_uniqueImagePublicId;

        public override void Initialize()
        {
            base.Initialize();

            m_uniqueImagePublicId = $"asset_image_{m_uniqueTestId}";
        }

        [Test]
        public void TestOAuthToken()
        {
            var result = m_cloudinary.GetResource(m_uniqueImagePublicId);
            Assert.IsNotNull(result.Error);
            Assert.IsTrue(result.Error.Message.Contains("Invalid token"));
        }

        protected override Account GetAccountInstance()
        {
            Account account = new Account(m_cloudName, FAKE_OAUTH_TOKEN);

            Assert.IsNotEmpty(account.Cloud, $"Cloud name must be specified in {CONFIG_PLACE}");
            Assert.IsNotEmpty(account.OAuthToken);

            return account;
        }
    }
}
