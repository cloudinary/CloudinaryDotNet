using CloudinaryDotNet.Actions;
using NUnit.Framework;
using SystemHttp = System.Net.Http;

namespace CloudinaryDotNet.Test
{
    public class UploadPresetsTest
    {
        private const string uploadPresetsRootUrl = "upload_presets";
        private const string folderName = "api_test_folder_name";
        private const string apiTestPreset = "api_test_upload_preset";
        private MockedCloudinary mockedCloudinary;

        [SetUp]
        public void OneTimeSetUp()
        {
            mockedCloudinary = new MockedCloudinary();
        }

        [Test]
        public void TestListUploadPresets()
        {
            mockedCloudinary.ListUploadPresets();

            mockedCloudinary.AssertHttpCall(SystemHttp.HttpMethod.Get, uploadPresetsRootUrl);
        }

        [Test]
        public void TestGetUploadPreset()
        {
            mockedCloudinary.GetUploadPreset(apiTestPreset);

            mockedCloudinary.AssertHttpCall(SystemHttp.HttpMethod.Get, $"{uploadPresetsRootUrl}/{apiTestPreset}");
        }

        [Test]
        public void TestDeleteUploadPreset()
        {
            mockedCloudinary.DeleteUploadPreset(apiTestPreset);

            mockedCloudinary.AssertHttpCall(SystemHttp.HttpMethod.Delete, $"{uploadPresetsRootUrl}/{apiTestPreset}");
        }

        [Test]
        public void TestUpdateUploadPreset()
        {
            var parameters = new UploadPresetParams
            {
                Name = apiTestPreset,
                Folder = folderName
            };

            mockedCloudinary.UpdateUploadPreset(parameters);

            mockedCloudinary.AssertHttpCall(SystemHttp.HttpMethod.Put, $"{uploadPresetsRootUrl}/{apiTestPreset}");
            Assert.True(mockedCloudinary.HttpRequestContent.Contains(folderName));
        }

        [Test]
        public void TestCreateUploadPreset()
        {
            var parameters = new UploadPresetParams
            {
                Name = apiTestPreset,
                Folder = folderName
            };

            mockedCloudinary.CreateUploadPreset(parameters);

            mockedCloudinary.AssertHttpCall(SystemHttp.HttpMethod.Post, uploadPresetsRootUrl);
            Assert.True(mockedCloudinary.HttpRequestContent.Contains(folderName));
        }
    }
}