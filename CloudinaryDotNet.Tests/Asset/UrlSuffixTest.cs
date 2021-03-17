using System;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace CloudinaryDotNet.Test.Asset
{
    [TestFixture]
    public class UrlSuffixTest
    {
        protected Api m_api;

        [OneTimeSetUp]
        public void Init()
        {
            var account = new Account(TestConstants.CloudName, TestConstants.DefaultApiKey, 
                TestConstants.DefaultApiSecret);
            m_api = new Api(account);
        }

        [Test]
        public void TestSupportUseRootPathTogetherWithUrlSuffixForPrivateCdn()
        {
            var actual = m_api.UrlImgUp.PrivateCdn(true).Suffix("hello").UseRootPath(true).BuildUrl("test");
            Assert.AreEqual("http://testcloud-res.cloudinary.com/test/hello", actual);
        }

        [Test]
        public void TestDisallowUseRootPathIfNotImageUploadForFacebook()
        {
            Assert.That(() => m_api.UrlImgUp.UseRootPath(true).PrivateCdn(true).Action("facebook").BuildUrl("test"), Throws.TypeOf<NotSupportedException>(), "Root path only supported for image/upload!");
        }

        [Test]
        public void TestDisallowUseRootPathIfNotImageUploadForRaw()
        {
            Assert.That(() => m_api.UrlImgUp.UseRootPath(true).PrivateCdn(true).ResourceType("raw").BuildUrl("test"), Throws.TypeOf<NotSupportedException>(), "Root path only supported for image/upload!");
        }

        [Test]
        public void TestDisallowUrlSuffixInNonUploadTypes()
        {
            Assert.That(() => m_api.UrlImgUp.Suffix("hello").PrivateCdn(true).Action("facebook").BuildUrl("test"), Throws.TypeOf<NotSupportedException>(), "URL Suffix only supported for image/upload and raw/upload!");
        }

        [Test]
        public void TestDisallowUrlSuffixWithSlash()
        {
            Assert.That(() => m_api.UrlImgUp.Suffix("hello/world").PrivateCdn(true).BuildUrl("test"), Throws.TypeOf<ArgumentException>(), "Suffix should not include . or /!");
        }

        [Test]
        public void TestDisallowUrlSuffixWithDot()
        {
            Assert.That(() => m_api.UrlImgUp.Suffix("hello.world").PrivateCdn(true).BuildUrl("test"), Throws.TypeOf<ArgumentException>(), "Suffix should not include . or /!");
        }

        [Test]
        public void TestSupportUrlSuffixForPrivateCdn()
        {
            string actual = m_api.UrlImgUp.Suffix("hello").PrivateCdn(true).BuildUrl("test");
            Assert.AreEqual("http://testcloud-res.cloudinary.com/images/test/hello", actual);

            actual = m_api.UrlImgUp.Suffix("hello").PrivateCdn(true).Transform(new Transformation().Angle(0)).BuildUrl("test");
            Assert.AreEqual("http://testcloud-res.cloudinary.com/images/a_0/test/hello", actual);
        }

        [Test]
        public void TestPutFormatAfterUrlSuffix()
        {
            string actual = m_api.UrlImgUp.Suffix("hello").PrivateCdn(true).Format("jpg").BuildUrl("test");
            Assert.AreEqual("http://testcloud-res.cloudinary.com/images/test/hello.jpg", actual);
        }

        [Test]
        public void TestNotSignTheUrlSuffix()
        {
            var r = new Regex("s--[0-9A-Za-z_-]{8}--", RegexOptions.Compiled);

            string url = m_api.UrlImgUp.Format("jpg").Signed(true).BuildUrl("test");
            var match = r.Match(url);

            Assert.IsTrue(match.Success);

            string actual = m_api.UrlImgUp.Format("jpg").PrivateCdn(true).Signed(true).Suffix("hello").BuildUrl("test");
            Assert.AreEqual("http://testcloud-res.cloudinary.com/images/" + match.Value + "/test/hello.jpg", actual);

            url = m_api.UrlImgUp.Format("jpg").Signed(true).Transform(new Transformation().Angle(0)).BuildUrl("test");
            match = r.Match(url);

            Assert.IsTrue(match.Success);

            actual = m_api.UrlImgUp.Format("jpg").PrivateCdn(true).Signed(true).Suffix("hello").Transform(new Transformation().Angle(0)).BuildUrl("test");

            Assert.AreEqual("http://testcloud-res.cloudinary.com/images/" + match.Value + "/a_0/test/hello.jpg", actual);
        }

        [Test]
        public void TestSupportUrlSuffixForRawUploads()
        {
            string actual = m_api.UrlImgUp.Suffix("hello").PrivateCdn(true).ResourceType("raw").BuildUrl("test");
            Assert.AreEqual("http://testcloud-res.cloudinary.com/files/test/hello", actual);
        }

        [Test]
        public void TestSupportUrlSuffixForSharedCdn()
        {
            string actual = m_api.UrlImgUp.Suffix("hello").Signed(true).Format("jpg").BuildUrl("test");
            Assert.AreEqual("http://res.cloudinary.com/testcloud/images/s--1TMilNWq--/test/hello.jpg", actual);
        }

        [Test]
        public void TestSupportUrlSuffixForPrivateImages()
        {
            Url url = new Url("testcloud").ResourceType("image").Action("private").Suffix("suffix");
            var urlStr = url.BuildUrl();
            StringAssert.Contains("/private_images/", urlStr);
            StringAssert.EndsWith("suffix", urlStr);
        }

        [Test]
        public void TestSupportUrlSuffixForAuthenticatedImages()
        {
            Url url = new Url("testcloud").ResourceType("image").Action("authenticated").Suffix("suffix");
            var urlStr = url.BuildUrl();
            StringAssert.Contains("/authenticated_images/", urlStr);
            StringAssert.EndsWith("suffix", urlStr);
        }
    }
}
