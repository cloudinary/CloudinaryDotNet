using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.AdminApi
{
    public class ManageFoldersTest : IntegrationTestBase
    {
        // For this test to work, "Auto-create folders" should be enabled in the Upload Settings, so this test is disabled by default.
        [Test, IgnoreFeature("auto_create_folders")]
        public void TestFolderApi()
        {
            // should allow to list folders and subfolders
            var subFolder1 = $"{m_folderPrefix}1/test_subfolder1";
            var subFolder2 = $"{m_folderPrefix}1/test_subfolder2";

            var publicIds = new List<string> {
                $"{m_folderPrefix}1/item",
                $"{m_folderPrefix}2/item",
                $"{subFolder1}/item",
                $"{subFolder2}/item"
            };

            publicIds.ForEach(p => m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = p,
                Tags = m_apiTag
            }));

            var result = m_cloudinary.RootFolders();
            Assert.Null(result.Error);
            Assert.IsTrue(result.Folders.Any(folder => folder.Name == $"{m_folderPrefix}1"));
            Assert.IsTrue(result.Folders.Any(folder => folder.Name == $"{m_folderPrefix}2"));
            Assert.NotZero(result.TotalCount);

            // TODO: fix race here (server might be not updated at this point)
            Thread.Sleep(2000);

            result = m_cloudinary.SubFolders($"{m_folderPrefix}1");

            Assert.AreEqual(2, result.Folders.Count);
            Assert.AreEqual(subFolder1, result.Folders[0].Path);
            Assert.AreEqual(subFolder2, result.Folders[1].Path);
            Assert.NotZero(result.TotalCount);

            result = m_cloudinary.SubFolders(m_folderPrefix);

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.NotNull(result.Error.Message);
            Assert.AreEqual($"Can't find folder with path {m_folderPrefix}", result.Error.Message);

            var deletionRes = m_cloudinary.DeleteFolder(subFolder1);

            Assert.AreEqual(HttpStatusCode.BadRequest, deletionRes.StatusCode);
            Assert.NotNull(deletionRes.Error);
            Assert.NotNull(deletionRes.Error.Message);
            Assert.AreEqual("Folder is not empty", deletionRes.Error.Message);

            m_cloudinary.DeleteResourcesByPrefix(subFolder1);

            deletionRes = m_cloudinary.DeleteFolder(subFolder1);

            Assert.Null(deletionRes.Error);
            Assert.AreEqual(1, deletionRes.Deleted.Count);
            Assert.AreEqual(subFolder1, deletionRes.Deleted[0]);
        }

        [Test]
        public void TestCreateFolder()
        {
            var testFolderName = $"{m_folderPrefix}_create_folder";

            var createFolderResult = m_cloudinary.CreateFolder(testFolderName);

            AssertCreateFolderResult(createFolderResult);

            Thread.Sleep(2000);

            var deletionResult = m_cloudinary.DeleteFolder(testFolderName);

            AssertFolderDeletionAfterCreate(deletionResult, testFolderName);
        }

        [Test]
        public async Task TestCreateFolderAsync()
        {
            var testFolderName = $"{m_folderPrefix}_create_folder_async";

            var createFolderResult = await m_cloudinary.CreateFolderAsync(testFolderName);

            AssertCreateFolderResult(createFolderResult);

            Thread.Sleep(2000);

            var deletionResult = await m_cloudinary.DeleteFolderAsync(testFolderName);

            AssertFolderDeletionAfterCreate(deletionResult, testFolderName);
        }

        [TestCase("")]
        [TestCase(null)]
        public void TestCreateFolderWithNullOrEmptyValue(string testFolderName)
        {
            Assert.Throws<ArgumentException>(() => m_cloudinary.CreateFolder(testFolderName));
        }

        [Test]
        public void TestCreateFolderWithSubFolders()
        {
            var testFolderName = $"{m_folderPrefix}_root_folder";
            var testSubFolderName = "test_sub_folder";
            var testPath = $"{testFolderName}/{testSubFolderName}";

            var createFolderResult = m_cloudinary.CreateFolder(testPath);

            AssertCreateFolderResult(createFolderResult);
            Assert.AreEqual(testSubFolderName, createFolderResult.Name);
            Assert.AreEqual(testPath, createFolderResult.Path);

            Thread.Sleep(2000);

            var result = m_cloudinary.RootFolders();
            Assert.Null(result.Error);
            Assert.IsTrue(result.Folders.Any(folder => folder.Name == testFolderName));

            result = m_cloudinary.SubFolders(testFolderName);

            Assert.AreEqual(1, result.Folders.Count);
            Assert.IsTrue(result.Folders.Count > 0);

            var deletionResult = m_cloudinary.DeleteFolder(testFolderName);

            AssertFolderDeletionAfterCreate(deletionResult, testFolderName);
        }

        private void AssertCreateFolderResult(CreateFolderResult createFolderResult)
        {
            Assert.NotNull(createFolderResult);
            Assert.IsTrue(createFolderResult.Success);
            Assert.NotNull(createFolderResult.Name);
            Assert.NotNull(createFolderResult.Path);
        }

        private void AssertFolderDeletionAfterCreate(DeleteFolderResult deletionResult, string testFolderName)
        {
            Assert.AreEqual(HttpStatusCode.OK, deletionResult.StatusCode);
            Assert.IsTrue(deletionResult.Deleted.Any(f => f == testFolderName));
        }
    }
}
