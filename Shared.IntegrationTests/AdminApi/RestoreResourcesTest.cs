using CloudinaryDotNet.Actions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CloudinaryDotNet.IntegrationTest.AdminApi
{
    public class RestoreResourcesTest: IntegrationTestBase
    {
        [Test, RetryWithDelay]
        public void TestRestoreNoBackup()
        {
            var publicId = GetUniquePublicId();

            var uploadParams_nobackup = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams_nobackup);
            var resource = m_cloudinary.GetResource(publicId);
            AssertGetResourceResultBeforeDeletion(resource, publicId);

            var delResult = m_cloudinary.DeleteResources(publicId);
            AssertResourceDeletionResult(delResult, publicId);

            resource = m_cloudinary.GetResource(publicId);
            AssertGetResourceResultAfterDeletionNoBackup(resource);

            var rResult = m_cloudinary.Restore(publicId);
            AssertRestoreResultNoBackup(rResult, publicId);
        }

        [Test, RetryWithDelay]
        public async Task TestRestoreNoBackupAsync()
        {
            var publicId = GetUniqueAsyncPublicId();

            var uploadParams_nobackup = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams_nobackup);
            var resource = await m_cloudinary.GetResourceAsync(publicId);
            AssertGetResourceResultBeforeDeletion(resource, publicId);

            var delResult = await m_cloudinary.DeleteResourcesAsync(publicId);
            AssertResourceDeletionResult(delResult, publicId);

            resource = await m_cloudinary.GetResourceAsync(publicId);
            AssertGetResourceResultAfterDeletionNoBackup(resource);

            var rResult = await m_cloudinary.RestoreAsync(publicId);
            AssertRestoreResultNoBackup(rResult, publicId);
        }

        private void AssertGetResourceResultAfterDeletionNoBackup(GetResourceResult resource)
        {
            Assert.IsTrue(string.IsNullOrEmpty(resource.PublicId));
        }

        private void AssertRestoreResultNoBackup(RestoreResult rResult, string publicId)
        {
            Assert.IsNotNull(rResult.JsonObj[publicId], string.Format("Should contain key \"{0}\". ", publicId));
            Assert.AreEqual("no_backup", rResult.JsonObj[publicId]["error"].ToString());
        }

        [Test, RetryWithDelay]
        public void TestRestore()
        {
            var publicId = GetUniquePublicId();

            var uploadParams_backup = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Backup = true,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams_backup);
            var resource_backup = m_cloudinary.GetResource(publicId);
            AssertGetResourceResultBeforeDeletion(resource_backup, publicId);

            var delResult_backup = m_cloudinary.DeleteResources(publicId);
            AssertResourceDeletionResult(delResult_backup, publicId);

            resource_backup = m_cloudinary.GetResource(publicId);
            AssertGetResourceResultAfterDeletion(resource_backup);

            var rResult_backup = m_cloudinary.Restore(publicId);
            AssertRestoreResult(rResult_backup, publicId);

            resource_backup = m_cloudinary.GetResource(publicId);
            AssertGetResourceResultAfterRestore(resource_backup);
        }

        [Test, RetryWithDelay]
        public async Task TestRestoreAsync()
        {
            var publicId = GetUniqueAsyncPublicId();

            var uploadParams_backup = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Backup = true,
                Tags = m_apiTag
            };

            await m_cloudinary.UploadAsync(uploadParams_backup);
            var resource_backup = await m_cloudinary.GetResourceAsync(publicId);
            AssertGetResourceResultBeforeDeletion(resource_backup, publicId);

            var delResult_backup = await m_cloudinary.DeleteResourcesAsync(publicId);
            AssertResourceDeletionResult(delResult_backup, publicId);

            resource_backup = await m_cloudinary.GetResourceAsync(publicId);
            AssertGetResourceResultAfterDeletion(resource_backup);

            var rResult_backup = await m_cloudinary.RestoreAsync(publicId);
            AssertRestoreResult(rResult_backup, publicId);

            resource_backup = await m_cloudinary.GetResourceAsync(publicId);
            AssertGetResourceResultAfterRestore(resource_backup);
        }

        private void AssertGetResourceResultBeforeDeletion(GetResourceResult resource_backup, string publicId)
        {
            Assert.IsNotNull(resource_backup);
            Assert.AreEqual(publicId, resource_backup.PublicId);
        }

        private void AssertResourceDeletionResult(DelResResult delResult_backup, string publicId)
        {
            Assert.AreEqual("deleted", delResult_backup.Deleted[publicId]);
        }

        private void AssertGetResourceResultAfterDeletion(GetResourceResult resource_backup)
        {
            Assert.AreEqual(0, resource_backup.Bytes);
        }

        private void AssertRestoreResult(RestoreResult rResult_backup, string publicId)
        {
            Assert.IsNotNull(rResult_backup.JsonObj[publicId], string.Format("Should contain key \"{0}\". ", publicId));
            Assert.AreEqual(publicId, rResult_backup.JsonObj[publicId]["public_id"].ToString());
            Assert.NotNull(rResult_backup.RestoredResources);
            Assert.IsTrue(rResult_backup.RestoredResources.Count > 0);
        }

        private void AssertGetResourceResultAfterRestore(GetResourceResult resource_backup)
        {
            Assert.IsFalse(string.IsNullOrEmpty(resource_backup.PublicId));
        }
    }
}
