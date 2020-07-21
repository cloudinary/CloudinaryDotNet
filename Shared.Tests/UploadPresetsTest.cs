using System.Linq;
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
        private const string evalStr = "if (resource_info['width'] < 450) { upload_options['tags'] = 'a,b' }";
        private MockedCloudinary mockedCloudinary;

        [SetUp]
        public void SetUp()
        {
            mockedCloudinary = new MockedCloudinary();
        }

        [Test]
        public void TestListUploadPresets()
        {
            var localCloudinaryMock = new MockedCloudinary("{presets: [{eval: 'some value'}]}");

            var result = localCloudinaryMock.ListUploadPresets();

            localCloudinaryMock.AssertHttpCall(SystemHttp.HttpMethod.Get, uploadPresetsRootUrl);
            Assert.AreEqual("some value", result.Presets.First().Eval);
        }

        [Test]
        public void TestGetUploadPreset()
        {
            var localCloudinaryMock = new MockedCloudinary("{eval: 'some value'}");

            var result = localCloudinaryMock.GetUploadPreset(apiTestPreset);

            localCloudinaryMock.AssertHttpCall(SystemHttp.HttpMethod.Get, $"{uploadPresetsRootUrl}/{apiTestPreset}");
            Assert.AreEqual("some value", result.Eval);
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
                Folder = folderName,
                Eval = evalStr
            };

            mockedCloudinary.UpdateUploadPreset(parameters);

            mockedCloudinary.AssertHttpCall(SystemHttp.HttpMethod.Put, $"{uploadPresetsRootUrl}/{apiTestPreset}");
            Assert.True(mockedCloudinary.HttpRequestContent.Contains(folderName));
            Assert.True(mockedCloudinary.HttpRequestContent.Contains(evalStr));
        }

        [Test]
        public void TestCreateUploadPreset()
        {
            var parameters = new UploadPresetParams
            {
                Name = apiTestPreset,
                Folder = folderName,
                Eval = evalStr
            };

            mockedCloudinary.CreateUploadPreset(parameters);

            mockedCloudinary.AssertHttpCall(SystemHttp.HttpMethod.Post, uploadPresetsRootUrl);
            Assert.True(mockedCloudinary.HttpRequestContent.Contains(folderName));
            Assert.True(mockedCloudinary.HttpRequestContent.Contains(evalStr));
        }
    }
}