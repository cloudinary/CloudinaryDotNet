using System;
using System.Collections.Generic;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.UploadApi
{
    public class ArchiveMethodsTest : IntegrationTestBase
    {
        [Test, RetryWithDelay]
        public void TestCreateArchive()
        {
            var targetPublicId = GetUniquePublicId();
            var archiveTag = GetMethodTag();

            var res = UploadResourceForTestArchive<ImageUploadParams>(archiveTag, true, 2.0);

            var parameters = new ArchiveParams()
                                            .Tags(new List<string> { archiveTag, "no_such_tag" })
                                            .TargetPublicId(targetPublicId)
                                            .TargetTags(new List<string> { m_apiTag });
            var result = m_cloudinary.CreateArchive(parameters);
            Assert.AreEqual($"{targetPublicId}.{FILE_FORMAT_ZIP}", result.PublicId);
            Assert.AreEqual(1, result.FileCount);

            var res2 = UploadResourceForTestArchive<ImageUploadParams>(archiveTag, false, 500);

            var transformations = new List<Transformation> { m_simpleTransformation, m_updateTransformation };
            parameters = new ArchiveParams().PublicIds(new List<string> { res.PublicId, res2.PublicId })
                                            .Transformations(transformations)
                                            .FlattenFolders(true)
                                            .SkipTransformationName(true)
                                            .UseOriginalFilename(true)
                                            .Tags(new List<string> { archiveTag })
                                            .TargetTags(new List<string> { m_apiTag })
                                            .AllowMissing(true);
            result = m_cloudinary.CreateArchive(parameters);
            Assert.AreEqual(2, result.FileCount);
        }

        [Test, RetryWithDelay]
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

        [Test, RetryWithDelay]
        public void TestCreateArchiveMultiplePublicIds()
        {
            // should support archiving based on multiple public IDs
            var parameters = UploadImageForArchiveAndPrepareParameters(GetMethodTag());
            var result = m_cloudinary.CreateArchive(parameters);

            Assert.AreEqual($"{parameters.TargetPublicId()}.{FILE_FORMAT_ZIP}", result.PublicId);
            Assert.AreEqual(1, result.FileCount);
        }

        [Test, RetryWithDelay]
        public void TestCreateArchiveMultipleResourceTypes()
        {
            var tag = GetMethodTag();
            var upRes1 = UploadResourceForTestArchive<RawUploadParams>(tag);
            var upRes2 = UploadResourceForTestArchive<ImageUploadParams>(tag);
            var upRes3 = UploadResourceForTestArchive<VideoUploadParams>(tag);

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

        [Test, RetryWithDelay]
        public void TestCreateZip()
        {
            var parameters = UploadImageForArchiveAndPrepareParameters(GetMethodTag());
            var result = m_cloudinary.CreateZip(parameters);

            Assert.AreEqual($"{parameters.TargetPublicId()}.{FILE_FORMAT_ZIP}", result.PublicId);
            Assert.AreEqual(1, result.FileCount);
        }

        [Test, RetryWithDelay]
        public void TestArchiveRequestParametersAndResponseFields()
        {
            var archiveTag = GetMethodTag();

            var parameters = UploadImageForArchiveAndPrepareParameters(GetMethodTag());
            parameters.ResourceType(ResourceType.Image.ToString().ToLower());
            parameters.Type("upload");
            parameters.Transformations(new List<Transformation>() { new Transformation().Crop("scale").Width(500) });
            parameters.ExpiresAt(1415060076);
            parameters.UseOriginalFilename(true);
            parameters.KeepDerived(true);
            parameters.FlattenFolders(true);
            parameters.SkipTransformationName(true);
            parameters.Tags(new List<string> { archiveTag });
            parameters.TargetTags(new List<string> { m_apiTag });
            parameters.AllowMissing(true);

            var result = m_cloudinary.CreateZip(parameters);

            Assert.IsNotNull(result.Version);
            Assert.IsNotNull(result.Signature);
            Assert.AreEqual(ResourceType.Raw, result.ResourceType);
            Assert.Greater(result.CreatedAt, new DateTime(1970, 01, 01));
            Assert.AreEqual("upload", result.Type);
            Assert.IsNotNull(result.Etag);
            Assert.AreEqual(false, result.Placeholder);
            Assert.NotZero(result.ResourceCount);
            Assert.NotZero(result.Tags.Length);
        }

        [Test, RetryWithDelay]
        public void TestDownloadArchiveUrl()
        {
            var archiveTag = GetMethodTag();
            var uploadResult = UploadResourceForTestArchive<ImageUploadParams>(archiveTag);
            var publicIds = new List<string> { uploadResult.PublicId };

            var archiveParams = new ArchiveParams().PublicIds(publicIds);
            var archiveUrl = m_cloudinary.DownloadArchiveUrl(archiveParams);

            Assert.True(UrlExists(archiveUrl));
        }

        [Test, RetryWithDelay]
        public void TestDownloadArchiveUrlForVideo()
        {
            var archiveTag = GetMethodTag();
            var uploadResult = UploadResourceForTestArchive<VideoUploadParams>(archiveTag);
            var publicIds = new List<string> { uploadResult.PublicId };

            var resourceType = ApiShared.GetCloudinaryParam(ResourceType.Video);
            var archiveParams = new ArchiveParams().ResourceType(resourceType).PublicIds(publicIds);
            var archiveUrl = m_cloudinary.DownloadArchiveUrl(archiveParams);

            Assert.True(UrlExists(archiveUrl));
        }

        [Test, RetryWithDelay]
        public void TestDownloadArchiveUrlForRaw()
        {
            var archiveTag = GetMethodTag();
            var uploadResult = UploadResourceForTestArchive<RawUploadParams>(archiveTag);
            var publicIds = new List<string> { uploadResult.PublicId };

            var resourceType = ApiShared.GetCloudinaryParam(ResourceType.Raw);
            var archiveParams = new ArchiveParams().ResourceType(resourceType).PublicIds(publicIds);
            var archiveUrl = m_cloudinary.DownloadArchiveUrl(archiveParams);

            Assert.True(UrlExists(archiveUrl));
        }

        [Test, RetryWithDelay]
        public void TestDownloadZipForImage()
        {
            var archiveTag = GetMethodTag();
            UploadResourceForTestArchive<ImageUploadParams>(archiveTag);

            var archiveUrl = m_cloudinary.DownloadZip(archiveTag, null);

            Assert.True(UrlExists(archiveUrl));
        }

        [Test, RetryWithDelay]
        public void TestDownloadZipForVideo()
        {
            var archiveTag = GetMethodTag();
            UploadResourceForTestArchive<VideoUploadParams>(archiveTag);
            var resourceType = ApiShared.GetCloudinaryParam(ResourceType.Video);

            var archiveUrl = m_cloudinary.DownloadZip(archiveTag, null, resourceType);

            Assert.True(UrlExists(archiveUrl));
        }

        private ArchiveParams UploadImageForArchiveAndPrepareParameters(string archiveTag)
        {
            UploadResourceForTestArchive<ImageUploadParams>($"{archiveTag},{m_apiTag}", true, 2.0);

            return new ArchiveParams().Tags(new List<string> { archiveTag, "non-existent-tag" }).
                TargetTags(new List<string> { m_apiTag }).TargetPublicId(GetUniquePublicId());
        }

        private RawUploadResult UploadResourceForTestArchive<T>(string archiveTag, bool useFileName = false, double imageWidth = 0.0)
            where T: RawUploadParams
        {
            var filesMap = new Dictionary<Type, string>
            {
                {typeof(ImageUploadParams), m_testImagePath},
                {typeof(VideoUploadParams), m_testVideoPath},
                {typeof(RawUploadParams), m_testPdfPath}
            };

            var uploadParams = Activator.CreateInstance<T>();
            uploadParams.File = new FileDescription(filesMap[typeof(T)]);
            uploadParams.Tags = $"{archiveTag},{m_apiTag}";
            uploadParams.UseFilename = useFileName;
            if (imageWidth > 0 && uploadParams is ImageUploadParams)
            {
                (uploadParams as ImageUploadParams).EagerTransforms = new List<Transformation>
                {
                    new Transformation().Crop("scale").Width(imageWidth)
                };
            }

            return uploadParams.GetType() != typeof(RawUploadParams) ?
                m_cloudinary.Upload(uploadParams) :
                m_cloudinary.Upload(uploadParams, ApiShared.GetCloudinaryParam(ResourceType.Raw));
        }

        [Test, RetryWithDelay]
        public void TestDownloadFolderWithResourceTypeAll()
        {
            var folderUrl = m_cloudinary.DownloadFolder("samples/", null);

            Assert.True(folderUrl.Contains(Constants.RESOURCE_TYPE_ALL));
        }

        [Test, RetryWithDelay]
        public void TestDownloadFolderValidUrl()
        {
            var folderUrl = m_cloudinary.DownloadFolder("folder/");

            Assert.IsNotEmpty(folderUrl);
            Assert.True(folderUrl.Contains("generate_archive"));
        }

        [Test, RetryWithDelay]
        public void TestDownloadFolderFlattenFolder()
        {
            var parameters = new ArchiveParams();
            parameters.FlattenFolders(true);

            var folderUrl = m_cloudinary.DownloadFolder("folder/", parameters);

            Assert.True(folderUrl.Contains("flatten_folders"));
        }

        [Test, RetryWithDelay]
        public void TestDownloadFolderExpiresAt()
        {
            const int expiresAt = 1415060076;

            var parameters = new ArchiveParams();
            parameters.ExpiresAt(expiresAt);

            var folderUrl = m_cloudinary.DownloadFolder("folder/", parameters);

            Assert.True(folderUrl.Contains($"expires_at={expiresAt}"));
        }

        [Test, RetryWithDelay]
        public void TestDownloadFolderUseOriginalFileName()
        {
            var parameters = new ArchiveParams();
            parameters.UseOriginalFilename(true);

            var folderUrl = m_cloudinary.DownloadFolder("folder/", parameters);

            Assert.True(folderUrl.Contains("use_original_filename"));
        }

        [Test]
        public void TestCreateArchiveErrorMessage()
        {
            var parameters = new ArchiveParams()
                .PublicIds(new List<string> { "sample", "not exist" })
                .FlattenFolders(true)
                .SkipTransformationName(true)
                .UseOriginalFilename(true)
                .AllowMissing(false);

            var folderUrl = m_cloudinary.CreateArchive(parameters);

            Assert.NotNull(folderUrl.Error);
        }

        [Test]
        public void TestDownloadBackedUpAsset()
        {
            var publicId = GetUniquePublicId();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Backup = true,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);
            m_cloudinary.DeleteResources(publicId);
            m_cloudinary.Restore(publicId);
            var getResourceParams = new GetResourceParams(publicId) { Versions = true };
            var getResourceResult = m_cloudinary.GetResource(getResourceParams);
            var assetId = getResourceResult.AssetId;
            var versionId = getResourceResult.Versions[0].VersionId;

            var assetBackedUpUrl = m_cloudinary.DownloadBackedUpAsset(assetId, versionId);
            
            Assert.True(assetBackedUpUrl.Contains(assetId));
            Assert.True(assetBackedUpUrl.Contains(versionId));
            Assert.True(UrlExists(assetBackedUpUrl));
        }
    }
}
