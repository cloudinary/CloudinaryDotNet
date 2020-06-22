using System;
using System.Linq;
using System.Threading;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.SearchApi
{
    class SearchApiTest : IntegrationTestBase
    {
        private string m_searchTag;
        private string m_expressionTag;
        private string m_expressionPublicId;
        private string m_singleResourcePublicId;
        private string[] m_publicIdsSorted;

        private const int INDEXING_WAIT_TIME = 5000;

        private const string SORT_FIELD = "public_id";
        private const string SORT_DIRECTION_ASC = "asc";

        private const string AGG_FIELD_VALUE = "resource_type";
        private const string METADATA_FIELD_NAME = "image_metadata";
        private const string TAGS_FIELD_NAME = "tags";
        private const string CONTEXT_FIELD_NAME = "context";
        private const string IMAGE_ANALYSIS_FIELD_NAME = "image_analysis";

        private const int FIRST_PAGE_SIZE = 1;
        private const int SECOND_PAGE_SIZE = 2;
        private const int RESOURCES_COUNT = 3;

        [OneTimeSetUp]
        public void InitSearchTests()
        {
            m_searchTag = GetMethodTag();
            m_expressionTag = $"tags:{m_searchTag}";
            m_publicIdsSorted = new string[RESOURCES_COUNT];


            for (var i = 0; i < RESOURCES_COUNT; i++)
            {
                var publicId = GetUniquePublicId();
                var uploadParams = new ImageUploadParams
                {
                    PublicId = publicId,
                    Tags = $"{m_searchTag},{m_apiTag}",
                    File = new FileDescription(m_testImagePath),
                };
                if (i == 0)
                {
                    m_singleResourcePublicId = publicId;
                    uploadParams.Context = new StringDictionary { { "some key", "some value" } };
                }
                m_publicIdsSorted[i] = publicId;
                m_cloudinary.Upload(uploadParams);
            }

            Array.Sort(m_publicIdsSorted);
            m_expressionPublicId = $"public_id: {m_publicIdsSorted[0]}";
            Thread.Sleep(INDEXING_WAIT_TIME);
        }

        [Test]
        public void TestSearchApiFindResourcesByTag()
        {
            var result = m_cloudinary.Search().Expression(m_expressionTag).Execute();

            Assert.NotNull(result.Resources);
            Assert.AreEqual(RESOURCES_COUNT, result.Resources.Count);
        }

        [Test]
        public void TestSearchResourceByPublicId()
        {
            var result = m_cloudinary.Search().Expression(m_expressionPublicId).Execute();
            Assert.Greater(result.TotalCount, 0);
        }

        [Test]
        public void TestPaginateResourcesLimitedByTagAndOrderedByAscendingPublicId()
        {
            var result = m_cloudinary.Search().MaxResults(FIRST_PAGE_SIZE)
                .Expression(m_expressionTag).SortBy(SORT_FIELD, SORT_DIRECTION_ASC).Execute();

            Assert.NotNull(result.Resources);
            Assert.AreEqual(FIRST_PAGE_SIZE, result.Resources.Count);
            Assert.AreEqual(RESOURCES_COUNT, result.TotalCount);
            Assert.AreEqual(m_publicIdsSorted.First(), result.Resources.First().PublicId);

            result = m_cloudinary.Search().MaxResults(SECOND_PAGE_SIZE)
                .Expression(m_expressionTag).SortBy(SORT_FIELD, SORT_DIRECTION_ASC)
                .NextCursor(result.NextCursor).Execute();

            Assert.NotNull(result.Resources);
            Assert.AreEqual(SECOND_PAGE_SIZE, result.Resources.Count);
            Assert.AreEqual(RESOURCES_COUNT, result.TotalCount);
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

        [Test]
        public void TestSearchAggregate()
        {
            var result = m_cloudinary.Search()
                .Expression(m_expressionTag).Aggregate(AGG_FIELD_VALUE).Execute();

            AssertSupportsAggregation(result);

            Assert.NotNull(result.Resources);
            Assert.AreEqual(RESOURCES_COUNT, result.Resources.Count);
            Assert.IsNotEmpty(result.Aggregations);
        }

        [Test]
        public void TestSearchWithField()
        {
            var result = m_cloudinary.Search().MaxResults(FIRST_PAGE_SIZE)
                .Expression(m_expressionTag).WithField(METADATA_FIELD_NAME).Execute();

            Assert.NotNull(result.Resources);
            Assert.AreEqual(FIRST_PAGE_SIZE, result.Resources.Count);
            Assert.IsNotEmpty(result.Resources.First().ImageMetadata);
        }

        [Test]
        public void TestRootResponseFieldsAreParsed()
        {
            var result = m_cloudinary.Search().MaxResults(FIRST_PAGE_SIZE)
                .Expression(m_expressionTag).Aggregate(AGG_FIELD_VALUE).Execute();

            AssertSupportsAggregation(result);

            Assert.Greater(result.TotalCount, 1);
            Assert.Greater(result.Time, 0);
            Assert.IsNotEmpty(result.Resources);
            Assert.IsNotNull(result.NextCursor);
            Assert.IsNotEmpty(result.Aggregations);
        }

        [Test]
        public void TestResourceResponseFieldsAreParsed()
        {
            var result = m_cloudinary.Search().Expression($"public_id: {m_singleResourcePublicId}")
                .WithField(METADATA_FIELD_NAME).WithField(IMAGE_ANALYSIS_FIELD_NAME)
                .WithField(CONTEXT_FIELD_NAME).WithField(TAGS_FIELD_NAME).Execute();
            var foundResource = result.Resources.First();

            Assert.AreEqual(m_singleResourcePublicId, foundResource.PublicId);
            Assert.AreEqual(string.Empty, foundResource.Folder);
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
