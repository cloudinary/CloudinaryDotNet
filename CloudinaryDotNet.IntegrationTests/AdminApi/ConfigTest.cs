using System;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTests.AdminApi
{
    public class ConfigTest : IntegrationTestBase
    {
        [Test, RetryWithDelay]
        public void TestGetConfig()
        {
            var result = m_cloudinary.GetConfig();

            AssertConfigResultWithoutSettings(result);
        }

        [Test, RetryWithDelay]
        public async Task TestGetConfigAsync()
        {
            var result = await m_cloudinary.GetConfigAsync();

            AssertConfigResultWithoutSettings(result);
        }

        [Test, RetryWithDelay]
        public void TestGetConfigWithSettings()
        {
            var configParams = new ConfigParams
            {
                Settings = true
            };

            var result = m_cloudinary.GetConfig(configParams);

            AssertConfigResultWithSettings(result);
        }

        [Test, RetryWithDelay]
        public async Task TestGetConfigWithSettingsAsync()
        {
            var configParams = new ConfigParams
            {
                Settings = true
            };

            var result = await m_cloudinary.GetConfigAsync(configParams);

            AssertConfigResultWithSettings(result);
        }

        private void AssertConfigResultWithSettings(ConfigResult result)
        {
            AssertCommonConfigAssertions(result);
            Assert.NotNull(result.Settings, "Settings should not be null");
            Assert.IsFalse(string.IsNullOrEmpty(result.Settings.FolderMode), "FolderMode should not be empty");
        }

        private void AssertConfigResultWithoutSettings(ConfigResult result)
        {
            AssertCommonConfigAssertions(result);
            Assert.IsNull(result.Settings, "Settings should be null when not included");
        }

        private void AssertCommonConfigAssertions(ConfigResult result)
        {
            Assert.NotNull(result, "Config result should not be null");
            Assert.AreEqual(m_cloudName, result.CloudName, "Cloud name does not match the expected value");
            Assert.NotNull(result.CreatedAt, "Created date should not be null");
            Assert.IsNull(result.Error?.Message, "Error message should be null");
        }
    }
}
