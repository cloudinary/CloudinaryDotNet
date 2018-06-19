using System;
using System.Linq;
using System.Threading;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet.Test;
using NUnit.Framework;

namespace Shared.Tests
{
    class SearchApiTest : IntegrationTestBase
    {
        private string m_searchTag;
        private string m_expressionTag;
        private string m_expressionPublicId;
        private string[] m_publicIdsSorted;

        private const int INDEXING_WAIT_TIME = 5000;

        private const string SORT_FIELD = "public_id";
        private const string SORT_DIRECTION_ASC = "asc";

        private const string AGG_FIELD_NAME = "aggregations";
        private const string AGG_FIELD_VALUE = "resource_type";
        private const string METADATA_FIELD_NAME = "image_metadata";

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
                string publicId = GetUniquePublicId();
                var uploadParams = new ImageUploadParams()
                {
                    PublicId = publicId,
                    Tags = $"{m_searchTag},{m_apiTag}",
                    File = new FileDescription(m_testImagePath)
                };
                m_publicIdsSorted[i] = publicId;
                m_cloudinary.Upload(uploadParams);
            }
            
            Array.Sort(m_publicIdsSorted);
            m_expressionPublicId = $"public_id: {m_publicIdsSorted[0]}";
            Thread.Sleep(INDEXING_WAIT_TIME);
        }

        [Test]
        public void SearchApiFindResourcesByTag()
        {
            SearchResult result = m_cloudinary.Search().Expression(m_expressionTag).Execute();

            Assert.NotNull(result.Resources);
            Assert.AreEqual(RESOURCES_COUNT, result.Resources.Count);
        }
        
        [Test]
        public void SearchResourceByPublicId()
        {
            var result = m_cloudinary.Search().Expression(m_expressionPublicId).Execute();
            Assert.True(result.TotalCount > 0);
        }

        [Test]
        public void PaginateResourcesLimitedByTagAndOrderdByAscendingPublicId()
        {
            SearchResult result = m_cloudinary.Search().MaxResults(FIRST_PAGE_SIZE)
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
        }

        [Test]
        public void SearchAggregateTest()
        {
            SearchResult result = m_cloudinary.Search()
                                        .Expression(m_expressionTag).Aggregate(AGG_FIELD_VALUE).Execute();

            Assert.NotNull(result.Resources);
            Assert.AreEqual(RESOURCES_COUNT, result.Resources.Count);
            Assert.True(result.JsonObj.ToString().Contains(AGG_FIELD_NAME));
        }

        [Test]
        public void SearchWithFieldTest()
        {
            SearchResult result = m_cloudinary.Search().MaxResults(FIRST_PAGE_SIZE)
                                        .Expression(m_expressionTag).WithField(METADATA_FIELD_NAME).Execute();

            Assert.NotNull(result.Resources);
            Assert.AreEqual(FIRST_PAGE_SIZE, result.Resources.Count);
            Assert.True(result.JsonObj.ToString().Contains(METADATA_FIELD_NAME));
        }
    }
}
