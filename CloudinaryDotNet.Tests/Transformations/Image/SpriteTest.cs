using NUnit.Framework;

namespace CloudinaryDotNet.Tests.Transformations.Image
{
    [TestFixture]
    public class SpriteTest
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
        public void TestSpriteCss()
        {
            var result = m_api.UrlImgUp.BuildSpriteCss("test");
            Assert.AreEqual(TestConstants.DefaultRootPath + "image/sprite/test.css", result);
            result = m_api.UrlImgUp.BuildSpriteCss("test.css");
            Assert.AreEqual(TestConstants.DefaultRootPath + "image/sprite/test.css", result);
        }

        [Test]
        public void TestSprite()
        {
            // should build urls to get sprite css and picture by tag (with transformations and prefix)

            var uri = m_api.UrlImgUp.Type("sprite").BuildUrl("teslistresourcesbytag1.png");
            Assert.AreEqual(TestConstants.DefaultRootPath + "image/sprite/teslistresourcesbytag1.png", uri);

            uri = m_api.UrlImgUp.Type("sprite").BuildUrl("teslistresourcesbytag1.css");
            Assert.AreEqual(TestConstants.DefaultRootPath + "image/sprite/teslistresourcesbytag1.css", uri);

            uri = m_api.ApiUrlImgUpV.CloudinaryAddr("http://api.cloudinary.com").Action("sprite").BuildUrl();
            Assert.AreEqual("http://api.cloudinary.com/v1_1/testcloud/image/sprite", uri);
        }

        [Test]
        public void TestSpriteCssPrefix()
        {
            // should build urls to get sprite css and picture by tag with prefix

            string uri = m_api.UrlImgUp.Type("sprite").Add("p_home_thing_").BuildUrl("logo.css");
            Assert.AreEqual(TestConstants.DefaultRootPath + "image/sprite/p_home_thing_/logo.css", uri);
        }

        [Test]
        public void TestSpriteTransform()
        {
            // should build urls to get sprite css and picture by tag with transformations

            Transformation t = new Transformation().Crop("fit").Height(60).Width(150);
            string uri = m_api.UrlImgUp.Action("sprite").Transform(t).BuildUrl("logo.png");
            Assert.AreEqual(TestConstants.DefaultRootPath + "image/sprite/c_fit,h_60,w_150/logo.png", uri);
        }
    }
}
