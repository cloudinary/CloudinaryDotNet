using System;
using System.Collections.Generic;
using CloudinaryDotNet.Actions;
using NUnit.Framework;
using SystemHttp = System.Net.Http;

namespace CloudinaryDotNet.Tests.UploadApi
{
    public class ConcatVideoMethodsTest
    {
        [Test]
        public void TestConcatVideoSendsUrlsAndOptionalParams()
        {
            var cloudinary = new MockedCloudinary();

            var prms = new ConcatVideoParams
            {
                Urls = new List<string>
                {
                    "https://example.com/segments/0.ts",
                    "https://example.com/segments/1.ts",
                },
                PublicId = "my_concatenated_video",
                NotificationUrl = "https://example.com/hook",
                UploadPreset = "test_preset",
                Overwrite = true,
                Tags = "a,b",
            };

            cloudinary.ConcatVideo(prms);

            cloudinary.AssertHttpCall(SystemHttp.HttpMethod.Post, "video/concat");

            foreach (var expected in new List<string>
            {
                "https://example.com/segments/0.ts",
                "https://example.com/segments/1.ts",
                "my_concatenated_video",
                "https://example.com/hook",
                "test_preset",
            })
            {
                StringAssert.Contains(expected, cloudinary.HttpRequestContent);
            }

            // urls is sent as urls[] (multipart array convention).
            StringAssert.Contains("urls[]", cloudinary.HttpRequestContent);

            // file should never appear since concat ignores it.
            StringAssert.DoesNotContain("name=\"file\"", cloudinary.HttpRequestContent);
        }

        [Test]
        public void TestConcatVideoThrowsOnEmptyUrls()
        {
            Assert.Throws<ArgumentException>(() => new ConcatVideoParams { Urls = new List<string>() }.Check());
            Assert.Throws<ArgumentException>(() => new ConcatVideoParams { Urls = null }.Check());
        }

        [Test]
        public void TestConcatVideoThrowsOnBlankEntry()
        {
            var prms = new ConcatVideoParams { Urls = new List<string> { "https://x", "  " } };
            Assert.Throws<ArgumentException>(() => prms.Check());
        }

        [Test]
        public void TestConcatVideoThrowsWhenExceedingMaxUrls()
        {
            var urls = new List<string>();
            for (var i = 0; i <= ConcatVideoParams.MaxUrls; i++)
            {
                urls.Add($"https://example.com/{i}.ts");
            }

            Assert.Throws<ArgumentException>(() => new ConcatVideoParams { Urls = urls }.Check());
        }
    }
}
