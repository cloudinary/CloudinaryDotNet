using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTests.UploadApi
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class UploadMethodsTest : IntegrationTestBase
    {
        private const string TEST_IMAGE_PREFIX = "TestImage";

        private const string MODERATION_MANUAL = "manual";
        private const string MODERATION_AWS_REK = "aws_rek";
        private const string MODERATION_WEBPURIFY = "webpurify";
        private const string TEST_REMOTE_IMG = "http://cloudinary.com/images/old_logo.png";
        private const string TEST_REMOTE_VIDEO = "http://res.cloudinary.com/demo/video/upload/v1496743637/dog.mp4";

        private Transformation m_implicitTransformation;

        protected readonly Transformation m_transformationAngleExtended =
            new Transformation().Angle(45).Height(210).Crop("scale");
        protected readonly Transformation m_transformationAr25 = new Transformation().Width(100).AspectRatio(2.5);
        protected readonly Transformation m_transformationAr69 = new Transformation().Width(100).AspectRatio(6, 9);
        protected readonly Transformation m_transformationAr30 = new Transformation().Width(150).AspectRatio("3.0");
        protected readonly Transformation m_transformationAr12 = new Transformation().Width(100).AspectRatio("1:2");
        protected readonly Transformation m_eagerTransformation = new EagerTransformation(
            new Transformation().Width(512).Height(512), new Transformation().Width(100).Crop("scale")).SetFormat(FILE_FORMAT_PNG);

        protected string m_implicitTransformationText;

        public override void Initialize()
        {
            base.Initialize();

            m_implicitTransformationText = m_suffix + "_implicit";
            m_implicitTransformation = new Transformation().Crop("scale").Overlay(new TextLayer().Text(m_implicitTransformationText).FontFamily("Arial").FontSize(60));

            AddCreatedTransformation(
                m_implicitTransformation, m_transformationAngleExtended,
                m_transformationAr25, m_transformationAr69,
                m_transformationAr30, m_transformationAr12,
                m_eagerTransformation);
        }

        [Test, RetryWithDelay]
        public void TestUploadLocalImage()
        {
            var uploadResult = UploadTestImageResource();

            AssertDefaultTestImageUploadAndSignature(uploadResult);
        }

        [Test, RetryWithDelay]
        public async Task TestUploadLocalImageAsync()
        {
            var uploadResult = await UploadTestImageResourceAsync();

            AssertDefaultTestImageUploadAndSignature(uploadResult);
        }

        private void AssertDefaultTestImageUploadAndSignature(ImageUploadResult result)
        {
            Assert.AreEqual(1920, result.Width, result.Error?.Message);
            Assert.AreEqual(1200, result.Height);
            Assert.AreEqual(FILE_FORMAT_JPG, result.Format);

            var api = new Api(m_account);

            var expectedSign = api.SignParameters(new SortedDictionary<string, object>
            {
                { "public_id", result.PublicId },
                { "version", result.Version }
            });

            Assert.AreEqual(expectedSign, result.Signature);
        }

        [Test, RetryWithDelay]
        public void TestUploadLocalCustomFilename()
        {
            var imageFileName = GetUniquePublicId(StorageType.upload, FILE_FORMAT_JPG);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageFileName, m_testImagePath),
                Tags = m_apiTag,
                UseFilename = true,
                UniqueFilename = false
            };

            var uploadResultImage = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(imageFileName, uploadResultImage.PublicId, uploadResultImage.Error?.Message);

            var pdfFileName = GetUniquePublicId(StorageType.upload, FILE_FORMAT_PDF);
            var filePdf = new FileDescription(m_testPdfPath) {FileName = pdfFileName };

            uploadParams.File = filePdf;

            var uploadResultPdf = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(pdfFileName, uploadResultPdf.PublicId, uploadResultPdf.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestUploadLocalUnicodeFilename()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(TEST_UNICODE_IMAGE, m_testUnicodeImagePath),
                Tags = m_apiTag
            };

            var uploadResultImage = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(TEST_UNICODE_IMAGE_NAME, uploadResultImage.OriginalFilename, uploadResultImage.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestUploadResultCinemagraphAnalysis()
        {
            var imageFileName = GetUniquePublicId(StorageType.upload, FILE_FORMAT_JPG);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageFileName, m_testImagePath),
                Tags = m_apiTag,
                UseFilename = true,
                UniqueFilename = false,
                CinemagraphAnalysis = true
            };

            var uploadResultImage = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(imageFileName, uploadResultImage.PublicId, uploadResultImage.Error?.Message);
            Assert.Zero(uploadResultImage.CinemagraphAnalysis.CinemagraphScore);
        }

        [Test, RetryWithDelay]
        public void TestUploadLocalPDFPages()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testPdfPath),
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(FILE_FORMAT_PDF, uploadResult.Format, uploadResult.Error?.Message);
            Assert.AreEqual(TEST_PDF_PAGES_COUNT, uploadResult.Pages);
        }

        [Test, RetryWithDelay]
        public void TestUploadLocalImageTimeout()
        {
            const int TIMEOUT = 1000;
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            };

            // Save original values
            var origAddr = m_cloudinary.Api.ApiBaseAddress;
            var origTimeout = m_cloudinary.Api.Timeout;

            var stopWatch = new Stopwatch();

            try
            {
                m_cloudinary.Api.ApiBaseAddress = "https://10.255.255.1";
                m_cloudinary.Api.Timeout = TIMEOUT;

                stopWatch.Start();
                m_cloudinary.Upload(uploadParams);
            }
            catch (Exception)
            {
                stopWatch.Stop();
            }
            finally
            {
                m_cloudinary.Api.ApiBaseAddress = origAddr;
                m_cloudinary.Api.Timeout = origTimeout;
                stopWatch.Stop();
            }

            // It should take no longer than twice the timeout that we gave + 1 second (it should respect timeout)
            Assert.LessOrEqual(stopWatch.ElapsedMilliseconds, 2 * TIMEOUT + 1000);
            // It should take at least timeout we provided, otherwise some other error occurred before
            Assert.GreaterOrEqual(stopWatch.ElapsedMilliseconds, TIMEOUT);
        }

        [Test, RetryWithDelay]
        public void TestUploadLocalVideo()
        {
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(m_testVideoPath),
                Tags = m_apiTag,
                MediaMetadata=true
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(640, uploadResult.Width, uploadResult.Error?.Message);
            Assert.AreEqual(320, uploadResult.Height);
            Assert.AreEqual(FILE_FORMAT_MP4, uploadResult.Format);
            Assert.NotNull(uploadResult.Audio);
            Assert.AreEqual("aac", uploadResult.Audio.Codec);
            Assert.NotNull(uploadResult.Video);
            Assert.AreEqual("h264", uploadResult.Video.Codec);
            Assert.False(uploadResult.IsAudio);
            Assert.Zero(uploadResult.Pages);
            Assert.Zero(uploadResult.Rotation);
            Assert.NotZero(uploadResult.NbFrames);
            Assert.IsFalse(string.IsNullOrEmpty(uploadResult.PlaybackUrl));

            var getResource = new GetResourceParams(uploadResult.PublicId) { ResourceType = ResourceType.Video };
            var info = m_cloudinary.GetResource(getResource);

            Assert.AreEqual(FILE_FORMAT_MP4, info.Format, info.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestUploadCustom()
        {
            var video = Api.GetCloudinaryParam(ResourceType.Video);
            var uploadResult = m_cloudinary.Upload(video,
                new Dictionary<string, object> { { "tags", m_apiTag }},
                new FileDescription(m_testVideoPath));

            Assert.NotNull(uploadResult);
            Assert.AreEqual(video, uploadResult.ResourceType, uploadResult.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestModerationManual()
        {
            var uploadParams = new RawUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Moderation = MODERATION_MANUAL,
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(uploadResult);
            Assert.NotNull(uploadResult.Moderation, uploadResult.Error?.Message);
            Assert.AreEqual(1, uploadResult.Moderation.Count);
            Assert.AreEqual(MODERATION_MANUAL, uploadResult.Moderation[0].Kind);
            Assert.AreEqual(ModerationStatus.Pending, uploadResult.Moderation[0].Status);

            var getResult = m_cloudinary.GetResource(uploadResult.PublicId);

            Assert.NotNull(getResult);
            Assert.NotNull(getResult.Moderation, getResult.Error?.Message);
            Assert.AreEqual(1, getResult.Moderation.Count);
            Assert.AreEqual(MODERATION_MANUAL, getResult.Moderation[0].Kind);
            Assert.AreEqual(ModerationStatus.Pending, getResult.Moderation[0].Status);
        }

        [Test, IgnoreAddon("rekognition"), RetryWithDelay]
        public void TestModerationAwsRek()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Moderation = MODERATION_AWS_REK,
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);
            Assert.IsNotNull(uploadResult.Moderation, uploadResult.Error?.Message);
            Assert.AreEqual(1, uploadResult.Moderation.Count);
            Assert.AreEqual(ModerationStatus.Approved, uploadResult.Moderation[0].Status);
            Assert.AreEqual(MODERATION_AWS_REK, uploadResult.Moderation[0].Kind);
        }

        [Test, IgnoreAddon("webpurify"), RetryWithDelay]
        public void TestModerationWebpurify()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Moderation = MODERATION_WEBPURIFY,
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);
            Assert.IsNotNull(uploadResult.Moderation, uploadResult.Error?.Message);
            Assert.AreEqual(1, uploadResult.Moderation.Count);
            Assert.AreEqual(MODERATION_WEBPURIFY, uploadResult.Moderation[0].Kind);
        }

        [Test, IgnoreAddon("rekognition"), RetryWithDelay]
        public void TestRekognitionFace()
        {
            // should support rekognition face
            // RekognitionFace add-on should be enabled for the used account
            var rekognitionFace = "rekognition_face";
            var complete = "complete";

            var uploadResult = m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            });

            Assert.IsNull(uploadResult.Info, uploadResult.Error?.Message);

            var updateResult = m_cloudinary.UpdateResource(new UpdateParams(uploadResult.PublicId)
            {
                Detection = rekognitionFace
            });

            if (updateResult.Error.Message.StartsWith("You don't have an active"))
            {
                Assert.Ignore(updateResult.Error.Message);
            }

            Assert.NotNull(updateResult.Info, uploadResult.Error?.Message);
            Assert.NotNull(updateResult.Info.Detection);
            Assert.NotNull(updateResult.Info.Detection.RekognitionFace);
            Assert.AreEqual(complete, updateResult.Info.Detection.RekognitionFace.Status);

            uploadResult = m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Detection = rekognitionFace,
                Tags = m_apiTag
            });

            Assert.NotNull(uploadResult.Info, uploadResult.Error?.Message);
            Assert.NotNull(uploadResult.Info.Detection);
            Assert.NotNull(uploadResult.Info.Detection.RekognitionFace);
            Assert.AreEqual(complete, uploadResult.Info.Detection.RekognitionFace.Status);
        }

        [Test, RetryWithDelay]
        public void TestUploadOverwrite()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = GetUniquePublicId(),
                Overwrite = false,
                Tags = m_apiTag
            };

            var img1 = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(img1);

            uploadParams.File = new FileDescription(m_testPdfPath);

            var img2 = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(img2);
            Assert.AreEqual(img1.Bytes, img2.Bytes, img1.Error?.Message ?? img2.Error?.Message);

            uploadParams.Overwrite = true;

            img2 = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(img2);
            Assert.AreNotEqual(img1.Bytes, img2.Bytes, img1.Error?.Message ?? img2.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestUploadLocalImageGetMetadata()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                PublicId = GetUniquePublicId(),
                ImageMetadata = true,
                MediaMetadata = true,
                Exif = true,
                Colors = true,
                Tags = m_apiTag,
                UseFilename = true,
                Faces = true,
                Phash = true,
                QualityAnalysis = true,
                ReturnDeleteToken = true
            };

            ImageUploadResult result = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(result.ImageMetadata, result.Error?.Message);
            Assert.NotNull(result.Exif);
            Assert.NotNull(result.Colors);
            Assert.Zero(result.IllustrationScore);
            Assert.False(result.SemiTransparent);
            Assert.False(result.Grayscale);
            Assert.NotNull(result.Eager);
            Assert.NotNull(result.Predominant);
        }

        [Test, RetryWithDelay]
        public void TestUploadLocalImageUseFilename()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                EagerAsync = true,
                UseFilename = true,
                Tags = m_apiTag
            };

            ImageUploadResult result = m_cloudinary.Upload(uploadParams);

            Assert.True(result.PublicId.StartsWith(TEST_IMAGE_PREFIX), result.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestUploadLocalImageUniqueFilename()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                EagerAsync = true,
                UseFilename = true,
                UniqueFilename = false,
                Tags = m_apiTag
            };

            var result = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(TEST_IMAGE_PREFIX, result.PublicId, result.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestUploadTransformationResize()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Transformation = m_resizeTransformation,
                Tags = m_apiTag
            };

            ImageUploadResult uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(m_resizeTransformationWidth, uploadResult.Width, uploadResult.Error?.Message);
            Assert.AreEqual(m_resizeTransformationHeight, uploadResult.Height);
            Assert.AreEqual(FILE_FORMAT_JPG, uploadResult.Format);
        }

        [Test, RetryWithDelay]
        public void TestUploadRemote()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(TEST_REMOTE_IMG),
                Tags = m_apiTag
            };

            // remote files should not be streamed
            Assert.IsNull(uploadParams.File.Stream);
            Assert.IsTrue(uploadParams.File.IsRemote);

            var uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(3381, uploadResult.Bytes, uploadResult.Error?.Message);
            Assert.AreEqual(241, uploadResult.Width);
            Assert.AreEqual(51, uploadResult.Height);
            Assert.AreEqual(FILE_FORMAT_PNG, uploadResult.Format);
        }

        [Test, RetryWithDelay]
        public void TestUploadDataUri()
        {
            var base64Image = "data:image/png;base64,iVBORw0KGgoAA\nAANSUhEUgAAABAAAAAQAQMAAAAlPW0iAAAABlBMVEUAAAD///+l2Z/dAAAAM0l\nEQVR4nGP4/5/h/1+G/58ZDrAz3D/McH8yw83NDDeNGe4Ug9C9zwz3gVLMDA/A6\nP9/AFGGFyjOXZtQAAAAAElFTkSuQmCC";

            var upload = new ImageUploadParams()
            {
                File = new FileDescription(base64Image),
                Tags = m_apiTag
            };

            var result = m_cloudinary.Upload(upload);

            Assert.AreEqual(16, result.Width, result.Error?.Message);
            Assert.AreEqual(16, result.Height);
        }

        [Test, RetryWithDelay]
        public void TestUploadAnsiAndUnicode()
        {
            // should allow get resource details
            var publicId = "%20Test Image_僅測試";
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                PublicId = publicId,
                Type = "authenticated",
                Overwrite = true,
                UseFilename = true,
                UniqueFilename = false,
                Format = "jpg",
                Tags = m_apiTag
            };

            var uploadresponse = m_cloudinary.Upload(uploadParams);

            var expectedUrl = uploadresponse.SecureUrl.OriginalString;

            var computedUrl = m_cloudinary.Api.UrlImgUp.Action("authenticated").Version(uploadresponse.Version).Secure(true).Signed(true).BuildUrl(publicId + ".jpg");

            Assert.AreEqual(expectedUrl, computedUrl);

        }

        [Test, RetryWithDelay]
        public void TestUploadStream()
        {
            byte[] bytes = File.ReadAllBytes(m_testImagePath);
            var streamed = "streamed";

            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                ImageUploadParams uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(streamed, memoryStream),
                    Tags = $"{m_apiTag},{streamed}"
                };

                ImageUploadResult uploadResult = m_cloudinary.Upload(uploadParams);

                Assert.AreEqual(1920, uploadResult.Width, uploadResult.Error?.Message);
                Assert.AreEqual(1200, uploadResult.Height);
                Assert.AreEqual(FILE_FORMAT_JPG, uploadResult.Format);
            }
        }

        [Test, RetryWithDelay]
        public void TestUploadLargeRawFiles()
        {
            // support uploading large raw files
            var largeFilePath = m_testLargeImagePath;
            int largeFileLength = (int)new FileInfo(largeFilePath).Length;

            var uploadParams = GetUploadLargeRawParams(largeFilePath);

            var result = m_cloudinary.UploadLarge(uploadParams, 5 * 1024 * 1024);

            AssertUploadLarge(result, largeFileLength);
        }

        [Test, RetryWithDelay]
        public async Task TestUploadLargeRawFilesAsync()
        {
            // support asynchronous uploading large raw files
            var largeFilePath = m_testLargeImagePath;
            int largeFileLength = (int)new FileInfo(largeFilePath).Length;

            var uploadParams = GetUploadLargeRawParams(largeFilePath);

            var result = await m_cloudinary.UploadLargeAsync(uploadParams, 5 * 1024 * 1024);

            AssertUploadLarge(result, largeFileLength);
        }

        private RawUploadParams GetUploadLargeRawParams(string path)
        {
            return new RawUploadParams()
            {
                File = new FileDescription(path),
                Tags = m_apiTag
            };
        }

        private void AssertUploadLarge(RawUploadResult result, int fileLength)
        {
            Assert.AreEqual(fileLength, result.Bytes, result.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestUploadLarge()
        {
            // support uploading large image

            var largeFilePath = m_testLargeImagePath;
            int fileLength = (int)new FileInfo(largeFilePath).Length;
            var result = m_cloudinary.UploadLarge(new ImageUploadParams()
            {
                File = new FileDescription(largeFilePath),
                Tags = m_apiTag
            }, 5 * 1024 * 1024);

            Assert.AreEqual(fileLength, result.Bytes, result.Error?.Message);
        }

        [Test, RetryWithDelay]
        public async Task TestUploadLargeAutoFilesAsync()
        {
            // support asynchronous uploading large raw files
            var largeFilePath = m_testLargeImagePath;
            int largeFileLength = (int)new FileInfo(largeFilePath).Length;

            var uploadParams =  new AutoUploadParams()
            {
                File = new FileDescription(largeFilePath),
                Tags = m_apiTag
            };

            var result = await m_cloudinary.UploadLargeAsync(uploadParams, 5 * 1024 * 1024);

            AssertUploadLarge(result, largeFileLength);

            Assert.AreEqual("image", result.ResourceType);
        }
        /// <summary>
        /// Test access control rules
        /// </summary>
        [Test, RetryWithDelay]
        public void TestUploadAccessControl()
        {
            var start = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = new DateTime(3000, 12, 31, 23, 59, 59, DateTimeKind.Utc);

            var accessControl = new List<AccessControlRule>
            { new AccessControlRule
                {
                    AccessType = AccessType.Anonymous,
                    Start = start,
                    End = end
                }
            };

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                AccessControl = accessControl,
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(1, uploadResult.AccessControl.Count, uploadResult.Error?.Message);

            Assert.AreEqual(AccessType.Anonymous, uploadResult.AccessControl[0].AccessType);
            Assert.AreEqual(start, uploadResult.AccessControl[0].Start);
            Assert.AreEqual(end, uploadResult.AccessControl[0].End);

            uploadParams.AccessControl.Add(new AccessControlRule { AccessType = AccessType.Token });

            uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(2, uploadResult.AccessControl.Count, uploadResult.Error?.Message);

            Assert.AreEqual(AccessType.Anonymous, uploadResult.AccessControl[0].AccessType);
            Assert.AreEqual(start, uploadResult.AccessControl[0].Start);
            Assert.AreEqual(end, uploadResult.AccessControl[0].End);

            Assert.AreEqual(AccessType.Token, uploadResult.AccessControl[1].AccessType);
            Assert.IsNull(uploadResult.AccessControl[1].Start);
            Assert.IsNull(uploadResult.AccessControl[1].End);
        }

        [Test, RetryWithDelay]
        public void TestUploadLargeVideoFromWeb()
        {
            // support uploading large video
            var result = m_cloudinary.UploadLarge(new VideoUploadParams()
            {
                File = new FileDescription(TEST_REMOTE_VIDEO),
                Tags = m_apiTag
            }, 5 * 1024 * 1024);

            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK, result.Error?.Message);
            Assert.AreEqual(result.Format, FILE_FORMAT_MP4);
        }

        [Test, RetryWithDelay]
        public void TestUploadLargeImageFromWeb()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(TEST_REMOTE_IMG),
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.UploadLarge(uploadParams);

            Assert.AreEqual(uploadResult.StatusCode, HttpStatusCode.OK, uploadResult.Error?.Message);
            Assert.AreEqual(FILE_FORMAT_PNG, uploadResult.Format);
        }

        [Test, RetryWithDelay]
        public void TestEager()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation, m_eagerTransformation },
                Tags = $"{m_apiTag},{GetMethodTag()}"
            };

            m_cloudinary.Upload(uploadParams);
            //TODO: fix this test, implement assertions
        }

        [Test, RetryWithDelay]
        public void TestAllowedFormats()
        {
            //should allow whitelisted formats if allowed_formats

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                AllowedFormats = new string[] { FILE_FORMAT_JPG },
                Tags = m_apiTag
            };

            var res = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(FILE_FORMAT_JPG, res.Format, res.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestAllowedFormatsWithIllegalFormat()
        {
            //should prevent non whitelisted formats from being uploaded if allowed_formats is specified

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                AllowedFormats = new string[] { FILE_FORMAT_PNG },
                Tags = m_apiTag
            };

            var res = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(HttpStatusCode.BadRequest, res.StatusCode, res.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestAllowedFormatsWithFormat()
        {
            //should allow non whitelisted formats if type is specified and convert to that type

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                AllowedFormats = new string[] { FILE_FORMAT_PNG },
                Format = FILE_FORMAT_PNG,
                Tags = m_apiTag
            };

            var res = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(FILE_FORMAT_PNG, res.Format, res.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestUploadHeaders()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = GetUniquePublicId(),
                Tags = m_apiTag
            };

            uploadParams.Headers = new Dictionary<string, string>
            {
                { "Link", "1" },
                { "Blink", "182" }
            };

            m_cloudinary.Upload(uploadParams);
            //TODO: fix this test, implement assertions
        }

        [Test, RetryWithDelay]
        public void TestAspectRatioTransformation()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag,
                PublicId = GetUniquePublicId(),
                Transformation = m_transformationAr25
            };
            ImageUploadResult iuResult25 = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(iuResult25);
            Assert.AreEqual(100, iuResult25.Width, iuResult25.Error?.Message);
            Assert.AreEqual(40, iuResult25.Height);

            uploadParams.PublicId = GetUniquePublicId();
            uploadParams.Transformation = m_transformationAr69;
            ImageUploadResult iuResult69 = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(iuResult69);
            Assert.AreEqual(100, iuResult69.Width, iuResult69.Error?.Message);
            Assert.AreEqual(150, iuResult69.Height);

            uploadParams.PublicId = GetUniquePublicId();
            uploadParams.Transformation = m_transformationAr30;
            ImageUploadResult iuResult30 = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(iuResult30);
            Assert.AreEqual(150, iuResult30.Width, iuResult30.Error?.Message);
            Assert.AreEqual(50, iuResult30.Height);

            uploadParams.PublicId = GetUniquePublicId();
            uploadParams.Transformation = m_transformationAr12;
            ImageUploadResult iuResult12 = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(iuResult12);
            Assert.AreEqual(100, iuResult12.Width, iuResult12.Error?.Message);
            Assert.AreEqual(200, iuResult12.Height);
        }

        [Test, RetryWithDelay]
        public void TestResponsiveBreakpointsToJson()
        {
            var responsiveBreakpoint = new ResponsiveBreakpoint().ToString(Formatting.None);
            Assert.AreEqual("{\"create_derived\":true}", responsiveBreakpoint, "an empty ResponsiveBreakpoint should have create_derived=true");

            var expectedToken1 = JToken.Parse("{\"create_derived\":false,\"max_width\":500,\"min_width\":100,\"max_images\":5,\"transformation\":\"a_45\"}");
            IEnumerable<string> expectedList1 = expectedToken1.Children().Select(s => s.ToString(Formatting.None));

            var breakpoint = new ResponsiveBreakpoint().CreateDerived(false)
                    .Transformation(m_simpleTransformationAngle)
                    .MaxWidth(500)
                    .MinWidth(100)
                    .MaxImages(5);

            var actualList1 = breakpoint.Children().Select(s => s.ToString(Formatting.None));
            CollectionAssert.AreEquivalent(expectedList1, actualList1);

            breakpoint.Transformation(m_transformationAngleExtended);

            var expectedToken2 = JToken.Parse("{\"create_derived\":false,\"max_width\":500,\"min_width\":100,\"max_images\":5,\"transformation\":\"a_45,c_scale,h_210\"}");
            var expectedList2 = expectedToken2.Children().Select(s => s.ToString(Formatting.None));

            var actualList2 = breakpoint.Children().Select(s => s.ToString(Formatting.None));
            CollectionAssert.AreEquivalent(expectedList2, actualList2);
        }

        [Test, RetryWithDelay]
        public void TestResponsiveBreakpointsFormat()
        {
            var breakpoint = new ResponsiveBreakpoint()
                                .Transformation(m_simpleTransformationAngle)
                                .MaxImages(1)
                                .Format(FILE_FORMAT_GIF);

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = GetUniquePublicId(),
                Tags = m_apiTag,
                ResponsiveBreakpoints = new List<ResponsiveBreakpoint> { breakpoint }
            };

            ImageUploadResult result = m_cloudinary.Upload(uploadParams);

            Assert.Null(result.Error, result.Error?.Message);
            Assert.NotNull(result.ResponsiveBreakpoints, "result should include 'ResponsiveBreakpoints'");
            Assert.AreEqual(1, result.ResponsiveBreakpoints.Count);

            Assert.AreEqual(TRANSFORM_A_45, result.ResponsiveBreakpoints[0].Transformation);
            StringAssert.EndsWith(FILE_FORMAT_GIF, result.ResponsiveBreakpoints[0].Breakpoints[0].Url,
                $"generated breakpoint should have '{FILE_FORMAT_GIF}' extension");
        }

        [Test(Description = "Use Image upload parameters as Ad-Hoc custom parameters")]
        public void TestAdHocParams()
        {
            var breakpoint = new ResponsiveBreakpoint().MaxImages(5).BytesStep(20)
                                .MinWidth(200).MaxWidth(1000).CreateDerived(false);

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            };

            uploadParams.AddCustomParam("public_id", GetUniquePublicId());
            uploadParams.AddCustomParam("IgnoredEmptyParameter", "");
            uploadParams.AddCustomParam("responsive_breakpoints", JsonConvert.SerializeObject(new List<ResponsiveBreakpoint> { breakpoint }));
            uploadParams.AddCustomParam("IgnoredNullParameter", null);

            var paramsDict = uploadParams.ToParamsDictionary();
            Assert.IsFalse(paramsDict.ContainsKey("IgnoredEmptyParameter"));
            Assert.IsFalse(paramsDict.ContainsKey("IgnoredNullParameter"));

            ImageUploadResult result = m_cloudinary.Upload(uploadParams);
            Assert.NotNull(result.ResponsiveBreakpoints, result.Error?.Message); //todo: check it in netCore
            Assert.AreEqual(1, result.ResponsiveBreakpoints.Count);

            Assert.AreEqual(5, result.ResponsiveBreakpoints[0].Breakpoints.Count);
            Assert.AreEqual(1000, result.ResponsiveBreakpoints[0].Breakpoints[0].Width);
            Assert.AreEqual(200, result.ResponsiveBreakpoints[0].Breakpoints[4].Width);
        }

        [Test, RetryWithDelay]
        public void TestUploadAndGetResource()
        {
            //should allow sending custom coordinates

            var coordinates = new Core.Rectangle(121, 31, 110, 151);

            var upResult = m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                CustomCoordinates = coordinates,
                Tags = m_apiTag,
                DisplayName = GetUniquePublicId()
            });

            Assert.IsNotEmpty(upResult.DisplayName);

            var result = m_cloudinary.GetResource(new GetResourceParams(upResult.PublicId)
            {
                Prefix = m_test_prefix,
                NextCursor = "test",
                StartAt = "start",
                Direction = "-1",
                Tags = true,
                Context = true,
                Moderation = true
            });

            Assert.NotNull(result.NextCursor, result.Error?.Message);
            Assert.NotZero(result.Tags.Length);
            Assert.IsNotEmpty(result.DisplayName);
        }

        [Test, RetryWithDelay]
        public void TestUploadVideoCinemagraphAnalysis()
        {
            var uploadResult = UploadTestVideoResource(uploadParams =>
            {
                uploadParams.CinemagraphAnalysis = true;
            });

            Assert.GreaterOrEqual(uploadResult.CinemagraphAnalysis.CinemagraphScore, 0, uploadResult.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestUploadImageCinemagraphAnalysis()
        {
            var uploadResult = UploadTestImageResource(uploadParams =>
            {
                uploadParams.CinemagraphAnalysis = true;
            });

            Assert.GreaterOrEqual(uploadResult.CinemagraphAnalysis.CinemagraphScore, 0, uploadResult.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestUploadImageAccessibilityAnalysis()
        {
            var uploadResult = UploadTestImageResource(uploadParams =>
            {
                uploadParams.AccessibilityAnalysis = true;
            });

            CloudinaryAssert.AccessibilityAnalysisNotEmpty(uploadResult.AccessibilityAnalysis);
        }

        [Test, RetryWithDelay]
        public void TestMetadata()
        {
            var metadataLabel = GetUniqueMetadataFieldLabel("resource_upload");
            var metadataParameters = new StringMetadataFieldCreateParams(metadataLabel);
            var metadataResult = m_cloudinary.AddMetadataField(metadataParameters);

            Assert.NotNull(metadataResult);

            var metadataFieldId = metadataResult.ExternalId;

            Assert.NotNull(metadataFieldId, metadataResult.Error?.Message);

            if (!string.IsNullOrEmpty(metadataFieldId))
                m_metadataFieldsToClear.Add(metadataFieldId);

            const string metadataValue = "test value";
            var metadata = new StringDictionary
            {
                {metadataFieldId, metadataValue}
            };

            var uploadResult = m_cloudinary.Upload(new ImageUploadParams
            {
                File = new FileDescription(m_testImagePath),
                MetadataFields = metadata
            });

            Assert.NotNull(uploadResult);
            Assert.AreEqual(HttpStatusCode.OK, uploadResult.StatusCode, uploadResult.Error?.Message);
            Assert.NotNull(uploadResult.MetadataFields);
        }

        [Test, RetryWithDelay]
        public void TestEvalUploadParameter()
        {
            const string evalStr = "if (resource_info['width'] > 450) " +
                                   "{ upload_options['quality_analysis'] = true }; " +
                                   "upload_options['context'] = 'width = ' + resource_info['width']";

            var uploadResult = UploadTestImageResource(uploadParams =>
            {
                uploadParams.Eval = evalStr;
            });

            Assert.AreEqual("1920", uploadResult.Context["custom"]?["width"]?.ToString());
            Assert.NotNull(uploadResult.QualityAnalysis, uploadResult.Error?.Message);
            Assert.IsInstanceOf<double>(uploadResult.QualityAnalysis.Focus);
        }

        [Test, RetryWithDelay]
        public void TestUploadFilenameOverride()
        {
            const string filenameOverride = "test_overridden";

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag,
                FilenameOverride = filenameOverride
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.IsNotNull(uploadResult);
            Assert.AreEqual(filenameOverride, uploadResult.OriginalFilename, uploadResult.Error?.Message);
        }

        //[Test, RetryWithDelay]
        //public void TestTextAlign()
        //{
        //    TextParams tParams = new TextParams("Sample text.");
        //    tParams.Background = "red";
        //    tParams.FontStyle = "italic";
        //    tParams.TextAlign = "center";

        //    string rString = GetMockBodyOfCloudinaryRequest(tParams, (p, t) => { return p.Text(t); });

        //    StringAssert.Contains("name=\"text_align\"\r\n\r\ncenter\r\n", rString);
        //}

        //[Test, RetryWithDelay]
        //public void TestPostParamsInTheBody()
        //{
        //    TextParams tParams = new TextParams("Sample text.");
        //    tParams.Background = "red";
        //    tParams.FontStyle = "italic";
        //    tParams.TextAlign = "center";

        //    string rString = GetMockBodyOfCloudinaryRequest(tParams, (p, t) =>
        //    {
        //        p.Api.Call(HttpMethod.POST, string.Empty, t.ToParamsDictionary(), null);
        //        return (TextResult)null;
        //    });

        //    StringAssert.Contains("name=\"text_align\"\r\n\r\ncenter\r\n", rString);
        //}

        //[Test, RetryWithDelay]
        //public void TestAllowWriteStreamBufferingSet()
        //{
        //    var largeFilePath = m_testLargeImagePath;
        //    var rawUploadParams = new RawUploadParams() { File = new FileDescription(largeFilePath) };

        //    //check of AllowWriteStreamBuffering option set to false
        //    HttpWebRequest requestDefault = null;
        //    GetMockBodyOfCloudinaryRequest(rawUploadParams, (p, t) => { return p.UploadLarge(t, 5 * 1024 * 1024); }, out requestDefault);
        //    Assert.IsFalse(requestDefault.AllowWriteStreamBuffering);
        //    Assert.IsFalse(requestDefault.AllowAutoRedirect);
        //}

        //[Test, RetryWithDelay]
        //public void TestExplicitInvalidate()
        //{
        //    ExplicitParams exp = new ExplicitParams("cloudinary")
        //    {
        //        EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(2.0) },
        //        Invalidate = true,
        //        Type = "twitter_name"
        //    };

        //    string rString = GetMockBodyOfCloudinaryRequest(exp, (p, t) => { return p.Explicit(t); });
        //    StringAssert.Contains("name=\"invalidate\"\r\n\r\ntrue\r\n", rString);
        //}
    }
}
