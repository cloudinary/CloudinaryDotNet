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
            var tag = GetMethodTag();

            UploadTestImageResource((uploadParams) =>
            {
                uploadParams.Tags = $"{tag},{m_apiTag}";
            },
            StorageType.multi);

            UploadTestImageResource((uploadParams) =>
            {
                uploadParams.Tags = $"{tag},{m_apiTag}";
                uploadParams.Transformation = m_simpleTransformation;
            },
            StorageType.multi);

            var multiParams = new MultiParams(tag);

            var result = m_cloudinary.Multi(multiParams);

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

        [Test]
        public async Task TestMultiTransformationAsync()
        {
            var tag = GetMethodTag();

            await UploadTestImageResourceAsync((uploadParams) =>
            {
                uploadParams.Tags = $"{tag},{m_apiTag}";
            },
            StorageType.multi);

            await UploadTestImageResourceAsync((uploadParams) =>
            {
                uploadParams.Tags = $"{tag},{m_apiTag}";
                uploadParams.Transformation = m_simpleTransformation;
            },
            StorageType.multi);

            var multiParams = new MultiParams(tag);

            var result = await m_cloudinary.MultiAsync(multiParams);

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

        private void AssertMultiResult(MultiResult result, string transformation, string fileFormat)
        {
            if (!string.IsNullOrEmpty(transformation))
                Assert.True(result.Url.AbsoluteUri.Contains(transformation));

            if (!string.IsNullOrEmpty(fileFormat))
                Assert.True(result.Url.AbsoluteUri.EndsWith($".{fileFormat}"));
        }
    }
}
