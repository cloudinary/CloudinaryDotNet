using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTests.AdminApi
{
    public class BrowseResourcesTest: IntegrationTestBase
    {
        private const string MODERATION_MANUAL = "manual";

        [Test, RetryWithDelay]
        public void TestListResources()
        {
            // should allow listing resources

            ListResourcesResult resources = m_cloudinary.ListResources();
            Assert.NotNull(resources);
            Assert.NotZero(resources.Resources.Length, resources.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestListByModerationUpdate()
        {
            // should support listing by moderation kind and value

            List<ImageUploadResult> uploadResults = new List<ImageUploadResult>();

            for (int i = 0; i < 3; i++)
            {
                uploadResults.Add(m_cloudinary.Upload(new ImageUploadParams()
                {
                    File = new FileDescription(m_testImagePath),
                    Moderation = MODERATION_MANUAL,
                    Tags = m_apiTag
                }));
            }

            m_cloudinary.UpdateResource(uploadResults[0].PublicId, ModerationStatus.Approved);
            m_cloudinary.UpdateResource(uploadResults[1].PublicId, ModerationStatus.Rejected);

            var requestParams = new ListResourcesByModerationParams()
            {
                MaxResults = MAX_RESULTS,
                ModerationKind = MODERATION_MANUAL,
            };

            requestParams.ModerationStatus = ModerationStatus.Approved;
            var approved = m_cloudinary.ListResources(requestParams);

            requestParams.ModerationStatus = ModerationStatus.Rejected;
            var rejected = m_cloudinary.ListResources(requestParams);

            requestParams.ModerationStatus = ModerationStatus.Pending;
            var pending = m_cloudinary.ListResources(requestParams);

            Assert.True(approved.Resources.Count(r => r.PublicId == uploadResults[0].PublicId) > 0, approved.Error?.Message);
            Assert.True(approved.Resources.Count(r => r.PublicId == uploadResults[1].PublicId) == 0, approved.Error?.Message);
            Assert.True(approved.Resources.Count(r => r.PublicId == uploadResults[2].PublicId) == 0, approved.Error?.Message);

            Assert.True(rejected.Resources.Count(r => r.PublicId == uploadResults[0].PublicId) == 0, rejected.Error?.Message);
            Assert.True(rejected.Resources.Count(r => r.PublicId == uploadResults[1].PublicId) > 0, rejected.Error?.Message);
            Assert.True(rejected.Resources.Count(r => r.PublicId == uploadResults[2].PublicId) == 0, rejected.Error?.Message);

            Assert.True(pending.Resources.Count(r => r.PublicId == uploadResults[0].PublicId) == 0, pending.Error?.Message);
            Assert.True(pending.Resources.Count(r => r.PublicId == uploadResults[1].PublicId) == 0, pending.Error?.Message);
            Assert.True(pending.Resources.Count(r => r.PublicId == uploadResults[2].PublicId) > 0, pending.Error?.Message);
        }

        [Test, Ignore("test needs to be re-written with mocking - it fails when there are many resources")]
        public void TestResourcesListingDirection()
        {
            // should allow listing resources in both directions

            var result = m_cloudinary.ListResources(new ListResourcesByPrefixParams()
            {
                Type = STORAGE_TYPE_UPLOAD,
                MaxResults = MAX_RESULTS,
                Direction = "asc"
            });

            var list1 = result.Resources.Select(r => r.PublicId).ToArray();

            result = m_cloudinary.ListResources(new ListResourcesByPrefixParams()
            {
                Type = STORAGE_TYPE_UPLOAD,
                MaxResults = MAX_RESULTS,
                Direction = "-1"
            });

            var list2 = result.Resources.Select(r => r.PublicId).Reverse().ToArray();

            Assert.AreEqual(list1.Length, list2.Length);
            for (int i = 0; i < list1.Length; i++)
            {
                Assert.AreEqual(list1[i], list2[i]);
            }
        }

        [Test, RetryWithDelay]
        public void TestResourcesCursor()
        {
            // should allow listing resources with cursor

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = GetUniquePublicId(),
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = GetUniquePublicId(),
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            var listParams = new ListResourcesParams()
            {
                ResourceType = ResourceType.Image,
                MaxResults = 1
            };

            var result1 = m_cloudinary.ListResources(listParams);

            Assert.IsNotNull(result1.Resources, result1.Error?.Message);
            Assert.AreEqual(1, result1.Resources.Length);
            Assert.IsFalse(String.IsNullOrEmpty(result1.NextCursor));

            listParams.NextCursor = result1.NextCursor;
            var result2 = m_cloudinary.ListResources(listParams);

            Assert.IsNotNull(result2.Resources);
            Assert.AreEqual(1, result2.Resources.Length, result2.Error?.Message);
            Assert.AreNotEqual(result1.Resources[0].PublicId, result2.Resources[0].PublicId);
        }

        [Test, RetryWithDelay]
        public void TestResourceFullyQualifiedPublicId()
        {
            // should return correct FullyQualifiedPublicId

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = GetUniquePublicId(),
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            var listParams = new ListResourcesParams()
            {
                ResourceType = ResourceType.Image,
                MaxResults = 1
            };

            var result = m_cloudinary.ListResources(listParams);

            Assert.IsNotNull(result.Resources, result.Error?.Message);
            Assert.AreEqual(1, result.Resources.Length);

            var res = result.Resources[0];
            var expectedFullQualifiedPublicId = $"{res.ResourceType}/{res.Type}/{res.PublicId}";

            Assert.AreEqual(expectedFullQualifiedPublicId, res.FullyQualifiedPublicId);
        }

        [Test, RetryWithDelay]
        public void TestListResourcesStartAt()
        {
            // should allow listing resources by start date - make sure your clock is set correctly!!!

            Thread.Sleep(2000);

            DateTime start = DateTime.UtcNow;
            var publicId = GetUniquePublicId();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Tags = m_apiTag
            };
            ImageUploadResult result = m_cloudinary.Upload(uploadParams);

            Thread.Sleep(2000);

            var resources = m_cloudinary.ListResources(
                new ListResourcesParams() { Type = STORAGE_TYPE_UPLOAD, StartAt = result.CreatedAt.AddMilliseconds(-10), Direction = "asc" });

            Assert.NotNull(resources.Resources, resources.Error?.Message);
            Assert.IsTrue(resources.Resources.Length > 0, "response should include at least one resources");
            Assert.IsNotNull(resources.Resources.FirstOrDefault(res => res.PublicId == result.PublicId));
        }

        [Test, RetryWithDelay]
        public void TestListResourcesByPrefix()
        {
            // should allow listing resources by prefix
            var publicId = GetUniquePublicId();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId,
                Context = new StringDictionary("context=abc"),
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            var result = m_cloudinary.ListResourcesByPrefix(publicId, true, true, true);

            //Assert.IsTrue(result.Resources.Where(res => res.PublicId.StartsWith("testlist")).Count() == result.Resources.Count());
            Assert.IsTrue(
                result
                    .Resources
                    .Where(res => (res.Context == null ? false : res.Context["custom"]["context"].ToString() == "abc"))
                    .Count() > 0, result.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestListResourcesByPublicIds()
        {
            var publicId1 = GetUniquePublicId();
            var publicId2 = GetUniquePublicId();
            var context = new StringDictionary("key=value", "key2=value2");

            // should allow listing resources by public ids
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId1,
                Context = context,
                Tags = m_apiTag
            };
            m_cloudinary.Upload(uploadParams);

            uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId2,
                Context = context,
                Tags = m_apiTag
            };
            m_cloudinary.Upload(uploadParams);

            List<string> publicIds = new List<string>()
            {
                publicId1,
                publicId2
            };
            var result = m_cloudinary.ListResourceByPublicIds(publicIds, true, true, true);

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Resources.Length, "expected to find {0} but got {1}", new Object[] { publicIds.Aggregate((current, next) => current + ", " + next), result.Resources.Select(r => r.PublicId).Aggregate((current, next) => current + ", " + next) });
            Assert.True(result.Resources.Where(r => r.Context != null).Count() == 2);
        }

        [Test, RetryWithDelay]
        public void TestListResourcesByAssetIds()
        {
            var publicId1 = GetUniquePublicId();
            var publicId2 = GetUniquePublicId();
            var context = new StringDictionary("key=value", "key2=value2");

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId1,
                Context = context,
                Tags = m_apiTag
            };
            var uploadResult1 = m_cloudinary.Upload(uploadParams);

            uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = publicId2,
                Context = context,
                Tags = m_apiTag
            };
            var uploadResult2 = m_cloudinary.Upload(uploadParams);

            var assetIds = new List<string>()
            {
                uploadResult1.AssetId,
                uploadResult2.AssetId
            };
            var result = m_cloudinary.ListResourcesByAssetIDs(assetIds, true, true, true);

            Assert.AreEqual(publicId1, result.Resources[0].PublicId);
            Assert.AreEqual(publicId2, result.Resources[1].PublicId);
        }

        [Test, RetryWithDelay]
        public void TestListResourcesByTag()
        {
            // should allow listing resources by tag
            var localTag = GetMethodTag();
            var file = new FileDescription(m_testImagePath);
            m_cloudinary.DeleteResourcesByTag(localTag);

            m_cloudinary.Upload(PrepareImageUploadParamsWithTag(localTag, file));
            m_cloudinary.Upload(PrepareImageUploadParamsWithTag(localTag, file));

            var result = m_cloudinary.ListResourcesByTag(localTag);
            AssertListResourcesByTagResult(result);
        }

        [Test, RetryWithDelay]
        public async Task TestListResourcesByTagAsync()
        {
            // should allow listing resources by tag
            var localTag = GetMethodTag();
            var file = new FileDescription(m_testImagePath);
            await m_cloudinary.DeleteResourcesByTagAsync(localTag);

            await m_cloudinary.UploadAsync(PrepareImageUploadParamsWithTag(localTag, file));
            await m_cloudinary.UploadAsync(PrepareImageUploadParamsWithTag(localTag, file));

            var result = await m_cloudinary.ListResourcesByTagAsync(localTag);
            AssertListResourcesByTagResult(result);
        }

        private ImageUploadParams PrepareImageUploadParamsWithTag(string localTag, FileDescription file)
        {
            return new ImageUploadParams()
            {
                File = file,
                Tags = $"{m_apiTag},{localTag}"
            };
        }

        private void AssertListResourcesByTagResult(ListResourcesResult result)
        {
            Assert.AreEqual(2, result.Resources.Count(), result.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestListTags()
        {
            // should allow listing tags
            UploadTestImageResource();

            var result = m_cloudinary.ListTags(new ListTagsParams());

            AssertListTagNotEmpty(result);
        }

        [Test, RetryWithDelay]
        public async Task TestListTagsAsync()
        {
            // should allow listing tags
            await UploadTestImageResourceAsync();

            var result = await m_cloudinary.ListTagsAsync(new ListTagsParams());

            AssertListTagNotEmpty(result);
        }

        private void AssertListTagNotEmpty(ListTagsResult result)
        {
            Assert.Greater(result.Tags.Length, 0, result.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestListTagsPrefix()
        {
            // should allow listing tag by prefix
            var tag1 = $"{GetMethodTag()}_1";
            var tag2 = $"{GetMethodTag()}_2"; ;
            var tag3 = $"{GetMethodTag()}_3"; ;

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{tag1},{m_apiTag}"
            };

            m_cloudinary.Upload(uploadParams);

            uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Tags = $"{tag2},{m_apiTag}"
            };

            m_cloudinary.Upload(uploadParams);

            ListTagsResult result = m_cloudinary.ListTagsByPrefix(m_apiTag);

            Assert.Contains(tag2, result.Tags, result.Error?.Message);

            result = m_cloudinary.ListTagsByPrefix(tag3);

            Assert.IsTrue(result.Tags.Length == 0, result.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestGetResource()
        {
            // should allow get resource details
            var publicId = GetUniquePublicId();
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                PublicId = publicId,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            GetResourceResult getResult = m_cloudinary.GetResource(
                new GetResourceParams(publicId) { Phash = true });

            Assert.IsNotNull(getResult);
            Assert.AreEqual(publicId, getResult.PublicId, getResult.Error?.Message);
            Assert.AreEqual(1920, getResult.Width);
            Assert.AreEqual(1200, getResult.Height);
            Assert.AreEqual(FILE_FORMAT_JPG, getResult.Format);
            Assert.AreEqual(1, getResult.Derived.Length);
            Assert.Null(getResult.ImageMetadata);
            Assert.NotNull(getResult.Phash);
        }

        [Test, RetryWithDelay]
        public void TestGetResourceWithMetadata()
        {
            // should allow get resource metadata
            var publicId = GetUniquePublicId();

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                EagerTransforms = new List<Transformation>() { m_simpleTransformation },
                PublicId = publicId,
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            GetResourceResult getResult = m_cloudinary.GetResource(
                new GetResourceParams(publicId)
                {
                    ImageMetadata = true
                });

            Assert.IsNotNull(getResult);
            Assert.AreEqual(publicId, getResult.PublicId, getResult.Error?.Message);
            Assert.NotNull(getResult.ImageMetadata);
        }

        [Test, RetryWithDelay]
        public void TestGetPdfResourceWithNumberOfPages()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testPdfPath),
                Tags = m_apiTag
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            Assert.AreEqual(FILE_FORMAT_PDF, uploadResult.Format, uploadResult.Error?.Message);
            Assert.AreEqual(TEST_PDF_PAGES_COUNT, uploadResult.Pages);

            GetResourceResult getResult = m_cloudinary.GetResource(
                new GetResourceParams(uploadResult.PublicId)
                {
                    ImageMetadata = true,
                    Pages = true
                });

            Assert.IsNotNull(getResult);
            Assert.AreEqual(uploadResult.PublicId, getResult.PublicId);
            Assert.NotNull(getResult.ImageMetadata, getResult.Error?.Message);
            Assert.AreEqual(uploadResult.Pages, getResult.Pages);
            Assert.AreEqual(getResult.Pages, TEST_PDF_PAGES_COUNT);
        }

        [Test, RetryWithDelay]
        public void TestListResourceTypes()
        {
            // should allow listing resource_types
            ListResourceTypesResult result = m_cloudinary.ListResourceTypes();
            Assert.Contains(ResourceType.Image, result.ResourceTypes, result.Error?.Message);
        }

        [Test, Ignore("test needs to be re-written with mocking - it fails when there are many resources")]
        public void TestListResourcesByType()
        {
            // should allow listing resources by type

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                PublicId = GetUniquePublicId(),
                Tags = m_apiTag
            };

            m_cloudinary.Upload(uploadParams);

            IEnumerable<Resource> result = GetAllResults((cursor) => m_cloudinary.ListResourcesByType(STORAGE_TYPE_UPLOAD, cursor));

            Assert.IsNotEmpty(result.Where(res => res.Type == STORAGE_TYPE_UPLOAD));
            Assert.IsEmpty(result.Where(res => res.Type != STORAGE_TYPE_UPLOAD));
        }

        [Test, RetryWithDelay]
        public void TestGetResourceCinemagraphAnalysis()
        {
            var uploadResult = UploadTestImageResource();
            var getResourceParams = new GetResourceParams(uploadResult.PublicId)
            {
                CinemagraphAnalysis = true
            };

            var getResult = m_cloudinary.GetResource(getResourceParams);

            Assert.GreaterOrEqual(getResult.CinemagraphAnalysis.CinemagraphScore, 0, getResult.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestGetResourceAccessibilityAnalysis()
        {
            var uploadResult = UploadTestImageResource();
            var getResourceParams = new GetResourceParams(uploadResult.PublicId)
            {
                AccessibilityAnalysis = true
            };

            var getResult = m_cloudinary.GetResource(getResourceParams);

            CloudinaryAssert.AccessibilityAnalysisNotEmpty(getResult.AccessibilityAnalysis);
        }

        [Test, RetryWithDelay]
        public void TestGetResourceVersions()
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(m_testImagePath),
                Backup = true
            };

            var uploadResult = m_cloudinary.Upload(uploadParams);

            var resultWithVersion = m_cloudinary.GetResource(new GetResourceParams(uploadResult.PublicId)
            {
                Versions = true
            });

            Assert.IsNotNull(resultWithVersion.Versions, resultWithVersion.Error?.Message);
            Assert.NotZero(resultWithVersion.Versions.Count);

            var result = m_cloudinary.GetResource(new GetResourceParams(uploadResult.PublicId));

            Assert.IsNull(result.Versions, result.Error?.Message);
        }
    }
}
