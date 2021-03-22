using System;
using System.Collections.Generic;
using CloudinaryDotNet.Actions;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using System.Web;

namespace CloudinaryDotNet.IntegrationTests.UploadApi
{
    public class SpriteMethodsTest : IntegrationTestBase
    {
        [Test, RetryWithDelay]
        public void TestSprite()
        {
            var spriteTag = GetMethodTag();
            var testTransformations = new[]{ m_resizeTransformation, m_updateTransformation, m_simpleTransformation };
            var uploadResults = testTransformations.Select(t =>
                UploadTestImageResource((uploadParams) =>
                {
                    uploadParams.Tags = $"{spriteTag},{m_apiTag}";
                    uploadParams.Transformation = t;
                },
                StorageType.sprite)
            ).ToList();
            var addedPublicIds = uploadResults.Select(uploadResult => uploadResult.PublicId).ToList();

            var spriteParams = new SpriteParams(spriteTag)
            {
                Format = FILE_FORMAT_JPG
            };
            var result = m_cloudinary.MakeSprite(spriteParams);
            AddCreatedPublicId(StorageType.sprite, result.PublicId);
            AssertSprite(result, FILE_FORMAT_JPG);
            CollectionAssert.AreEqual(addedPublicIds, result.ImageInfos.Keys, result.Error?.Message);

            var urls = uploadResults.Select(uploadResult => uploadResult.Url.ToString()).ToList();
            spriteParams = new SpriteParams(urls)
            {
                Format = FILE_FORMAT_JPG
            };
            result = m_cloudinary.MakeSprite(spriteParams);
            AddCreatedPublicId(StorageType.sprite, result.PublicId);
            AssertSprite(result, FILE_FORMAT_JPG);
            Assert.AreEqual(addedPublicIds.Count, result.ImageInfos.Keys.Count, result.Error?.Message);
        }

        [Test, RetryWithDelay]
        public async Task TestSpriteAsync()
        {
            var spriteTag = GetMethodTag();
            var testTransformations = new[] { m_resizeTransformation, m_updateTransformation, m_simpleTransformation };
            var uploadTasks = testTransformations.Select(async t =>
            {
                var uploadResult = await UploadTestImageResourceAsync((uploadParams) =>
                {
                    uploadParams.Tags = $"{spriteTag},{m_apiTag}";
                    uploadParams.Transformation = t;
                },
                StorageType.sprite);
                return uploadResult;
            });
            var uploadResults = await Task.WhenAll(uploadTasks);
            var addedPublicIds = uploadResults.Select(uploadResult => uploadResult.PublicId).ToList();

            var spriteParams = new SpriteParams(spriteTag)
            {
                Format = FILE_FORMAT_JPG
            };
            var result = await m_cloudinary.MakeSpriteAsync(spriteParams);
            AddCreatedPublicId(StorageType.sprite, result.PublicId);
            AssertSprite(result, FILE_FORMAT_JPG);
            CollectionAssert.AreEqual(addedPublicIds, result.ImageInfos.Keys, result.Error?.Message);

            var urls = uploadResults.Select(uploadResult => uploadResult.Url.ToString()).ToList();
            spriteParams = new SpriteParams(urls)
            {
                Format = FILE_FORMAT_JPG
            };
            result = await m_cloudinary.MakeSpriteAsync(spriteParams);
            AddCreatedPublicId(StorageType.sprite, result.PublicId);
            AssertSprite(result, FILE_FORMAT_JPG);
            Assert.AreEqual(addedPublicIds.Count, result.ImageInfos.Keys.Count, result.Error?.Message);
        }

        private void AssertSprite(SpriteResult result, string fileFormat)
        {
            Assert.NotNull(result?.ImageInfos, result?.Error?.Message);
            Assert.NotNull(result.ImageUrl);
            StringAssert.EndsWith(fileFormat, result.ImageUrl.ToString());
            Assert.AreEqual(result.ImageUrl, result.ImageUrl);
            Assert.NotNull(result.CssUrl);
            Assert.AreEqual(result.CssUrl, result.CssUrl);
            Assert.NotNull(result.JsonUrl);
            Assert.AreEqual(result.JsonUrl, result.JsonUrl);
            Assert.NotNull(result.SecureCssUrl);
            Assert.AreEqual(result.SecureCssUrl, result.SecureCssUrl);
            Assert.NotNull(result.SecureImageUrl);
            Assert.NotNull(result.SecureJsonUrl);
        }

        [Test, RetryWithDelay]
        public void TestSpriteTransformation()
        {
            var publicId1 = GetUniquePublicId(StorageType.sprite);
            var publicId2 = GetUniquePublicId(StorageType.sprite);
            var publicId3 = GetUniquePublicId(StorageType.sprite);

            var spriteTag = GetMethodTag();

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{spriteTag},{m_apiTag}",
                PublicId = publicId1,
                Transformation = m_simpleTransformation
            };
            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = publicId2;
            uploadParams.Transformation = m_updateTransformation;
            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = publicId3;
            uploadParams.Transformation = m_explicitTransformation;
            m_cloudinary.Upload(uploadParams);

            SpriteParams sprite = new SpriteParams(spriteTag)
            {
                Transformation = m_resizeTransformation
            };

            SpriteResult result = m_cloudinary.MakeSprite(sprite);
            AddCreatedPublicId(StorageType.sprite, result.PublicId);

            Assert.NotNull(result);
            Assert.NotNull(result.ImageInfos, result.Error?.Message);
            foreach (var item in result.ImageInfos)
            {
                Assert.AreEqual(m_resizeTransformationWidth, item.Value.Width);
                Assert.AreEqual(m_resizeTransformationHeight, item.Value.Height);
            }
        }

        [Test]
        public void TestDownloadSprite()
        {
            const string spriteTestTag = "sprite_test_tag";
            const string url1 = "https://res.cloudinary.com/demo/image/upload/sample";
            const string url2 = "https://res.cloudinary.com/demo/image/upload/car";

            var paramsFromTag = new SpriteParams(spriteTestTag);
            var urlFromTag = m_cloudinary.DownloadSprite(paramsFromTag);
            var paramsFromUrl = new SpriteParams(new List<string> { url1, url2 });
            var urlFromUrls = m_cloudinary.DownloadSprite(paramsFromUrl);

            var expectedUrl = "https://api.cloudinary.com/v1_1/" + m_cloudinary.Api.Account.Cloud + "/image/sprite";
            var uriFromTag = new Uri(urlFromTag);
            var uriFromUrls = new Uri(urlFromUrls);
            Assert.True(uriFromTag.ToString().StartsWith(expectedUrl));
            Assert.True(uriFromUrls.ToString().StartsWith(expectedUrl));

            var uriParamsFromTag = HttpUtility.ParseQueryString(uriFromTag.Query);
            Assert.AreEqual("download", uriParamsFromTag["mode"]);
            Assert.AreEqual(spriteTestTag, uriParamsFromTag["tag"]);
            Assert.NotNull(uriParamsFromTag["timestamp"]);
            Assert.NotNull(uriParamsFromTag["signature"]);

            var uriParamsFromUrls = HttpUtility.ParseQueryString(uriFromUrls.Query);
            Assert.AreEqual("download", uriParamsFromUrls["mode"]);
            Assert.True(uriParamsFromUrls["urls[]"].Contains(url1));
            Assert.True(uriParamsFromUrls["urls[]"].Contains(url2));
            Assert.NotNull(uriParamsFromUrls["timestamp"]);
            Assert.NotNull(uriParamsFromUrls["signature"]);
        }
    }
}
