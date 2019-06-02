using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;


namespace CloudinaryDotNet
{
    /// <summary>
    /// Authentication token for the token-based authentication feature.
    /// Allows you to limit the validity of the image delivery URL.
    /// </summary>
    public class AuthToken
    {
        /// <summary>
        /// Name of the cloudinary cookie with token.
        /// </summary>
        public static string AUTH_TOKEN_NAME = "__cld_token__";

        /// <summary>
        /// Reqular expression pattern to match special characters in URL.
        /// </summary>
        public static string UNSAFE_RE = "[ \"#%&\\'\\/:;<=>?@\\[\\\\\\]^`{\\|}~]";

        /// <summary>
        /// Authentication token explicitly set to NULL.
        /// </summary>
        public static AuthToken NULL_AUTH_TOKEN = new AuthToken().SetNull();

        /// <summary>
        /// Name of the cookie token.
        /// </summary>
        public string tokenName = AUTH_TOKEN_NAME;

        /// <summary>
        /// The encryption key received from Cloudinary to sign token with.
        /// </summary>
        public string key;

        /// <summary>
        /// Timestamp in UNIX time when the cookie becomes valid. Default value: the current time.
        /// </summary>
        public long startTime;

        /// <summary>
        /// Timestamp in UNIX time when the cookie will expire.
        /// </summary>
        public long expiration;

        /// <summary>
        /// A specific IP Address that can access the authenticated images.
        /// </summary>
        public string ip;

        /// <summary>
        /// An Access Control List for limiting the allowed URL path (e.g., /image/authenticated/*).
        /// </summary>
        public string acl;

        /// <summary>
        /// Duration that the cookie is valid in seconds.
        /// </summary>
        public long duration;

        private bool isNullToken = false;

        /// <summary>
        /// Instantiates the <see cref="AuthToken"/> object.
        /// </summary>
        public AuthToken()
        {

        }

        /// <summary>
        /// Instantiates the <see cref="AuthToken"/> object.
        /// </summary>
        /// <param name="key">The encryption key received from Cloudinary to sign token with.</param>
        public AuthToken(string key)
        {
            this.key = key;
        }

        /// <summary>
        /// Set the Start time when the cookie becomes valid.
        /// </summary>
        /// <param name="startTime">Timestamp in UNIX time when the cookie becomes valid.</param>
        public AuthToken StartTime(long startTime)
        {
            this.startTime = startTime;
            return this;
        }

        /// <summary>
        /// Set the cookie expiration time.
        /// </summary>
        /// <param name="expiration">Timestamp in UNIX time when the cookie will expire.</param>
        public AuthToken Expiration(long expiration)
        {
            this.expiration = expiration;
            return this;
        }

        /// <summary>
        /// Set the IP for access the asset.
        /// </summary>
        /// <param name="ip">Only this IP address can access the resource.</param>
        public AuthToken Ip(string ip)
        {
            this.ip = ip;
            return this;
        }

        /// <summary>
        /// Set the Access Control List for limiting the allowed URL path to a specified pattern.
        /// </summary>
        /// <param name="acl">The pattern (e.g., /image/authenticated/*)</param>
        public AuthToken Acl(string acl)
        {
            this.acl = acl;
            return this;
        }

        /// <summary>
        /// Set the duration that the cookie is valid.
        /// </summary>
        /// <param name="duration">The duration that the cookie is valid in seconds.</param>
        public AuthToken Duration(long duration)
        {
            this.duration = duration;
            return this;
        }

        /// <summary>
        /// Generate authentication token.
        /// </summary>
        /// <returns>Generated authentication token.</returns>
        public string Generate()
        {
            return Generate(null);
        }

        /// <summary>
        /// Generate authentication token for the URL.
        /// </summary>
        /// <param name="url">URL to generate authentication token.</param>
        /// <returns>Generated authentication token.</returns>
        public string Generate(string url)
        {
            long expiration = this.expiration;
            if (expiration == 0)
            {
                if (duration > 0)
                {
                    long start = startTime > 0 ? startTime : Utils.UnixTimeNowSeconds();
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
                tokenParts.Add(string.Format("acl={0}", EscapeUrlToLower(acl)));
            }

            List<string> toSign = new List<string>(tokenParts);

            // Add URL only if ACL is not provided
            if (!string.IsNullOrWhiteSpace(url) && string.IsNullOrWhiteSpace(acl))
            {
                toSign.Add(string.Format("url={0}", EscapeUrlToLower(url)));
            }
            string auth = Digest(string.Join("~", toSign));
            tokenParts.Add(string.Format("hmac={0}", auth));

            return tokenName + "=" + string.Join("~", tokenParts);
        }

        /// <summary>
        /// Make a copy of the token.
        /// </summary>
        /// <returns>A new instance of the token.</returns>
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

        /// <summary>
        /// Check the equality of two tokens.
        /// </summary>
        /// <param name="o">The authentication token to compare.</param>
        /// <returns>True - if tokens are equal. Otherwise false.</returns>
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

        /// <summary>
        /// Encode and lowercase the URL.
        /// </summary>
        /// <param name="url">URL for escaping.</param>
        /// <returns>Escaped URL in lowercase.</returns>
        protected string EscapeUrlToLower(string url)
        {
            var r = new Regex(UNSAFE_RE, RegexOptions.Compiled | RegexOptions.RightToLeft);
            return r.Replace(url, m =>
            {
                var encodedItem = string.Join("", m.Value.Select(c => "%" + Convert.ToByte(c).ToString("x2")));
                return encodedItem.ToLower();
            });
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

        /// <summary>
        /// Convert hex string to the array of bytes.
        /// </summary>
        /// <param name="s">Hex string to convert.</param>
        /// <returns>An array of bytes.</returns>
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

        /// <summary>
        /// Compute a hashcode for the token.
        /// </summary>
        /// <returns>The hashcode for the token.</returns>
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
