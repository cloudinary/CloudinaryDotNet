using Cloudinary.Test.Configuration;
using CloudinaryDotNet.Actions;
using Ionic.Zip;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace CloudinaryDotNet.Test
{
    public class CloudinaryTest : IntegrationTestBase
    {
        [Test]
        public void TestUploadLocalImage()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath)
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
        public void TestUploadLocalImageTimeout()
        {
            var code = WebExceptionStatus.Success;
            var timeout = 11000;
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath)
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
            catch (WebException e)
            {
                code = e.Status;
                stopWatch.Stop();
                Console.WriteLine("Error {0}", e.Message);
            }
            finally
            {
                m_cloudinary.Api.ApiBaseAddress = origAddr;
                stopWatch.Stop();
            }

            //Assert.AreEqual(WebExceptionStatus.Timeout, code);
            Console.WriteLine("Elapsed {0}", stopWatch.ElapsedMilliseconds);
            Assert.LessOrEqual(timeout - 1000, stopWatch.ElapsedMilliseconds);
            Assert.GreaterOrEqual(timeout + 1000, stopWatch.ElapsedMilliseconds);

        }

        [Test]
        public void TestUploadLocalVideo()
        {
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(m_testVideoPath)
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
                File = new FileDescription(m_testImagePath)
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
                File = new FileDescription(m_testPdfPath)
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
                File = new FileDescription(m_testImagePath)
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
                File = new FileDescription(m_testImagePath)
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
                File = new FileDescription(m_testImagePath)
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
            m_cloudinary.DeleteResources(uploadResult.PublicId);

            uploadResult = m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Detection = "rekognition_face"
            });

            Assert.NotNull(uploadResult.Info);
            Assert.NotNull(uploadResult.Info.Detection);
            Assert.NotNull(uploadResult.Info.Detection.RekognitionFace);
            Assert.AreEqual("complete", uploadResult.Info.Detection.RekognitionFace.Status);
            m_cloudinary.DeleteResources(uploadResult.PublicId);
        }

        [Test]
        public void TestSimilaritySearchUpdate()
        {
            // should support requesting similarity search

            var uploadResult = m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath)
            });

            var updateResult = m_cloudinary.UpdateResource(new UpdateParams(uploadResult.PublicId)
            {
                SimilaritySearch = "illegal"
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, updateResult.StatusCode);
            Assert.True(updateResult.Error.Message.StartsWith("Illegal value"));
        }

        [Test]
        public void TestUploadOverwrite()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = "test_raw_overwrite" + new Random().Next(),
                Overwrite = false,
                Tags = m_test_tag
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
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(2.0) },
                PublicId = "test--3",
                Metadata = true,
                Exif = true,
                Colors = true
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

            var faceCoordinates = new List<CloudinaryDotNet.Core.Rectangle>()
            {
                new CloudinaryDotNet.Core.Rectangle(121,31,110,151),
                new CloudinaryDotNet.Core.Rectangle(120,30,109,150)
            };

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                FaceCoordinates = faceCoordinates,
                Faces = true
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
                Type = "upload"
            };

            var explicitRes = m_cloudinary.Explicit(explicitParams);

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
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(2.0) },
                EagerAsync = true,
                UseFilename = true,
                NotificationUrl = "http://www.google.com"
            };

            ImageUploadResult result = m_cloudinary.Upload(uploadParams);

            Assert.True(result.PublicId.StartsWith("TestImage"));
        }

        [Test]
        public void TestAutoTaggingRequest()
        {
            //should support requesting auto tagging

            var res = m_cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                AutoTagging = 0.5f
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, res.StatusCode);
            Assert.True(res.Error.Message.StartsWith("Must use"));
        }

        [Test]
        public void TestUploadLocalImageUniqueFilename()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(2.0) },
                EagerAsync = true,
                UseFilename = true,
                UniqueFilename = false,
                NotificationUrl = "http://www.google.com"
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
                Transformation = new Transformation().Width(512).Height(512),
                Tags = "transformation"
            };

            ImageUploadResult uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(512, uploadResult.Width);
            Assert.AreEqual(512, uploadResult.Height);
            Assert.AreEqual("jpg", uploadResult.Format);
        }

        [Test]
        public void TestEnglishText()
        {
            TextParams tParams = new TextParams("Sample text.");
            tParams.Background = "red";
            tParams.FontStyle = "italic";
            TextResult textResult = m_cloudinary.Text(tParams);

            Assert.IsTrue(textResult.Width > 0);
            Assert.IsTrue(textResult.Height > 0);
        }

        [Test]
        public void TestRussianText()
        {
            TextResult textResult = m_cloudinary.Text("Пример текста.");

            Assert.AreEqual(100, textResult.Width);
            Assert.AreEqual(13, textResult.Height);
        }

        [Test]
        public void TestDestroyRaw()
        {
            RawUploadParams uploadParams = new RawUploadParams()
            {
                File = new FileDescription(m_testImagePath)
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
                Tags = "remote"
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
                File = new FileDescription("data:image/png;base64,iVBORw0KGgoAA\nAANSUhEUgAAABAAAAAQAQMAAAAlPW0iAAAABlBMVEUAAAD///+l2Z/dAAAAM0l\nEQVR4nGP4/5/h/1+G/58ZDrAz3D/McH8yw83NDDeNGe4Ug9C9zwz3gVLMDA/A6\nP9/AFGGFyjOXZtQAAAAAElFTkSuQmCC")
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
                    Tags = "streamed"
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
                File = new FileDescription(largeFilePath)
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
                File = new FileDescription(largeFilePath)
            }, 5 * 1024 * 1024);

            Assert.AreEqual(fileLength, result.Length);
        }

        [Test]
        public void TestPublishByTag()
        {
            string publicId = "TestForPublish" + m_suffix;
            
            var uploadParams = new ImageUploadParams()
            {  
                File = new FileDescription(m_testImagePath),
                Tags = publicId,
                PublicId = publicId,
                Overwrite = true,
                Type = "private"
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            var publish_result = m_cloudinary.PublishResourceByTag(publicId, new PublishResourceParams()
            {
                ResourceType = ResourceType.Image
            });



            DelResResult delResult = m_cloudinary.DeleteResourcesByTag(
                "TestForPublish");

            Assert.AreEqual(publish_result.Published.Count, 1);
        }

        [Test]
        public void TestUpdateAccessModeByTag()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = "TestForUpdateAccessMode",
                PublicId = "TestForUpdateAccessMode",
                Overwrite = true,
                Type = "private"
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            var update_result = m_cloudinary.UpdateResourceAccessModeByTag("TestForUpdateAccessMode", new UpdateResourceAccessModeParams()
            {
                ResourceType = ResourceType.Image,
                Type = "upload",
                AccessMode = "public"
            });

            //Assert.AreEqual(publish_result.Published.Count, 1);

            DelResResult delResult = m_cloudinary.DeleteResourcesByTag(
                "TestForUpdateAccessMode");

        }

        [Test]
        public void TestUpdateAccessModeById()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = "TestForUpdateAccessMode",
                PublicId = "TestForUpdateAccessMode",
                Overwrite = true,
                Type = "private"
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            List<string> ids = new List<string>();
            ids.Add("TestForUpdateAccessMode");

            var update_result = m_cloudinary.UpdateResourceAccessModeByIds(new UpdateResourceAccessModeParams()
            {
                ResourceType = ResourceType.Image,
                Type = "upload",
                AccessMode = "public",
                PublicIds = ids
            });

            //Assert.AreEqual(publish_result.Published.Count, 1);

            DelResResult delResult = m_cloudinary.DeleteResourcesByTag(
                "TestForUpdateAccessMode");

        }

        [Test]
        public void TestUploadLargeFromWeb()
        {
            // support uploading large image

            var largeFilePath = "http://res.cloudinary.com/demo/video/upload/v1496743637/dog.mp4";
            var result = m_cloudinary.UploadLarge(new ImageUploadParams()
            {
                File = new FileDescription(largeFilePath)
            }, 5 * 1024 * 1024);

            Assert.AreEqual(result.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.AreEqual(result.Format, "mp4");
        }

        //[Test]
        //public void RenameParams()
        //{
        //    RenameParams rp = new RenameParams("async_test_vid", "fold1/async_test_vid");
        //    rp.AddCustomParam("resource_type", "video");
        //    RenameResult res = m_cloudinary.Rename(rp);
        //    Assert.AreEqual(res.StatusCode, System.Net.HttpStatusCode.OK);
        //}

        [Test]
        public void TestTagAdd()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath)
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

        [Test]
        public void TestTagMultiple()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath)
            };

            var uploadResult1 = m_cloudinary.Upload(uploadParams);
            var uploadResult2 = m_cloudinary.Upload(uploadParams);

            var tagParams = new TagParams()
            {
                PublicIds = new List<string>() {
                    uploadResult1.PublicId,
                    uploadResult2.PublicId
                },
                Tag = "tag1"
            };

            m_cloudinary.Tag(tagParams);

            // remove second ID
            tagParams.PublicIds.RemoveAt(1);
            tagParams.Tag = "tag2";

            m_cloudinary.Tag(tagParams);

            var r = m_cloudinary.GetResource(uploadResult1.PublicId);
            Assert.NotNull(r.Tags);
            Assert.True(r.Tags.SequenceEqual(new string[] { "tag1", "tag2" }));

            r = m_cloudinary.GetResource(uploadResult2.PublicId);
            Assert.NotNull(r.Tags);
            Assert.True(r.Tags.SequenceEqual(new string[] { "tag1" }));

            tagParams.Command = TagCommand.Remove;
            tagParams.Tag = "tag1";
            tagParams.PublicIds = new List<string>() { uploadResult1.PublicId };

            m_cloudinary.Tag(tagParams);

            r = m_cloudinary.GetResource(uploadResult1.PublicId);
            Assert.NotNull(r.Tags);
            Assert.True(r.Tags.SequenceEqual(new string[] { "tag2" }));

            tagParams.Command = TagCommand.Replace;
            tagParams.Tag = "tag3";

            m_cloudinary.Tag(tagParams);

            r = m_cloudinary.GetResource(uploadResult1.PublicId);
            Assert.NotNull(r.Tags);
            Assert.True(r.Tags.SequenceEqual(new string[] { "tag3" }));
        }

        [Test]
        public void TestTagReplace()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = "test++++++tag"
            };

            ImageUploadResult uploadResult = m_cloudinary.Upload(uploadParams);

            TagParams tagParams = new TagParams()
            {
                Command = TagCommand.Replace,
                Tag = "another-tag-test"
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
            Assert.IsTrue(result.ResourceTypes.Contains(ResourceType.Image));
        }

        [Test, Ignore("test needs to be re-written with mocking - it fails when there are many resources")]
        public void TestListResources()
        {
            // should allow listing resources

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = "testlistresources",
                Tags = "hello"
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);
            IEnumerable<Resource> resources = new Resource[0];
            resources = GetAllResults((cursor) => m_cloudinary.ListResources(cursor));
            Assert.IsTrue(resources.Where(res => res.PublicId == uploadParams.PublicId && res.Type == "upload" && res.Tags.Count() == 1 && res.Tags[0] == "hello").Count() > 0);
        }

        protected IEnumerable<Resource> GetAllResults(Func<String, ListResourcesResult> list)
        {
            ListResourcesResult current = list(null);
            IEnumerable<Resource> resources = current.Resources;
            for (; resources != null && current.NextCursor != null; current = list(current.NextCursor))
            {
                resources = resources.Concat(current.Resources);
            }

            return resources;
        }

        [Test, Ignore("test needs to be re-written with mocking - it fails when there are many resources")]
        public void TestListResourcesByType()
        {
            // should allow listing resources by type

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = "testlistresourcesbytype"
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

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = "testlistblablabla",
                Context = new StringDictionary("context=abc")
            };

            m_cloudinary.Upload(uploadParams);

            var result = m_cloudinary.ListResourcesByPrefix("testlist", true, true, true);

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
                PublicId = "test_context",
                Context = new StringDictionary("key=value", "key2=value2")
            };

            var uploaded = m_cloudinary.Upload(uploadParams);

            var res = m_cloudinary.GetResource(uploaded.PublicId);

            Assert.AreEqual("value", res.Context["custom"]["key"].ToString());
            Assert.AreEqual("value2", res.Context["custom"]["key2"].ToString());
        }

        [Test]
        public void TestListResourcesByPublicIds()
        {
            // should allow listing resources by public ids

            List<string> publicIds = new List<string>()
                {
                    "testlistresources",
                    "testlistblablabla",
                    "test_context"
                };
            var result = m_cloudinary.ListResourceByPublicIds(publicIds, true, true, true);

            Assert.NotNull(result);
            Assert.AreEqual(3, result.Resources.Length, "expected to find {0} but got {1}", new Object[] { publicIds.Aggregate((current, next) => current + ", " + next), result.Resources.Select(r => r.PublicId).Aggregate((current, next) => current + ", " + next) });
            Assert.True(result.Resources.Where(r => r.Tags != null && r.Tags.Length > 0 && r.Tags[0] == "hello").Count() == 1);
            Assert.True(result.Resources.Where(r => r.Context != null).Count() == 2);
        }

        [Test]
        public void TestListResourcesByTag()
        {
            // should allow listing resources by tag

            var file = new FileDescription(m_testImagePath);
            var tag = "teslistresourcesbytag1";
            m_cloudinary.DeleteResourcesByTag(tag);
            var uploadParams = new ImageUploadParams()
            {
                File = file,
                Tags = tag + ",beauty"
            };

            m_cloudinary.Upload(uploadParams);

            uploadParams = new ImageUploadParams()
            {
                File = file,
                Tags = tag
            };

            m_cloudinary.Upload(uploadParams);
            var result = m_cloudinary.ListResourcesByTag(tag);
            Assert.AreEqual(2, result.Resources.Count());
            m_cloudinary.DeleteResourcesByTag(tag);
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
                    Moderation = "manual"
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

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = "testlistresources1"
            };

            m_cloudinary.Upload(uploadParams);

            uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = "testlistresources2"
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
                EagerTransforms = new List<Transformation>() {
                    new Transformation().Width(100),
                    new EagerTransformation(
                        new Transformation().Width(10),
                        new Transformation().Angle(10)).SetFormat("png")
                },
                Tags = "eager,transformation"
            };

            m_cloudinary.Upload(uploadParams);
        }

        [Test]
        public void TestRename()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath)
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
        public void TestGetResource()
        {
            // should allow get resource details
            String publicId = "testgetresource" + m_suffix;
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(2.0) },
                PublicId = publicId
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
            ///Assert.IsNotNullOrEmpty(getResult.Phash);
        }

        [Test]
        public void TestGetResourceWithMetadata()
        {
            // should allow get resource metadata

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(2.0) },
                PublicId = "testgetresource2"
            };

            m_cloudinary.Upload(uploadParams);

            GetResourceResult getResult = m_cloudinary.GetResource(
                new GetResourceParams("testgetresource2")
                {
                    Metadata = true
                });

            Assert.IsNotNull(getResult);
            Assert.AreEqual("testgetresource2", getResult.PublicId);
            Assert.NotNull(getResult.Metadata);
        }

        [Test]
        public void TestDeleteDerived()
        {
            // should allow deleting derived resource

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { new Transformation().Width(101).Crop("scale") },
                PublicId = "testdeletederived"
            };

            m_cloudinary.Upload(uploadParams);

            GetResourceResult resource = m_cloudinary.GetResource("testdeletederived");

            Assert.IsNotNull(resource);
            Assert.AreEqual(1, resource.Derived.Length);

            DelDerivedResResult delDerivedResult =
                m_cloudinary.DeleteDerivedResources(resource.Derived[0].Id);

            Assert.AreEqual(1, delDerivedResult.Deleted.Values.Count);

            resource = m_cloudinary.GetResource("testdeletederived");

            Assert.IsFalse(String.IsNullOrEmpty(resource.PublicId));
        }

        [Test]
        public void TestDelete()
        {
            // should allow deleting resources

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = "testdelete",
            };

            m_cloudinary.Upload(uploadParams);

            GetResourceResult resource = m_cloudinary.GetResource("testdelete");

            Assert.IsNotNull(resource);
            Assert.AreEqual("testdelete", resource.PublicId);

            DelResResult delResult = m_cloudinary.DeleteResources(
                "randomstringopa", "testdeletederived", "testdelete");

            Assert.AreEqual("not_found", delResult.Deleted["randomstringopa"]);
            Assert.AreEqual("deleted", delResult.Deleted["testdelete"]);

            resource = m_cloudinary.GetResource("testdelete");

            Assert.IsTrue(String.IsNullOrEmpty(resource.PublicId));
        }

        [Test]
        public void TestDeleteByPrefix()
        {
            // should allow deleting resources

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = "testdelete"
            };

            m_cloudinary.Upload(uploadParams);

            GetResourceResult resource = m_cloudinary.GetResource("testdelete");

            Assert.IsNotNull(resource);
            Assert.AreEqual("testdelete", resource.PublicId);

            DelResResult delResult = m_cloudinary.DeleteResourcesByPrefix(
                "testdel");

            resource = m_cloudinary.GetResource("testdelete");

            Assert.IsTrue(String.IsNullOrEmpty(resource.PublicId));
        }

        [Test]
        public void TestDeleteByTag()
        {
            // should allow deleting resources

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = "api_test4",
                Tags = "api_test_tag_for_delete"
            };

            m_cloudinary.Upload(uploadParams);

            GetResourceResult resource = m_cloudinary.GetResource(
                "api_test4");

            Assert.IsNotNull(resource);
            Assert.AreEqual("api_test4", resource.PublicId);

            DelResResult delResult = m_cloudinary.DeleteResourcesByTag(
                "api_test_tag_for_delete");

            resource = m_cloudinary.GetResource("api_test4");

            Assert.IsTrue(String.IsNullOrEmpty(resource.PublicId));
        }

        [Test]
        public void TestRestoreNoBackup()
        {
            const string TEST_PUBLIC_ID = "testdelandrestore_nobackup";

            ImageUploadParams uploadParams_nobackup = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = TEST_PUBLIC_ID
            };

            m_cloudinary.Upload(uploadParams_nobackup);
            GetResourceResult resource = m_cloudinary.GetResource(TEST_PUBLIC_ID);
            Assert.IsNotNull(resource);
            Assert.AreEqual(TEST_PUBLIC_ID, resource.PublicId);

            DelResResult delResult = m_cloudinary.DeleteResources(TEST_PUBLIC_ID);
            Assert.AreEqual("deleted", delResult.Deleted[TEST_PUBLIC_ID]);

            resource = m_cloudinary.GetResource(TEST_PUBLIC_ID);
            Assert.IsTrue(string.IsNullOrEmpty(resource.PublicId));

            RestoreResult rResult = m_cloudinary.Restore(TEST_PUBLIC_ID);
            Assert.IsNotNull(rResult.JsonObj[TEST_PUBLIC_ID], string.Format("Should contain key \"{0}\". ", TEST_PUBLIC_ID));
            Assert.AreEqual("no_backup", rResult.JsonObj[TEST_PUBLIC_ID]["error"].ToString());
        }

        [Test]
        public void TestRestore()
        {
            const string TEST_PUBLIC_ID = "delete_restore";

            ImageUploadParams uploadParams_backup = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = TEST_PUBLIC_ID,
                Backup = true
            };

            m_cloudinary.Upload(uploadParams_backup);
            GetResourceResult resource_backup = m_cloudinary.GetResource(TEST_PUBLIC_ID);
            Assert.IsNotNull(resource_backup);
            Assert.AreEqual(TEST_PUBLIC_ID, resource_backup.PublicId);

            DelResResult delResult_backup = m_cloudinary.DeleteResources(TEST_PUBLIC_ID);
            Assert.AreEqual("deleted", delResult_backup.Deleted[TEST_PUBLIC_ID]);

            resource_backup = m_cloudinary.GetResource(TEST_PUBLIC_ID);
            Assert.AreEqual(0, resource_backup.Length);

            RestoreResult rResult_backup = m_cloudinary.Restore(TEST_PUBLIC_ID);
            Assert.IsNotNull(rResult_backup.JsonObj[TEST_PUBLIC_ID], string.Format("Should contain key \"{0}\". ", TEST_PUBLIC_ID));
            Assert.AreEqual(TEST_PUBLIC_ID, rResult_backup.JsonObj[TEST_PUBLIC_ID]["public_id"].ToString());

            resource_backup = m_cloudinary.GetResource(TEST_PUBLIC_ID);
            Assert.IsFalse(string.IsNullOrEmpty(resource_backup.PublicId));
        }

        [Test]
        public void TestListTags()
        {
            // should allow listing tags

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = "api_test_custom"
            };

            m_cloudinary.Upload(uploadParams);

            ListTagsResult result = m_cloudinary.ListTags();

            Assert.IsTrue(result.Tags.Contains("api_test_custom"));
        }

        [Test]
        public void TestAllowedFormats()
        {
            //should allow whitelisted formats if allowed_formats
            
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                AllowedFormats = new string[] { "jpg" },
                Tags = m_test_tag
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
                AllowedFormats = new string[] { "png" }
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
                Format = "png"
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
                Moderation = "manual"
            });

            Assert.NotNull(uploadResult);

            var updateResult = m_cloudinary.UpdateResource(new UpdateParams(uploadResult.PublicId) { ModerationStatus = Actions.ModerationStatus.Approved });

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

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = "api_test_custom1"
            };

            m_cloudinary.Upload(uploadParams);

            uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = "api_test_brustom"
            };

            m_cloudinary.Upload(uploadParams);

            ListTagsResult result = m_cloudinary.ListTagsByPrefix("api_test");

            Assert.IsTrue(result.Tags.Contains("api_test_brustom"));

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
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(100) },
                Tags = "transformation"
            };

            m_cloudinary.Upload(uploadParams);

            ListTransformsResult result = m_cloudinary.ListTransformations();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Transformations);
            TransformDesc td = result.Transformations.Where(t => t.Name == "c_scale,w_100").First();
            Assert.IsTrue(td.Used);
        }

        [Test]
        public void TestGetTransform()
        {
            // should allow getting transformation metadata

            var t = new Transformation().Crop("scale").Dpr(1.3).Width(2.0);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { t },
                Tags = "transformation"
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            var result = m_cloudinary.GetTransform("c_scale,dpr_1.3,w_2.0");

            Assert.IsNotNull(result);
            Assert.AreEqual(t.Generate(), new Transformation(result.Info[0]).Generate());
        }

        [Test]
        public void TestUpdateTransformStrict()
        {
            // should allow updating transformation allowed_for_strict

            Transformation t = new Transformation().Crop("scale").Width(100);
            string tags = string.Concat(m_test_tag, "_transformation");

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { t },
                Tags = tags
            };

            m_cloudinary.Upload(uploadParams);

            UpdateTransformParams updateParams = new UpdateTransformParams()
            {
                Transformation = "c_scale,w_100",
                Strict = true
            };

            UpdateTransformResult result = m_cloudinary.UpdateTransform(updateParams);

            GetTransformResult getResult = m_cloudinary.GetTransform("c_scale,w_100");

            Assert.IsNotNull(getResult);
            Assert.AreEqual(true, getResult.Strict);

            updateParams.Strict = false;
            m_cloudinary.UpdateTransform(updateParams);

            getResult = m_cloudinary.GetTransform("c_scale,w_100");

            Assert.IsNotNull(getResult);
            Assert.AreEqual(false, getResult.Strict);
        }

        [Test]
        public void TestUpdateTransformUnsafe()
        {
            // should allow unsafe update of named transformation

            var r = m_cloudinary.CreateTransform(
                new CreateTransformParams()
                {
                    Name = "api_test_transformation3",
                    Transform = new Transformation().Crop("scale").Width(102)
                });

            var updateParams = new UpdateTransformParams()
            {
                Transformation = "api_test_transformation3",
                UnsafeTransform = new Transformation().Crop("scale").Width(103)
            };

            var result = m_cloudinary.UpdateTransform(updateParams);

            var getResult = m_cloudinary.GetTransform("api_test_transformation3");

            Assert.IsNotNull(getResult.Info);
            Assert.AreEqual(updateParams.UnsafeTransform.Generate(), new Transformation(getResult.Info).Generate());
            //Assert.IsFalse(getResult.Used);
        }

        [Test]
        public void TestCreateTransform()
        {
            // should allow creating named transformation

            Transformation t = new Transformation().Crop("scale").Width(102);

            CreateTransformParams create = new CreateTransformParams()
            {
                Name = "api_test_transformation",
                Transform = t
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
            Assert.AreEqual(t.Generate(), new Transformation(getResult.Info[0]).Generate());
        }

        [Test]
        public void TestDeleteTransform()
        {
            // should allow deleting named transformation

            m_cloudinary.DeleteTransform("api_test_transformation2");

            CreateTransformParams create = new CreateTransformParams()
            {
                Name = "api_test_transformation2",
                Transform = new Transformation().Crop("scale").Width(103)
            };

            TransformResult createResult = m_cloudinary.CreateTransform(create);

            Assert.AreEqual("created", createResult.Message);

            m_cloudinary.DeleteTransform("api_test_transformation2");

            GetTransformResult getResult = m_cloudinary.GetTransform(
                new GetTransformParams() { Transformation = "api_test_transformation2" });

            Assert.AreEqual(HttpStatusCode.NotFound, getResult.StatusCode);
        }

        [Test]
        public void TestDeleteTransformImplicit()
        {
            // should allow deleting implicit transformation

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(100) }
            };

            m_cloudinary.Upload(uploadParams);

            GetTransformParams getParams = new GetTransformParams()
            {
                Transformation = "c_scale,w_100"
            };

            GetTransformResult getResult = m_cloudinary.GetTransform(getParams);

            Assert.AreEqual(HttpStatusCode.OK, getResult.StatusCode);

            m_cloudinary.DeleteTransform("c_scale,w_100");

            getResult = m_cloudinary.GetTransform(getParams);

            Assert.AreEqual(HttpStatusCode.NotFound, getResult.StatusCode);
        }

        [Test]
        public void TestUploadHeaders()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = "headers"
            };

            uploadParams.Headers = new Dictionary<string, string>();
            uploadParams.Headers.Add("Link", "1");
            uploadParams.Headers.Add("Blink", "182");

            m_cloudinary.Upload(uploadParams);
        }

        //[Test]
        //public void TestAgentPlatformHeaders()
        //{
        //    HttpWebRequest request = null;
        //    Func<string, HttpWebRequest> requestBuilder = (x) =>
        //    {
        //        request = HttpWebRequest.Create(x) as HttpWebRequest;
        //        return request;
        //    };
        //    m_cloudinary.Api.RequestBuilder = requestBuilder;
        //    m_cloudinary.Api.UserPlatform = "Test/1.0";
        //    m_cloudinary.RootFolders();

        //    //Can't test the result, so we just verify the UserAgent parameter is sent to the server
        //    StringAssert.AreEqualIgnoringCase(string.Format("{0} {1}", m_cloudinary.Api.UserPlatform, Api.USER_AGENT), request.UserAgent);
        //    StringAssert.IsMatch(@"Test\/1\.0 CloudinaryDotNet\/(\d+)\.(\d+)\.(\d+)", request.UserAgent);
        //}

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
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(2.0) },
                Type = "facebook"
            };

            ExplicitResult expResult = m_cloudinary.Explicit(exp);

            string url = new Url(m_account.Cloud).ResourceType("image").Add("facebook").
                Transform(new Transformation().Crop("scale").Width(2.0)).
                Format("png").Version(expResult.Version).BuildUrl("cloudinary");

            Assert.AreEqual(url, expResult.Eager[0].Uri.AbsoluteUri);
        }

        [Test]
        public void TestExplicitContext()
        {
            var exp = new ExplicitParams("cloudinary")
            {
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(2.0) },
                Type = "facebook",
                Context = new StringDictionary("context1=254")
            };

            var expResult = m_cloudinary.Explicit(exp);

            Assert.IsNotNull(expResult);

            var getResult = m_cloudinary.GetResource(new GetResourceParams(expResult.PublicId) { Type = "facebook" });

            Assert.IsNotNull(getResult);
            Assert.AreEqual("254", getResult.Context["custom"]["context1"].ToString());
        }

        [Test]
        public void TestSprite()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = "logo,beauty",
                PublicId = "logo1",
                Transformation = new Transformation().Width(200).Height(100)
            };

            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = "logo2";
            uploadParams.Transformation = new Transformation().Width(100).Height(100);

            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = "logo3";
            uploadParams.Transformation = new Transformation().Width(100).Height(300);

            m_cloudinary.Upload(uploadParams);

            SpriteParams sprite = new SpriteParams("logo");

            SpriteResult result = m_cloudinary.MakeSprite(sprite);

            Assert.NotNull(result);
            Assert.NotNull(result.ImageInfos);
            Assert.AreEqual(3, result.ImageInfos.Count);
            Assert.Contains("logo1", result.ImageInfos.Keys);
            Assert.Contains("logo2", result.ImageInfos.Keys);
            Assert.Contains("logo3", result.ImageInfos.Keys);
        }

        [Test]
        public void TestSpriteTransformation()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = "logotrans",
                PublicId = "logotrans1",
                Transformation = new Transformation().Width(200).Height(100)
            };

            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = "logotrans2";
            uploadParams.Transformation = new Transformation().Width(100).Height(100);

            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = "logotrans3";
            uploadParams.Transformation = new Transformation().Width(100).Height(300);

            m_cloudinary.Upload(uploadParams);

            SpriteParams sprite = new SpriteParams("logotrans");
            sprite.Transformation = new Transformation().Width(100).Height(100).Crop("scale");

            SpriteResult result = m_cloudinary.MakeSprite(sprite);

            Assert.NotNull(result);
            Assert.NotNull(result.ImageInfos);
            foreach (var item in result.ImageInfos)
            {
                Assert.AreEqual(100, item.Value.Width);
                Assert.AreEqual(100, item.Value.Height);
            }
        }

        [Test]
        public void TestJsonObject()
        {
            ExplicitParams exp = new ExplicitParams("cloudinary")
            {

                EagerTransforms = new List<Transformation>() {
                    new EagerTransformation().Crop("scale").Width(2.0) },
                Type = "facebook"
            };

            var result = m_cloudinary.Explicit(exp);

            Assert.NotNull(result.JsonObj);
            Assert.AreEqual(result.PublicId, result.JsonObj["public_id"].ToString());
        }

        [Test]
        public void TestUsage()
        {
            UploadTestResource("TestUsage"); // making sure at least one resource exists
            var result = m_cloudinary.GetUsage();
            DeleteTestResource("TestUsage");

            var plans = new List<string>() { "Free", "Advanced" };

            Assert.True(plans.Contains(result.Plan));
            Assert.True(result.Resources > 0);
            Assert.True(result.Objects.Used < result.Objects.Limit);
            Assert.True(result.Bandwidth.Used < result.Bandwidth.Limit);

        }

        [Test]
        public void TestMultiTransformation()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = "test--5",
                PublicId = "test--5-1"
            };

            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = "test--5-2";
            uploadParams.Transformation = new Transformation().Width(100).Height(300);

            m_cloudinary.Upload(uploadParams);

            MultiParams multi = new MultiParams("test--5");
            MultiResult result = m_cloudinary.Multi(multi);
            Assert.True(result.Uri.AbsoluteUri.EndsWith(".gif"));

            multi.Transformation = new Transformation().Width(100);
            result = m_cloudinary.Multi(multi);
            Assert.True(result.Uri.AbsoluteUri.Contains("w_100"));

            multi.Transformation = new Transformation().Width(111);
            multi.Format = "pdf";
            result = m_cloudinary.Multi(multi);
            Assert.True(result.Uri.AbsoluteUri.Contains("w_111"));
            Assert.True(result.Uri.AbsoluteUri.EndsWith(".pdf"));
        }

        [Test]
        public void TestAspectRatioTransformation()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = "arTransformation",
                PublicId = "arTransformation25",
                Transformation = new Transformation().Width(100).AspectRatio(2.5)
            };
            ImageUploadResult iuResult25 = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(iuResult25);
            Assert.AreEqual(100, iuResult25.Width);
            Assert.AreEqual(40, iuResult25.Height);

            uploadParams.PublicId = "arTransformation69";
            uploadParams.Transformation = new Transformation().Width(100).AspectRatio(6, 9);
            ImageUploadResult iuResult69 = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(iuResult69);
            Assert.AreEqual(100, iuResult69.Width);
            Assert.AreEqual(150, iuResult69.Height);

            uploadParams.PublicId = "arTransformation30";
            uploadParams.Transformation = new Transformation().Width(150).AspectRatio("3.0");
            ImageUploadResult iuResult30 = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(iuResult30);
            Assert.AreEqual(150, iuResult30.Width);
            Assert.AreEqual(50, iuResult30.Height);

            uploadParams.PublicId = "arTransformation12";
            uploadParams.Transformation = new Transformation().Width(100).AspectRatio("1:2");
            ImageUploadResult iuResult12 = m_cloudinary.Upload(uploadParams);

            Assert.NotNull(iuResult12);
            Assert.AreEqual(100, iuResult12.Width);
            Assert.AreEqual(200, iuResult12.Height);
        }

        [Test]
        public void TestJsConfig()
        {
            string config = m_cloudinary.GetCloudinaryJsConfig().ToString();

            Assert.AreEqual(
                "<script src=\"/Scripts/jquery.ui.widget.js\"></script>\r\n" +
                "<script src=\"/Scripts/jquery.iframe-transport.js\"></script>\r\n" +
                "<script src=\"/Scripts/jquery.fileupload.js\"></script>\r\n" +
                "<script src=\"/Scripts/jquery.cloudinary.js\"></script>\r\n" +
                "<script type='text/javascript'>\r\n" +
                "$.cloudinary.config({\r\n" +
                "  \"cloud_name\": \"" + m_account.Cloud + "\",\r\n" +
                "  \"api_key\": \"" + m_account.ApiKey + "\",\r\n" +
                "  \"private_cdn\": false,\r\n" +
                "  \"cdn_subdomain\": false\r\n" +
                "});\r\n" +
                "</script>\r\n", config);
        }

        [Test]
        public void TestJsConfigFull()
        {
            string config = m_cloudinary.GetCloudinaryJsConfig(true, @"https://raw.github.com/cloudinary/cloudinary_js/master/js").ToString();

            Assert.AreEqual(
                "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.ui.widget.js\"></script>\r\n" +
                "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.iframe-transport.js\"></script>\r\n" +
                "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.fileupload.js\"></script>\r\n" +
                "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.cloudinary.js\"></script>\r\n" +
                "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/canvas-to-blob.min.js\"></script>\r\n" +
                "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.fileupload-image.js\"></script>\r\n" +
                "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.fileupload-process.js\"></script>\r\n" +
                "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/jquery.fileupload-validate.js\"></script>\r\n" +
                "<script src=\"https://raw.github.com/cloudinary/cloudinary_js/master/js/load-image.min.js\"></script>\r\n" +
                "<script type='text/javascript'>\r\n" +
                "$.cloudinary.config({\r\n" +
                "  \"cloud_name\": \"" + m_account.Cloud + "\",\r\n" +
                "  \"api_key\": \"" + m_account.ApiKey + "\",\r\n" +
                "  \"private_cdn\": false,\r\n" +
                "  \"cdn_subdomain\": false\r\n" +
                "});\r\n" +
                "</script>\r\n", config);
        }

        [Test]
        public void TestExplode()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testPdfPath),
                PublicId = "testexplode"
            };

            m_cloudinary.Upload(uploadParams);

            var result = m_cloudinary.Explode(new ExplodeParams(
                "testexplode",
                new Transformation().Page("all")));

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

            var preset = new UploadPresetParams()
            {
                Name = "api_test_upload_preset",
                Folder = "folder",
                DisallowPublicId = true,
                Unsigned = true,
                AllowedFormats = new string[] { "jpg", "bmp" }
            };

            var result = m_cloudinary.CreateUploadPreset(preset);

            preset = new UploadPresetParams()
            {
                Name = "api_test_upload_preset2",
                Folder = "folder2",
                Tags = "a,b,c",
                Context = new StringDictionary("a=b", "c=d"),
                Transformation = new Transformation().Width(100).Crop("scale"),
                EagerTransforms = new List<object>() { new Transformation().X(100) },
                FaceCoordinates = "1,2,3,4"
            };

            result = m_cloudinary.CreateUploadPreset(preset);

            var presets = m_cloudinary.ListUploadPresets();

            Assert.AreEqual(presets.Presets[0].Name, "api_test_upload_preset2");
            Assert.AreEqual(presets.Presets[1].Name, "api_test_upload_preset");

            var delResult = m_cloudinary.DeleteUploadPreset("api_test_upload_preset");
            Assert.AreEqual("deleted", delResult.Message);
            delResult = m_cloudinary.DeleteUploadPreset("api_test_upload_preset2");
            Assert.AreEqual("deleted", delResult.Message);
        }

        [Test]
        public void TestGetUploadPreset()
        {
            // should allow getting a single upload_preset

            var tags = new string[] { "a", "b", "c" };

            var @params = new UploadPresetParams()
            {
                Tags = String.Join(",", tags),
                Context = new StringDictionary("a=b", "c=d"),
                Transformation = new Transformation().Width(100).Crop("scale"),
                EagerTransforms = new List<object>() { new Transformation().X(100) },
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
            Assert.AreEqual("100", preset.Settings.Transformation[0]["width"].ToString());
            Assert.AreEqual("scale", preset.Settings.Transformation[0]["crop"].ToString());

            m_cloudinary.DeleteUploadPreset(preset.Name);
        }

        [Test]
        public void TestDeleteUploadPreset()
        {
            // should allow deleting upload_presets

            m_cloudinary.CreateUploadPreset(new UploadPresetParams()
            {
                Name = "api_test_upload_preset4",
                Folder = "folder"
            });

            var result = m_cloudinary.DeleteUploadPreset("api_test_upload_preset4");

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            result = m_cloudinary.DeleteUploadPreset("api_test_upload_preset4");

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public void TestUpdateUploadPreset()
        {
            // should allow updating upload presets

            var presetToCreate = new UploadPresetParams()
            {
                Folder = "folder",
                Context = new StringDictionary("a=b", "b=c"),
                Transformation = new Transformation().X(100),
                EagerTransforms = new List<object>() { new Transformation().X(100).Y(100), "w_50" },
                AllowedFormats = new string[] { "jpg", "png" },
                Tags = "a,b,c",
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

            m_cloudinary.DeleteUploadPreset(preset.Name);
        }

        [Test]
        public void TestUnsignedUpload()
        {
            // should support unsigned uploading using presets

            var preset = m_cloudinary.CreateUploadPreset(new UploadPresetParams()
            {
                Folder = "upload_folder",
                Unsigned = true
            });

            var acc = new Account(CloudinarySettings.Settings.CloudName);
            var cloudinary = new Cloudinary(acc);

            var upload = cloudinary.Upload(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                UploadPreset = preset.Name,
                Unsigned = true
            });

            Assert.NotNull(upload.PublicId);
            Assert.True(upload.PublicId.StartsWith("upload_folder"));

            m_cloudinary.DeleteUploadPreset(preset.Name);
        }

        [Test]
        public void TestListResourcesStartAt()
        {
            // should allow listing resources by start date - make sure your clock is set correctly!!!

            Thread.Sleep(2000);

            DateTime start = DateTime.UtcNow;

            ImageUploadResult result = UploadTestResource("TestListResourcesStartAt");

            Thread.Sleep(2000);

            var resources = m_cloudinary.ListResources(
                new ListResourcesParams() { Type = "upload", StartAt = result.CreatedAt.AddMilliseconds(-10), Direction = "asc" });

            DeleteTestResource("TestListResourcesStartAt");

            Assert.NotNull(resources.Resources, "response should include resources");
            Assert.True(resources.Resources.Length > 0, "response should include at least one resources");
            Assert.AreEqual(result.PublicId, resources.Resources[0].PublicId);
        }

        [Test]
        public void TestCustomCoordinates()
        {
            //should allow sending custom coordinates

            var coordinates = new CloudinaryDotNet.Core.Rectangle(121, 31, 110, 151);

            var upResult = m_cloudinary.Upload(new ImageUploadParams() { File = new FileDescription(m_testImagePath), CustomCoordinates = coordinates });

            var result = m_cloudinary.GetResource(new GetResourceParams(upResult.PublicId) { Coordinates = true });

            Assert.NotNull(result.Coordinates);
            Assert.NotNull(result.Coordinates.Custom);
            Assert.AreEqual(1, result.Coordinates.Custom.Length);
            Assert.AreEqual(4, result.Coordinates.Custom[0].Length);
            Assert.AreEqual(coordinates.X, result.Coordinates.Custom[0][0]);
            Assert.AreEqual(coordinates.Y, result.Coordinates.Custom[0][1]);
            Assert.AreEqual(coordinates.Width, result.Coordinates.Custom[0][2]);
            Assert.AreEqual(coordinates.Height, result.Coordinates.Custom[0][3]);

            coordinates = new CloudinaryDotNet.Core.Rectangle(122, 32, 110, 152);

            var exResult = m_cloudinary.Explicit(new ExplicitParams(upResult.PublicId) { CustomCoordinates = coordinates, Type = "upload" });

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

            var coordinates = new CloudinaryDotNet.Core.Rectangle(121, 31, 110, 151);

            var upResult = m_cloudinary.Upload(new ImageUploadParams() { File = new FileDescription(m_testImagePath) });

            var updResult = m_cloudinary.UpdateResource(new UpdateParams(upResult.PublicId) { CustomCoordinates = coordinates });

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
            string publicId = string.Concat(m_suffix, "_TestUpdateQuality");
            var upResult = m_cloudinary.Upload(new ImageUploadParams() { File = new FileDescription(m_testImagePath), PublicId = publicId, Overwrite = true, Tags = m_test_tag });
            var updResult = m_cloudinary.UpdateResource(new UpdateParams(upResult.PublicId) { QualityOveride = "auto:best" });
            Assert.AreEqual(updResult.StatusCode, HttpStatusCode.OK);
            Assert.Null(updResult.Error);
            Assert.AreEqual(updResult.PublicId, publicId);
        }

        // For this test to work, "Auto-create folders" should be enabled in the Upload Settings, so this test is disabled by default.
        public void TestFolderApi()
        {
            // should allow to list folders and subfolders

            m_cloudinary.Upload(new ImageUploadParams() { File = new FileDescription(m_testImagePath), PublicId = "test_folder1/item" });
            m_cloudinary.Upload(new ImageUploadParams() { File = new FileDescription(m_testImagePath), PublicId = "test_folder2/item" });
            m_cloudinary.Upload(new ImageUploadParams() { File = new FileDescription(m_testImagePath), PublicId = "test_folder1/test_subfolder1/item" });
            m_cloudinary.Upload(new ImageUploadParams() { File = new FileDescription(m_testImagePath), PublicId = "test_folder1/test_subfolder2/item" });

            var result = m_cloudinary.RootFolders();
            Assert.Null(result.Error);
            Assert.AreEqual("test_folder1", result.Folders[0].Name);
            Assert.AreEqual("test_folder2", result.Folders[1].Name);

            result = m_cloudinary.SubFolders("test_folder1");

            Assert.AreEqual("test_folder1/test_subfolder1", result.Folders[0].Path);
            Assert.AreEqual("test_folder1/test_subfolder2", result.Folders[1].Path);

            result = m_cloudinary.SubFolders("test_folder");

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.NotNull(result.Error);
            Assert.NotNull(result.Error.Message);
            Assert.AreEqual("Can't find folder with path test_folder", result.Error.Message);

            m_cloudinary.DeleteResourcesByPrefix("test_folder");
        }

        [Test]
        public void TestResponsiveBreakpointsToJson()
        {
            var responsiveBreakpoint = new ResponsiveBreakpoint().ToString(Formatting.None);
            Assert.AreEqual("{\"create_derived\":true}", responsiveBreakpoint, "an empty ResponsiveBreakpoint should have create_derived=true");

            var expectedToken1 = JToken.Parse("{\"create_derived\":false,\"max_width\":500,\"min_width\":100,\"max_images\":5,\"transformation\":\"a_45\"}");
            IEnumerable<string> expectedList1 = expectedToken1.Children().Select(s => s.ToString(Formatting.None));

            Transformation transform = new Transformation().Angle(45);

            var breakpoint = new ResponsiveBreakpoint().CreateDerived(false)
                    .Transformation(transform)
                    .MaxWidth(500)
                    .MinWidth(100)
                    .MaxImages(5);

            var actualList1 = breakpoint.Children().Select(s => s.ToString(Formatting.None));
            CollectionAssert.AreEquivalent(expectedList1, actualList1);

            breakpoint.Transformation(transform.Height(210).Crop("scale"));

            var expectedToken2 = JToken.Parse("{\"create_derived\":false,\"max_width\":500,\"min_width\":100,\"max_images\":5,\"transformation\":\"a_45,c_scale,h_210\"}");
            var expectedList2 = expectedToken2.Children().Select(s => s.ToString(Formatting.None));

            var actualList2 = breakpoint.Children().Select(s => s.ToString(Formatting.None));
            CollectionAssert.AreEquivalent(expectedList2, actualList2);
        }

        [Test]
        public void TestResponsiveBreakpoints()
        {
            var breakpoint = new ResponsiveBreakpoint().MaxImages(5).BytesStep(20)
                                .MinWidth(200).MaxWidth(1000).CreateDerived(false);

            var breakpoint2 = new ResponsiveBreakpoint().Transformation(new Transformation().Width(0.9).Crop("scale").Radius(50)).MaxImages(4).BytesStep(20)
                                .MinWidth(100).MaxWidth(900).CreateDerived(false);
            // An array of breakpoints
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = "responsiveBreakpoint_id",
                Tags = "test",
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
            ExplicitParams exp = new ExplicitParams("responsiveBreakpoint_id")
            {
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(2.0) },
                Type = "upload",
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
                File = new FileDescription(m_testImagePath)
            };

            uploadParams.AddCustomParam("public_id", "test_ad_hoc_params_id");
            uploadParams.AddCustomParam("tags", "test");
            uploadParams.AddCustomParam("IgnoredEmptyParameter", "");
            uploadParams.AddCustomParam("responsive_breakpoints", JsonConvert.SerializeObject(new List<ResponsiveBreakpoint> { breakpoint }));
            uploadParams.AddCustomParam("IgnoredNullParameter", null);

            var paramsDict = uploadParams.ToParamsDictionary();
            Assert.AreEqual(3, paramsDict.Count);
            Assert.IsFalse(paramsDict.ContainsKey("IgnoredEmptyParameter"));
            Assert.IsFalse(paramsDict.ContainsKey("IgnoredNullParameter"));

            ImageUploadResult result = m_cloudinary.Upload(uploadParams);
            Assert.NotNull(result.ResponsiveBreakpoints);
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

        /// <summary>
        /// Uploads test image with params specified
        /// </summary>
        private ImageUploadResult UploadImageForTestArchive(string archiveTag, double width, bool useFileName)
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(width) },
                UseFilename = useFileName,
                Tags = archiveTag
            };
            return m_cloudinary.Upload(uploadParams);
        }

        [Test]
        public void TestCreateArchive()
        {
            string archiveTag = string.Format("{0}_archive_tag_{1}", m_test_tag, UnixTimeNow());
            string targetPublicId = string.Format("{0}_archive_id_{1}", m_suffix, UnixTimeNow());

            ImageUploadResult res = UploadImageForTestArchive(archiveTag, 2.0, true);

            ArchiveParams parameters = new ArchiveParams().Tags(new List<string> { archiveTag, "no_such_tag" }).TargetPublicId(targetPublicId);
            ArchiveResult result = m_cloudinary.CreateArchive(parameters);
            Assert.AreEqual(string.Format("{0}.zip", targetPublicId), result.PublicId);
            Assert.AreEqual(1, result.FileCount);

            ImageUploadResult res2 = UploadImageForTestArchive(archiveTag, 500, false);

            parameters = new ArchiveParams().PublicIds(new List<string> { res.PublicId, res2.PublicId })
                                            .Transformations(new List<Transformation> { new Transformation().Width("0.5"), new Transformation().Width(2) })
                                            .FlattenFolders(true)
                                            .SkipTransformationName(true)
                                            .UseOriginalFilename(true);
            result = m_cloudinary.CreateArchive(parameters);

           Assert.AreEqual(2, result.FileCount);
        }

        [Test]
        public void TestCreateArchiveRawResources()
        {
            RawUploadParams uploadParams = new RawUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Folder = "test_folder",
                Type = "private"
            };

            RawUploadResult uploadResult1 = m_cloudinary.Upload(uploadParams, "raw");

            uploadParams.File = new FileDescription(m_testPdfPath);

            RawUploadResult uploadResult2 = m_cloudinary.Upload(uploadParams, "raw");

            string targetPublicId = string.Format("archive_id_{0}", UnixTimeNow());

            ArchiveParams parameters = new ArchiveParams().PublicIds(new List<string> { uploadResult1.PublicId, uploadResult2.PublicId })
                                            .ResourceType("raw")
                                            .Type("private")
                                            .UseOriginalFilename(true);
            ArchiveResult result = m_cloudinary.CreateArchive(parameters);
            Assert.AreEqual(2, result.FileCount);
        }

        [Test]
        public void TestCreateArchiveMultiplePublicIds()
        {
            // should support archiving based on multiple public IDs
            string archiveTag = string.Format(string.Concat(m_test_tag, "_{0}"), UnixTimeNow()); 
            string targetPublicId = string.Format(string.Concat("archive_id_{0}_", m_suffix), UnixTimeNow()); 

            UploadImageForTestArchive(archiveTag, 2.0, true);

            ArchiveParams parameters = new ArchiveParams().Tags(new List<string> { archiveTag, "sadf" }).TargetPublicId(targetPublicId);
            ArchiveResult result = m_cloudinary.CreateArchive(parameters);
            Assert.AreEqual(string.Format("{0}.zip", targetPublicId), result.PublicId);
            Assert.AreEqual(1, result.FileCount);
        }

        [Test]
        public void TestDownloadArchive()
        {
            string archiveTag = string.Format(string.Concat(m_test_tag, "_{0}"), UnixTimeNow());
            string targetPublicId = string.Format(string.Concat("archive_id_{0}_", m_suffix), UnixTimeNow());

            UploadImageForTestArchive(archiveTag, 2.0, true);
            UploadImageForTestArchive(archiveTag, 500, false);

            var parameters = new ArchiveParams().Tags(new List<string> { archiveTag }).TargetPublicId(targetPublicId);
            string url = m_cloudinary.DownloadArchiveUrl(parameters);

            using (var client = new WebClient())
            {
                byte[] data = client.DownloadData(url);
                using (var s = new MemoryStream(data))
                {
                    ZipFile zip = ZipFile.Read(s);
                    var cnt = zip.Entries.Count;
                    Assert.AreEqual(2, cnt);
                }
            }
        }

        [Test]
        public void SearchResourceByTag()
        {
            string publicId = string.Format("TestForTagSearch_{0}", m_suffix);
            string tagForSearch = string.Format("TestForTagSearch_{0}", m_test_tag);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = tagForSearch,
                PublicId = publicId,
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);
            Thread.Sleep(10000);
            var resource = m_cloudinary.GetResource(new GetResourceParams(publicId) { });
            
            Assert.NotNull(resource);
            Assert.AreEqual(resource.PublicId, publicId);

            var result = m_cloudinary.Search().Expression(string.Format("tags: {0}", tagForSearch)).Execute();
            Assert.True(result.TotalCount > 0);
            Assert.AreEqual(result.Resources[0].PublicId, resource.PublicId);
        }

        [Test]
        public void SearchResourceByPublicId()
        {
            string publicId = string.Concat(m_suffix, "_TestForTagSearch");

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                
                PublicId = publicId,
                Overwrite = true,
                Type = "private"
            };
            var uploadResult = m_cloudinary.Upload(uploadParams);
            Thread.Sleep(10000);
            var result = m_cloudinary.Search().Expression(string.Format("public_id: {0}", publicId)).Execute();
            Assert.True(result.TotalCount > 0);
            DelResResult delResult = m_cloudinary.DeleteResources(new string[] { publicId });
        }

        [Test]
        public void TestSearchResourceByExpression()
        {
            string publicId = string.Concat(m_suffix, "_TestForSearch");

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_test_tag,
                PublicId = publicId, 
                Overwrite = true,
                Type = "private"
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);
            Thread.Sleep(10000);

            SearchResult result = m_cloudinary.Search().Expression("resource_type: image").Execute();
            Assert.True(result.TotalCount > 0);  
            
            result = m_cloudinary.Search().Expression(string.Format("public_id: {0}", publicId)).Execute();
            Assert.True(result.TotalCount > 0);

            DelResResult delResult = m_cloudinary.DeleteResources(new string[] { publicId });
        }

        [Test]
        public void TestClearAllTags()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = "Tag1, Tag2, Tag3",
                PublicId = "TestClearAllTags",
                Overwrite = true,
                Type = "upload",

            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(uploadResult.Tags.Length, 3);

            List<string> pIds = new List<string>();
            pIds.Add("TestClearAllTags");

            TagResult tagResult = m_cloudinary.Tag(new TagParams()
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

            DelResResult delResult = m_cloudinary.DeleteResources(new DelResParams()
            {
                PublicIds = pIds
            });

        }

        [Test]
        public void TestAddContext()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath), 
                PublicId = "TestContext",
                Overwrite = true,
                Type = "upload",

            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            List<string> pIds = new List<string>();
            pIds.Add("TestContext");

            ContextResult contextResult = m_cloudinary.Context(new ContextParams()
            {
                Command = ContextCommand.Add,
                PublicIds = pIds,
                Type = "upload",
                Context = "TestContext"

            });

            Assert.True(contextResult.PublicIds.Length > 0);

            var getResResult = m_cloudinary.GetResource(new GetResourceParams(pIds[0])
            {
                PublicId = pIds[0],
                Type = "upload",
                ResourceType = ResourceType.Image
            });


            DelResResult delResult = m_cloudinary.DeleteResources(new DelResParams()
            {
                PublicIds = pIds
            });

        }

    }
}
