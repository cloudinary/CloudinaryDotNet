using NUnit.Framework;

namespace CloudinaryDotNet.Tests.Transformations.Video
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

        [TestCase("false", "vc_h265:auto:auto:bframes_no")]
        [TestCase("true", "vc_h265:auto:auto")]
        public void TestVideoCodecBFrames(string bFrame, string expected)
        {
            var actual = new Transformation()
                .VideoCodec("codec", "h265", "profile", "auto", "level", "auto", "b_frames", bFrame).ToString();
            Assert.AreEqual(expected, actual);
        }
    }
}
