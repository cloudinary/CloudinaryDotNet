using CloudinaryDotNet.Actions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CloudinaryDotNet.IntegrationTest.UploadApi
{
    public class DestroyMethodsTest : IntegrationTestBase
    {
        [Test]
        public void TestDestroyRaw()
        {
            var uploadResult = UploadTestRawResource(type: ApiShared.GetCloudinaryParam(ResourceType.Raw));

            var deletionParams = GetDeletionParams(uploadResult.PublicId);
            var destroyResult = m_cloudinary.Destroy(deletionParams);

            CheckDestroyed(destroyResult);
        }

        [Test]
        public async Task TestDestroyRawAsync()
        {
            var uploadResult = await UploadTestRawResourceAsync(type: ApiShared.GetCloudinaryParam(ResourceType.Raw));

            var deletionParams = GetDeletionParams(uploadResult.PublicId);
            var destroyResult = await m_cloudinary.DestroyAsync(deletionParams);

            CheckDestroyed(destroyResult);
        }

        private DeletionParams GetDeletionParams(string publicId)
        {
            return new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Raw
            };
        }

        private void CheckDestroyed(DeletionResult result)
        {
            Assert.AreEqual("ok", result.Result);
        }
    }
}
