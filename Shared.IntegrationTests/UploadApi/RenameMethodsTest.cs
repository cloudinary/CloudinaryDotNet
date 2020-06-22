using System.Net;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.UploadApi
{
    public class RenameMethodsTest : IntegrationTestBase
    {
        [Test]
        public void TestRename()
        {
            var toPublicId = GetUniquePublicId();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            };
            var uploadResult1 = m_cloudinary.Upload(uploadParams);

            uploadParams.File = new FileDescription(m_testIconPath);
            var uploadResult2 = m_cloudinary.Upload(uploadParams);

            var renameResult = m_cloudinary.Rename(uploadResult1.PublicId, toPublicId);
            Assert.AreEqual(HttpStatusCode.OK, renameResult.StatusCode);
            
            var getResult = m_cloudinary.GetResource(toPublicId);
            Assert.NotNull(getResult);

            renameResult = m_cloudinary.Rename(uploadResult2.PublicId, toPublicId);
            Assert.True(renameResult.StatusCode == HttpStatusCode.BadRequest);

            m_cloudinary.Rename(uploadResult2.PublicId, toPublicId, true);

            getResult = m_cloudinary.GetResource(toPublicId);
            Assert.NotNull(getResult);
            Assert.AreEqual(FILE_FORMAT_ICO, getResult.Format);
        }

        [Test]
        public void TestRenameToType()
        {
            string publicId = GetUniquePublicId();
            string newPublicId = GetUniquePublicId();

            var uploadParams = new ImageUploadParams()
            {
                PublicId = publicId,
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag,
                Type = STORAGE_TYPE_UPLOAD
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);
            Assert.AreEqual(uploadResult.StatusCode, HttpStatusCode.OK);

            RenameParams renameParams = new RenameParams(publicId, newPublicId)
            {
                ToType = STORAGE_TYPE_UPLOAD
            };

            var renameResult = m_cloudinary.Rename(renameParams);
            Assert.AreEqual(renameResult.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(renameResult.Type, STORAGE_TYPE_UPLOAD);
            Assert.AreEqual(renameResult.PublicId, newPublicId);
        }
    }
}
