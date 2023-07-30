using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests
{
    public class SearchTest
    {
        private MockedCloudinary cloudinary = new MockedCloudinary();

        private Search search;

        private string searchExpression = "resource_type:image AND tags=kitten AND uploaded_at>1d AND bytes>1m";

        private const string B64Query = "eyJleHByZXNzaW9uIjoicmVzb3VyY2VfdHlwZTppbWFnZSBBTkQgdGFncz1raXR0ZW4gQU5EIHV" +
                                        "wbG9hZGVkX2F0PjFkIEFORCBieXRlcz4xbSIsIm1heF9yZXN1bHRzIjozMCwic29ydF9ieSI6W3" +
                                        "sicHVibGljX2lkIjoiZGVzYyJ9XX0=";

        private const string Ttl300Sig  = "431454b74cefa342e2f03e2d589b2e901babb8db6e6b149abf25bc0dd7ab20b7";
        private const string Ttl1000Sig = "25b91426a37d4f633a9b34383c63889ff8952e7ffecef29a17d600eeb3db0db7";
        private const string NextCursor = "8c452e112d4c88ac7c9ffb3a2a41c41bef24";

        private const string SearchUrlPrefix = "https://res.cloudinary.com/test123/search";

        [SetUp]
        public void SetUp()
        {
            cloudinary = new MockedCloudinary
            {
                Api =
                {
                    Secure = true
                }
            };

            search = cloudinary.Search()
                .Expression(searchExpression)
                .SortBy("public_id", "desc")
                .MaxResults(30);
        }


        [Test]
        public void TestSearchUrl()
        {
            Assert.AreEqual($"{SearchUrlPrefix}/{Ttl300Sig}/300/{B64Query}", search.ToUrl());
        }

        [Test]
        public void TestSearchUrlWithNextCursor()
        {
            Assert.AreEqual(
            $"{SearchUrlPrefix}/{Ttl300Sig}/300/{B64Query}/{NextCursor}",
             search.ToUrl(null, NextCursor)
             );
        }

        [Test]
        public void TestSearchUrlWithCustomTtlAndNextCursor()
        {
            Assert.AreEqual(
                $"{SearchUrlPrefix}/{Ttl1000Sig}/1000/{B64Query}/{NextCursor}",
                search.ToUrl(1000, NextCursor)
            );
        }

        [Test]
        public void TestSearchUrlWithCustomTtlAndNextCursorSetFromTheClass()
        {
            Assert.AreEqual(
                $"{SearchUrlPrefix}/{Ttl1000Sig}/1000/{B64Query}/{NextCursor}",
                search.Ttl(1000).NextCursor(NextCursor).ToUrl()
            );
        }

        [Test]
        public void TestSearchUrlPrivateCdn()
        {
            cloudinary.Api.UsePrivateCdn = true;

            Assert.AreEqual(
                $"https://test123-res.cloudinary.com/search/{Ttl300Sig}/300/{B64Query}",
                cloudinary.Search().Expression(searchExpression).SortBy("public_id", "desc")
                    .MaxResults(30).ToUrl()
            );
        }


        [Test]
        public void TestShouldNotDuplicateValues()
        {
            cloudinary
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

            AssertCorrectRequest(cloudinary.HttpRequestContent);
        }

        private static void AssertCorrectRequest(string request)
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
