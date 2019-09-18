using CloudinaryDotNet.Actions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CloudinaryDotNet.IntegrationTest.UploadApi
{
    public class ExplodeMethodsTest : IntegrationTestBase
    {
        protected readonly Transformation m_transformationExplode = new Transformation().Page("all");

        public override void Initialize()
        {
            base.Initialize();
            AddCreatedTransformation(m_transformationExplode);
        }

        [Test]
        public void TestExplode()
        {
            var uploadResult = UploadTestImageResource((uploadParams) =>
            {
                uploadParams.File = new FileDescription(m_testPdfPath);
            });

            var explodeParams = CreateExplodeParams(uploadResult.PublicId, m_transformationExplode);

            var result = m_cloudinary.Explode(explodeParams);

            CheckExplodeStatus(result);
        }

        [Test]
        public async Task TestExplodeAsync()
        {
            var uploadResult = await UploadTestImageResourceAsync((uploadParams) =>
            {
                uploadParams.File = new FileDescription(m_testPdfPath);
            });

            var explodeParams = CreateExplodeParams(uploadResult.PublicId, m_transformationExplode);

            var result = await m_cloudinary.ExplodeAsync(explodeParams);

            CheckExplodeStatus(result);
        }

        private ExplodeParams CreateExplodeParams(string publicId, Transformation transformation)
        {
            return new ExplodeParams(publicId, transformation);
        }

        private void CheckExplodeStatus(ExplodeResult result)
        {
            Assert.AreEqual("processing", result.Status);
        }
    }
}
