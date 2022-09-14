using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTests.AdminApi
{
    public class UpdateResourcesTest: IntegrationTestBase
    {
        private const string ILLEGAL_STRING = "illegal";
        private const string ILLEGAL_MESSAGE = "Illegal value";
        private const string MODERATION_MANUAL = "manual";

        private string m_implicitTransformationText;
        private Transformation m_implicitTransformation;

        public override void Initialize()
        {
            base.Initialize();

            m_implicitTransformationText = m_suffix + "_implicit";
            m_implicitTransformation = new Transformation().Crop("scale").Overlay(new TextLayer().Text(m_implicitTransformationText).FontFamily("Arial").FontSize(60));
        }

        [Test, RetryWithDelay]
        public void TestOcrUpdate()
        {
            // should support requesting ocr info
            var uploadResult = m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            });

            var updateResult = m_cloudinary.UpdateResource(new UpdateParams(uploadResult.PublicId)
            {
                Ocr = ILLEGAL_STRING
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, updateResult.StatusCode, updateResult.Error?.Message);
            Assert.True(updateResult.Error.Message.StartsWith(ILLEGAL_MESSAGE));
        }

        [Test, IgnoreAddon("ocr"), RetryWithDelay]
        public void TestOcrUpdateResult()
        {
            // should support requesting ocr info from adv_ocr addon
            var uploadResult = m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag,
                Transformation = m_implicitTransformation
            });

            var updateResult = m_cloudinary.UpdateResource(new UpdateParams(uploadResult.PublicId)
            {
                Ocr = "adv_ocr"
            });

            Assert.AreEqual(HttpStatusCode.OK, updateResult.StatusCode, updateResult.Error?.Message);

            // OCR sometimes replaces `_`,`-` with space or empty string and adds newline at the end, ignore those
            CloudinaryAssert.StringsAreEqualIgnoringCaseAndChars(
                m_implicitTransformationText,
                updateResult.Info.Ocr.AdvOcr.Data[0].FullTextAnnotation.Text,
                "_- \n");
        }

        [Test, RetryWithDelay]
        public void TestRawConvertUpdate()
        {
            // should support requesting raw conversion

            var uploadResult = m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testPdfPath),
                Tags = m_apiTag
            });

            var updateResult = m_cloudinary.UpdateResource(new UpdateParams(uploadResult.PublicId)
            {
                RawConvert = ILLEGAL_STRING
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, updateResult.StatusCode, updateResult.Error?.Message);
            Assert.True(updateResult.Error.Message.StartsWith(ILLEGAL_MESSAGE));
        }

        [Test, RetryWithDelay]
        public void TestCategorizationUpdate()
        {
            // should support requesting categorization

            var uploadResult = m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            });

            var updateResult = m_cloudinary.UpdateResource(new UpdateParams(uploadResult.PublicId)
            {
                Categorization = ILLEGAL_STRING
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, updateResult.StatusCode, updateResult.Error?.Message);
            Assert.True(updateResult.Error.Message.StartsWith(ILLEGAL_MESSAGE));
        }

        [Test, RetryWithDelay]
        public void TestDetectionUpdate()
        {
            // should support requesting detection

            var uploadResult = m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            });

            var updateResult = m_cloudinary.UpdateResource(new UpdateParams(uploadResult.PublicId)
            {
                Detection = ILLEGAL_STRING
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, updateResult.StatusCode, updateResult.Error?.Message);
            Assert.True(updateResult.Error.Message.StartsWith(ILLEGAL_MESSAGE));
        }

        /// <summary>
        /// Test that we can update access control of the resource
        /// </summary>
        [Test, RetryWithDelay]
        public void TestUpdateAccessControl()
        {
            var start = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = new DateTime(3000, 12, 31, 23, 59, 59, DateTimeKind.Utc);

            var accessControl = new List<AccessControlRule> { new AccessControlRule {
                AccessType = AccessType.Anonymous,
                Start = start,
                End = end
            }};

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag,
                AccessControl = accessControl
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(1, uploadResult.AccessControl.Count, uploadResult.Error?.Message);

            Assert.AreEqual(AccessType.Anonymous, uploadResult.AccessControl[0].AccessType);
            Assert.AreEqual(start, uploadResult.AccessControl[0].Start);
            Assert.AreEqual(end, uploadResult.AccessControl[0].End);

            var newAccessControl = new List<AccessControlRule> { new AccessControlRule {
                AccessType = AccessType.Token,
                Start = end,
                End = start
            }};

            var updateResult = m_cloudinary.UpdateResource(
                new UpdateParams(uploadResult.PublicId) { AccessControl = newAccessControl }
            );

            Assert.AreEqual(1, updateResult.AccessControl.Count, updateResult.Error?.Message);

            Assert.AreEqual(AccessType.Token, updateResult.AccessControl[0].AccessType);
            Assert.AreEqual(end, updateResult.AccessControl[0].Start);
            Assert.AreEqual(start, updateResult.AccessControl[0].End);
        }

        [Test, RetryWithDelay]
        public void TestUpdateQuality()
        {
            //should update quality
            string publicId = GetUniquePublicId();
            var upResult = m_cloudinary.Upload(new ImageUploadParams() { File = new FileDescription(m_testImagePath), PublicId = publicId, Overwrite = true, Tags = m_apiTag });
            var updResult = m_cloudinary.UpdateResource(new UpdateParams(upResult.PublicId) { QualityOverride = "auto:best" });
            Assert.AreEqual(updResult.StatusCode, HttpStatusCode.OK, updResult.Error?.Message);
            Assert.Null(updResult.Error);
            Assert.AreEqual(updResult.PublicId, publicId);
        }

        [Test, RetryWithDelay]
        public void TestUpdateCustomCoordinates()
        {
            //should update custom coordinates

            var coordinates = new Core.Rectangle(121, 31, 110, 151);

            var upResult = UploadTestImageResource();

            m_cloudinary.UpdateResource(
                new UpdateParams(upResult.PublicId)
                {
                    CustomCoordinates = coordinates
                });

            var result = m_cloudinary.GetResource(
                new GetResourceParams(upResult.PublicId)
                {
                    Coordinates = true
                });

            AssertUpdatedCustomCoordinates(result, coordinates);
        }

        [Test, RetryWithDelay]
        public async Task TestUpdateCustomCoordinatesAsync()
        {
            //should update custom coordinates
            var upResult = await UploadTestImageResourceAsync();

            var coordinates = new Core.Rectangle(121, 31, 110, 151);

            await m_cloudinary.UpdateResourceAsync(
                new UpdateParams(upResult.PublicId)
                {
                    CustomCoordinates = coordinates
                });

            var result = await m_cloudinary.GetResourceAsync(
                new GetResourceParams(upResult.PublicId)
                {
                    Coordinates = true
                });

            AssertUpdatedCustomCoordinates(result, coordinates);
        }

        private void AssertUpdatedCustomCoordinates(GetResourceResult result, Core.Rectangle coordinates)
        {
            Assert.NotNull(result.Coordinates, result.Error?.Message);
            Assert.NotNull(result.Coordinates.Custom);
            Assert.AreEqual(1, result.Coordinates.Custom.Length);
            Assert.AreEqual(4, result.Coordinates.Custom[0].Length);
            Assert.AreEqual(coordinates.X, result.Coordinates.Custom[0][0]);
            Assert.AreEqual(coordinates.Y, result.Coordinates.Custom[0][1]);
            Assert.AreEqual(coordinates.Width, result.Coordinates.Custom[0][2]);
            Assert.AreEqual(coordinates.Height, result.Coordinates.Custom[0][3]);
        }

        [Test, RetryWithDelay]
        public void TestManualModeration()
        {
            // should support setting manual moderation status

            var uploadResult = m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Moderation = MODERATION_MANUAL,
                Tags = m_apiTag
            });

            Assert.NotNull(uploadResult);

            var updateResult = m_cloudinary.UpdateResource(new UpdateParams(uploadResult.PublicId) { ModerationStatus = ModerationStatus.Approved });

            Assert.NotNull(updateResult);
            Assert.NotNull(updateResult.Moderation, updateResult.Error?.Message);
            Assert.AreEqual(1, updateResult.Moderation.Count);
            Assert.AreEqual(ModerationStatus.Approved, updateResult.Moderation[0].Status);
        }

        [Test, IgnoreFeature("dynamic_folders"), RetryWithDelay]
        public void TestUpdateDynamicFolderAttributes()
        {
            //should update quality
            var publicId = GetUniquePublicId();
            var assetFolder = GetUniqueFolder();
            var displayName = GetUniquePublicId();
            var upResult = m_cloudinary.Upload(new ImageUploadParams
                {
                    File = new FileDescription(m_testImagePath),
                    PublicId = publicId,
                    Tags = m_apiTag
                }
            );

            var updResult = m_cloudinary.UpdateResource(
                new UpdateParams(upResult.PublicId)
                {
                    AssetFolder = assetFolder,
                    DisplayName = displayName,
                    UniqueDisplayName = true

                });
            Assert.AreEqual(updResult.StatusCode, HttpStatusCode.OK, updResult.Error?.Message);
            Assert.Null(updResult.Error);

            Assert.AreEqual(updResult.PublicId, publicId);
            Assert.AreEqual(assetFolder, updResult.AssetFolder);
            Assert.AreEqual(displayName, updResult.DisplayName);
        }

        [Test, RetryWithDelay]
        public void TestUpdateResourceMetadata()
        {
            var uploadResult = m_cloudinary.Upload(new ImageUploadParams
            {
                File = new FileDescription(m_testImagePath),
            });
            var metadataFieldId = CreateMetadataField("metadata_update");

            const string metadataValue = "test value";
            var metadata = new StringDictionary
            {
                {metadataFieldId, metadataValue}
            };

            var updateResult = m_cloudinary.UpdateResource(new UpdateParams(uploadResult.PublicId)
            {
                Metadata = metadata
            });

            Assert.NotNull(updateResult);
            Assert.AreEqual(HttpStatusCode.OK, updateResult.StatusCode, updateResult.Error?.Message);
            Assert.NotNull(updateResult.MetadataFields);
        }

        [Test, RetryWithDelay]
        public void TestUpdateMetadata()
        {
            var uploadResult = m_cloudinary.Upload(new ImageUploadParams
            {
                File = new FileDescription(m_testImagePath),
            });
            var metadataExternalId = CreateMetadataField("metadata_update");

            var updateParams = new MetadataUpdateParams
            {
                PublicIds = new List<string> { uploadResult.PublicId },
            };
            updateParams.Metadata.Add(metadataExternalId, "new value");

            var updateResult = m_cloudinary.UpdateMetadata(updateParams);

            Assert.NotNull(updateResult);
            Assert.AreEqual(HttpStatusCode.OK, updateResult.StatusCode, updateResult.Error?.Message);
            Assert.IsNotEmpty(updateResult.PublicIds);
        }
    }
}
