using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.AdminApi
{
    public class RestoreResourcesTest: IntegrationTestBase
    {
        [Test]
        public void TestRestoreNoBackup()
        {
            string publicId = GetUniquePublicId();

            ImageUploadParams uploadParams_nobackup = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams_nobackup);
            GetResourceResult resource = m_cloudinary.GetResource(publicId);
            Assert.IsNotNull(resource);
            Assert.AreEqual(publicId, resource.PublicId);

            DelResResult delResult = m_cloudinary.DeleteResources(publicId);
            Assert.AreEqual("deleted", delResult.Deleted[publicId]);

            resource = m_cloudinary.GetResource(publicId);
            Assert.IsTrue(string.IsNullOrEmpty(resource.PublicId));

            RestoreResult rResult = m_cloudinary.Restore(publicId);
            Assert.IsNotNull(rResult.JsonObj[publicId], string.Format("Should contain key \"{0}\". ", publicId));
            Assert.AreEqual("no_backup", rResult.JsonObj[publicId]["error"].ToString());
        }

        [Test]
        public void TestRestore()
        {
            string publicId = GetUniquePublicId();

            ImageUploadParams uploadParams_backup = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Backup = true,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams_backup);
            GetResourceResult resource_backup = m_cloudinary.GetResource(publicId);
            Assert.IsNotNull(resource_backup);
            Assert.AreEqual(publicId, resource_backup.PublicId);

            DelResResult delResult_backup = m_cloudinary.DeleteResources(publicId);
            Assert.AreEqual("deleted", delResult_backup.Deleted[publicId]);

            resource_backup = m_cloudinary.GetResource(publicId);
            Assert.AreEqual(0, resource_backup.Length);

            RestoreResult rResult_backup = m_cloudinary.Restore(publicId);
            Assert.IsNotNull(rResult_backup.JsonObj[publicId], string.Format("Should contain key \"{0}\". ", publicId));
            Assert.AreEqual(publicId, rResult_backup.JsonObj[publicId]["public_id"].ToString());

            resource_backup = m_cloudinary.GetResource(publicId);
            Assert.IsFalse(string.IsNullOrEmpty(resource_backup.PublicId));
        }
    }
}
