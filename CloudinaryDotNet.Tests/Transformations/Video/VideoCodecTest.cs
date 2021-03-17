﻿using NUnit.Framework;

namespace CloudinaryDotNet.Test.Transformations.Video
{
    [TestFixture]
    public class VideoCodecTest
    {
        [TestCase(VideoCodec.Auto, "vc_auto")]
        [TestCase(VideoCodec.Vp8, "vc_vp8")]
        [TestCase(VideoCodec.Vp9, "vc_vp9")]
        [TestCase(VideoCodec.Prores, "vc_prores")]
        [TestCase(VideoCodec.H264, "vc_h264")]
        [TestCase(VideoCodec.H265, "vc_h265")]
        [TestCase(VideoCodec.Theora, "vc_theora")]
        public void TestVideoCodecsConstantValues(string actual, string expected)
        {
            Assert.AreEqual(expected, new Transformation().VideoCodec(actual).ToString());
        }

        [Test]
        public void TestVideoCodec()
        {
            // should support a string value

            var actual = new Transformation().VideoCodec("auto").ToString();
            Assert.AreEqual("vc_auto", actual);

            // should support a hash value

            actual = new Transformation().VideoCodec("codec", "h264", "profile", "basic", "level", "3.1").ToString();
            Assert.AreEqual("vc_h264:basic:3.1", actual);
        }
    }
}
