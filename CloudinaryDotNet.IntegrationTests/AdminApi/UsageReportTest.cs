using System;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTests.AdminApi
{
    public class UsageReportTest: IntegrationTestBase
    {
        [Test, RetryWithDelay]
        public void TestUsage()
        {
            UploadTestImageResource(); // making sure at least one resource exists

            var result = m_cloudinary.GetUsage();

            AssertUsageResult(result);
        }

        [Test, RetryWithDelay]
        public async Task TestUsageAsync()
        {
            await UploadTestImageResourceAsync(); // making sure at least one resource exists

            var result = await m_cloudinary.GetUsageAsync();

            AssertUsageResult(result);
        }

        [Test, RetryWithDelay]
        public void TestUsageByDate()
        {
            var result = m_cloudinary.GetUsage(GetYesterdayDate());

            AssertUsageResult(result);
        }

        [Test, RetryWithDelay]
        public async Task TestUsageByDateAsync()
        {
            var result = await m_cloudinary.GetUsageAsync(GetYesterdayDate());

            AssertUsageResult(result);
        }

        private static void AssertUsageResult(UsageResult result)
        {
            Assert.NotNull(result.LastUpdated, result.Error?.Message);
            Assert.IsNull(result.Error?.Message);
        }

        private static DateTime GetYesterdayDate()
        {
            return DateTime.Today.AddDays(-1);
        }
    }
}
