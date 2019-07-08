using System;
using System.Collections.Generic;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.Test.Transformations.Image
{
    [TestFixture]
    public class ImageTransformationTest
    {
        [Test]
        public void TestBackground()
        {
            // should support background
            var transformation = new Transformation().Background("red").ToString();
            Assert.AreEqual("b_red", transformation);
            transformation = new Transformation().Background("#112233").ToString();
            Assert.AreEqual("b_rgb:112233", transformation);
        }

        [Test]
        public void TestDefaultImage()
        {
            // should support default_image
            var transformation = new Transformation().DefaultImage("default").ToString();
            Assert.AreEqual("d_default", transformation);
        }

        [Test]
        public void TestAngle()
        {
            // should support angle
            var transformation = new Transformation().Angle(12).ToString();
            Assert.AreEqual("a_12", transformation);
            transformation = new Transformation().Angle("exif", "12").ToString();
            Assert.AreEqual("a_exif.12", transformation);
        }

        [Test]
        public void TestOverlay()
        {
            // should support overlay
            var transformation = new Transformation().Overlay("text:hello");
            Assert.AreEqual("l_text:hello", transformation.ToString());
            // should not pass width/height to html if overlay
            transformation = new Transformation().Overlay("text:hello").Width(100).Height(100);
            Assert.IsNull(transformation.HtmlHeight);
            Assert.IsNull(transformation.HtmlWidth);
            Assert.AreEqual("h_100,l_text:hello,w_100", transformation.ToString());
        }

        [Test]
        public void TestOverlayHelpers()
        {
            var layerTests = new Dictionary<BaseLayer, string>
            {
                { new Layer().PublicId("logo"), "logo" },
                { new Layer().PublicId("folder/logo"), "folder:logo" },
                { new Layer().PublicId("logo").Type("private"), "private:logo" },
                { new Layer().PublicId("logo").Format("png"), "logo.png" },
                { new Layer().ResourceType("video").PublicId("cat"), "video:cat" },
                {
                    new TextLayer()
                        .Text("Hello World, Nice to meet you?")
                        .FontFamily("Arial")
                        .FontSize(18),
                    "text:Arial_18:Hello%20World%252C%20Nice%20to%20meet%20you%3F"
                },
                {
                    new TextLayer("Hello World?")
                        .FontFamily("Arial")
                        .FontSize(18)
                        .FontWeight("bold")
                        .FontStyle("italic")
                        .LetterSpacing("4")
                        .LineSpacing("3"),
                    "text:Arial_18_bold_italic_letter_spacing_4_line_spacing_3:Hello%20World%3F"
                },
                {
                    new TextLayer("Hello World, Nice to meet you?")
                        .FontFamily("Arial")
                        .FontSize(18)
                        .FontAntialiasing(FontAntialiasing.Best)
                        .FontHinting(FontHinting.Medium),
                    "text:Arial_18_antialias_best_hinting_medium:Hello%20World%252C%20Nice%20to%20meet%20you%3F"
                },
                { new SubtitlesLayer().PublicId("sample_sub_en.srt"), "subtitles:sample_sub_en.srt" },
                {
                    new SubtitlesLayer()
                        .PublicId("sample_sub_he.srt")
                        .FontFamily("Arial")
                        .FontSize(40),
                    "subtitles:Arial_40:sample_sub_he.srt"
                }
            };

            foreach (var layerTest in layerTests)
            {
                var expected = layerTest.Value;
                var actual = layerTest.Key.ToString();
                Assert.IsTrue(expected.Equals(actual, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        [Test]
        public void TestMultipleLayers()
        {
            var transformation = new Transformation()
                .Overlay("One").Chain()
                .Overlay("Two").Chain()
                .Overlay("Three").Chain()
                .Overlay("One").Chain()
                .Overlay("Two").Chain();
            Assert.AreEqual("l_One/l_Two/l_Three/l_One/l_Two", transformation.ToString());

        }

        [Test]
        public void TestUnderlay()
        {
            var transformation = new Transformation().Underlay("text:hello");
            Assert.AreEqual("u_text:hello", transformation.ToString());
            // should not pass width/height to html if underlay
            transformation = new Transformation().Underlay("text:hello").Width(100).Height(100);
            Assert.IsNull(transformation.HtmlHeight);
            Assert.IsNull(transformation.HtmlWidth);
            Assert.AreEqual("h_100,u_text:hello,w_100", transformation.ToString());
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
        public void TestOpacity()
        {
            var transformation = new Transformation().Overlay("overlay.png").Opacity(40);
            Assert.AreEqual("l_overlay.png,o_40", transformation.ToString());
        }

        [Test]
        public void TestCrop()
        {
            var transformation = new Transformation().Width(100).Height(101);
            Assert.AreEqual("h_101,w_100", transformation.ToString());
            Assert.AreEqual("101", transformation.HtmlHeight);
            Assert.AreEqual("100", transformation.HtmlWidth);

            transformation = new Transformation().Width(100).Height(101).Crop("crop");
            Assert.AreEqual("c_crop,h_101,w_100", transformation.ToString());
        }

        [Test]
        public void TestTransformations()
        {
            // should use x, y, radius, prefix, gravity and quality from options

            var transformation = new Transformation().X(1).Y(2).Radius(3).Gravity(Gravity.Center).Quality(0.4).Prefix("a");
            Assert.AreEqual("g_center,p_a,q_0.4,r_3,x_1,y_2", transformation.ToString());
        }

        [Test]
        public void TestSimpleTransformation()
        {
            // should support named transformation

            var transformation = new Transformation().Named("blip");
            Assert.AreEqual("t_blip", transformation.ToString());
        }

        [Test]
        public void TestTransformationArray()
        {
            // should support array of named transformations

            var transformation = new Transformation().Named("blip", "blop");
            Assert.AreEqual("t_blip.blop", transformation.ToString());
        }

        [Test]
        public void TestBaseTransformationChain()
        {
            // should support base transformation

            var transformation = new Transformation().X(100).Y(100).Crop("fill").Chain().Crop("crop").Width(100);
            var result = transformation.ToString();
            Assert.AreEqual("100", transformation.HtmlWidth);
            Assert.AreEqual("c_fill,x_100,y_100/c_crop,w_100", result);
        }

        [Test]
        public void TestEagerTransformationList()
        {
            var list = new List<Transformation>(){
                new EagerTransformation().SetFormat("jpg").Crop("scale").Width(2.0),
                new EagerTransformation(new Transformation().Width(10),new Transformation().Angle(10)),
                new Transformation().Width(20).Height(20)
            };

            var uploadParams = new ImageUploadParams
            {
                EagerTransforms = list
            };

            var dict = uploadParams.ToParamsDictionary();

            Assert.AreEqual("c_scale,w_2.0/jpg|w_10/a_10|h_20,w_20", dict["eager"]);
        }

        [Test]
        public void TestBaseTransformationArray()
        {
            // should support array of base transformations

            var transformation = new Transformation().X(100).Y(100).Width(200).Crop("fill").Chain().Radius(10).Chain().Crop("crop").Width(100);
            var result = transformation.ToString();
            Assert.AreEqual("100", transformation.HtmlWidth);
            Assert.AreEqual("c_fill,w_200,x_100,y_100/r_10/c_crop,w_100", result);
        }

        [Test]
        public void TestTransformationAutoWidth()
        {
            // should support transformations with width:auto and width:auto_breakpoints

            var transformation = new Transformation().Width("auto:20").Crop("fill");
            Assert.AreEqual("c_fill,w_auto:20", transformation.ToString());

            transformation = new Transformation().Width("auto:20:350").Crop("fill");
            Assert.AreEqual("c_fill,w_auto:20:350", transformation.ToString());

            transformation = new Transformation().Width("auto:breakpoints").Crop("fill");
            Assert.AreEqual("c_fill,w_auto:breakpoints", transformation.ToString());

            transformation = new Transformation().Width("auto:breakpoints_100_1900_20_15").Crop("fill");
            Assert.AreEqual("c_fill,w_auto:breakpoints_100_1900_20_15", transformation.ToString());

            transformation = new Transformation().Width("auto:breakpoints:json").Crop("fill");
            Assert.AreEqual("c_fill,w_auto:breakpoints:json", transformation.ToString());
        }

        [Test]
        public void TestEffect()
        {
            // should support effect
            var transformation = new Transformation().Effect("sepia");
            Assert.AreEqual("e_sepia", transformation.ToString());
        }

        [Test]
        public void TestEffectWithParam()
        {
            // should support effect with param
            var transformation = new Transformation().Effect("sepia", 10);
            Assert.AreEqual("e_sepia:10", transformation.ToString());
        }

        [Test]
        public void TestDensity()
        {
            // should support density
            var transformation = new Transformation().Density(150);
            Assert.AreEqual("dn_150", transformation.ToString());
        }

        [Test]
        public void TestZoom()
        {
            // should support zooming
            var transformation = new Transformation().Crop("crop").Gravity(Gravity.Face).Zoom(3);
            Assert.AreEqual("c_crop,g_face,z_3", transformation.ToString());
        }

        [Test]
        public void TestPage()
        {
            // should support page
            var transformation = new Transformation().Page(5);
            Assert.AreEqual("pg_5", transformation.ToString());
        }

        [Test]
        public void TestBorder()
        {
            // should support border
            var transformation = new Transformation().Border(5, "black");
            Assert.AreEqual("bo_5px_solid_black", transformation.ToString());
            transformation = new Transformation().Border(5, "#ffaabbdd");
            Assert.AreEqual("bo_5px_solid_rgb:ffaabbdd", transformation.ToString());
            transformation = new Transformation().Border("1px_solid_blue");
            Assert.AreEqual("bo_1px_solid_blue", transformation.ToString());
        }

        [Test]
        public void TestFlags()
        {
            // should support flags
            var transformation = new Transformation().Flags("abc");
            Assert.AreEqual("fl_abc", transformation.ToString());
            transformation = new Transformation().Flags("abc", "def");
            Assert.AreEqual("fl_abc.def", transformation.ToString());
        }

        [Test]
        public void TestResponsiveWidthTransform()
        {
            // should support responsive width

            var transformation = new Transformation().Width(100).Height(100).Crop("crop").ResponsiveWidth(true);
            var result = transformation.ToString();
            Assert.True(transformation.IsResponsive);
            Assert.AreEqual("c_crop,h_100,w_100/c_limit,w_auto", result);
            Transformation.ResponsiveWidthTransform = new Transformation().Width("auto").Crop("pad");
            transformation = new Transformation().Width(100).Height(100).Crop("crop").ResponsiveWidth(true);
            result = transformation.ToString();
            Assert.True(transformation.IsResponsive);
            Assert.AreEqual("c_crop,h_100,w_100/c_pad,w_auto", result);
            Transformation.ResponsiveWidthTransform = null;
        }

        [Test]
        public void TestUploadTransformationEffect()
        {
            var effect = new Transformation().Effect("art:incognito");

            Assert.AreEqual(effect.ToString(), "e_art:incognito");
        }

        [Test]
        public void TestBlurPixelateEffectsTransformation()
        {
            var transformation = new Transformation().Effect("blur_region");
            Assert.AreEqual("e_blur_region", transformation.ToString());

            transformation = new Transformation().Effect("pixelate_region");
            Assert.AreEqual("e_pixelate_region", transformation.ToString());
        }
    }
}
