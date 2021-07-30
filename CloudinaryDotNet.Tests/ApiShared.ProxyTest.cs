#if NETSTANDARD2_0
using NUnit.Framework;

namespace CloudinaryDotNet.Tests
{
    public class ApiSharedProxyTest
    {
        private ApiShared _apiShared;

        [SetUp]
        public void SetUp()
        {
            _apiShared = new ApiShared();
        }

        [Test]
        public void TestDoesNotRecreateClientOnEmptyProxy([Values(null, "")] string proxy)
        {
            var originalClient = _apiShared.Client;
            _apiShared.ApiProxy = proxy;

            Assert.AreEqual(originalClient, _apiShared.Client);
        }

        [Test]
        public void TestDoesNotRecreateClientOnTheSameProxy()
        {
            var proxy = "http://proxy.com";

            _apiShared.ApiProxy = proxy;
            var originalClient = _apiShared.Client;

            _apiShared.ApiProxy = proxy;

            Assert.AreEqual(originalClient, _apiShared.Client);
        }

        [Test]
        public void TestRecreatesClientWhenNewProxyIsSet()
        {
            var proxy = "http://proxy.com";
            var originalClient = _apiShared.Client;

            _apiShared.ApiProxy = proxy;

            Assert.AreNotEqual(originalClient, _apiShared.Client);
        }
    }
}
#endif
