using System.Collections.Generic;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.UploadApi
{
    public class PublishMethodsTest : IntegrationTestBase
    {
        private const string STORAGE_TYPE_AUTHENTICATED = "authenticated";

        [Test]
        public void TestPublishByIds()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{m_apiTag}",
                PublicId = GetUniquePublicId(StorageType.upload, "test"),
                Overwrite = true,
                Type = STORAGE_TYPE_AUTHENTICATED
            };

            var result = m_cloudinary.Upload(uploadParams);

            var publish_result = m_cloudinary.PublishResourceByIds(null, new PublishResourceParams()
            {
                PublicIds = new List<string> { result.PublicId },
                ResourceType = ResourceType.Image,
                Type = STORAGE_TYPE_AUTHENTICATED
            });
            Assert.NotNull(publish_result.Published);
            Assert.AreEqual(1, publish_result.Published.Count);
        }

        [Test]
        public void TestPublishByPrefix()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{m_apiTag}",
                PublicId = GetUniquePublicId(),
                Overwrite = true,
                Type = STORAGE_TYPE_AUTHENTICATED
            };

            m_cloudinary.Upload(uploadParams);

            var publish_result = m_cloudinary.PublishResourceByPrefix(
                                        uploadParams.PublicId.Substring(0, uploadParams.PublicId.Length - 2), new PublishResourceParams());

            Assert.NotNull(publish_result.Published);
            Assert.AreEqual(1, publish_result.Published.Count);
        }

        [Test]
        public void TestPublishByTag()
        {
            var publishTag = GetMethodTag();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{publishTag},{m_apiTag}",
                PublicId = GetUniquePublicId(),
                Overwrite = true,
                Type = STORAGE_TYPE_PRIVATE
            };

            m_cloudinary.Upload(uploadParams);

            var publish_result = m_cloudinary.PublishResourceByTag(publishTag, new PublishResourceParams()
            {
                ResourceType = ResourceType.Image,
            });

            Assert.NotNull(publish_result.Published);
            Assert.AreEqual(1, publish_result.Published.Count);
        }

        [Test]
        public void TestPublishWithType()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{m_apiTag}",
                PublicId = GetUniquePublicId(),
                Overwrite = true,
                Type = STORAGE_TYPE_AUTHENTICATED
            };

            m_cloudinary.Upload(uploadParams);

            //publish with wrong type - verify publish fails
            var publish_result = m_cloudinary.PublishResourceByIds(null, new PublishResourceParams()
            {
                PublicIds = new List<string> { uploadParams.PublicId },
                ResourceType = ResourceType.Image,
                Type = STORAGE_TYPE_PRIVATE
            });

            Assert.NotNull(publish_result.Published);
            Assert.NotNull(publish_result.Failed);
            Assert.AreEqual(0, publish_result.Published.Count);
            Assert.AreEqual(1, publish_result.Failed.Count);

            //publish with correct type - verify publish succeeds
            publish_result = m_cloudinary.PublishResourceByIds(null, new PublishResourceParams()
            {
                PublicIds = new List<string> { uploadParams.PublicId },
                ResourceType = ResourceType.Image,
                Type = STORAGE_TYPE_AUTHENTICATED
            });

            Assert.NotNull(publish_result.Published);
            Assert.NotNull(publish_result.Failed);
            Assert.AreEqual(1, publish_result.Published.Count);
            Assert.AreEqual(0, publish_result.Failed.Count);
        }
    }
}
