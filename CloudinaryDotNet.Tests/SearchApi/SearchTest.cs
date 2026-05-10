using System.Collections.Generic;
using System.Linq;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests.SearchApi
{
    public class SearchTest
    {
        private MockedCloudinary _cloudinary = new MockedCloudinary();

        private Search _search;

        private const string SearchExpression = "resource_type:image AND tags=kitten AND uploaded_at>1d AND bytes>1m";

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
            _cloudinary = new MockedCloudinary
            {
                Api =
                {
                    Secure = true
                }
            };

            _search = _cloudinary.Search()
                .Expression(SearchExpression)
                .SortBy("public_id", "desc")
                .MaxResults(30);
        }


        [Test]
        public void TestSearchUrl()
        {
            Assert.AreEqual($"{SearchUrlPrefix}/{Ttl300Sig}/300/{B64Query}", _search.ToUrl());
        }

        [Test]
        public void TestSearchUrlWithNextCursor()
        {
            Assert.AreEqual(
            $"{SearchUrlPrefix}/{Ttl300Sig}/300/{B64Query}/{NextCursor}",
             _search.ToUrl(null, NextCursor)
             );
        }

        [Test]
        public void TestSearchUrlWithCustomTtlAndNextCursor()
        {
            Assert.AreEqual(
                $"{SearchUrlPrefix}/{Ttl1000Sig}/1000/{B64Query}/{NextCursor}",
                _search.ToUrl(1000, NextCursor)
            );
        }

        [Test]
        public void TestSearchUrlWithCustomTtlAndNextCursorSetFromTheClass()
        {
            Assert.AreEqual(
                $"{SearchUrlPrefix}/{Ttl1000Sig}/1000/{B64Query}/{NextCursor}",
                _search.Ttl(1000).NextCursor(NextCursor).ToUrl()
            );
        }

        [Test]
        public void TestSearchUrlPrivateCdn()
        {
            _cloudinary.Api.UsePrivateCdn = true;

            Assert.AreEqual(
                $"https://test123-res.cloudinary.com/search/{Ttl300Sig}/300/{B64Query}",
                _cloudinary.Search().Expression(SearchExpression).SortBy("public_id", "desc")
                    .MaxResults(30).ToUrl()
            );
        }

        [Test]
        public void TestShouldNotDuplicateValues()
        {
            _cloudinary
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

            AssertCorrectRequest(_cloudinary.HttpRequestContent);
        }

        [Test]
        public void TestCreatedByAllFieldsDeserialize()
        {
            const string accessKey  = "123456789012345";
            const string customId   = "user@example.com";
            const string externalId = "abc123def456ghi789jkl012mno345";

            var responseJson = JsonConvert.SerializeObject(new
            {
                total_count = 1,
                time = 24,
                resources = new[]
                {
                    new
                    {
                        asset_id     = "aabbccddeeff00112233445566778899",
                        public_id    = "sample_user_upload",
                        resource_type = "image",
                        type         = "upload",
                        created_at   = "2026-05-10T12:40:00+00:00",
                        status       = "active",
                        created_by   = new { access_key = accessKey, custom_id = customId,  external_id = externalId },
                        uploaded_by  = new { access_key = accessKey, custom_id = customId,  external_id = externalId },
                    }
                }
            });

            var cloudinary = new MockedCloudinary(responseJson);
            var result = cloudinary.Search().Execute();

            Assert.AreEqual(1, result.Resources.Count);
            var resource = result.Resources.First();

            Assert.IsNotNull(resource.CreatedBy,  "CreatedBy should not be null");
            Assert.AreEqual(accessKey,  resource.CreatedBy.AccessKey);
            Assert.AreEqual(customId,   resource.CreatedBy.CustomId);
            Assert.AreEqual(externalId, resource.CreatedBy.ExternalId);

            Assert.IsNotNull(resource.UploadedBy,  "UploadedBy should not be null");
            Assert.AreEqual(accessKey,  resource.UploadedBy.AccessKey);
            Assert.AreEqual(customId,   resource.UploadedBy.CustomId);
            Assert.AreEqual(externalId, resource.UploadedBy.ExternalId);
        }

        [Test]
        public void TestCreatedByAccessKeyOnlyDeserializes()
        {
            const string accessKey = "987654321098765";

            var responseJson = JsonConvert.SerializeObject(new
            {
                total_count = 1,
                time = 10,
                resources = new[]
                {
                    new
                    {
                        asset_id      = "ffeeddccbbaa99887766554433221100",
                        public_id     = "sample_api_key_upload",
                        resource_type = "image",
                        type          = "upload",
                        created_at    = "2026-05-05T01:12:51+00:00",
                        status        = "active",
                        created_by    = new { access_key = accessKey },
                        uploaded_by   = new { access_key = accessKey },
                    }
                }
            });

            var cloudinary = new MockedCloudinary(responseJson);
            var result = cloudinary.Search().Execute();

            var resource = result.Resources.First();

            Assert.IsNotNull(resource.CreatedBy);
            Assert.AreEqual(accessKey, resource.CreatedBy.AccessKey);
            Assert.IsNull(resource.CreatedBy.CustomId,   "CustomId should be null for API-key uploads");
            Assert.IsNull(resource.CreatedBy.ExternalId, "ExternalId should be null for API-key uploads");
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
