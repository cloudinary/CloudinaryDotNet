using CloudinaryDotNet.Actions;
using NUnit.Framework;
using SystemHttp = System.Net.Http;

namespace CloudinaryDotNet.Tests.AdminApi
{
    [TestFixture]
    public class AnalysisTest
    {
        private const string TestUrl = "https://res.cloudinary.com/demo/image/upload/dog";
        private const string TestRequestId = "e8870bdfda21bf44e6b03c165b9d9fe7";

        private const string TestCaption =
            "A small brown and white dog is running through a lush green field, chasing a red ball with its mouth.";

        private const string ResponseData = @"
{
  ""data"": {
    ""entity"": """ + TestUrl + @""",
    ""analysis"": {
      ""data"": {
        ""caption"": """ + TestCaption + @"""
      },
      ""model_version"": 3
    }
  },
  ""request_id"": """ + TestRequestId + @"""
}";

        [Test]
        public void TestAnalyze()
        {
            var localCloudinaryMock = new MockedCloudinary(ResponseData);

            var result = localCloudinaryMock.Analyze(new AnalyzeParams()
            {
                AnalysisType = "captioning",
                Uri = TestUrl,
                Parameters = new AnalyzeUriRequestParameters()
                {
                    Custom = new CustomParameters()
                    {
                        ModelName = "my_model",
                        ModelVersion = 1
                    }
                }
            });

            localCloudinaryMock.AssertHttpCall(
                SystemHttp.HttpMethod.Post,
                "analysis/analyze/uri",
                "",
                "v2"
            );

            var requestContent = localCloudinaryMock.RequestJson();

            Assert.AreEqual("my_model", requestContent["parameters"]?["custom"]?["model_name"]?.ToString());
            Assert.AreEqual("1", requestContent["parameters"]?["custom"]?["model_version"]?.ToString());

            Assert.NotNull(result);

            Assert.AreEqual(TestRequestId, result.RequestId);
            Assert.AreEqual(TestUrl, result.Data.Entity);
            Assert.AreEqual(TestCaption, result.Data.Analysis["data"]?["caption"]?.ToString());
        }
    }
}
