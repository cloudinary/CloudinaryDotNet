using System;
using System.Net;
using System.Reflection;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest
{
    [TestFixture]
    public partial class IntegrationTestBase
    {

        [OneTimeSetUp]
        public virtual void Initialize()
        {
            Initialize(typeof(IntegrationTestBase).GetTypeInfo().Assembly);
        }

        protected virtual string GetMethodTag([System.Runtime.CompilerServices.CallerMemberName]string memberName = "")
        {
            return $"{m_apiTag}_{memberName}";
        }

        protected bool CanArchiveFileBeDownloaded(string url)
        {
            var request = WebRequest.Create(new Uri(url));
            request.Method = "GET";
            using (var response = (HttpWebResponse)request.GetResponseAsync().Result)
            {
                return response.StatusCode == HttpStatusCode.OK;
            }
        }
    }
}
