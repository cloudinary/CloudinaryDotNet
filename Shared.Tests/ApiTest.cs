using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.Test
{
    [TestFixture]
    public partial class ApiTest
    {
        protected Api m_api;
        protected string m_defaultRootPath;
        protected string m_defaultImgUpPath;
        protected string m_defaultVideoUpPath;
        protected string m_defaultImgFetchPath;

        [OneTimeSetUp]
        public void Init()
        {
            Account account = new Account("testcloud", "1234", "abcd");
            m_api = new Api(account);
            m_defaultRootPath = "http://res.cloudinary.com/testcloud/";
            m_defaultImgUpPath = m_defaultRootPath + "image/upload/";
            m_defaultVideoUpPath = m_defaultRootPath + "video/upload/";
            m_defaultImgFetchPath = m_defaultRootPath + "image/fetch/";
        }

        [Test]
        public void TestSign()
        {
            SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();

            parameters.Add("public_id", "sample");
            parameters.Add("timestamp", "1315060510");

            Assert.AreEqual("c3470533147774275dd37996cc4d0e68fd03cd4f", m_api.SignParameters(parameters));
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
            Assert.AreEqual(m_defaultImgUpPath + "test", uri);
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
            Assert.AreEqual(m_defaultImgUpPath + "test.jpg", uri);
        }

        [Test]
        public void TestOpacity()
        {
            var trans = new Transformation().Overlay("overlay.png").Opacity(40);
            var uri = m_api.UrlImgUp.Transform(trans).BuildUrl("test.jpg");
            Assert.AreEqual(m_defaultImgUpPath + "l_overlay.png,o_40/test.jpg", uri);
        }

        [Test]
        public void TestCrop()
        {
            Transformation transformation = new Transformation().Width(100).Height(101);
            string uri = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");

            Assert.AreEqual(m_defaultImgUpPath + "h_101,w_100/test", uri);
            Assert.AreEqual("101", transformation.HtmlHeight);
            Assert.AreEqual("100", transformation.HtmlWidth);

            transformation = new Transformation().Width(100).Height(101).Crop("crop");
            uri = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");

            Assert.AreEqual(m_defaultImgUpPath + "c_crop,h_101,w_100/test", uri);
        }

        [Test]
        public void TestDifferentHtmlDimensions()
        {
            var trans = new Transformation().Width(100).Height(101).SetHtmlWidth(50).SetHtmlHeight(51);
            var tag = m_api.UrlImgUp.Transform(trans).BuildImageTag("test").ToString();
            Assert.AreEqual("<img src=\"http://res.cloudinary.com/testcloud/image/upload/h_101,w_100/test\" width=\"50\" height=\"51\"/>", tag);
        }

        [Test]
        public void TestTransformations()
        {
            // should use x, y, radius, prefix, gravity and quality from options

            Transformation transformation = new Transformation().X(1).Y(2).Radius(3).Gravity("center").Quality(0.4).Prefix("a");
            string uri = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "g_center,p_a,q_0.4,r_3,x_1,y_2/test", uri);
        }

        [Test]
        public void TestSimpleTransformation()
        {
            // should support named transformation

            Transformation transformation = new Transformation().Named("blip");
            string uri = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "t_blip/test", uri);
        }

        [Test]
        public void TestTransformationArray()
        {
            // should support array of named transformations

            Transformation transformation = new Transformation().Named("blip", "blop");
            string uri = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "t_blip.blop/test", uri);
        }

        [Test]
        public void TestBaseTransformationChain()
        {
            // should support base transformation

            Transformation transformation = new Transformation().X(100).Y(100).Crop("fill").Chain().Crop("crop").Width(100);
            string uri = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual("100", transformation.HtmlWidth);
            Assert.AreEqual(m_defaultImgUpPath + "c_fill,x_100,y_100/c_crop,w_100/test", uri);
        }

        [Test]
        public void TestEagerTransformationList()
        {
            List<Transformation> list = new List<Transformation>(){
                new EagerTransformation().SetFormat("jpg").Crop("scale").Width(2.0),
                new EagerTransformation(new Transformation().Width(10),new Transformation().Angle(10)),
                new Transformation().Width(20).Height(20)
            };

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                EagerTransforms = list
            };

            SortedDictionary<string, object> dict = uploadParams.ToParamsDictionary();

            Assert.AreEqual("c_scale,w_2.0/jpg|w_10/a_10|h_20,w_20", dict["eager"]);
        }

        [Test]
        public void TestBaseTransformationArray()
        {
            // should support array of base transformations

            Transformation transformation = new Transformation().X(100).Y(100).Width(200).Crop("fill").Chain().Radius(10).Chain().Crop("crop").Width(100);
            string uri = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual("100", transformation.HtmlWidth);
            Assert.AreEqual(m_defaultImgUpPath + "c_fill,w_200,x_100,y_100/r_10/c_crop,w_100/test", uri);
        }

        [Test]
        public void TestTransformationAutoWidth()
        {
            // should support transformations with width:auto and width:auto_breakpoints

            Transformation transformation = new Transformation().Width("auto:20").Crop("fill");
            string uri = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "c_fill,w_auto:20/test", uri);

            transformation = new Transformation().Width("auto:20:350").Crop("fill");
            uri = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "c_fill,w_auto:20:350/test", uri);

            transformation = new Transformation().Width("auto:breakpoints").Crop("fill");
            uri = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "c_fill,w_auto:breakpoints/test", uri);

            transformation = new Transformation().Width("auto:breakpoints_100_1900_20_15").Crop("fill");
            uri = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "c_fill,w_auto:breakpoints_100_1900_20_15/test", uri);

            transformation = new Transformation().Width("auto:breakpoints:json").Crop("fill");
            uri = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "c_fill,w_auto:breakpoints:json/test", uri);
        }

        [Test]
        public void TestExcludeEmptyTransformation()
        {
            Transformation transformation = new Transformation().Chain().X(100).Y(100).Crop("fill").Chain();
            string uri = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "c_fill,x_100,y_100/test", uri);
        }

        [Test]
        public void TestAction()
        {
            // should use type of action from options

            string uri = m_api.UrlImgUp.Action("facebook").BuildUrl("test");
            Assert.AreEqual(m_defaultRootPath + "image/facebook/test", uri);
        }

        [Test]
        public void TestResourceType()
        {
            // should use resource_type from options

            string uri = m_api.Url.ResourceType("raw").Action("upload").BuildUrl("test");
            Assert.AreEqual(m_defaultRootPath + "raw/upload/test", uri);
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
            Assert.AreEqual(m_defaultRootPath + "image/fetch/http://test", uri);
        }

        [Test]
        public void TestFetch()
        {
            // should escape fetch urls

            string uri = m_api.Url.ResourceType("image").Action("fetch").BuildUrl("http://blah.com/hello?a=b");
            Assert.AreEqual(m_defaultRootPath + "image/fetch/http://blah.com/hello%3Fa%3Db", uri);
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
            Assert.AreEqual(m_defaultRootPath + "image/youtube/http://www.youtube.com/watch%3Fv%3Dd9NF2edxy-M", uri);
        }

        [Test]
        public void TestBackground()
        {
            // should support background
            Transformation transformation = new Transformation().Background("red");
            String result = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "b_red/test", result);
            transformation = new Transformation().Background("#112233");
            result = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "b_rgb:112233/test", result);
        }

        [Test]
        public void TestDefaultImage()
        {
            // should support default_image
            Transformation transformation = new Transformation().DefaultImage("default");
            String result = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "d_default/test", result);
        }

        [Test]
        public void TestAngle()
        {
            // should support angle
            Transformation transformation = new Transformation().Angle(12);
            String result = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "a_12/test", result);
            transformation = new Transformation().Angle("exif", "12");
            result = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "a_exif.12/test", result);
        }

        [Test]
        public void TestOverlay()
        {
            // should support overlay
            Transformation transformation = new Transformation().Overlay("text:hello");
            String result = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "l_text:hello/test", result);
            // should not pass width/height to html if overlay
            transformation = new Transformation().Overlay("text:hello").Width(100).Height(100);
            result = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.IsNull(transformation.HtmlHeight);
            Assert.IsNull(transformation.HtmlWidth);
            Assert.AreEqual(m_defaultImgUpPath + "h_100,l_text:hello,w_100/test", result);
        }

        [Test]
        public void TestOverlayHelpers()
        {
            Dictionary<BaseLayer, string> layerTests = new Dictionary<BaseLayer, string>();
            layerTests.Add(new Layer().PublicId("logo"), "logo");
            layerTests.Add(new Layer().PublicId("folder/logo"), "folder:logo");
            layerTests.Add(new Layer().PublicId("logo").Type("private"), "private:logo");
            layerTests.Add(new Layer().PublicId("logo").Format("png"), "logo.png");
            layerTests.Add(new Layer().ResourceType("video").PublicId("cat"), "video:cat");
            layerTests.Add(new TextLayer().Text("Hello World, Nice to meet you?")
                                              .FontFamily("Arial")
                                              .FontSize(18), "text:Arial_18:Hello%20World%252C%20Nice%20to%20meet%20you%3F");
            layerTests.Add(new TextLayer("Hello World, Nice to meet you?")
                                              .FontFamily("Arial")
                                              .FontSize(18)
                                              .FontWeight("bold")
                                              .FontStyle("italic")
                                              .LetterSpacing("4")
                                              .LineSpacing("3"), "text:Arial_18_bold_italic_letter_spacing_4_line_spacing_3:Hello%20World%252C%20Nice%20to%20meet%20you%3F");
            layerTests.Add(new SubtitlesLayer().PublicId("sample_sub_en.srt"), "subtitles:sample_sub_en.srt");
            layerTests.Add(new SubtitlesLayer().PublicId("sample_sub_he.srt").FontFamily("Arial").FontSize(40), "subtitles:Arial_40:sample_sub_he.srt");

            foreach (var layerTest in layerTests)
            {
                string expected = layerTest.Value;
                string actual = layerTest.Key.ToString();
                Assert.IsTrue(expected.Equals(actual, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        [Test]
        public void TestMultipleLayers()
        {
            
            Transformation t = new Transformation()
                .Overlay("One").Chain()
                .Overlay("Two").Chain()
                .Overlay("Three").Chain()
                .Overlay("One").Chain()
                .Overlay("Two").Chain();
            var actual = m_api.UrlImgUp.Transform(t).BuildUrl("sample.jpg");
            Assert.AreEqual(m_defaultImgUpPath + "l_One/l_Two/l_Three/l_One/l_Two/sample.jpg", actual);
            
        }

        [Test]
        public void TestUnderlay()
        {
            Transformation transformation = new Transformation().Underlay("text:hello");
            String result = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "u_text:hello/test", result);
            // should not pass width/height to html if underlay
            transformation = new Transformation().Underlay("text:hello").Width(100).Height(100);
            result = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.IsNull(transformation.HtmlHeight);
            Assert.IsNull(transformation.HtmlWidth);
            Assert.AreEqual(m_defaultImgUpPath + "h_100,u_text:hello,w_100/test", result);
        }

        [Test]
        public void TestOverlayError1()
        {
            var transformation = new Transformation().Overlay(new TextLayer().PublicId("test").FontStyle("italic"));
            Assert.That(() => transformation.ToString(), Throws.TypeOf<ArgumentException>(), "Must supply fontFamily for text in overlay");
        }

        [Test]
        public void TestOverlayError2()
        {
            var transformation = new Transformation().Overlay(new VideoLayer());
            Assert.That(() => transformation.ToString(), Throws.TypeOf<ArgumentException>(), "Must supply publicId for non-text underlay");
        }

        [Test]
        public void TestFetchFormat()
        {
            // should support format for fetch urls
            String result = m_api.UrlImgUp.Format("jpg").Action("fetch").BuildUrl("http://cloudinary.com/images/logo.png");
            Assert.AreEqual(m_defaultRootPath + "image/fetch/f_jpg/http://cloudinary.com/images/logo.png", result);
        }

        [Test]
        public void TestEffect()
        {
            // should support effect
            Transformation transformation = new Transformation().Effect("sepia");
            String result = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "e_sepia/test", result);
        }

        [Test]
        public void TestEffectWithParam()
        {
            // should support effect with param
            Transformation transformation = new Transformation().Effect("sepia", 10);
            String result = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "e_sepia:10/test", result);
        }

        [Test]
        public void TestDensity()
        {
            // should support density
            Transformation transformation = new Transformation().Density(150);
            String result = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "dn_150/test", result);
        }

        [Test]
        public void TestZoom()
        {
            // should support zooming
            var transformation = new Transformation().Crop("crop").Gravity("face").Zoom(3);
            var result = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "c_crop,g_face,z_3/test", result);
        }

        [Test]
        public void TestPage()
        {
            // should support page
            Transformation transformation = new Transformation().Page(5);
            String result = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "pg_5/test", result);
        }

        [Test]
        public void TestBorder()
        {
            // should support border
            Transformation transformation = new Transformation().Border(5, "black");
            String result = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "bo_5px_solid_black/test", result);
            transformation = new Transformation().Border(5, "#ffaabbdd");
            result = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "bo_5px_solid_rgb:ffaabbdd/test", result);
            transformation = new Transformation().Border("1px_solid_blue");
            result = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "bo_1px_solid_blue/test", result);
        }

        [Test]
        public void TestFlags()
        {
            // should support flags
            Transformation transformation = new Transformation().Flags("abc");
            String result = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "fl_abc/test", result);
            transformation = new Transformation().Flags("abc", "def");
            result = m_api.UrlImgUp.Transform(transformation).BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "fl_abc.def/test", result);
        }

        [Test]
        public void TestTransformationClone()
        {
            // transformation should be cloneable
            Transformation transform1 = new Transformation().X(100).Y(100).Width(200).Crop("fill").Chain().Radius(10).Chain().Crop("crop").Width(100).Angle("12", "13", "14");
            Transformation transform2 = transform1.Clone();
            transform1 = transform1.Angle("22", "23").Chain().Crop("fill");

            Assert.AreEqual(3, transform1.NestedTransforms.Count);
            Assert.AreEqual(1, transform1.Params.Count);
            Assert.AreEqual(2, transform2.NestedTransforms.Count);
            Assert.AreEqual(3, transform2.Params.Count);
        }

        [Test]
        public void TestUrlClone()
        {
            // url should be cloneable
            Transformation t1 = new Transformation().Angle(12);
            Transformation t2 = new Transformation().Crop("fill");
            Url url1 = m_api.UrlImgUp.Transform(t1);
            Url url2 = url1.Clone().Action("go").Transform(t2);
            string result1 = url1.BuildUrl("test");
            string result2 = url2.BuildUrl("test");
            Assert.AreEqual(m_defaultImgUpPath + "a_12/test", result1);
            Assert.AreEqual(m_defaultRootPath + "image/go/c_fill/test", result2);
        }

        [Test]
        public void TestInitFromUri()
        {
            CloudinaryDotNet.Cloudinary cloudinary = new CloudinaryDotNet.Cloudinary("cloudinary://a:b@test123");
        }
        
        [Test]
        public void TestInitFromEnvironmentVariable()
        {
            Environment.SetEnvironmentVariable("CLOUDINARY_URL", "cloudinary://a:b@test123");
            CloudinaryDotNet.Cloudinary cloudinary = new CloudinaryDotNet.Cloudinary();
        }
        
        [Test]
        public void TestSecureDistributionFromUrl()
        {
            // should take secure distribution from url if secure=TRUE

            CloudinaryDotNet.Cloudinary cloudinary = new CloudinaryDotNet.Cloudinary("cloudinary://a:b@test123/config.secure.distribution.com");
            string url = cloudinary.Api.UrlImgUp.BuildUrl("test");

            Assert.AreEqual("https://config.secure.distribution.com/image/upload/test", url);
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
        public void TestSprite()
        {
            // should build urls to get sprite css and picture by tag (with transformations and prefix)

            string uri = m_api.UrlImgUp.Action("sprite").BuildUrl("teslistresourcesbytag1.png");
            Assert.AreEqual(m_defaultRootPath + "image/sprite/teslistresourcesbytag1.png", uri);

            uri = m_api.UrlImgUp.Action("sprite").BuildUrl("teslistresourcesbytag1.css");
            Assert.AreEqual(m_defaultRootPath + "image/sprite/teslistresourcesbytag1.css", uri);

            uri = m_api.ApiUrlImgUpV.CloudinaryAddr("http://api.cloudinary.com").Action("sprite").BuildUrl();
            Assert.AreEqual("http://api.cloudinary.com/v1_1/testcloud/image/sprite", uri);
        }

        [Test]
        public void TestSpriteTransform()
        {
            // should build urls to get sprite css and picture by tag with transformations

            Transformation t = new Transformation().Crop("fit").Height(60).Width(150);
            string uri = m_api.UrlImgUp.Action("sprite").Transform(t).BuildUrl("logo.png");
            Assert.AreEqual(m_defaultRootPath + "image/sprite/c_fit,h_60,w_150/logo.png", uri);
        }

        [Test]
        public void TestSpriteCssPrefix()
        {
            // should build urls to get sprite css and picture by tag with prefix

            string uri = m_api.UrlImgUp.Action("sprite").Add("p_home_thing_").BuildUrl("logo.css");
            Assert.AreEqual(m_defaultRootPath + "image/sprite/p_home_thing_/logo.css", uri);
        }

        [Test]
        public void TestFolders()
        {
            // should add version if public_id contains /

            string result = m_api.UrlImgUp.BuildUrl("folder/test");
            Assert.AreEqual(m_defaultImgUpPath + "v1/folder/test", result);
            result = m_api.UrlImgUp.Version("123").BuildUrl("folder/test");
            Assert.AreEqual(m_defaultImgUpPath + "v123/folder/test", result);
            result = m_api.UrlImgUp.BuildUrl("1/av1/test");
            Assert.AreEqual(m_defaultImgUpPath + "v1/1/av1/test", result);
        }

        [Test]
        public void TestFoldersWithVersion()
        {
            // should not add version if public_id contains version already

            string result = m_api.UrlImgUp.BuildUrl("v1234/test");
            Assert.AreEqual(m_defaultImgUpPath + "v1234/test", result);
        }

        [Test]
        public void TestShortenUrl()
        {
            // should allow to shorted image/upload urls

            string result = m_api.UrlImgUp.Shorten(true).BuildUrl("test");
            Assert.AreEqual(m_defaultRootPath + "iu/test", result);
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
                Assert.AreEqual(m_defaultImgUpPath + "" + entry.Value, result);
            }
        }

        [Test]
        public void TestSpriteCss()
        {
            var result = m_api.UrlImgUp.BuildSpriteCss("test");
            Assert.AreEqual(m_defaultRootPath + "image/sprite/test.css", result);
            result = m_api.UrlImgUp.BuildSpriteCss("test.css");
            Assert.AreEqual(m_defaultRootPath + "image/sprite/test.css", result);
        }

        [Test]
        public void TestSignedUrl()
        {
            // should correctly sign a url

            var api = new Api("cloudinary://a:b@test123");

            var expected = "http://res.cloudinary.com/test123/image/upload/s--Ai4Znfl3--/c_crop,h_20,w_10/v1234/image.jpg";
            var actual = api.UrlImgUp.Version("1234")
                    .Transform(new Transformation().Crop("crop").Width(10).Height(20))
                    .Signed(true)
                    .BuildUrl("image.jpg");

            Assert.AreEqual(expected, actual);

            expected = "http://res.cloudinary.com/test123/image/upload/s----SjmNDA--/v1234/image.jpg";
            actual = api.UrlImgUp.Version("1234")
                    .Signed(true)
                    .BuildUrl("image.jpg");

            Assert.AreEqual(expected, actual);

            expected = "http://res.cloudinary.com/test123/image/upload/s--Ai4Znfl3--/c_crop,h_20,w_10/image.jpg";
            actual = api.UrlImgUp
                    .Transform(new Transformation().Crop("crop").Width(10).Height(20))
                     .Signed(true)
                    .BuildUrl("image.jpg");

            Assert.AreEqual(expected, actual);

            expected = "http://res.cloudinary.com/test123/image/upload/s--eMXgzFAO--/c_crop,h_20,w_1/v1/image.jpg";
            actual = api.UrlImgUp.Version("1")
                    .Transform(new Transformation().Crop("crop").Width(1).Height(20))
                    .Signed(true)
                    .BuildUrl("image.jpg");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSupportUseRootPathInSharedDistribution()
        {
            var actual = m_api.UrlImgUp.UseRootPath(true).PrivateCdn(false).BuildUrl("test");
            Assert.AreEqual(m_defaultRootPath + "test", actual);
            actual = m_api.UrlImgUp.UseRootPath(true).PrivateCdn(false).Transform(new Transformation().Angle(0)).BuildUrl("test");
            Assert.AreEqual(m_defaultRootPath + "a_0/test", actual);
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

        [Test]
        public void TestResponsiveWidthTransform()
        {
            // should support responsive width

            var trans = new Transformation().Width(100).Height(100).Crop("crop").ResponsiveWidth(true);
            var result = m_api.UrlImgUp.Transform(trans).BuildUrl("test");
            Assert.True(trans.IsResponsive);
            Assert.AreEqual(m_defaultImgUpPath + "c_crop,h_100,w_100/c_limit,w_auto/test", result);
            Transformation.ResponsiveWidthTransform = new Transformation().Width("auto").Crop("pad"); trans = new Transformation().Width(100).Height(100).Crop("crop").ResponsiveWidth(true);
            result = m_api.UrlImgUp.Transform(trans).BuildUrl("test");
            Assert.True(trans.IsResponsive);
            Assert.AreEqual(m_defaultImgUpPath + "c_crop,h_100,w_100/c_pad,w_auto/test", result);
            Transformation.ResponsiveWidthTransform = null;
        }

        [Test]
        public void TestVideoCodec()
        {
            // should support a string value

            var actual = m_api.UrlVideoUp.Transform(new Transformation().VideoCodec("auto")).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "vc_auto/video_id", actual);

            // should support a hash value

            actual = m_api.UrlVideoUp.Transform(new Transformation().VideoCodec("codec", "h264", "profile", "basic", "level", "3.1")).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "vc_h264:basic:3.1/video_id", actual);
        }

        [Test]
        public void TestAudioCodec()
        {
            // should support a string value

            var actual = m_api.UrlVideoUp.Transform(new Transformation().AudioCodec("acc")).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "ac_acc/video_id", actual);
        }

        [Test]
        public void TestBitRate()
        {
            // should support a numeric value

            var actual = m_api.UrlVideoUp.Transform(new Transformation().BitRate(2048)).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "br_2048/video_id", actual);

            // should support a string value

            actual = m_api.UrlVideoUp.Transform(new Transformation().BitRate("44k")).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "br_44k/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().BitRate("1m")).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "br_1m/video_id", actual);
        }

        [Test]
        public void TestAudioFrequency()
        {
            // should support an integer value

            String actual = m_api.UrlVideoUp.Transform(new Transformation().AudioFrequency(44100)).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "af_44100/video_id", actual);

            // should support a string value

            actual = m_api.UrlVideoUp.Transform(new Transformation().AudioFrequency("44100")).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "af_44100/video_id", actual);
        }

        [Test]
        public void TestKeyframeInterval()
        {
            // should support an integer value
            String actual = m_api.UrlVideoUp.Transform(new Transformation().KeyframeInterval(100)).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "ki_100/video_id", actual);

            // should support a string value
            actual = m_api.UrlVideoUp.Transform(new Transformation().KeyframeInterval("100")).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "ki_100/video_id", actual);
        }

        [Test]
        public void TestVideoSampling()
        {
            String actual = m_api.UrlVideoUp.Transform(new Transformation().VideoSamplingFrames(20)).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "vs_20/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().VideoSamplingSeconds(20)).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "vs_20s/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().VideoSamplingSeconds(20.0)).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "vs_20.0s/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().VideoSampling("2.3s")).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "vs_2.3s/video_id", actual);
        }

        [Test]
        public void TestStartOffset()
        {
            var actual = m_api.UrlVideoUp.Transform(new Transformation().StartOffset(2.63)).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "so_2.63/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().StartOffset("2.63p")).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "so_2.63p/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().StartOffset("2.63%")).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "so_2.63p/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().StartOffsetPercent(2.63)).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "so_2.63p/video_id", actual);
        }

        [Test]
        public void TestDuration()
        {
            var actual = m_api.UrlVideoUp.Transform(new Transformation().Duration(2.63)).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "du_2.63/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().Duration("2.63p")).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "du_2.63p/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().Duration("2.63%")).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "du_2.63p/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().DurationPercent(2.63)).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "du_2.63p/video_id", actual);
        }

        [Test]
        public void TestOffset()
        {
            var actual = m_api.UrlVideoUp.Transform(new Transformation().Offset("2.66..3.21")).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "eo_3.21,so_2.66/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().Offset(new float[] { 2.67f, 3.22f })).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "eo_3.22,so_2.67/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().Offset(new double[] { 2.67, 3.22 })).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "eo_3.22,so_2.67/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().Offset(new String[] { "35%", "70%" })).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "eo_70p,so_35p/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().Offset(new String[] { "36p", "71p" })).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "eo_71p,so_36p/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().Offset(new String[] { "35.5p", "70.5p" })).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "eo_70.5p,so_35.5p/video_id", actual);
        }

        [Test]
        public void TestStartEndOffset()
        {
            var actual = m_api.UrlVideoUp.Transform(new Transformation().StartOffset("2.66").EndOffset("3.21")).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "eo_3.21,so_2.66/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().StartOffset(2.67f).EndOffset(3.22f)).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "eo_3.22,so_2.67/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().StartOffset(2.67).EndOffset(3.22)).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "eo_3.22,so_2.67/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().StartOffset("35%").EndOffset("70%")).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "eo_70p,so_35p/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().StartOffset("36p").EndOffset("71p")).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "eo_71p,so_36p/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().StartOffset("35.5p").EndOffset("70.5p")).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "eo_70.5p,so_35.5p/video_id", actual);
        }

        [Test]
        public void TestZoomVideo()
        {
            String actual = m_api.UrlVideoUp.Transform(new Transformation().Zoom("1.5")).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "z_1.5/video_id", actual);
            actual = m_api.UrlVideoUp.Transform(new Transformation().Zoom(1.5)).BuildUrl("video_id");
            Assert.AreEqual(m_defaultVideoUpPath + "z_1.5/video_id", actual);
        }

        [Test]
        public void TestVideoTag()
        {
            var expectedUrl = m_defaultVideoUpPath + "movie";
            var expectedTag = "<video poster='{0}.jpg'>" + "<source src='{0}.webm' type='video/webm'>"
                    + "<source src='{0}.mp4' type='video/mp4'>"
                    + "<source src='{0}.ogv' type='video/ogg'>"
                    + "</video>";
            expectedTag = String.Format(expectedTag, expectedUrl);
            Assert.AreEqual(expectedTag, m_api.UrlVideoUp.BuildVideoTag("movie").ToString());
        }

        [Test]
        public void TestVideoTagWithAttributes()
        {
            var attributes = new StringDictionary(
                "autoplay=true",
                "controls",
                "loop",
                "muted=true",
                "preload",
                "style=border: 1px");

            var expectedUrl = m_defaultVideoUpPath + "movie";
            var expectedTag = "<video autoplay='true' controls loop muted='true' poster='{0}.jpg' preload style='border: 1px'>"
                    + "<source src='{0}.webm' type='video/webm'>"
                    + "<source src='{0}.mp4' type='video/mp4'>"
                    + "<source src='{0}.ogv' type='video/ogg'>" + "</video>";
            expectedTag = String.Format(expectedTag, expectedUrl);
            Assert.AreEqual(expectedTag, m_api.UrlVideoUp.BuildVideoTag("movie", attributes).ToString());
        }

        [Test]
        public void TestVideoTagWithTransformation()
        {
            var transformation = new Transformation().VideoCodec("codec", "h264").AudioCodec("acc").StartOffset(3);
            var expectedUrl = m_defaultVideoUpPath + "ac_acc,so_3.0,vc_h264/movie";
            var expectedTag = "<video height='100' poster='{0}.jpg' src='{0}.mp4' width='200'></video>";
            expectedTag = String.Format(expectedTag, expectedUrl);

            var actualTag = m_api.UrlVideoUp.Transform(transformation).SourceTypes(new string[] { "mp4" })
                    .BuildVideoTag("movie", "html_height=100", "html_width=200").ToString();
            Assert.AreEqual(expectedTag, actualTag);

            expectedTag = "<video height='100' poster='{0}.jpg' width='200'>"
                    + "<source src='{0}.webm' type='video/webm'>"
                    + "<source src='{0}.mp4' type='video/mp4'>"
                    + "<source src='{0}.ogv' type='video/ogg'>"
                    + "</video>";
            expectedTag = String.Format(expectedTag, expectedUrl);
            actualTag = m_api.UrlVideoUp.Transform(transformation)
                    .BuildVideoTag("movie", new StringDictionary() { { "html_height", "100" }, { "html_width", "200" } })
                    .ToString();

            Assert.AreEqual(expectedTag, actualTag);

            transformation.Width(250);
            expectedUrl = m_defaultVideoUpPath + "ac_acc,so_3.0,vc_h264,w_250/movie";
            expectedTag = "<video poster='{0}.jpg' width='250'>"
                    + "<source src='{0}.webm' type='video/webm'>"
                    + "<source src='{0}.mp4' type='video/mp4'>"
                    + "<source src='{0}.ogv' type='video/ogg'>"
                    + "</video>";
            expectedTag = String.Format(expectedTag, expectedUrl);
            actualTag = m_api.UrlVideoUp.Transform(transformation)
                    .BuildVideoTag("movie").ToString();
            Assert.AreEqual(expectedTag, actualTag);

            transformation.Crop("fit");
            expectedUrl = m_defaultVideoUpPath + "ac_acc,c_fit,so_3.0,vc_h264,w_250/movie";
            expectedTag = "<video poster='{0}.jpg'>"
                    + "<source src='{0}.webm' type='video/webm'>"
                    + "<source src='{0}.mp4' type='video/mp4'>"
                    + "<source src='{0}.ogv' type='video/ogg'>"
                    + "</video>";
            expectedTag = String.Format(expectedTag, expectedUrl);
            actualTag = m_api.UrlVideoUp.Transform(transformation)
                    .BuildVideoTag("movie").ToString();
            Assert.AreEqual(expectedTag, actualTag);
        }

        [Test]
        public void TestVideoTagWithFallback()
        {
            var expectedUrl = m_defaultVideoUpPath + "movie";
            var fallback = "<span id='spanid'>Cannot display video</span>";
            var expectedTag = "<video poster='{0}.jpg' src='{0}.mp4'>{1}</video>";
            expectedTag = String.Format(expectedTag, expectedUrl, fallback);
            var actualTag = m_api.UrlVideoUp.FallbackContent(fallback).SourceTypes(new String[] { "mp4" })
                    .BuildVideoTag("movie").ToString();
            Assert.AreEqual(expectedTag, actualTag);

            expectedTag = "<video poster='{0}.jpg'>" + "<source src='{0}.webm' type='video/webm'>"
                    + "<source src='{0}.mp4' type='video/mp4'>" + "<source src='{0}.ogv' type='video/ogg'>{1}" + "</video>";
            expectedTag = String.Format(expectedTag, expectedUrl, fallback);
            actualTag = m_api.UrlVideoUp.FallbackContent(fallback).BuildVideoTag("movie").ToString();
            Assert.AreEqual(expectedTag, actualTag);
        }

        [Test]
        public void TestVideoTagWithSourceTypes()
        {
            var expectedUrl = m_defaultVideoUpPath + "movie";
            var expectedTag = "<video poster='{0}.jpg'>" + "<source src='{0}.ogv' type='video/ogg'>"
                    + "<source src='{0}.mp4' type='video/mp4'>" + "</video>";
            expectedTag = String.Format(expectedTag, expectedUrl);
            string actualTag = m_api.UrlVideoUp.SourceTypes(new string[] { "ogv", "mp4" })
                    .BuildVideoTag("movie.mp4").ToString();
            Assert.AreEqual(expectedTag, actualTag);
        }

        [Test]
        public void TestVideoTagWithSourceTransformation()
        {
            var expectedUrl = m_defaultVideoUpPath + "q_50/w_100/movie";
            var expectedOgvUrl = m_defaultVideoUpPath + "q_50/w_100/q_70/movie";
            var expectedMp4Url = m_defaultVideoUpPath + "q_50/w_100/q_30/movie";
            var expectedTag = "<video poster='{0}.jpg' width='100'>"
                    + "<source src='{0}.webm' type='video/webm'>"
                    + "<source src='{1}.mp4' type='video/mp4'>"
                    + "<source src='{2}.ogv' type='video/ogg'>"
                    + "</video>";
            expectedTag = String.Format(expectedTag, expectedUrl, expectedMp4Url, expectedOgvUrl);
            var actualTag = m_api.UrlVideoUp.Transform(new Transformation().Quality(50).Chain().Width(100))
                    .SourceTransformationFor("mp4", new Transformation().Quality(30))
                    .SourceTransformationFor("ogv", new Transformation().Quality(70))
                    .BuildVideoTag("movie").ToString();
            Assert.AreEqual(expectedTag, actualTag);

            expectedTag = "<video poster='{0}.jpg' width='100'>" + "<source src='{0}.webm' type='video/webm'>"
                    + "<source src='{1}.mp4' type='video/mp4'>" + "</video>";
            expectedTag = String.Format(expectedTag, expectedUrl, expectedMp4Url);
            actualTag = m_api.UrlVideoUp.Transform(new Transformation().Quality(50).Chain().Width(100))
                    .SourceTransformationFor("mp4", new Transformation().Quality(30))
                    .SourceTransformationFor("ogv", new Transformation().Quality(70))
                    .SourceTypes("webm", "mp4").BuildVideoTag("movie").ToString();
            Assert.AreEqual(expectedTag, actualTag);
        }

        [Test]
        public void TestVideoTagWithPoster()
        {
            var expectedUrl = m_defaultVideoUpPath + "movie";
            var posterUrl = "http://image/somewhere.jpg";
            var expectedTag = "<video poster='{0}' src='{1}.mp4'></video>";
            expectedTag = String.Format(expectedTag, posterUrl, expectedUrl);
            var actualTag = m_api.UrlVideoUp.SourceTypes("mp4").Poster(posterUrl)
                    .BuildVideoTag("movie").ToString();
            Assert.AreEqual(expectedTag, actualTag);

            posterUrl = m_defaultVideoUpPath + "g_north/movie.jpg";
            expectedTag = "<video poster='{0}' src='{1}.mp4'></video>";
            expectedTag = String.Format(expectedTag, posterUrl, expectedUrl);
            actualTag = m_api.UrlVideoUp.SourceTypes("mp4")
                    .Poster(new Transformation().Gravity("north"))
                    .BuildVideoTag("movie").ToString();
            Assert.AreEqual(expectedTag, actualTag);

            posterUrl = m_defaultVideoUpPath + "g_north/my_poster.jpg";
            expectedTag = "<video poster='{0}' src='{1}.mp4'></video>";
            expectedTag = String.Format(expectedTag, posterUrl, expectedUrl);
            actualTag = m_api.UrlVideoUp.SourceTypes("mp4")
                .Poster(m_api.UrlVideoUp.Source("my_poster").Format("jpg").Transform(new Transformation().Gravity("north")))
                .BuildVideoTag("movie").ToString();
            Assert.AreEqual(expectedTag, actualTag);

            expectedTag = "<video src='{0}.mp4'></video>";
            expectedTag = String.Format(expectedTag, expectedUrl);
            actualTag = m_api.UrlVideoUp.SourceTypes("mp4").Poster(null).BuildVideoTag("movie").ToString();
            Assert.AreEqual(expectedTag, actualTag);

            actualTag = m_api.UrlVideoUp.SourceTypes("mp4").Poster(false).BuildVideoTag("movie").ToString();
            Assert.AreEqual(expectedTag, actualTag);
        }

        [Test]
        public void TestEmptySource()
        {
            var url = m_api.UrlImgUp.BuildUrl();
            Assert.AreEqual(m_defaultImgUpPath, url + "/");
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
            //image for overlay
            //http://image.com/img/seatrade_supplier_logo.jpg

            //fetch image
            //http://image.com/files/8813/5551/7470/cruise-ship.png

            var transformation = new Transformation().Overlay(new FetchLayer().Url("http://image.com/img/seatrade_supplier_logo.jpg"));
            var uri = m_api.UrlImgFetch.Transform(transformation).BuildUrl("http://image.com/files/8813/5551/7470/cruise-ship.png");
            Assert.AreEqual(m_defaultImgFetchPath + "l_fetch:aHR0cDovL2ltYWdlLmNvbS9pbWcvc2VhdHJhZGVfc3VwcGxpZXJfbG9nby5qcGc=/http://image.com/files/8813/5551/7470/cruise-ship.png", uri);
        }

        [Test]
        public void TestUploadTransformationEffect()
        {
            Transformation effect = new Transformation().Effect("art:incognito");

            Assert.AreEqual(effect.ToString(), "e_art:incognito");
        }

        [Test]
        public void TestSignParameters()
        {
            Dictionary<string, object> paramsSetOne = new Dictionary<string, object>() {
                { "Param1", "anyString"},
                { "Param2", 25},
                { "Param3", 25.35f},
            };

            Dictionary<string, object> paramsSetTwo = new Dictionary<string, object>(paramsSetOne) {
                { "resource_type", "image" },
                { "file", "anyFile" },
                { "api_key", "343dsfdf033e-23zx" }
            };

            StringAssert.AreEqualIgnoringCase(m_api.SignParameters(paramsSetOne), m_api.SignParameters(paramsSetTwo), "The signatures are not equal.");

            paramsSetTwo.Add("Param4", "test");

            StringAssert.AreNotEqualIgnoringCase(m_api.SignParameters(paramsSetOne), m_api.SignParameters(paramsSetTwo), "The signatures are equal.");
        }
    }
}
