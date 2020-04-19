using System;
using System.Collections.Generic;
using System.IO;
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
        private string _rootFolder1;
        private string _rootFolder2;
        private string _subFolder1;
        private string _subFolder2;

        [SetUp]
        public void SetUp()
        {
            _rootFolder1 = GetUniqueFolder("1") ;
            _rootFolder2 = GetUniqueFolder("2");
            _subFolder1 = $"{_rootFolder1}/test_subfolder1";
            _subFolder2 = $"{_rootFolder1}/test_subfolder2";

            var folders = new List<string> {
                $"{_rootFolder1}/item",
                $"{_rootFolder2}/item",
                $"{_subFolder1}/item",
                $"{_subFolder2}/item"
            };

            folders.ForEach(folder => { AssertCreateFolderResult(m_cloudinary.CreateFolder(folder)); });

            WaitForServerUpdate();
        }

        [Test]
        public void TestFolderApi()
        {
            var result = m_cloudinary.RootFolders();
            AssertGetRootFolders(result);

            result = m_cloudinary.SubFolders(_rootFolder1);

            AssertGetSubFolders(result);

            result = m_cloudinary.SubFolders(m_folderPrefix);

            AssertGetSubFoldersError(result);

            var deletionRes = m_cloudinary.DeleteFolder($"{_subFolder1}_");

            AssertDeleteFolderError(deletionRes);

            m_cloudinary.DeleteResourcesByPrefix(_subFolder1);

            deletionRes = m_cloudinary.DeleteFolder(_subFolder1);

            AssertDeleteFolder(deletionRes);
        }

        [Test]
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

        [Test]
        public async Task TestFolderApiAsync()
        {
            var result = await m_cloudinary.RootFoldersAsync();
            AssertGetRootFolders(result);

            result = await m_cloudinary.SubFoldersAsync(_rootFolder1);

            AssertGetSubFolders(result);

            result = await m_cloudinary.SubFoldersAsync(m_folderPrefix);

            AssertGetSubFoldersError(result);

            var deletionRes = await m_cloudinary.DeleteFolderAsync($"{_subFolder1}_");

            AssertDeleteFolderError(deletionRes);

            await m_cloudinary.DeleteResourcesByPrefixAsync(_subFolder1);

            deletionRes = await m_cloudinary.DeleteFolderAsync(_subFolder1);

            AssertDeleteFolder(deletionRes);
        }

        [Test]
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

            result = await m_cloudinary.SubFoldersAsync(_rootFolder1, parameters: getFoldersParams);

            AssertGetFolders(result);
            Assert.IsNotNull(result.NextCursor);

            getFoldersParams = new GetFoldersParams
            {
                MaxResults = 2,
                NextCursor = result.NextCursor
            };
            result = await m_cloudinary.SubFoldersAsync(_rootFolder1, parameters: getFoldersParams);

            AssertGetFolders(result);
        }

        private static void WaitForServerUpdate()
        {
            // TODO: fix race here (server might be not updated at this point)
            Thread.Sleep(2000);
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
            Assert.AreEqual(_subFolder1, deletionRes.Deleted[0]);
        }

        [Test]
        public void TestCreateFolder()
        {
            var folder  = GetUniqueFolder("create_folder");

            var createFolderResult = m_cloudinary.CreateFolder(folder);

            AssertCreateFolderResult(createFolderResult);
        }

        [Test]
        public async Task TestCreateFolderAsync()
        {
            var folder  = GetUniqueFolder("create_folder_async");

            var createFolderResult = await m_cloudinary.CreateFolderAsync(folder);

            AssertCreateFolderResult(createFolderResult);
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
            var testFolderName = GetUniqueFolder("root_folder");
            const string testSubFolderName = "test_sub_folder";
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
