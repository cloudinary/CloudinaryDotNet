using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using CloudinaryDotNet.Actions;
using NUnit.Framework;
using SystemHttp = System.Net.Http;

namespace CloudinaryDotNet.Tests.AdminApi
{
    [TestFixture]
    public class UsageReportResponseTest : ApiShared
    {

        [OneTimeSetUp]
        public void Init()
        {

        }

        [Test]
        public void TestUsageReportResponse()
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
            AssertUsageResult(result);
        }

        private void AssertUsageResult(UsageResult result)
        {
            Assert.NotNull(result.LastUpdated, result.Error?.Message);
            Assert.IsNull(result.Error?.Message);

            Assert.IsNull(result.AdvOcr);
            Assert.IsNull(result.Aspose);
            Assert.IsNull(result.AwsRekModeration);
            Assert.NotNull(result.AzureVideoIndexer);
            Assert.NotNull(result.BackgroundRemoval);
            Assert.NotNull(result.Bandwidth);
            Assert.NotNull(result.Credits);
            Assert.NotNull(result.DerivedResources);
            Assert.NotNull(result.MediaLimits);
            Assert.NotNull(result.ObjectDetection);
            Assert.NotNull(result.Objects);
            Assert.NotNull(result.Plan);
            Assert.NotNull(result.Remaining);
            Assert.NotNull(result.Requests);
            Assert.NotNull(result.Resources);
            Assert.IsNull(result.SearchApi);
            Assert.NotNull(result.Storage);
            Assert.IsNull(result.StyleTransfer);
            Assert.NotNull(result.Transformations);
            Assert.IsNull(result.Url2png);
            Assert.IsNull(result.Webpurify);
        }

    }
}
