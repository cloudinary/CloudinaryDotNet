using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.Test
{
    public class TransformationsAdminApiTest : IntegrationTestBase
    {
        protected List<GetTransformResult> results = new List<GetTransformResult>();
        protected object[][] m_transformationDetails;

        [OneTimeSetUp]
        public override void Initialize()
        {
            base.Initialize();

            var transformationName = GetUniqueTransformationName();

            m_transformationDetails = new object[][] {
                // {Name, Transformation, Format}
                new object[]{transformationName, m_simpleTransformation, null}, // To create named transformation.
                new object[]{m_updateTransformationWithExtAsString, m_updateTransformation, FILE_FORMAT_JPG}, // To create transformation with extension '/jpg'.
                new object[]{m_updateTransformationWithEmptyExtAsString, m_updateTransformation, String.Empty} // To create transformation with extension '/'.
            };
        }

        [SetUp]
        public void CreateTransformations()
        {
            /* 
           Should create three kinds of transformations: Named, 
           unamed with extension (e.g., w_150,h_150/jpg), and unamed with empty extension (e.g., w_150,h_15) 
           */
            foreach (var transformationDetail in m_transformationDetails)
            {
                string name = transformationDetail.GetValue(0).ToString();
                var transformation = (Transformation)transformationDetail.GetValue(1);
                string format = null;

                if (transformationDetail[2] != null)
                    format = transformationDetail.GetValue(2).ToString();

                var createResult = CreateTransformationForTest(name, transformation, format);

                Assert.AreEqual("created", createResult.Message);

                var getResult = GetTransformationForTest(name);

                Assert.AreEqual(true, getResult.Strict);
                Assert.AreEqual(false, getResult.Used);
                Assert.AreEqual(1, getResult.Info.Length);
                Assert.AreEqual(transformation.Generate(), new Transformation(getResult.Info[0]).Generate());

                results.Add(getResult);

                // Testing deletion for each option
                m_cloudinary.DeleteTransform(name);

                var getResultAfterDeletion = GetTransformationForTest(name);

                Assert.AreEqual(HttpStatusCode.NotFound, getResultAfterDeletion.StatusCode);
            }
        }

        [OneTimeTearDown]
        public override void Cleanup()
        {
            base.Cleanup();            
        }

        public TransformResult CreateTransformationForTest(string name, Transformation transformation, string format = null)
        {
            CreateTransformParams create = new CreateTransformParams()
            {
                Name = name,
                Transform = transformation,
                Format = format
            };

            TransformResult result = m_cloudinary.CreateTransform(create);

            return result;
        }

        public GetTransformResult GetTransformationForTest(string name, string format = null)
        {
            GetTransformParams get = new GetTransformParams()
            {
                Transformation = name
            };

            GetTransformResult result = m_cloudinary.GetTransform(get);

            return result;
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
        public void TestCreateTransformation()
        {
             //unamed with empty extension
             if (results[1].Info[0].TryGetValue("format", out var formatResult))
             {
                 string formatStr = formatResult.ToString();

                 Assert.AreEqual(FILE_FORMAT_JPG, formatStr);
             }
             //unamed with empty extension
             else if (results[2].Info[0].TryGetValue("extension", out var extension))
             {
                 string extensionStr = extension.ToString();

                 Assert.AreEqual("none", extensionStr);
             }
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
            Assert.IsTrue(td.Used);
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
            Assert.AreEqual(updateParams.UnsafeTransform.Generate(), new Transformation(getResult.Info).Generate());
        }
    }
}
