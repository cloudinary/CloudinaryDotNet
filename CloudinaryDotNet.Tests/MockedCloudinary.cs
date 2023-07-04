using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;

namespace CloudinaryDotNet.Tests
{
    public class MockedCloudinary : Cloudinary
    {
        public Mock<HttpMessageHandler> HandlerMock;
        public string HttpRequestContent;
        private const string cloudName = "test_cloud";

        public MockedCloudinary(string responseStr = "{}", HttpResponseHeaders httpResponseHeaders = null) : base("cloudinary://a:b@test_cloud")
        {
            HandlerMock = new Mock<HttpMessageHandler>();

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseStr)
            };

            if (httpResponseHeaders != null)
            {
                foreach (var httpResponseHeader in httpResponseHeaders)
                {
                    httpResponseMessage.Headers.Add(httpResponseHeader.Key, httpResponseHeader.Value);
                }
            }

            HandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>(
                    (httpRequestMessage, cancellationToken) =>
                    {
                        HttpRequestContent = httpRequestMessage.Content?
                            .ReadAsStringAsync()
                            .GetAwaiter()
                            .GetResult();
                    })
                .ReturnsAsync(httpResponseMessage);
            Api.Client = new HttpClient(HandlerMock.Object);
        }

        /// <summary>
        /// <para>Asserts that a given HTTP call was sent with expected parameters.</para>
        /// </summary>
        /// <param name="httpMethod">Expected HTTP method type.</param>
        /// <param name="localPath">Expected local part of the called Uri.</param>
        /// <param name="query">Query parameters</param>
        public void AssertHttpCall(System.Net.Http.HttpMethod httpMethod, string localPath, string query = "")
        {
            HandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == httpMethod &&
                    req.RequestUri.LocalPath == $"/v1_1/{cloudName}/{localPath}" &&
                    req.RequestUri.Query == query &&
                    req.Properties.Count == 0
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }


        public JToken RequestJson()
        {
            return JToken.Parse(HttpRequestContent);
        }
    }
}
