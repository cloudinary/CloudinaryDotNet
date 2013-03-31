using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Cloudinary.Test.Properties;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.Test
{
    [TestFixture]
    public class CloudinaryTest
    {
        Account m_account;
        Cloudinary m_cloudinary;
        string m_testImagePath;
        string m_testPdfPath;

        [SetUp]
        public void Initialize()
        {
            m_account = new Account(
                Settings.Default.CloudName,
                Settings.Default.ApiKey,
                Settings.Default.ApiSecret);

            if (String.IsNullOrEmpty(m_account.Cloud))
                Console.WriteLine("Cloud name must be specified in test configuration (app.config)!");

            if (String.IsNullOrEmpty(m_account.ApiKey))
                Console.WriteLine("Cloudinary API key must be specified in test configuration (app.config)!");

            if (String.IsNullOrEmpty(m_account.ApiSecret))
                Console.WriteLine("Cloudinary API secret must be specified in test configuration (app.config)!");

            Assert.IsFalse(String.IsNullOrEmpty(m_account.Cloud));
            Assert.IsFalse(String.IsNullOrEmpty(m_account.ApiKey));
            Assert.IsFalse(String.IsNullOrEmpty(m_account.ApiSecret));

            m_cloudinary = new Cloudinary(m_account);

            m_testImagePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestImage.jpg");
            m_testPdfPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "multipage.pdf");

            Resources.TestImage.Save(m_testImagePath);
            File.WriteAllBytes(m_testPdfPath, Resources.multipage);
        }

        [Test]
        public void TestUploadLocalImage()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath)
            };

            ImageUploadResult uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(737633, uploadResult.Length);
            Assert.AreEqual(1920, uploadResult.Width);
            Assert.AreEqual(1200, uploadResult.Height);
            Assert.AreEqual("jpg", uploadResult.Format);

            SortedDictionary<string, object> checkParams = new SortedDictionary<string, object>();
            checkParams.Add("public_id", uploadResult.PublicId);
            checkParams.Add("version", uploadResult.Version);

            Api api = new Api(m_account);
            string expectedSign = api.GetSign(checkParams);

            Assert.AreEqual(expectedSign, uploadResult.Signature);
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
        public void TestUploadTransformationResize()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Transformation = new Transformation().Width(512).Height(512),
                Tags = "transformation"
            };

            ImageUploadResult uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(169674, uploadResult.Length);
            Assert.AreEqual(512, uploadResult.Width);
            Assert.AreEqual(512, uploadResult.Height);
            Assert.AreEqual("jpg", uploadResult.Format);
        }

        [Test]
        public void TestEnglishText()
        {
            TextResult textResult = m_cloudinary.Text("Sample text.");

            Assert.AreEqual(67, textResult.Width);
            Assert.AreEqual(10, textResult.Height);
        }

        [Test]
        public void TestRussianText()
        {
            TextResult textResult = m_cloudinary.Text("Пример текста.");

            Assert.AreEqual(88, textResult.Width);
            Assert.AreEqual(10, textResult.Height);
        }

        [Test]
        public void TestDestroyRaw()
        {
            RawUploadParams uploadParams = new RawUploadParams()
            {
                File = new FileDescription(m_testImagePath)
            };

            RawUploadResult uploadResult = m_cloudinary.Upload(uploadParams);

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
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription("http://cloudinary.com/images/logo.png"),
                Tags = "remote"
            };

            ImageUploadResult uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(3381, uploadResult.Length);
            Assert.AreEqual(241, uploadResult.Width);
            Assert.AreEqual(51, uploadResult.Height);
            Assert.AreEqual("png", uploadResult.Format);
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

                Assert.AreEqual(737633, uploadResult.Length);
                Assert.AreEqual(1920, uploadResult.Width);
                Assert.AreEqual(1200, uploadResult.Height);
                Assert.AreEqual("jpg", uploadResult.Format);
            }
        }

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

        [Test]
        public void TestListResources()
        {
            // should allow listing resources

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = "testlistresources"
            };

            m_cloudinary.Upload(uploadParams);

            ListResourcesResult result = m_cloudinary.ListResources();

            Assert.IsTrue(result.Resources.Where(res => res.PublicId == uploadParams.PublicId && res.Type == "upload").Count() > 0);
        }

        [Test]
        public void TestListResourcesByType()
        {
            // should allow listing resources by type

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = "testlistresourcesbytype"
            };

            m_cloudinary.Upload(uploadParams);

            ListResourcesResult result = m_cloudinary.ListResourcesByType("upload", null);

            Assert.IsTrue(result.Resources.Where(res => res.PublicId == uploadParams.PublicId && res.Type == "upload").Count() > 0);
        }

        [Test]
        public void TestListResourcesByPrefix()
        {
            // should allow listing resources by prefix

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = "testlistblablabla"
            };

            m_cloudinary.Upload(uploadParams);

            ListResourcesResult result = m_cloudinary.ListResourcesByPrefix("upload", "testlist", null);

            Assert.IsTrue(result.Resources.Where(res => res.PublicId.StartsWith("testlist")).Count() == result.Resources.Count());
        }

        [Test]
        public void TestListResourcesByTag()
        {
            // should allow listing resources by tag

            FileDescription file = new FileDescription(m_testImagePath);

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = file,
                Tags = "teslistresourcesbytag1,beauty"
            };

            m_cloudinary.Upload(uploadParams);

            uploadParams = new ImageUploadParams()
            {
                File = file,
                Tags = "teslistresourcesbytag1"
            };

            m_cloudinary.Upload(uploadParams);

            ListResourcesResult result = m_cloudinary.ListResourcesByTag("teslistresourcesbytag1", null);

            Assert.AreEqual(2, result.Resources.Count());
        }

        [Test]
        public void TestResourcesCursor()
        {
            // should allow listing resources with cursor

            ImageUploadParams uploadParams = new ImageUploadParams()
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

            ListResourcesParams listParams = new ListResourcesParams()
            {
                ResourceType = ResourceType.Image,
                MaxResults = 1
            };

            ListResourcesResult result1 = m_cloudinary.ListResources(listParams);

            Assert.IsNotNull(result1.Resources);
            Assert.AreEqual(1, result1.Resources.Length);
            Assert.IsFalse(String.IsNullOrEmpty(result1.NextCursor));

            listParams.NextCursor = result1.NextCursor;

            ListResourcesResult result2 = m_cloudinary.ListResources(listParams);

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
        public void TestGetResource()
        {
            // should allow get resource metadata

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(2.0) },
                PublicId = "testgetresource"
            };

            m_cloudinary.Upload(uploadParams);

            GetResourceResult getResult = m_cloudinary.GetResource("testgetresource");

            Assert.IsNotNull(getResult);
            Assert.AreEqual("testgetresource", getResult.PublicId);
            Assert.AreEqual(737633, getResult.Length);
            Assert.AreEqual(1920, getResult.Width);
            Assert.AreEqual(1200, getResult.Height);
            Assert.AreEqual("jpg", getResult.Format);
            Assert.AreEqual(1, getResult.Derived.Length);
            Assert.Null(getResult.Metadata);
        }

        [Test]
        public void TestGetResourceWithMetadata()
        {
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
                PublicId = "testdelete"
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

        // Test disabled because it deletes all images in the remote account.
        public void DeleteAllInLoop()
        {
            return;
            string nextCursor = String.Empty;

            while (true)
            {
                ListResourcesResult existingResources = String.IsNullOrEmpty(nextCursor) ?
                    m_cloudinary.ListResources() :
                    m_cloudinary.ListResources(nextCursor);

                nextCursor = existingResources.NextCursor;

                DelResParams deleteParams = new DelResParams();

                bool resourcesLeft = false;
                foreach (var res in existingResources.Resources)
                {
                    if (res.Type != "sprite")
                    {
                        deleteParams.Type = res.Type;
                        resourcesLeft = true;
                        break;
                    }
                }

                if (!resourcesLeft) break;

                foreach (var resource in existingResources.Resources)
                {
                    if (resource.Type == deleteParams.Type)
                        deleteParams.PublicIds.Add(resource.PublicId);
                }

                Console.WriteLine("Deleting {0} resources of type {1}...", deleteParams.PublicIds.Count, deleteParams.Type);

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

            Transformation t = new Transformation().Crop("scale").Width(2.0);

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { t },
                Tags = "transformation"
            };

            m_cloudinary.Upload(uploadParams);

            GetTransformResult result = m_cloudinary.GetTransform("c_scale,w_2");

            Assert.IsNotNull(result);
            Assert.AreEqual(t.Generate(), new Transformation(result.Info[0]).Generate());
        }

        [Test]
        public void TestUpdateTransform()
        {
            // should allow updating transformation allowed_for_strict

            Transformation t = new Transformation().Crop("scale").Width(100);

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { t },
                Tags = "transformation"
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

        [Test]
        public void TestExplicit()
        {
            ExplicitParams exp = new ExplicitParams("cloudinary")
            {
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(2.0) },
                Type = "twitter_name"
            };

            ExplicitResult expResult = m_cloudinary.Explicit(exp);

            string url = new Url(m_account.Cloud).ResourceType("image").Add("twitter_name").
                Transform(new Transformation().Crop("scale").Width(2.0)).
                Format("png").Version(expResult.Version).BuildUrl("cloudinary");

            Assert.AreEqual(url, expResult.Eager[0].Uri.AbsoluteUri);
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
                 Type = "twitter_name"
             };

            var result = m_cloudinary.Explicit(exp);

            Assert.NotNull(result.JsonObj);
            Assert.AreEqual(result.PublicId, result.JsonObj["public_id"].ToString());
        }

        [Test]
        public void TestUsage()
        {
            var result = m_cloudinary.GetUsage();

            Assert.NotNull(result);
            Assert.AreEqual("Free", result.Plan);
            Assert.True(result.Resources > 0);
            Assert.True(result.Objects.Used < result.Objects.Limit);
            Assert.True(result.Bandwidth.Used < result.Bandwidth.Limit);
            Assert.True(result.Storage.Used < result.Storage.Limit);
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

            Assert.NotNull(result);
            Assert.AreEqual("processing", result.Status);
        }
    }
}
