﻿using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests.Asset
{
    [TestFixture]
    public partial class SignatureTest
    {
        private const string PUBLIC_ID1 = "b8sjhoslj8cq8ovoa0ma";
        private const string PUBLIC_ID2 = "z5sjhoskl2cq8ovoa0mv";
        private const string VERSION1 = "1555337587";
        private const string VERSION2 = "1555337588";

        protected Api m_api;

        [OneTimeSetUp]
        public void Init()
        {
            var account = new Account(TestConstants.CloudName, TestConstants.DefaultApiKey,
                TestConstants.DefaultApiSecret);
            m_api = new Api(account);
        }

        [SetUp]
        public void BeforeTest()
        {
            m_api.SignatureAlgorithm = SignatureAlgorithm.SHA1;
        }

        [TearDown]
        public void AfterTest()
        {
            m_api.SignatureAlgorithm = SignatureAlgorithm.SHA1;
        }

        [Test]
        public void TestSign()
        {
            var parameters = new SortedDictionary<string, object>();

            parameters.Add("public_id", "sample");
            parameters.Add("timestamp", "1315060510");

            Assert.AreEqual("c3470533147774275dd37996cc4d0e68fd03cd4f", m_api.SignParameters(parameters));
        }

        [Test]
        public void TestSignSha1()
        {
            var parameters = new SortedDictionary<string, object>
            {
                { "cloud_name", "dn6ot3ged"},
                { "timestamp", "1568810420"},
                { "username", "user@cloudinary.com"}
            };
            var api = new Api("cloudinary://a:hdcixPpR2iKERPwqvH6sHdK9cyac@test123");

            Assert.AreEqual("14c00ba6d0dfdedbc86b316847d95b9e6cd46d94", api.SignParameters(parameters));
        }

        [Test]
        public void TestSignSha256()
        {
            var parameters = new SortedDictionary<string, object>
            {
                { "cloud_name", "dn6ot3ged"},
                { "timestamp", "1568810420"},
                { "username", "user@cloudinary.com"}
            };
            var api = new Api("cloudinary://a:hdcixPpR2iKERPwqvH6sHdK9cyac@test123");
            api.SignatureAlgorithm = SignatureAlgorithm.SHA256;

            Assert.AreEqual("45ddaa4fa01f0c2826f32f669d2e4514faf275fe6df053f1a150e7beae58a3bd", api.SignParameters(parameters));
        }

        /// <summary>
        /// Should prevent parameter smuggling via & characters in parameter values.
        /// </summary>
        [Test]
        public void TestApiSignRequestPreventsParameterSmuggling()
        {
            const string testCloudName = "dn6ot3ged";
            const string testSecret = "hdcixPpR2iKERPwqvH6sHdK9cyac";

            // Test with notification_url containing & characters
            var paramsWithAmpersand = new SortedDictionary<string, object>
            {
                { "cloud_name", testCloudName },
                { "timestamp", 1568810420 },
                { "notification_url", "https://fake.com/callback?a=1&tags=hello,world" }
            };

            var api = new Api($"cloudinary://key:{testSecret}@test123");
            var signatureWithAmpersand = api.SignParameters(paramsWithAmpersand);

            // Test that attempting to smuggle parameters by splitting the notification_url fails
            var paramsSmugggled = new SortedDictionary<string, object>
            {
                { "cloud_name", testCloudName },
                { "timestamp", 1568810420 },
                { "notification_url", "https://fake.com/callback?a=1" },
                { "tags", "hello,world" }  // This would be smuggled if & encoding didn't work
            };

            var signatureSmugggled = api.SignParameters(paramsSmugggled);

            // The signatures should be different, proving that parameter smuggling is prevented
            Assert.AreNotEqual(signatureWithAmpersand, signatureSmugggled,
                            "Signatures should be different to prevent parameter smuggling");

            // Verify the expected signature for the properly encoded case
            const string expectedSignature = "4fdf465dd89451cc1ed8ec5b3e314e8a51695704";
            Assert.AreEqual(expectedSignature, signatureWithAmpersand);

            // Verify the expected signature for the smuggled parameters case
            const string expectedSmuggledSignature = "7b4e3a539ff1fa6e6700c41b3a2ee77586a025f9";
            Assert.AreEqual(expectedSmuggledSignature, signatureSmugggled);
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

        [Test]
        public void TestVerifyApiResponseSignature()
        {
            var responseParameters = new SortedDictionary<string, object> {
                { "public_id", PUBLIC_ID1},
                { "version", VERSION1}
            };
            var correctSignature = m_api.SignParameters(responseParameters);

            Assert.IsTrue(m_api.VerifyApiResponseSignature(PUBLIC_ID1, VERSION1, correctSignature),
                "The response signature is valid for the same parameters");

            responseParameters["version"] = VERSION2;
            var newVersionSignature = m_api.SignParameters(responseParameters);

            Assert.IsFalse(m_api.VerifyApiResponseSignature(PUBLIC_ID1, VERSION1, newVersionSignature),
                "The response signature is invalid for the wrong version");

            responseParameters["version"] = VERSION1;
            responseParameters["public_id"] = PUBLIC_ID2;
            var anotherResourceSignature = m_api.SignParameters(responseParameters);

            Assert.IsFalse(m_api.VerifyApiResponseSignature(PUBLIC_ID1, VERSION1, anotherResourceSignature),
                "The response signature is invalid for the wrong resource");
        }

        [Test]
        public void TestVerifyApiResponseSignatureSha256()
        {
            var api = new Api("cloudinary://a:X7qLTrsES31MzxxkxPPA-pAGGfU@test123");
            api.SignatureAlgorithm = SignatureAlgorithm.SHA256;
            const string correctSignature = "cc69ae4ed73303fbf4a55f2ae5fc7e34ad3a5c387724bfcde447a2957cacdfea";

            var verificationResult = api.VerifyApiResponseSignature("tests/logo.png", "1", correctSignature);

            Assert.IsTrue(verificationResult);
        }

        [Test]
        public void TestVerifyNotificationSignature()
        {
            var responseParameters = new SortedDictionary<string, object> {
                { "public_id", PUBLIC_ID1},
                { "version", VERSION1},
                { "width", "1000"},
                { "height", "800"}
            };
            var responseJson = JsonConvert.SerializeObject(responseParameters);

            var currentTimestamp = Utils.UnixTimeNowSeconds();
            var validResponseTimestamp = currentTimestamp - 5000;

            var payload = $"{responseJson}{validResponseTimestamp}{m_api.Account.ApiSecret}";
            var responseSignature = Utils.ComputeHexHash(payload);

            const string testMessagePart = "The notification signature is";

            Assert.IsTrue(m_api.VerifyNotificationSignature(responseJson, validResponseTimestamp,
                responseSignature), $"{testMessagePart} valid for matching and not expired signature");

            Assert.IsFalse(m_api.VerifyNotificationSignature(responseJson, validResponseTimestamp,
                responseSignature, 4000), $"{testMessagePart} is invalid for matching but expired signature");

            Assert.IsFalse(m_api.VerifyNotificationSignature(responseJson, validResponseTimestamp,
                responseSignature + "chars"), $"{testMessagePart} invalid for non matching and not expired signature");

            Assert.IsFalse(m_api.VerifyNotificationSignature(responseJson, validResponseTimestamp,
                responseSignature + "chars", 4000), $"{testMessagePart} invalid for non matching and expired signature");
        }

        [Test]
        public void TestVerifyNotificationSignatureSha256()
        {
            m_api.SignatureAlgorithm = SignatureAlgorithm.SHA256;
            var currentTimestamp = Utils.UnixTimeNowSeconds();
            var validResponseTimestamp = currentTimestamp - 5000;

            var responseParameters = new SortedDictionary<string, object>();
            var responseJson = JsonConvert.SerializeObject(responseParameters);
            var payload = $"{responseJson}{validResponseTimestamp}{m_api.Account.ApiSecret}";
            var correctSignature = Utils.ComputeHexHash(payload, SignatureAlgorithm.SHA256);

            var verificationResult = m_api.VerifyNotificationSignature(responseJson, validResponseTimestamp, correctSignature);

            Assert.IsTrue(verificationResult);
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

        [TestCase("testFolder/%20Test Image_僅測試", "s--MIMNFCL4--/c_scale,h_200,w_300/v1/testFolder/%2520Test%20Image_%E5%83%85%E6%B8%AC%E8%A9%A6.jpg")]
        [TestCase("testFolder/TestImage_僅測試", "s--FvJh0bXb--/c_scale,h_200,w_300/v1/testFolder/TestImage_%E5%83%85%E6%B8%AC%E8%A9%A6.jpg")]
        [TestCase("testFolder/Test Image_僅測試", "s--_jfm-XyC--/c_scale,h_200,w_300/v1/testFolder/Test%20Image_%E5%83%85%E6%B8%AC%E8%A9%A6.jpg")]
        [TestCase("testFolder/Test Image", "s--rmmMhYpj--/c_scale,h_200,w_300/v1/testFolder/Test%20Image.jpg")]
        [TestCase("testFolder/TestImage", "s--lzoFcAJk--/c_scale,h_200,w_300/v1/testFolder/TestImage.jpg")]
        [TestCase("testFolder/%20Test Image", "s--AlS6-LgU--/c_scale,h_200,w_300/v1/testFolder/%2520Test%20Image.jpg")]
        [TestCase("testFolder/%20TestImage", "s--8xAbqJri--/c_scale,h_200,w_300/v1/testFolder/%2520TestImage.jpg")]
        public void TestSignedAnsiAndUnicodeUrl(string publicId, string expectedPath)
        {
            var urlBuilder = m_api.UrlImgUp.Action("authenticated")
                .Transform(new Transformation().Crop("scale").Height(200).Width(300))
                .Secure()
                .Signed(true);

            Assert.IsTrue(urlBuilder.BuildUrl($"{publicId}.jpg").EndsWith(expectedPath));
            Assert.IsTrue(urlBuilder.Format("jpg").BuildUrl(publicId).EndsWith(expectedPath));
        }

        [Test]
        public void TestSignedUrlSha256()
        {
            var api = new Api("cloudinary://a:b@test123");
            api.SignatureAlgorithm = SignatureAlgorithm.SHA256;

            var signedUrl = api.UrlImgUp.Signed(true).BuildUrl("sample.jpg");

            const string expectedUrl = "http://res.cloudinary.com/test123/image/upload/s--2hbrSMPO--/sample.jpg";
            Assert.AreEqual(expectedUrl, signedUrl);
        }

        [Test]
        public void TestSignatureLength()
        {
            var api = new Api("cloudinary://a:b@test123");

            var shortUrl = api.UrlImgUp.Signed(true).BuildUrl("sample.jpg");
            const string expectedShortUrl = "http://res.cloudinary.com/test123/image/upload/s--v2fTPYTu--/sample.jpg";
            Assert.AreEqual(expectedShortUrl, shortUrl);

            var longUrl = api.UrlImgUp.Signed(true).LongUrlSignature(true).BuildUrl("sample.jpg");
            const string expectedLongUrl = "http://res.cloudinary.com/test123/image/upload/s--2hbrSMPOjj5BJ4xV7SgFbRDevFaQNUFf--/sample.jpg";
            Assert.AreEqual(expectedLongUrl, longUrl);
        }

        /// <summary>
        /// Should apply the configured signature version.
        /// </summary>
        [Test]
        public void TestConfiguredSignatureVersionIsApplied()
        {
            var api = new Api("cloudinary://key:hdcixPpR2iKERPwqvH6sHdK9cyac@test123");
            var params1 = new SortedDictionary<string, object>
            {
                { "cloud_name", "dn6ot3ged" },
                { "timestamp", 1568810420 },
                { "notification_url", "https://fake.com/callback?a=1&tags=hello,world" }
            };

            // Test version 1 (no encoding)
            api.SignatureVersion = 1;
            var signatureV1 = api.SignParameters(params1);

            // Test version 2 (with encoding)
            api.SignatureVersion = 2;
            var signatureV2 = api.SignParameters(params1);

            // Should be different
            Assert.AreNotEqual(signatureV1, signatureV2);

            // Version 2 should match expected signature from smuggling test
            Assert.AreEqual("4fdf465dd89451cc1ed8ec5b3e314e8a51695704", signatureV2);
        }

        /// <summary>
        /// Should apply the configured signature version.
        /// </summary>
        [Test]
        public void TestSignatureVersionAffectsVerification()
        {
            const string testSecret = "hdcixPpR2iKERPwqvH6sHdK9cyac";
            var api = new Api($"cloudinary://key:{testSecret}@test123");

            // VerifyApiResponseSignature should use version 1
            var publicIdWithAmpersand = "callback?a=1&tags=hello,world";
            var version = "1568810420";

            var v1Signature = api.SignParameters(new SortedDictionary<string, object> { { "public_id", publicIdWithAmpersand }, { "version", version } }, 1);

            // API defaults to v2 but verify should still use v1
            Assert.IsTrue(api.VerifyApiResponseSignature(publicIdWithAmpersand, version, v1Signature));
        }
    }
}
