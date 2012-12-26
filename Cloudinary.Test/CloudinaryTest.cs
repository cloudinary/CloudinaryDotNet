using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using CloudinaryDotNet.Actions;
using NUnit.Framework;
using Cloudinary.Test.Properties;

namespace CloudinaryDotNet.Test
{
    [TestFixture]
    public class CloudinaryTest
    {
        Account m_account;
        Cloudinary m_cloudinary;
        string m_testImagePath;

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

            m_cloudinary = new Cloudinary(m_account, true);

            m_testImagePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestImage.jpg");

            Resources.TestImage.Save(m_testImagePath);
        }

        [Test]
        public void TestUploadLocalImage()
        {
            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg"))
            })
            {
                ImageUploadResult uploadResult = m_cloudinary.Upload(uploadParams);

                Assert.AreEqual(759100, uploadResult.Length);
                Assert.AreEqual(1920, uploadResult.Width);
                Assert.AreEqual(1200, uploadResult.Height);
                Assert.AreEqual("jpg", uploadResult.Format);

                SortedDictionary<string, object> checkParams = new SortedDictionary<string, object>();
                checkParams.Add("public_id", uploadResult.PublicId);
                checkParams.Add("version", uploadResult.Version);

                Api api = new Api(m_account, false);
                string expectedSign = api.GetSign(checkParams);

                Assert.AreEqual(expectedSign, uploadResult.Signature);
            }
        }

        [Test]
        public void TestUploadTransformationResize()
        {
            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                Transformation = new Transformation().Width(512).Height(512),
                Tags = "transformation"
            })
            {

                ImageUploadResult uploadResult = m_cloudinary.Upload(uploadParams);

                Assert.AreEqual(191141, uploadResult.Length);
                Assert.AreEqual(512, uploadResult.Width);
                Assert.AreEqual(512, uploadResult.Height);
                Assert.AreEqual("jpg", uploadResult.Format);
            }
        }

        [Test]
        public void TestEnglishText()
        {
            TextParams textParams = new TextParams()
            {
                Text = "Sample text."
            };

            TextResult textResult = m_cloudinary.Text(textParams);

            Assert.AreEqual(67, textResult.Width);
            Assert.AreEqual(10, textResult.Height);
        }

        [Test]
        public void TestRussianText()
        {
            TextParams textParams = new TextParams()
            {
                Text = "Пример текста.",
            };

            TextResult textResult = m_cloudinary.Text(textParams);

            Assert.AreEqual(88, textResult.Width);
            Assert.AreEqual(10, textResult.Height);
        }

        [Test]
        public void TestDestroyRaw()
        {
            RawUploadResult uploadResult;

            using (RawUploadParams uploadParams = new RawUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg"))
            })
            {
                uploadResult = m_cloudinary.Upload(uploadParams);
            }

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
            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription("http://cloudinary.com/images/logo.png"),
                Tags = "remote"
            })
            {
                ImageUploadResult uploadResult = m_cloudinary.Upload(uploadParams);

                Assert.AreEqual(3381, uploadResult.Length);
                Assert.AreEqual(241, uploadResult.Width);
                Assert.AreEqual(51, uploadResult.Height);
                Assert.AreEqual("png", uploadResult.Format);
            }
        }

        [Test]
        public void TestUploadStream()
        {
            byte[] bytes = File.ReadAllBytes(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg"));

            MemoryStream memoryStream = new MemoryStream(bytes);

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription("streamed", memoryStream),
                Tags = "streamed"
            })
            {
                ImageUploadResult uploadResult = m_cloudinary.Upload(uploadParams);

                Assert.AreEqual(759100, uploadResult.Length);
                Assert.AreEqual(1920, uploadResult.Width);
                Assert.AreEqual(1200, uploadResult.Height);
                Assert.AreEqual("jpg", uploadResult.Format);
            }

            Assert.AreEqual(false, memoryStream.CanRead);
            Assert.AreEqual(false, memoryStream.CanRead);
        }

        [Test]
        public void TestTagAdd()
        {
            ImageUploadResult uploadResult;

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg"))
            })
            {
                uploadResult = m_cloudinary.Upload(uploadParams);
            }

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
            ImageUploadResult uploadResult;

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                Tags = "test++++++tag"
            })
            {
                uploadResult = m_cloudinary.Upload(uploadParams);
            }

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

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                PublicId = "testlistresources"
            })
            {
                m_cloudinary.Upload(uploadParams);

                ListResourcesResult result = m_cloudinary.ListResources();

                Assert.IsTrue(result.Resources.Where(res => res.PublicId == uploadParams.PublicId && res.Type == "upload").Count() > 0);
            }
        }

        [Test]
        public void TestListResourcesByType()
        {
            // should allow listing resources by type

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                PublicId = "testlistresourcesbytype"
            })
            {
                m_cloudinary.Upload(uploadParams);

                ListResourcesResult result = m_cloudinary.ListResourcesByType("upload", null);

                Assert.IsTrue(result.Resources.Where(res => res.PublicId == uploadParams.PublicId && res.Type == "upload").Count() > 0);
            }
        }

        [Test]
        public void TestListResourcesByPrefix()
        {
            // should allow listing resources by prefix

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                PublicId = "testlistblablabla"
            })
            {
                m_cloudinary.Upload(uploadParams);

                ListResourcesResult result = m_cloudinary.ListResourcesByPrefix("upload", "testlist", null);

                Assert.IsTrue(result.Resources.Where(res => res.PublicId.StartsWith("testlist")).Count() == result.Resources.Count());
            }
        }

        [Test]
        public void TestListResourcesByTag()
        {
            // should allow listing resources by tag

            FileDescription file = new FileDescription(Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "TestImage.jpg"));
            try
            {
                ImageUploadParams uploadParams = new ImageUploadParams()
                {
                    File = file,
                    Tags = "teslistresourcesbytag1,beauty"
                };

                m_cloudinary.Upload(uploadParams);
            }
            finally
            {
                file.Dispose();
            }

            file = new FileDescription(Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "TestImage.jpg"));

            try
            {
                ImageUploadParams uploadParams = new ImageUploadParams()
                {
                    File = file,
                    Tags = "teslistresourcesbytag1"
                };

                m_cloudinary.Upload(uploadParams);
            }
            finally
            {
                file.Dispose();
            }

            ListResourcesResult result = m_cloudinary.ListResourcesByTag("teslistresourcesbytag1", null);

            Assert.AreEqual(2, result.Resources.Count());
        }

        [Test]
        public void TestResourcesCursor()
        {
            // should allow listing resources with cursor

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                PublicId = "testlistresources1"
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                PublicId = "testlistresources2"
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

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
            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                Eager = new EagerTransformation(new Transformation().Crop("scale").Width(2.0)),
                Tags = "eager,transformation"
            })
            {
                m_cloudinary.Upload(uploadParams);
            }
        }

        [Test]
        public void TestGetResource()
        {
            // should allow get resource metadata

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                Eager = new EagerTransformation(new Transformation().Crop("scale").Width(2.0)),
                PublicId = "testgetresource"
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

            GetResourceResult getResult = m_cloudinary.GetResource("testgetresource");

            Assert.IsNotNull(getResult);
            Assert.AreEqual("testgetresource", getResult.PublicId);
            Assert.AreEqual(759100, getResult.Length);
            Assert.AreEqual(1920, getResult.Width);
            Assert.AreEqual(1200, getResult.Height);
            Assert.AreEqual("jpg", getResult.Format);
            Assert.AreEqual(1, getResult.Derived.Length);
        }

        [Test]
        public void TestDeleteDerived()
        {
            // should allow deleting derived resource

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                Eager = new EagerTransformation(new Transformation().Width(101).Crop("scale")),
                PublicId = "testdeletederived"
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

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

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                PublicId = "testdelete"
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

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

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                PublicId = "testdelete"
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

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

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                PublicId = "api_test4",
                Tags = "api_test_tag_for_delete"
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

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

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                Tags = "api_test_custom"
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

            ListTagsResult result = m_cloudinary.ListTags();

            Assert.IsTrue(result.Tags.Contains("api_test_custom"));
        }

        [Test]
        public void DeleteAllInLoop()
        {
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

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                Tags = "api_test_custom1"
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                Tags = "api_test_brustom"
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

            ListTagsResult result = m_cloudinary.ListTagsByPrefix("api_test");

            Assert.IsTrue(result.Tags.Contains("api_test_brustom"));

            result = m_cloudinary.ListTagsByPrefix("nononothereisnosuchtag");

            Assert.IsTrue(result.Tags.Length == 0);
        }

        [Test]
        public void TestListTransformations()
        {
            // should allow listing transformations

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                Eager = new EagerTransformation(new Transformation().Crop("scale").Width(100)),
                Tags = "transformation"
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

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

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                Eager = new EagerTransformation(t),
                Tags = "transformation"
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

            GetTransformResult result = m_cloudinary.GetTransform("c_scale,w_2");

            Assert.IsNotNull(result);
            Assert.AreEqual(t.Generate(), new Transformation(result.Info[0]).Generate());
        }

        [Test]
        public void TestUpdateTransform()
        {
            // should allow updating transformation allowed_for_strict

            Transformation t = new Transformation().Crop("scale").Width(100);

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                Eager = new EagerTransformation(t),
                Tags = "transformation"
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

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

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                Eager = new EagerTransformation(new Transformation().Crop("scale").Width(100))
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

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
            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                PublicId = "headers"
            })
            {
                uploadParams.Headers = new Dictionary<string, string>();
                uploadParams.Headers.Add("Link", "1");
                uploadParams.Headers.Add("Blink", "182");

                m_cloudinary.Upload(uploadParams);
            }
        }

        [Test]
        public void TestExplicit()
        {
            ExplicitParams exp = new ExplicitParams("cloudinary")
            {
                Eager = new EagerTransformation(new Transformation().Crop("scale").Width(2.0)),
                Type = "twitter_name"
            };

            ExplicitResult expResult = m_cloudinary.Explicit(exp);

            string url = new Url(m_account.Cloud, false).ResourceType("image").Add("twitter_name").
                Transform(new Transformation().Crop("scale").Width(2.0)).
                Format("png").Version(expResult.Version).BuildUrl("cloudinary");

            Assert.AreEqual(url, expResult.Eager[0].Uri.AbsoluteUri);
        }

        [Test]
        public void TestSprite()
        {
            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                Tags = "logo,beauty",
                PublicId = "logo1",
                Transformation = new Transformation().Width(200).Height(100)
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                Tags = "logo",
                PublicId = "logo2",
                Transformation = new Transformation().Width(100).Height(100)
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                Tags = "logo",
                PublicId = "logo3",
                Transformation = new Transformation().Width(100).Height(300)
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

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
            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                Tags = "logotrans",
                PublicId = "logotrans1",
                Transformation = new Transformation().Width(200).Height(100)
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                Tags = "logotrans",
                PublicId = "logotrans2",
                Transformation = new Transformation().Width(100).Height(100)
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

            using (ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImage.jpg")),
                Tags = "logotrans",
                PublicId = "logotrans3",
                Transformation = new Transformation().Width(100).Height(300)
            })
            {
                m_cloudinary.Upload(uploadParams);
            }

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
    }
}
