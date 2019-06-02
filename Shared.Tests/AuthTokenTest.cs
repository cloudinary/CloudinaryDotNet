using System.Threading;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace CloudinaryDotNet.Test
{
    class AuthTokenTest : IntegrationTestBase
    {
        public const string KEY = "00112233FF99";
        public const string ALT_KEY = "CCBB2233FF00";

        private const int DURATION      = 300; //seconds
        private const int DURATION2     = 100; //seconds

        private const long START_TIME   = 1111111111;
        private const long START_TIME2  = 222222222;
        private const long START_TIME3  = 11111111;

        private const long EXPIRATION   = START_TIME  + DURATION;
        private const long EXPIRATION2  = START_TIME2 + DURATION2;
        private const long EXPIRATION2A = START_TIME2 + DURATION;
        private const long EXPIRATION3  = START_TIME3 + DURATION;

        private const int SLEEP_TIME_MS = 1001;

        private const string VERSION    = "1486020273";

        private const string CUSTOM_USER = "foobar";

        private const string SAMPLE_JPG = "sample.jpg";

        private const string    ACL_ALL         = "*";
        private readonly string ACL_IMAGE       = $"/{Constants.RESOURCE_TYPE_IMAGE}/*";
        private readonly string ACL_CUSTOM_USER = $"/*/t_{CUSTOM_USER}";

        protected const string authTokenTestTransformationAsString = "c_scale,w_300";
        protected readonly Transformation authTokenTestTransformation = new Transformation().Crop("scale").Width(300);

        private string m_cloudUrl;
        private string m_ImageUrl;
        private string m_AuthenticatedImageUrl;

        private Api _api;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            m_account = new Account("test123", "a", "b");
            _api = new Api(m_account)
            {
                UsePrivateCdn = true,
                PrivateCdn = "test123"
            };

            m_cloudUrl = $"http://{m_account.Cloud}-res.cloudinary.com";
            m_ImageUrl = $"{m_cloudUrl}/{Constants.RESOURCE_TYPE_IMAGE}";
            m_AuthenticatedImageUrl = $"{m_ImageUrl}/{STORAGE_TYPE_AUTHENTICATED}";
        }

        [TearDown]
        public void TearDown()
        {
            CloudinaryConfiguration.AuthToken = null;
        }

        /// <summary>
        /// Should generate an authorization token with startTime and duration
        /// </summary>
        [Test]
        public void GenerateTokenWithStartAndDuration()
        {
            AuthToken t = new AuthToken(KEY);
            t.StartTime(START_TIME).Acl(ACL_IMAGE).Duration(DURATION);
            Assert.AreEqual(
                $"__cld_token__=st={START_TIME}~exp={EXPIRATION}~acl=%2f{Constants.RESOURCE_TYPE_IMAGE}%2f*" +
                "~hmac=1751370bcc6cfe9e03f30dd1a9722ba0f2cdca283fa3e6df3342a00a7528cc51",
                t.Generate());
        }

        /// <summary>
        /// Should generate an authorization token with duration
        /// </summary>
        [Test]
        public void GenerateTokenWithDuration()
        {
            var firstExp = Utils.UnixTimeNowSeconds() + DURATION;
            Thread.Sleep(SLEEP_TIME_MS);

            var token = new AuthToken(KEY).Acl(ACL_ALL).Duration(DURATION).Generate();

            Thread.Sleep(SLEEP_TIME_MS);
            var secondExp = Utils.UnixTimeNowSeconds() + DURATION;

            Regex r = new Regex("exp=(\\d+)");
            Assert.True(r.IsMatch(token));

            var expString = r.Matches(token)[0].ToString().Replace("exp=", string.Empty);
            var actual = long.Parse(expString);

            Assert.Greater(actual, firstExp);
            Assert.Less(actual, secondExp);
            Assert.AreEqual(new AuthToken(KEY).Acl(ACL_ALL).Expiration(actual).Generate(), token);
        }

        [Test]
        public void TestAuthenticatedUrl()
        {
            var token = new AuthToken(KEY).Acl(ACL_ALL).Duration(DURATION).StartTime(START_TIME3);
            CloudinaryConfiguration.AuthToken = token;

            //should add token if authToken is globally set and signed = true;
            string url = _api.Url.Signed(true).ResourceType(Constants.RESOURCE_TYPE_IMAGE).
                Version(VERSION).BuildUrl(SAMPLE_JPG);

            Assert.AreEqual(
                $"{m_ImageUrl}/v{VERSION}/{SAMPLE_JPG}?__cld_token__=st={START_TIME3}~exp={EXPIRATION3}~acl=*" +
                "~hmac=67c908ea11bde7bced81926fe6dfa683f116a4f24714f150844b0160352588ba",
                url
            );

            // should not add token if signed is false
            url = _api.Url.Signed(false).ResourceType(Constants.RESOURCE_TYPE_IMAGE).
                Version(VERSION).BuildUrl(SAMPLE_JPG);

            Assert.AreEqual($"{m_ImageUrl}/v{VERSION}/{SAMPLE_JPG}", url);

            //should not add token if authToken is globally set but null auth token is explicitly set and signed = true
            url = _api.Url.Signed(true).AuthToken(AuthToken.NULL_AUTH_TOKEN).
                ResourceType(Constants.RESOURCE_TYPE_IMAGE).Version(VERSION).BuildUrl(SAMPLE_JPG);

            Assert.AreEqual($"{m_ImageUrl}/v{VERSION}/{SAMPLE_JPG}", url);

            //explicit authToken should override global setting
            token = new AuthToken(ALT_KEY).StartTime(START_TIME2).Duration(DURATION2);
            url = _api.Url.Signed(true).AuthToken(token).ResourceType(Constants.RESOURCE_TYPE_IMAGE)
                .Transform(authTokenTestTransformation).Action(STORAGE_TYPE_AUTHENTICATED).BuildUrl(SAMPLE_JPG);

            Assert.AreEqual(
                $"{m_AuthenticatedImageUrl}/{authTokenTestTransformationAsString}/{SAMPLE_JPG}?__cld_token__=st={START_TIME2}" +
                $"~exp={EXPIRATION2}~hmac=55cfe516530461213fe3b3606014533b1eca8ff60aeab79d1bb84c9322eebc1f",
                url
            );

            //should compute expiration as start time + duration
            token = new AuthToken(ALT_KEY).StartTime(START_TIME3).Duration(DURATION);
            url = _api.Url.Signed(true).AuthToken(token).Version(VERSION).Action(STORAGE_TYPE_AUTHENTICATED)
                .ResourceType(Constants.RESOURCE_TYPE_IMAGE).BuildUrl(SAMPLE_JPG);

            Assert.AreEqual(
                $"{m_AuthenticatedImageUrl}/v{VERSION}/{SAMPLE_JPG}?__cld_token__=st={START_TIME3}~exp={EXPIRATION3}" +
                "~hmac=4e6979377bf65734f3b9a1d984b0c8c393ca804520b0f74d1688be0cce80dbc1",
                url
            );
        }

        [Test]
        public void TestIgnoreUrlIfAclIsProvided()
        {
            var aclToken = new AuthToken(KEY).Duration(DURATION).Acl(ACL_CUSTOM_USER).StartTime(START_TIME2)
                .Generate();

            var aclTokenUrlIgnored = new AuthToken(KEY).Duration(DURATION).Acl(ACL_CUSTOM_USER).StartTime(START_TIME2)
                .Generate(m_ImageUrl);

            Assert.AreEqual(
                aclToken,
                aclTokenUrlIgnored
            );
        }

        [Test]
        public void TestTokenGeneration()
        {
            var cookieToken = new AuthToken(KEY).Duration(DURATION).Acl(ACL_CUSTOM_USER).StartTime(START_TIME2)
                .Generate();

            Assert.AreEqual(
                $"__cld_token__=st={START_TIME2}~exp={EXPIRATION2A}~acl=%2f*%2ft_{CUSTOM_USER}" +
                "~hmac=8e39600cc18cec339b21fe2b05fcb64b98de373355f8ce732c35710d8b10259f",
                cookieToken
            );
        }

        [Test]
        public void TestUrlInTag()
        {
            //should add token to an image tag url
            AuthToken t = new AuthToken(TOKEN_KEY).StartTime(START_TIME).Acl(ACL_IMAGE).Duration(DURATION);
            string url = _api.Url.AuthToken(t).Signed(true).ResourceType(Constants.RESOURCE_TYPE_IMAGE)
                .Version(VERSION).BuildImageTag(SAMPLE_JPG);

            Assert.AreEqual(
                $"<img src=\"{m_ImageUrl}/v{VERSION}/{SAMPLE_JPG}?__cld_token__=st={START_TIME}~exp={EXPIRATION}" +
                $"~acl=%2f{Constants.RESOURCE_TYPE_IMAGE}%2f*" +
                "~hmac=1751370bcc6cfe9e03f30dd1a9722ba0f2cdca283fa3e6df3342a00a7528cc51\"/>",
                url
            );

        }

        [Test]
        public void TestGenerateCookieAuthToken()
        {
            var token = new AuthToken(TOKEN_KEY).Duration(DURATION).Acl(ACL_CUSTOM_USER).StartTime(START_TIME2);
            var cookieToken = token.Generate();

            Assert.AreEqual(
                $"__cld_token__=st={START_TIME2}~exp={EXPIRATION2A}~acl=%2f*%2ft_{CUSTOM_USER}" +
                "~hmac=8e39600cc18cec339b21fe2b05fcb64b98de373355f8ce732c35710d8b10259f",
                cookieToken
            );
        }
    }
}
