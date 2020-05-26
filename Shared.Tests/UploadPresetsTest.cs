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

        [Test]
        public void TestListUploadPresets()
        {
            var mockedCloudinary = new MockedCloudinary();

            mockedCloudinary.ListUploadPresets();

            mockedCloudinary.AssertHttpCall(SystemHttp.HttpMethod.Get, uploadPresetsRootUrl);
        }

        [Test]
        public void TestGetUploadPreset()
        {
            var mockedCloudinary = new MockedCloudinary();
            
            mockedCloudinary.GetUploadPreset(apiTestPreset);

            mockedCloudinary.AssertHttpCall(SystemHttp.HttpMethod.Get, $"{uploadPresetsRootUrl}/{apiTestPreset}");
        }

        [Test]
        public void TestDeleteUploadPreset()
        {
            var mockedCloudinary = new MockedCloudinary();

            mockedCloudinary.DeleteUploadPreset(apiTestPreset);

            mockedCloudinary.AssertHttpCall(SystemHttp.HttpMethod.Delete, $"{uploadPresetsRootUrl}/{apiTestPreset}");
        }

        [Test]
        public void TestUpdateUploadPreset()
        {
            var mockedCloudinary = new MockedCloudinary();
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
            var mockedCloudinary = new MockedCloudinary();
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