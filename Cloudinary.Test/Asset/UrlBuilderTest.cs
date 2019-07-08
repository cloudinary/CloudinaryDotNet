using System.Collections.Generic;
using System.Net;
using System.Web;
using NUnit.Framework;

namespace CloudinaryDotNet.Test.Asset
{
    [TestFixture]
    public partial class UrlBuilderTest
    {
        [Test]
        public void TestCallbackUrl()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("test", "http://localhost/test/do", ""), new HttpResponse(null));

            string s = m_api.BuildCallbackUrl();
            Assert.AreEqual("http://localhost/Content/cloudinary_cors.html", s);
        }

        [Test]
        public void TestUploadParamsWithCallback()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("test", "http://localhost:50/test/do", ""), new HttpResponse(null));

            string s = m_api.PrepareUploadParams(null);
            Assert.True(s.Contains("http://localhost:50/Content/cloudinary_cors.html"));

            var parameters = new SortedDictionary<string, object>()
            {
                {"callback", "/custom/custom_cors.html"}
            };

            s = m_api.PrepareUploadParams(parameters);
            Assert.True(s.Contains("http://localhost:50/custom/custom_cors.html"));

            parameters = new SortedDictionary<string, object>()
            {
                {"callback", "https://cloudinary.com/test/cloudinary_cors.html"}
            };

            s = m_api.PrepareUploadParams(parameters);
            Assert.True(s.Contains("https://cloudinary.com/test/cloudinary_cors.html"));
        }

        [Test]
        public void TestAgentPlatformHeaders()
        {
            HttpWebRequest request = HttpWebRequest.Create("http://dummy.com") as HttpWebRequest;
            m_api.UserPlatform = "Test/1.0";

            m_api.PrepareRequestBody(request, HttpMethod.GET, new SortedDictionary<string, object>(), new FileDescription(""));

            //Can't test the result, so we just verify the UserAgent parameter is sent to the server
            StringAssert.AreEqualIgnoringCase(string.Format("{0} {1}", m_api.UserPlatform, Api.USER_AGENT), request.UserAgent);

            StringAssert.IsMatch(@"Test\/1\.0 CloudinaryDotNet\/(\d+)\.(\d+)\.(\d+) \(.*\)", request.UserAgent);
        }

    }
}
