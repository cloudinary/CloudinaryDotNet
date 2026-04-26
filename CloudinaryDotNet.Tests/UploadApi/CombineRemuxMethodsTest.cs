using System;
using System.Collections.Generic;
using CloudinaryDotNet.Actions;
using NUnit.Framework;
using SystemHttp = System.Net.Http;

namespace CloudinaryDotNet.Tests.UploadApi
{
    public class CombineRemuxMethodsTest
    {
        [Test]
        public void TestCombineRemuxSendsUrlsAndOptionalParams()
        {
            var cloudinary = new MockedCloudinary();

            var prms = new CombineRemuxParams
            {
                Urls = new List<string>
                {
                    "https://example.com/chunks/0.ts",
                    "https://example.com/chunks/1.ts",
                },
                PublicId = "my_combined_video",
                NotificationUrl = "https://example.com/hook",
                UploadPreset = "test_preset",
                Overwrite = true,
                Tags = "a,b",
            };

            cloudinary.CombineRemux(prms);

            cloudinary.AssertHttpCall(SystemHttp.HttpMethod.Post, "video/combine_remux");

            foreach (var expected in new List<string>
            {
                "https://example.com/chunks/0.ts",
                "https://example.com/chunks/1.ts",
                "my_combined_video",
                "https://example.com/hook",
                "test_preset",
            })
            {
                StringAssert.Contains(expected, cloudinary.HttpRequestContent);
            }

            // urls is sent as urls[] (multipart array convention).
            StringAssert.Contains("urls[]", cloudinary.HttpRequestContent);

            // file should never appear since combine_remux ignores it.
            StringAssert.DoesNotContain("name=\"file\"", cloudinary.HttpRequestContent);
        }

        [Test]
        public void TestCombineRemuxThrowsOnEmptyUrls()
        {
            Assert.Throws<ArgumentException>(() => new CombineRemuxParams { Urls = new List<string>() }.Check());
            Assert.Throws<ArgumentException>(() => new CombineRemuxParams { Urls = null }.Check());
        }

        [Test]
        public void TestCombineRemuxThrowsOnBlankEntry()
        {
            var prms = new CombineRemuxParams { Urls = new List<string> { "https://x", "  " } };
            Assert.Throws<ArgumentException>(() => prms.Check());
        }

        [Test]
        public void TestCombineRemuxThrowsWhenExceedingMaxUrls()
        {
            var urls = new List<string>();
            for (var i = 0; i <= CombineRemuxParams.MaxUrls; i++)
            {
                urls.Add($"https://example.com/{i}.ts");
            }

            Assert.Throws<ArgumentException>(() => new CombineRemuxParams { Urls = urls }.Check());
        }
    }
}
