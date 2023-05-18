using NUnit.Framework;
using SystemHttp = System.Net.Http;

namespace CloudinaryDotNet.Tests.AdminApi
{
    [TestFixture]
    public class UsageReportResponseTest
    {
        [Test]
        public void TestBackgroundRemovalInUsageResponse()
        {
            var responseData = @"
                {
                  'plan': 'Basic',
                  'last_updated': '2019-11-10',
                  'transformations': { 'usage': 1218018, 'credits_usage': 1.22}, 
                  'objects': { 'usage': 1217216}, 
                  'bandwidth': { 'usage': 268903064875, 'credits_usage': 0.24}, 
                  'storage': { 'usage': 10298444599, 'credits_usage': 0.01}, 
                  'credits': { 'usage': 1.47}, 
                  'requests': 877212,
                  'resources': 1239,
                  'derived_resources': 10091,
                  'background_removal': { 'usage': 15, 'limit': 50},
                  'azure_video_indexer': { 'usage': 2340, 'limit': 5000}, 
                  'object_detection': { 'usage': 340, 'limit': 500},
                  'media_limits': {
                    'image_max_size_bytes': 157286400, 
                    'video_max_size_bytes': 3145728000, 
                    'raw_max_size_bytes': 2097152000, 
                    'image_max_px': 100000000, 
                    'asset_max_total_px': 300000000
                  }
                }";

            var localCloudinaryMock = new MockedCloudinary(responseData);

            var result = localCloudinaryMock.GetUsage();

            localCloudinaryMock.AssertHttpCall(SystemHttp.HttpMethod.Get, "usage");

            Assert.NotNull(result);

            Assert.AreEqual(result.BackgroundRemoval.CreditsUsage, 0);
            Assert.AreEqual(result.BackgroundRemoval.Limit, 50);
            Assert.AreEqual(result.BackgroundRemoval.Used, 15);
            Assert.AreEqual(result.BackgroundRemoval.UsedPercent, 0);
        }
    }
}
