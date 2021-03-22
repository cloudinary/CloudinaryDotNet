using System.Collections.Generic;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTests.UploadApi
{
    public class PublishMethodsTest : IntegrationTestBase
    {
        private const string STORAGE_TYPE_AUTHENTICATED = "authenticated";

        [Test, RetryWithDelay]
        public void TestPublishByIds()
        {
            var publicId = GetUniquePublicId(StorageType.upload, "test");
            var result = UploadTestImage($"{m_apiTag}", publicId, STORAGE_TYPE_AUTHENTICATED);

            var publishResult = m_cloudinary.PublishResourceByIds(null, new PublishResourceParams
            {
                PublicIds = new List<string> { result.PublicId },
                ResourceType = ResourceType.Image,
                Type = STORAGE_TYPE_AUTHENTICATED
            });

            Assert.NotNull(publishResult.Published, publishResult.Error?.Message);
            Assert.AreEqual(1, publishResult.Published.Count);
        }

        [Test]
        public async Task TestPublishByIdsAsync()
        {
            var publicId = GetUniquePublicId(StorageType.upload, "test");
            var tag = $"{m_apiTag}";
            var result = UploadTestImage(tag, publicId, STORAGE_TYPE_AUTHENTICATED);

            var publishResult = await m_cloudinary.PublishResourceByIdsAsync(tag, new PublishResourceParams
            {
                PublicIds = new List<string> { result.PublicId },
                ResourceType = ResourceType.Image,
                Type = STORAGE_TYPE_AUTHENTICATED
            }, null);

            Assert.NotNull(publishResult.Published, publishResult.Error?.Message);
            Assert.AreEqual(1, publishResult.Published.Count);
        }

        [Test, RetryWithDelay]
        public void TestPublishByPrefix()
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{m_apiTag}",
                PublicId = GetUniquePublicId(),
                Overwrite = true,
                Type = STORAGE_TYPE_AUTHENTICATED
            };

            m_cloudinary.Upload(uploadParams);

            var publishResult = m_cloudinary.PublishResourceByPrefix(
                                        uploadParams.PublicId.Substring(0, uploadParams.PublicId.Length - 2), new PublishResourceParams());

            Assert.NotNull(publishResult.Published, publishResult.Error?.Message);
            Assert.AreEqual(1, publishResult.Published.Count);
        }

        [Test, RetryWithDelay]
        public void TestPublishByTag()
        {
            var publishTag = GetMethodTag();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{publishTag},{m_apiTag}",
                PublicId = GetUniquePublicId(),
                Overwrite = true,
                Type = STORAGE_TYPE_PRIVATE
            };

            m_cloudinary.Upload(uploadParams);

            var publishResult = m_cloudinary.PublishResourceByTag(publishTag, new PublishResourceParams()
            {
                ResourceType = ResourceType.Image,
            });

            Assert.NotNull(publishResult.Published, publishResult.Error?.Message);
            Assert.AreEqual(1, publishResult.Published.Count);
        }

        [Test, RetryWithDelay]
        public void TestPublishWithType()
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{m_apiTag}",
                PublicId = GetUniquePublicId(),
                Overwrite = true,
                Type = STORAGE_TYPE_AUTHENTICATED
            };

            m_cloudinary.Upload(uploadParams);

            //publish with wrong type - verify publish fails
            var publishResult = m_cloudinary.PublishResourceByIds(null, new PublishResourceParams()
            {
                PublicIds = new List<string> { uploadParams.PublicId },
                ResourceType = ResourceType.Image,
                Type = STORAGE_TYPE_PRIVATE
            });

            Assert.NotNull(publishResult.Published, publishResult.Error?.Message);
            Assert.NotNull(publishResult.Failed);
            Assert.AreEqual(0, publishResult.Published.Count);
            Assert.AreEqual(1, publishResult.Failed.Count);

            //publish with correct type - verify publish succeeds
            publishResult = m_cloudinary.PublishResourceByIds(null, new PublishResourceParams()
            {
                PublicIds = new List<string> { uploadParams.PublicId },
                ResourceType = ResourceType.Image,
                Type = STORAGE_TYPE_AUTHENTICATED
            });

            Assert.NotNull(publishResult.Published, publishResult.Error?.Message);
            Assert.NotNull(publishResult.Failed);
            Assert.AreEqual(1, publishResult.Published.Count);
            Assert.AreEqual(0, publishResult.Failed.Count);
        }

        private ImageUploadResult UploadTestImage(string tags, string publicId, string type)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(m_testImagePath),
                Tags = tags,
                PublicId = publicId,
                Overwrite = true,
                Type = type
            };

            var result = m_cloudinary.Upload(uploadParams);
            return result;
        }
    }
}
