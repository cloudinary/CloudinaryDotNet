using System;
using System.Linq;
using CloudinaryDotNet.Actions;
using NUnit.Framework;
using SystemHttp = System.Net.Http;

namespace CloudinaryDotNet.Tests
{
    public class UploadPresetsTest
    {
        private const string uploadPresetsRootUrl = "upload_presets";
        private const string folderName = "api_test_folder_name";
        private const string apiTestPreset = "api_test_upload_preset";

        private const string evalStr = "if (resource_info['width'] > 450) " +
                                       "{ upload_options['quality_analysis'] = true }; " +
                                       "upload_options['context'] = 'width = ' + resource_info['width']";

        private const string OnSuccessStr = "current_asset.update({tags: [\"autocaption\"]});";

        private MockedCloudinary mockedCloudinary;

        [SetUp]
        public void SetUp()
        {
            mockedCloudinary = new MockedCloudinary();
        }

        [Test]
        public void TestListUploadPresets()
        {
            var localCloudinaryMock = new MockedCloudinary("{presets: [{eval: 'some value', on_success: '" +
                                                           OnSuccessStr + "'}]}");

            var result = localCloudinaryMock.ListUploadPresets();

            localCloudinaryMock.AssertHttpCall(SystemHttp.HttpMethod.Get, uploadPresetsRootUrl);
            Assert.AreEqual("some value", result.Presets.First().Eval);
            Assert.AreEqual(OnSuccessStr, result.Presets.First().OnSuccess);
        }

        [Test]
        public void TestListUploadPresetsBooleanValues()
        {
            const string responseStr = @"
            {
              ""presets"": [
                { ""name"": ""int-str"", ""settings"": { ""overwrite"": ""1"" } },
                { ""name"": ""bool"", ""settings"": { ""overwrite"": true } },
                { ""name"": ""bool-str"", ""settings"": { ""overwrite"": ""true"" } },
                { ""name"": ""bool-str-false"", ""settings"": { ""overwrite"": ""false"" } },
                { ""name"": ""empty"", ""settings"": { ""overwrite"": """" } },
                { ""name"": ""default"", ""settings"": { ""no_overwrite"": true } },
              ]
            }";

            var localCloudinaryMock = new MockedCloudinary(responseStr);

            var result = localCloudinaryMock.ListUploadPresets();

            localCloudinaryMock.AssertHttpCall(SystemHttp.HttpMethod.Get, uploadPresetsRootUrl);

            Assert.IsTrue(result.Presets.First().Settings.Overwrite);
            Assert.IsTrue(result.Presets[1].Settings.Overwrite);
            Assert.IsTrue(result.Presets[2].Settings.Overwrite);
            Assert.IsFalse(result.Presets[3].Settings.Overwrite);
            Assert.IsNull(result.Presets[4].Settings.Overwrite);
            Assert.IsNull(result.Presets[5].Settings.Overwrite);
        }

        [TestCase("x-featureratelimit-limit", 123)]
        [TestCase("X-FeatureRateLimit-Limit", 123)]
        public void TestFeatureRateLimitLimitHeader(string headerName, long headerValue)
        {
            var message = new SystemHttp.HttpResponseMessage();
            var headers = message.Headers;

            headers.Add(headerName, headerValue.ToString());

            var localCloudinaryMock = new MockedCloudinary(httpResponseHeaders: headers);

            var result = localCloudinaryMock.ListUploadPresets();

            Assert.AreEqual(headerValue, result.Limit);
        }

        [TestCase("x-featureratelimit-reset", "Tue, 04 Jul 2023 19:00:00 GMT")]
        [TestCase("X-FeatureRateLimit-Reset", "Tue, 04 Jul 2023 19:00:00 GMT")]
        public void TestFeatureRateLimitLimitReset(string headerName, string headerValue)
        {
            var message = new SystemHttp.HttpResponseMessage();
            var headers = message.Headers;

            headers.Add(headerName, headerValue);

            var localCloudinaryMock = new MockedCloudinary(httpResponseHeaders: headers);

            var result = localCloudinaryMock.ListUploadPresets();

            Assert.AreEqual(DateTime.Parse(headerValue), result.Reset);
        }

        [TestCase("x-featureratelimit-remaining", 456)]
        [TestCase("X-FeatureRateLimit-Remaining", 465)]
        public void TestFeatureRateLimitLimitRemaining(string headerName, long headerValue)
        {
            var message = new SystemHttp.HttpResponseMessage();
            var headers = message.Headers;

            headers.Add(headerName, headerValue.ToString());

            var localCloudinaryMock = new MockedCloudinary(httpResponseHeaders: headers);

            var result = localCloudinaryMock.ListUploadPresets();

            Assert.AreEqual(headerValue, result.Remaining);
        }

        [Test]
        public void TestGetUploadPreset()
        {
            var localCloudinaryMock = new MockedCloudinary("{eval: 'some value', on_success: '" +
                                                           OnSuccessStr + "'}");

            var result = localCloudinaryMock.GetUploadPreset(apiTestPreset);

            localCloudinaryMock.AssertHttpCall(SystemHttp.HttpMethod.Get, $"{uploadPresetsRootUrl}/{apiTestPreset}");
            Assert.AreEqual("some value", result.Eval);
            Assert.AreEqual(OnSuccessStr, result.OnSuccess);
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
                Eval = evalStr,
                OnSuccess = OnSuccessStr
            };

            mockedCloudinary.UpdateUploadPreset(parameters);

            mockedCloudinary.AssertHttpCall(SystemHttp.HttpMethod.Put, $"{uploadPresetsRootUrl}/{apiTestPreset}");
            Assert.True(mockedCloudinary.HttpRequestContent.Contains(folderName));
            Assert.True(mockedCloudinary.HttpRequestContent.Contains(evalStr));
            Assert.True(mockedCloudinary.HttpRequestContent.Contains(OnSuccessStr));
        }

        [Test]
        public void TestCreateUploadPreset()
        {
            var parameters = new UploadPresetParams
            {
                Name = apiTestPreset,
                Folder = folderName,
                Eval = evalStr,
                OnSuccess = OnSuccessStr
            };

            mockedCloudinary.CreateUploadPreset(parameters);

            mockedCloudinary.AssertHttpCall(SystemHttp.HttpMethod.Post, uploadPresetsRootUrl);
            Assert.True(mockedCloudinary.HttpRequestContent.Contains(folderName));
            Assert.True(mockedCloudinary.HttpRequestContent.Contains(evalStr));
            Assert.True(mockedCloudinary.HttpRequestContent.Contains(OnSuccessStr));
        }
    }
}
