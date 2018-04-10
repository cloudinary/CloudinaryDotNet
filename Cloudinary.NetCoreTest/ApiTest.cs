using System;
using System.Collections.Generic;
using System.Net.Http;
using NUnit.Framework;

namespace CloudinaryDotNet.Test
{
    [TestFixture]
    public partial class ApiTest
    {
        [Test]
        public void TestAgentPlatformHeaders()
        {
            HttpRequestMessage request = new HttpRequestMessage { RequestUri = new Uri("http://dummy.com") };
            m_api.UserPlatform = "Test/1.0";

            m_api.PrepareRequestBody(request, HttpMethod.GET, new SortedDictionary<string, object>(), new FileDescription(""));

            //Can't test the result, so we just verify the UserAgent parameter is sent to the server
            StringAssert.AreEqualIgnoringCase(string.Format("{0} {1}", m_api.UserPlatform, Api.USER_AGENT), request.Headers.UserAgent.ToString());
            StringAssert.IsMatch(@"Test\/1\.0 CloudinaryDotNet\/(\d+)\.(\d+)\.(\d+) \(.*\)", request.Headers.UserAgent.ToString());
        }

    }
}
