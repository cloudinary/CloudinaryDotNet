using CloudinaryDotNet.Actions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CloudinaryDotNet.IntegrationTest.UploadApi
{
    public class SpriteMethodsTest : IntegrationTestBase
    {
        [Test]
        public void TestSprite()
        {
            var spriteTag = GetMethodTag();

            var testTransformations = new[]{ m_resizeTransformation, m_updateTransformation, m_simpleTransformation };

            var addedPublicIds = testTransformations.Select(t =>
            {
                var uploadResult = UploadTestImageResource((uploadParams) =>
                {
                    uploadParams.Tags = $"{spriteTag},{m_apiTag}";
                    uploadParams.Transformation = t;
                },
                StorageType.sprite);

                return uploadResult.PublicId;
            }).ToList();

            var spriteParams = CreateSpriteParams(spriteTag, FILE_FORMAT_JPG);

            var result = m_cloudinary.MakeSprite(spriteParams);

            AddCreatedPublicId(StorageType.sprite, result.PublicId);

            AssertSprite(result, addedPublicIds, FILE_FORMAT_JPG);
        }

        [Test]
        public async Task TestSpriteAsync()
        {
            var spriteTag = GetMethodTag();

            var testTransformations = new[] { m_resizeTransformation, m_updateTransformation, m_simpleTransformation };

            var addedPublicIdsTasks = testTransformations.Select(async t =>
            {
                var uploadResult = await UploadTestImageResourceAsync((uploadParams) =>
                {
                    uploadParams.Tags = $"{spriteTag},{m_apiTag}";
                    uploadParams.Transformation = t;
                },
                StorageType.sprite);

                return uploadResult.PublicId;
            });

            var addedPublicIdsTask = await Task.WhenAll(addedPublicIdsTasks);

            var spriteParams = CreateSpriteParams(spriteTag, FILE_FORMAT_JPG);

            var result = await m_cloudinary.MakeSpriteAsync(spriteParams);

            AddCreatedPublicId(StorageType.sprite, result.PublicId);

            AssertSprite(result, addedPublicIdsTask, FILE_FORMAT_JPG);
        }

        private SpriteParams CreateSpriteParams(string tag, string fileFormat)
        {
            return new SpriteParams(tag)
            {
                Format = fileFormat
            };
        }

        private void AssertSprite(SpriteResult result, IEnumerable<string> publicIds, string fileFormat)
        {
            Assert.NotNull(result?.ImageInfos);
            Assert.NotNull(result.ImageUrl);
            StringAssert.EndsWith(fileFormat, result.ImageUrl.ToString());
            CollectionAssert.AreEqual(publicIds, result.ImageInfos.Keys);
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

        [Test]
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
            Assert.NotNull(result.ImageInfos);
            foreach (var item in result.ImageInfos)
            {
                Assert.AreEqual(m_resizeTransformationWidth, item.Value.Width);
                Assert.AreEqual(m_resizeTransformationHeight, item.Value.Height);
            }
        }
    }
}
