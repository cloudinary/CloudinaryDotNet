using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.UploadApi
{
    public class TagMethodsTest : IntegrationTestBase
    {
        [Test]
        public void TestTagAdd()
        {
            var uploadResult = UploadTestImageResource();

            var tagParams = GetAddTagParams(uploadResult.PublicId, GetMethodTag());

            var tagResult = m_cloudinary.Tag(tagParams);

            CheckTagParamsAdd(tagResult, uploadResult.PublicId);
        }

        [Test]
        public async Task TestTagAddAsync()
        {
            var uploadResult = await UploadTestImageResourceAsync();

            var tagParams = GetAddTagParams(uploadResult.PublicId, GetMethodTag());

            var tagResult = await m_cloudinary.TagAsync(tagParams);

            CheckTagParamsAdd(tagResult, uploadResult.PublicId);
        }

        private void CheckTagParamsAdd(TagResult result, string publicId)
        {
            Assert.AreEqual(1, result.PublicIds.Length);
            Assert.AreEqual(publicId, result.PublicIds[0]);
        }

        private TagParams GetAddTagParams(string publicId, string tag)
        {
            var tagParams = new TagParams()
            {
                Command = TagCommand.Add,
                Tag = tag
            };

            tagParams.PublicIds.Add(publicId);

            return tagParams;
        }

        /// <summary>
        /// Test that we can add a tag for a video resource
        /// </summary>
        [Test]
        public void TestVideoTagAdd()
        {
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(m_testVideoPath)
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);
            var tagParams = new TagParams()
            {
                Command = TagCommand.Add,
                Tag = m_apiTag,
                ResourceType = ResourceType.Video
            };

            tagParams.PublicIds.Add(uploadResult.PublicId);

            var tagResult = m_cloudinary.Tag(tagParams);

            Assert.AreEqual(1, tagResult.PublicIds.Length);
            Assert.AreEqual(uploadResult.PublicId, tagResult.PublicIds[0]);
        }

        [Test]
        public void TestTagMultiple()
        {
            var methodTag = GetMethodTag();

            var testTag1 = $"{methodTag}_1";
            var testTag2 = $"{methodTag}_2";
            var testTag3 = $"{methodTag}_3";

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            };

            var uploadResult1 = m_cloudinary.Upload(uploadParams);
            var uploadResult2 = m_cloudinary.Upload(uploadParams);

            var tagParams = new TagParams()
            {
                PublicIds = new List<string>() {
                    uploadResult1.PublicId,
                    uploadResult2.PublicId
                },
                Tag = testTag1
            };

            m_cloudinary.Tag(tagParams);

            // remove second ID
            tagParams.PublicIds.RemoveAt(1);
            tagParams.Tag = testTag2;

            m_cloudinary.Tag(tagParams);

            var r = m_cloudinary.GetResource(uploadResult1.PublicId);
            Assert.NotNull(r.Tags);
            Assert.Contains(testTag1, r.Tags);
            Assert.Contains(testTag2, r.Tags);

            r = m_cloudinary.GetResource(uploadResult2.PublicId);
            Assert.NotNull(r.Tags);
            Assert.Contains(testTag1, r.Tags);

            tagParams.Command = TagCommand.Remove;
            tagParams.Tag = testTag1;
            tagParams.PublicIds = new List<string>() { uploadResult1.PublicId };

            m_cloudinary.Tag(tagParams);

            r = m_cloudinary.GetResource(uploadResult1.PublicId);
            Assert.NotNull(r.Tags);
            Assert.Contains(testTag2, r.Tags);

            tagParams.Command = TagCommand.Replace;
            tagParams.Tag = $"{m_apiTag},{testTag3}";

            m_cloudinary.Tag(tagParams);

            r = m_cloudinary.GetResource(uploadResult1.PublicId);
            Assert.NotNull(r.Tags);
            Assert.True(r.Tags.SequenceEqual(new string[] { m_apiTag, testTag3 }));
        }

        [Test]
        public void TestTagReplace()
        {
            var tag = GetMethodTag();
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            };

            ImageUploadResult uploadResult = m_cloudinary.Upload(uploadParams);

            TagParams tagParams = new TagParams()
            {
                Command = TagCommand.Replace,
                Tag = $"{tag},{m_apiTag}"
            };

            tagParams.PublicIds.Add(uploadResult.PublicId);

            TagResult tagResult = m_cloudinary.Tag(tagParams);

            Assert.AreEqual(1, tagResult.PublicIds.Length);
            Assert.AreEqual(uploadResult.PublicId, tagResult.PublicIds[0]);
        }

        [Test]
        public void TestClearAllTags()
        {
            var publicId = GetUniquePublicId();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = "Tag1, Tag2, Tag3",
                PublicId = publicId,
                Overwrite = true,
                Type = STORAGE_TYPE_UPLOAD
            };

            m_cloudinary.Upload(uploadParams);

            List<string> pIds = new List<string>();
            pIds.Add(publicId);

            m_cloudinary.Tag(new TagParams()
            {
                Command = TagCommand.RemoveAll,
                PublicIds = pIds,
                Type = STORAGE_TYPE_UPLOAD,

            });

            var getResResult = m_cloudinary.GetResource(new GetResourceParams(pIds[0])
            {
                PublicId = pIds[0],
                Type = STORAGE_TYPE_UPLOAD,
                ResourceType = ResourceType.Image
            });

            Assert.Null(getResResult.Tags);
        }
    }
}
