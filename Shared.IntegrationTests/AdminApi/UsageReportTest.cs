using System.Collections.Generic;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.AdminApi
{
    public class UsageReportTest: IntegrationTestBase
    {
        [Test]
        public void TestUsage()
        {
            UploadTestImageResource(); // making sure at least one resource exists

            var result = m_cloudinary.GetUsage();

            AssertUsageResult(result);
        }

        [Test]
        public async Task TestUsageAsync()
        {
            await UploadTestImageResourceAsync(); // making sure at least one resource exists

            var result = await m_cloudinary.GetUsageAsync();

            AssertUsageResult(result);
        }

        private void AssertUsageResult(UsageResult result)
        {
            Assert.True(result.Resources > 0);
            Assert.NotNull(result.Objects);
            Assert.True(result.Objects.Used > 0);
            Assert.True(result.Objects.Limit > 0);
            Assert.NotNull(result.Bandwidth);
            Assert.True(result.Bandwidth.Limit > 0);
            Assert.True(result.Bandwidth.Used > 0);
            Assert.NotNull(result.Transformations);
            Assert.NotNull(result.AwsRekModeration);
            Assert.NotNull(result.AdvOcr);
            Assert.NotNull(result.Webpurify);
            Assert.NotNull(result.SearchApi);
            Assert.NotNull(result.MediaLimits);
            Assert.True(result.MediaLimits.Count > 0);
            Assert.NotNull(result.LastUpdated);
        }
    }
}
