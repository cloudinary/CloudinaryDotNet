using System.Linq;
using CloudinaryDotNet.Actions;
using NUnit.Framework;
using SystemHttp = System.Net.Http;

namespace CloudinaryDotNet.Tests.AdminApi
{
    [TestFixture]
    public class VisualSearchTest
    {
        private readonly string _responseData = @"
{
    'resources': [
        {
          'asset_id': '4db33872f5176841896baa4d3b4d1d5c',
          'public_id': 'sample',
          'format': 'jpg',
          'version': 1688960098,
          'resource_type': 'image',
          'type': 'upload',
          'created_at': '2023-07-10T03:34:58Z',
          'bytes': 120253,
          'width': 864,
          'height': 576,
          'asset_folder': '',
          'display_name': 'sample',
          'access_mode': 'public',
          'url': 'http://res.cloudinary.com/demo/image/upload/v1688960098/sample.jpg',
          'secure_url': 'https://res.cloudinary.com/demo/image/upload/v1688960098/sample.jpg',
          'tags': []
        }
    ],
    'total_count': 1
}
";
        [Test]
        public void TestVisualSearch()
        {
            var localCloudinaryMock = new MockedCloudinary(_responseData);

            var result = localCloudinaryMock.VisualSearch(new VisualSearchParams
            {
                ImageAssetId = TestConstants.TestAssetId,
                ImageUrl = TestConstants.TestRemoteImg,
                //ImageFile = new FileDescription("sample.jpg"),
                Text = "sample image",
            });

            //var requestContent = localCloudinaryMock.HttpRequestContent;

            //Assert.True(requestContent.Contains("image_file"));

            localCloudinaryMock.AssertHttpCall(
                SystemHttp.HttpMethod.Post,
                "resources/visual_search"
            );

            Assert.NotNull(result);

            Assert.AreEqual(1, result.TotalCount);
            Assert.AreEqual("sample", result.Resources[0].PublicId);
        }
    }
}
