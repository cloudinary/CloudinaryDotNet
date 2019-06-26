using CloudinaryDotNet.Actions;
using NUnit.Framework;

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
    }
}
