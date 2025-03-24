using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTests.SearchApi
{
    class SearchApiTest : IntegrationTestBase
    {
        private string m_searchTag;
        private string m_expressionTag;
        private string m_expressionPublicId;
        private string m_singleResourcePublicId;
        private string[] m_publicIdsSorted;
        private Dictionary<string, string> m_assetIds = new Dictionary<string, string>();
        private const int IndexingWaitTime = 5000;

        private const string SortField = "public_id";
        private const string SortDirectionAsc = "asc";

        private const string AggFieldValue = "resource_type";
        private const string MetadataFieldName = "image_metadata";
        private const string StructuredMetadataFieldName = "metadata";
        private const string TagsFieldName = "tags";
        private const string ContextFieldName = "context";
        private const string ImageAnalysisFieldName = "image_analysis";
        private const string SecureUrlFieldName = "secure_url";
        private const string UrlFieldName = "url";

        private const int FirstPageSize = 1;
        private const int SecondPageSize = 2;
        private const int ResourcesCount = 3;

        [OneTimeSetUp]
        public void InitSearchTests()
        {
            m_searchTag = GetMethodTag();
            m_expressionTag = $"tags:{m_searchTag}";
            m_publicIdsSorted = new string[ResourcesCount];

            CreateMetadataField("metadata_search");

            for (var i = 0; i < ResourcesCount; i++)
            {
                var publicId = GetUniquePublicId();
                var uploadParams = new ImageUploadParams
                {
                    PublicId = publicId,
                    Tags = $"{m_searchTag},{m_apiTag}",
                    File = new FileDescription(m_testImagePath),
                    ImageMetadata = true,
                };
                if (i == 0)
                {
                    m_singleResourcePublicId = publicId;
                    uploadParams.Context = new StringDictionary { { "some key", "some value" } };
                    uploadParams.Moderation = "manual";
                }
                m_publicIdsSorted[i] = publicId;
                var r = m_cloudinary.Upload(uploadParams);
                m_assetIds.Add(publicId, r.AssetId);
            }

            m_cloudinary.UpdateResource(m_singleResourcePublicId, ModerationStatus.Approved);

            Array.Sort(m_publicIdsSorted);
            m_expressionPublicId = $"public_id: {m_publicIdsSorted[0]}";
            Thread.Sleep(IndexingWaitTime);
        }

        [TestCase("asset_id=")]
        [TestCase("asset_id:")]
        [RetryWithDelay]
        public void TestSearchByAssetId(string key)
        {
            var result = m_cloudinary.Search().Expression($"{key}{m_assetIds[m_singleResourcePublicId]}").Execute();
            Assert.AreEqual(1, result.Resources.Count);
            Assert.AreEqual(m_singleResourcePublicId, result.Resources.First().PublicId);
        }

        [Test, RetryWithDelay]
        public void TestSearchByModerationStatus()
        {
            var result = m_cloudinary.Search().Expression("moderation_status=approved").Execute();
            Assert.GreaterOrEqual(result.Resources.Count, 1);
            Assert.AreEqual(m_singleResourcePublicId, result.Resources.First().PublicId);
            Assert.AreEqual(result.Resources.First().ModerationKind, "manual");
            Assert.AreEqual(result.Resources.First().ModerationStatus, ModerationStatus.Approved);
        }

        [Test, RetryWithDelay]
        public void TestSearchApiFindResourcesByTag()
        {
            var result = m_cloudinary.Search().Expression(m_expressionTag).Execute();

            Assert.NotNull(result.Resources, result.Error?.Message);
            Assert.AreEqual(ResourcesCount, result.Resources.Count);
        }

        [Test, RetryWithDelay]
        public void TestSearchResourceByPublicId()
        {
            var result = m_cloudinary.Search().Expression(m_expressionPublicId).Execute();
            Assert.Greater(result.TotalCount, 0, result.Error?.Message);
        }

        [Test, RetryWithDelay]
        public void TestPaginateResourcesLimitedByTagAndOrderedByAscendingPublicId()
        {
            var result = m_cloudinary.Search().MaxResults(FirstPageSize)
                .Expression(m_expressionTag).SortBy(SortField, SortDirectionAsc).Execute();

            Assert.NotNull(result.Resources, result.Error?.Message);
            Assert.AreEqual(FirstPageSize, result.Resources.Count);
            Assert.AreEqual(ResourcesCount, result.TotalCount);
            Assert.AreEqual(m_publicIdsSorted.First(), result.Resources.First().PublicId);

            result = m_cloudinary.Search().MaxResults(SecondPageSize)
                .Expression(m_expressionTag).SortBy(SortField, SortDirectionAsc)
                .NextCursor(result.NextCursor).Execute();

            Assert.NotNull(result.Resources, result.Error?.Message);
            Assert.AreEqual(SecondPageSize, result.Resources.Count);
            Assert.AreEqual(ResourcesCount, result.TotalCount);
            Assert.AreEqual(m_publicIdsSorted.Last(), result.Resources.Last().PublicId);

            Assert.True(string.IsNullOrEmpty(result.Resources[0].Folder));
            Assert.NotNull(result.Resources[0].FileName);
            Assert.NotNull(result.Resources[0].Version);
            Assert.AreEqual(ResourceType.Image, result.Resources[0].ResourceType);
            Assert.NotNull(result.Resources[0].Type);
            Assert.NotNull(result.Resources[0].UploadedAt);
            Assert.Zero(result.Resources[0].BackupBytes);
            Assert.NotZero(result.Resources[0].AspectRatio);
            Assert.NotZero(result.Resources[0].Pixels);
            Assert.NotZero(result.Resources[0].Pages);
            Assert.NotNull(result.Resources[0].Url);
            Assert.NotNull(result.Resources[0].SecureUrl);
            Assert.NotNull(result.Resources[0].Status);
            Assert.Null(result.Resources[0].AccessControl);
            Assert.NotNull(result.Resources[0].Etag);
            Assert.NotNull(result.Resources[0].UploadedBy);
            Assert.NotNull(result.Resources[0].CreatedBy);
        }

        [Test, RetryWithDelay]
        public void TestSearchAggregate()
        {
            var result = m_cloudinary.Search()
                .Expression(m_expressionTag).Aggregate(AggFieldValue).Execute();

            AssertSupportsAggregation(result);

            Assert.NotNull(result.Resources, result.Error?.Message);
            Assert.AreEqual(ResourcesCount, result.Resources.Count);
            Assert.IsNotEmpty(result.Aggregations);
        }

        [Test, RetryWithDelay]
        public void TestSearchWithField()
        {
            var result = m_cloudinary.Search().MaxResults(FirstPageSize)
                .Expression(m_expressionTag).WithField(MetadataFieldName).Execute();

            Assert.NotNull(result.Resources, result.Error?.Message);
            Assert.AreEqual(FirstPageSize, result.Resources.Count);
            Assert.IsNotEmpty(result.Resources.First().ImageMetadata);
        }

        [Test, RetryWithDelay]
        public void TestSearchFields()
        {
            var result = m_cloudinary.Search().MaxResults(FirstPageSize)
                .Expression(m_expressionTag).Fields(MetadataFieldName)
                .Fields(new List<string>{SecureUrlFieldName, SortField}).Execute();

            Assert.NotNull(result.Resources, result.Error?.Message);
            Assert.AreEqual(FirstPageSize, result.Resources.Count);
            Assert.IsNotEmpty(result.Resources.First().PublicId);
            Assert.IsNotEmpty(result.Resources.First().ImageMetadata);
            Assert.IsNotEmpty(result.Resources.First().SecureUrl);
            Assert.IsNull(result.Resources.First().Url);
        }

        [Test, RetryWithDelay]
        public void TestRootResponseFieldsAreParsed()
        {
            var result = m_cloudinary.Search().MaxResults(FirstPageSize)
                .Expression(m_expressionTag).Aggregate(AggFieldValue).Execute();

            AssertSupportsAggregation(result);

            Assert.Greater(result.TotalCount, 1, result.Error?.Message);
            Assert.Greater(result.Time, 0);
            Assert.IsNotEmpty(result.Resources);
            Assert.IsNotNull(result.NextCursor);
            Assert.IsNotEmpty(result.Aggregations);
        }

        [Test, RetryWithDelay]
        public void TestResourceResponseFieldsAreParsed()
        {
            var result = m_cloudinary.Search().Expression($"public_id: {m_singleResourcePublicId}")
                .WithField(MetadataFieldName).WithField(ImageAnalysisFieldName)
                .WithField(ContextFieldName).WithField(TagsFieldName).WithField(StructuredMetadataFieldName)
                .Execute();
            var foundResource = result.Resources.First();

            Assert.AreEqual(m_singleResourcePublicId, foundResource.PublicId, result.Error?.Message);
            Assert.True(string.IsNullOrEmpty(foundResource.Folder));
            Assert.AreEqual(m_singleResourcePublicId, foundResource.FileName);
            Assert.AreEqual(FILE_FORMAT_JPG, foundResource.Format);
            Assert.IsNotEmpty(foundResource.Version);
            Assert.AreEqual(ResourceType.Image, foundResource.ResourceType);
            Assert.AreEqual(STORAGE_TYPE_UPLOAD, foundResource.Type);
            Assert.IsNotEmpty(foundResource.CreatedAt);
            Assert.IsNotEmpty(foundResource.UploadedAt);
            Assert.AreEqual(93502, foundResource.Bytes);
            Assert.IsTrue(foundResource.BackupBytes >= 0);
            Assert.AreEqual(1920, foundResource.Width);
            Assert.AreEqual(1200, foundResource.Height);
            Assert.AreEqual(1.6, foundResource.AspectRatio);
            Assert.AreEqual(2304000, foundResource.Pixels);
            Assert.AreEqual(1, foundResource.Pages);
            Assert.IsTrue(foundResource.Url.Contains("http://"));
            Assert.IsTrue(foundResource.SecureUrl.Contains("https://"));
            Assert.AreEqual("active", foundResource.Status);
            Assert.IsNull(foundResource.AccessControl);
            Assert.IsNotEmpty(foundResource.Etag);
            Assert.Contains(m_searchTag, foundResource.Tags);
            Assert.IsNotEmpty(foundResource.ImageMetadata);
            Assert.AreEqual(1, foundResource.Context.Count);
            Assert.AreEqual(0, foundResource.ImageAnalysis.FaceCount);
            Assert.IsNotNull(foundResource.ImageAnalysis);
        }

        private static void AssertSupportsAggregation(SearchResult result)
        {
            if (result.Error?.Message.Contains("does not support aggregations") == true)
            {
                Assert.Inconclusive(result.Error.Message);
            }
        }
    }
}
