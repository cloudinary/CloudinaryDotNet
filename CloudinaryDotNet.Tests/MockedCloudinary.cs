using System;
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
        private const string CloudName = "test123";
        public HttpRequestHeaders HttpRequestHeaders;

        public MockedCloudinary(string responseStr = "{}", HttpResponseHeaders httpResponseHeaders = null, Account account = null)
            : base(account ?? new Account(CloudName, "key", "secret"))
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
                        HttpRequestHeaders = httpRequestMessage.Headers;
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
                    req.RequestUri.LocalPath == $"/v1_1/{CloudName}/{localPath}" &&
                    req.RequestUri.Query == query
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
