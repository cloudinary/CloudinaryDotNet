using System.Collections.Generic;
using System.Web;
using NUnit.Framework;

namespace CloudinaryDotNet.Test
{
    [TestFixture]
    public partial class ApiTest
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
    }
}
