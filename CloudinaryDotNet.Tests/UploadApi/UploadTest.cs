using System;
using System.Collections.Generic;
using CloudinaryDotNet.Actions;
using NUnit.Framework;
using SystemHttp = System.Net.Http;

namespace CloudinaryDotNet.Tests.UploadApi
{
    public class UploadTest
    {
        [Test]
        public void TestDefaultUploadTimestampAndSignature()
        {
            var cloudinary = new MockedCloudinary();


            var iuParams = new ImageUploadParams()
            {
                File = new FileDescription(TestConstants.TestRemoteImg)
            };

            cloudinary.Upload(iuParams);

            cloudinary.AssertHttpCall(SystemHttp.HttpMethod.Post, "image/upload");

            foreach (var expected in new List<string>
            {
                "api_key",
                "timestamp",
                "signature",
                TestConstants.TestRemoteImg
            })
            {
                StringAssert.Contains(expected, cloudinary.HttpRequestContent);
            }
        }

        [Test]
        public void TestCustomUploadTimestampAndSignature()
        {
            var cloudinary = new MockedCloudinary();

            var testDate = DateTime.Parse("2024-01-01 12:34:56Z", System.Globalization.CultureInfo.InvariantCulture)
                .ToUniversalTime();
            var timestamp = "1704112496";

            var signature = "c8ec18c58c626d509ffa37e15329020e5c9158dc";

            var iuParams = new ImageUploadParams()
            {
                File = new FileDescription(TestConstants.TestRemoteImg),
                Timestamp = testDate,
                Signature = signature
            };

            cloudinary.Upload(iuParams);

            cloudinary.AssertHttpCall(SystemHttp.HttpMethod.Post, "image/upload");

            foreach (var expected in new List<string>
                     {
                         "api_key",
                         "key",
                         "timestamp",
                         timestamp,
                         "signature",
                         signature,
                         "file",
                         TestConstants.TestRemoteImg
                     })
            {
                StringAssert.Contains(expected, cloudinary.HttpRequestContent);
            }
        }
    }
}
