using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace CloudinaryDotNet.Tests
{
    public static class MockHelpers 
    {
        public static Mock<HttpMessageHandler> SetupMock(this IMockedApi mockedApi, string responseStr)
        {
            var mock = new Mock<HttpMessageHandler>();
            mock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>(
                    (httpRequestMessage, cancellationToken) =>
                    {
                        mockedApi.HttpRequestContent = httpRequestMessage.Content?
                            .ReadAsStringAsync()
                            .GetAwaiter()
                            .GetResult();
                    })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseStr)
                });
                return mock;
        }

        public static void AssertHttpCall(
               this IMockedApi mockedApi, 
               System.Net.Http.HttpMethod httpMethod, 
               string localPath
        )
        {
            mockedApi.HandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == httpMethod &&
                    req.RequestUri.LocalPath == $"/v1_1/{CloudName}/{localPath}" &&
                    req.Properties.Count == 0
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }
        public const string CloudName = "test_cloud";
    }

    public interface IMockedApi
    {
        Mock<HttpMessageHandler> HandlerMock { get; set; }
        string HttpRequestContent { set; }
    }

    public class MockedCloudinaryAdmin : CloudinaryAdmin, IMockedApi
    {
        public Mock<HttpMessageHandler> HandlerMock { get; set; }
        public string HttpRequestContent { get; set; }

        public MockedCloudinaryAdmin(string responseStr = "{}") : base($"cloudinary://a:b@{MockHelpers.CloudName}")
        {
            HandlerMock = this.SetupMock(responseStr); 
            Api.Client = new HttpClient(HandlerMock.Object);
        }
    }

    public class MockedCloudinaryUpload : CloudinaryUpload, IMockedApi
    {
        public Mock<HttpMessageHandler> HandlerMock { get; set; }
        public string HttpRequestContent { get; set; }

        public MockedCloudinaryUpload(string responseStr = "{}") : base($"cloudinary://a:b@{MockHelpers.CloudName}")
        {
            HandlerMock = this.SetupMock(responseStr); 
            Api.Client = new HttpClient(HandlerMock.Object);
        }
    }
}
