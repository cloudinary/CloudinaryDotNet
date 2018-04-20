using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.Test
{
    class AsyncMethodsTest : IntegrationTestBase
    {
        private string m_transformationName;

        public override void Initialize()
        {
            base.Initialize();
            m_transformationName = $"{m_apiTest2}_ASYNC";
        }

        [Test]
        public void TestUploadLocalImageAsync()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.UploadAsync(uploadParams).Result;

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
        public void TestEnglishTextAsync()
        {
            TextParams tParams = new TextParams("Sample text.")
            {
                Background = "red",
                FontStyle = "italic",
                PublicId = GetUniquePublicId(ResourceParameterType.text)                 
            };

            TextResult textResult = m_cloudinary.TextAsync(tParams).Result;

            Assert.IsTrue(textResult.Width > 0);
            Assert.IsTrue(textResult.Height > 0);
        }

        [Test]
        public void TestDestroyRawAsync()
        {
            RawUploadParams uploadParams = new RawUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            };

            RawUploadResult uploadResult = m_cloudinary.UploadAsync(uploadParams, Api.GetCloudinaryParam(ResourceType.Raw)).Result;

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
                File = new FileDescription(largeFilePath),
                Tags = m_apiTag,
                PublicId = GetUniquePublicId()
            }, 5 * 1024 * 1024).Result;

            Assert.AreEqual(fileLength, result.Length);
        }

        [Test]
        public void TestTagAddAsync()
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            };

            ImageUploadResult uploadResult = m_cloudinary.UploadAsync(uploadParams).Result;

            TagParams tagParams = new TagParams()
            {
                Command = TagCommand.Add,
                Tag = "SomeTagAsync"
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
            var publicId = GetUniquePublicId();
            var rndString = GetUniquePublicId();

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

            List<string> pubIds = new List<string>
            {
                rndString,
                publicId
            };

            DelResParams delResParams = new DelResParams()
            {
                PublicIds = pubIds
            };

            DelResResult delResult = m_cloudinary.DeleteResourcesAsync(delResParams).Result;

            Assert.AreEqual("not_found", delResult.Deleted[rndString]);
            Assert.AreEqual("deleted", delResult.Deleted[publicId]);

            resource = m_cloudinary.GetResource(publicId);

            Assert.IsTrue(String.IsNullOrEmpty(resource.PublicId));
        }
        
        [Test]
        public void TestListTagsAsync()
        {
            // should allow listing tags
            var testTag = $"{m_apiTag}_TestListTagsAsync";

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{m_apiTag},{testTag}"
            };

            var res = m_cloudinary.UploadAsync(uploadParams).Result;

            ListTagsResult result = m_cloudinary.ListTagsAsync(new ListTagsParams()
            {
                MaxResults = MAX_RESULTS
            }).Result;

            Assert.IsTrue(result.Tags.Contains(testTag));
        }

        [Test]
        public void TestListTransformationsAsync()
        {
            // should allow listing transformations

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.UploadAsync(uploadParams).Result;

            ListTransformsResult result = m_cloudinary.ListTransformationsAsync(new ListTransformsParams()).Result;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Transformations);
            Assert.NotNull(result.Transformations.FirstOrDefault(t => t.Name == m_simpleTransformationName));
        }

        [Test]
        public void TestGetTransformAsync()
        {
            // should allow getting transformation metadata

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_resizeTransformation },
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.UploadAsync(uploadParams).Result;

            var result = m_cloudinary.GetTransformAsync(new GetTransformParams
            {
                Transformation = m_resizeTransformationName
            }).Result;

            Assert.IsNotNull(result);
        }

        [Test]
        public void TestUpdateTransformStrictAsync()
        {
            // should allow updating transformation allowed_for_strict

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_resizeTransformation },
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            var updateParams = new UpdateTransformParams()
            {
                Transformation = m_simpleTransformationName,
                Strict = true
            };

            UpdateTransformResult result = m_cloudinary.UpdateTransformAsync(updateParams).Result;

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
        public void TestCreateTransformAsync()
        {
            // should allow creating named transformation

            CreateTransformParams create = new CreateTransformParams()
            {
                Name = m_transformationName,
                Transform = m_simpleTransformation
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
            Assert.AreEqual(m_simpleTransformationName, new Transformation(getResult.Info[0]).Generate());
        }

        [Test]
        public void TestExplicitAsync()
        {
            var cloudinary = "cloudinary";
            var type = ResourceParameterType.facebook.ToString();

            ExplicitParams exp = new ExplicitParams(cloudinary)
            {
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                Type = type,
                Tags = m_apiTag
            };

            ExplicitResult expResult = m_cloudinary.ExplicitAsync(exp).Result;
            AddCreatedPublicId(ResourceParameterType.facebook, expResult.PublicId);

            string url = new Url(m_account.Cloud).ResourceType(Api.GetCloudinaryParam(ResourceType.Image)).Add(type).
                Transform(m_simpleTransformation).
                Format(FILE_FORMAT_PNG).Version(expResult.Version).BuildUrl(cloudinary);

            Assert.AreEqual(url, expResult.Eager[0].Uri.AbsoluteUri);
        }

        [Test]
        public void TestSpriteAsync()
        {
            var publicId1 = GetUniquePublicId(ResourceParameterType.sprite);
            var publicId2 = GetUniquePublicId(ResourceParameterType.sprite);
            var publicId3 = GetUniquePublicId(ResourceParameterType.sprite);

            var spriteTag = $"{GetMethodTag()}_ASYNC";

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
            uploadParams.Transformation = m_resizeTransformation;
            m_cloudinary.Upload(uploadParams);

            SpriteParams sprite = new SpriteParams(spriteTag);
            SpriteResult result = m_cloudinary.MakeSpriteAsync(sprite).Result;
            AddCreatedPublicId(ResourceParameterType.sprite, result.PublicId);

            Assert.NotNull(result);
            Assert.NotNull(result.ImageInfos);
            Assert.AreEqual(3, result.ImageInfos.Count);
            Assert.Contains(publicId1, result.ImageInfos.Keys);
            Assert.Contains(publicId2, result.ImageInfos.Keys);
            Assert.Contains(publicId3, result.ImageInfos.Keys);
        }

        [Test]
        public void TestUsageAsync()
        {
            var publicId = GetUniquePublicId();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testPdfPath),
                PublicId = publicId,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            var result = m_cloudinary.GetUsageAsync().Result;

            var plans = new List<string>() { "Free", "Advanced" };

            Assert.True(plans.Contains(result.Plan));
            Assert.True(result.Resources > 0);
            Assert.True(result.Objects.Used < result.Objects.Limit);
            Assert.True(result.Bandwidth.Used < result.Bandwidth.Limit);
        }

        [Test]
        public void TestMultiTransformationAsync()
        {
            var multiTag = $"{GetMethodTag()}_ASYNC";

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{multiTag},{m_apiTag}",
                PublicId = GetUniquePublicId(ResourceParameterType.multi)
            };
            m_cloudinary.Upload(uploadParams);

            uploadParams.PublicId = GetUniquePublicId(ResourceParameterType.multi);
            uploadParams.Transformation = m_simpleTransformation;
            m_cloudinary.Upload(uploadParams);

            MultiParams multi = new MultiParams(multiTag);
            
            MultiResult result = m_cloudinary.MultiAsync(multi).Result;
            AddCreatedPublicId(ResourceParameterType.multi, result.PublicId);
            Assert.True(result.Uri.AbsoluteUri.EndsWith($".{FILE_FORMAT_GIF}"));

            multi.Transformation = m_resizeTransformation;
            result = m_cloudinary.MultiAsync(multi).Result;
            AddCreatedPublicId(ResourceParameterType.multi, result.PublicId);
            Assert.True(result.Uri.AbsoluteUri.Contains(TRANSFORM_W_512));

            multi.Transformation = m_simpleTransformationAngle;
            multi.Format = FILE_FORMAT_PDF;
            result = m_cloudinary.MultiAsync(multi).Result;
            AddCreatedPublicId(ResourceParameterType.multi, result.PublicId);
            Assert.True(result.Uri.AbsoluteUri.Contains(TRANSFORM_A_45));
            Assert.True(result.Uri.AbsoluteUri.EndsWith($".{FILE_FORMAT_PDF}"));
        }

        [Test]
        public void TestExplodeAsync()
        {
            var publicId = GetUniquePublicId();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testPdfPath),
                PublicId = publicId, 
                Tags = m_apiTag
            };
            m_cloudinary.Upload(uploadParams);

            var result = m_cloudinary.ExplodeAsync(new ExplodeParams(publicId, m_explodeTransformation)).Result;

            Assert.AreEqual("processing", result.Status);
        }

        [Test]
        public void TestUpdateCustomCoordinatesAsync()
        {
            //should update custom coordinates
            var coordinates = new Core.Rectangle(121, 31, 110, 151);
            var upResult = m_cloudinary.UploadAsync(new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = m_apiTag
            }).Result;

            var updResult = m_cloudinary.UpdateResourceAsync(new UpdateParams(upResult.PublicId)
            {
                CustomCoordinates = coordinates
            }).Result;

            var result = m_cloudinary.GetResourceAsync(new GetResourceParams(upResult.PublicId)
            {
                Coordinates = true
            }).Result;

            Assert.NotNull(result.Coordinates);
            Assert.NotNull(result.Coordinates.Custom);
            Assert.AreEqual(1, result.Coordinates.Custom.Length);
            Assert.AreEqual(4, result.Coordinates.Custom[0].Length);
            Assert.AreEqual(coordinates.X, result.Coordinates.Custom[0][0]);
            Assert.AreEqual(coordinates.Y, result.Coordinates.Custom[0][1]);
            Assert.AreEqual(coordinates.Width, result.Coordinates.Custom[0][2]);
            Assert.AreEqual(coordinates.Height, result.Coordinates.Custom[0][3]);
        }

        protected override string GetUniquePublicId(ResourceParameterType actionType, string suffix = "")
        {
            return base.GetUniquePublicId(actionType, "_async");
        }

        [OneTimeTearDown]
        public override void Cleanup()
        {
            base.Cleanup();
            m_cloudinary.DeleteTransform(m_transformationName);
        }
    }
}
