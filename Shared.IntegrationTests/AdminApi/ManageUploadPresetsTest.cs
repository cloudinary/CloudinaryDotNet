using System.Collections.Generic;
using System.Net;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.AdminApi
{
    public class ManageUploadPresetsTest : IntegrationTestBase
    {
        [Test]
        public void TestListUploadPresets()
        {
            var deleted = "deleted";
            var testUploadPreset1 = GetUniquePresetName();
            var testUploadPreset2 = GetUniquePresetName();

            // should allow creating and listing upload_presets
            var preset = new UploadPresetParams()
            {
                Name = testUploadPreset1,
                Folder = "folder",
                DisallowPublicId = true,
                Unsigned = true,
                Tags = m_apiTag,
                AllowedFormats = new string[] { FILE_FORMAT_JPG, FILE_FORMAT_BMP }
            };

            var result = m_cloudinary.CreateUploadPreset(preset);

            preset = new UploadPresetParams()
            {
                Name = testUploadPreset2,
                Folder = "folder2",
                Tags = $"{m_apiTag},a,b,c",
                Context = new StringDictionary("a=b", "c=d"),
                Transformation = m_simpleTransformation,
                EagerTransforms = new List<object>() { m_resizeTransformation },
                FaceCoordinates = "1,2,3,4"
            };

            result = m_cloudinary.CreateUploadPreset(preset);

            var presets = m_cloudinary.ListUploadPresets();

            Assert.AreEqual(presets.Presets[0].Name, testUploadPreset2);
            Assert.AreEqual(presets.Presets[1].Name, testUploadPreset1);

            var delResult = m_cloudinary.DeleteUploadPreset(testUploadPreset1);
            Assert.AreEqual(deleted, delResult.Message);
            delResult = m_cloudinary.DeleteUploadPreset(testUploadPreset2);
            Assert.AreEqual(deleted, delResult.Message);
        }

        [Test]
        public void TestGetUploadPreset()
        {
            // should allow getting a single upload_preset
            var folder = "folder";

            var @params = new UploadPresetParams()
            {
                Tags = $"a,b,c,{m_apiTag}",
                Name = GetUniquePresetName(),
                Context = new StringDictionary("a=b", "c=d"),
                Transformation = m_simpleTransformation,
                EagerTransforms = new List<object>() { m_resizeTransformation },
                FaceCoordinates = "1,2,3,4",
                Unsigned = true,
                QualityAnalysis = true,
                Folder = folder,
                AllowedFormats = new[] { FILE_FORMAT_JPG, FILE_FORMAT_PDF }
            };

            var creationResult = m_cloudinary.CreateUploadPreset(@params);

            var preset = m_cloudinary.GetUploadPreset(creationResult.Name);

            Assert.AreEqual(creationResult.Name, preset.Name);
            Assert.AreEqual(true, preset.Unsigned);
            Assert.IsTrue(preset.Settings.QualityAnalysis);
            Assert.AreEqual(folder, preset.Settings.Folder);
            Assert.AreEqual("0.5", preset.Settings.Transformation[0]["width"].ToString());
            Assert.AreEqual("scale", preset.Settings.Transformation[0]["crop"].ToString());
        }

        [Test]
        public void TestDeleteUploadPreset()
        {
            // should allow deleting upload_presets
            var preset = GetUniquePresetName();

            m_cloudinary.CreateUploadPreset(new UploadPresetParams()
            {
                Name = preset,
                Folder = "folder"
            });

            var result = m_cloudinary.DeleteUploadPreset(preset);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            result = m_cloudinary.DeleteUploadPreset(preset);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public void TestUpdateUploadPreset()
        {
            // should allow updating upload presets

            var presetToCreate = new UploadPresetParams()
            {
                Folder = "folder",
                Name = GetUniquePresetName(),
                Context = new StringDictionary("a=b", "b=c"),
                Transformation = m_simpleTransformation,
                EagerTransforms = new List<object>() { m_resizeTransformation, m_updateTransformation },
                AllowedFormats = new string[] { FILE_FORMAT_JPG, FILE_FORMAT_PNG },
                Tags = $"a,b,c,{m_apiTag}",
                FaceCoordinates = "1,2,3,4",
                Live = false
            };

            var presetName = m_cloudinary.CreateUploadPreset(presetToCreate).Name;

            var preset = m_cloudinary.GetUploadPreset(presetName);
            Assert.IsFalse(preset.Settings.Live);

            var presetToUpdate = new UploadPresetParams(preset)
            {
                Colors = true,
                Unsigned = true,
                DisallowPublicId = true,
                QualityAnalysis = true,
                Live = true
            };

            var result = m_cloudinary.UpdateUploadPreset(presetToUpdate);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("updated", result.Message);

            preset = m_cloudinary.GetUploadPreset(presetName);

            Assert.AreEqual(presetName, preset.Name);
            Assert.IsTrue(preset.Unsigned);
            Assert.IsTrue(preset.Settings.QualityAnalysis);
            Assert.IsTrue(preset.Settings.Live);

            // TODO: compare settings of preset and presetToUpdate
        }

        [Test]
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
