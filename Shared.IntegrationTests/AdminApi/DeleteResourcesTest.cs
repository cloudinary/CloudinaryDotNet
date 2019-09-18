using CloudinaryDotNet.Actions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudinaryDotNet.IntegrationTest.AdminApi
{
    public class DeleteResourcesTest: IntegrationTestBase
    {
        [Test]
        public void TestDelete()
        {
            // should allow deleting resources
            var uploadResult = UploadTestImageResource();

            var uploadedPublicId = uploadResult.PublicId;
            var nonExistingPublicId = GetUniquePublicId();

            var resource = m_cloudinary.GetResource(uploadedPublicId);

            CheckResourceExists(resource, uploadedPublicId);

            var delResult = m_cloudinary.DeleteResources(nonExistingPublicId, uploadedPublicId);

            CheckResourceDeleted(delResult, uploadedPublicId, nonExistingPublicId);

            resource = m_cloudinary.GetResource(uploadedPublicId);

            CheckResourceDoesNotExist(resource);
        }

        [Test]
        public async Task TestDeleteAsync()
        {
            // should allow deleting resources
            var uploadResult = await UploadTestImageResourceAsync();

            var uploadedPublicId = uploadResult.PublicId;
            var nonExistingPublicId = GetUniquePublicId();

            var resource = await m_cloudinary.GetResourceAsync(uploadedPublicId);

            CheckResourceExists(resource, uploadedPublicId);

            var delResult = await m_cloudinary.DeleteResourcesAsync(nonExistingPublicId, uploadedPublicId);

            CheckResourceDeleted(delResult, uploadedPublicId, nonExistingPublicId);

            resource = await m_cloudinary.GetResourceAsync(uploadedPublicId);

            CheckResourceDoesNotExist(resource);
        }

        private void CheckResourceExists(GetResourceResult resource, string publicId)
        {
            Assert.IsNotNull(resource);
            Assert.AreEqual(publicId, resource.PublicId);
        }

        private void CheckResourceDoesNotExist(GetResourceResult resource)
        {
            Assert.IsNotNull(resource);
            Assert.IsNull(resource.PublicId);
        }

        private void CheckResourceDeleted(DelResResult result, string deletedPublicId, string nonExistingPublicId)
        {
            Assert.IsNotNull(result);
            Assert.AreEqual("not_found", result.Deleted[nonExistingPublicId]);
            Assert.AreEqual("deleted", result.Deleted[deletedPublicId]);
        }

        [Test]
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
            Assert.AreEqual(3, resource.Derived.Length);

            var delParams = new DelResParams { Transformations = transformations };
            delParams.PublicIds.Add(publicId);

            DelResResult delResult = m_cloudinary.DeleteResources(delParams);
            Assert.IsNotNull(delResult.Deleted);
            Assert.AreEqual(1, delResult.Deleted.Count);

            resource = m_cloudinary.GetResource(publicId);
            Assert.IsNotNull(resource);
            Assert.AreEqual(resource.Derived.Length, 0);
        }

        [Test]
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
            Assert.AreEqual(publicId, resource.PublicId);

            m_cloudinary.DeleteResourcesByPrefix(prefix);
            resource = m_cloudinary.GetResource(publicId);
            Assert.IsTrue(String.IsNullOrEmpty(resource.PublicId));
        }

        [Test]
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
            Assert.AreEqual(3, resource.Derived.Length);

            var delResult = m_cloudinary.DeleteResources(new DelResParams
            {
                Prefix = prefix,
                Transformations = transformations
            });
            Assert.NotNull(delResult.Deleted);
            Assert.AreEqual(delResult.Deleted.Count, 1);

            resource = m_cloudinary.GetResource(publicId);
            Assert.IsNotNull(resource);
            Assert.AreEqual(resource.Derived.Length, 0);
        }

        [Test]
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
            Assert.AreEqual(publicId, resource.PublicId);

            DelResResult delResult = m_cloudinary.DeleteResourcesByTag(tag);

            resource = m_cloudinary.GetResource(publicId);
            Assert.IsTrue(String.IsNullOrEmpty(resource.PublicId));
        }

        [Test]
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

            Assert.NotNull(delResult.Deleted);
            Assert.AreEqual(delResult.Deleted.Count, 1);

            delResult = m_cloudinary.DeleteResources(new DelResParams
            {
                Tag = tag,
                Transformations = new List<Transformation>() { m_simpleTransformationAngle, m_explicitTransformation }
            });

            Assert.NotNull(delResult.Deleted);

            GetResourceResult resource = m_cloudinary.GetResource(publicId);
            Assert.IsNotNull(resource);
            Assert.AreEqual(resource.Derived.Length, 0);
        }

        [Test]
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
            Assert.AreEqual(1, resource.Derived.Length);

            DelDerivedResResult delDerivedResult = m_cloudinary.DeleteDerivedResources(resource.Derived[0].Id);
            Assert.AreEqual(1, delDerivedResult.Deleted.Values.Count);

            resource = m_cloudinary.GetResource(publicId);
            Assert.IsFalse(String.IsNullOrEmpty(resource.PublicId));
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
                    MaxResults = MAX_RESULTS
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
    }
}
