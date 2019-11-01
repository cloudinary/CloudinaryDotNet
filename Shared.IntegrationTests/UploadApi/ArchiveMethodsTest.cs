using System;
using System.Collections.Generic;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.UploadApi
{
    public class ArchiveMethodsTest : IntegrationTestBase
    {
        [Test]
        public void TestCreateArchive()
        {
            var targetPublicId = GetUniquePublicId();
            var archiveTag = GetMethodTag();

            var res = UploadImageForTestArchive(archiveTag, 2.0, true);

            var parameters = new ArchiveParams()
                                            .Tags(new List<string> { archiveTag, "no_such_tag" })
                                            .TargetPublicId(targetPublicId)
                                            .TargetTags(new List<string> { m_apiTag });
            var result = m_cloudinary.CreateArchive(parameters);
            Assert.AreEqual($"{targetPublicId}.{FILE_FORMAT_ZIP}", result.PublicId);
            Assert.AreEqual(1, result.FileCount);

            var res2 = UploadImageForTestArchive(archiveTag, 500, false);

            var transformations = new List<Transformation> { m_simpleTransformation, m_updateTransformation };
            parameters = new ArchiveParams().PublicIds(new List<string> { res.PublicId, res2.PublicId })
                                            .Transformations(transformations)
                                            .FlattenFolders(true)
                                            .SkipTransformationName(true)
                                            .UseOriginalFilename(true)
                                            .Tags(new List<string> { archiveTag })
                                            .TargetTags(new List<string> { m_apiTag });
            result = m_cloudinary.CreateArchive(parameters);
            Assert.AreEqual(2, result.FileCount);
        }

        [Test]
        public void TestCreateArchiveRawResources()
        {
            var raw = Api.GetCloudinaryParam(ResourceType.Raw);
            var tag = GetMethodTag();

            var uploadParams = new RawUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Folder = "test_folder",
                Type = STORAGE_TYPE_PRIVATE,
                Tags = $"{tag},{m_apiTag}"
            };
            var uploadResult1 = m_cloudinary.Upload(uploadParams, raw);

            uploadParams.File = new FileDescription(m_testPdfPath);
            var uploadResult2 = m_cloudinary.Upload(uploadParams, raw);

            var parameters = new ArchiveParams()
                                            .PublicIds(new List<string> { uploadResult1.PublicId, uploadResult2.PublicId })
                                            .ResourceType(raw)
                                            .Type(STORAGE_TYPE_PRIVATE)
                                            .UseOriginalFilename(true)
                                            .TargetTags(new List<string> { m_apiTag });
            var result = m_cloudinary.CreateArchive(parameters);
            Assert.AreEqual(2, result.FileCount);
        }

        [Test]
        public void TestCreateArchiveMultiplePublicIds()
        {
            // should support archiving based on multiple public IDs
            var parameters = UploadImageForArchiveAndPrepareParameters(GetMethodTag());
            var result = m_cloudinary.CreateArchive(parameters);

            Assert.AreEqual($"{parameters.TargetPublicId()}.{FILE_FORMAT_ZIP}", result.PublicId);
            Assert.AreEqual(1, result.FileCount);
        }

        [Test]
        public void TestCreateArchiveMultipleResourceTypes()
        {
            var raw = ApiShared.GetCloudinaryParam(ResourceType.Raw);

            var tag = GetMethodTag();

            var rawUploadParams = new RawUploadParams()
            {
                File = new FileDescription(m_testPdfPath),
                Tags = $"{tag},{m_apiTag}"
            };

            var upRes1 = m_cloudinary.Upload(rawUploadParams, raw);

            var imageUploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{tag},{m_apiTag}"
            };

            var upRes2 = m_cloudinary.Upload(imageUploadParams);

            var videoUploadParams = new VideoUploadParams()
            {
                File = new FileDescription(m_testVideoPath),
                Tags = $"{tag},{m_apiTag}"
            };

            var upRes3 = m_cloudinary.Upload(videoUploadParams);

            var fQPublicIds = new List<string>
            {
                upRes1.FullyQualifiedPublicId,
                upRes2.FullyQualifiedPublicId,
                upRes3.FullyQualifiedPublicId
            };

            var parameters = new ArchiveParams()
                .UseOriginalFilename(true)
                .TargetTags(new List<string> { tag, m_apiTag });

            var ex = Assert.Throws<ArgumentException>(() => m_cloudinary.CreateArchive(parameters));

            StringAssert.StartsWith("At least one of the following", ex.Message);

            parameters.ResourceType("auto").Tags(new List<string> { "tag" });

            ex = Assert.Throws<ArgumentException>(() => m_cloudinary.CreateArchive(parameters));

            StringAssert.StartsWith("To create an archive with multiple types of assets", ex.Message);

            parameters.ResourceType("").Tags(null).FullyQualifiedPublicIds(fQPublicIds);

            ex = Assert.Throws<ArgumentException>(() => m_cloudinary.CreateArchive(parameters));

            StringAssert.StartsWith("To create an archive with multiple types of assets", ex.Message);

            Assert.AreEqual(fQPublicIds, parameters.FullyQualifiedPublicIds());

            parameters.ResourceType("auto");

            var result = m_cloudinary.CreateArchive(parameters);

            Assert.AreEqual(3, result.FileCount);
        }

        [Test]
        public void TestCreateZip()
        {
            var parameters = UploadImageForArchiveAndPrepareParameters(GetMethodTag());
            var result = m_cloudinary.CreateZip(parameters);

            Assert.AreEqual($"{parameters.TargetPublicId()}.{FILE_FORMAT_ZIP}", result.PublicId);
            Assert.AreEqual(1, result.FileCount);
        }

        [Test]
        public void TestDownloadArchiveUrlForImage()
        {
            var tag = GetMethodTag();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{tag},{m_apiTag}"
            };
            var uploadResult = m_cloudinary.Upload(uploadParams);
            var publicIds = new List<string> { uploadResult.PublicId };

            var archiveParams = new ArchiveParams().PublicIds(publicIds);
            var archiveUrl = m_cloudinary.DownloadArchiveUrl(archiveParams);

            Assert.True(CanArchiveFileBeDownloaded(archiveUrl));
        }

        [Test]
        public void TestDownloadArchiveUrlForVideo()
        {
            var tag = GetMethodTag();
            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(m_testVideoPath),
                Tags = $"{tag},{m_apiTag}"
            };
            var uploadResult = m_cloudinary.Upload(uploadParams);
            var publicIds = new List<string> { uploadResult.PublicId };

            var video = ApiShared.GetCloudinaryParam(ResourceType.Video);
            var archiveParams = new ArchiveParams().ResourceType(video).PublicIds(publicIds);
            var archiveUrl = m_cloudinary.DownloadArchiveUrl(archiveParams);

            Assert.True(CanArchiveFileBeDownloaded(archiveUrl));
        }

        [Test]
        public void TestDownloadArchiveUrlForRaw()
        {
            var tag = GetMethodTag();
            var raw = ApiShared.GetCloudinaryParam(ResourceType.Raw);
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(m_testPdfPath),
                Tags = $"{tag},{m_apiTag}"
            };
            var uploadResult = m_cloudinary.Upload(uploadParams, raw);
            var publicIds = new List<string> { uploadResult.PublicId };

            var archiveParams = new ArchiveParams().ResourceType(raw).PublicIds(publicIds);
            var archiveUrl = m_cloudinary.DownloadArchiveUrl(archiveParams);

            Assert.True(CanArchiveFileBeDownloaded(archiveUrl));
        }

        [Test]
        public void TestDownloadZipForImage()
        {
            var tag = GetMethodTag();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{tag},{m_apiTag}"
            };
            m_cloudinary.Upload(uploadParams);

            var archiveUrl = m_cloudinary.DownloadZip(tag, null);

            Assert.True(CanArchiveFileBeDownloaded(archiveUrl));
        }

        [Test]
        public void TestDownloadZipForVideo()
        {
            var tag = GetMethodTag();
            var video = ApiShared.GetCloudinaryParam(ResourceType.Video);
            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(m_testVideoPath),
                Tags = $"{tag},{m_apiTag}"
            };
            m_cloudinary.Upload(uploadParams);
            
            var archiveUrl = m_cloudinary.DownloadZip(tag, null, video);

            Assert.True(CanArchiveFileBeDownloaded(archiveUrl));
        }

        private ImageUploadResult UploadImageForTestArchive(string archiveTag, double width, bool useFileName)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(width) },
                UseFilename = useFileName,
                Tags = $"{archiveTag},{m_apiTag}"
            };
            return m_cloudinary.Upload(uploadParams);
        }

        private ArchiveParams UploadImageForArchiveAndPrepareParameters(string archiveTag)
        {
            UploadImageForTestArchive($"{archiveTag},{m_apiTag}", 2.0, true);

            return new ArchiveParams().Tags(new List<string> { archiveTag, "non-existent-tag" }).
                TargetTags(new List<string> { m_apiTag }).TargetPublicId(GetUniquePublicId());
        }
    }
}
