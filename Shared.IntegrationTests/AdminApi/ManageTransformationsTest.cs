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
        public async Task TestListTransformationsAsync()
        {
            // should allow listing transformations

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                Tags = m_apiTag
            };

            await m_cloudinary.UploadAsync(uploadParams);

            var result = await m_cloudinary.ListTransformationsAsync();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Transformations);

            var td = result.Transformations
                .Where(t => t.Name == m_simpleTransformationAsString)
                .First();

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
        public async Task TestGetTransformAsync()
        {
            // should allow getting transformation metadata

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_updateTransformation },
                Tags = m_apiTag
            };

            await m_cloudinary.UploadAsync(uploadParams);

            var result = await m_cloudinary.GetTransformAsync(m_updateTransformationAsString);

            Assert.IsNotNull(result);
            Assert.AreEqual(m_updateTransformation.ToString(), new Transformation(result.Info[0]).ToString());
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
        public async Task TestUpdateTransformStrictAsync()
        {
            // should allow updating transformation allowed_for_strict

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                Tags = m_apiTag
            };

            await m_cloudinary.UploadAsync(uploadParams);

            var updateParams = new UpdateTransformParams()
            {
                Transformation = m_simpleTransformationAsString,
                Strict = true
            };

            await m_cloudinary.UpdateTransformAsync(updateParams);

            var getResult = await m_cloudinary.GetTransformAsync(m_simpleTransformationAsString);

            Assert.IsNotNull(getResult);
            Assert.AreEqual(true, getResult.Strict);

            updateParams.Strict = false;
            await m_cloudinary.UpdateTransformAsync(updateParams);

            getResult = await m_cloudinary.GetTransformAsync(m_simpleTransformationAsString);

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
        public async Task TestCreateTransformAsync()
        {
            // should allow creating named transformation

            var createParams = new CreateTransformParams()
            {
                Name = GetUniqueAsyncTransformationName(),
                Transform = m_simpleTransformation
            };

            await m_cloudinary.CreateTransformAsync(createParams);

            var get = new GetTransformParams()
            {
                Transformation = createParams.Name
            };

            var getResult = await m_cloudinary.GetTransformAsync(get);

            Assert.IsNotNull(getResult);
            Assert.AreEqual(true, getResult.Strict);
            Assert.AreEqual(false, getResult.Used);
            Assert.AreEqual(1, getResult.Info.Length);
            Assert.AreEqual(m_simpleTransformation.Generate(), new Transformation(getResult.Info[0]).Generate());
        }
    }
}
