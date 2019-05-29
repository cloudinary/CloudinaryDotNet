using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CloudinaryDotNet.Test
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public partial class CloudinaryTest : IntegrationTestBase
    {
        private const string ILLEGAL_STRING = "illegal";
        private const string ILLEGAL_MESSAGE = "Illegal value";
        private const string TEST_IMAGE_PREFIX = "TestImage";

        private const string ACCESS_MODE_PUBLIC = "public";
        private const string MODERATION_MANUAL = "manual";
        private const string MODERATION_AWS_REK = "aws_rek";
        private const string MODERATION_WEBPURIFY = "webpurify";

        private Transformation m_implicitTransformation;

        protected readonly Transformation m_transformationAngleExtended =
            new Transformation().Angle(45).Height(210).Crop("scale");
        protected readonly Transformation m_transformationAr25 = new Transformation().Width(100).AspectRatio(2.5);
        protected readonly Transformation m_transformationAr69 = new Transformation().Width(100).AspectRatio(6, 9);
        protected readonly Transformation m_transformationAr30 = new Transformation().Width(150).AspectRatio("3.0");
        protected readonly Transformation m_transformationAr12 = new Transformation().Width(100).AspectRatio("1:2");
        protected readonly Transformation m_transformationExplode = new Transformation().Page("all");
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
                m_transformationExplode, m_eagerTransformation);
        }

        [Test]
        public void TestUploadLocalImage()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(1920, uploadResult.Width);
            Assert.AreEqual(1200, uploadResult.Height);
            Assert.AreEqual(FILE_FORMAT_JPG, uploadResult.Format);

            var checkParams = new SortedDictionary<string, object>
            {
                { "public_id", uploadResult.PublicId },
                { "version", uploadResult.Version }
            };

            var api = new Api(m_account);
            string expectedSign = api.SignParameters(checkParams);

            Assert.AreEqual(expectedSign, uploadResult.Signature);
        }

        [Test]
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

            Assert.AreEqual(imageFileName, uploadResultImage.PublicId);

            var pdfFileName = GetUniquePublicId(StorageType.upload, FILE_FORMAT_PDF);
            var filePdf = new FileDescription(m_testPdfPath) {FileName = pdfFileName };

            uploadParams.File = filePdf;

            var uploadResultPdf = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(pdfFileName, uploadResultPdf.PublicId);
        }

        [Test]
        public void TestUploadLocalPDFPages()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testPdfPath),
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(FILE_FORMAT_PDF, uploadResult.Format);
            Assert.AreEqual(TEST_PDF_PAGES_COUNT, uploadResult.Pages);
        }

        [Test]
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

        [Test]
        public void TestUploadLocalVideo()
        {
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(m_testVideoPath),
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(640, uploadResult.Width);
            Assert.AreEqual(320, uploadResult.Height);
            Assert.AreEqual(FILE_FORMAT_MP4, uploadResult.Format);
            Assert.NotNull(uploadResult.Audio);
            Assert.AreEqual("aac", uploadResult.Audio.Codec);
            Assert.NotNull(uploadResult.Video);
            Assert.AreEqual("h264", uploadResult.Video.Codec);

            var getResource = new GetResourceParams(uploadResult.PublicId) { ResourceType = ResourceType.Video };
            var info = m_cloudinary.GetResource(getResource);

            Assert.AreEqual(FILE_FORMAT_MP4, info.Format);
        }

        [Test]
        public void TestUploadCustom()
        {
            var video = Api.GetCloudinaryParam(ResourceType.Video);
            var uploadResult = m_cloudinary.Upload(video,
                new Dictionary<string, object> { { "tags", m_apiTag }},
                new FileDescription(m_testVideoPath));

            Assert.NotNull(uploadResult);
            Assert.AreEqual(video, uploadResult.ResourceType);
        }

        [Test]
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
            Assert.NotNull(uploadResult.Moderation);
            Assert.AreEqual(1, uploadResult.Moderation.Count);
            Assert.AreEqual(MODERATION_MANUAL, uploadResult.Moderation[0].Kind);
            Assert.AreEqual(ModerationStatus.Pending, uploadResult.Moderation[0].Status);

            var getResult = m_cloudinary.GetResource(uploadResult.PublicId);

            Assert.NotNull(getResult);
            Assert.NotNull(getResult.Moderation);
            Assert.AreEqual(1, getResult.Moderation.Count);
            Assert.AreEqual(MODERATION_MANUAL, getResult.Moderation[0].Kind);
            Assert.AreEqual(ModerationStatus.Pending, getResult.Moderation[0].Status);
        }

        [Test, IgnoreAddon("aws_rek")]
        public void TestModerationAwsRek()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Moderation = MODERATION_AWS_REK,
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);
            Assert.IsNotNull(uploadResult.Moderation);
            Assert.AreEqual(1, uploadResult.Moderation.Count);
            Assert.AreEqual(ModerationStatus.Approved, uploadResult.Moderation[0].Status);
            Assert.AreEqual(MODERATION_AWS_REK, uploadResult.Moderation[0].Kind);
        }

        [Test, IgnoreAddon("webpurify")]
        public void TestModerationWebpurify()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Moderation = MODERATION_WEBPURIFY,
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);
            Assert.IsNotNull(uploadResult.Moderation);
            Assert.AreEqual(1, uploadResult.Moderation.Count);
            Assert.AreEqual(MODERATION_WEBPURIFY, uploadResult.Moderation[0].Kind);
        }

        [Test]
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

            Assert.AreEqual(HttpStatusCode.BadRequest, updateResult.StatusCode);
            Assert.True(updateResult.Error.Message.StartsWith(ILLEGAL_MESSAGE));
        }

        [Test, IgnoreAddon("adv_ocr")]
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

            Assert.AreEqual(HttpStatusCode.OK, updateResult.StatusCode);

            // OCR sometimes replaces `_`,`-` with space or empty string and adds newline at the end, ignore those
            CloudinaryAssert.StringsAreEqualIgnoringCaseAndChars(
                m_implicitTransformationText,
                updateResult.Info.Ocr.AdvOcr.Data[0].FullTextAnnotation.Text,
                "_- \n");
        }

        [Test]
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

            Assert.AreEqual(HttpStatusCode.BadRequest, updateResult.StatusCode);
            Assert.True(updateResult.Error.Message.StartsWith(ILLEGAL_MESSAGE));
        }

        [Test]
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

            Assert.AreEqual(HttpStatusCode.BadRequest, updateResult.StatusCode);
            Assert.True(updateResult.Error.Message.StartsWith(ILLEGAL_MESSAGE));
        }

        [Test]
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

            Assert.AreEqual(HttpStatusCode.BadRequest, updateResult.StatusCode);
            Assert.True(updateResult.Error.Message.StartsWith(ILLEGAL_MESSAGE));
        }

        [Test, IgnoreAddon("rekognition")]
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

            Assert.IsNull(uploadResult.Info);

            var updateResult = m_cloudinary.UpdateResource(new UpdateParams(uploadResult.PublicId)
            {
                Detection = rekognitionFace
            });

            if (updateResult.Error.Message.StartsWith("You don't have an active"))
            {
                Assert.Ignore(updateResult.Error.Message);
            }

            Assert.NotNull(updateResult.Info);
            Assert.NotNull(updateResult.Info.Detection);
            Assert.NotNull(updateResult.Info.Detection.RekognitionFace);
            Assert.AreEqual(complete, updateResult.Info.Detection.RekognitionFace.Status);

            uploadResult = m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Detection = rekognitionFace,
                Tags = m_apiTag
            });

            Assert.NotNull(uploadResult.Info);
            Assert.NotNull(uploadResult.Info.Detection);
            Assert.NotNull(uploadResult.Info.Detection.RekognitionFace);
            Assert.AreEqual(complete, uploadResult.Info.Detection.RekognitionFace.Status);
        }

        [Test]
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
            Assert.AreEqual(img1.Length, img2.Length);

            uploadParams.Overwrite = true;

            img2 = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(img2);
            Assert.AreNotEqual(img1.Length, img2.Length);
        }

        [Test]
        public void TestUploadLocalImageGetMetadata()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                PublicId = GetUniquePublicId(),
                Metadata = true,
                Exif = true,
                Colors = true,
                Tags = m_apiTag
            };

            ImageUploadResult result = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(result.Metadata);
            Assert.NotNull(result.Exif);
            Assert.NotNull(result.Colors);
        }

        [Test]
        public void TestFaceCoordinates()
        {
            //should allow sending face coordinates

            var faceCoordinates = new List<Core.Rectangle>()
            {
                new Core.Rectangle(121,31,110,151),
                new Core.Rectangle(120,30,109,150)
            };

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                FaceCoordinates = faceCoordinates,
                Faces = true,
                Tags = m_apiTag
            };

            var uploadRes = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(uploadRes.Faces);
            Assert.AreEqual(2, uploadRes.Faces.Length);
            Assert.AreEqual(4, uploadRes.Faces[0].Length);
            for (int i = 0; i < 2; i++)
            {
                Assert.AreEqual(faceCoordinates[i].X, uploadRes.Faces[i][0]);
                Assert.AreEqual(faceCoordinates[i].Y, uploadRes.Faces[i][1]);
                Assert.AreEqual(faceCoordinates[i].Width, uploadRes.Faces[i][2]);
                Assert.AreEqual(faceCoordinates[i].Height, uploadRes.Faces[i][3]);
            }

            var explicitParams = new ExplicitParams(uploadRes.PublicId)
            {
                FaceCoordinates = "122,32,111,152",
                Type = STORAGE_TYPE_UPLOAD,
                Tags = m_apiTag
            };

            m_cloudinary.Explicit(explicitParams);

            var res = m_cloudinary.GetResource(
                new GetResourceParams(uploadRes.PublicId) { Faces = true });

            Assert.NotNull(res.Faces);
            Assert.AreEqual(1, res.Faces.Length);
            Assert.AreEqual(4, res.Faces[0].Length);
            Assert.AreEqual(122, res.Faces[0][0]);
            Assert.AreEqual(32, res.Faces[0][1]);
            Assert.AreEqual(111, res.Faces[0][2]);
            Assert.AreEqual(152, res.Faces[0][3]);
        }

        [Test]
        public void TestQualityAnalysis()
        {
            //should return quality analysis information
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                QualityAnalysis = true,
                Tags = m_apiTag
            };

            var uploadRes = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(uploadRes.QualityAnalysis);
            Assert.IsInstanceOf<double>(uploadRes.QualityAnalysis.Focus);

            var explicitParams = new ExplicitParams(uploadRes.PublicId)
            {
                QualityAnalysis = true,
                Type = STORAGE_TYPE_UPLOAD,
                Tags = m_apiTag
            };

            var explicitResult = m_cloudinary.Explicit(explicitParams);

            Assert.NotNull(explicitResult.QualityAnalysis);
            Assert.IsInstanceOf<double>(explicitResult.QualityAnalysis.Focus);

            var res = m_cloudinary.GetResource(new GetResourceParams(uploadRes.PublicId) { QualityAnalysis = true });

            Assert.NotNull(res.QualityAnalysis);
            Assert.IsInstanceOf<double>(res.QualityAnalysis.Focus);
        }
        [Test]
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

            Assert.True(result.PublicId.StartsWith(TEST_IMAGE_PREFIX));
        }

        [Test]
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

            Assert.AreEqual(TEST_IMAGE_PREFIX, result.PublicId);
        }

        [Test]
        public void TestUploadTransformationResize()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Transformation = m_resizeTransformation,
                Tags = m_apiTag
            };

            ImageUploadResult uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(m_resizeTransformationWidth, uploadResult.Width);
            Assert.AreEqual(m_resizeTransformationHeight, uploadResult.Height);
            Assert.AreEqual(FILE_FORMAT_JPG, uploadResult.Format);
        }

        [Test]
        public void TestEnglishText()
        {
            TextParams tParams = new TextParams("Sample text.")
            {
                Background = "red",
                FontStyle = "italic",
                PublicId = GetUniquePublicId(StorageType.text)
            };

            TextResult textResult = m_cloudinary.Text(tParams);

            Assert.IsTrue(textResult.Width > 0);
            Assert.IsTrue(textResult.Height > 0);
        }

        [Test]
        public void TestRussianText()
        {
            TextParams tParams = new TextParams("Пример текста.")
            {
                PublicId = GetUniquePublicId(StorageType.text)
            };

            TextResult textResult = m_cloudinary.Text(tParams);

            Assert.AreEqual(100, textResult.Width);
            Assert.AreEqual(13, textResult.Height);
        }

        [Test]
        public void TestDestroyRaw()
        {
            RawUploadParams uploadParams = new RawUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            };

            RawUploadResult uploadResult = m_cloudinary.Upload(uploadParams, Api.GetCloudinaryParam(ResourceType.Raw));

            Assert.NotNull(uploadResult);

            DeletionParams destroyParams = new DeletionParams(uploadResult.PublicId)
            {
                ResourceType = ResourceType.Raw
            };

            DeletionResult destroyResult = m_cloudinary.Destroy(destroyParams);

            Assert.AreEqual("ok", destroyResult.Result);
        }

        [Test]
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

            Assert.AreEqual(3381, uploadResult.Length);
            Assert.AreEqual(241, uploadResult.Width);
            Assert.AreEqual(51, uploadResult.Height);
            Assert.AreEqual(FILE_FORMAT_PNG, uploadResult.Format);
        }

        [Test]
        public void TestUploadDataUri()
        {
            var base64Image = "data:image/png;base64,iVBORw0KGgoAA\nAANSUhEUgAAABAAAAAQAQMAAAAlPW0iAAAABlBMVEUAAAD///+l2Z/dAAAAM0l\nEQVR4nGP4/5/h/1+G/58ZDrAz3D/McH8yw83NDDeNGe4Ug9C9zwz3gVLMDA/A6\nP9/AFGGFyjOXZtQAAAAAElFTkSuQmCC";

            var upload = new ImageUploadParams()
            {
                File = new FileDescription(base64Image),
                Tags = m_apiTag
            };

            var result = m_cloudinary.Upload(upload);

            Assert.AreEqual(16, result.Width);
            Assert.AreEqual(16, result.Height);
        }

        [Test]
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

                Assert.AreEqual(1920, uploadResult.Width);
                Assert.AreEqual(1200, uploadResult.Height);
                Assert.AreEqual(FILE_FORMAT_JPG, uploadResult.Format);
            }
        }

        [Test]
        public void TestUploadLargeRawFiles()
        {
            // support uploading large raw files

            var largeFilePath = m_testLargeImagePath;
            int fileLength = (int)new FileInfo(largeFilePath).Length;
            var result = m_cloudinary.UploadLarge(new RawUploadParams()
            {
                File = new FileDescription(largeFilePath),
                Tags = m_apiTag
            }, 5 * 1024 * 1024);

            Assert.AreEqual(fileLength, result.Length);
        }

        [Test]
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

            Assert.AreEqual(fileLength, result.Length);
        }

        /// <summary>
        /// Test access control rules
        /// </summary>
        [Test]
        public void TestUploadAccessControl()
        {
            var start = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = new DateTime(3000, 12, 31, 23, 59, 59, DateTimeKind.Utc);

            var accessControl = new List<AccessControlRule> { new AccessControlRule
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

            Assert.AreEqual(1, uploadResult.AccessControl.Count);

            Assert.AreEqual(AccessType.Anonymous, uploadResult.AccessControl[0].AccessType);
            Assert.AreEqual(start, uploadResult.AccessControl[0].Start);
            Assert.AreEqual(end, uploadResult.AccessControl[0].End);

            uploadParams.AccessControl.Add(new AccessControlRule { AccessType = AccessType.Token });

            uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(2, uploadResult.AccessControl.Count);

            Assert.AreEqual(AccessType.Anonymous, uploadResult.AccessControl[0].AccessType);
            Assert.AreEqual(start, uploadResult.AccessControl[0].Start);
            Assert.AreEqual(end, uploadResult.AccessControl[0].End);

            Assert.AreEqual(AccessType.Token, uploadResult.AccessControl[1].AccessType);
            Assert.IsNull(uploadResult.AccessControl[1].Start);
            Assert.IsNull(uploadResult.AccessControl[1].End);
        }

        [Test]
        public void TestPublishByIds()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{m_apiTag}",
                PublicId = GetUniquePublicId(),
                Overwrite = true,
                Type = STORAGE_TYPE_AUTHENTICATED
            };

            var result = m_cloudinary.Upload(uploadParams);

            var publish_result = m_cloudinary.PublishResourceByIds(null, new PublishResourceParams()
            {
                PublicIds = new List<string> { result.PublicId },
                ResourceType = ResourceType.Image,
                Type = STORAGE_TYPE_AUTHENTICATED
            });

            Assert.NotNull(publish_result.Published);
            Assert.AreEqual(1, publish_result.Published.Count);
        }

        [Test]
        public void TestPublishByPrefix()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{m_apiTag}",
                PublicId = GetUniquePublicId(),
                Overwrite = true,
                Type = STORAGE_TYPE_AUTHENTICATED
            };

            m_cloudinary.Upload(uploadParams);

            var publish_result = m_cloudinary.PublishResourceByPrefix(
                                        uploadParams.PublicId.Substring(0, uploadParams.PublicId.Length - 2), new PublishResourceParams());

            Assert.NotNull(publish_result.Published);
            Assert.AreEqual(1, publish_result.Published.Count);
        }

        [Test]
        public void TestPublishByTag()
        {
            var publishTag = GetMethodTag();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{publishTag},{m_apiTag}",
                PublicId = GetUniquePublicId(),
                Overwrite = true,
                Type = STORAGE_TYPE_PRIVATE
            };

            m_cloudinary.Upload(uploadParams);

            var publish_result = m_cloudinary.PublishResourceByTag(publishTag, new PublishResourceParams()
            {
                ResourceType = ResourceType.Image,
            });

            Assert.NotNull(publish_result.Published);
            Assert.AreEqual(1, publish_result.Published.Count);
        }

        [Test]
        public void TestPublishWithType()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{m_apiTag}",
                PublicId = GetUniquePublicId(),
                Overwrite = true,
                Type = STORAGE_TYPE_AUTHENTICATED
            };

            m_cloudinary.Upload(uploadParams);

            //publish with wrong type - verify publish fails
            var publish_result = m_cloudinary.PublishResourceByIds(null, new PublishResourceParams()
            {
                PublicIds = new List<string> { uploadParams.PublicId },
                ResourceType = ResourceType.Image,
                Type = STORAGE_TYPE_PRIVATE
            });

            Assert.NotNull(publish_result.Published);
            Assert.NotNull(publish_result.Failed);
            Assert.AreEqual(0, publish_result.Published.Count);
            Assert.AreEqual(1, publish_result.Failed.Count);

            //publish with correct type - verify publish succeeds
            publish_result = m_cloudinary.PublishResourceByIds(null, new PublishResourceParams()
            {
                PublicIds = new List<string> { uploadParams.PublicId },
                ResourceType = ResourceType.Image,
                Type = STORAGE_TYPE_AUTHENTICATED
            });

            Assert.NotNull(publish_result.Published);
            Assert.NotNull(publish_result.Failed);
            Assert.AreEqual(1, publish_result.Published.Count);
            Assert.AreEqual(0, publish_result.Failed.Count);
        }

        [Test]
        public void TestUpdateAccessModeByTag()
        {
            var updateTag = GetMethodTag();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{m_apiTag},{updateTag}",
                PublicId = GetUniquePublicId(),
                Overwrite = true,
                Type = STORAGE_TYPE_PRIVATE
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            var update_result = m_cloudinary.UpdateResourceAccessModeByTag(updateTag,
                new UpdateResourceAccessModeParams()
                {
                    ResourceType = ResourceType.Image,
                    Type = STORAGE_TYPE_UPLOAD,
                    AccessMode = ACCESS_MODE_PUBLIC
                });

            //TODO: fix this test, make assertions working

            //Assert.AreEqual(publish_result.Published.Count, 1);
        }

        [Test]
        public void TestUpdateAccessModeById()
        {
            var publicId = GetUniquePublicId();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag,
                PublicId = publicId,
                Overwrite = true,
                Type = STORAGE_TYPE_PRIVATE
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            List<string> ids = new List<string>
            {
                publicId
            };

            var update_result = m_cloudinary.UpdateResourceAccessModeByIds(new UpdateResourceAccessModeParams()
            {
                ResourceType = ResourceType.Image,
                Type = STORAGE_TYPE_UPLOAD,
                AccessMode = ACCESS_MODE_PUBLIC,
                PublicIds = ids
            });

            //TODO: fix this test, make assertions working

            //Assert.AreEqual(publish_result.Published.Count, 1);
        }

        /// <summary>
        /// Test that we can update access control of the resource
        /// </summary>
        [Test]
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

            Assert.AreEqual(1, uploadResult.AccessControl.Count);

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

            Assert.AreEqual(1, updateResult.AccessControl.Count);

            Assert.AreEqual(AccessType.Token, updateResult.AccessControl[0].AccessType);
            Assert.AreEqual(end, updateResult.AccessControl[0].Start);
            Assert.AreEqual(start, updateResult.AccessControl[0].End);
        }

        [Test]
        public void TestUploadLargeVideoFromWeb()
        {
            // support uploading large video
            var result = m_cloudinary.UploadLarge(new VideoUploadParams()
            {
                File = new FileDescription(TEST_REMOTE_VIDEO),
                Tags = m_apiTag
            }, 5 * 1024 * 1024);

            Assert.AreEqual(result.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.AreEqual(result.Format, FILE_FORMAT_MP4);
        }

        [Test]
        public void TestUploadLargeImageFromWeb()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(TEST_REMOTE_IMG),
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.UploadLarge(uploadParams);

            Assert.AreEqual(uploadResult.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(FILE_FORMAT_PNG, uploadResult.Format);
        }

        [Test]
        public void TestTagAdd()
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
                Command = TagCommand.Add,
                Tag = tag
            };

            tagParams.PublicIds.Add(uploadResult.PublicId);

            TagResult tagResult = m_cloudinary.Tag(tagParams);

            Assert.AreEqual(1, tagResult.PublicIds.Length);
            Assert.AreEqual(uploadResult.PublicId, tagResult.PublicIds[0]);
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
        public void TestListResourceTypes()
        {
            // should allow listing resource_types
            ListResourceTypesResult result = m_cloudinary.ListResourceTypes();
            Assert.Contains(ResourceType.Image, result.ResourceTypes);
        }

        [Test]
        public void TestListResources()
        {
            // should allow listing resources

            ListResourcesResult resources = m_cloudinary.ListResources();
            Assert.NotNull(resources);
        }

        [Test, Ignore("test needs to be re-written with mocking - it fails when there are many resources")]
        public void TestListResourcesByType()
        {
            // should allow listing resources by type

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = GetUniquePublicId(),
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            IEnumerable<Resource> result = GetAllResults((cursor) => m_cloudinary.ListResourcesByType(STORAGE_TYPE_UPLOAD, cursor));

            Assert.IsNotEmpty(result.Where(res => res.Type == STORAGE_TYPE_UPLOAD));
            Assert.IsEmpty(result.Where(res => res.Type != STORAGE_TYPE_UPLOAD));
        }

        [Test]
        public void TestListResourcesByPrefix()
        {
            // should allow listing resources by prefix
            var publicId = GetUniquePublicId();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Context = new StringDictionary("context=abc"),
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            var result = m_cloudinary.ListResourcesByPrefix(publicId, true, true, true);

            //Assert.IsTrue(result.Resources.Where(res => res.PublicId.StartsWith("testlist")).Count() == result.Resources.Count());
            Assert.IsTrue(
                result
                .Resources
                .Where(res => (res.Context == null ? false : res.Context["custom"]["context"].ToString() == "abc"))
                .Count() > 0);
        }

        [Test]
        public void TestContextEscaping()
        {
            var context = new StringDictionary();
            context.Add("key", "val=ue");

            var uploadParams = new ImageUploadParams { Context=context };
            Assert.AreEqual(@"key=val\=ue", uploadParams.ToParamsDictionary()["context"]);

            context.Add(@"hello=world|2", "goodbye|wo=rld2");

            var contextParams = new ContextParams()
            {
                Context = @"val\=ue",
                ContextDict = context
            };

            Assert.AreEqual(@"key=val\=ue|hello\=world\|2=goodbye\|wo\=rld2|val\=ue", contextParams.ToParamsDictionary()["context"]);
        }

        [Test, Ignore("test needs to be re-written with mocking - it fails when there are many resources")]
        public void TestResourcesListingDirection()
        {
            // should allow listing resources in both directions

            var result = m_cloudinary.ListResources(new ListResourcesByPrefixParams()
            {
                Type = STORAGE_TYPE_UPLOAD,
                MaxResults = MAX_RESULTS,
                Direction = "asc"
            });

            var list1 = result.Resources.Select(r => r.PublicId).ToArray();

            result = m_cloudinary.ListResources(new ListResourcesByPrefixParams()
            {
                Type = STORAGE_TYPE_UPLOAD,
                MaxResults = MAX_RESULTS,
                Direction = "-1"
            });

            var list2 = result.Resources.Select(r => r.PublicId).Reverse().ToArray();

            Assert.AreEqual(list1.Length, list2.Length);
            for (int i = 0; i < list1.Length; i++)
            {
                Assert.AreEqual(list1[i], list2[i]);
            }
        }

        [Test]
        public void TestContext()
        {
            //should allow sending context

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = GetUniquePublicId(),
                Context = new StringDictionary("key=value", "key2=value2"),
                Tags = m_apiTag
            };

            var uploaded = m_cloudinary.Upload(uploadParams);

            var res = m_cloudinary.GetResource(uploaded.PublicId);

            Assert.AreEqual("value", res.Context["custom"]["key"].ToString());
            Assert.AreEqual("value2", res.Context["custom"]["key2"].ToString());
        }

        [Test]
        public void TestListResourcesByPublicIds()
        {
            var publicId1 = GetUniquePublicId();
            var publicId2 = GetUniquePublicId();
            var context = new StringDictionary("key=value", "key2=value2");

            // should allow listing resources by public ids
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId1,
                Context = context,
                Tags = m_apiTag
            };
            m_cloudinary.Upload(uploadParams);

            uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId2,
                Context = context,
                Tags = m_apiTag
            };
            m_cloudinary.Upload(uploadParams);

            List<string> publicIds = new List<string>()
                {
                    publicId1,
                    publicId2
                };
            var result = m_cloudinary.ListResourceByPublicIds(publicIds, true, true, true);

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Resources.Length, "expected to find {0} but got {1}", new Object[] { publicIds.Aggregate((current, next) => current + ", " + next), result.Resources.Select(r => r.PublicId).Aggregate((current, next) => current + ", " + next) });
            Assert.True(result.Resources.Where(r => r.Context != null).Count() == 2);
        }

        [Test]
        public void TestListResourcesByTag()
        {
            // should allow listing resources by tag
            var localTag = GetMethodTag();

            var file = new FileDescription(m_testImagePath);
            m_cloudinary.DeleteResourcesByTag(localTag);
            var uploadParams = new ImageUploadParams()
            {
                File = file,
                Tags = $"{m_apiTag},{localTag}"
            };

            m_cloudinary.Upload(uploadParams);

            uploadParams = new ImageUploadParams()
            {
                File = file,
                Tags = $"{m_apiTag},{localTag}"
            };

            m_cloudinary.Upload(uploadParams);
            var result = m_cloudinary.ListResourcesByTag(localTag);
            Assert.AreEqual(2, result.Resources.Count());
        }

        [Test]
        public void TestListByModerationUpdate()
        {
            // should support listing by moderation kind and value

            List<ImageUploadResult> uploadResults = new List<ImageUploadResult>();

            for (int i = 0; i < 3; i++)
            {
                uploadResults.Add(m_cloudinary.Upload(new ImageUploadParams()
                {
                    File = new FileDescription(m_testImagePath),
                    Moderation = MODERATION_MANUAL,
                    Tags = m_apiTag
                }));
            }

            m_cloudinary.UpdateResource(uploadResults[0].PublicId, ModerationStatus.Approved);
            m_cloudinary.UpdateResource(uploadResults[1].PublicId, ModerationStatus.Rejected);

            var requestParams = new ListResourcesByModerationParams()
            {
                MaxResults = MAX_RESULTS,
                ModerationKind = MODERATION_MANUAL,
            };

            requestParams.ModerationStatus = ModerationStatus.Approved;
            var approved = m_cloudinary.ListResources(requestParams);

            requestParams.ModerationStatus = ModerationStatus.Rejected;
            var rejected = m_cloudinary.ListResources(requestParams);

            requestParams.ModerationStatus = ModerationStatus.Pending;
            var pending = m_cloudinary.ListResources(requestParams);

            Assert.True(approved.Resources.Where(r => r.PublicId == uploadResults[0].PublicId).Count() > 0);
            Assert.True(approved.Resources.Where(r => r.PublicId == uploadResults[1].PublicId).Count() == 0);
            Assert.True(approved.Resources.Where(r => r.PublicId == uploadResults[2].PublicId).Count() == 0);

            Assert.True(rejected.Resources.Where(r => r.PublicId == uploadResults[0].PublicId).Count() == 0);
            Assert.True(rejected.Resources.Where(r => r.PublicId == uploadResults[1].PublicId).Count() > 0);
            Assert.True(rejected.Resources.Where(r => r.PublicId == uploadResults[2].PublicId).Count() == 0);

            Assert.True(pending.Resources.Where(r => r.PublicId == uploadResults[0].PublicId).Count() == 0);
            Assert.True(pending.Resources.Where(r => r.PublicId == uploadResults[1].PublicId).Count() == 0);
            Assert.True(pending.Resources.Where(r => r.PublicId == uploadResults[2].PublicId).Count() > 0);
        }

        [Test]
        public void TestResourcesCursor()
        {
            // should allow listing resources with cursor

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = GetUniquePublicId(),
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = GetUniquePublicId(),
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            var listParams = new ListResourcesParams()
            {
                ResourceType = ResourceType.Image,
                MaxResults = 1
            };

            var result1 = m_cloudinary.ListResources(listParams);

            Assert.IsNotNull(result1.Resources);
            Assert.AreEqual(1, result1.Resources.Length);
            Assert.IsFalse(String.IsNullOrEmpty(result1.NextCursor));

            listParams.NextCursor = result1.NextCursor;
            var result2 = m_cloudinary.ListResources(listParams);

            Assert.IsNotNull(result2.Resources);
            Assert.AreEqual(1, result2.Resources.Length);
            Assert.AreNotEqual(result1.Resources[0].PublicId, result2.Resources[0].PublicId);
        }

        [Test]
        public void TestResourceFullyQualifiedPublicId()
        {
            // should return correct FullyQualifiedPublicId

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = GetUniquePublicId(),
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            var listParams = new ListResourcesParams()
            {
                ResourceType = ResourceType.Image,
                MaxResults = 1
            };

            var result = m_cloudinary.ListResources(listParams);

            Assert.IsNotNull(result.Resources);
            Assert.AreEqual(1, result.Resources.Length);

            var res = result.Resources[0];
            var expectedFullQualifiedPublicId = $"{res.ResourceType}/{res.Type}/{res.PublicId}";

            Assert.AreEqual(expectedFullQualifiedPublicId, res.FullyQualifiedPublicId);
        }

        [Test]
        public void TestEager()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation, m_eagerTransformation },
                Tags = $"{m_apiTag},{GetMethodTag()}"
            };

            var result = m_cloudinary.Upload(uploadParams);
            //TODO: fix this test, implement assertions
        }

        [Test]
        public void TestRename()
        {
            var toPublicId = GetUniquePublicId();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            };
            var uploadResult1 = m_cloudinary.Upload(uploadParams);

            uploadParams.File = new FileDescription(m_testIconPath);
            var uploadResult2 = m_cloudinary.Upload(uploadParams);

            var renameResult = m_cloudinary.Rename(uploadResult1.PublicId, toPublicId);

            var getResult = m_cloudinary.GetResource(toPublicId);
            Assert.NotNull(getResult);

            renameResult = m_cloudinary.Rename(uploadResult2.PublicId, toPublicId);
            Assert.True(renameResult.StatusCode == HttpStatusCode.BadRequest);

            m_cloudinary.Rename(uploadResult2.PublicId, toPublicId, true);

            getResult = m_cloudinary.GetResource(toPublicId);
            Assert.NotNull(getResult);
            Assert.AreEqual(FILE_FORMAT_ICO, getResult.Format);
        }

        [Test]
        public void TestRenameToType()

        {
            string publicId = GetUniquePublicId();
            string newPublicId = GetUniquePublicId();

            var uploadParams = new ImageUploadParams()
            {
                PublicId = publicId,
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag,
                Type = STORAGE_TYPE_UPLOAD
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);
            Assert.AreEqual(uploadResult.StatusCode, HttpStatusCode.OK);

            RenameParams renameParams = new RenameParams(publicId, newPublicId)
            {
                ToType = STORAGE_TYPE_UPLOAD
            };

            var renameResult = m_cloudinary.Rename(renameParams);
            Assert.AreEqual(renameResult.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(renameResult.Type, STORAGE_TYPE_UPLOAD);
            Assert.AreEqual(renameResult.PublicId, newPublicId);
        }

        [Test]
        public void TestGetResource()
        {
            // should allow get resource details
            var publicId = GetUniquePublicId();
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                PublicId = publicId,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            GetResourceResult getResult = m_cloudinary.GetResource(
                new GetResourceParams(publicId) { Phash = true });

            Assert.IsNotNull(getResult);
            Assert.AreEqual(publicId, getResult.PublicId);
            Assert.AreEqual(1920, getResult.Width);
            Assert.AreEqual(1200, getResult.Height);
            Assert.AreEqual(FILE_FORMAT_JPG, getResult.Format);
            Assert.AreEqual(1, getResult.Derived.Length);
            Assert.Null(getResult.Metadata);
            Assert.NotNull(getResult.Phash);
        }

        [Test]
        public void TestGetResourceWithMetadata()
        {
            // should allow get resource metadata
            var publicId = GetUniquePublicId();

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                PublicId = publicId,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            GetResourceResult getResult = m_cloudinary.GetResource(
                new GetResourceParams(publicId)
                {
                    Metadata = true
                });

            Assert.IsNotNull(getResult);
            Assert.AreEqual(publicId, getResult.PublicId);
            Assert.NotNull(getResult.Metadata);
        }

        [Test]
        public void TestGetPdfResourceWithNumberOfPages()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testPdfPath),
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(FILE_FORMAT_PDF, uploadResult.Format);
            Assert.AreEqual(TEST_PDF_PAGES_COUNT, uploadResult.Pages);

            GetResourceResult getResult = m_cloudinary.GetResource(
                new GetResourceParams(uploadResult.PublicId)
                {
                    Metadata = true,
                    Pages = true
                });

            Assert.IsNotNull(getResult);
            Assert.AreEqual(uploadResult.PublicId, getResult.PublicId);
            Assert.NotNull(getResult.Metadata);
            Assert.AreEqual(uploadResult.Pages, getResult.Pages);
            Assert.AreEqual(getResult.Pages, TEST_PDF_PAGES_COUNT);
        }

        [Test]
        public void TestDeleteDerived()
        {
            // should allow deleting derived resource
            var publicId = GetUniquePublicId();

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                PublicId = publicId,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            GetResourceResult resource = m_cloudinary.GetResource(publicId);

            Assert.IsNotNull(resource);
            Assert.AreEqual(1, resource.Derived.Length);

            DelDerivedResResult delDerivedResult = m_cloudinary.DeleteDerivedResources(resource.Derived[0].Id);
            Assert.AreEqual(1, delDerivedResult.Deleted.Values.Count);

            resource = m_cloudinary.GetResource(publicId);
            Assert.IsFalse(String.IsNullOrEmpty(resource.PublicId));
        }

        [Test]
        public void TestDelete()
        {
            // should allow deleting resources
            var publicId = GetUniquePublicId();
            var nonExistingPublicId = GetUniquePublicId();

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            GetResourceResult resource = m_cloudinary.GetResource(publicId);

            Assert.IsNotNull(resource);
            Assert.AreEqual(publicId, resource.PublicId);

            DelResResult delResult = m_cloudinary.DeleteResources(
                nonExistingPublicId, publicId);

            Assert.AreEqual("not_found", delResult.Deleted[nonExistingPublicId]);
            Assert.AreEqual("deleted", delResult.Deleted[publicId]);

            resource = m_cloudinary.GetResource(publicId);

            Assert.IsTrue(String.IsNullOrEmpty(resource.PublicId));
        }

        [Test]
        public void TestDeleteByTransformation()
        {
            // should allow deleting resources by transformations
            var publicId = GetUniquePublicId();

            var transformations = new List<Transformation>
            {
                m_simpleTransformation,
                m_simpleTransformationAngle,
                m_explicitTransformation
            };

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = m_apiTag,
                EagerTransforms = transformations
            };

            m_cloudinary.Upload(uploadParams);

            var resource = m_cloudinary.GetResource(publicId);

            Assert.IsNotNull(resource);
            Assert.AreEqual(3, resource.Derived.Length);

            var delParams = new DelResParams {Transformations = transformations};
            delParams.PublicIds.Add(publicId);

            DelResResult delResult = m_cloudinary.DeleteResources(delParams);
            Assert.IsNotNull(delResult.Deleted);
            Assert.AreEqual(1, delResult.Deleted.Count);

            resource = m_cloudinary.GetResource(publicId);
            Assert.IsNotNull(resource);
            Assert.AreEqual(resource.Derived.Length, 0);
        }

        [Test]
        public void TestDeleteByPrefix()
        {
            // should allow deleting resources
            var publicId = GetUniquePublicId();
            var prefix = publicId.Substring(0, publicId.Length - 1);

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = m_apiTag
            };
            m_cloudinary.Upload(uploadParams);

            GetResourceResult resource = m_cloudinary.GetResource(publicId);
            Assert.IsNotNull(resource);
            Assert.AreEqual(publicId, resource.PublicId);

            m_cloudinary.DeleteResourcesByPrefix(prefix);
            resource = m_cloudinary.GetResource(publicId);
            Assert.IsTrue(String.IsNullOrEmpty(resource.PublicId));
        }

        [Test]
        public void TestDeleteByPrefixAndTransformation()
        {
            // should allow deleting resources
            var publicId = GetUniquePublicId();
            var prefix = publicId.Substring(0, publicId.Length - 1);
            var transformations = new List<Transformation>
            {
                m_simpleTransformation,
                m_simpleTransformationAngle,
                m_explicitTransformation
            };

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = m_apiTag,
                EagerTransforms = transformations
            };
            m_cloudinary.Upload(uploadParams);

            GetResourceResult resource = m_cloudinary.GetResource(publicId);
            Assert.IsNotNull(resource);
            Assert.AreEqual(3, resource.Derived.Length);

            var delResult = m_cloudinary.DeleteResources(new DelResParams
            {
                Prefix = prefix,
                Transformations = transformations
            });
            Assert.NotNull(delResult.Deleted);
            Assert.AreEqual(delResult.Deleted.Count, 1);

            resource = m_cloudinary.GetResource(publicId);
            Assert.IsNotNull(resource);
            Assert.AreEqual(resource.Derived.Length, 0);
        }

        [Test]
        public void TestDeleteByTag()
        {
            // should allow deleting resources
            var publicId = GetUniquePublicId();
            var tag = GetMethodTag();

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = $"{tag},{m_apiTag}"
            };

            m_cloudinary.Upload(uploadParams);

            GetResourceResult resource = m_cloudinary.GetResource(publicId);

            Assert.IsNotNull(resource);
            Assert.AreEqual(publicId, resource.PublicId);

            DelResResult delResult = m_cloudinary.DeleteResourcesByTag(tag);

            resource = m_cloudinary.GetResource(publicId);
            Assert.IsTrue(String.IsNullOrEmpty(resource.PublicId));
        }

        [Test]
        public void TestDeleteByTagAndTransformation()
        {
            // should allow deleting resources
            string publicId = GetUniquePublicId();
            string tag = GetMethodTag();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = tag,
                EagerTransforms = new List<Transformation>()
                {
                    m_simpleTransformation,
                    m_simpleTransformationAngle,
                    m_explicitTransformation
                },
            };

            m_cloudinary.Upload(uploadParams);

            DelResResult delResult = m_cloudinary.DeleteResources(new DelResParams
            {
                Tag = tag,
                Transformations = new List<Transformation> {m_simpleTransformation}
            });

            Assert.NotNull(delResult.Deleted);
            Assert.AreEqual(delResult.Deleted.Count, 1);

            delResult = m_cloudinary.DeleteResources(new DelResParams
            {
                Tag = tag,
                Transformations = new List<Transformation>() { m_simpleTransformationAngle, m_explicitTransformation }
            });

            Assert.NotNull(delResult.Deleted);

            GetResourceResult resource = m_cloudinary.GetResource(publicId);
            Assert.IsNotNull(resource);
            Assert.AreEqual(resource.Derived.Length, 0);
        }

        [Test]
        public void TestRestoreNoBackup()
        {
            string publicId = GetUniquePublicId();

            ImageUploadParams uploadParams_nobackup = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams_nobackup);
            GetResourceResult resource = m_cloudinary.GetResource(publicId);
            Assert.IsNotNull(resource);
            Assert.AreEqual(publicId, resource.PublicId);

            DelResResult delResult = m_cloudinary.DeleteResources(publicId);
            Assert.AreEqual("deleted", delResult.Deleted[publicId]);

            resource = m_cloudinary.GetResource(publicId);
            Assert.IsTrue(string.IsNullOrEmpty(resource.PublicId));

            RestoreResult rResult = m_cloudinary.Restore(publicId);
            Assert.IsNotNull(rResult.JsonObj[publicId], string.Format("Should contain key \"{0}\". ", publicId));
            Assert.AreEqual("no_backup", rResult.JsonObj[publicId]["error"].ToString());
        }

        [Test]
        public void TestRestore()
        {
            string publicId = GetUniquePublicId();

            ImageUploadParams uploadParams_backup = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Backup = true,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams_backup);
            GetResourceResult resource_backup = m_cloudinary.GetResource(publicId);
            Assert.IsNotNull(resource_backup);
            Assert.AreEqual(publicId, resource_backup.PublicId);

            DelResResult delResult_backup = m_cloudinary.DeleteResources(publicId);
            Assert.AreEqual("deleted", delResult_backup.Deleted[publicId]);

            resource_backup = m_cloudinary.GetResource(publicId);
            Assert.AreEqual(0, resource_backup.Length);

            RestoreResult rResult_backup = m_cloudinary.Restore(publicId);
            Assert.IsNotNull(rResult_backup.JsonObj[publicId], string.Format("Should contain key \"{0}\". ", publicId));
            Assert.AreEqual(publicId, rResult_backup.JsonObj[publicId]["public_id"].ToString());

            resource_backup = m_cloudinary.GetResource(publicId);
            Assert.IsFalse(string.IsNullOrEmpty(resource_backup.PublicId));
        }

        [Test]
        public void TestListTags()
        {
            // should allow listing tags
            UploadTestResource();

            ListTagsResult result = m_cloudinary.ListTags(new ListTagsParams() );
            Assert.Greater(result.Tags.Length, 0);
        }

        [Test]
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

            Assert.AreEqual(FILE_FORMAT_JPG, res.Format);
        }

        [Test]
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

            Assert.AreEqual(HttpStatusCode.BadRequest, res.StatusCode);
        }

        [Test]
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

            Assert.AreEqual(FILE_FORMAT_PNG, res.Format);
        }

        [Test]
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
            Assert.NotNull(updateResult.Moderation);
            Assert.AreEqual(1, updateResult.Moderation.Count);
            Assert.AreEqual(ModerationStatus.Approved, updateResult.Moderation[0].Status);
        }

        // Test disabled because it deletes all images in the remote account.
        [Test, Ignore("will delete all resources in the account")]
        public void DeleteAllInLoop()
        {
            string nextCursor = String.Empty;

            do
            {
                var response = m_cloudinary.ListUploadPresets(nextCursor);
                nextCursor = response.NextCursor;

                foreach (var preset in response.Presets)
                {
                    m_cloudinary.DeleteUploadPreset(preset.Name);
                }
            } while (!String.IsNullOrEmpty(nextCursor));

            HashSet<string> types = new HashSet<string>();

            do
            {
                var listParams = new ListResourcesParams()
                {
                    NextCursor = nextCursor,
                    MaxResults = MAX_RESULTS
                };

                var existingResources = m_cloudinary.ListResources(listParams);
                nextCursor = existingResources.NextCursor;

                foreach (var res in existingResources.Resources)
                {
                    types.Add(res.Type);
                }
            } while (!String.IsNullOrEmpty(nextCursor));

            foreach (var type in types)
            {
                var deleteParams = new DelResParams() { Type = type, All = true };

                m_cloudinary.DeleteResources(deleteParams);
            }
        }

        [Test]
        public void TestListTagsPrefix()
        {
            // should allow listing tag by prefix
            var tag1 = $"{GetMethodTag()}_1";
            var tag2 = $"{GetMethodTag()}_2"; ;
            var tag3 = $"{GetMethodTag()}_3"; ;

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{tag1},{m_apiTag}"
            };

            m_cloudinary.Upload(uploadParams);

            uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{tag2},{m_apiTag}"
            };

            m_cloudinary.Upload(uploadParams);

            ListTagsResult result = m_cloudinary.ListTagsByPrefix(m_apiTag);

            Assert.Contains(tag2, result.Tags);

            result = m_cloudinary.ListTagsByPrefix(tag3);

            Assert.IsTrue(result.Tags.Length == 0);
        }

        [Test]
        public void TestListTransformations()
        {
            // should allow listing transformations

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            ListTransformsResult result = m_cloudinary.ListTransformations();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Transformations);
            TransformDesc td = result.Transformations.Where(t => t.Name == m_simpleTransformationAsString).First();
            Assert.IsFalse(td.Named);
            Assert.IsTrue(td.Used);
        }

        [Test]
        public void TestGetTransform()
        {
            // should allow getting transformation metadata

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_updateTransformation },
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            var result = m_cloudinary.GetTransform(m_updateTransformationAsString);

            Assert.IsNotNull(result);
            Assert.AreEqual(m_updateTransformation.ToString(), new Transformation(result.Info[0]).ToString());
        }

        [Test]
        public void TestUpdateTransformStrict()
        {
            // should allow updating transformation allowed_for_strict

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            UpdateTransformParams updateParams = new UpdateTransformParams()
            {
                Transformation = m_simpleTransformationAsString,
                Strict = true
            };

            m_cloudinary.UpdateTransform(updateParams);

            GetTransformResult getResult = m_cloudinary.GetTransform(m_simpleTransformationAsString);

            Assert.IsNotNull(getResult);
            Assert.AreEqual(true, getResult.Strict);

            updateParams.Strict = false;
            m_cloudinary.UpdateTransform(updateParams);

            getResult = m_cloudinary.GetTransform(m_simpleTransformationAsString);

            Assert.IsNotNull(getResult);
            Assert.AreEqual(false, getResult.Strict);
        }

        [Test]
        public void TestUpdateTransformUnsafe()
        {
            string transformationName = GetUniqueTransformationName();
            // should allow unsafe update of named transformation
            m_cloudinary.CreateTransform(
                new CreateTransformParams()
                {
                    Name = transformationName,
                    Transform = m_simpleTransformation
                });

            var updateParams = new UpdateTransformParams()
            {
                Transformation = transformationName,
                UnsafeTransform = m_updateTransformation
            };

            m_cloudinary.UpdateTransform(updateParams);

            var getResult = m_cloudinary.GetTransform(transformationName);

            Assert.IsNotNull(getResult.Info);
            Assert.IsTrue(getResult.Named);
            Assert.AreEqual(updateParams.UnsafeTransform.Generate(), new Transformation(getResult.Info).Generate());
        }

        [Test]
        public void TestCreateTransform()
        {
            // should allow creating named transformation

            CreateTransformParams create = new CreateTransformParams()
            {
                Name = GetUniqueTransformationName(),
                Transform = m_simpleTransformation
            };

            m_cloudinary.CreateTransform(create);

            GetTransformParams get = new GetTransformParams()
            {
                Transformation = create.Name
            };

            GetTransformResult getResult = m_cloudinary.GetTransform(get);

            Assert.IsNotNull(getResult);
            Assert.AreEqual(true, getResult.Strict);
            Assert.AreEqual(false, getResult.Used);
            Assert.AreEqual(1, getResult.Info.Length);
            Assert.AreEqual(m_simpleTransformation.Generate(), new Transformation(getResult.Info[0]).Generate());
        }

        [Test]
        public void TestDeleteTransform()
        {
            // should allow deleting named transformation
            string transformationName = GetUniqueTransformationName();

            m_cloudinary.DeleteTransform(transformationName);

            CreateTransformParams create = new CreateTransformParams()
            {
                Name = transformationName,
                Transform = m_simpleTransformation
            };

            TransformResult createResult = m_cloudinary.CreateTransform(create);

            Assert.AreEqual("created", createResult.Message);

            m_cloudinary.DeleteTransform(transformationName);

            GetTransformResult getResult = m_cloudinary.GetTransform(
                new GetTransformParams() { Transformation = transformationName });

            Assert.AreEqual(HttpStatusCode.NotFound, getResult.StatusCode);
        }

        [Test]
        public void TestDeleteTransformImplicit()
        {
            // should allow deleting implicit transformation
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_implicitTransformation },
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            GetTransformParams getParams = new GetTransformParams()
            {
                Transformation = m_implicitTransformation.ToString()
            };

            GetTransformResult getResult = m_cloudinary.GetTransform(getParams);

            Assert.AreEqual(HttpStatusCode.OK, getResult.StatusCode);

            m_cloudinary.DeleteTransform(m_implicitTransformation.ToString());

            getResult = m_cloudinary.GetTransform(getParams);

            Assert.AreEqual(HttpStatusCode.NotFound, getResult.StatusCode);
        }

        [Test]
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


        //[Test]
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

        //[Test]
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

        [Test]
        public void TestExplicit()
        {
            string facebook = StorageType.facebook.ToString();
            string cloudinary = "cloudinary";
            ExplicitParams exp = new ExplicitParams(cloudinary)
            {
                EagerTransforms = new List<Transformation>() { m_explicitTransformation },
                Type = facebook,
                Tags = m_apiTag
            };

            ExplicitResult expResult = m_cloudinary.Explicit(exp);

            string url = new Url(m_account.Cloud).ResourceType(Api.GetCloudinaryParam(ResourceType.Image)).Add(facebook).
                Transform(m_explicitTransformation).
                Format(FILE_FORMAT_PNG).Version(expResult.Version).BuildUrl(cloudinary);

            Assert.AreEqual(url, expResult.Eager[0].Uri.AbsoluteUri);
        }

        [Test]
        public void TestExplicitContext()
        {
            string facebook = StorageType.facebook.ToString();

            var exp = new ExplicitParams("cloudinary")
            {
                EagerTransforms = new List<Transformation>() { m_explicitTransformation },
                Type = facebook,
                Context = new StringDictionary("context1=254"),
                Tags = m_apiTag
            };

            var expResult = m_cloudinary.Explicit(exp);

            Assert.IsNotNull(expResult);

            var getResult = m_cloudinary.GetResource(new GetResourceParams(expResult.PublicId) { Type = facebook });

            Assert.IsNotNull(getResult);
            Assert.AreEqual("254", getResult.Context["custom"]["context1"].ToString());
        }

        /// <summary>
        /// Test asynchronous processing in explicit API calls
        /// </summary>
        [Test]
        public void TestExplicitAsyncProcessing()
        {
            string publicId = GetUniquePublicId();
            string facebook = StorageType.facebook.ToString();

            ExplicitParams exp = new ExplicitParams(publicId)
            {
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(2.0) },
                Type = facebook,
                Async = true,
            };

            ExplicitResult expAsyncResult = m_cloudinary.Explicit(exp);

            Assert.AreEqual("pending", expAsyncResult.Status);
            Assert.AreEqual(Api.GetCloudinaryParam(ResourceType.Image), expAsyncResult.ResourceType);
            Assert.AreEqual(facebook, expAsyncResult.Type);
            Assert.AreEqual(publicId, expAsyncResult.PublicId);
        }

        [Test]
        public void TestExplicitVideo()
        {
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(m_testVideoPath),
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            var exp = new ExplicitParams(uploadResult.PublicId)
            {
                Type = "upload",
                ResourceType = ResourceType.Video,
                Context = new StringDictionary("context1=254")
            };

            var expResult = m_cloudinary.Explicit(exp);

            Assert.IsNotNull(expResult);

            var getResult = m_cloudinary.GetResource(new GetResourceParams(expResult.PublicId) { ResourceType = ResourceType.Video});

            Assert.IsNotNull(getResult);
            Assert.AreEqual("254", getResult.Context["custom"]["context1"].ToString());
        }

        [Test]
        public void TestSprite()
        {
            var publicId1 = GetUniquePublicId(StorageType.sprite);
            var publicId2 = GetUniquePublicId(StorageType.sprite);
            var publicId3 = GetUniquePublicId(StorageType.sprite);

            var spriteTag = GetMethodTag();

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{spriteTag},{m_apiTag}",
                PublicId = publicId1,
                Transformation = m_resizeTransformation
            };
            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = publicId2;
            uploadParams.Transformation = m_updateTransformation;
            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = publicId3;
            uploadParams.Transformation = m_simpleTransformation;
            m_cloudinary.Upload(uploadParams);

            SpriteParams sprite = new SpriteParams(spriteTag)
            {
                Format = FILE_FORMAT_JPG
            };

            SpriteResult result = m_cloudinary.MakeSprite(sprite);
            AddCreatedPublicId(StorageType.sprite, result.PublicId);

            Assert.NotNull(result);
            Assert.NotNull(result.ImageInfos);
            Assert.AreEqual(3, result.ImageInfos.Count);

            StringAssert.EndsWith(FILE_FORMAT_JPG, result.ImageUri.ToString());

            Assert.Contains(publicId1, result.ImageInfos.Keys);
            Assert.Contains(publicId2, result.ImageInfos.Keys);
            Assert.Contains(publicId3, result.ImageInfos.Keys);
        }

        [Test]
        public void TestSpriteTransformation()
        {
            var publicId1 = GetUniquePublicId(StorageType.sprite);
            var publicId2 = GetUniquePublicId(StorageType.sprite);
            var publicId3 = GetUniquePublicId(StorageType.sprite);

            var spriteTag = GetMethodTag();

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{spriteTag},{m_apiTag}",
                PublicId = publicId1,
                Transformation = m_simpleTransformation
            };
            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = publicId2;
            uploadParams.Transformation = m_updateTransformation;
            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = publicId3;
            uploadParams.Transformation = m_explicitTransformation;
            m_cloudinary.Upload(uploadParams);

            SpriteParams sprite = new SpriteParams(spriteTag)
            {
                Transformation = m_resizeTransformation
            };

            SpriteResult result = m_cloudinary.MakeSprite(sprite);
            AddCreatedPublicId(StorageType.sprite, result.PublicId);

            Assert.NotNull(result);
            Assert.NotNull(result.ImageInfos);
            foreach (var item in result.ImageInfos)
            {
                Assert.AreEqual(m_resizeTransformationWidth, item.Value.Width);
                Assert.AreEqual(m_resizeTransformationHeight, item.Value.Height);
            }
        }

        [Test]
        public void TestJsonObject()
        {
            ExplicitParams exp = new ExplicitParams("cloudinary")
            {
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                Type = StorageType.facebook.ToString(),
                Tags = m_apiTag
            };

            var result = m_cloudinary.Explicit(exp);
            AddCreatedPublicId(StorageType.facebook, result.PublicId);

            Assert.NotNull(result.JsonObj);
            Assert.AreEqual(result.PublicId, result.JsonObj["public_id"].ToString());
        }

        [Test]
        public void TestUsage()
        {
            UploadTestResource(); // making sure at least one resource exists

            var result = m_cloudinary.GetUsage();

            var plans = new List<string>() { "Free", "Advanced" };

            Assert.True(plans.Contains(result.Plan));
            Assert.True(result.Resources > 0);
            Assert.True(result.Objects.Used < result.Objects.Limit);
            Assert.True(result.Bandwidth.Used < result.Bandwidth.Limit);

        }

        [Test]
        public void TestMultiTransformation()
        {
            var publicId1 = GetUniquePublicId(StorageType.multi);
            var publicId2 = GetUniquePublicId(StorageType.multi);
            var tag = GetMethodTag();

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{tag},{m_apiTag}",
                PublicId = publicId1
            };

            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = publicId2;
            uploadParams.Transformation = m_simpleTransformation;
            m_cloudinary.Upload(uploadParams);

            MultiParams multi = new MultiParams(tag);
            MultiResult result = m_cloudinary.Multi(multi);
            AddCreatedPublicId(StorageType.multi, result.PublicId);
            Assert.True(result.Uri.AbsoluteUri.EndsWith($".{FILE_FORMAT_GIF}"));

            multi.Transformation = m_resizeTransformation;
            result = m_cloudinary.Multi(multi);
            AddCreatedPublicId(StorageType.multi, result.PublicId);
            Assert.IsTrue(result.Uri.AbsoluteUri.Contains(TRANSFORM_W_512));

            multi.Transformation = m_simpleTransformationAngle;
            multi.Format = FILE_FORMAT_PDF;
            result = m_cloudinary.Multi(multi);
            AddCreatedPublicId(StorageType.multi, result.PublicId);

            Assert.True(result.Uri.AbsoluteUri.Contains(TRANSFORM_A_45));
            Assert.True(result.Uri.AbsoluteUri.EndsWith($".{FILE_FORMAT_PDF}"));
        }

        [Test]
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
            Assert.AreEqual(100, iuResult25.Width);
            Assert.AreEqual(40, iuResult25.Height);

            uploadParams.PublicId = GetUniquePublicId();
            uploadParams.Transformation = m_transformationAr69;
            ImageUploadResult iuResult69 = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(iuResult69);
            Assert.AreEqual(100, iuResult69.Width);
            Assert.AreEqual(150, iuResult69.Height);

            uploadParams.PublicId = GetUniquePublicId();
            uploadParams.Transformation = m_transformationAr30;
            ImageUploadResult iuResult30 = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(iuResult30);
            Assert.AreEqual(150, iuResult30.Width);
            Assert.AreEqual(50, iuResult30.Height);

            uploadParams.PublicId = GetUniquePublicId();
            uploadParams.Transformation = m_transformationAr12;
            ImageUploadResult iuResult12 = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(iuResult12);
            Assert.AreEqual(100, iuResult12.Width);
            Assert.AreEqual(200, iuResult12.Height);
        }

        [Test]
        public void TestJsConfig()
        {
            var config = m_cloudinary.GetCloudinaryJsConfig().ToString();
            var expected = String.Join(
                Environment.NewLine,
                new List<string>
                {
                    "<script src=\"/Scripts/jquery.ui.widget.js\"></script>",
                    "<script src=\"/Scripts/jquery.iframe-transport.js\"></script>",
                    "<script src=\"/Scripts/jquery.fileupload.js\"></script>",
                    "<script src=\"/Scripts/jquery.cloudinary.js\"></script>",
                    "<script type='text/javascript'>",
                    "$.cloudinary.config({",
                    "  \"cloud_name\": \"" + m_account.Cloud + "\",",
                    "  \"api_key\": \"" + m_account.ApiKey + "\",",
                    "  \"private_cdn\": false,",
                    "  \"cdn_subdomain\": false",
                    "});",
                    "</script>",
                    ""
                }
            );

            Assert.AreEqual(expected, config);
        }

        [Test]
        public void TestJsConfigFull()
        {
            var config = m_cloudinary.GetCloudinaryJsConfig(true, @"https://raw.github.com/cloudinary/cloudinary_js/master/js").ToString();
            var expected = String.Join(
                Environment.NewLine,
                new List<string>
                {
                    "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.ui.widget.js\"></script>",
                    "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.iframe-transport.js\"></script>",
                    "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.fileupload.js\"></script>",
                    "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.cloudinary.js\"></script>",
                    "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/canvas-to-blob.min.js\"></script>",
                    "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.fileupload-image.js\"></script>",
                    "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.fileupload-process.js\"></script>",
                    "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.fileupload-validate.js\"></script>",
                    "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/load-image.min.js\"></script>",
                    "<script type='text/javascript'>",
                    "$.cloudinary.config({",
                    "  \"cloud_name\": \"" + m_account.Cloud + "\",",
                    "  \"api_key\": \"" + m_account.ApiKey + "\",",
                    "  \"private_cdn\": false,",
                    "  \"cdn_subdomain\": false",
                    "});",
                    "</script>",
                    ""
                }
            );

            Assert.AreEqual(expected, config);
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

        [Test]
        public void TestDownloadPrivate()
        {
            string result = m_cloudinary.DownloadPrivate("zihltjwsyczm700kqj1z");
            Assert.True(Regex.IsMatch(result, @"https://api\.cloudinary\.com/v1_1/[^/]*/image/download\?api_key=\d*&public_id=zihltjwsyczm700kqj1z&signature=\w{40}&timestamp=\d{10}"));
        }

        [Test]
        public void TestDownloadZip()
        {
            string result = m_cloudinary.DownloadZip("api_test_custom1", null);
            Assert.True(Regex.IsMatch(result, @"https://api\.cloudinary\.com/v1_1/[^/]*/image/download_tag\.zip\?api_key=\d*&signature=\w{40}&tag=api_test_custom1&timestamp=\d{10}"));
        }

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
                FaceCoordinates = "1,2,3,4"
            };

            var presetName = m_cloudinary.CreateUploadPreset(presetToCreate).Name;

            var preset = m_cloudinary.GetUploadPreset(presetName);

            var presetToUpdate = new UploadPresetParams(preset)
            {
                Colors = true,
                Unsigned = true,
                DisallowPublicId = true,
                QualityAnalysis = true
            };

            var result = m_cloudinary.UpdateUploadPreset(presetToUpdate);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("updated", result.Message);

            preset = m_cloudinary.GetUploadPreset(presetName);

            Assert.AreEqual(presetName, preset.Name);
            Assert.IsTrue(preset.Unsigned);
            Assert.IsTrue(preset.Settings.QualityAnalysis);

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

        [Test]
        public void TestListResourcesStartAt()
        {
            // should allow listing resources by start date - make sure your clock is set correctly!!!

            Thread.Sleep(2000);

            DateTime start = DateTime.UtcNow;
            var publicId = GetUniquePublicId();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = m_apiTag
            };
            ImageUploadResult result = m_cloudinary.Upload(uploadParams);

            Thread.Sleep(2000);

            var resources = m_cloudinary.ListResources(
                new ListResourcesParams() { Type = STORAGE_TYPE_UPLOAD, StartAt = result.CreatedAt.AddMilliseconds(-10), Direction = "asc" });

            Assert.NotNull(resources.Resources, "response should include resources");
            Assert.IsTrue(resources.Resources.Length > 0, "response should include at least one resources");
            Assert.IsNotNull(resources.Resources.FirstOrDefault(res => res.PublicId == result.PublicId));
        }

        [Test]
        public void TestCustomCoordinates()
        {
            //should allow sending custom coordinates

            var coordinates = new Core.Rectangle(121, 31, 110, 151);

            var upResult = m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                CustomCoordinates = coordinates,
                Tags = m_apiTag
            });

            var result = m_cloudinary.GetResource(new GetResourceParams(upResult.PublicId) { Coordinates = true });

            Assert.NotNull(result.Coordinates);
            Assert.NotNull(result.Coordinates.Custom);
            Assert.AreEqual(1, result.Coordinates.Custom.Length);
            Assert.AreEqual(4, result.Coordinates.Custom[0].Length);
            Assert.AreEqual(coordinates.X, result.Coordinates.Custom[0][0]);
            Assert.AreEqual(coordinates.Y, result.Coordinates.Custom[0][1]);
            Assert.AreEqual(coordinates.Width, result.Coordinates.Custom[0][2]);
            Assert.AreEqual(coordinates.Height, result.Coordinates.Custom[0][3]);

            coordinates = new Core.Rectangle(122, 32, 110, 152);

            var exResult = m_cloudinary.Explicit(new ExplicitParams(upResult.PublicId)
            {
                CustomCoordinates = coordinates,
                Type = STORAGE_TYPE_UPLOAD,
                Tags = m_apiTag
            });

            result = m_cloudinary.GetResource(new GetResourceParams(upResult.PublicId) { Coordinates = true });

            Assert.NotNull(result.Coordinates);
            Assert.NotNull(result.Coordinates.Custom);
            Assert.AreEqual(1, result.Coordinates.Custom.Length);
            Assert.AreEqual(4, result.Coordinates.Custom[0].Length);
            Assert.AreEqual(coordinates.X, result.Coordinates.Custom[0][0]);
            Assert.AreEqual(coordinates.Y, result.Coordinates.Custom[0][1]);
            Assert.AreEqual(coordinates.Width, result.Coordinates.Custom[0][2]);
            Assert.AreEqual(coordinates.Height, result.Coordinates.Custom[0][3]);
        }

        [Test]
        public void TestUpdateCustomCoordinates()
        {
            //should update custom coordinates

            var coordinates = new Core.Rectangle(121, 31, 110, 151);

            var upResult = m_cloudinary.Upload(new ImageUploadParams() { File = new FileDescription(m_testImagePath), Tags = m_apiTag });

            m_cloudinary.UpdateResource(new UpdateParams(upResult.PublicId) { CustomCoordinates = coordinates });

            var result = m_cloudinary.GetResource(new GetResourceParams(upResult.PublicId) { Coordinates = true });

            Assert.NotNull(result.Coordinates);
            Assert.NotNull(result.Coordinates.Custom);
            Assert.AreEqual(1, result.Coordinates.Custom.Length);
            Assert.AreEqual(4, result.Coordinates.Custom[0].Length);
            Assert.AreEqual(coordinates.X, result.Coordinates.Custom[0][0]);
            Assert.AreEqual(coordinates.Y, result.Coordinates.Custom[0][1]);
            Assert.AreEqual(coordinates.Width, result.Coordinates.Custom[0][2]);
            Assert.AreEqual(coordinates.Height, result.Coordinates.Custom[0][3]);
        }

        [Test]
        public void TestUpdateQuality()
        {
            //should update quality
            string publicId = GetUniquePublicId();
            var upResult = m_cloudinary.Upload(new ImageUploadParams() { File = new FileDescription(m_testImagePath), PublicId = publicId, Overwrite = true, Tags = m_apiTag });
            var updResult = m_cloudinary.UpdateResource(new UpdateParams(upResult.PublicId) { QualityOverride = "auto:best" });
            Assert.AreEqual(updResult.StatusCode, HttpStatusCode.OK);
            Assert.Null(updResult.Error);
            Assert.AreEqual(updResult.PublicId, publicId);
        }

        // For this test to work, "Auto-create folders" should be enabled in the Upload Settings, so this test is disabled by default.
        [Test, IgnoreFeature("auto_create_folders")]
        public void TestFolderApi()
        {
            // should allow to list folders and subfolders
            var subFolder1 = $"{m_folderPrefix}1/test_subfolder1";
            var subFolder2 = $"{m_folderPrefix}1/test_subfolder2";

            var publicIds = new List<string> {
                $"{m_folderPrefix}1/item",
                $"{m_folderPrefix}2/item",
                $"{subFolder1}/item",
                $"{subFolder2}/item"
            };

            publicIds.ForEach(p => m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = p,
                Tags = m_apiTag
            }));

            var result = m_cloudinary.RootFolders();
            Assert.Null(result.Error);
            Assert.IsTrue(result.Folders.Any(folder => folder.Name == $"{m_folderPrefix}1"));
            Assert.IsTrue(result.Folders.Any(folder => folder.Name == $"{m_folderPrefix}2"));

            // TODO: fix race here (server might be not updated at this point)
            Thread.Sleep(2000);

            result = m_cloudinary.SubFolders($"{m_folderPrefix}1");

            Assert.AreEqual(2, result.Folders.Count);
            Assert.AreEqual(subFolder1, result.Folders[0].Path);
            Assert.AreEqual(subFolder2, result.Folders[1].Path);

            result = m_cloudinary.SubFolders(m_folderPrefix);

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.NotNull(result.Error.Message);
            Assert.AreEqual($"Can't find folder with path {m_folderPrefix}", result.Error.Message);

            var deletionRes = m_cloudinary.DeleteFolder(subFolder1);

            Assert.AreEqual(HttpStatusCode.BadRequest, deletionRes.StatusCode);
            Assert.NotNull(deletionRes.Error);
            Assert.NotNull(deletionRes.Error.Message);
            Assert.AreEqual("Folder is not empty", deletionRes.Error.Message);

            m_cloudinary.DeleteResourcesByPrefix(subFolder1);

            deletionRes = m_cloudinary.DeleteFolder(subFolder1);

            Assert.Null(deletionRes.Error);
            Assert.AreEqual(1, deletionRes.Deleted.Count);
            Assert.AreEqual(subFolder1, deletionRes.Deleted[0]);
        }

        [Test]
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

        [Test]
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

            Assert.Null(result.Error);
            Assert.NotNull(result.ResponsiveBreakpoints, "result should include 'ResponsiveBreakpoints'");
            Assert.AreEqual(1, result.ResponsiveBreakpoints.Count);

            Assert.AreEqual(TRANSFORM_A_45, result.ResponsiveBreakpoints[0].Transformation);
            StringAssert.EndsWith(FILE_FORMAT_GIF, result.ResponsiveBreakpoints[0].Breakpoints[0].Url,
                $"generated breakpoint should have '{FILE_FORMAT_GIF}' extension");
        }

        [Test]
        public void TestResponsiveBreakpoints()
        {
            var publicId = GetUniquePublicId();
            var breakpoint = new ResponsiveBreakpoint().MaxImages(5).BytesStep(20)
                                .MinWidth(200).MaxWidth(1000).CreateDerived(false);

            var breakpoint2 = new ResponsiveBreakpoint().Transformation(m_simpleTransformation).MaxImages(4)
                                .BytesStep(20).MinWidth(100).MaxWidth(900).CreateDerived(false);

            // An array of breakpoints
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = m_apiTag,
                ResponsiveBreakpoints = new List<ResponsiveBreakpoint> { breakpoint, breakpoint2 }
            };
            ImageUploadResult result = m_cloudinary.Upload(uploadParams);
            Assert.Null(result.Error);
            Assert.NotNull(result.ResponsiveBreakpoints, "result should include 'ResponsiveBreakpoints'");
            Assert.AreEqual(2, result.ResponsiveBreakpoints.Count);

            Assert.AreEqual(5, result.ResponsiveBreakpoints[0].Breakpoints.Count);
            Assert.AreEqual(1000, result.ResponsiveBreakpoints[0].Breakpoints[0].Width);
            Assert.AreEqual(200, result.ResponsiveBreakpoints[0].Breakpoints[4].Width);

            Assert.AreEqual(4, result.ResponsiveBreakpoints[1].Breakpoints.Count);
            Assert.AreEqual(900, result.ResponsiveBreakpoints[1].Breakpoints[0].Width);
            Assert.AreEqual(100, result.ResponsiveBreakpoints[1].Breakpoints[3].Width);

            // responsive breakpoints for Explicit()
            ExplicitParams exp = new ExplicitParams(publicId)
            {
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                Type = STORAGE_TYPE_UPLOAD,
                Tags = m_apiTag,
                ResponsiveBreakpoints = new List<ResponsiveBreakpoint> { breakpoint2.CreateDerived(true) }
            };

            ExplicitResult expResult = m_cloudinary.Explicit(exp);

            Assert.AreEqual(1, expResult.ResponsiveBreakpoints.Count);
            Assert.AreEqual(4, expResult.ResponsiveBreakpoints[0].Breakpoints.Count);
            Assert.AreEqual(900, expResult.ResponsiveBreakpoints[0].Breakpoints[0].Width);
            Assert.AreEqual(100, expResult.ResponsiveBreakpoints[0].Breakpoints[3].Width);
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
            Assert.NotNull(result.ResponsiveBreakpoints); //todo: check it in netCore
            Assert.AreEqual(1, result.ResponsiveBreakpoints.Count);

            Assert.AreEqual(5, result.ResponsiveBreakpoints[0].Breakpoints.Count);
            Assert.AreEqual(1000, result.ResponsiveBreakpoints[0].Breakpoints[0].Width);
            Assert.AreEqual(200, result.ResponsiveBreakpoints[0].Breakpoints[4].Width);
        }

        //[Test]
        //public void TestTextAlign()
        //{
        //    TextParams tParams = new TextParams("Sample text.");
        //    tParams.Background = "red";
        //    tParams.FontStyle = "italic";
        //    tParams.TextAlign = "center";

        //    string rString = GetMockBodyOfCloudinaryRequest(tParams, (p, t) => { return p.Text(t); });

        //    StringAssert.Contains("name=\"text_align\"\r\n\r\ncenter\r\n", rString);
        //}

        //[Test]
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

        private ImageUploadResult UploadImageForTestArchive(string archiveTag, double width, bool useFileName)
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(width) },
                UseFilename = useFileName,
                Tags = $"{archiveTag},{m_apiTag}"
            };
            return m_cloudinary.Upload(uploadParams);
        }

        [Test]
        public void TestCreateArchive()
        {
            string targetPublicId = GetUniquePublicId();
            string archiveTag = GetMethodTag();

            ImageUploadResult res = UploadImageForTestArchive(archiveTag, 2.0, true);

            ArchiveParams parameters = new ArchiveParams()
                                            .Tags(new List<string> { archiveTag, "no_such_tag" })
                                            .TargetPublicId(targetPublicId)
                                            .TargetTags(new List<string> { m_apiTag });
            ArchiveResult result = m_cloudinary.CreateArchive(parameters);
            Assert.AreEqual($"{targetPublicId}.{FILE_FORMAT_ZIP}", result.PublicId);
            Assert.AreEqual(1, result.FileCount);

            ImageUploadResult res2 = UploadImageForTestArchive(archiveTag, 500, false);

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
            string raw = Api.GetCloudinaryParam(ResourceType.Raw);
            var tag = GetMethodTag();

            RawUploadParams uploadParams = new RawUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Folder = "test_folder",
                Type = STORAGE_TYPE_PRIVATE,
                Tags = $"{tag},{m_apiTag}"
            };
            RawUploadResult uploadResult1 = m_cloudinary.Upload(uploadParams, raw);

            uploadParams.File = new FileDescription(m_testPdfPath);
            RawUploadResult uploadResult2 = m_cloudinary.Upload(uploadParams, raw);

            ArchiveParams parameters = new ArchiveParams()
                                            .PublicIds(new List<string> { uploadResult1.PublicId, uploadResult2.PublicId })
                                            .ResourceType(raw)
                                            .Type(STORAGE_TYPE_PRIVATE)
                                            .UseOriginalFilename(true)
                                            .TargetTags(new List<string> { m_apiTag });
            ArchiveResult result = m_cloudinary.CreateArchive(parameters);
            Assert.AreEqual(2, result.FileCount);
        }

        private ArchiveParams UploadImageForArchiveAndPrepareParameters(string archiveTag)
        {
            UploadImageForTestArchive($"{archiveTag},{m_apiTag}", 2.0, true);

            return new ArchiveParams().Tags(new List<string> { archiveTag, "non-existent-tag" }).TargetTags(new List<string> { m_apiTag }).TargetPublicId(GetUniquePublicId());
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

            parameters.ResourceType("auto").Tags(new List<string> {"tag"});

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

        /// <summary>
        /// Should create a zip archive
        /// </summary>
        [Test]
        public void TestCreateZip()
        {
            var parameters = UploadImageForArchiveAndPrepareParameters(GetMethodTag());
            var result = m_cloudinary.CreateZip(parameters);

            Assert.AreEqual($"{parameters.TargetPublicId()}.{FILE_FORMAT_ZIP}", result.PublicId);
            Assert.AreEqual(1, result.FileCount);
        }

        [Test]
        public void TestDownloadArchiveUrl()
        {
            var archiveTag = GetMethodTag();
            var parameters = new ArchiveParams().Tags(new List<string> { archiveTag });

            var urlStr = m_cloudinary.DownloadArchiveUrl(parameters);

            var dicQueryString = new Uri(urlStr).Query.Split('&').ToDictionary(
                c => Uri.UnescapeDataString(c.Split('=')[0]), c => Uri.UnescapeDataString(c.Split('=')[1])
            );

            Assert.AreEqual("download", dicQueryString["mode"]);
            Assert.AreEqual(archiveTag, dicQueryString["tags[]"]);
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

        [Test]
        public void TestAddContext()
        {
            var publicId = GetUniquePublicId();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Overwrite = true,
                Type = STORAGE_TYPE_UPLOAD,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            List<string> pIds = new List<string> { publicId };

            ContextResult contextResult = m_cloudinary.Context(new ContextParams()
            {
                Command = ContextCommand.Add,
                PublicIds = pIds,
                Type = STORAGE_TYPE_UPLOAD,
                Context = "TestContext"
            });

            Assert.True(contextResult.PublicIds.Length > 0);

            m_cloudinary.GetResource(new GetResourceParams(pIds[0])
            {
                PublicId = pIds[0],
                Type = STORAGE_TYPE_UPLOAD,
                ResourceType = ResourceType.Image
            });
        }

        [Test]
        public void TestArchiveParamsCheck()
        {
            Assert.Throws<ArgumentException>(new ArchiveParams().Check, "Should require atleast on option specified: PublicIds, Tags or Prefixes");
        }
        [Test]
        public void TestCreateTransformParamsCheck()
        {
            var p = new CreateTransformParams();
            Assert.Throws<ArgumentException>(p.Check, "Should require Name");
            p.Name = "some_name";
            Assert.Throws<ArgumentException>(p.Check, "Should require Transformation");
        }

        [Test]
        public void TestDelDerivedResParamsCheck()
        {
            var p = new DelDerivedResParams();
            Assert.Throws<ArgumentException>(p.Check, "Should require either DerivedResources or Tranformations not null");

            p.DerivedResources = new List<string>();
            Assert.Throws<ArgumentException>(p.Check, "Should require at least on item in either DerivedResources or Tranformations specified");

            p.Transformations = new List<Transformation>() { new Transformation() };
            Assert.Throws<ArgumentException>(p.Check, "Should require PublicId");
        }

        [Test]
        public void TestDeletionParamsCheck()
        {
            Assert.Throws<ArgumentException>(new DeletionParams("").Check, "Should require PublicId");
        }

        [Test]
        public void TestDelResParamsCheck()
        {
            var p = new DelResParams();
            Assert.Throws<ArgumentException>(p.Check, "Should require either PublicIds or Prefix or Tag specified");
        }

        [Test]
        public void TestExplicitParamsCheck()
        {
            Assert.Throws<ArgumentException>(new ExplicitParams("").Check, "Should require PublicId");
        }

        [Test]
        public void TestExplodeParamsCheck()
        {
            var p = new ExplodeParams("",null);
            Assert.Throws<ArgumentException>(p.Check, "Should require PublicId");

            p.PublicId = "publicId";
            Assert.Throws<ArgumentException>(p.Check, "Should require Transformation");
        }

        [Test]
        public void TestGetResourceParamsCheck()
        {
            Assert.Throws<ArgumentException>(new GetResourceParams("").Check, "Should require PublicId");
        }

        [Test]
        public void TestGetTransformParamsCheck()
        {
            Assert.Throws<ArgumentException>(new GetTransformParams().Check, "Should require Transformation");
        }

        [Test]
        public void TestMultiParamsCheck()
        {
            Assert.Throws<ArgumentException>(new MultiParams("").Check, "Should require Tag");
        }

        [Test]
        public void TestRawUploadParamsCheck()
        {
            var p = new RawUploadParams();
            Assert.Throws<ArgumentException>(p.Check, "Should require File");

            p.File = new FileDescription("", new MemoryStream());
            Assert.Throws<ArgumentException>(p.Check, "Should require FilePath and Stream specified for local file");
        }

        [Test]
        public void TestRenameParamsCheck()
        {
            var p = new RenameParams("", "");
            Assert.Throws<ArgumentException>(p.Check, "Should require FromPublicId");

            p.FromPublicId = "FromPublicId";
            Assert.Throws<ArgumentException>(p.Check,"Should require ToPublicId");
        }

        [Test]
        public void TestRestoreParamsCheck()
        {
            Assert.Throws<ArgumentException>(new RestoreParams().Check, "Should require at least one PublicId");
        }

        [Test]
        public void TestSpriteParamsCheck()
        {
            Assert.Throws<ArgumentException>(new SpriteParams("").Check, "Should require Tag");
        }

        [Test]
        public void TestTextParamsCheck()
        {
            Assert.Throws<ArgumentException>(new TextParams().Check, "Should require Text");
        }

        [Test]
        public void TestUpdateParamsCheck()
        {
            Assert.Throws<ArgumentException>(new UpdateParams("").Check, "Should require PublicId");
        }

        [Test]
        public void TestUpdateTransformParamsCheck()
        {
            Assert.Throws<ArgumentException>(new UpdateTransformParams().Check, "Should require Transformation");
        }

        [Test]
        public void UploadMappingParamsCheckTest()
        {
            var p = new UploadMappingParams { MaxResults = 1000 };
            Assert.Throws<ArgumentException>(p.Check, "Should require MaxResults value less or equal 500");
        }

        [Test]
        public void UploadPresetParamsCheckTest()
        {
            var p = new UploadPresetParams { Overwrite = true, Unsigned = true };
            Assert.Throws<ArgumentException>(p.Check, "Should require only one property set to true: Overwrite or Unsigned");
        }
    }
}
