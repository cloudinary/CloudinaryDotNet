using CloudinaryDotNet.Actions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CloudinaryDotNet.IntegrationTests.UploadApi
{
    public class DestroyMethodsTest : IntegrationTestBase
    {
        [Test, RetryWithDelay]
        public void TestDestroyRaw()
        {
            var uploadResult = UploadTestRawResource(type: ApiShared.GetCloudinaryParam(ResourceType.Raw));

            var deletionParams = GetDeletionParams(uploadResult.PublicId);
            var destroyResult = m_cloudinary.Destroy(deletionParams);

            AssertDestroyed(destroyResult);
        }

        [Test, RetryWithDelay]
        public async Task TestDestroyRawAsync()
        {
            var uploadResult = await UploadTestRawResourceAsync(type: ApiShared.GetCloudinaryParam(ResourceType.Raw));

            var deletionParams = GetDeletionParams(uploadResult.PublicId);
            var destroyResult = await m_cloudinary.DestroyAsync(deletionParams);

            AssertDestroyed(destroyResult);
        }

        private DeletionParams GetDeletionParams(string publicId)
        {
            return new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Raw
            };
        }

        private void AssertDestroyed(DeletionResult result)
        {
            Assert.AreEqual("ok", result.Result, result.Error?.Message);
        }
    }
}
