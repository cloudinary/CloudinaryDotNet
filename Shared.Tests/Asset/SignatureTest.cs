using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CloudinaryDotNet.Test.Asset
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
            CloudinaryConfiguration.SignatureAlgorithm = SignatureAlgorithm.SHA1;
        }

        [TearDown]
        public void AfterTest()
        {
            CloudinaryConfiguration.SignatureAlgorithm = SignatureAlgorithm.SHA1;
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
            CloudinaryConfiguration.SignatureAlgorithm = SignatureAlgorithm.SHA256;
            var api = new Api("cloudinary://a:hdcixPpR2iKERPwqvH6sHdK9cyac@test123");

            Assert.AreEqual("45ddaa4fa01f0c2826f32f669d2e4514faf275fe6df053f1a150e7beae58a3bd", api.SignParameters(parameters));
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
            CloudinaryConfiguration.SignatureAlgorithm = SignatureAlgorithm.SHA256;
            var api = new Api("cloudinary://a:X7qLTrsES31MzxxkxPPA-pAGGfU@test123");
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
            CloudinaryConfiguration.SignatureAlgorithm = SignatureAlgorithm.SHA256;
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

        [Test]
        public void TestSignedUrlSha256()
        {
            var api = new Api("cloudinary://a:b@test123");
            CloudinaryConfiguration.SignatureAlgorithm = SignatureAlgorithm.SHA256;

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
    }
}
