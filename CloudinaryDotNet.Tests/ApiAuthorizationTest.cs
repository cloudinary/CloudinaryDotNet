using System;
using System.Collections.Generic;
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
                File = GetFileDescription()
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
                File = GetFileDescription()
            };
            await m_mockedCloudinary.UploadAsync(uploadParams);

            AssertUploadSignature();
        }

        [TestCaseSource(typeof(UploadApiProvider), nameof(UploadApiProvider.UploadApis))]
        public async Task TestUploadAuthorization(Func<MockedCloudinary, Task> func)
        {
            InitCloudinaryApi(m_apiKey, m_apiSecret);

            await func(m_mockedCloudinary);

            AssertUploadSignature();
        }

        private static FileDescription GetFileDescription()
            => new FileDescription("foo", new MemoryStream(new byte[5]));

        private void AssertUploadSignature()
        {
            var httpRequestContent = m_mockedCloudinary.HttpRequestContent;
            Assert.IsTrue(httpRequestContent.Contains("signature"));
            Assert.IsTrue(httpRequestContent.Contains("api_key"));
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

        private static class UploadApiProvider
        {
            public static IEnumerable<object> UploadApis()
            {
                yield return new Func<MockedCloudinary, Task>[] 
                    { m => m.UploadAsync(new VideoUploadParams { File = GetFileDescription() }) };

                yield return new Func<MockedCloudinary, Task>[] 
                    { m => m.UploadAsync(new ImageUploadParams { File = GetFileDescription() }) };

                yield return new Func<MockedCloudinary, Task>[] 
                    { m => m.UploadAsync(new RawUploadParams { File = GetFileDescription() }) };

                yield return new Func<MockedCloudinary, Task>[] 
                    { m => m.UploadLargeAsync(new RawUploadParams { File = GetFileDescription() }) };

                yield return new Func<MockedCloudinary, Task>[] 
                    { m => m.UploadLargeRawAsync(new RawUploadParams { File = GetFileDescription() }) };

                yield return new Func<MockedCloudinary, Task>[] 
                    { m => m.TagAsync(new TagParams()) };

                yield return new Func<MockedCloudinary, Task>[] 
                    { m => m.ContextAsync(new ContextParams()) };

                yield return new Func<MockedCloudinary, Task>[] 
                    { m => m.ExplicitAsync(new ExplicitParams("id")) };

                yield return new Func<MockedCloudinary, Task>[] 
                    { m => m.ExplodeAsync(new ExplodeParams("id", new Transformation())) };

                yield return new Func<MockedCloudinary, Task>[] 
                    { m => m.CreateZipAsync(new ArchiveParams().PublicIds(new List<string> { "id" })) };

                yield return new Func<MockedCloudinary, Task>[] 
                    { m => m.CreateArchiveAsync(new ArchiveParams().PublicIds(new List<string> { "id" })) };

                yield return new Func<MockedCloudinary, Task>[] 
                    { m => m.MakeSpriteAsync(new SpriteParams("tag")) };

                yield return new Func<MockedCloudinary, Task>[] 
                    { m => m.MultiAsync(new MultiParams("tag")) };

                yield return new Func<MockedCloudinary, Task>[] 
                    { m => m.TextAsync(new TextParams("text")) };

                yield return new Func<MockedCloudinary, Task>[] 
                    { m => m.CreateSlideshowAsync(
                            new CreateSlideshowParams { ManifestTransformation = new Transformation() }) };
            }
        }
    }
}
