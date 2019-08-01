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
            RawUploadParams uploadParams = new RawUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            };

            RawUploadResult uploadResult = m_cloudinary.Upload(uploadParams, Api.GetCloudinaryParam(ResourceType.Raw));

            Assert.NotNull(uploadResult);

            DeletionParams destroyParams = new DeletionParams(uploadResult.PublicId)
            {
                ResourceType = ResourceType.Raw
            };

            DeletionResult destroyResult = m_cloudinary.Destroy(destroyParams);

            Assert.AreEqual("ok", destroyResult.Result);
        }

        [Test]
        public async Task TestDestroyRawAsync()
        {
            var uploadParams = new RawUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            };

            var uploadResult = await m_cloudinary.UploadAsync(
                uploadParams,
                ApiShared.GetCloudinaryParam(ResourceType.Raw));

            Assert.NotNull(uploadResult);

            var destroyParams = new DeletionParams(uploadResult.PublicId)
            {
                ResourceType = ResourceType.Raw
            };

            var destroyResult = await m_cloudinary.DestroyAsync(destroyParams);

            Assert.AreEqual("ok", destroyResult.Result);
        }
    }
}
