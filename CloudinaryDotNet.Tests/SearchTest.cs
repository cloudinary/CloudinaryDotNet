using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests
{
    public class SearchTest
    {
        private MockedCloudinary m_cloudinary = new MockedCloudinary();

        [Test]
        public void TestShouldNotDuplicateValues()
        {
            m_cloudinary
                .Search()
                .SortBy("created_at", "asc")
                .SortBy("created_at", "desc")
                .SortBy("public_id", "asc")
                .Aggregate("format")
                .Aggregate("format")
                .Aggregate("resource_type")
                .WithField("context")
                .WithField("context")
                .WithField("tags")
                .Execute();

            AssertCorrectRequest(m_cloudinary.HttpRequestContent);
        }

        private void AssertCorrectRequest(string request)
        {
            var requestJson = JToken.Parse(request);

            Assert.IsNotNull(requestJson["sort_by"]);
            Assert.AreEqual(
                new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string> { ["created_at"] = "desc" },
                    new Dictionary<string, string> { ["public_id"] = "asc" }
                },
                requestJson["sort_by"]
                    .Children<JObject>()
                    .Select(item =>
                        new Dictionary<string, string>
                        {
                            [item.Properties().First().Name] = item.Properties().First().Value.ToString()
                        })
                );

            Assert.IsNotNull(requestJson["aggregate"]);
            Assert.AreEqual(new[] { "format", "resource_type" }, requestJson["aggregate"].Values<string>());

            Assert.IsNotNull(requestJson["with_field"]);
            Assert.AreEqual(new[] { "context", "tags" }, requestJson["with_field"].Values<string>());
        }
    }
}
