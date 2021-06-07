using CloudinaryDotNet.Actions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudinaryDotNet.IntegrationTests.AdminApi
{
    public class DeleteResourcesTest: IntegrationTestBase
    {
        [Test, RetryWithDelay]
        public void TestDelete()
        {
            // should allow deleting resources
            var uploadResult = UploadTestImageResource();

            var uploadedPublicId = uploadResult.PublicId;
            var nonExistingPublicId = GetUniquePublicId();

            var resource = m_cloudinary.GetResource(uploadedPublicId);

            AssertResourceExists(resource, uploadedPublicId);

            var delResult = m_cloudinary.DeleteResources(nonExistingPublicId, uploadedPublicId);

            AssertResourceDeleted(delResult, uploadedPublicId, nonExistingPublicId);

            resource = m_cloudinary.GetResource(uploadedPublicId);

            AssertResourceDoesNotExist(resource);
        }

        [Test, RetryWithDelay]
        public async Task TestDeleteAsync()
        {
            // should allow deleting resources
            var uploadResult = await UploadTestImageResourceAsync();

            var uploadedPublicId = uploadResult.PublicId;
            var nonExistingPublicId = GetUniquePublicId();

            var resource = await m_cloudinary.GetResourceAsync(uploadedPublicId);

            AssertResourceExists(resource, uploadedPublicId);

            var delResult = await m_cloudinary.DeleteResourcesAsync(nonExistingPublicId, uploadedPublicId);

            AssertResourceDeleted(delResult, uploadedPublicId, nonExistingPublicId);

            resource = await m_cloudinary.GetResourceAsync(uploadedPublicId);

            AssertResourceDoesNotExist(resource);
        }

        private void AssertResourceExists(GetResourceResult resource, string publicId)
        {
            Assert.IsNotNull(resource);
            Assert.AreEqual(publicId, resource.PublicId, resource.Error?.Message);
        }

        private void AssertResourceDoesNotExist(GetResourceResult resource)
        {
            Assert.IsNotNull(resource);
            Assert.IsNull(resource.PublicId, resource.Error?.Message);
        }

        private void AssertResourceDeleted(DelResResult result, string deletedPublicId, string nonExistingPublicId)
        {
            Assert.IsNotNull(result);
            Assert.AreEqual("not_found", result.Deleted[nonExistingPublicId], result.Error?.Message);
            Assert.AreEqual("deleted", result.Deleted[deletedPublicId]);
        }

        [Test, RetryWithDelay]
        [Parallelizable(ParallelScope.None)]
        public void TestDeleteByTransformation()
        {
            // should allow deleting resources by transformations
            var publicId = GetUniquePublicId();

            var transformations = new List<Transformation>
            {
                m_simpleTransformation,
                m_simpleTransformationAngle,
                m_explicitTransformation
            };

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = m_apiTag,
                EagerTransforms = transformations
            };

            m_cloudinary.Upload(uploadParams);

            var resource = m_cloudinary.GetResource(publicId);

            Assert.IsNotNull(resource);
            Assert.AreEqual(3, resource.Derived.Length, resource.Error?.Message);

            var delParams = new DelResParams { Transformations = transformations };
            delParams.PublicIds.Add(publicId);

            DelResResult delResult = m_cloudinary.DeleteResources(delParams);
            Assert.IsNotNull(delResult.Deleted, delResult.Error?.Message);
            Assert.AreEqual(1, delResult.Deleted.Count);

            resource = m_cloudinary.GetResource(publicId);
            Assert.IsNotNull(resource);
            Assert.AreEqual(resource.Derived.Length, 0, resource.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestDeleteByPrefix()
        {
            // should allow deleting resources
            var publicId = GetUniquePublicId();
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
            Assert.AreEqual(publicId, resource.PublicId, resource.Error?.Message);

            m_cloudinary.DeleteResourcesByPrefix(prefix);
            resource = m_cloudinary.GetResource(publicId);
            Assert.IsTrue(String.IsNullOrEmpty(resource.PublicId), resource.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestDeleteByPrefixAndTransformation()
        {
            // should allow deleting resources
            var publicId = GetUniquePublicId();
            var prefix = publicId.Substring(0, publicId.Length - 1);
            var transformations = new List<Transformation>
            {
                m_simpleTransformation,
                m_simpleTransformationAngle,
                m_explicitTransformation
            };

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = m_apiTag,
                EagerTransforms = transformations
            };
            m_cloudinary.Upload(uploadParams);

            GetResourceResult resource = m_cloudinary.GetResource(publicId);
            Assert.IsNotNull(resource);
            Assert.AreEqual(3, resource.Derived.Length, resource.Error?.Message);

            var delResult = m_cloudinary.DeleteResources(new DelResParams
            {
                Prefix = prefix,
                Transformations = transformations
            });
            Assert.NotNull(delResult.Deleted);
            Assert.AreEqual(delResult.Deleted.Count, 1, delResult.Error?.Message);

            resource = m_cloudinary.GetResource(publicId);
            Assert.IsNotNull(resource);
            Assert.AreEqual(resource.Derived.Length, 0, resource.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestDeleteByTag()
        {
            // should allow deleting resources
            var publicId = GetUniquePublicId();
            var tag = GetMethodTag();

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = $"{tag},{m_apiTag}"
            };

            m_cloudinary.Upload(uploadParams);

            GetResourceResult resource = m_cloudinary.GetResource(publicId);

            Assert.IsNotNull(resource);
            Assert.AreEqual(publicId, resource.PublicId, resource.Error?.Message);

            DelResResult delResult = m_cloudinary.DeleteResourcesByTag(tag);

            resource = m_cloudinary.GetResource(publicId);
            Assert.IsTrue(String.IsNullOrEmpty(resource.PublicId), resource.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestDeleteByTagAndTransformation()
        {
            // should allow deleting resources
            string publicId = GetUniquePublicId();
            string tag = GetMethodTag();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = tag,
                EagerTransforms = new List<Transformation>()
                {
                    m_simpleTransformation,
                    m_simpleTransformationAngle,
                    m_explicitTransformation
                },
            };

            m_cloudinary.Upload(uploadParams);

            DelResResult delResult = m_cloudinary.DeleteResources(new DelResParams
            {
                Tag = tag,
                Transformations = new List<Transformation> { m_simpleTransformation }
            });

            Assert.NotNull(delResult.Deleted, delResult.Error?.Message);
            Assert.AreEqual(delResult.Deleted.Count, 1, delResult.Error?.Message);

            delResult = m_cloudinary.DeleteResources(new DelResParams
            {
                Tag = tag,
                Transformations = new List<Transformation>() { m_simpleTransformationAngle, m_explicitTransformation }
            });

            Assert.NotNull(delResult.Deleted, delResult.Error?.Message);

            GetResourceResult resource = m_cloudinary.GetResource(publicId);
            Assert.IsNotNull(resource);
            Assert.AreEqual(resource.Derived.Length, 0, resource.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestDeleteDerived()
        {
            // should allow deleting derived resource
            var publicId = GetUniquePublicId();

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
            Assert.AreEqual(1, resource.Derived.Length, resource.Error?.Message);

            DelDerivedResResult delDerivedResult = m_cloudinary.DeleteDerivedResources(resource.Derived[0].Id);
            Assert.AreEqual(1, delDerivedResult.Deleted.Values.Count, delDerivedResult.Error?.Message);

            resource = m_cloudinary.GetResource(publicId);
            Assert.IsFalse(String.IsNullOrEmpty(resource.PublicId), resource.Error?.Message);
        }

        [Test]
        public void TestDeleteResultDeleteCountProperty()
        {
            // should allow deleting resources by transformations
            var publicId = GetUniquePublicId();

            var transformations = new List<Transformation>
            {
                m_simpleTransformation,
                m_simpleTransformationAngle,
                m_explicitTransformation
            };

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = m_apiTag,
                EagerTransforms = transformations
            };

            m_cloudinary.Upload(uploadParams);

            var delParams = new DelResParams { Transformations = transformations };
            delParams.PublicIds.Add(publicId);

            DelResResult delResult = m_cloudinary.DeleteResources(delParams);
            Assert.IsNotNull(delResult.Deleted, delResult.Error?.Message);
            Assert.AreEqual(1, delResult.DeletedCounts.Count);
        }
    }
}
