using System.Collections.Generic;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests
{
    public class SearchTest
    {
        private Search m_search;

        [SetUp]
        public void SetUp()
        {
            m_search = new Search(null);
        }

        [Test]
        public void TestShouldNotDuplicateSortByValues()
        {
            var expected = new Dictionary<string, List<Dictionary<string, string>>>
            {
                ["sort_by"] = new List<Dictionary<string, string>>()
                {
                    {
                        new Dictionary<string, string>() { ["created_at"] = "desc"}
                    }
                }
            };

            var query = m_search.SortBy("created_at", "asc").SortBy("created_at", "desc").ToQuery();
            Assert.AreEqual(expected, query);
        }

        [Test]
        public void TestShouldNotDuplicateAggregateValues()
        {
            var expected = new Dictionary<string, object>
            {
                ["aggregate"] = new List<object>() { "format" }
            };

            var query = m_search.Aggregate("format").Aggregate("format").ToQuery();
            Assert.AreEqual(expected, query);
        }

        [Test]
        public void TestShouldNotDuplicateWithFieldValues()
        {
            var expected = new Dictionary<string, object>
            {
                ["with_field"] = new List<object>() { "context" }
            };

            var query = m_search.WithField("context").WithField("context").ToQuery();
            Assert.AreEqual(expected, query);
        }
    }
}
