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

        protected readonly Transformation m_transformationAngleExtended = new Transformation().Angle(45).Height(210).Crop("scale");
        protected readonly Transformation m_transformationAr25 = new Transformation().Width(100).AspectRatio(2.5);
        protected readonly Transformation m_transformationAr69 = new Transformation().Width(100).AspectRatio(6, 9);
        protected readonly Transformation m_transformationAr30 = new Transformation().Width(150).AspectRatio("3.0");
        protected readonly Transformation m_transformationAr12 = new Transformation().Width(100).AspectRatio("1:2");
        protected readonly Transformation m_transformationExplode = new Transformation().Page("all");

        protected readonly Transformation m_eagerTransformation = new EagerTransformation(new Transformation().Width(512).Height(512), new Transformation().Width(100).Crop("scale")).SetFormat("png");


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
            Assert.AreEqual("jpg", uploadResult.Format);

            var checkParams = new SortedDictionary<string, object>();
            checkParams.Add("public_id", uploadResult.PublicId);
            checkParams.Add("version", uploadResult.Version);

            var api = new Api(m_account);
            string expectedSign = api.SignParameters(checkParams);

            Assert.AreEqual(expectedSign, uploadResult.Signature);
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

            Assert.AreEqual("pdf", uploadResult.Format);
            Assert.AreEqual(TEST_PDF_PAGES_COUNT, uploadResult.Pages);
        }

        [Test]
        public void TestUploadLocalImageTimeout()
        {
            var timeout = 3000;
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            };

            var origAddr = m_cloudinary.Api.ApiBaseAddress;
            Stopwatch stopWatch = new Stopwatch();
            m_cloudinary.Api.ApiBaseAddress = "https://10.255.255.1";
            m_cloudinary.Api.Timeout = timeout;
            try
            {
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
                stopWatch.Stop();
            }
            
            Assert.LessOrEqual(timeout - 2000, stopWatch.ElapsedMilliseconds);
            Assert.GreaterOrEqual(timeout + 2000, stopWatch.ElapsedMilliseconds);
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
            Assert.AreEqual("mp4", uploadResult.Format);
            Assert.NotNull(uploadResult.Audio);
            Assert.AreEqual("aac", uploadResult.Audio.Codec);
            Assert.NotNull(uploadResult.Video);
            Assert.AreEqual("h264", uploadResult.Video.Codec);

            var getResource = new GetResourceParams(uploadResult.PublicId) { ResourceType = ResourceType.Video };
            var info = m_cloudinary.GetResource(getResource);

            Assert.AreEqual("mp4", info.Format);
        }

        [Test]
        public void TestUploadCustom()
        {
            var file = new FileDescription(m_testVideoPath);
            var uploadResult = m_cloudinary.Upload("video", null, file);

            m_cloudinary.DeleteResources(new DelResParams { PublicIds = new List<string> { uploadResult.PublicId }, ResourceType = ResourceType.Video });
            Assert.NotNull(uploadResult);
            Assert.AreEqual("video", uploadResult.ResourceType);
        }

        [Test]
        public void TestModeration()
        {
            var uploadParams = new RawUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Moderation = "manual",
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(uploadResult);
            Assert.NotNull(uploadResult.Moderation);
            Assert.AreEqual(1, uploadResult.Moderation.Count);
            Assert.AreEqual("manual", uploadResult.Moderation[0].Kind);
            Assert.AreEqual(ModerationStatus.Pending, uploadResult.Moderation[0].Status);

            var getResult = m_cloudinary.GetResource(uploadResult.PublicId);

            Assert.NotNull(getResult);
            Assert.NotNull(getResult.Moderation);
            Assert.AreEqual(1, getResult.Moderation.Count);
            Assert.AreEqual("manual", getResult.Moderation[0].Kind);
            Assert.AreEqual(ModerationStatus.Pending, getResult.Moderation[0].Status);
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
                Ocr = "illegal"
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, updateResult.StatusCode);
            Assert.True(updateResult.Error.Message.StartsWith("Illegal value"));
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
                RawConvert = "illegal"
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, updateResult.StatusCode);
            Assert.True(updateResult.Error.Message.StartsWith("Illegal value"));
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
                Categorization = "illegal"
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, updateResult.StatusCode);
            Assert.True(updateResult.Error.Message.StartsWith("Illegal value"));
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
                Detection = "illegal"
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, updateResult.StatusCode);
            Assert.True(updateResult.Error.Message.StartsWith("Illegal value"));
        }

        [Test, Ignore("Requires Rekognition plugin")]
        public void TestRekognitionFace()
        {
            // should support rekognition face
            // RekognitionFace add-on should be enabled for the used account

            var uploadResult = m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            });

            Assert.IsNull(uploadResult.Info);

            var updateResult = m_cloudinary.UpdateResource(new UpdateParams(uploadResult.PublicId)
            {
                Detection = "rekognition_face"
            });

            Assert.NotNull(updateResult.Info);
            Assert.NotNull(updateResult.Info.Detection);
            Assert.NotNull(updateResult.Info.Detection.RekognitionFace);
            Assert.AreEqual("complete", updateResult.Info.Detection.RekognitionFace.Status);

            uploadResult = m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Detection = "rekognition_face",
                Tags = m_apiTag
            });

            Assert.NotNull(uploadResult.Info);
            Assert.NotNull(uploadResult.Info.Detection);
            Assert.NotNull(uploadResult.Info.Detection.RekognitionFace);
            Assert.AreEqual("complete", uploadResult.Info.Detection.RekognitionFace.Status);
        }

        [Test]
        public void TestUploadOverwrite()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = $"{m_apiTest}_TestUploadOverwrite",
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
                PublicId = $"{m_apiTest}_TestUploadLocalImageGetMetadata",
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
                Type = "upload",
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
        public void TestUploadLocalImageUseFilename()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                EagerAsync = true,
                UseFilename = true,
                NotificationUrl = "http://www.google.com",
                Tags = m_apiTag
            };

            ImageUploadResult result = m_cloudinary.Upload(uploadParams);

            Assert.True(result.PublicId.StartsWith("TestImage"));
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
                NotificationUrl = "http://www.google.com",
                Tags = m_apiTag
            };

            var result = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual("TestImage", result.PublicId);
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

            Assert.AreEqual(512, uploadResult.Width);
            Assert.AreEqual(512, uploadResult.Height);
            Assert.AreEqual("jpg", uploadResult.Format);
        }

        [Test]
        public void TestEnglishText()
        {
            TextParams tParams = new TextParams("Sample text.")
            {
                Background = "red",
                FontStyle = "italic",
                PublicId = $"{m_apiTest1}_SAMPLE_TEXT"
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
                PublicId = $"{m_apiTest2}_SAMPLE_TEXT"
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

            RawUploadResult uploadResult = m_cloudinary.Upload(uploadParams, "raw");

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
                File = new FileDescription("http://cloudinary.com/images/old_logo.png"),
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(3381, uploadResult.Length);
            Assert.AreEqual(241, uploadResult.Width);
            Assert.AreEqual(51, uploadResult.Height);
            Assert.AreEqual("png", uploadResult.Format);
        }

        [Test]
        public void TestUploadDataUri()
        {
            var upload = new ImageUploadParams()
            {
                File = new FileDescription("data:image/png;base64,iVBORw0KGgoAA\nAANSUhEUgAAABAAAAAQAQMAAAAlPW0iAAAABlBMVEUAAAD///+l2Z/dAAAAM0l\nEQVR4nGP4/5/h/1+G/58ZDrAz3D/McH8yw83NDDeNGe4Ug9C9zwz3gVLMDA/A6\nP9/AFGGFyjOXZtQAAAAAElFTkSuQmCC"),
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

            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                ImageUploadParams uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription("streamed", memoryStream),
                    Tags = $"{m_apiTag},streamed"
                };

                ImageUploadResult uploadResult = m_cloudinary.Upload(uploadParams);

                Assert.AreEqual(1920, uploadResult.Width);
                Assert.AreEqual(1200, uploadResult.Height);
                Assert.AreEqual("jpg", uploadResult.Format);
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
            var end   = new DateTime(3000, 12, 31, 23, 59, 59, DateTimeKind.Utc);

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
            
            uploadParams.AccessControl.Add(new AccessControlRule{AccessType = AccessType.Token});
            
            uploadResult = m_cloudinary.Upload(uploadParams);
            
            Assert.AreEqual(2, uploadResult.AccessControl.Count);
            
            Assert.AreEqual(AccessType.Anonymous, uploadResult.AccessControl[0].AccessType);
            Assert.AreEqual(start,  uploadResult.AccessControl[0].Start);
            Assert.AreEqual(end, uploadResult.AccessControl[0].End);
            
            Assert.AreEqual(AccessType.Token, uploadResult.AccessControl[1].AccessType);
            Assert.IsNull(uploadResult.AccessControl[1].Start);
            Assert.IsNull(uploadResult.AccessControl[1].End);
        }

        [Test]
        public void TestPublishByTag()
        {
            var publishTag = $"{m_apiTest}_TestPublishByTag";

            var uploadParams = new ImageUploadParams()
            {  
                File = new FileDescription(m_testImagePath),
                Tags = $"{publishTag},{m_apiTag}",
                PublicId = publishTag,
                Overwrite = true,
                Type = "private"
            };

            m_cloudinary.Upload(uploadParams);

            var publish_result = m_cloudinary.PublishResourceByTag(publishTag, new PublishResourceParams()
            {
                ResourceType = ResourceType.Image, 
            });

            Assert.AreEqual(1, publish_result.Published.Count);
        }

        [Test]
        public void TestUpdateAccessModeByTag()
        {
            var publishTag = $"{m_apiTest}_TestForUpdateAccessMode";
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{m_apiTag},{publishTag}",
                PublicId = publishTag,
                Overwrite = true,
                Type = "private"
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            var update_result = m_cloudinary.UpdateResourceAccessModeByTag(publishTag, new UpdateResourceAccessModeParams()
            {
                ResourceType = ResourceType.Image,
                Type = "upload",
                AccessMode = "public"
            });

            //TODO: fix this test, make assertions working

            //Assert.AreEqual(publish_result.Published.Count, 1);
        }

        [Test]
        public void TestUpdateAccessModeById()
        {
            var publishTag = $"{m_apiTest}_TestForUpdateAccessMode";
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{m_apiTag},{publishTag}",
                PublicId = publishTag,
                Overwrite = true,
                Type = "private"
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            List<string> ids = new List<string>();
            ids.Add(publishTag);

            var update_result = m_cloudinary.UpdateResourceAccessModeByIds(new UpdateResourceAccessModeParams()
            {
                ResourceType = ResourceType.Image,
                Type = "upload",
                AccessMode = "public",
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
            var end   = new DateTime(3000, 12, 31, 23, 59, 59, DateTimeKind.Utc);

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
                new UpdateParams(uploadResult.PublicId) {AccessControl = newAccessControl}
            );
            
            Assert.AreEqual(1, updateResult.AccessControl.Count);
            
            Assert.AreEqual(AccessType.Token, updateResult.AccessControl[0].AccessType);
            Assert.AreEqual(end, updateResult.AccessControl[0].Start);
            Assert.AreEqual(start, updateResult.AccessControl[0].End);
        }

        [Test]
        public void TestUploadLargeFromWeb()
        {
            // support uploading large image

            var largeFilePath = "http://res.cloudinary.com/demo/video/upload/v1496743637/dog.mp4";
            var result = m_cloudinary.UploadLarge(new ImageUploadParams()
            {
                File = new FileDescription(largeFilePath),
                Tags = m_apiTag
            }, 5 * 1024 * 1024);

            Assert.AreEqual(result.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.AreEqual(result.Format, "mp4");
        }

        [Test]
        public void TestTagAdd()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            };

            ImageUploadResult uploadResult = m_cloudinary.Upload(uploadParams);

            TagParams tagParams = new TagParams()
            {
                Command = TagCommand.Add,
                Tag = "test-------tag"
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
                Tag = m_test_tag,
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
            var testTag1 = $"{m_apiTag}_Tag1";
            var testTag2 = $"{m_apiTag}_Tag2";
            var testTag3 = $"{m_apiTag}_Tag3";

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
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"test++++++tag,{m_apiTag}"
            };

            ImageUploadResult uploadResult = m_cloudinary.Upload(uploadParams);

            TagParams tagParams = new TagParams()
            {
                Command = TagCommand.Replace,
                Tag = $"{m_apiTag}_another-tag-test,{m_apiTag}"
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
                PublicId = $"{m_apiTest}_TestListResourcesByType",
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            IEnumerable<Resource> result = GetAllResults((cursor) => m_cloudinary.ListResourcesByType("upload", cursor));

            Assert.IsNotEmpty(result.Where(res => res.Type == "upload"));
            Assert.IsEmpty(result.Where(res => res.Type != "upload"));
        }

        [Test]
        public void TestListResourcesByPrefix()
        {
            // should allow listing resources by prefix
            var publicId = $"testlist{m_apiTest1}";

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
            Assert.IsTrue(result.Resources.Where(res => (res.Context == null ? false : res.Context["custom"]["context"].ToString() == "abc")).Count() > 0);
        }

        [Test, Ignore("test needs to be re-written with mocking - it fails when there are many resources")]
        public void TestResourcesListingDirection()
        {
            // should allow listing resources in both directions

            var result = m_cloudinary.ListResources(new ListResourcesByPrefixParams()
            {
                Type = "upload",
                MaxResults = 500,
                Direction = "asc"
            });

            var list1 = result.Resources.Select(r => r.PublicId).ToArray();

            result = m_cloudinary.ListResources(new ListResourcesByPrefixParams()
            {
                Type = "upload",
                MaxResults = 500,
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
                PublicId = $"{m_apiTest}_TestContext",
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
            var publicId1 = $"{m_apiTest1}TestListResourcesByPublicIds";
            var publicId2 = $"{m_apiTest2}TestListResourcesByPublicIds";
            var context = new StringDictionary("key=value", "key2=value2");
            // should allow listing resources by public ids
            var uploadParams = new ImageUploadParams() { File = new FileDescription(m_testImagePath), PublicId = publicId1, Context = context, Tags = m_apiTag };
            m_cloudinary.Upload(uploadParams);

            uploadParams = new ImageUploadParams() { File = new FileDescription(m_testImagePath), PublicId = publicId2, Context = context, Tags = m_apiTag };
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
            var localTag = $"{m_apiTag}_LIST_RESOURCES";

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
                    Moderation = "manual",
                    Tags = m_apiTag
                }));
            }

            m_cloudinary.UpdateResource(uploadResults[0].PublicId, ModerationStatus.Approved);
            m_cloudinary.UpdateResource(uploadResults[1].PublicId, ModerationStatus.Rejected);

            var requestParams = new ListResourcesByModerationParams()
            {
                MaxResults = 500,
                ModerationKind = "manual",
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
            var publicId = $"{m_apiTest}_TestResourcesCursor";

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = $"{publicId}1",
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = $"{publicId}2",
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
        public void TestEager()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation, m_eagerTransformation },
                Tags = $"{m_apiTag},eager,transformation"
            };

            var result = m_cloudinary.Upload(uploadParams);
            //TODO: fix this test, implement assertions
        }

        [Test]
        public void TestRename()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            };

            var uploadResult1 = m_cloudinary.Upload(uploadParams);

            uploadParams.File = new FileDescription(m_testIconPath);
            var uploadResult2 = m_cloudinary.Upload(uploadParams);

            var renameResult = m_cloudinary.Rename(uploadResult1.PublicId, uploadResult1.PublicId + "2");

            var getResult = m_cloudinary.GetResource(uploadResult1.PublicId + "2");
            Assert.NotNull(getResult);

            renameResult = m_cloudinary.Rename(uploadResult2.PublicId, uploadResult1.PublicId + "2");
            Assert.True(renameResult.StatusCode == HttpStatusCode.BadRequest);

            m_cloudinary.Rename(uploadResult2.PublicId, uploadResult1.PublicId + "2", true);

            getResult = m_cloudinary.GetResource(uploadResult1.PublicId + "2");
            Assert.NotNull(getResult);
            Assert.AreEqual("ico", getResult.Format);
        }

        [Test]
        public void TestRenameToType()

        {
            string publicId = string.Concat("renameType_", m_apiTest);
            string newPublicId = string.Concat("renameNewType_", m_apiTest);
            string type = "upload";
            string toType = "private";

            var uploadParams = new ImageUploadParams()
            {
                PublicId = publicId,
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag,
                Type = type
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);
            Assert.AreEqual(uploadResult.StatusCode, HttpStatusCode.OK);

            RenameParams renameParams = new RenameParams(publicId, newPublicId)
            {
                ToType = toType
            };

            var renameResult = m_cloudinary.Rename(renameParams);
            Assert.AreEqual(renameResult.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(renameResult.Type, toType);
            Assert.AreEqual(renameResult.PublicId, newPublicId);
        }

        [Test]
        public void TestGetResource()
        {
            // should allow get resource details
            var publicId = $"testgetresource_{m_apiTest2}";
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
            Assert.AreEqual("jpg", getResult.Format);
            Assert.AreEqual(1, getResult.Derived.Length);
            Assert.Null(getResult.Metadata);
            Assert.NotNull(getResult.Phash);
        }

        [Test]
        public void TestGetResourceWithMetadata()
        {
            // should allow get resource metadata
            var publicId = $"{m_apiTest}_TestGetResourceWithMetadata";

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
        public void TestDeleteDerived()
        {
            // should allow deleting derived resource
            var publicId = $"{m_apiTest}_TestDeleteDerived";

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
            var publicId = $"{m_apiTest}_TestDelete";
            var rndString = $"{m_apiTest}_SomeRandomString";

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
                rndString, "not_used_string", publicId);

            Assert.AreEqual("not_found", delResult.Deleted[rndString]);
            Assert.AreEqual("deleted", delResult.Deleted[publicId]);

            resource = m_cloudinary.GetResource(publicId);

            Assert.IsTrue(String.IsNullOrEmpty(resource.PublicId));
        }

        [Test]
        public void TestDeleteByPrefix()
        {
            // should allow deleting resources
            var publicId = $"{m_apiTest}_TestDeleteByPrefix";
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

            DelResResult delResult = m_cloudinary.DeleteResourcesByPrefix(prefix);

            resource = m_cloudinary.GetResource(publicId);
            Assert.IsTrue(String.IsNullOrEmpty(resource.PublicId));
        }

        [Test]
        public void TestDeleteByTag()
        {
            // should allow deleting resources
            var publicId = $"{m_apiTest}_TestDeleteByTag";
            var tag = $"{m_apiTag}_TestDeleteByTag";

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
        public void TestRestoreNoBackup()
        {
            string publicId = $"{m_apiTest}_TestRestoreNoBackup";

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
            string publicId = $"{m_apiTest}_TestRestore";

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
            var tag = $"{m_apiTag}_LTags";

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{tag},{m_apiTag}"
            };

            m_cloudinary.Upload(uploadParams);

            ListTagsResult result = m_cloudinary.ListTags(new ListTagsParams() { MaxResults = 500 });

            Assert.Contains(tag, result.Tags);
        }

        [Test]
        public void TestAllowedFormats()
        {
            //should allow whitelisted formats if allowed_formats
            
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                AllowedFormats = new string[] { "jpg" },
                Tags = m_apiTag
            };

            var res = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual("jpg", res.Format);
        }

        [Test]
        public void TestAllowedFormatsWithIllegalFormat()
        {
            //should prevent non whitelisted formats from being uploaded if allowed_formats is specified

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                AllowedFormats = new string[] { "png" },
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
                AllowedFormats = new string[] { "png" },
                Format = "png",
                Tags = m_apiTag
            };

            var res = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual("png", res.Format);
        }

        [Test]
        public void TestManualModeration()
        {
            // should support setting manual moderation status

            var uploadResult = m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Moderation = "manual",
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
                    MaxResults = 500
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
            var tag = $"{m_apiTag}_TestListTagsPrefix";
            var tag2 = $"{m_apiTag}_TestListTagsPrefix2";

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{tag},{m_apiTag}"
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

            result = m_cloudinary.ListTagsByPrefix("nononothereisnosuchtag");

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
            TransformDesc td = result.Transformations.Where(t => t.Name == m_simpleTransformationName).First();
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

            var result = m_cloudinary.GetTransform(m_updateTransformationName);

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
                Transformation = m_simpleTransformationName,
                Strict = true
            };

            m_cloudinary.UpdateTransform(updateParams);

            GetTransformResult getResult = m_cloudinary.GetTransform(m_simpleTransformationName);

            Assert.IsNotNull(getResult);
            Assert.AreEqual(true, getResult.Strict);

            updateParams.Strict = false;
            m_cloudinary.UpdateTransform(updateParams);

            getResult = m_cloudinary.GetTransform(m_simpleTransformationName);

            Assert.IsNotNull(getResult);
            Assert.AreEqual(false, getResult.Strict);
        }

        [Test]
        public void TestUpdateTransformUnsafe()
        {
            string transformName = m_apiTest;
            // should allow unsafe update of named transformation
            m_cloudinary.CreateTransform(
                new CreateTransformParams()
                {
                    Name = transformName,
                    Transform = m_simpleTransformation
                });

            var updateParams = new UpdateTransformParams()
            {
                Transformation = transformName,
                UnsafeTransform = m_updateTransformation
            };

            m_cloudinary.UpdateTransform(updateParams);

            var getResult = m_cloudinary.GetTransform(transformName);

            Assert.IsNotNull(getResult.Info);
            Assert.AreEqual(updateParams.UnsafeTransform.Generate(), new Transformation(getResult.Info).Generate());
        }

        [Test]
        public void TestCreateTransform()
        {
            // should allow creating named transformation

            CreateTransformParams create = new CreateTransformParams()
            {
                Name = m_apiTest1,
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
            string transformName = m_apiTest2;
            // should allow deleting named transformation

            m_cloudinary.DeleteTransform(transformName);

            CreateTransformParams create = new CreateTransformParams()
            {
                Name = transformName,
                Transform = m_simpleTransformation
            };

            TransformResult createResult = m_cloudinary.CreateTransform(create);

            Assert.AreEqual("created", createResult.Message);

            m_cloudinary.DeleteTransform(transformName);

            GetTransformResult getResult = m_cloudinary.GetTransform(
                new GetTransformParams() { Transformation = transformName });

            Assert.AreEqual(HttpStatusCode.NotFound, getResult.StatusCode);
        }

        [Test]
        public void TestDeleteTransformImplicit()
        {
            // should allow deleting implicit transformation

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            GetTransformParams getParams = new GetTransformParams()
            {
                Transformation = m_simpleTransformationName
            };

            GetTransformResult getResult = m_cloudinary.GetTransform(getParams);

            Assert.AreEqual(HttpStatusCode.OK, getResult.StatusCode);

            m_cloudinary.DeleteTransform(m_simpleTransformationName);

            getResult = m_cloudinary.GetTransform(getParams);

            Assert.AreEqual(HttpStatusCode.NotFound, getResult.StatusCode);
        }

        [Test]
        public void TestUploadHeaders()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = $"{m_apiTest}_TestUploadHeaders",
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
            ExplicitParams exp = new ExplicitParams("cloudinary")
            {
                EagerTransforms = new List<Transformation>() { m_explicitTransformation },
                Type = "facebook",
                Tags = m_apiTag
            };

            ExplicitResult expResult = m_cloudinary.Explicit(exp);

            string url = new Url(m_account.Cloud).ResourceType("image").Add("facebook").
                Transform(m_explicitTransformation).
                Format("png").Version(expResult.Version).BuildUrl("cloudinary");

            Assert.AreEqual(url, expResult.Eager[0].Uri.AbsoluteUri);
        }

        [Test]
        public void TestExplicitContext()
        {
            var exp = new ExplicitParams("cloudinary")
            {
                EagerTransforms = new List<Transformation>() { m_explicitTransformation },
                Type = "facebook",
                Context = new StringDictionary("context1=254"),
                Tags = m_apiTag
            };

            var expResult = m_cloudinary.Explicit(exp);

            Assert.IsNotNull(expResult);

            var getResult = m_cloudinary.GetResource(new GetResourceParams(expResult.PublicId) { Type = "facebook" });

            Assert.IsNotNull(getResult);
            Assert.AreEqual("254", getResult.Context["custom"]["context1"].ToString());
        }
        
        /// <summary>
        /// Test asynchronous processing in explicit API calls
        /// </summary>
        [Test]
        public void TestExplicitAsyncProcessing()
        {

            ExplicitParams exp = new ExplicitParams("cloudinary")
            {
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(2.0) },
                Type = "facebook",
                Async = true,
            };

            ExplicitResult expAsyncResult = m_cloudinary.Explicit(exp);
            
            Assert.AreEqual("pending", expAsyncResult.Status);
            Assert.AreEqual("image", expAsyncResult.ResourceType);
            Assert.AreEqual("facebook", expAsyncResult.Type);
            Assert.AreEqual("cloudinary", expAsyncResult.PublicId);
        }

        [Test]
        public void TestExplicitVideo()
        {
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(m_testVideoPath),
                Tags = m_test_tag
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
            var publishTag = $"{m_apiTest1}_LOGO";
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{publishTag},{m_apiTag}",
                PublicId = publishTag,
                Transformation = m_resizeTransformation
            };
            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = $"{publishTag}_logo2";
            uploadParams.Transformation = m_updateTransformation;
            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = $"{publishTag}_logo3";
            uploadParams.Transformation = m_simpleTransformation;
            m_cloudinary.Upload(uploadParams);

            SpriteParams sprite = new SpriteParams(publishTag);
            SpriteResult result = m_cloudinary.MakeSprite(sprite);

            Assert.NotNull(result);
            Assert.NotNull(result.ImageInfos);
            Assert.AreEqual(3, result.ImageInfos.Count);
            Assert.Contains(publishTag, result.ImageInfos.Keys);
            Assert.Contains($"{publishTag}_logo2", result.ImageInfos.Keys);
            Assert.Contains($"{publishTag}_logo3", result.ImageInfos.Keys);
        }

        [Test]
        public void TestSpriteTransformation()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{m_apiTest1}_SPRITE,{m_apiTag}",
                PublicId = $"{m_apiTest1}_SPRITE",
                Transformation = m_simpleTransformation
            };
            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = $"{m_apiTest1}_logotrans2";
            uploadParams.Transformation = m_updateTransformation;
            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = $"{m_apiTest}_logotrans3";
            uploadParams.Transformation = m_explicitTransformation;
            m_cloudinary.Upload(uploadParams);

            SpriteParams sprite = new SpriteParams($"{m_apiTest1}_SPRITE");
            sprite.Transformation = m_resizeTransformation;

            SpriteResult result = m_cloudinary.MakeSprite(sprite);

            Assert.NotNull(result);
            Assert.NotNull(result.ImageInfos);
            foreach (var item in result.ImageInfos)
            {
                Assert.AreEqual(512, item.Value.Width);
                Assert.AreEqual(512, item.Value.Height);
            }
        }

        [Test]
        public void TestJsonObject()
        {
            var publicId = "cloudinary";
            ExplicitParams exp = new ExplicitParams(publicId)
            {
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                Type = "facebook",
                Tags = m_apiTag
            };

            var result = m_cloudinary.Explicit(exp);

            Assert.NotNull(result.JsonObj);
            Assert.AreEqual(result.PublicId, result.JsonObj["public_id"].ToString());
        }

        [Test]
        public void TestUsage()
        {
            var publicId = $"{m_apiTest}_TestUsage";

            UploadTestResource(publicId); // making sure at least one resource exists
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
            var publicId1 = $"{m_apiTest1}_TestMultiTransformation";
            var publicId2 = $"{m_apiTest2}_TestMultiTransformation";
            var tag = $"{m_apiTest}_MULTI";

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
            Assert.True(result.Uri.AbsoluteUri.EndsWith(".gif"));

            multi.Transformation = m_resizeTransformation;
            result = m_cloudinary.Multi(multi);
            Assert.IsTrue(result.Uri.AbsoluteUri.Contains("w_512"));

            multi.Transformation = m_simpleTransformationAngle;
            multi.Format = "pdf";
            result = m_cloudinary.Multi(multi);

            Assert.True(result.Uri.AbsoluteUri.Contains("a_45"));
            Assert.True(result.Uri.AbsoluteUri.EndsWith(".pdf"));
        }

        [Test]
        public void TestAspectRatioTransformation()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{m_apiTag},arTransformation",
                PublicId = $"arTransformation25_{m_apiTest}",
                Transformation = m_transformationAr25
            };
            ImageUploadResult iuResult25 = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(iuResult25);
            Assert.AreEqual(100, iuResult25.Width);
            Assert.AreEqual(40, iuResult25.Height);

            uploadParams.PublicId = $"arTransformation69_{m_apiTest}";
            uploadParams.Transformation = m_transformationAr69;
            ImageUploadResult iuResult69 = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(iuResult69);
            Assert.AreEqual(100, iuResult69.Width);
            Assert.AreEqual(150, iuResult69.Height);

            uploadParams.PublicId = $"arTransformation30_{m_apiTest}";
            uploadParams.Transformation = m_transformationAr30;
            ImageUploadResult iuResult30 = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(iuResult30);
            Assert.AreEqual(150, iuResult30.Width);
            Assert.AreEqual(50, iuResult30.Height);

            uploadParams.PublicId = $"arTransformation12_{m_apiTest}";
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
            var publicId = $"testexplode_{m_apiTest}";

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
            // should allow creating and listing upload_presets
            var preset1 = $"{m_apiTest1}_api_test_upload_preset";
            var preset2 = $"{m_apiTest2}_api_test_upload_preset";

            var preset = new UploadPresetParams()
            {
                Name = preset1,
                Folder = "folder",
                DisallowPublicId = true,
                Unsigned = true,
                Tags = m_apiTag,
                AllowedFormats = new string[] { "jpg", "bmp" }
            };

            var result = m_cloudinary.CreateUploadPreset(preset);

            preset = new UploadPresetParams()
            {
                Name = preset2,
                Folder = "folder2",
                Tags = $"{m_apiTag},a,b,c",
                Context = new StringDictionary("a=b", "c=d"),
                Transformation = m_simpleTransformation,
                EagerTransforms = new List<object>() { m_resizeTransformation },
                FaceCoordinates = "1,2,3,4"
            };

            result = m_cloudinary.CreateUploadPreset(preset);

            var presets = m_cloudinary.ListUploadPresets();

            Assert.AreEqual(presets.Presets[0].Name, preset2);
            Assert.AreEqual(presets.Presets[1].Name, preset1);

            var delResult = m_cloudinary.DeleteUploadPreset(preset1);
            Assert.AreEqual("deleted", delResult.Message);
            delResult = m_cloudinary.DeleteUploadPreset(preset2);
            Assert.AreEqual("deleted", delResult.Message);
        }

        [Test]
        public void TestGetUploadPreset()
        {
            // should allow getting a single upload_preset

            var @params = new UploadPresetParams()
            {
                Tags = $"a,b,c,{m_apiTag}",
                Name = $"{m_apiTest1}_upload_preset",
                Context = new StringDictionary("a=b", "c=d"),
                Transformation = m_simpleTransformation,
                EagerTransforms = new List<object>() { m_resizeTransformation },
                FaceCoordinates = "1,2,3,4",
                Unsigned = true,
                Folder = "folder",
                AllowedFormats = new string[] { "jpg", "pdf" }
            };

            var creationResult = m_cloudinary.CreateUploadPreset(@params);

            var preset = m_cloudinary.GetUploadPreset(creationResult.Name);

            Assert.AreEqual(creationResult.Name, preset.Name);
            Assert.AreEqual(true, preset.Unsigned);
            Assert.AreEqual("folder", preset.Settings.Folder);
            Assert.AreEqual("2", preset.Settings.Transformation[0]["width"].ToString());
            Assert.AreEqual("scale", preset.Settings.Transformation[0]["crop"].ToString());
        }

        [Test]
        public void TestDeleteUploadPreset()
        {
            // should allow deleting upload_presets
            var preset = $"{m_apiTest}_TestDeleteUploadPreset";

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
                Name = $"{m_apiTest2}_upload_preset",
                Context = new StringDictionary("a=b", "b=c"),
                Transformation = m_simpleTransformation,
                EagerTransforms = new List<object>() { m_resizeTransformation, m_updateTransformation },
                AllowedFormats = new string[] { "jpg", "png" },
                Tags = $"a,b,c,{m_apiTag}",
                FaceCoordinates = "1,2,3,4"
            };

            var presetName = m_cloudinary.CreateUploadPreset(presetToCreate).Name;

            var preset = m_cloudinary.GetUploadPreset(presetName);

            var presetToUpdate = new UploadPresetParams(preset);

            presetToUpdate.Colors = true;
            presetToUpdate.Unsigned = true;
            presetToUpdate.DisallowPublicId = true;

            var result = m_cloudinary.UpdateUploadPreset(presetToUpdate);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("updated", result.Message);

            preset = m_cloudinary.GetUploadPreset(presetName);

            Assert.AreEqual(presetName, preset.Name);
            Assert.AreEqual(true, preset.Unsigned);

            // TODO: compare settings of preset and presetToUpdate
        }

        [Test]
        public void TestUnsignedUpload()
        {
            // should support unsigned uploading using presets

            var preset = m_cloudinary.CreateUploadPreset(new UploadPresetParams()
            {
                Name = $"{m_apiTest2}_upload_preset_unsigned",
                Folder = "upload_folder",
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
            Assert.True(upload.PublicId.StartsWith("upload_folder"));
        }

        [Test]
        public void TestListResourcesStartAt()
        {
            // should allow listing resources by start date - make sure your clock is set correctly!!!

            Thread.Sleep(2000);

            DateTime start = DateTime.UtcNow;
            var publicId = $"{m_apiTag}_TestListResourcesStartAt";

            ImageUploadResult result = UploadTestResource(publicId);

            Thread.Sleep(2000);

            var resources = m_cloudinary.ListResources(
                new ListResourcesParams() { Type = "upload", StartAt = result.CreatedAt.AddMilliseconds(-10), Direction = "asc" });

            Assert.NotNull(resources.Resources, "response should include resources");
            Assert.IsTrue(resources.Resources.Length > 0, "response should include at least one resources");
            Assert.IsNotNull(resources.Resources.FirstOrDefault(res => res.PublicId == result.PublicId));
        }

        [Test]
        public void TestCustomCoordinates()
        {
            //should allow sending custom coordinates

            var coordinates = new Core.Rectangle(121, 31, 110, 151);

            var upResult = m_cloudinary.Upload(new ImageUploadParams() { File = new FileDescription(m_testImagePath), CustomCoordinates = coordinates, Tags = m_apiTag });

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

            var exResult = m_cloudinary.Explicit(new ExplicitParams(upResult.PublicId) { CustomCoordinates = coordinates, Type = "upload", Tags = m_apiTag });

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
            string publicId = $"{m_apiTest}_TestUpdateQuality";
            var upResult = m_cloudinary.Upload(new ImageUploadParams() { File = new FileDescription(m_testImagePath), PublicId = publicId, Overwrite = true, Tags = m_apiTag });
            var updResult = m_cloudinary.UpdateResource(new UpdateParams(upResult.PublicId) { QualityOveride = "auto:best" });
            Assert.AreEqual(updResult.StatusCode, HttpStatusCode.OK);
            Assert.Null(updResult.Error);
            Assert.AreEqual(updResult.PublicId, publicId);
        }

        // For this test to work, "Auto-create folders" should be enabled in the Upload Settings, so this test is disabled by default.
        public void TestFolderApi()
        {
            // should allow to list folders and subfolders

            m_cloudinary.Upload(new ImageUploadParams() { File = new FileDescription(m_testImagePath), PublicId = $"{m_folderPrefix}1/item", Tags = m_apiTag });
            m_cloudinary.Upload(new ImageUploadParams() { File = new FileDescription(m_testImagePath), PublicId = $"{m_folderPrefix}2/item", Tags = m_apiTag });
            m_cloudinary.Upload(new ImageUploadParams() { File = new FileDescription(m_testImagePath), PublicId = $"{m_folderPrefix}1/test_subfolder1/item", Tags = m_apiTag });
            m_cloudinary.Upload(new ImageUploadParams() { File = new FileDescription(m_testImagePath), PublicId = $"{m_folderPrefix}1/test_subfolder2/item", Tags = m_apiTag });

            var result = m_cloudinary.RootFolders();
            Assert.Null(result.Error);
            Assert.AreEqual($"{m_folderPrefix}1", result.Folders[0].Name);
            Assert.AreEqual($"{m_folderPrefix}2", result.Folders[1].Name);

            result = m_cloudinary.SubFolders($"{m_folderPrefix}1");

            Assert.AreEqual($"{m_folderPrefix}1/test_subfolder1", result.Folders[0].Path);
            Assert.AreEqual($"{m_folderPrefix}1/test_subfolder2", result.Folders[1].Path);

            result = m_cloudinary.SubFolders(m_folderPrefix);

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.NotNull(result.Error.Message);
            Assert.AreEqual("Can't find folder with path test_folder", result.Error.Message);

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
        public void TestResponsiveBreakpoints()
        {
            var publicId = $"{m_apiTest}_TestResponsiveBreakpoints";
            var breakpoint = new ResponsiveBreakpoint().MaxImages(5).BytesStep(20)
                                .MinWidth(200).MaxWidth(1000).CreateDerived(false);

            //var transformation = new Transformation().Width(0.9).Crop("scale").Radius(50);
            var breakpoint2 = new ResponsiveBreakpoint().Transformation(m_simpleTransformation).MaxImages(4).BytesStep(20)
                                .MinWidth(100).MaxWidth(900).CreateDerived(false);

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
                Type = "upload",
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

            uploadParams.AddCustomParam("public_id", "test_ad_hoc_params_id");
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
            string archiveTag = $"{m_apiTest}_archive_tag_{UnixTimeNow()}";
            string targetPublicId = $"{m_apiTest}_archive_id_{UnixTimeNow()}";

            ImageUploadResult res = UploadImageForTestArchive(archiveTag, 2.0, true);

            ArchiveParams parameters = new ArchiveParams()
                                            .Tags(new List<string> { archiveTag, "no_such_tag" })
                                            .TargetPublicId(targetPublicId)
                                            .TargetTags(new List<string> { m_apiTag });
            ArchiveResult result = m_cloudinary.CreateArchive(parameters);
            Assert.AreEqual(string.Format("{0}.zip", targetPublicId), result.PublicId);
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
            var tag = $"{m_apiTag}_TestCreateArchiveRawResources";
            RawUploadParams uploadParams = new RawUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Folder = "test_folder",
                Type = "private",
                Tags = $"{tag},{m_apiTag}"
            };

            RawUploadResult uploadResult1 = m_cloudinary.Upload(uploadParams, "raw");

            uploadParams.File = new FileDescription(m_testPdfPath);

            RawUploadResult uploadResult2 = m_cloudinary.Upload(uploadParams, "raw");

            ArchiveParams parameters = new ArchiveParams()
                                            .PublicIds(new List<string> { uploadResult1.PublicId, uploadResult2.PublicId })
                                            .ResourceType("raw")
                                            .Type("private")
                                            .UseOriginalFilename(true)
                                            .TargetTags(new List<string> { m_apiTag });
            ArchiveResult result = m_cloudinary.CreateArchive(parameters);
            Assert.AreEqual(2, result.FileCount);
        }

        private ArchiveParams UploadImageForArchiveAndPrepareParameters()
        {
            string archiveTag = $"{m_apiTag}_{UnixTimeNow()}"; 
            string targetPublicId = $"archive_id_{UnixTimeNow()}_{m_apiTest}";

            UploadImageForTestArchive($"{archiveTag},{m_apiTag}", 2.0, true);

            return new ArchiveParams().Tags(new List<string> { archiveTag, "non-existent-tag" }).TargetTags(new List<string> { m_apiTag }).TargetPublicId(targetPublicId);
        }

        [Test]
        public void TestCreateArchiveMultiplePublicIds()
        {
            // should support archiving based on multiple public IDs
            var parameters = UploadImageForArchiveAndPrepareParameters();
            var result = m_cloudinary.CreateArchive(parameters);

            Assert.AreEqual(string.Format("{0}.zip", parameters.TargetPublicId()), result.PublicId);
            Assert.AreEqual(1, result.FileCount);
        }

        /// <summary>
        /// Should create a zip archive
        /// </summary>
        [Test]
        public void TestCreateZip()
        {
            var parameters = UploadImageForArchiveAndPrepareParameters();
            var result = m_cloudinary.CreateZip(parameters);

            Assert.AreEqual(string.Format("{0}.zip", parameters.TargetPublicId()), result.PublicId);
            Assert.AreEqual(1, result.FileCount);
        }
        
        [Test]
        public void TestDownloadArchiveUrl()
        {
            var archiveTag = string.Format(string.Concat(m_apiTag, "_{0}"), UnixTimeNow());
            var parameters = new ArchiveParams().Tags(new List<string> { archiveTag });

            var urlStr = m_cloudinary.DownloadArchiveUrl(parameters);
            
            var dicQueryString = new Uri(urlStr).Query.Split('&').ToDictionary(
                c => Uri.UnescapeDataString(c.Split('=')[0]), c => Uri.UnescapeDataString(c.Split('=')[1])
            );
            
            Assert.AreEqual("download", dicQueryString["mode"]);
            Assert.AreEqual(archiveTag, dicQueryString["tags[]"]);
        }

        [Test]
        public void SearchResourceByTag()
        {
            string publicId = $"{m_apiTest}_TestForTagSearch";
            string tagForSearch = $"{m_apiTag}_TestForTagSearch";

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{tagForSearch},{m_apiTag}",
                PublicId = publicId,
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);
            Thread.Sleep(10000);
            var resource = m_cloudinary.GetResource(uploadResult.PublicId);
            
            Assert.NotNull(resource);
            Assert.AreEqual(resource.PublicId, publicId);

            var result = m_cloudinary.Search().Expression(string.Format("tags: {0}", tagForSearch)).Execute();
            Assert.True(result.TotalCount > 0);
            Assert.AreEqual(result.Resources[0].PublicId, resource.PublicId);
        }

        [Test]
        public void SearchResourceByPublicId()
        {
            string publicId = $"{m_apiTest}_SearchResourceByPublicId";

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Overwrite = true,
                Type = "private",
                Tags = m_apiTag
            };
            m_cloudinary.Upload(uploadParams);
            Thread.Sleep(10000);
            var result = m_cloudinary.Search().Expression(string.Format("public_id: {0}", publicId)).Execute();
            Assert.True(result.TotalCount > 0);
        }

        [Test]
        public void TestSearchResourceByExpression()
        {
            string publicId = $"{m_apiTest}_TestSearchResourceByExpression";

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId, 
                Overwrite = true,
                Type = "private",
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);
            Thread.Sleep(10000);

            SearchResult result = m_cloudinary.Search().Expression("resource_type: image").Execute();
            Assert.True(result.TotalCount > 0);  
            
            result = m_cloudinary.Search().Expression(string.Format("public_id: {0}", publicId)).Execute();
            Assert.True(result.TotalCount > 0);
        }

        [Test]
        public void TestClearAllTags()
        {
            var publicId = $"{m_apiTest}_TestClearAllTags";

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = "Tag1, Tag2, Tag3",
                PublicId = publicId,
                Overwrite = true,
                Type = "upload"
            };

            m_cloudinary.Upload(uploadParams);

            List<string> pIds = new List<string>();
            pIds.Add(publicId);

            m_cloudinary.Tag(new TagParams()
            {
                Command = TagCommand.RemoveAll,
                PublicIds = pIds,
                Type = "upload",

            });

            var getResResult = m_cloudinary.GetResource(new GetResourceParams(pIds[0])
            {
                PublicId = pIds[0],
                Type = "upload",
                ResourceType = ResourceType.Image
            });

            Assert.Null(getResResult.Tags);
        }

        [Test]
        public void TestAddContext()
        {
            var publicId = $"{m_apiTest}_TestAddContext";

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath), 
                PublicId = publicId,
                Overwrite = true,
                Type = "upload",
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            List<string> pIds = new List<string>();
            pIds.Add(publicId);

            ContextResult contextResult = m_cloudinary.Context(new ContextParams()
            {
                Command = ContextCommand.Add,
                PublicIds = pIds,
                Type = "upload",
                Context = "TestContext"
            });

            Assert.True(contextResult.PublicIds.Length > 0);

            m_cloudinary.GetResource(new GetResourceParams(pIds[0])
            {
                PublicId = pIds[0],
                Type = "upload",
                ResourceType = ResourceType.Image
            });
        }

        [OneTimeTearDown]
        public override void Cleanup()
        {
            base.Cleanup();
            m_cloudinary.DeleteTransform(m_updateTransformation.ToString());

            var transforms = new List<object>
            {
                m_explicitTransformation,
                m_updateTransformation,
                m_apiTest,
                m_apiTag,
                m_apiTest1,
                m_apiTest2, 
            };

            transforms.ForEach(t => m_cloudinary.DeleteTransform(t.ToString()));

            var presets = new[] {
                $"{m_apiTest1}_upload_preset",
                $"{m_apiTest2}_upload_preset",
                $"{m_apiTest2}_upload_preset_unsigned",
                $"{m_apiTest1}_api_test_upload_preset",
                $"{m_apiTest2}_api_test_upload_preset"
            };
            foreach (var preset in presets)
            {
                m_cloudinary.DeleteUploadPreset(preset);
            }

            var resources = new Dictionary<string, string>
            {
                { $"{m_apiTest1}_SAMPLE_TEXT"         ,"text" },
                { $"{m_apiTest2}_SAMPLE_TEXT"         ,"text" },
                { $"{m_apiTest}_MULTI"                 ,"multi" },
                { $"{m_apiTest}_MULTI,gif,h_512,w_512" ,"multi" },
                { $"{m_apiTest}_MULTI,pdf,a_45"        ,"multi" },
                { $"{m_apiTest1}_SPRITE"              ,"sprite" },
                { $"{m_apiTest1}_SPRITE,h_512,w_512"  ,"sprite" },
                { $"{m_apiTest1}_LOGO"                ,"sprite" },
            };
            var grouped = resources.GroupBy(gr => gr.Value).ToDictionary(r => r.Key, r => r.Select(t => t.Key).ToList());
            foreach (var resource in grouped)
            {
                m_cloudinary.DeleteResources(new DelResParams() { Type = resource.Key, ResourceType = ResourceType.Image, PublicIds = resource.Value });
            }
        }
    }
}
