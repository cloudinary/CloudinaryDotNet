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

        [Test]
        public void TestListTransformations()
        {
            // should allow listing transformations

            UploadTestImageResource((uploadParams) =>
            {
                uploadParams.EagerTransforms = new List<Transformation>() { m_simpleTransformation };
            });

            var result = m_cloudinary.ListTransformations();

            CheckNotEmptyListAndContainsTransformation(result, m_simpleTransformationAsString);
        }

        [Test]
        public async Task TestListTransformationsAsync()
        {
            // should allow listing transformations

            await UploadTestImageResourceAsync((uploadParams) =>
            {
                uploadParams.EagerTransforms = new List<Transformation>() { m_simpleTransformation };
            });

            var result = await m_cloudinary.ListTransformationsAsync();

            CheckNotEmptyListAndContainsTransformation(result, m_simpleTransformationAsString);
        }

        private void CheckNotEmptyListAndContainsTransformation(ListTransformsResult result, string transformation)
        {
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Transformations);

            var td = result.Transformations
                .Where(t => t.Name == transformation)
                .First();

            Assert.IsFalse(td.Named);
            Assert.IsTrue(td.Used);
        }

        [Test]
        public void TestGetTransform()
        {
            // should allow getting transformation metadata

            UploadTestImageResource((uploadParams) => {
                uploadParams.EagerTransforms = new List<Transformation>() { m_updateTransformation };
            });

            var result = m_cloudinary.GetTransform(m_updateTransformationAsString);

            CheckGetTransform(result, m_updateTransformation);
        }

        [Test]
        public async Task TestGetTransformAsync()
        {
            // should allow getting transformation metadata

            await UploadTestImageResourceAsync((uploadParams) => {
                uploadParams.EagerTransforms = new List<Transformation>() { m_updateTransformation };
            });

            var result = await m_cloudinary.GetTransformAsync(m_updateTransformationAsString);

            CheckGetTransform(result, m_updateTransformation);
        }

        private void CheckGetTransform(GetTransformResult result, Transformation transformation)
        {
            Assert.IsNotNull(result);
            Assert.AreEqual(transformation.ToString(), new Transformation(result.Info[0]).ToString());
        }

        [Test]
        public void TestGetTransformParamsCheck()
        {
            Assert.Throws<ArgumentException>(new GetTransformParams().Check, "Should require Transformation");
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

            CheckGetTransformResultIsStrict(getResult, m_simpleTransformationAsString, true);

            updateParams.Strict = false;
            m_cloudinary.UpdateTransform(updateParams);

            getResult = m_cloudinary.GetTransform(m_simpleTransformationAsString);

            CheckGetTransformResultIsStrict(getResult, m_simpleTransformationAsString, false);
        }

        [Test]
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

            CheckGetTransformResultIsStrict(getResult, m_simpleTransformationAsString, true);

            updateParams.Strict = false;
            await m_cloudinary.UpdateTransformAsync(updateParams);

            getResult = await m_cloudinary.GetTransformAsync(m_simpleTransformationAsString);

            CheckGetTransformResultIsStrict(getResult, m_simpleTransformationAsString, false);
        }

        private UpdateTransformParams GetUpdateTransformParamsStrict(string transformation)
        {
            return new UpdateTransformParams()
            {
                Transformation = transformation,
                Strict = true
            };
        }

        private void CheckGetTransformResultIsStrict(GetTransformResult result, string transformName, bool isStrict)
        {
            StringAssert.AreEqualIgnoringCase(transformName, result?.Name);
            Assert.AreEqual(isStrict, result.Strict);
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

            var createParams = GetCreateTransformParams(m_simpleTransformation);

            m_cloudinary.CreateTransform(createParams);

            var getResult = m_cloudinary.GetTransform(
                new GetTransformParams { Transformation = createParams.Name });

            CheckCreateTransform(getResult, m_simpleTransformation);
        }

        [Test]
        public async Task TestCreateTransformAsync()
        {
            // should allow creating named transformation

            var createParams = GetCreateTransformParams(m_simpleTransformation);

            await m_cloudinary.CreateTransformAsync(createParams);

            var getResult = await m_cloudinary.GetTransformAsync(
                new GetTransformParams { Transformation = createParams.Name });

            CheckCreateTransform(getResult, m_simpleTransformation);
        }

        private CreateTransformParams GetCreateTransformParams(Transformation transformation)
        {
            return new CreateTransformParams()
            {
                Name = GetUniqueTransformationName(),
                Transform = transformation
            };
        }

        private void CheckCreateTransform(GetTransformResult result, Transformation testTransformation)
        {
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Strict);
            Assert.AreEqual(false, result.Used);
            Assert.AreEqual(1, result.Info.Length);
            Assert.AreEqual(testTransformation.Generate(), new Transformation(result.Info[0]).Generate());
        }
    }
}
