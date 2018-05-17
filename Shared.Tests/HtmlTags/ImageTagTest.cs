using NUnit.Framework;
using CloudinaryDotNet;
using CloudinaryDotNet.HtmlTags;


namespace CloudinaryDotNet.Test
{
    [TestFixture]
    class ImageTagTest : IntegrationTestBase
    {
        private Url m_url;
        private string m_defaultRootPath;
        private string m_defaultImgUpPath;

        [SetUp]
        public void SetUp()
        {
            m_url = new Url(m_cloudName)
                .ResourceType(Constants.RESOURCE_TYPE_IMAGE)
                .Action(Constants.ACTION_NAME_UPLOAD);
            m_defaultRootPath = $"http://res.cloudinary.com/{m_cloudName}";
            m_defaultImgUpPath = $"{m_defaultRootPath}/image/upload";
        }

        [Test]
        public void SimpleImageTag()
        {
            var tag = new ImageTag(TEST_IMAGE);
            Assert.AreEqual($"<img src=\"{TEST_IMAGE}\">", tag.ToString());
        }

        [Test]
        public void ImageTagFromCloudinaryUrl()
        {
            var cloudinaryUrl = m_url.Source(TEST_IMAGE);
            var tag = new ImageTag(cloudinaryUrl).ToString();
            Assert.AreEqual($"<img src=\"{cloudinaryUrl.BuildUrl()}\">", tag);

            cloudinaryUrl.Transform(m_resizeTransformation);
            tag = new ImageTag(cloudinaryUrl).ToString();
            Assert.AreEqual(
                $"<img src=\"{cloudinaryUrl.BuildUrl()}\" width=\"{RESIZE_TRANSFORM_W}\" " +
                $"height=\"{RESIZE_TRANSFORM_H}\">",
                tag
            );

        }

        [Test]
        public void TestDifferentHtmlDimensions()
        {
            var htmlWidth = 50;
            var htmlHeight = 51;

            var trans = m_resizeTransformation.SetHtmlWidth(htmlWidth).SetHtmlHeight(htmlHeight);
            var tag = new ImageTag(m_url.Transform(trans).Source(TEST_IMAGE)).ToString();

            Assert.AreEqual(
                $"<img src=\"{m_defaultImgUpPath}/{m_resizeTransformationAsString}/{TEST_IMAGE}\" " +
                $"width=\"{htmlWidth}\" height=\"{htmlHeight}\">",
                tag
            );
        }

        [Test]
        public void TestImageTag()
        {
            var dict = new StringDictionary
            {
                ["alt"] = "my image"
            };

            var result = new ImageTag(m_url.Transform(m_cropTransformation).Source(TEST_IMAGE)).Attrs(dict).ToString();
            Assert.AreEqual(
                $"<img src=\"{m_defaultImgUpPath}/{m_cropTransformationAsString}/{TEST_IMAGE}\" " +
                $"alt=\"my image\" width=\"{RESIZE_TRANSFORM_W}\" height=\"{RESIZE_TRANSFORM_H}\">",
                result
            );
        }

        [Test]
        public void TestResponsiveWidth()
        {
            var customAttributes = new StringDictionary("alt=my image");
            var transformation = m_cropTransformation.ResponsiveWidth(true);
            var result = new ImageTag(m_url.Transform(transformation).Source(TEST_IMAGE)).Attrs(customAttributes)
                .ToString();

            Assert.AreEqual(
                $"<img alt=\"my image\" class=\"cld-responsive\" " +
                $"data-src=\"{m_defaultImgUpPath}/{m_cropTransformationAsString}/c_limit,w_auto/{TEST_IMAGE}\">",
                result
            );

            result = new ImageTag(m_url.Transform(transformation).Source(TEST_IMAGE))
                .Attrs(new StringDictionary("alt=my image", "class=extra"))
                .ToString();

            Assert.AreEqual(
                $"<img alt=\"my image\" class=\"extra cld-responsive\" " +
                $"data-src=\"{m_defaultImgUpPath}/{m_cropTransformationAsString}/c_limit,w_auto/{TEST_IMAGE}\">",
                result
            );

            transformation = new Transformation().Width("auto").Crop("crop");
            result = new ImageTag(m_url.Transform(transformation).Source(TEST_IMAGE))
                .Attrs(customAttributes)
                .Option("responsive_placeholder", "blank")
                .ToString();

            Assert.AreEqual(
                    $"<img src=\"data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7\" " +
                    $"alt=\"my image\" class=\"cld-responsive\" " +
                    $"data-src=\"{m_defaultImgUpPath}/c_crop,w_auto/{TEST_IMAGE}\">",
                    result);

            result = new ImageTag(m_url.Transform(transformation).Source(TEST_IMAGE))
                .Attrs(customAttributes)
                .Option("responsive_placeholder", "other.gif")
                .ToString();

            Assert.AreEqual(
                    $"<img src=\"other.gif\" alt=\"my image\" class=\"cld-responsive\" " +
                    $"data-src=\"{m_defaultImgUpPath}/c_crop,w_auto/{TEST_IMAGE}\">",
                    result);
        }

        [Test]
        public void TestImageTagAutoDpr()
        {
            var transform = m_cropTransformation.Dpr("auto");
            var transformStr = $"c_crop,dpr_auto,h_{RESIZE_TRANSFORM_H}, w_{RESIZE_TRANSFORM_W}";

            var result = new ImageTag(m_url.Transform(transform).Source(TEST_IMAGE)).ToString();

            Assert.True(transform.HiDpi);
            Assert.AreEqual(
                $"<img width=\"{RESIZE_TRANSFORM_W}\" height=\"{RESIZE_TRANSFORM_H}\" class=\"cld-hidpi\" " +
                $"data-src=\"{m_defaultImgUpPath}/{transformStr}/{TEST_IMAGE}\">",
                result
            );
        }

        [Test]
        public void TestImageTagNoParams()
        {
            var result = new ImageTag(m_url.Transform(m_cropTransformation).Source(TEST_IMAGE)).ToString();

            Assert.AreEqual(
                $"<img src=\"{m_defaultImgUpPath}/{m_cropTransformationAsString}/{TEST_IMAGE} " +
                $"\"width=\"{RESIZE_TRANSFORM_W}\" height=\"{RESIZE_TRANSFORM_H}\">",
                result
            );
        }

        [Test]
        public void TestImageTagParams()
        {
            var result = new ImageTag(m_url.Transform(m_cropTransformation).Source(TEST_IMAGE))
                .Attrs(new StringDictionary("alt=my image name \"TestImg\"", "test=My image name is 'Test2'"))
                .ToString();

            Assert.AreEqual(
                $"<img src=\"{m_defaultImgUpPath}/{m_cropTransformationAsString}/{TEST_IMAGE}\" " +
                $"alt=\"my image name &quot;TestImg&quot;\" test=\"My image name is &#39;Test2&#39;\" " +
                $"width=\"{RESIZE_TRANSFORM_W}\" height=\"{RESIZE_TRANSFORM_H}\">",
                result
            );
        }
    }
}
