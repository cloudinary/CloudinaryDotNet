using System;
using System.Collections.Generic;
using CloudinaryDotNet.Actions;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Web;

namespace CloudinaryDotNet.IntegrationTests.UploadApi
{
    public class MultiMethodsTest : IntegrationTestBase
    {
        [Test, RetryWithDelay]
        public void TestMultiTransformation()
        {
            var tag = GetMethodTag();
            var uploadResult1 = UploadTestImageResource((uploadParams) =>
            {
                uploadParams.Tags = $"{tag},{m_apiTag}";
            },
            StorageType.multi);
            var uploadResult2 = UploadTestImageResource((uploadParams) =>
            {
                uploadParams.Tags = $"{tag},{m_apiTag}";
                uploadParams.Transformation = m_simpleTransformation;
            },
            StorageType.multi);

            var urls = new List<string> { uploadResult1.Url.ToString(), uploadResult2.Url.ToString() };
            var multiParams = new MultiParams(urls);
            var result = m_cloudinary.Multi(multiParams);
            AddCreatedPublicId(StorageType.multi, result.PublicId);
            AssertMultiResult(result, null, FILE_FORMAT_GIF);

            multiParams = new MultiParams(tag);
            result = m_cloudinary.Multi(multiParams);
            AddCreatedPublicId(StorageType.multi, result.PublicId);
            AssertMultiResult(result, null, FILE_FORMAT_GIF);

            multiParams.Transformation = m_resizeTransformation;
            result = m_cloudinary.Multi(multiParams);
            AddCreatedPublicId(StorageType.multi, result.PublicId);
            AssertMultiResult(result, TRANSFORM_W_512, null);

            multiParams.Transformation = m_simpleTransformationAngle;
            multiParams.Format = FILE_FORMAT_PDF;
            result = m_cloudinary.Multi(multiParams);
            AddCreatedPublicId(StorageType.multi, result.PublicId);
            AssertMultiResult(result, TRANSFORM_A_45, FILE_FORMAT_PDF);
        }

        [Test, RetryWithDelay]
        public async Task TestMultiTransformationAsync()
        {
            var tag = GetMethodTag();
            var uploadResult1 = await UploadTestImageResourceAsync((uploadParams) =>
            {
                uploadParams.Tags = $"{tag},{m_apiTag}";
            },
            StorageType.multi);
            var uploadResult2 = await UploadTestImageResourceAsync((uploadParams) =>
            {
                uploadParams.Tags = $"{tag},{m_apiTag}";
                uploadParams.Transformation = m_simpleTransformation;
            },
            StorageType.multi);

            var urls = new List<string> { uploadResult1.Url.ToString(), uploadResult2.Url.ToString() };
            var multiParams = new MultiParams(urls);
            var result = await m_cloudinary.MultiAsync(multiParams);
            AddCreatedPublicId(StorageType.multi, result.PublicId);
            AssertMultiResult(result, null, FILE_FORMAT_GIF);

            multiParams = new MultiParams(tag);
            result = await m_cloudinary.MultiAsync(multiParams);
            AddCreatedPublicId(StorageType.multi, result.PublicId);
            AssertMultiResult(result, null, FILE_FORMAT_GIF);

            multiParams.Transformation = m_resizeTransformation;
            result = await m_cloudinary.MultiAsync(multiParams);
            AddCreatedPublicId(StorageType.multi, result.PublicId);
            AssertMultiResult(result, TRANSFORM_W_512, null);

            multiParams.Transformation = m_simpleTransformationAngle;
            multiParams.Format = FILE_FORMAT_PDF;
            result = await m_cloudinary.MultiAsync(multiParams);
            AddCreatedPublicId(StorageType.multi, result.PublicId);
            AssertMultiResult(result, TRANSFORM_A_45, FILE_FORMAT_PDF);
        }

        [Test]
        public void TestDownloadMulti()
        {
            const string multiTestTag = "multi_test_tag";
            const string url1 = "https://res.cloudinary.com/demo/image/upload/sample";
            const string url2 = "https://res.cloudinary.com/demo/image/upload/car";

            var paramsFromTag = new MultiParams(multiTestTag);
            var urlFromTag = m_cloudinary.DownloadMulti(paramsFromTag);
            var paramsFromUrl = new MultiParams(new List<string> {url1, url2});
            var urlFromUrls = m_cloudinary.DownloadMulti(paramsFromUrl);

            var expectedUrl = "https://api.cloudinary.com/v1_1/" + m_cloudinary.Api.Account.Cloud + "/image/multi";
            var uriFromTag = new Uri(urlFromTag);
            var uriFromUrls = new Uri(urlFromUrls);
            Assert.True(uriFromTag.ToString().StartsWith(expectedUrl));
            Assert.True(uriFromUrls.ToString().StartsWith(expectedUrl));

            var uriParamsFromTag = HttpUtility.ParseQueryString(uriFromTag.Query);
            Assert.AreEqual("download", uriParamsFromTag["mode"]);
            Assert.AreEqual(multiTestTag, uriParamsFromTag["tag"]);
            Assert.NotNull(uriParamsFromTag["timestamp"]);
            Assert.NotNull(uriParamsFromTag["signature"]);

            var uriParamsFromUrls = HttpUtility.ParseQueryString(uriFromUrls.Query);
            Assert.AreEqual("download", uriParamsFromUrls["mode"]);
            Assert.True(uriParamsFromUrls["urls[]"].Contains(url1));
            Assert.True(uriParamsFromUrls["urls[]"].Contains(url2));
            Assert.NotNull(uriParamsFromUrls["timestamp"]);
            Assert.NotNull(uriParamsFromUrls["signature"]);
        }

        private void AssertMultiResult(MultiResult result, string transformation, string fileFormat)
        {
            if (!string.IsNullOrEmpty(transformation))
                Assert.True(result.Url.AbsoluteUri.Contains(transformation));

            if (!string.IsNullOrEmpty(fileFormat))
                Assert.True(result.Url.AbsoluteUri.EndsWith($".{fileFormat}"));
        }
    }
}
