using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests
{
    public class ApiAuthorizationTest
    {
        private const string m_oauthToken = "NTQ0NjJkZmQ5OTM2NDE1ZTZjNGZmZj17";
        private const string m_cloudName = "test123";
        private const string m_apiKey = "key";
        private const string m_apiSecret = "secret";
        private MockedCloudinary m_mockedCloudinary;

        [Test]
        public async Task TestOAuthTokenAdminApi()
        {
            InitCloudinaryApi();

            await m_mockedCloudinary.PingAsync();

            AssertHasBearerAuthorization(m_mockedCloudinary, m_oauthToken);
        }

        [Test]
        public async Task TestKeyAndSecretAdminApi()
        {
            InitCloudinaryApi(m_apiKey, m_apiSecret);

            await m_mockedCloudinary.PingAsync();

            AssertHasBasicAuthorization(m_mockedCloudinary, "a2V5OnNlY3JldA==");
        }

        [Test]
        public async Task TestOAuthTokenUploadApi()
        {
            InitCloudinaryApi();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.GetTempFileName())
            };

            await m_mockedCloudinary.UploadAsync(uploadParams);

            AssertHasBearerAuthorization(m_mockedCloudinary, m_oauthToken);
            Assert.IsFalse(m_mockedCloudinary.HttpRequestContent.Contains("signature"));
        }

        [Test]
        public async Task TestKeyAndSecretUploadApi()
        {
            InitCloudinaryApi(m_apiKey, m_apiSecret);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.GetTempFileName())
            };
            await m_mockedCloudinary.UploadAsync(uploadParams);

            Assert.IsTrue(m_mockedCloudinary.HttpRequestContent.Contains("signature"));
            Assert.IsTrue(m_mockedCloudinary.HttpRequestContent.Contains("api_key"));
        }

        [Test]
        public async Task TestMissingCredentialsUploadApi()
        {
            InitCloudinaryApi(null, null);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.GetTempFileName()),
                Unsigned = true,
                UploadPreset = "api_test_upload_preset"
            };

            await m_mockedCloudinary.UploadAsync(uploadParams);

            Assert.IsTrue(m_mockedCloudinary.HttpRequestContent.Contains("upload_preset"));
        }

        private void InitCloudinaryApi()
        {
            m_mockedCloudinary = new MockedCloudinary(account: new Account(m_cloudName, m_oauthToken));
        }

        private void InitCloudinaryApi(string apiKey, string apiSecret)
        {
            m_mockedCloudinary = new MockedCloudinary(account: new Account(m_cloudName, apiKey, apiSecret));
        }

        private void AssertHasAuthorization(MockedCloudinary cloudinary, string scheme, string value) =>
            Assert.AreEqual(cloudinary.HttpRequestHeaders.Authorization, new AuthenticationHeaderValue(scheme, value));

        private void AssertHasBearerAuthorization(MockedCloudinary cloudinary, string value) =>
            AssertHasAuthorization(cloudinary, "Bearer", value);

        private void AssertHasBasicAuthorization(MockedCloudinary cloudinary, string value) =>
            AssertHasAuthorization(cloudinary, "Basic", value);
    }
}
