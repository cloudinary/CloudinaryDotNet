using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet.Test;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Cloudinary.Test
{
    class AsyncMethodsTest : IntegrationTestBase
    {
        [Test]
        public void TestUploadLocalImageAsync()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath)
            };

            var uploadResult = m_cloudinary.UploadAsync(uploadParams).Result;

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
        public void TestEnglishTextAsync()
        {
            TextParams tParams = new TextParams("Sample text.");
            tParams.Background = "red";
            tParams.FontStyle = "italic";
            TextResult textResult = m_cloudinary.TextAsync(tParams).Result;

            Assert.IsTrue(textResult.Width > 0);
            Assert.IsTrue(textResult.Height > 0);
        }

        [Test]
        public void TestDestroyRawAsync()
        {
            RawUploadParams uploadParams = new RawUploadParams()
            {
                File = new FileDescription(m_testImagePath)
            };

            RawUploadResult uploadResult = m_cloudinary.UploadAsync(uploadParams, "raw").Result;

            Assert.NotNull(uploadResult);

            DeletionParams destroyParams = new DeletionParams(uploadResult.PublicId)
            {
                ResourceType = ResourceType.Raw
            };

            DeletionResult destroyResult = m_cloudinary.DestroyAsync(destroyParams).Result;

            Assert.AreEqual("ok", destroyResult.Result);
        }

        [Test]
        public void TestUploadLargeRawFilesAsync()
        {
            // support uploading large raw files

            var largeFilePath = m_testLargeImagePath;
            int fileLength = (int)new FileInfo(largeFilePath).Length;
            var result = m_cloudinary.UploadLargeRawAsync(new RawUploadParams()
            {
                File = new FileDescription(largeFilePath)
            }, 5 * 1024 * 1024).Result;

            Assert.AreEqual(fileLength, result.Length);
        }

        [Test]
        public void TestTagAddAsync()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath)
            };

            ImageUploadResult uploadResult = m_cloudinary.UploadAsync(uploadParams).Result;

            TagParams tagParams = new TagParams()
            {
                Command = TagCommand.Add,
                Tag = "test-------tag"
            };

            tagParams.PublicIds.Add(uploadResult.PublicId);

            TagResult tagResult = m_cloudinary.TagAsync(tagParams).Result;

            Assert.AreEqual(1, tagResult.PublicIds.Length);
            Assert.AreEqual(uploadResult.PublicId, tagResult.PublicIds[0]);
        }

       [Test]
        public void TestDeleteAsync()
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

            List<string> pubIds = new List<string>();
            pubIds.Add("randomstringopa");
            pubIds.Add("testdeletederived");
            pubIds.Add("testdelete");

            DelResParams delResParams = new DelResParams()
            {
                PublicIds = pubIds
            };

            DelResResult delResult = m_cloudinary.DeleteResourcesAsync(delResParams).Result;

            Assert.AreEqual("not_found", delResult.Deleted["randomstringopa"]);
            Assert.AreEqual("deleted", delResult.Deleted["testdelete"]);

            resource = m_cloudinary.GetResource("testdelete");

            Assert.IsTrue(String.IsNullOrEmpty(resource.PublicId));
        }

        [Test]
        public void TestListTagsAsync()
        {
            // should allow listing tags

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = "api_test_custom"
            };

            m_cloudinary.Upload(uploadParams);

            ListTagsResult result = m_cloudinary.ListTagsAsync(new ListTagsParams()).Result;

            Assert.IsTrue(result.Tags.Contains("api_test_custom"));
        }

        [Test]
        public void TestListTransformationsAsync()
        {
            // should allow listing transformations

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(100) },
                Tags = "transformation"
            };

            m_cloudinary.UploadAsync(uploadParams);

            ListTransformsResult result = m_cloudinary.ListTransformationsAsync(new ListTransformsParams()).Result;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Transformations);
            TransformDesc td = result.Transformations.Where(t => t.Name == "c_scale,w_100").First();
            Assert.IsTrue(td.Used);
        }

        [Test]
        public void TestGetTransformAsync()
        {
            // should allow getting transformation metadata

            var t = new Transformation().Crop("scale").Dpr(1.3).Width(2.0);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { t },
                Tags = "transformation"
            };

            var uploadResult = m_cloudinary.UploadAsync(uploadParams).Result;

            var result = m_cloudinary.GetTransformAsync(new GetTransformParams { Transformation = "c_scale, dpr_1.3, w_2.0" }).Result;

            Assert.IsNotNull(result);
        }

        [Test]
        public void TestUpdateTransformStrictAsync()
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

            UpdateTransformResult result = m_cloudinary.UpdateTransformAsync(updateParams).Result;

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
        public void TestCreateTransformAsync()
        {
            // should allow creating named transformation

            Transformation t = new Transformation().Crop("scale").Width(102);

            CreateTransformParams create = new CreateTransformParams()
            {
                Name = "api_test_transformation",
                Transform = t
            };

            var result = m_cloudinary.CreateTransformAsync(create).Result;

            Assert.IsNotNull(result);

            GetTransformParams get = new GetTransformParams()
            {
                Transformation = create.Name
            };

            GetTransformResult getResult = m_cloudinary.GetTransformAsync(get).Result;

            Assert.IsNotNull(getResult);
            Assert.AreEqual(true, getResult.Strict);
            Assert.AreEqual(false, getResult.Used);
            Assert.AreEqual(1, getResult.Info.Length);
            Assert.AreEqual(t.Generate(), new Transformation(getResult.Info[0]).Generate());
        }

        [Test]
        public void TestExplicitAsync()
        {
            ExplicitParams exp = new ExplicitParams("cloudinary")
            {
                EagerTransforms = new List<Transformation>() { new Transformation().Crop("scale").Width(2.0) },
                Type = "facebook"
            };

            ExplicitResult expResult = m_cloudinary.ExplicitAsync(exp).Result;

            string url = new Url(m_account.Cloud).ResourceType("image").Add("facebook").
                Transform(new Transformation().Crop("scale").Width(2.0)).
                Format("png").Version(expResult.Version).BuildUrl("cloudinary");

            Assert.AreEqual(url, expResult.Eager[0].Uri.AbsoluteUri);
        }

        [Test]
        public void TestSpriteAsync()
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

            SpriteResult result = m_cloudinary.MakeSpriteAsync(sprite).Result;

            Assert.NotNull(result);
            Assert.NotNull(result.ImageInfos);
            Assert.AreEqual(3, result.ImageInfos.Count);
            Assert.Contains("logo1", result.ImageInfos.Keys);
            Assert.Contains("logo2", result.ImageInfos.Keys);
            Assert.Contains("logo3", result.ImageInfos.Keys);
        }

        [Test]
        public void TestUsageAsync()
        {
            UploadTestResource("TestUsage"); // making sure at least one resource exists
            var result = m_cloudinary.GetUsageAsync().Result;
            DeleteTestResource("TestUsage");

            var plans = new List<string>() { "Free", "Advanced" };

            Assert.True(plans.Contains(result.Plan));
            Assert.True(result.Resources > 0);
            Assert.True(result.Objects.Used < result.Objects.Limit);
            Assert.True(result.Bandwidth.Used < result.Bandwidth.Limit);
        }

        [Test]
        public void TestMultiTransformationAsync()
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
            MultiResult result = m_cloudinary.MultiAsync(multi).Result;
            Assert.True(result.Uri.AbsoluteUri.EndsWith(".gif"));

            multi.Transformation = new Transformation().Width(100);
            result = m_cloudinary.MultiAsync(multi).Result;
            Assert.True(result.Uri.AbsoluteUri.Contains("w_100"));

            multi.Transformation = new Transformation().Width(111);
            multi.Format = "pdf";
            result = m_cloudinary.MultiAsync(multi).Result;
            Assert.True(result.Uri.AbsoluteUri.Contains("w_111"));
            Assert.True(result.Uri.AbsoluteUri.EndsWith(".pdf"));
        }

        [Test]
        public void TestExplodeAsync()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testPdfPath),
                PublicId = "testexplode"
            };

            m_cloudinary.Upload(uploadParams);

            var result = m_cloudinary.ExplodeAsync(new ExplodeParams(
                "testexplode",
                new Transformation().Page("all"))).Result;

            Assert.AreEqual("processing", result.Status);
        }

        [Test]
        public void TestUpdateCustomCoordinatesAsync()
        {
            //should update custom coordinates

            var coordinates = new CloudinaryDotNet.Core.Rectangle(121, 31, 110, 151);

            var upResult = m_cloudinary.UploadAsync(new ImageUploadParams() { File = new FileDescription(m_testImagePath) }).Result;

            var updResult = m_cloudinary.UpdateResourceAsync(new UpdateParams(upResult.PublicId) { CustomCoordinates = coordinates }).Result;

            var result = m_cloudinary.GetResourceAsync(new GetResourceParams(upResult.PublicId) { Coordinates = true }).Result;

            Assert.NotNull(result.Coordinates);
            Assert.NotNull(result.Coordinates.Custom);
            Assert.AreEqual(1, result.Coordinates.Custom.Length);
            Assert.AreEqual(4, result.Coordinates.Custom[0].Length);
            Assert.AreEqual(coordinates.X, result.Coordinates.Custom[0][0]);
            Assert.AreEqual(coordinates.Y, result.Coordinates.Custom[0][1]);
            Assert.AreEqual(coordinates.Width, result.Coordinates.Custom[0][2]);
            Assert.AreEqual(coordinates.Height, result.Coordinates.Custom[0][3]);
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
    }
}
