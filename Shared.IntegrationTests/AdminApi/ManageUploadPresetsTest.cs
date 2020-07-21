using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.AdminApi
{
    public class ManageUploadPresetsTest : IntegrationTestBase
    {
        private const string presetFolder = "upload_folder";
        [Test, RetryWithDelay]

        public void TestUnsignedUpload()
        {
            // should support unsigned uploading using presets
            var uploadResult = PresetAndGetImageUploadResul();

            Assert.NotNull(uploadResult.PublicId);
            Assert.True(uploadResult.PublicId.StartsWith(presetFolder));
        }

        [Test]
        public void TestUploadPresetAccessibilityAnalysis()
        {
            // should support unsigned uploading using presets
            var uploadResult = PresetAndGetImageUploadResul(true);

            CloudinaryAssert.AccessibilityAnalysisNotEmpty(uploadResult.AccessibilityAnalysis);
        }

        private ImageUploadResult PresetAndGetImageUploadResul(bool accessibilityAnalysis = false)
        {
            var presetParams = new UploadPresetParams()
            {
                Name = GetUniquePresetName(),
                Folder = presetFolder,
                Unsigned = true,
                Tags = m_apiTag
            };

            if (accessibilityAnalysis)
            {
                presetParams.AccessibilityAnalysis = true;
            }

            var preset = m_cloudinary.CreateUploadPreset(presetParams);

            var acc = new Account(m_cloudName);
            var cloudinary = new Cloudinary(acc);

            return cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                UploadPreset = preset.Name,
                Unsigned = true,
                Tags = m_apiTag
            });
        }
    }
}
