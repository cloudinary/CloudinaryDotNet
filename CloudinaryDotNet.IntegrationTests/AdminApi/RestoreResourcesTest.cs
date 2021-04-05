using CloudinaryDotNet.Actions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudinaryDotNet.IntegrationTests.AdminApi
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
            var resource = m_adminApi.GetResource(publicId);
            AssertGetResourceResultBeforeDeletion(resource, publicId);

            var delResult = m_adminApi.DeleteResources(publicId);
            AssertResourceDeletionResult(delResult, publicId);

            resource = m_adminApi.GetResource(publicId);
            AssertGetResourceResultAfterDeletionNoBackup(resource);

            var rResult = m_adminApi.Restore(publicId);
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
            var resource = await m_adminApi.GetResourceAsync(publicId);
            AssertGetResourceResultBeforeDeletion(resource, publicId);

            var delResult = await m_adminApi.DeleteResourcesAsync(publicId);
            AssertResourceDeletionResult(delResult, publicId);

            resource = await m_adminApi.GetResourceAsync(publicId);
            AssertGetResourceResultAfterDeletionNoBackup(resource);

            var rResult = await m_adminApi.RestoreAsync(publicId);
            AssertRestoreResultNoBackup(rResult, publicId);
        }

        private void AssertGetResourceResultAfterDeletionNoBackup(GetResourceResult resource)
        {
            Assert.IsTrue(string.IsNullOrEmpty(resource.PublicId), resource.Error?.Message);
        }

        private void AssertRestoreResultNoBackup(RestoreResult rResult, string publicId)
        {
            Assert.IsNotNull(rResult.JsonObj[publicId], string.Format("Should contain key \"{0}\". ", publicId), rResult.Error?.Message);
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
            var resource_backup = m_adminApi.GetResource(publicId);
            AssertGetResourceResultBeforeDeletion(resource_backup, publicId);

            var delResult_backup = m_adminApi.DeleteResources(publicId);
            AssertResourceDeletionResult(delResult_backup, publicId);

            resource_backup = m_adminApi.GetResource(publicId);
            AssertGetResourceResultAfterDeletion(resource_backup);

            var rResult_backup = m_adminApi.Restore(publicId);
            AssertRestoreResult(rResult_backup, publicId);

            resource_backup = m_adminApi.GetResource(publicId);
            AssertGetResourceResultAfterRestore(resource_backup);
        }

        [Test, RetryWithDelay]
        public void TestRestoreWithVersions()
        {
            var publicIdBackup1 = GetUniquePublicId(suffix: "backup1");
            var publicIdBackup2 = GetUniquePublicId(suffix: "backup2");

            var uploadParams_backup1 = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicIdBackup1,
                Backup = true,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams_backup1);

            var uploadParams_backup2 = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicIdBackup2,
                Backup = true,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams_backup2);

            var resource_backup1 = m_adminApi.GetResource(new GetResourceParams(publicIdBackup1) { Versions = true });
            var resource_backup2 = m_adminApi.GetResource(new GetResourceParams(publicIdBackup2) { Versions = true });

            m_adminApi.DeleteResources(publicIdBackup1);
            m_adminApi.DeleteResources(publicIdBackup2);

            var rResult_backup = m_adminApi.Restore(new RestoreParams
            {
                PublicIds = new List<string>
                {
                    publicIdBackup1,
                    publicIdBackup2
                },
                Versions = new List<string>
                {
                    resource_backup1.Versions.First().VersionId,
                    resource_backup2.Versions.First().VersionId
                }
            });

            Assert.NotZero(rResult_backup.RestoredResources.Count, rResult_backup.Error?.Message);

            resource_backup1 = m_adminApi.GetResource(publicIdBackup1);
            resource_backup2 = m_adminApi.GetResource(publicIdBackup2);

            AssertGetResourceResultAfterRestore(resource_backup1);
            AssertGetResourceResultAfterRestore(resource_backup2);
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
            var resource_backup = await m_adminApi.GetResourceAsync(publicId);
            AssertGetResourceResultBeforeDeletion(resource_backup, publicId);

            var delResult_backup = await m_adminApi.DeleteResourcesAsync(publicId);
            AssertResourceDeletionResult(delResult_backup, publicId);

            resource_backup = await m_adminApi.GetResourceAsync(publicId);
            AssertGetResourceResultAfterDeletion(resource_backup);

            var rResult_backup = await m_adminApi.RestoreAsync(publicId);
            AssertRestoreResult(rResult_backup, publicId);

            resource_backup = await m_adminApi.GetResourceAsync(publicId);
            AssertGetResourceResultAfterRestore(resource_backup);
        }

        private void AssertGetResourceResultBeforeDeletion(GetResourceResult resource_backup, string publicId)
        {
            Assert.IsNotNull(resource_backup, resource_backup.Error?.Message);
            Assert.AreEqual(publicId, resource_backup.PublicId);
        }

        private void AssertResourceDeletionResult(DelResResult delResult_backup, string publicId)
        {
            Assert.AreEqual("deleted", delResult_backup.Deleted[publicId], delResult_backup.Error?.Message);
        }

        private void AssertGetResourceResultAfterDeletion(GetResourceResult resource_backup)
        {
            Assert.AreEqual(0, resource_backup.Bytes, resource_backup.Error?.Message);
        }

        private void AssertRestoreResult(RestoreResult rResult_backup, string publicId)
        {
            Assert.IsNotNull(rResult_backup.JsonObj[publicId], string.Format("Should contain key \"{0}\". ", publicId), rResult_backup.Error?.Message);
            Assert.AreEqual(publicId, rResult_backup.JsonObj[publicId]["public_id"].ToString());
            Assert.NotNull(rResult_backup.RestoredResources);
            Assert.IsTrue(rResult_backup.RestoredResources.Count > 0);
        }

        private void AssertGetResourceResultAfterRestore(GetResourceResult resource_backup)
        {
            Assert.IsFalse(string.IsNullOrEmpty(resource_backup.PublicId), resource_backup.Error?.Message);
        }

        class RestoreResourcesTestViaCloudinary : RestoreResourcesTest
        {
            public RestoreResourcesTestViaCloudinary()
            {
                AdminApiFactory = a => new Cloudinary(a);
            }
        }
    }
}
