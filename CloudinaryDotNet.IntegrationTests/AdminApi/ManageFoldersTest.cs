using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTests.AdminApi
{
    public class ManageFoldersTest : IntegrationTestBase
    {
        private string _rootFolder1;
        private string _rootFolder2;
        private string _subFolder1;
        private string _subFolder2;

        private const string NON_EXISTING_FOLDER = "non_existing_folder";
        private const string TEST_SUB_FOLDER = "test_sub_folder";

        [SetUp]
        public void SetUp()
        {
            PrepareFoldersForTests();
            WaitForServerUpdate();
        }

        [Test, RetryWithDelay]
        public void TestFolderApi()
        {
            var result = m_cloudinary.RootFolders();
            AssertGetRootFolders(result);

            result = m_cloudinary.SubFolders(_rootFolder1);

            AssertGetSubFolders(result);

            result = m_cloudinary.SubFolders(m_folderPrefix);

            AssertGetSubFoldersError(result);

            var deletionRes = m_cloudinary.DeleteFolder(NON_EXISTING_FOLDER);

            AssertDeleteFolderError(deletionRes);

            m_cloudinary.DeleteResourcesByPrefix(_subFolder1);

            deletionRes = m_cloudinary.DeleteFolder(_subFolder1);

            AssertDeleteFolder(deletionRes);
        }

        [Test, RetryWithDelay]
        public void TestFolderApiWithParameters()
        {
            var getFoldersParams = new GetFoldersParams
            {
                MaxResults = 2
            };

            var result = m_cloudinary.RootFolders(getFoldersParams);

            AssertGetFolders(result);
            Assert.IsNotNull(result.NextCursor);

            getFoldersParams = new GetFoldersParams
            {
                MaxResults = 2,
                NextCursor = result.NextCursor
            };
            result = m_cloudinary.RootFolders(getFoldersParams);

            AssertGetFolders(result);

            getFoldersParams = new GetFoldersParams
            {
                MaxResults = 2
            };

            result = m_cloudinary.SubFolders(_rootFolder1, getFoldersParams);

            AssertGetFolders(result);
            Assert.IsNotNull(result.NextCursor);

            getFoldersParams = new GetFoldersParams
            {
                MaxResults = 2,
                NextCursor = result.NextCursor
            };
            result = m_cloudinary.SubFolders(_rootFolder1, getFoldersParams);

            AssertGetFolders(result);
        }

        [Test, RetryWithDelay]
        public async Task TestFolderApiAsync()
        {
            var result = await m_cloudinary.RootFoldersAsync();
            AssertGetRootFolders(result);

            result = await m_cloudinary.SubFoldersAsync(_rootFolder1);

            AssertGetSubFolders(result);

            result = await m_cloudinary.SubFoldersAsync(m_folderPrefix);

            AssertGetSubFoldersError(result);

            var deletionRes = await m_cloudinary.DeleteFolderAsync(NON_EXISTING_FOLDER);

            AssertDeleteFolderError(deletionRes);

            await m_cloudinary.DeleteResourcesByPrefixAsync(_subFolder1);

            deletionRes = await m_cloudinary.DeleteFolderAsync(_subFolder1);

            AssertDeleteFolder(deletionRes);
        }

        [Test, RetryWithDelay]
        public async Task TestFolderApiWithParametersAsync()
        {
            var getFoldersParams = new GetFoldersParams
            {
                MaxResults = 2
            };

            var result = await m_cloudinary.RootFoldersAsync(getFoldersParams);

            AssertGetFolders(result);
            Assert.IsNotNull(result.NextCursor);

            getFoldersParams = new GetFoldersParams
            {
                MaxResults = 2,
                NextCursor = result.NextCursor
            };
            result = await m_cloudinary.RootFoldersAsync(getFoldersParams);

            AssertGetFolders(result);

            getFoldersParams = new GetFoldersParams
            {
                MaxResults = 2
            };

            result = await m_cloudinary.SubFoldersAsync(_rootFolder1, getFoldersParams);

            AssertGetFolders(result);
            Assert.IsNotNull(result.NextCursor);

            getFoldersParams = new GetFoldersParams
            {
                MaxResults = 2,
                NextCursor = result.NextCursor
            };
            result = await m_cloudinary.SubFoldersAsync(_rootFolder1, getFoldersParams);

            AssertGetFolders(result);
        }

        private static void WaitForServerUpdate()
        {
            // TODO: fix race here (server might be not updated at this point)
            Thread.Sleep(2000);
        }

        private void PrepareFoldersForTests()
        {
            _rootFolder1 = GetUniqueFolder("1");
            _rootFolder2 = GetUniqueFolder("2");
            _subFolder1 = GetSubFolder(_rootFolder1, "1");
            _subFolder2 = GetSubFolder(_rootFolder1, "2");

            var folders = new List<string>
            {
                GetItemSubFolder(_rootFolder1),
                GetItemSubFolder(_rootFolder2),
                GetItemSubFolder(_subFolder1),
                GetItemSubFolder(_subFolder2)
            };

            folders.ForEach(folder => m_cloudinary.CreateFolder(folder));
        }

        private string GetSubFolder(string folder, string subFolderSuffix = "")
        {
            return $"{folder}/{TEST_SUB_FOLDER}{subFolderSuffix}";
        }

        private string GetItemSubFolder(string folder)
        {
            return $"{folder}/item";
        }

        private void AssertGetFolders(GetFoldersResult result)
        {
            Assert.Null(result.Error);
            Assert.IsTrue(result.Folders.Count > 0);
        }

        private void AssertGetRootFolders(GetFoldersResult result)
        {
            Assert.Null(result.Error);
            Assert.IsTrue(result.Folders.Any(folder => folder.Name == _rootFolder1));
            Assert.IsTrue(result.Folders.Any(folder => folder.Name == _rootFolder2));
            Assert.NotZero(result.TotalCount);
        }

        private void AssertGetSubFolders(GetFoldersResult result)
        {
            Assert.AreEqual(3, result.Folders.Count);
            Assert.IsTrue(result.Folders.Any(_ => _.Path == _subFolder1));
            Assert.IsTrue(result.Folders.Any(_ => _.Path == _subFolder2));
            Assert.NotZero(result.TotalCount);
        }

        private void AssertGetSubFoldersError(GetFoldersResult result)
        {
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.NotNull(result.Error.Message);
            Assert.AreEqual($"Can't find folder with path {m_folderPrefix}", result.Error.Message);
        }

        private void AssertDeleteFolderError(DeleteFolderResult deletionRes)
        {
            Assert.AreEqual(HttpStatusCode.NotFound, deletionRes.StatusCode);
            Assert.NotNull(deletionRes.Error);
            Assert.NotNull(deletionRes.Error.Message);
        }

        private void AssertDeleteFolder(DeleteFolderResult deletionRes)
        {
            Assert.Null(deletionRes.Error);
            Assert.AreEqual(2, deletionRes.Deleted.Count);
            Assert.Contains(_subFolder1, deletionRes.Deleted);
        }

        [Test, RetryWithDelay]
        public void TestCreateFolder()
        {
            var folder  = GetUniqueFolder("create_folder");

            var createFolderResult = m_cloudinary.CreateFolder(folder);

            AssertCreateFolderResult(createFolderResult);
        }

        [Test, RetryWithDelay]
        public async Task TestCreateFolderAsync()
        {
            var folder  = GetUniqueFolder("create_folder_async");

            var createFolderResult = await m_cloudinary.CreateFolderAsync(folder);

            AssertCreateFolderResult(createFolderResult);
        }

        [TestCase(""), RetryWithDelay]
        [TestCase(null)]
        public void TestCreateFolderWithNullOrEmptyValue(string testFolderName)
        {
            Assert.Throws<ArgumentException>(() => m_cloudinary.CreateFolder(testFolderName));
        }

        [Test, RetryWithDelay]
        public void TestCreateFolderWithSubFolders()
        {
            var testFolderName = GetUniqueFolder("root_folder");
            var testPath = GetSubFolder(testFolderName);

            var createFolderResult = m_cloudinary.CreateFolder(testPath);

            AssertCreateFolderResult(createFolderResult);
            Assert.AreEqual(TEST_SUB_FOLDER, createFolderResult.Name);
            Assert.AreEqual(testPath, createFolderResult.Path);

            WaitForServerUpdate();

            var result = m_cloudinary.RootFolders();

            Assert.Null(result.Error);
            Assert.IsTrue(result.Folders.Any(folder => folder.Name == testFolderName));

            result = m_cloudinary.SubFolders(testFolderName);

            Assert.AreEqual(1, result.Folders.Count);
            Assert.IsTrue(result.Folders.Count > 0);
        }

        private void AssertCreateFolderResult(CreateFolderResult createFolderResult)
        {
            Assert.NotNull(createFolderResult);
            Assert.IsTrue(createFolderResult.Success);
            Assert.NotNull(createFolderResult.Name);
            Assert.NotNull(createFolderResult.Path);
        }
    }
}
