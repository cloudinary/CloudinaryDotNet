using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;


namespace CloudinaryDotNet
{
    public class AuthToken
    {
        public static string AUTH_TOKEN_NAME = "__cld_token__";

        public static AuthToken NULL_AUTH_TOKEN = new AuthToken().SetNull();

        public string tokenName = AUTH_TOKEN_NAME;
        public string key;
        public long startTime;
        public long endTime;
        public long expiration;
        public string ip;
        public string acl;
        public long window;
        public long duration;
        private bool isNullToken = false;

        public AuthToken()
        {

        }

        public AuthToken(string key)
        {
            this.key = key;
        }

        public AuthToken StartTime(long startTime)
        {
            this.startTime = startTime;
            return this;
        }

        public AuthToken Expiration(long expiration)
        {
            this.expiration = expiration;
            return this;
        }

        public AuthToken Ip(string ip)
        {
            this.ip = ip;
            return this;
        }

        public AuthToken Acl(string acl)
        {
            this.acl = acl;
            return this;
        }

        public AuthToken Duration(long duration)
        {
            this.duration = duration;
            return this;
        }

        public string Generate()
        {
            return Generate(null);
        }

        public string Generate(string url)
        {
            long expiration = this.expiration;
            if (expiration == 0)
            {
                if (duration > 0)
                {
                    long start = startTime > 0 ? startTime : DateTime.Now.Ticks / 1000L;
                    expiration = start + duration;
                }
                else
                {
                    throw new ArgumentException("Must provide either expiration or duration");
                }
            }

            List<string> tokenParts = new List<string>();

            if (!string.IsNullOrWhiteSpace(ip))
            {
                tokenParts.Add(string.Format("ip={0}", ip));
            }

            if (startTime > 0)
            {
                tokenParts.Add(string.Format("st={0}", startTime.ToString()));
            }

            tokenParts.Add(string.Format("exp={0}", expiration.ToString()));

            if (!string.IsNullOrWhiteSpace(acl))
            {
                tokenParts.Add(string.Format("acl={0}", EscapeToLower(acl)));
            }

            List<string> toSign = new List<string>(tokenParts);

            if (!string.IsNullOrWhiteSpace(url))
            {
                toSign.Add(string.Format("url={0}", EscapeToLower(url)));
            }
            string auth = Digest(string.Join("~", toSign));
            tokenParts.Add(string.Format("hmac={0}", auth));

            return tokenName + "=" + string.Join("~", tokenParts);
        }

        public AuthToken Copy()
        {
            AuthToken authToken = new AuthToken(key);

            authToken.tokenName = tokenName;
            authToken.startTime = startTime;
            authToken.expiration = expiration;
            authToken.ip = ip;
            authToken.acl = acl;
            authToken.duration = duration;

            return authToken;
        }

        private AuthToken SetNull()
        {
            isNullToken = true;
            return this;
        }

        public override bool Equals(object o)
        {
            if (o is AuthToken)
            {
                AuthToken other = (AuthToken)o;
                return (isNullToken && other.isNullToken) ||
                    key == null ? other.key == null : key == other.key &&
                    tokenName == other.tokenName &&
                    startTime == other.startTime &&
                    expiration == other.expiration &&
                    duration == other.duration &&
                    (ip == null ? other.ip == null : ip == other.ip) &&
                    (acl == null ? other.acl == null : acl == other.acl);
            }
            else
            {
                return false;
            }
        }

        private string EscapeUrl(string url)
        {
            return Uri.EscapeDataString(url);
        }

        protected string EscapeToLower(string url)
        {
            string escaped = string.Empty;

            var encodedUrl = Utils.EncodedUrl(url);
            StringBuilder sb = new StringBuilder(encodedUrl);
            string result = sb.ToString();
            string regex = "%..";
            Regex r = new Regex(regex, RegexOptions.Compiled);
            foreach (Match ItemMatch in r.Matches(sb.ToString()))
            {
                string buf = sb.ToString().Substring(ItemMatch.Index, ItemMatch.Length).ToLower();
                sb.Remove(ItemMatch.Index, ItemMatch.Length);
                sb.Insert(ItemMatch.Index, buf);
            }

            return sb.ToString();
        }

        private string Digest(string message)
        {
            byte[] binKey = HexStringToByteArray(this.key);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
            HMACSHA256 hmac = new HMACSHA256(binKey);
            hmac.Initialize();
            byte[] signed = hmac.ComputeHash(buffer);
            string hex = BitConverter.ToString(signed).Replace("-", string.Empty).ToLower();

            return hex;
        }

        public static byte[] HexStringToByteArray(string s)
        {
            int len = s.Length;
            byte[] data = new byte[len / 2];
            for (int i = 0; i < len; i += 2)
            {
                data[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
            }
            return data;
        }

        public override int GetHashCode()
        {
            if (isNullToken)
                return 0;
            else
            {
                List<string> hashComponents = new List<string>();
                hashComponents.Add(tokenName);
                hashComponents.Add(startTime.ToString());
                hashComponents.Add(expiration.ToString());
                hashComponents.Add(duration.ToString());
                hashComponents.Add(ip);
                hashComponents.Add(acl);

                return hashComponents.GetHashCode();
            }
        }
    }
}
