using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CloudinaryDotNet
{
    public class Url : ICloneable
    {
        const string CL_BLANK = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";

        ISignProvider m_signProvider;

        string m_cloudName;
        string m_cloudinaryAddr = Api.ADDR_RES;
        string m_apiVersion;

        bool m_shorten;
        bool m_secure;
        bool m_usePrivateCdn;
        bool m_signed;
        bool m_useRootPath;
        string m_suffix;
        string m_privateCdn;
        string m_version;
        string m_cName;
        bool m_useSubDomain;

        List<string> m_customParts = new List<string>();

        string m_action = String.Empty;
        string m_resourceType = String.Empty;

        Transformation m_transformation = null;

        public Url(string cloudName)
        {
            m_cloudName = cloudName;
        }

        public Url(string cloudName, ISignProvider signProvider)
            : this(cloudName)
        {
            m_signProvider = signProvider;
        }

        public string FormatValue { get; set; }

        public Url Shorten(bool shorten)
        {
            m_shorten = shorten;
            return this;
        }

        public Url CloudinaryAddr(string cloudinaryAddr)
        {
            m_cloudinaryAddr = cloudinaryAddr;
            return this;
        }

        public Url CloudName(string cloudName)
        {
            m_cloudName = cloudName;
            return this;
        }

        public Url Add(string part)
        {
            if (!String.IsNullOrEmpty(part))
                m_customParts.Add(part);

            return this;
        }

        public Url Action(string action)
        {
            m_action = action;
            return this;
        }

        public Url ApiVersion(string apiVersion)
        {
            m_apiVersion = apiVersion;
            return this;
        }

        public Url Version(string version)
        {
            m_version = version;
            return this;
        }

        public Url Signed(bool signed)
        {
            m_signed = signed;
            return this;
        }

        public Url ResourceType(string resourceType)
        {
            m_resourceType = resourceType;
            return this;
        }

        public Url Format(string format)
        {
            FormatValue = format;
            return this;
        }

        public Url SecureDistribution(string privateCdn)
        {
            m_privateCdn = privateCdn;
            return this;
        }

        public Url CName(string cName)
        {
            m_cName = cName;
            return this;
        }

        public Url Transform(Transformation transformation)
        {
            m_transformation = transformation;
            return this;
        }

        public Url Secure(bool secure = true)
        {
            m_secure = secure;
            return this;
        }

        public Url PrivateCdn(bool usePrivateCdn)
        {
            m_usePrivateCdn = usePrivateCdn;
            return this;
        }

        public Url CSubDomain(bool useSubDomain)
        {
            m_useSubDomain = useSubDomain;
            return this;
        }

        public Url UseRootPath(bool useRootPath)
        {
            m_useRootPath = useRootPath;
            return this;
        }

        public Url Suffix(string suffix)
        {
            m_suffix = suffix;
            return this;
        }

        public Transformation Transformation
        {
            get
            {
                if (m_transformation == null) m_transformation = new Transformation();
                return m_transformation;
            }
        }

        public string BuildSpriteCss(string source)
        {
            m_action = "sprite";
            if (!source.EndsWith(".css")) FormatValue = "css";
            return BuildUrl(source);
        }

#if NET40
        public IHtmlString BuildImageTag(string source, params string[] keyValuePairs)
#else
        public string BuildImageTag(string source, params string[] keyValuePairs)
#endif
        {
            return BuildImageTag(source, new StringDictionary(keyValuePairs));
        }

#if NET40
        public IHtmlString BuildImageTag(string source, StringDictionary dict = null)
#else
        public string BuildImageTag(string source, StringDictionary dict)
#endif
        {
            if (source == null)
                throw new ArgumentException("source must not be null!");

            if (dict == null)
                dict = new StringDictionary();

            string url = BuildUrl(source);

            if (!String.IsNullOrEmpty(Transformation.HtmlWidth))
                dict.Add("width", Transformation.HtmlWidth);

            if (!String.IsNullOrEmpty(Transformation.HtmlHeight))
                dict.Add("height", Transformation.HtmlHeight);

            if (Transformation.HiDpi || Transformation.IsResponsive)
            {
                var extraClass = Transformation.IsResponsive ? "cld-responsive" : "cld-hidpi";
                var userClass = dict["class"];
                dict["class"] = userClass == null ? extraClass : userClass + " " + extraClass;
                dict.Add("data-src", url);
                var responsivePlaceholder = dict.Remove("responsive_placeholder");
                if (responsivePlaceholder == "blank")
                    responsivePlaceholder = CL_BLANK;
                url = responsivePlaceholder;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<img");
            if (!String.IsNullOrEmpty(url))
                sb.Append(" src='").Append(url).Append("'");

            foreach (var item in dict)
            {
                sb.Append(" ").Append(item.Key).Append("='").Append(item.Value).Append("'");
            }

            sb.Append("/>");

#if NET40
            return new HtmlString(sb.ToString());
#else
            return sb.ToString();
#endif
        }

        public string BuildUrl()
        {
            return BuildUrl(String.Empty);
        }

        public string BuildUrl(string source)
        {
            if (String.IsNullOrEmpty(m_cloudName))
                throw new ArgumentException("cloudName must be specified!");

            if (!m_usePrivateCdn && m_useRootPath)
                throw new NotSupportedException("Root path only supported in private CDN!");

            if (!m_usePrivateCdn && !String.IsNullOrEmpty(m_suffix))
                throw new NotSupportedException("URL Suffix only supported in private CDN!");

            if (source == null) return null;

            if (Regex.IsMatch(source.ToLower(), "^https?:/.*") &&
                (m_action == "upload" || m_action == "asset"))
            {
                return source;
            }

            if (m_action == "fetch" && !String.IsNullOrEmpty(FormatValue))
            {
                Transformation.FetchFormat(FormatValue);
                FormatValue = null;
            }

            string transformationStr = Transformation.Generate();

            var src = UpdateSource(source);

            bool sharedDomain;
            var prefix = GetPrefix(src.Source, out sharedDomain);

            List<string> urlParts = new List<string>(new string[] { prefix });
            if (!String.IsNullOrEmpty(m_apiVersion))
            {
                urlParts.Add(m_apiVersion);
                urlParts.Add(m_cloudName);
            }
            else if (sharedDomain)
            {
                urlParts.Add(m_cloudName);
            }

            UpdateAction();

            urlParts.Add(m_resourceType);
            urlParts.Add(m_action);
            urlParts.AddRange(m_customParts);

            if (src.SourceToSign.Contains("/") && !Regex.IsMatch(src.SourceToSign, "^v[0-9]+/") && !Regex.IsMatch(src.SourceToSign, "https?:/.*") && String.IsNullOrEmpty(m_version))
            {
                m_version = "1";
            }

            var version = String.IsNullOrEmpty(m_version) ? String.Empty : String.Format("v{0}", m_version);

            if (m_signed)
            {
                if (m_signProvider == null)
                    throw new NullReferenceException("Reference to ISignProvider-compatible object must be provided in order to sign URI!");

                var signedPart = String.Join("/", new string[] { transformationStr, src.SourceToSign });
                signedPart = Regex.Replace(signedPart, "^/+", String.Empty);
                signedPart = Regex.Replace(signedPart, "([^:])/{2,}", "$1/");
                signedPart = Regex.Replace(signedPart, "/$", String.Empty);

                signedPart = m_signProvider.SignUriPart(signedPart);
                urlParts.Add(signedPart);
            }

            urlParts.Add(transformationStr);
            urlParts.Add(version);
            urlParts.Add(src.Source);

            string uriStr = String.Join("/", urlParts.ToArray());
            uriStr = Regex.Replace(uriStr, "([^:])/{2,}", "$1/");
            uriStr = Regex.Replace(uriStr, "/$", String.Empty);

            return uriStr;
        }

        private CSource UpdateSource(string source)
        {
            CSource src = null;

            if (Regex.IsMatch(source.ToLower(), "^https?:/.*"))
            {
                src = new CSource(Encode(source));
            }
            else
            {
                src = new CSource(Encode(Decode(source)));

                if (!String.IsNullOrEmpty(m_suffix))
                {
                    if (Regex.IsMatch(m_suffix, "[\\./]"))
                        throw new ArgumentException("Suffix should not include . or /!");

                    src.Source += "/" + m_suffix;
                }

                if (!String.IsNullOrEmpty(FormatValue))
                {
                    src += "." + FormatValue;
                }
            }

            return src;
        }

        private string GetPrefix(string source, out bool sharedDomain)
        {
            string prefix;
            sharedDomain = !m_usePrivateCdn;
            string privateCdn = m_privateCdn;
            if (m_secure)
            {
                if (String.IsNullOrEmpty(privateCdn) || Cloudinary.OLD_AKAMAI_SHARED_CDN == privateCdn)
                {
                    privateCdn = m_usePrivateCdn ? m_cloudName + "-res.cloudinary.com" : Cloudinary.SHARED_CDN;
                }
                sharedDomain |= privateCdn == Cloudinary.SHARED_CDN;

                if (sharedDomain && m_useSubDomain)
                    privateCdn = privateCdn.Replace(
                        "res.cloudinary.com",
                        "res-" + Shard(source) + ".cloudinary.com");

                prefix = String.Format("https://{0}", privateCdn);
            }
            else
            {
                if (Regex.IsMatch(m_cloudinaryAddr.ToLower(), "^https?:/.*"))
                {
                    prefix = m_cloudinaryAddr;
                }
                else if (m_cName != null)
                {
                    string subDomain = m_useSubDomain ? "a" + Shard(source) + "." : String.Empty;
                    prefix = "http://" + subDomain + m_cName;
                }
                else
                {
                    string subDomain = m_useSubDomain ? "-" + Shard(source) : String.Empty;
                    string host = (m_usePrivateCdn ? m_cloudName + "-" : String.Empty) + "res" + subDomain + ".cloudinary.com";

                    prefix = "http://" + host;
                }
            }

            return prefix;
        }

        private void UpdateAction()
        {
            if (!String.IsNullOrEmpty(m_suffix))
            {
                if (m_resourceType == "image" && m_action == "upload")
                {
                    m_resourceType = "images";
                    m_action = null;
                }
                else if (m_resourceType == "raw" && m_action == "upload")
                {
                    m_resourceType = "files";
                    m_action = null;
                }
                else
                {
                    throw new NotSupportedException("URL Suffix only supported for image/upload and raw/upload!");
                }
            }

            if (m_useRootPath)
            {
                if (m_resourceType == "image" && m_action == "upload"
                    || m_resourceType == "images" && String.IsNullOrEmpty(m_action))
                {
                    m_resourceType = String.Empty;
                    m_action = String.Empty;
                }
                else
                {
                    throw new NotSupportedException("Root path only supported for image/upload!");
                }
            }

            if (m_shorten && m_resourceType == "image" && m_action == "upload")
            {
                m_resourceType = String.Empty;
                m_action = "iu";
            }
        }

        private static string Shard(string input)
        {
            uint hash = Crc32.ComputeChecksum(Encoding.UTF8.GetBytes(input));
            return ((hash % 5 + 5) % 5 + 1).ToString();
        }

        private static string Decode(string input)
        {
            StringBuilder resultStr = new StringBuilder(input.Length);

            int pos = 0;

            while (pos < input.Length)
            {
                int ppos = input.IndexOf('%', pos);
                if (ppos == -1)
                {
                    resultStr.Append(input.Substring(pos));
                    pos = input.Length;
                }
                else
                {
                    resultStr.Append(input.Substring(pos, ppos - pos));
                    char ch = (char)Int16.Parse(input.Substring(ppos + 1, 2), NumberStyles.HexNumber);
                    resultStr.Append(ch);
                    pos = ppos + 3;
                }
            }

            return resultStr.ToString();
        }

        private static string Encode(string input)
        {
            StringBuilder resultStr = new StringBuilder(input.Length);
            foreach (char ch in input)
            {
                if (!IsSafe(ch))
                {
                    resultStr.Append('%');
                    resultStr.Append(String.Format("{0:X2}", (short)ch));
                }
                else
                {
                    resultStr.Append(ch);
                }
            }
            return resultStr.ToString();
        }

        private static bool IsSafe(char ch)
        {
            if (ch >= 0x30 && ch <= 0x39) return true; // 0-9
            if (ch >= 0x41 && ch <= 0x5a) return true; // A-Z
            if (ch >= 0x61 && ch <= 0x7a) return true; // a-z

            return "/:-_.*".IndexOf(ch) >= 0;
        }

        #region ICloneable

        public Url Clone()
        {
            Url newUrl = (Url)this.MemberwiseClone();

            if (m_transformation != null)
                newUrl.m_transformation = this.m_transformation.Clone();
            else
                newUrl.m_transformation = null;

            newUrl.m_customParts = new List<string>();
            foreach (var part in m_customParts)
            {
                newUrl.m_customParts.Add(part);
            }

            return newUrl;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion
    }

    public class UrlBuilder : UriBuilder
    {
        StringDictionary m_queryString = null;

        public StringDictionary QueryString
        {
            get
            {
                if (m_queryString == null)
                {
                    m_queryString = new StringDictionary();
                }

                return m_queryString;
            }
        }

        public string PageName
        {
            get
            {
                string path = base.Path;
                return path.Substring(path.LastIndexOf("/") + 1);
            }
            set
            {
                string path = base.Path;
                path = path.Substring(0, path.LastIndexOf("/"));
                base.Path = string.Concat(path, "/", value);
            }
        }

        public UrlBuilder()
            : base()
        {
        }

        public UrlBuilder(string uri)
            : base(uri)
        {
            PopulateQueryString();
        }

        public UrlBuilder(string uri, IDictionary<string, object> @params)
            : base(uri)
        {
            PopulateQueryString();
            SetParameters(@params);
        }

        public UrlBuilder(Uri uri)
            : base(uri)
        {
            PopulateQueryString();
        }

        public UrlBuilder(string schemeName, string hostName)
            : base(schemeName, hostName)
        {
        }

        public UrlBuilder(string scheme, string host, int portNumber)
            : base(scheme, host, portNumber)
        {
        }

        public UrlBuilder(string scheme, string host, int port, string pathValue)
            : base(scheme, host, port, pathValue)
        {
        }

        public UrlBuilder(string scheme, string host, int port, string path, string extraValue)
            : base(scheme, host, port, path, extraValue)
        {
        }

        public UrlBuilder(System.Web.UI.Page page)
            : base(page.Request.Url.AbsoluteUri)
        {
            PopulateQueryString();
        }

        public void SetParameters(IDictionary<string, object> @params)
        {
            foreach (var param in @params)
            {
                if (param.Value is IEnumerable<string>)
                {
                    foreach (var s in (IEnumerable<string>)param.Value)
                    {
                        QueryString.Add(param.Key + "[]", s);
                    }
                }
                else
                {
                    QueryString[param.Key] = param.Value.ToString();
                }
            }
        }

        public new string ToString()
        {
            BuildQueryString();

            return base.Uri.AbsoluteUri;
        }

        public void Navigate()
        {
            _Navigate(true);
        }

        public void Navigate(bool endResponse)
        {
            _Navigate(endResponse);
        }

        private void _Navigate(bool endResponse)
        {
            string uri = this.ToString();
            HttpContext.Current.Response.Redirect(uri, endResponse);
        }

        private void PopulateQueryString()
        {
            string query = base.Query;

            if (query == string.Empty || query == null)
            {
                return;
            }

            if (m_queryString == null)
            {
                m_queryString = new StringDictionary();
            }

            m_queryString.Clear();

            query = query.Substring(1); //remove the ?

            string[] pairs = query.Split(new char[] { '&' });
            foreach (string s in pairs)
            {
                string[] pair = s.Split(new char[] { '=' });

                m_queryString[pair[0]] = (pair.Length > 1) ? pair[1] : string.Empty;
            }
        }

        private void BuildQueryString()
        {
            if (m_queryString == null) return;

            int count = m_queryString.Count;

            if (count == 0)
            {
                base.Query = string.Empty;
                return;
            }

            string[] keys = new string[count];
            string[] values = new string[count];
            string[] pairs = new string[count];

            m_queryString.Keys.CopyTo(keys, 0);
            m_queryString.Values.CopyTo(values, 0);

            for (int i = 0; i < count; i++)
            {
                pairs[i] = string.Concat(keys[i], "=", values[i]);
            }

            base.Query = string.Join("&", pairs);
        }
    }

    public static class Crc32
    {
        static uint[] table;

        public static uint ComputeChecksum(byte[] bytes)
        {
            uint crc = 0xffffffff;
            for (int i = 0; i < bytes.Length; ++i)
            {
                byte index = (byte)(((crc) & 0xff) ^ bytes[i]);
                crc = (uint)((crc >> 8) ^ table[index]);
            }
            return ~crc;
        }

        public static byte[] ComputeChecksumBytes(byte[] bytes)
        {
            return BitConverter.GetBytes(ComputeChecksum(bytes));
        }

        static Crc32()
        {
            uint poly = 0xedb88320;
            table = new uint[256];
            uint temp = 0;
            for (uint i = 0; i < table.Length; ++i)
            {
                temp = i;
                for (int j = 8; j > 0; --j)
                {
                    if ((temp & 1) == 1)
                    {
                        temp = (uint)((temp >> 1) ^ poly);
                    }
                    else
                    {
                        temp >>= 1;
                    }
                }
                table[i] = temp;
            }
        }
    }

    class CSource
    {
        public CSource(string source)
        {
            SourceToSign = Source = source;
        }

        public static CSource operator +(CSource src, string value)
        {
            src.Source += value;
            src.SourceToSign += value;

            return src;
        }

        public string Source;
        public string SourceToSign;
    }
}
