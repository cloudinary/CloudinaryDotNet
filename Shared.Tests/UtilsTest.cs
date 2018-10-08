using NUnit.Framework;
using System;

namespace CloudinaryDotNet.Test
{
    class UtilsTest : IntegrationTestBase
    {
        private static readonly DateTime dateTime       = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime localDateTime  = dateTime.ToLocalTime();
        private static readonly long     unixDateTime   = 946684800;

        [Test]
        public void TestToUnixTimeSeconds()
        {
            Assert.AreEqual(unixDateTime, Utils.ToUnixTimeSeconds(dateTime));
            Assert.AreEqual(unixDateTime, Utils.ToUnixTimeSeconds(localDateTime));
        }

        [Test]
        public void TestFromUnixTimeSeconds()
        {
            Assert.AreEqual(dateTime, Utils.FromUnixTimeSeconds(unixDateTime));
        }

        [Test]
        public void TestUnixTimeNowSeconds()
        {
            // We don't have appropriate mocking mechanism to fake DateTime.Now at the moment
            // just check that resulting value it between start and end

            var start = DateTime.UtcNow.AddSeconds(-1);
            var nowUnixSeconds = Utils.UnixTimeNowSeconds();
            var end = DateTime.UtcNow.AddSeconds(1);

            var now = Utils.FromUnixTimeSeconds(nowUnixSeconds);

            Assert.LessOrEqual(start, now);
            Assert.GreaterOrEqual(end, now);
        }

        [Test]
        public void TestIsRemoteFile()
        {
            string[] remoteFiles =
            {
                "ftp://ftp.cloudinary.com/images/old_logo.png",
                "http://cloudinary.com/images/old_logo.png",
                "https://cloudinary.com/images/old_logo.png",
                "s3://s3-us-west-2.amazonaws.com/cloudinary/images/old_logo.png",
                "gs://cloudinary/images/old_logo.png",
            };
            string[] localFiles =
            {
                @"c:\s3\test.txt",
                @"..\https.png",
            };
            foreach (var path in remoteFiles)
            {
                Assert.IsTrue(Utils.IsRemoteFile(path), $"Path '${path}' should be remote");
            }
            foreach (var path in localFiles)
            {
                Assert.IsFalse(Utils.IsRemoteFile(path), $"Path '${path}' should be local");
            }
        }
    }
}
