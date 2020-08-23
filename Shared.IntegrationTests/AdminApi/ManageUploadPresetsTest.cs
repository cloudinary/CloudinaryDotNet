using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.AdminApi
{
    public class ManageUploadPresetsTest : IntegrationTestBase
    {
        [Test, RetryWithDelay]
        public void TestUnsignedUpload()
        {
            // should support unsigned uploading using presets
            var folder = "upload_folder";
            var preset = m_cloudinary.CreateUploadPreset(new UploadPresetParams()
            {
                Name = GetUniquePresetName(),
                Folder = folder,
                Unsigned = true,
                Tags = m_apiTag
            });

            var acc = new Account(m_cloudName);
            var cloudinary = new Cloudinary(acc);

            var upload = cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                UploadPreset = preset.Name,
                Unsigned = true,
                Tags = m_apiTag
            });

            Assert.NotNull(upload.PublicId);
            Assert.True(upload.PublicId.StartsWith(folder));
        }
    }
}
