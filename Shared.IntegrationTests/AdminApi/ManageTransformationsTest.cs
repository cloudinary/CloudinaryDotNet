using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.AdminApi
{
    public class ManageTransformationsTest : IntegrationTestBase
    {
        private Transformation m_implicitTransformation;

        public override void Initialize()
        {
            base.Initialize();

            var implicitTransformationText = m_suffix + "_implicit";
            m_implicitTransformation = new Transformation().Crop("scale").Overlay(new TextLayer().Text(implicitTransformationText).FontFamily("Arial").FontSize(60));
        }

        [Test, RetryWithDelay]
        public void TestListTransformations()
        {
            // should allow listing transformations

            UploadTestImageResource((uploadParams) =>
            {
                uploadParams.EagerTransforms = new List<Transformation>() { m_simpleTransformation };
            });

            var result = m_cloudinary.ListTransformations();

            AssertNotEmptyListAndContainsTransformation(result, m_simpleTransformationAsString);
        }

        [Test, RetryWithDelay]
        public async Task TestListTransformationsAsync()
        {
            // should allow listing transformations

            await UploadTestImageResourceAsync((uploadParams) =>
            {
                uploadParams.EagerTransforms = new List<Transformation>() { m_simpleTransformation };
            });

            var result = await m_cloudinary.ListTransformationsAsync();

            AssertNotEmptyListAndContainsTransformation(result, m_simpleTransformationAsString);
        }

        private void AssertNotEmptyListAndContainsTransformation(ListTransformsResult result, string transformation)
        {
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Transformations);

            var td = result.Transformations
                .Where(t => t.Name == transformation)
                .First();

            Assert.IsFalse(td.Named);
            Assert.IsTrue(td.Used);
        }

        [Test, RetryWithDelay]
        public void TestGetTransform()
        {
            // should allow getting transformation metadata

            UploadTestImageResource((uploadParams) =>
            {
                uploadParams.EagerTransforms = new List<Transformation>() { m_updateTransformation };
            });

            var result = m_cloudinary.GetTransform(m_updateTransformationAsString);

            AssertGetTransform(result, m_updateTransformation);
        }

        [Test, RetryWithDelay]
        public async Task TestGetTransformAsync()
        {
            // should allow getting transformation metadata

            await UploadTestImageResourceAsync((uploadParams) =>
            {
                uploadParams.EagerTransforms = new List<Transformation>() { m_updateTransformation };
            });

            var result = await m_cloudinary.GetTransformAsync(m_updateTransformationAsString);

            AssertGetTransform(result, m_updateTransformation);
        }

        private void AssertGetTransform(GetTransformResult result, Transformation transformation)
        {
            Assert.IsNotNull(result);
            Assert.AreEqual(transformation.ToString(), new Transformation(result.Info[0]).ToString());
        }

        [Test, RetryWithDelay]
        public void TestGetTransformParamsCheck()
        {
            Assert.Throws<ArgumentException>(new GetTransformParams().Check, "Should require Transformation");
        }

        [Test, RetryWithDelay]
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

        [Test, RetryWithDelay]
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

        [Test, RetryWithDelay]
        public void TestUpdateTransformStrict()
        {
            // should allow updating transformation allowed_for_strict

            UploadTestImageResource((uploadParams) =>
            {
                uploadParams.EagerTransforms = new List<Transformation>() { m_simpleTransformation };
            });

            var updateParams = GetUpdateTransformParamsStrict(m_simpleTransformationAsString);

            m_cloudinary.UpdateTransform(updateParams);

            var getResult = m_cloudinary.GetTransform(m_simpleTransformationAsString);

            AssertGetTransformResultIsStrict(getResult, m_simpleTransformationAsString, true);

            updateParams.AllowedForStrict = false;
            m_cloudinary.UpdateTransform(updateParams);

            getResult = m_cloudinary.GetTransform(m_simpleTransformationAsString);

            AssertGetTransformResultIsStrict(getResult, m_simpleTransformationAsString, false);
        }

        [Test, RetryWithDelay]
        public async Task TestUpdateTransformStrictAsync()
        {
            // should allow updating transformation allowed_for_strict

            await UploadTestImageResourceAsync((uploadParams) =>
            {
                uploadParams.EagerTransforms = new List<Transformation>() { m_simpleTransformation };
            });

            var updateParams = GetUpdateTransformParamsStrict(m_simpleTransformationAsString);

            await m_cloudinary.UpdateTransformAsync(updateParams);

            var getResult = await m_cloudinary.GetTransformAsync(m_simpleTransformationAsString);

            AssertGetTransformResultIsStrict(getResult, m_simpleTransformationAsString, true);

            updateParams.AllowedForStrict = false;
            await m_cloudinary.UpdateTransformAsync(updateParams);

            getResult = await m_cloudinary.GetTransformAsync(m_simpleTransformationAsString);

            AssertGetTransformResultIsStrict(getResult, m_simpleTransformationAsString, false);
        }

        private UpdateTransformParams GetUpdateTransformParamsStrict(string transformation)
        {
            return new UpdateTransformParams()
            {
                Transformation = transformation,
                AllowedForStrict = true
            };
        }

        private void AssertGetTransformResultIsStrict(GetTransformResult result, string transformName, bool isStrict)
        {
            StringAssert.AreEqualIgnoringCase(transformName, result?.Name);
            Assert.AreEqual(isStrict, result.AllowedForStrict);
        }

        [Test, RetryWithDelay]
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
                UnsafeUpdate = m_updateTransformation
            };

            m_cloudinary.UpdateTransform(updateParams);

            var getResult = m_cloudinary.GetTransform(transformationName);

            Assert.IsNotNull(getResult.Info);
            Assert.IsTrue(getResult.Named);
            Assert.AreEqual(updateParams.UnsafeUpdate.Generate(), new Transformation(getResult.Info).Generate());
        }

        [Test, RetryWithDelay]
        public void TestUpdateUnsafeUpdate()
        {
            TestUpdateTransformWithUnsafeUpdate();
        }

        [Test, RetryWithDelay]
        public void TestCreateTransformAllowStrict()
        {
            TestUpdateTransformWithUnsafeUpdate(true);
        }

        [Test, RetryWithDelay]
        public void TestCreateTransform()
        {
            // should allow creating named transformation

            var createParams = GetCreateTransformParams(m_simpleTransformation);

            var createResult = m_cloudinary.CreateTransform(createParams);
            Assert.IsNull(createResult.Error, createResult.Error?.Message);

            var getResult = m_cloudinary.GetTransform(
                new GetTransformParams { Transformation = createParams.Name });

            AssertCreateTransform(getResult, m_simpleTransformation);
        }

        /// <summary>
        /// Should create two kinds of transformations:
        /// unnamed with extension(e.g., w_150, h_150/jpg),
        /// and unnamed with empty extension(e.g., w_150, h_15).
        /// The latter leads to transformation ending with "/", which means "No extension, use original format".
        /// If format is not provided or set to None, only transformation is used(without the trailing "/")
        /// </summary>
        /// <param name="testExtention">Format key can be a string value(jpg, gif, etc) or can be set to "" (empty string)</param>
        [Test, RetryWithDelay]
        [TestCase("jpg")]
        [TestCase("")]
        public void TestUnnamedTransformationWithFormat(string testExtention)
        {
            var formatFieldName = string.IsNullOrEmpty(testExtention) ? "extension" : "format";
            var transformationAsStr = m_simpleTransformation.ToString();

            // delete if transformation with given definition and format already exists
            m_cloudinary.DeleteTransform($"{transformationAsStr}/{testExtention}");

            // should allow creating unnamed transformation with specifying format
            var createResult = m_cloudinary.CreateTransform(new CreateTransformParams()
            {
                Name = $"{transformationAsStr}/{testExtention}",
                Transform = m_simpleTransformation,
                Format = testExtention
            });
            Assert.IsNull(createResult.Error, createResult.Error?.Message);

            var getResult = m_cloudinary.GetTransform(new GetTransformParams
            {
                Transformation = transformationAsStr,
                Format = testExtention
            });
            Assert.IsNull(getResult.Error, getResult.Error?.Message);
            Assert.IsNotNull(getResult.Info);
            Assert.IsTrue(getResult.Info.Any(dict => dict.ContainsKey(formatFieldName) && (string) dict[formatFieldName] == (string.IsNullOrEmpty(testExtention) ? "none" : testExtention)));
            AssertCreateTransform(getResult, m_simpleTransformation);

            var deleteResult = m_cloudinary.DeleteTransform($"{transformationAsStr}/{testExtention}");
            Assert.IsNull(deleteResult.Error, deleteResult.Error?.Message);
        }

        [Test, RetryWithDelay]
        public async Task TestCreateTransformAsync()
        {
            // should allow creating named transformation

            var createParams = GetCreateTransformParams(m_simpleTransformation);

            await m_cloudinary.CreateTransformAsync(createParams);

            var getResult = await m_cloudinary.GetTransformAsync(
                new GetTransformParams { Transformation = createParams.Name });

            AssertCreateTransform(getResult, m_simpleTransformation);
        }


        [Test, RetryWithDelay]
        public async Task TestGetTransformNextCursorAsync()
        {
            // should allow creating named transformation

            var createParams = GetCreateTransformParams(m_simpleTransformation);

            await m_cloudinary.CreateTransformAsync(createParams);

            var getResult = await m_cloudinary.GetTransformAsync(new GetTransformParams
            {
                Transformation = createParams.Name,
                MaxResults = 1,
                NextCursor = "   "
            });

            AssertCreateTransform(getResult, m_simpleTransformation);
        }

        private CreateTransformParams GetCreateTransformParams(Transformation transformation)
        {
            return new CreateTransformParams()
            {
                Name = GetUniqueTransformationName(),
                Transform = transformation
            };
        }

        private void AssertCreateTransform(GetTransformResult result, Transformation testTransformation)
        {
            Assert.IsNotNull(result);
            Assert.IsNull(result.Error, result.Error?.Message);
            Assert.AreEqual(true, result.AllowedForStrict);
            Assert.AreEqual(false, result.Used);
            Assert.AreEqual(1, result.Info.Length);
            Assert.AreEqual(testTransformation.Generate(), new Transformation(result.Info[0]).Generate());
        }

        private void TestUpdateTransformWithUnsafeUpdate(bool allowedForstrict = false)
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
                UnsafeUpdate = m_updateTransformation,
                AllowedForStrict = allowedForstrict
            };

            m_cloudinary.UpdateTransform(updateParams);

            var getResult = m_cloudinary.GetTransform(transformationName);

            Assert.IsNotNull(getResult.Info);
            Assert.IsTrue(getResult.Named);
            Assert.AreEqual(updateParams.UnsafeUpdate.Generate(), new Transformation(getResult.Info).Generate());
        }
    }
}
