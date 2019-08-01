using CloudinaryDotNet.Actions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CloudinaryDotNet.IntegrationTest.UploadApi
{
    public class MultiMethodsTest : IntegrationTestBase
    {
        [Test]
        public void TestMultiTransformation()
        {
            var publicId1 = GetUniquePublicId(StorageType.multi);
            var publicId2 = GetUniquePublicId(StorageType.multi);
            var tag = GetMethodTag();

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{tag},{m_apiTag}",
                PublicId = publicId1
            };

            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = publicId2;
            uploadParams.Transformation = m_simpleTransformation;
            m_cloudinary.Upload(uploadParams);

            MultiParams multi = new MultiParams(tag);
            MultiResult result = m_cloudinary.Multi(multi);
            AddCreatedPublicId(StorageType.multi, result.PublicId);
            Assert.True(result.Uri.AbsoluteUri.EndsWith($".{FILE_FORMAT_GIF}"));

            multi.Transformation = m_resizeTransformation;
            result = m_cloudinary.Multi(multi);
            AddCreatedPublicId(StorageType.multi, result.PublicId);
            Assert.IsTrue(result.Uri.AbsoluteUri.Contains(TRANSFORM_W_512));

            multi.Transformation = m_simpleTransformationAngle;
            multi.Format = FILE_FORMAT_PDF;
            result = m_cloudinary.Multi(multi);
            AddCreatedPublicId(StorageType.multi, result.PublicId);

            Assert.True(result.Uri.AbsoluteUri.Contains(TRANSFORM_A_45));
            Assert.True(result.Uri.AbsoluteUri.EndsWith($".{FILE_FORMAT_PDF}"));
        }

        [Test]
        public async Task TestMultiTransformationAsync()
        {
            var publicId1 = GetUniqueAsyncPublicId(StorageType.multi);
            var publicId2 = GetUniqueAsyncPublicId(StorageType.multi);
            var tag = GetMethodTag();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{tag},{m_apiTag}",
                PublicId = publicId1
            };

            await m_cloudinary.UploadAsync(uploadParams);

            uploadParams.PublicId = publicId2;
            uploadParams.Transformation = m_simpleTransformation;
            await m_cloudinary.UploadAsync(uploadParams);

            var multi = new MultiParams(tag);
            var result = await m_cloudinary.MultiAsync(multi);
            AddCreatedPublicId(StorageType.multi, result.PublicId);

            Assert.True(result.Uri.AbsoluteUri.EndsWith($".{FILE_FORMAT_GIF}"));

            multi.Transformation = m_resizeTransformation;
            result = await m_cloudinary.MultiAsync(multi);
            AddCreatedPublicId(StorageType.multi, result.PublicId);

            Assert.IsTrue(result.Uri.AbsoluteUri.Contains(TRANSFORM_W_512));

            multi.Transformation = m_simpleTransformationAngle;
            multi.Format = FILE_FORMAT_PDF;
            result = await m_cloudinary.MultiAsync(multi);
            AddCreatedPublicId(StorageType.multi, result.PublicId);

            Assert.True(result.Uri.AbsoluteUri.Contains(TRANSFORM_A_45));
            Assert.True(result.Uri.AbsoluteUri.EndsWith($".{FILE_FORMAT_PDF}"));
        }
    }
}
