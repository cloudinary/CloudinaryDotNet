using NUnit.Framework;
using SystemHttp = System.Net.Http;

namespace CloudinaryDotNet.Tests.SearchApi
{
    public class SearchFoldersTest
    {
        private MockedCloudinary _cloudinary = new MockedCloudinary();

        [SetUp]
        public void SetUp()
        {
            _cloudinary = new MockedCloudinary();
        }

        [Test]
        public void TestShouldSearchFolders()
        {
            _cloudinary
                .SearchFolders()
                .Expression("path:*")
                .MaxResults(1)
                .Execute();

            _cloudinary.AssertHttpCall(SystemHttp.HttpMethod.Post, "folders/search");

            var requestJson = _cloudinary.RequestJson();

            Assert.IsNotNull(requestJson["expression"]);
            Assert.AreEqual("path:*", requestJson["expression"].ToString());
            Assert.IsNotNull(requestJson["max_results"]);
            Assert.AreEqual("1", requestJson["max_results"].ToString());
        }
    }
}
