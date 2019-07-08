using CloudinaryDotNet.Actions;
using NUnit.Framework;

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
            var publicId = GetUniquePublicId();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testPdfPath),
                PublicId = publicId,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            var explodeParams = new ExplodeParams(publicId, m_transformationExplode);
            var result = m_cloudinary.Explode(explodeParams);

            Assert.AreEqual("processing", result.Status);
        }
    }
}
