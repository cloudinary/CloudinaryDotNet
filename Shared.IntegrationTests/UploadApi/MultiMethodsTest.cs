using CloudinaryDotNet.Actions;
using NUnit.Framework;

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
    }
}
