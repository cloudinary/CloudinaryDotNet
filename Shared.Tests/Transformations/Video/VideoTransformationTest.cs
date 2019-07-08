using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CloudinaryDotNet.Test.Transformations.Video
{
    [TestFixture]
    public class VideoTransformationTest
    {
        [Test]
        public void TestAudioCodec()
        {
            // should support a string value

            var actual = new Transformation().AudioCodec("acc").ToString();
            Assert.AreEqual("ac_acc", actual);
        }

        [Test]
        public void TestBitRate()
        {
            // should support a numeric value

            var actual = new Transformation().BitRate(2048).ToString();
            Assert.AreEqual("br_2048", actual);

            // should support a string value

            actual = new Transformation().BitRate("44k").ToString();
            Assert.AreEqual("br_44k", actual);
            actual = new Transformation().BitRate("1m").ToString();
            Assert.AreEqual("br_1m", actual);
        }

        [Test]
        public void TestAudioFrequency()
        {
            // should support an integer value

            var actual = new Transformation().AudioFrequency(44100).ToString();
            Assert.AreEqual("af_44100", actual);

            // should support a string value

            actual = new Transformation().AudioFrequency("44100").ToString();
            Assert.AreEqual("af_44100", actual);

            // should support an enum value

            actual = new Transformation().AudioFrequency(AudioFrequency.AF44100).ToString();
            Assert.AreEqual("af_44100", actual);
        }

        [Test]
        public void TestKeyframeInterval()
        {
            Assert.AreEqual("ki_10.0", new Transformation().KeyframeInterval(10).ToString());
            Assert.AreEqual("ki_0.05", new Transformation().KeyframeInterval(0.05f).ToString());
            Assert.AreEqual("ki_3.45", new Transformation().KeyframeInterval(3.45f).ToString());
            Assert.AreEqual("ki_300.0", new Transformation().KeyframeInterval(300).ToString());
            Assert.AreEqual("ki_10", new Transformation().KeyframeInterval("10").ToString());
            Assert.AreEqual("", new Transformation().KeyframeInterval("").ToString());
            Assert.AreEqual("", new Transformation().KeyframeInterval(null).ToString());

            Assert.That(() => new Transformation().KeyframeInterval(-10).ToString(),
                Throws.TypeOf<ArgumentException>(), "Should throw an exception when keyframe interval is less than 0.");
            Assert.That(() => new Transformation().KeyframeInterval(0f).ToString(),
                Throws.TypeOf<ArgumentException>(), "Should throw an exception when keyframe interval equals 0.");
        }

        [Test]
        public void TestStreamingProfile()
        {
            var spTransformation = new Transformation().StreamingProfile("some-profile").ToString();
            Assert.AreEqual("sp_some-profile", spTransformation);
        }

        [Test]
        public void TestVideoSampling()
        {
            var actual = new Transformation().VideoSamplingFrames(20).ToString();
            Assert.AreEqual("vs_20", actual);
            actual = new Transformation().VideoSamplingSeconds(20).ToString();
            Assert.AreEqual("vs_20s", actual);
            actual = new Transformation().VideoSamplingSeconds(20.0).ToString();
            Assert.AreEqual("vs_20.0s", actual);
            actual = new Transformation().VideoSampling("2.3s").ToString();
            Assert.AreEqual("vs_2.3s", actual);
        }

        [Test]
        public void TestStartOffset()
        {
            var actual = new Transformation().StartOffset(2.63).ToString();
            Assert.AreEqual("so_2.63", actual);
            actual = new Transformation().StartOffset("2.63p").ToString();
            Assert.AreEqual("so_2.63p", actual);
            actual = new Transformation().StartOffset("2.63%").ToString();
            Assert.AreEqual("so_2.63p", actual);
            actual = new Transformation().StartOffsetPercent(2.63).ToString();
            Assert.AreEqual("so_2.63p", actual);
            actual = new Transformation().StartOffset("auto").ToString();
            Assert.AreEqual("so_auto", actual);
            actual = new Transformation().StartOffsetAuto().ToString();
            Assert.AreEqual("so_auto", actual);
        }

        [Test]
        public void TestDuration()
        {
            var actual = new Transformation().Duration(2.63).ToString();
            Assert.AreEqual("du_2.63", actual);
            actual = new Transformation().Duration("2.63p").ToString();
            Assert.AreEqual("du_2.63p", actual);
            actual = new Transformation().Duration("2.63%").ToString();
            Assert.AreEqual("du_2.63p", actual);
            actual = new Transformation().DurationPercent(2.63).ToString();
            Assert.AreEqual("du_2.63p", actual);
        }

        [Test]
        public void TestOffset()
        {
            var actual = new Transformation().Offset("2.66..3.21").ToString();
            Assert.AreEqual("eo_3.21,so_2.66", actual);
            actual = new Transformation().Offset(new float[] { 2.67f, 3.22f }).ToString();
            Assert.AreEqual("eo_3.22,so_2.67", actual);
            actual = new Transformation().Offset(new double[] { 2.67, 3.22 }).ToString();
            Assert.AreEqual("eo_3.22,so_2.67", actual);
            actual = new Transformation().Offset(new String[] { "35%", "70%" }).ToString();
            Assert.AreEqual("eo_70p,so_35p", actual);
            actual = new Transformation().Offset(new String[] { "36p", "71p" }).ToString();
            Assert.AreEqual("eo_71p,so_36p", actual);
            actual = new Transformation().Offset(new String[] { "35.5p", "70.5p" }).ToString();
            Assert.AreEqual("eo_70.5p,so_35.5p", actual);
        }

        [Test]
        public void TestStartEndOffset()
        {
            var actual = new Transformation().StartOffset("2.66").EndOffset("3.21").ToString();
            Assert.AreEqual("eo_3.21,so_2.66", actual);
            actual = new Transformation().StartOffset(2.67f).EndOffset(3.22f).ToString();
            Assert.AreEqual("eo_3.22,so_2.67", actual);
            actual = new Transformation().StartOffset(2.67).EndOffset(3.22).ToString();
            Assert.AreEqual("eo_3.22,so_2.67", actual);
            actual = new Transformation().StartOffset("35%").EndOffset("70%").ToString();
            Assert.AreEqual("eo_70p,so_35p", actual);
            actual = new Transformation().StartOffset("36p").EndOffset("71p").ToString();
            Assert.AreEqual("eo_71p,so_36p", actual);
            actual = new Transformation().StartOffset("35.5p").EndOffset("70.5p").ToString();
            Assert.AreEqual("eo_70.5p,so_35.5p", actual);
        }

        [Test]
        public void TestZoomVideo()
        {
            var actual = new Transformation().Zoom("1.5").ToString();
            Assert.AreEqual("z_1.5", actual);
            actual = new Transformation().Zoom(1.5).ToString();
            Assert.AreEqual("z_1.5", actual);
        }

        [Test]
        public void TestVideoFps()
        {
            var testPairs = new Dictionary<Transformation, string>()
            {
                {new Transformation().Fps(24, 29.97), "fps_24-29.97"},
                {new Transformation().Fps(29.97), "fps_29.97"},
                {new Transformation().Fps(24), "fps_24"},
                {new Transformation().Fps(null, 29.97), "fps_-29.97"},
                {new Transformation().Fps(24, null), "fps_24-"},
                {new Transformation().Fps("$v"), "fps_$v"},
                {new Transformation().Fps("$min", "$max"), "fps_$min-$max"},
                {new Transformation().Fps("24-29.97"), "fps_24-29.97"},
            };
            foreach (var pair in testPairs)
            {
                var actual = pair.Key.ToString();
                Assert.AreEqual(pair.Value, actual);
            }
        }
    }
}
