using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Text.RegularExpressions;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests.Asset
{
    [TestFixture]
    public partial class UrlBuilderTest
    {
        protected Api m_api;

        private const string TestVersion = "1234";
        private const string TestVersionStr = "v1234";
        private const string DefaultVersionStr = "v1";
        private const string TestFolder = "folder/test";
        private const string TestImageId = "image.jpg";

        private const string FetchVideoUrl = "https://demo-res.cloudinary.com/videos/dog.mp4";
        private const string FetchVideoUrlBase64Enc = "aHR0cHM6Ly9kZW1vLXJlcy5jbG91ZGluYXJ5LmNvbS92aWRlb3MvZG9nLm1wNA==";

        [OneTimeSetUp]
        public void Init()
        {
            var account = new Account(TestConstants.CloudName, TestConstants.DefaultApiKey,
                TestConstants.DefaultApiSecret);
            m_api = new Api(account);
        }

        [Test]
        public void TestEnumToString()
        {
            // should escape http urls

            TagCommand command = TagCommand.SetExclusive;
            string commandStr = Api.GetCloudinaryParam<TagCommand>(command);
            Assert.AreEqual(commandStr, "set_exclusive");
        }

        [Test]
        public void TestCloudName()
        {
            // should use cloud_name from account

            string uri = m_api.UrlImgUp.BuildUrl("test");
            Assert.AreEqual(TestConstants.DefaultImageUpPath + "test", uri);
        }

        [Test]
        public void TestCustomCloudName()
        {
            // should allow overriding cloud_name in url

            string uri = m_api.UrlImgUp.CloudName("test123").BuildUrl("test");
            Assert.AreEqual("http://res.cloudinary.com/test123/image/upload/test", uri);
        }

        [Test]
        public void TestSecureDistribution()
        {
            // should use default secure distribution if secure=TRUE

            string uri = m_api.UrlImgUp.Secure(true).BuildUrl("test");
            Assert.AreEqual("https://res.cloudinary.com/testcloud/image/upload/test", uri);
        }

        [Test]
        public void TestSecureDistributionApiSettings()
        {
            const string expectedNonSecure = "http://res.cloudinary.com/testcloud/image/upload/test";
            const string expectedSecure = "https://res.cloudinary.com/testcloud/image/upload/test";

            var api = new Api(m_api.Account);

            // should be non-secure by default
            string uri = api.UrlImgUp.BuildUrl("test");
            Assert.AreEqual(expectedNonSecure, uri);

            api.Secure = true;

            // should use api settings
            uri = api.UrlImgUp.BuildUrl("test");
            Assert.AreEqual(expectedSecure, uri);

            // should override api settings
            uri = api.UrlImgUp.Secure(false).BuildUrl("test");
            Assert.AreEqual(expectedNonSecure, uri);
        }

        [Test]
        public void TestSecureAkamai()
        {
            // should default to akamai if secure is given with private_cdn and no secure_distribution

            string uri = m_api.UrlImgUp.Secure(true).PrivateCdn(true).BuildUrl("test");
            Assert.AreEqual("https://testcloud-res.cloudinary.com/image/upload/test", uri);
        }

        [Test]
        public void TestSecureNonAkamai()
        {
            // should not add cloud_name if private_cdn and secure non akamai secure_distribution

            string uri = m_api.UrlImgUp.Secure(true).PrivateCdn(true).SecureDistribution("something.cloudfront.net").BuildUrl("test");
            Assert.AreEqual("https://something.cloudfront.net/image/upload/test", uri);
        }

        [Test]
        public void TestHttpsSharding()
        {
            var uri = m_api.UrlImgUp.Secure(true).CSubDomain(true).BuildUrl("image.jpg");
            Assert.AreEqual("https://res-5.cloudinary.com/testcloud/image/upload/image.jpg", uri);
        }

        [Test]
        public void TestHttpSharding()
        {
            var uri = m_api.UrlImgUp.CSubDomain(true).BuildUrl("image.jpg");
            Assert.AreEqual("http://res-5.cloudinary.com/testcloud/image/upload/image.jpg", uri);
        }

        [Test]
        public void TestHttpPrivateCdn()
        {
            // should not add cloud_name if private_cdn and not secure

            string uri = m_api.UrlImgUp.PrivateCdn(true).BuildUrl("test");
            Assert.AreEqual("http://testcloud-res.cloudinary.com/image/upload/test", uri);
        }

        [Test]
        public void TestSecureDistributionOverwrite()
        {
            // should allow overwriting secure distribution if secure=TRUE

            string uri = m_api.UrlImgUp.Secure().SecureDistribution("something.else.com").BuildUrl("test");
            Assert.AreEqual("https://something.else.com/testcloud/image/upload/test", uri);
        }

        [Test]
        public void TestFormat()
        {
            // should use format from options

            string uri = m_api.UrlImgUp.Format("jpg").BuildUrl("test");
            Assert.AreEqual(TestConstants.DefaultImageUpPath + "test.jpg", uri);
        }


        [Test]
        public void TestAction()
        {
            // should use type of action from options

            string uri = m_api.UrlImgUp.Action("facebook").BuildUrl("test");
            Assert.AreEqual(TestConstants.DefaultRootPath + "image/facebook/test", uri);
        }

        [Test]
        public void TestResourceType()
        {
            // should use resource_type from options

            string uri = m_api.Url.ResourceType("raw").Action("upload").BuildUrl("test");
            Assert.AreEqual(TestConstants.DefaultRootPath + "raw/upload/test", uri);
        }

        [Test]
        public void TestIgnoreHttp()
        {
            // should ignore http links only if type is not given or is asset

            string uri = m_api.UrlImgUp.BuildUrl("http://test");
            Assert.AreEqual("http://test", uri);
            uri = m_api.Url.ResourceType("image").Action("asset").BuildUrl("http://test");
            Assert.AreEqual("http://test", uri);
            uri = m_api.Url.ResourceType("image").Action("fetch").BuildUrl("http://test");
            Assert.AreEqual(TestConstants.DefaultRootPath + "image/fetch/http://test", uri);
        }

        [Test]
        public void TestFetch()
        {
            // should escape fetch urls

            string uri = m_api.Url.ResourceType("image").Action("fetch").BuildUrl("http://blah.com/hello?a=b");
            Assert.AreEqual(TestConstants.DefaultRootPath + "image/fetch/http://blah.com/hello%3Fa%3Db", uri);
        }

        [Test]
        public void TestCdnName()
        {
            // should support extenal cname

            string uri = m_api.UrlImgUp.CName("hello.com").BuildUrl("test");
            Assert.AreEqual("http://hello.com/testcloud/image/upload/test", uri);
        }

        [Test]
        public void TestSubDomain()
        {
            // should support extenal cname with cdn_subdomain on

            string uri = m_api.UrlImgUp.CName("hello.com").CSubDomain(true).BuildUrl("test");
            Assert.AreEqual("http://a2.hello.com/testcloud/image/upload/test", uri);
        }

        [Test]
        public void TestHttpEscape()
        {
            string uri = m_api.Url.ResourceType("image").Action("youtube").BuildUrl("http://www.youtube.com/watch?v=d9NF2edxy-M");
            Assert.AreEqual(TestConstants.DefaultRootPath + "image/youtube/http://www.youtube.com/watch%3Fv%3Dd9NF2edxy-M", uri);
        }


        [Test]
        public void TestFetchFormat()
        {
            // should support format for fetch urls
            String result = m_api.UrlImgUp.Format("jpg").Action("fetch").BuildUrl("http://cloudinary.com/images/logo.png");
            Assert.AreEqual(TestConstants.DefaultRootPath + "image/fetch/f_jpg/http://cloudinary.com/images/logo.png", result);
        }

        [Test]
        public void TestUrlClone()
        {
            // url should be cloneable
            var layer = new TextLayer("Hello").FontFamily("Arial").FontSize(10);
            var transformation = new Transformation().Angle(12).Overlay(layer);

            Url url1 = m_api.UrlImgUp.Transform(transformation);
            Url url2 = url1.Clone().Action("go");
            transformation.Angle(14);
            layer.FontSize(20);

            string result1 = url1.BuildUrl("test");
            string result2 = url2.BuildUrl("test");

            Assert.AreEqual(TestConstants.DefaultImageUpPath + "a_14,l_text:Arial_20:Hello/test", result1,
                "Original Url should not be affected by changes to a cloned Url");
            Assert.AreEqual(TestConstants.DefaultRootPath + "image/go/a_12,l_text:Arial_10:Hello/test", result2,
                "Cloned Url should not be affected by changes to source Url params and layers");
        }

        [Test]
        public void TestInitFromUri()
        {
            var cloudinary = new CloudinaryDotNet.Cloudinary("cloudinary://a:b@test123");
        }

        [Test]
        public void TestInitFromUriProperlyFormattedUrl()
        {
            var cloudinary = new CloudinaryDotNet.Cloudinary("cloudinary://123456789012345:ALKJdjklLJAjhkKJ45hBK92baj3@test");
        }

        [Test]
        public void TestInitFromUriProperlyFormattedUrlAsEnvVar()
        {
            Environment.SetEnvironmentVariable("CLOUDINARY_URL", "cloudinary://123456789012345:ALKJdjklLJAjhkKJ45hBK92baj3@test");

            var cloudinary = new CloudinaryDotNet.Cloudinary();
        }

        [Test]
        public void TestInitFromUriInsensitiveToCaseUrl()
        {
            var cloudinary = new CloudinaryDotNet.Cloudinary("CLOUDINARY://123456789012345:ALKJdjklLJAjhkKJ45hBK92baj3@test");
        }

        [Test]
        public void TestInitFromUriInsensitiveToCaseUrlAsEnvVar()
        {
            Environment.SetEnvironmentVariable("CLOUDINARY_URL", "CLOUDINARY://123456789012345:ALKJdjklLJAjhkKJ45hBK92baj3@test");

            var cloudinary = new CloudinaryDotNet.Cloudinary();
        }

        [Test]
        public void TestInitFromUriEmptyUrl()
        {
            var exception = Assert.Throws<ArgumentException>(() => new CloudinaryDotNet.Cloudinary(string.Empty));

            AssertCloudinaryUrlExceptionMessage(exception);
        }

        [Test]
        public void TestInitFromUriEmptyUrlAsEnvVar()
        {
            Environment.SetEnvironmentVariable("CLOUDINARY_URL", string.Empty);

            var exception = Assert.Throws<ArgumentException>(() => new CloudinaryDotNet.Cloudinary());

            AssertCloudinaryUrlExceptionMessage(exception);
        }

        [Test]
        public void TestInitFromUriHttpsProtocol()
        {
            var exception = Assert.Throws<ArgumentException>(() => new CloudinaryDotNet.Cloudinary("https://123456789012345:ALKJdjklLJAjhkKJ45hBK92baj3@test?cloudinary=foo"));

            AssertCloudinaryUrlExceptionMessage(exception);
        }

        [Test]
        public void TestInitFromUriHttpsProtocolAsEnvVar()
        {
            Environment.SetEnvironmentVariable("CLOUDINARY_URL", "https://123456789012345:ALKJdjklLJAjhkKJ45hBK92baj3@test?cloudinary=foo");

            var exception = Assert.Throws<ArgumentException>(() => new CloudinaryDotNet.Cloudinary());

            AssertCloudinaryUrlExceptionMessage(exception);
        }

        private void AssertCloudinaryUrlExceptionMessage(ArgumentException exception)
        {
            Assert.That(exception.Message, Is.EqualTo("Invalid CLOUDINARY_URL scheme. Expecting to start with 'cloudinary://'"));
        }

        [Test]
        public void TestInitFromEnvironmentVariable()
        {
            Environment.SetEnvironmentVariable("CLOUDINARY_URL", "cloudinary://a:b@test123");
            var cloudinary = new CloudinaryDotNet.Cloudinary();
        }

        [Test]
        public void TestSecureDistributionFromUrl()
        {
            // should take secure distribution from url if secure=TRUE

            var cloudinary = new CloudinaryDotNet.Cloudinary("cloudinary://a:b@test123/config.secure.distribution.com");
            string url = cloudinary.Api.UrlImgUp.BuildUrl("test");

            Assert.AreEqual("https://config.secure.distribution.com/image/upload/test", url);
        }

        [Test]
        public void TestFolders()
        {
            // should add version if public_id contains /

            string result = m_api.UrlImgUp.BuildUrl(TestFolder);
            Assert.AreEqual(TestConstants.DefaultImageUpPath + $"{DefaultVersionStr}/{TestFolder}", result);
            result = m_api.UrlImgUp.Version(TestVersion).BuildUrl(TestFolder);
            Assert.AreEqual(TestConstants.DefaultImageUpPath + $"{TestVersionStr}/{TestFolder}", result);
            result = m_api.UrlImgUp.BuildUrl($"1/a{DefaultVersionStr}/{TestImageId}");
            Assert.AreEqual(TestConstants.DefaultImageUpPath + $"{DefaultVersionStr}/1/a{DefaultVersionStr}/{TestImageId}", result);
        }

        [Test]
        public void TestFoldersWithVersion()
        {
            // should not add version if public_id contains version already

            string result = m_api.UrlImgUp.BuildUrl($"{TestVersionStr}/{TestImageId}");
            Assert.AreEqual(TestConstants.DefaultImageUpPath + $"{TestVersionStr}/{TestImageId}", result);
        }

        [Test]
        public void TestForceVersion()
        {
            var api = new Api(m_api.Account);

            var result = api.UrlImgUp.BuildUrl(TestFolder);
            Assert.AreEqual($"{TestConstants.DefaultImageUpPath}{DefaultVersionStr}/{TestFolder}", result);

            // Should not add default version if ForceVersion is set to false
            result = api.UrlImgUp.ForceVersion(false).BuildUrl(TestFolder);
            Assert.AreEqual($"{TestConstants.DefaultImageUpPath}{TestFolder}", result);

            // Explicitly set version is always passed
            result = api.UrlImgUp.Version(TestVersion).ForceVersion(false).BuildUrl(TestFolder);
            Assert.AreEqual($"{TestConstants.DefaultImageUpPath}{TestVersionStr}/{TestFolder}", result);

            result = api.UrlImgUp.Version(TestVersion).ForceVersion(false).BuildUrl(TestImageId);
            Assert.AreEqual($"{TestConstants.DefaultImageUpPath}{TestVersionStr}/{TestImageId}", result);

            // Should use ForceVersion from Api instance
            api.ForceVersion = false;

            result = api.UrlImgUp.BuildUrl(TestFolder);
            Assert.AreEqual($"{TestConstants.DefaultImageUpPath}{TestFolder}", result);

            // Should override ForceVersion from Api instance
            result = api.UrlImgUp.ForceVersion().BuildUrl(TestFolder);
            Assert.AreEqual($"{TestConstants.DefaultImageUpPath}{DefaultVersionStr}/{TestFolder}", result);
        }

        [Test]
        public void TestShortenUrl()
        {
            // should allow to shorted image/upload urls

            string result = m_api.UrlImgUp.Shorten(true).BuildUrl("test");
            Assert.AreEqual(TestConstants.DefaultRootPath + "iu/test", result);
        }

        [Test]
        public void TestEscapePublicId()
        {
            // should escape public_ids

            var tests = new Dictionary<string, string>()
            {
                {"a b", "a%20b"},
                {"a+b", "a%2Bb"},
                {"a%20b", "a%20b"},
                {"a-b", "a-b"  },
                {"a??b", "a%3F%3Fb"}
            };

            foreach (var entry in tests)
            {
                string result = m_api.UrlImgUp.BuildUrl(entry.Key);
                Assert.AreEqual(TestConstants.DefaultImageUpPath + "" + entry.Value, result);
            }
        }

        [Test]
        public void TestSupportUseRootPathInSharedDistribution()
        {
            var actual = m_api.UrlImgUp.UseRootPath(true).PrivateCdn(false).BuildUrl("test");
            Assert.AreEqual(TestConstants.DefaultRootPath + "test", actual);
            actual = m_api.UrlImgUp.UseRootPath(true).PrivateCdn(false).Transform(new Transformation().Angle(0)).BuildUrl("test");
            Assert.AreEqual(TestConstants.DefaultRootPath + "a_0/test", actual);
        }

        [Test]
        public void TestSupportUseRootPathForPrivateCdn()
        {
            var actual = m_api.UrlImgUp.PrivateCdn(true).UseRootPath(true).BuildUrl("test");
            Assert.AreEqual("http://testcloud-res.cloudinary.com/test", actual);

            actual = m_api.UrlImgUp.PrivateCdn(true).Transform(new Transformation().Angle(0)).UseRootPath(true).BuildUrl("test");
            Assert.AreEqual("http://testcloud-res.cloudinary.com/a_0/test", actual);
        }

        [Test]
        public void TestEmptySource()
        {
            var url = m_api.UrlImgUp.BuildUrl();
            Assert.AreEqual(TestConstants.DefaultImageUpPath, url + "/");
        }

        [Test]
        public void TestCloudParams()
        {
            var @params = new StringDictionary("a", "b=c", "d=gggg===ggg====");
            Assert.AreEqual("a", @params.Pairs[0]);
            Assert.Null(@params["a"]);
            Assert.AreEqual("c", @params["b"]);
            Assert.AreEqual("gggg===ggg====", @params["d"]);
        }

        [Test]
        public void TestFetchLayerUrl()
        {
            var transformation = new Transformation().Overlay(new FetchLayer().Url("https://www.test.com/test/JE01118-YGP900_1_lar.jpg?version=432023"));
            var uri = m_api.UrlImgFetch.Transform(transformation).BuildUrl("http://image.com/files/8813/5551/7470/cruise-ship.png");
            Assert.AreEqual(TestConstants.DefaultImageFetchPath +
                            "l_fetch:aHR0cHM6Ly93d3cudGVzdC5jb20vdGVzdC9KRTAxMTE4LVlHUDkwMF8xX2xhci5qcGc_dmVyc2lvbj00MzIwMjM=" +
                            "/http://image.com/files/8813/5551/7470/cruise-ship.png", uri);
        }

        [Test]
        public void TestFetchLayerVideoUrl()
        {
            var transformation = new Transformation().Overlay(new FetchLayer().Url(FetchVideoUrl).ResourceType(Constants.RESOURCE_TYPE_VIDEO));
            var uri = m_api.UrlVideoUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(TestConstants.DefaultVideoUpPath + "l_video:fetch:" + FetchVideoUrlBase64Enc + "/test", uri);
        }

        [Test]
        public void TestExcludeEmptyTransformation()
        {
            var transformation = new Transformation().Chain().X(100).Y(100).Crop("fill").Chain();
            var uri = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(TestConstants.DefaultImageUpPath + "c_fill,x_100,y_100/test", uri);
        }

        [Test]
        public void TestAgentPlatformHeaders()
        {
            var request = new HttpRequestMessage { RequestUri = new Uri("http://dummy.com") };
            m_api.UserPlatform = "Test/1.0";

            m_api.PrepareRequestBody(
                request,
                HttpMethod.GET,
                new SortedDictionary<string, object>());

            //Can't test the result, so we just verify the UserAgent parameter is sent to the server
            StringAssert.AreEqualIgnoringCase($"{m_api.UserPlatform} {ApiShared.USER_AGENT}",
                request.Headers.UserAgent.ToString());
            StringAssert.IsMatch(@"Test\/1\.0 CloudinaryDotNet\/(\d+)\.(\d+)\.(\d+) \(.*\)",
                request.Headers.UserAgent.ToString());
        }

        [Test]
        public void TestDownloadArchiveUrl()
        {
            var cloudinary = new Cloudinary("cloudinary://a:b@test123");
            var parameters = new ArchiveParams().Tags(new List<string> { "some_tag" });

            var urlArchiveImage = cloudinary.DownloadArchiveUrl(parameters);
            var decodedImageUrl = Uri.UnescapeDataString(urlArchiveImage);
            var rgImage = Regex.IsMatch(decodedImageUrl, @"https://api\.cloudinary\.com/v1_1/[^/]*/image/generate_archive\?api_key=a&mode=download&signature=\w{40}&tags\[]=some_tag&timestamp=\d{10}");
            Assert.True(rgImage);

            parameters.ResourceType("video");

            var urlArchiveVideo = cloudinary.DownloadArchiveUrl(parameters);
            var decodedVideoUrl = Uri.UnescapeDataString(urlArchiveVideo);
            var regVideo = Regex.IsMatch(decodedVideoUrl, @"https://api\.cloudinary\.com/v1_1/[^/]*/video/generate_archive\?api_key=a&mode=download&signature=\w{40}&tags\[]=some_tag&timestamp=\d{10}");
            Assert.True(regVideo);
        }

        [Test]
        public void TestDownloadArchiveUrlShouldSupportTargetPublicId()
        {
            var cloudinary = new Cloudinary("cloudinary://a:b@test123");
            var parameters = new ArchiveParams().Tags(new List<string> { "some_tag" }).TargetPublicId(TestImageId);

            var urlArchiveImage = cloudinary.DownloadArchiveUrl(parameters);
            var decodedImageUrl = Uri.UnescapeDataString(urlArchiveImage);
            var rgImage = Regex.IsMatch(decodedImageUrl, @"https://api\.cloudinary\.com/v1_1/[^/]*/image/generate_archive\?api_key=a&mode=download&signature=\w{40}&tags\[]=some_tag&target_public_id=" + TestImageId + @"&timestamp=\d{10}");
            Assert.True(rgImage);
        }

        [Test]
        public void TestDownloadPrivate()
        {
            var cloudinary = new Cloudinary("cloudinary://a:b@test123");
            var expiresAt = Utils.UnixTimeNowSeconds() + 7200;
            const string testPublicId = "zihltjwsyczm700kqj1z";

            var urlPrivateImage = cloudinary.DownloadPrivate(testPublicId, expiresAt: expiresAt);
            var rgImage = Regex.IsMatch(urlPrivateImage, @"https://api\.cloudinary\.com/v1_1/[^/]*/image/download\?api_key=a&expires_at=" + expiresAt + @"&public_id=zihltjwsyczm700kqj1z&signature=\w{40}&timestamp=\d{10}");
            Assert.True(rgImage);

            var urlPrivateVideo = cloudinary.DownloadPrivate(testPublicId, expiresAt: expiresAt, resourceType: "video");
            var rgVideo = Regex.IsMatch(urlPrivateVideo, @"https://api\.cloudinary\.com/v1_1/[^/]*/video/download\?api_key=a&expires_at=" + expiresAt + @"&public_id=zihltjwsyczm700kqj1z&signature=\w{40}&timestamp=\d{10}");
            Assert.True(rgVideo);
        }

        [Test]
        public void TestDownloadZip()
        {
            var cloudinary = new Cloudinary("cloudinary://a:b@test123");
            const string testTag = "api_test_custom1";

            var urlZipImage = cloudinary.DownloadZip(testTag, null);
            var rgImage = Regex.IsMatch(urlZipImage, @"https://api\.cloudinary\.com/v1_1/[^/]*/image/download_tag\.zip\?api_key=a&signature=\w{40}&tag=api_test_custom1&timestamp=\d{10}");
            Assert.True(rgImage);

            var urlZipVideo = cloudinary.DownloadZip(testTag, null, "video");
            var rgVideo = Regex.IsMatch(urlZipVideo, @"https://api\.cloudinary\.com/v1_1/[^/]*/video/download_tag\.zip\?api_key=a&signature=\w{40}&tag=api_test_custom1&timestamp=\d{10}");
            Assert.True(rgVideo);
        }

        [Test]
        public void TestEscapeApiUrl()
        {
            const string folderName = "sub^folder test";

            var url = m_api.ApiUrlV.Add("folders").Add(folderName).BuildUrl();

            Assert.IsTrue(url.EndsWith("/folders/sub%5Efolder%20test"));
        }

        [Test]
        public void TestApiUrlWithPrivateCdn()
        {
            var cloudinary = new Cloudinary("cloudinary://a:b@test123/test123-res.cloudinary.com?cname=mycname.com");

            const string testTag = "api_test_custom1";
            var urlZipImage = cloudinary.DownloadZip(testTag, null);

            StringAssert.StartsWith("https://api.cloudinary.com", urlZipImage);
        }

        [Test]
        public void TestTextLayerStyleIdentifierVariables()
        {
            string buildUrl(Func<TextLayer, TextLayer> setTextStyleAction) =>
                m_api.UrlImgUp.Transform(
                    new Transformation()
                        .Variable("$style", "!Arial_12!")
                        .Chain()
                        .Overlay(
                            setTextStyleAction(new TextLayer().Text("hello-world"))
                        )
                ).BuildUrl("sample");

            var expected =
                "http://res.cloudinary.com/testcloud/image/upload/$style_!Arial_12!/l_text:$style:hello-world/sample";

            Assert.AreEqual(expected, buildUrl(l => l.TextStyle("$style")));
            Assert.AreEqual(expected, buildUrl(l => l.TextStyle(new Expression("$style"))));
        }
    }
}
