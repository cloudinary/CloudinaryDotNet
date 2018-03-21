using System;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace CloudinaryDotNet.Test
{
    class AuthTokenTest : IntegrationTestBase
    {
        public const string KEY = "00112233FF99";
        public const string ALT_KEY = "CCBB2233FF00";

        [Test]
        public void GenerateTokenWithStartAndDuration()
        {
            //should generate an authorization token with startTime and duration
            AuthToken t = new AuthToken(KEY);
            t.StartTime(1111111111).Acl("/image/*").Duration(300);
            Assert.AreEqual("__cld_token__=st=1111111111~exp=1111111411~acl=%2fimage%2f*~hmac=1751370bcc6cfe9e03f30dd1a9722ba0f2cdca283fa3e6df3342a00a7528cc51", t.Generate());
        }

        [Test]
        public void GenerateTokenWithDuration()
        {
            //should generate an authorization token with startTime and duration
            long firstExp = DateTime.Now.ToUniversalTime().Ticks / 1000L + 300; 
            System.Threading.Thread.Sleep(1200);
            string token = new AuthToken(KEY).Acl("*").Duration(300).Generate();
            System.Threading.Thread.Sleep(1200);
            long secondExp = DateTime.Now.ToUniversalTime().Ticks / 1000L + 300; 
            Regex r = new Regex("exp=(\\d+)");
            Assert.True(r.IsMatch(token));
            string expString = r.Matches(token)[0].ToString().Replace("exp=", string.Empty);
            long actual = long.Parse(expString);
            Assert.True(actual >= firstExp);
            Assert.AreEqual(token, new AuthToken(KEY).Acl("*").Expiration(actual).Generate());
        }

        [Test]
        public void TestAuthenticatedUrl()
        {
            var token = new AuthToken(KEY).Acl("*").Duration(300);
            token.StartTime(11111111);
            CloudinaryConfiguration.AuthToken = token;
            
            //should add token if authToken is globally set and signed = true;
            string url = m_cloudinary.Api.Url.Signed(true).ResourceType("image").Version("1486020273").BuildUrl();
            Assert.AreEqual("http://res.cloudinary.com/" + m_account.Cloud + "/image/v1486020273?__cld_token__=st=11111111~exp=11111411~acl=*~hmac=67c908ea11bde7bced81926fe6dfa683f116a4f24714f150844b0160352588ba", url);

            // should not add token if signed is false
            url = m_cloudinary.Api.Url.Signed(false).ResourceType("image").Version("1486020273").BuildUrl();
            Assert.AreEqual("http://res.cloudinary.com/" + m_account.Cloud + "/image/v1486020273", url);

            //should not add token if authToken is globally set but null auth token is explicitly set and signed = true
            url = m_cloudinary.Api.Url.Signed(true).AuthToken(AuthToken.NULL_AUTH_TOKEN).ResourceType("image").Version("1486020273").BuildUrl();
            Assert.AreEqual("http://res.cloudinary.com/" + m_account.Cloud + "/image/v1486020273", url);

            //explicit authToken should override global setting
            url = m_cloudinary.Api.Url.Signed(true).AuthToken(new AuthToken(ALT_KEY).StartTime(222222222).Duration(100)).ResourceType("image").Version("1486020273").Transform
                (new Transformation().Crop("scale").Width(300)).BuildUrl();
            Assert.AreEqual("http://res.cloudinary.com/" + m_account.Cloud + "/image/c_scale,w_300/v1486020273?__cld_token__=st=222222222~exp=222222322~hmac=91f3401f8ff19755194b386010f3577a6d2fd7f72c44c793fa498419ad8633b6", url);

           //should compute expiration as start time + duration
            url = m_cloudinary.Api.Url.Signed(true).AuthToken(new AuthToken(ALT_KEY).StartTime(11111111).Duration(300)).Version("1486020273").BuildUrl("sample.jpg");
            Assert.AreEqual("http://res.cloudinary.com/" + m_account.Cloud + "/v1486020273/sample.jpg?__cld_token__=st=11111111~exp=11111411~hmac=a72618e343a488898732429b74e4a8cbb5767e2372421b8351d5381b8c6451ed", url);
            CloudinaryConfiguration.AuthToken = null;

        }

        [Test]
        public void TestTokenGeneration()
        {
            AuthToken token = new AuthToken(KEY);
            token.Duration(300);
            string user = "foobar"; // username taken from elsewhere
            token.Acl("/*/t_" + user);
            token.StartTime(222222222); // we can't rely on the default "now" value in tests
            string cookieToken = token.Generate();
            Assert.AreEqual("__cld_token__=st=222222222~exp=222222522~acl=%2f*%2ft_foobar~hmac=8e39600cc18cec339b21fe2b05fcb64b98de373355f8ce732c35710d8b10259f", cookieToken);
        }

        [Test]
        public void TestUrlInTag()
        {
            //should add token to an image tag url";
            AuthToken t = new AuthToken(TOKEN_KEY);
            t.StartTime(1111111111).Acl("/image/*").Duration(300);
            string url = m_cloudinary.Api.Url.AuthToken(t).Signed(true).ResourceType("image").Version("1486020273").BuildImageTag("sample.jpg");
            Assert.AreEqual("<img src=\"http://res.cloudinary.com/" + m_account.Cloud + "/image/v1486020273/sample.jpg?__cld_token__=st=1111111111~exp=1111111411~acl=%2fimage%2f*~hmac=1751370bcc6cfe9e03f30dd1a9722ba0f2cdca283fa3e6df3342a00a7528cc51\"/>", url);

        }

        [Test]
        public void TestGenerateCookieAuthToken()
        {
            AuthToken token = new AuthToken(TOKEN_KEY);
            token.duration = 300;
            string user = "foobar";
            token.acl = "/*/t_" + user;
            token.StartTime(222222222);
            string cookieToken = token.Generate();

            Assert.AreEqual("__cld_token__=st=222222222~exp=222222522~acl=%2f*%2ft_foobar~hmac=8e39600cc18cec339b21fe2b05fcb64b98de373355f8ce732c35710d8b10259f", cookieToken);
        }
    }
}
