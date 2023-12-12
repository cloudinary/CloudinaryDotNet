using System;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests.Tag
{
    [TestFixture]
    public class VideoTagTest
    {
        protected Api m_api;
        private const string SOURCE_MOVIE = "movie";

        [OneTimeSetUp]
        public void Init()
        {
            var account = new Account(TestConstants.CloudName, TestConstants.DefaultApiKey,
                TestConstants.DefaultApiSecret);
            m_api = new Api(account);
        }

        [Test]
        public void TestVideoTag()
        {
            const string expectedUrl = TestConstants.DefaultVideoUpPath + SOURCE_MOVIE;
            var expectedTag = "<video poster='{0}.jpg'>" + "<source src='{0}.webm' type='video/webm'>"
                    + "<source src='{0}.mp4' type='video/mp4'>"
                    + "<source src='{0}.ogv' type='video/ogg'>"
                    + "</video>";
            expectedTag = String.Format(expectedTag, expectedUrl);
            Assert.AreEqual(expectedTag, m_api.UrlVideoUp.BuildVideoTag(SOURCE_MOVIE).ToString());
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

            var expectedUrl = TestConstants.DefaultVideoUpPath + SOURCE_MOVIE;
            var expectedTag = "<video autoplay='true' controls loop muted='true' poster='{0}.jpg' preload style='border: 1px'>"
                    + "<source src='{0}.webm' type='video/webm'>"
                    + "<source src='{0}.mp4' type='video/mp4'>"
                    + "<source src='{0}.ogv' type='video/ogg'>" + "</video>";
            expectedTag = String.Format(expectedTag, expectedUrl);
            Assert.AreEqual(expectedTag, m_api.UrlVideoUp.BuildVideoTag(SOURCE_MOVIE, attributes).ToString());
        }

        [Test]
        public void TestVideoTagWithTransformation()
        {
            var transformation = new Transformation().VideoCodec("codec", "h264").AudioCodec("acc").StartOffset(3);
            var expectedUrl = TestConstants.DefaultVideoUpPath + $"ac_acc,so_3.0,vc_h264/{SOURCE_MOVIE}";
            var expectedTag = "<video height='100' poster='{0}.jpg' src='{0}.mp4' width='200'></video>";
            expectedTag = String.Format(expectedTag, expectedUrl);

            var actualTag = m_api.UrlVideoUp.Transform(transformation).SourceTypes(new string[] { "mp4" })
                        .BuildVideoTag(SOURCE_MOVIE, "html_height=100", "html_width=200").ToString();
            Assert.AreEqual(expectedTag, actualTag);

            expectedTag = "<video height='100' poster='{0}.jpg' width='200'>"
                    + "<source src='{0}.webm' type='video/webm'>"
                    + "<source src='{0}.mp4' type='video/mp4'>"
                    + "<source src='{0}.ogv' type='video/ogg'>"
                    + "</video>";
            expectedTag = String.Format(expectedTag, expectedUrl);
            actualTag = m_api.UrlVideoUp.Transform(transformation)
                    .BuildVideoTag(SOURCE_MOVIE, new StringDictionary() { { "html_height", "100" }, { "html_width", "200" } })
                    .ToString();

            Assert.AreEqual(expectedTag, actualTag);

            transformation.Width(250);
            expectedUrl = TestConstants.DefaultVideoUpPath + $"ac_acc,so_3.0,vc_h264,w_250/{SOURCE_MOVIE}";
            expectedTag = "<video poster='{0}.jpg' width='250'>"
                    + "<source src='{0}.webm' type='video/webm'>"
                    + "<source src='{0}.mp4' type='video/mp4'>"
                    + "<source src='{0}.ogv' type='video/ogg'>"
                    + "</video>";
            expectedTag = String.Format(expectedTag, expectedUrl);
            actualTag = m_api.UrlVideoUp.Transform(transformation)
                    .BuildVideoTag(SOURCE_MOVIE).ToString();
            Assert.AreEqual(expectedTag, actualTag);

            transformation.Crop("fit");
            expectedUrl = TestConstants.DefaultVideoUpPath + $"ac_acc,c_fit,so_3.0,vc_h264,w_250/{SOURCE_MOVIE}";
            expectedTag = "<video poster='{0}.jpg'>"
                    + "<source src='{0}.webm' type='video/webm'>"
                    + "<source src='{0}.mp4' type='video/mp4'>"
                    + "<source src='{0}.ogv' type='video/ogg'>"
                    + "</video>";
            expectedTag = String.Format(expectedTag, expectedUrl);
            actualTag = m_api.UrlVideoUp.Transform(transformation)
                    .BuildVideoTag(SOURCE_MOVIE).ToString();
            Assert.AreEqual(expectedTag, actualTag);
        }

        [Test]
        public void TestVideoTagWithDefaultSources()
        {
            var expectedTag =
                $"<video poster=\'{TestConstants.DefaultVideoUpPath}{SOURCE_MOVIE}.jpg\'>" +
                $"<source src=\'{TestConstants.DefaultVideoUpPath}vc_h265/{SOURCE_MOVIE}.mp4\' type=\'video/mp4; codecs=hev1\'>" +
                $"<source src=\'{TestConstants.DefaultVideoUpPath}vc_vp9/{SOURCE_MOVIE}.webm\' type=\'video/webm; codecs=vp9\'>" +
                $"<source src=\'{TestConstants.DefaultVideoUpPath}vc_auto/{SOURCE_MOVIE}.mp4\' type=\'video/mp4\'>" +
                $"<source src=\'{TestConstants.DefaultVideoUpPath}vc_auto/{SOURCE_MOVIE}.webm\' type=\'video/webm\'>" +
                "</video>";

            var actualTag = m_api.UrlVideoUp.VideoSources(Url.DefaultVideoSources).BuildVideoTag(SOURCE_MOVIE);

            Assert.AreEqual(expectedTag, actualTag);
        }

        [Test]
        public void TestVideoTagWithDefaultSourcesUseFetchFormat()
        {
            const string movie = SOURCE_MOVIE + ".mp4";
            var expectedTag =
                $"<video poster=\'{TestConstants.DefaultVideoUpPath}f_jpg/{movie}\'>" +
                $"<source src=\'{TestConstants.DefaultVideoUpPath}f_mp4,vc_h265/{movie}\' type=\'video/mp4; codecs=hev1\'>" +
                $"<source src=\'{TestConstants.DefaultVideoUpPath}f_webm,vc_vp9/{movie}\' type=\'video/webm; codecs=vp9\'>" +
                $"<source src=\'{TestConstants.DefaultVideoUpPath}f_mp4,vc_auto/{movie}\' type=\'video/mp4\'>" +
                $"<source src=\'{TestConstants.DefaultVideoUpPath}f_webm,vc_auto/{movie}\' type=\'video/webm\'>" +
                "</video>";

            var actualTag = m_api.UrlVideoUp.VideoSources(Url.DefaultVideoSources).UseFetchFormat().BuildVideoTag(movie);

            Assert.AreEqual(expectedTag, actualTag);
        }

        [Test]
        public void TestVideoTagWithCustomSources()
        {
            var customSources = new[]
            {
                new VideoSource
                {
                    Type = "mp4",
                    Codecs = new[] {"vp8", "vorbis"},
                    Transformation = new Transformation().VideoCodec("auto")
                },
                new VideoSource
                {
                    Type = "webm",
                    Codecs = new[] {"avc1.4D401E", "mp4a.40.2"},
                    Transformation = new Transformation().VideoCodec("auto")
                }
            };

            var expectedTag =
                $"<video poster=\'{TestConstants.DefaultVideoUpPath}{SOURCE_MOVIE}.jpg\'>" +
                $"<source src=\'{TestConstants.DefaultVideoUpPath}vc_auto/{SOURCE_MOVIE}.mp4\'" +
                " type=\'video/mp4; codecs=vp8, vorbis\'>" +
                $"<source src=\'{TestConstants.DefaultVideoUpPath}vc_auto/{SOURCE_MOVIE}.webm\'" +
                " type=\'video/webm; codecs=avc1.4D401E, mp4a.40.2\'>" +
                "</video>";

            var actualTag = m_api.UrlVideoUp.VideoSources(customSources).BuildVideoTag(SOURCE_MOVIE);

            Assert.AreEqual(expectedTag, actualTag);
        }

        [Test]
        public void TestVideoTagWithTransformations()
        {
            var paramsDict = new StringDictionary()
            {
                {"html_height", "100"},
                {"html_width", "200"},
            };

            var urlPrefix = $"{TestConstants.DefaultVideoUpPath}ac_acc,so_3,";

            var expectedTag =
                $"<video height='100' poster=\'{urlPrefix}vc_h264/{SOURCE_MOVIE}.jpg\' width='200'>" +
                $"<source src=\'{urlPrefix}vc_h265/{SOURCE_MOVIE}.mp4\' type=\'video/mp4; codecs=hev1\'>" +
                $"<source src=\'{urlPrefix}vc_vp9/{SOURCE_MOVIE}.webm\' type=\'video/webm; codecs=vp9\'>" +
                $"<source src=\'{urlPrefix}vc_auto/{SOURCE_MOVIE}.mp4\' type=\'video/mp4\'>" +
                $"<source src=\'{urlPrefix}vc_auto/{SOURCE_MOVIE}.webm\' type=\'video/webm\'>" +
                "</video>";

            var actualTag = m_api.UrlVideoUp
                .VideoSources(Url.DefaultVideoSources)
                .SourceTypes("mp4")
                .Transform(new Transformation().VideoCodec("h264").AudioCodec("acc").StartOffset("3"))
                .BuildVideoTag(SOURCE_MOVIE, paramsDict);

            Assert.AreEqual(expectedTag, actualTag);
        }

        [Test]
        public void TestVideoTagWithFallback()
        {
            var expectedUrl = TestConstants.DefaultVideoUpPath + SOURCE_MOVIE;
            var fallback = "<span id='spanid'>Cannot display video</span>";
            var expectedTag = "<video poster='{0}.jpg' src='{0}.mp4'>{1}</video>";
            expectedTag = String.Format(expectedTag, expectedUrl, fallback);
            var actualTag = m_api.UrlVideoUp.FallbackContent(fallback).SourceTypes(new String[] { "mp4" })
                    .BuildVideoTag(SOURCE_MOVIE).ToString();
            Assert.AreEqual(expectedTag, actualTag);

            expectedTag = "<video poster='{0}.jpg'>" + "<source src='{0}.webm' type='video/webm'>"
                    + "<source src='{0}.mp4' type='video/mp4'>" + "<source src='{0}.ogv' type='video/ogg'>{1}" + "</video>";
            expectedTag = String.Format(expectedTag, expectedUrl, fallback);
            actualTag = m_api.UrlVideoUp.FallbackContent(fallback).BuildVideoTag(SOURCE_MOVIE).ToString();
            Assert.AreEqual(expectedTag, actualTag);
        }

        [Test]
        public void TestVideoTagWithSourceTypes()
        {
            var expectedUrl = TestConstants.DefaultVideoUpPath + SOURCE_MOVIE;
            var expectedTag = "<video poster='{0}.jpg'>" + "<source src='{0}.ogv' type='video/ogg'>"
                    + "<source src='{0}.mp4' type='video/mp4'>" + "</video>";
            expectedTag = String.Format(expectedTag, expectedUrl);
            string actualTag = m_api.UrlVideoUp.SourceTypes(new string[] { "ogv", "mp4" })
                    .BuildVideoTag($"{SOURCE_MOVIE}.mp4").ToString();
            Assert.AreEqual(expectedTag, actualTag);
        }

        [Test]
        public void TestVideoTagWithSourceTransformation()
        {
            var expectedUrl = TestConstants.DefaultVideoUpPath + $"q_50/w_100/{SOURCE_MOVIE}";
            var expectedOgvUrl = TestConstants.DefaultVideoUpPath + $"q_50/w_100/q_70/{SOURCE_MOVIE}";
            var expectedMp4Url = TestConstants.DefaultVideoUpPath + $"q_50/w_100/q_30/{SOURCE_MOVIE}";
            var expectedTag = "<video poster='{0}.jpg' width='100'>"
                    + "<source src='{0}.webm' type='video/webm'>"
                    + "<source src='{1}.mp4' type='video/mp4'>"
                    + "<source src='{2}.ogv' type='video/ogg'>"
                    + "</video>";
            expectedTag = String.Format(expectedTag, expectedUrl, expectedMp4Url, expectedOgvUrl);
            var actualTag = m_api.UrlVideoUp.Transform(new Transformation().Quality(50).Chain().Width(100))
                    .SourceTransformationFor("mp4", new Transformation().Quality(30))
                    .SourceTransformationFor("ogv", new Transformation().Quality(70))
                    .BuildVideoTag(SOURCE_MOVIE).ToString();
            Assert.AreEqual(expectedTag, actualTag);

            expectedTag = "<video poster='{0}.jpg' width='100'>" + "<source src='{0}.webm' type='video/webm'>"
                    + "<source src='{1}.mp4' type='video/mp4'>" + "</video>";
            expectedTag = String.Format(expectedTag, expectedUrl, expectedMp4Url);
            actualTag = m_api.UrlVideoUp.Transform(new Transformation().Quality(50).Chain().Width(100))
                    .SourceTransformationFor("mp4", new Transformation().Quality(30))
                    .SourceTransformationFor("ogv", new Transformation().Quality(70))
                    .SourceTypes("webm", "mp4").BuildVideoTag(SOURCE_MOVIE).ToString();
            Assert.AreEqual(expectedTag, actualTag);
        }

        [Test]
        public void TestVideoTagWithPoster()
        {
            var expectedUrl = TestConstants.DefaultVideoUpPath + SOURCE_MOVIE;
            var posterUrl = "http://image/somewhere.jpg";
            var expectedTag = "<video poster='{0}' src='{1}.mp4'></video>";
            expectedTag = String.Format(expectedTag, posterUrl, expectedUrl);
            var actualTag = m_api.UrlVideoUp.SourceTypes("mp4").Poster(posterUrl)
                    .BuildVideoTag(SOURCE_MOVIE).ToString();
            Assert.AreEqual(expectedTag, actualTag);

            posterUrl = TestConstants.DefaultVideoUpPath + $"g_north/{SOURCE_MOVIE}.jpg";
            expectedTag = "<video poster='{0}' src='{1}.mp4'></video>";
            expectedTag = String.Format(expectedTag, posterUrl, expectedUrl);
            actualTag = m_api.UrlVideoUp.SourceTypes("mp4")
                    .Poster(new Transformation().Gravity(Gravity.North))
                    .BuildVideoTag(SOURCE_MOVIE).ToString();
            Assert.AreEqual(expectedTag, actualTag);

            posterUrl = TestConstants.DefaultVideoUpPath + "g_north/my_poster.jpg";
            expectedTag = "<video poster='{0}' src='{1}.mp4'></video>";
            expectedTag = String.Format(expectedTag, posterUrl, expectedUrl);
            actualTag = m_api.UrlVideoUp.SourceTypes("mp4")
                .Poster(m_api.UrlVideoUp.Source("my_poster").Format("jpg").Transform(new Transformation().Gravity(Gravity.North)))
                .BuildVideoTag(SOURCE_MOVIE).ToString();
            Assert.AreEqual(expectedTag, actualTag);

            expectedTag = "<video src='{0}.mp4'></video>";
            expectedTag = String.Format(expectedTag, expectedUrl);
            actualTag = m_api.UrlVideoUp.SourceTypes("mp4").Poster(null).BuildVideoTag(SOURCE_MOVIE).ToString();
            Assert.AreEqual(expectedTag, actualTag);

            actualTag = m_api.UrlVideoUp.SourceTypes("mp4").Poster(false).BuildVideoTag(SOURCE_MOVIE).ToString();
            Assert.AreEqual(expectedTag, actualTag);
        }
    }
}
