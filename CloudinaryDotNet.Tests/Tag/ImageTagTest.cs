using System.Collections.Generic;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests.Tag
{
    [TestFixture]
    public class ImageTagTest
    {
        protected Api m_api;

        [OneTimeSetUp]
        public void Init()
        {
            Account account = new Account(TestConstants.CloudName, TestConstants.DefaultApiKey,
                TestConstants.DefaultApiSecret);
            m_api = new Api(account);
        }

        [Test]
        public void TestImageUploadTag()
        {
            var htmlOptions = new Dictionary<string, string>();
            htmlOptions.Add("htmlattr", "htmlvalue");

            var result = m_api.BuildUploadForm("test-field", "auto", null, htmlOptions).ToString();

            Assert.IsTrue(result.Contains("type='file'"));
            Assert.IsTrue(result.Contains("data-cloudinary-field='test-field'"));
            Assert.IsTrue(result.Contains("class='cloudinary-fileupload'"));
            Assert.IsTrue(result.Contains("htmlattr='htmlvalue'"));

            htmlOptions.Clear();
            htmlOptions.Add("class", "myclass");

            result = m_api.BuildUploadForm("test-field", "auto", null, htmlOptions).ToString();

            Assert.IsTrue(result.Contains("class='cloudinary-fileupload myclass'"));
        }

        [Test]
        public void TestImageDirectUploadTags()
        {
            var htmlOptions = new Dictionary<string, string> ();
            var parameters = new SortedDictionary<string, object>
            {
                {"tags", new List<string> {"user_218", "screencast"}}
            };

            var result = m_api.BuildUploadForm("test-field", "auto", parameters, htmlOptions);

            Assert.IsTrue(result.Contains(@"""tags"":""user_218|screencast"""));
        }

        [Test]
        public void TestImageDirectUploadTransformations()
        {
            var htmlOptions = new Dictionary<string, string>();
            var transformation = new EagerTransformation().SetFormat("m3u8").RawTransformation("sp_full_hd");
            var parameters = new SortedDictionary<string, object>
            {
                { "eager", transformation}
            };

            var result = m_api.BuildUploadForm("test-field", "auto", parameters, htmlOptions);

            Assert.IsTrue(result.Contains(@"""eager"":""sp_full_hd/m3u8"""));
        }

        [Test]
        public void TestImageTag()
        {
            var transformation = new Transformation().Width(100).Height(101).Crop("crop");

            var dict = new StringDictionary();
            dict["alt"] = "my image";

            var result = m_api.UrlImgUp.Transform(transformation).BuildImageTag("test", dict).ToString();
            Assert.AreEqual("<img src=\"http://res.cloudinary.com/testcloud/image/upload/c_crop,h_101,w_100/test\" alt=\"my image\" width=\"100\" height=\"101\"/>", result);
        }

        [Test]
        public void TestResponsiveWidth()
        {
            var transformation = new Transformation().Width(0.9).Height(0.9).Crop("crop").ResponsiveWidth(true);
            var result = m_api.UrlImgUp.Transform(transformation).BuildImageTag("test", new StringDictionary("alt=my image"));
            Assert.AreEqual(
                    "<img alt=\"my image\" class=\"cld-responsive\" data-src=\"http://res.cloudinary.com/testcloud/image/upload/c_crop,h_0.9,w_0.9/c_limit,w_auto/test\"/>",
                    result.ToString());
            result = m_api.UrlImgUp.Transform(transformation).BuildImageTag("test", new StringDictionary("alt=my image", "class=extra"));
            Assert.AreEqual(
                    "<img alt=\"my image\" class=\"extra cld-responsive\" data-src=\"http://res.cloudinary.com/testcloud/image/upload/c_crop,h_0.9,w_0.9/c_limit,w_auto/test\"/>",
                    result.ToString());
            transformation = new Transformation().Width("auto").Crop("crop");
            result = m_api.UrlImgUp.Transform(transformation).BuildImageTag("test", new StringDictionary("alt=my image", "responsive_placeholder=blank"));
            Assert.AreEqual(
                    "<img src=\"data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7\" alt=\"my image\" class=\"cld-responsive\" data-src=\"http://res.cloudinary.com/testcloud/image/upload/c_crop,w_auto/test\"/>",
                    result.ToString());
            result = m_api.UrlImgUp.Transform(transformation).BuildImageTag("test", new StringDictionary("alt=my image", "responsive_placeholder=other.gif"));
            Assert.AreEqual(
                    "<img src=\"other.gif\" alt=\"my image\" class=\"cld-responsive\" data-src=\"http://res.cloudinary.com/testcloud/image/upload/c_crop,w_auto/test\"/>",
                    result.ToString());
        }

        [Test]
        public void TestImageTagAutoDpr()
        {
            var transform = new Transformation().Width(100).Height(101).Crop("crop").Dpr("auto");

            var result = m_api.UrlImgUp.Transform(transform).BuildImageTag("test").ToString();

            Assert.True(transform.HiDpi);
            Assert.AreEqual("<img width=\"100\" height=\"101\" class=\"cld-hidpi\" data-src=\"http://res.cloudinary.com/testcloud/image/upload/c_crop,dpr_auto,h_101,w_100/test\"/>", result);
        }

        [Test]
        public void TestImageTagNoParams()
        {
            Transformation transformation = new Transformation().Width(100).Height(101).Crop("crop");

            var result = m_api.UrlImgUp.Transform(transformation).BuildImageTag("test").ToString();
            Assert.AreEqual("<img src=\"http://res.cloudinary.com/testcloud/image/upload/c_crop,h_101,w_100/test\" width=\"100\" height=\"101\"/>", result);
        }

        [Test]
        public void TestImageTagParams()
        {
            Transformation transformation = new Transformation().Width(100).Height(101).Crop("crop");

            var result = m_api.UrlImgUp.Transform(transformation).BuildImageTag("test", "alt=my image name \"TestImg\"", "test=My image name is 'Test2'").ToString();
            Assert.AreEqual("<img src=\"http://res.cloudinary.com/testcloud/image/upload/c_crop,h_101,w_100/test\" alt=\"my image name &quot;TestImg&quot;\" test=\"My image name is &#39;Test2&#39;\" width=\"100\" height=\"101\"/>", result);
        }

        [Test]
        public void TestDifferentHtmlDimensions()
        {
            var trans = new Transformation().Width(100).Height(101).SetHtmlWidth(50).SetHtmlHeight(51);
            var tag = m_api.UrlImgUp.Transform(trans).BuildImageTag("test").ToString();
            Assert.AreEqual("<img src=\"http://res.cloudinary.com/testcloud/image/upload/h_101,w_100/test\" width=\"50\" height=\"51\"/>", tag);
        }
    }
}
