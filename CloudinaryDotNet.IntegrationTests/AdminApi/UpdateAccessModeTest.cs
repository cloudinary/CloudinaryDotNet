using System.Collections.Generic;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTests.AdminApi
{
    public class UpdateAccessModeTest : IntegrationTestBase
    {
        private const string ACCESS_MODE_PUBLIC = "public";

        [Test, RetryWithDelay]
        public void TestUpdateAccessModeByTag()
        {
            var updateTag = GetMethodTag();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{m_apiTag},{updateTag}",
                PublicId = GetUniquePublicId(),
                Overwrite = true,
                Type = STORAGE_TYPE_PRIVATE
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            var update_result = m_adminApi.UpdateResourceAccessModeByTag(updateTag,
                new UpdateResourceAccessModeParams()
                {
                    ResourceType = ResourceType.Image,
                    Type = STORAGE_TYPE_UPLOAD,
                    AccessMode = ACCESS_MODE_PUBLIC
                });

            //TODO: fix this test, make assertions working

            //Assert.AreEqual(publish_result.Published.Count, 1);
        }

        [Test, RetryWithDelay]
        public void TestUpdateAccessModeById()
        {
            var publicId = GetUniquePublicId();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag,
                PublicId = publicId,
                Overwrite = true,
                Type = STORAGE_TYPE_PRIVATE
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            List<string> ids = new List<string>
            {
                publicId
            };

            var update_result = m_adminApi.UpdateResourceAccessModeByIds(new UpdateResourceAccessModeParams()
            {
                ResourceType = ResourceType.Image,
                Type = STORAGE_TYPE_UPLOAD,
                AccessMode = ACCESS_MODE_PUBLIC,
                PublicIds = ids
            });

            //TODO: fix this test, make assertions working

            //Assert.AreEqual(publish_result.Published.Count, 1);
        }

        class UpdateAccessModeTestViaCloudinary : UpdateAccessModeTest
        {
            public UpdateAccessModeTestViaCloudinary()
            {
                AdminApiFactory = a => new Cloudinary(a);
            }
        }
    }
}
