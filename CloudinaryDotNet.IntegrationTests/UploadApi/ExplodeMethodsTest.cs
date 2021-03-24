using CloudinaryDotNet.Actions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CloudinaryDotNet.IntegrationTests.UploadApi
{
    public class ExplodeMethodsTest : IntegrationTestBase
    {
        protected readonly Transformation m_transformationExplode = new Transformation().Page("all");

        public override void Initialize()
        {
            base.Initialize();
            AddCreatedTransformation(m_transformationExplode);
        }

        [Test, RetryWithDelay]
        public void TestExplode()
        {
            var uploadResult = UploadTestImageResource((uploadParams) =>
            {
                uploadParams.File = new FileDescription(m_testPdfPath);
            });

            var explodeParams = CreateExplodeParams(uploadResult.PublicId, m_transformationExplode);

            var result = m_cloudinary.Explode(explodeParams);

            AssertExplodeStatus(result);
        }

        [Test, RetryWithDelay]
        public async Task TestExplodeAsync()
        {
            var uploadResult = await UploadTestImageResourceAsync((uploadParams) =>
            {
                uploadParams.File = new FileDescription(m_testPdfPath);
            });

            var explodeParams = CreateExplodeParams(uploadResult.PublicId, m_transformationExplode);

            var result = await m_cloudinary.ExplodeAsync(explodeParams);

            AssertExplodeStatus(result);
        }

        [Test, RetryWithDelay]
        public async Task TestExplodeTypeParamAsync()
        {
            var uploadResult = await UploadTestImageResourceAsync((uploadParams) =>
            {
                uploadParams.File = new FileDescription(m_testPdfPath);
            });

            var explodeParams = CreateExplodeParams(uploadResult.PublicId, m_transformationExplode);
            explodeParams.Type = AssetType.Upload;

            var result = await m_cloudinary.ExplodeAsync(explodeParams);

            AssertExplodeStatus(result);
        }

        private ExplodeParams CreateExplodeParams(string publicId, Transformation transformation)
        {
            return new ExplodeParams(publicId, transformation);
        }

        private void AssertExplodeStatus(ExplodeResult result)
        {
            Assert.AreEqual("processing", result.Status);
        }
    }
}
