using CloudinaryDotNet.Actions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CloudinaryDotNet.IntegrationTest.UploadApi
{
    public class SpriteMethodsTest : IntegrationTestBase
    {
        [Test]
        public void TestSprite()
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
                Transformation = m_resizeTransformation
            };
            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = publicId2;
            uploadParams.Transformation = m_updateTransformation;
            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = publicId3;
            uploadParams.Transformation = m_simpleTransformation;
            m_cloudinary.Upload(uploadParams);

            SpriteParams sprite = new SpriteParams(spriteTag)
            {
                Format = FILE_FORMAT_JPG
            };

            SpriteResult result = m_cloudinary.MakeSprite(sprite);
            AddCreatedPublicId(StorageType.sprite, result.PublicId);

            Assert.NotNull(result);
            Assert.NotNull(result.ImageInfos);
            Assert.AreEqual(3, result.ImageInfos.Count);

            StringAssert.EndsWith(FILE_FORMAT_JPG, result.ImageUri.ToString());

            Assert.Contains(publicId1, result.ImageInfos.Keys);
            Assert.Contains(publicId2, result.ImageInfos.Keys);
            Assert.Contains(publicId3, result.ImageInfos.Keys);
        }

        [Test]
        public async Task TestSpriteAsync()
        {
            var publicId1 = GetUniqueAsyncPublicId(StorageType.sprite);
            var publicId2 = GetUniqueAsyncPublicId(StorageType.sprite);
            var publicId3 = GetUniqueAsyncPublicId(StorageType.sprite);

            var spriteTag = GetMethodTag();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{spriteTag},{m_apiTag}",
                PublicId = publicId1,
                Transformation = m_resizeTransformation
            };
            await m_cloudinary.UploadAsync(uploadParams);

            uploadParams.PublicId = publicId2;
            uploadParams.Transformation = m_updateTransformation;
            await m_cloudinary.UploadAsync(uploadParams);

            uploadParams.PublicId = publicId3;
            uploadParams.Transformation = m_simpleTransformation;
            await m_cloudinary.UploadAsync(uploadParams);

            var sprite = new SpriteParams(spriteTag)
            {
                Format = FILE_FORMAT_JPG
            };

            SpriteResult result = await m_cloudinary.MakeSpriteAsync(sprite);
            AddCreatedPublicId(StorageType.sprite, result.PublicId);

            Assert.NotNull(result);
            Assert.NotNull(result.ImageInfos);
            Assert.AreEqual(3, result.ImageInfos.Count);

            StringAssert.EndsWith(FILE_FORMAT_JPG, result.ImageUri.ToString());

            Assert.Contains(publicId1, result.ImageInfos.Keys);
            Assert.Contains(publicId2, result.ImageInfos.Keys);
            Assert.Contains(publicId3, result.ImageInfos.Keys);
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
