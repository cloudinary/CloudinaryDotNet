﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#if NET40
using System.Web;
#endif

namespace CloudinaryDotNet
{
    public class Url : Core.ICloneable
    {
        /// <summary>
        /// Recommended sources for video tag
        /// </summary>
        public static readonly VideoSource[] DefaultVideoSources = {
            new VideoSource
            {
                Type = "mp4", Codecs = new[]{"hev1"}, Transformation = new Transformation().VideoCodec("h265")
            },
            new VideoSource
            {
                Type = "webm", Codecs = new[]{"vp9"}, Transformation = new Transformation().VideoCodec("vp9")
            },
            new VideoSource
            {
                Type = "mp4", Transformation = new Transformation().VideoCodec("auto")
            },
            new VideoSource
            {
                Type = "webm", Transformation = new Transformation().VideoCodec("auto")
            }
        };

        protected const string CL_BLANK = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";
        protected static readonly string[] DEFAULT_VIDEO_SOURCE_TYPES = { "webm", "mp4", "ogv" };
        protected static readonly Regex VIDEO_EXTENSION_RE = new Regex("\\.(" + String.Join("|", DEFAULT_VIDEO_SOURCE_TYPES) + ")$", RegexOptions.Compiled);

        protected ISignProvider m_signProvider;
        protected AuthToken m_AuthToken;

        protected string m_cloudName;
        protected string m_cloudinaryAddr = Api.ADDR_RES;
        protected string m_apiVersion;

        protected bool m_shorten;
        protected bool m_secure;
        protected bool m_usePrivateCdn;
        protected bool m_signed;
        protected bool m_useRootPath;
        protected string m_suffix;
        protected string m_privateCdn;
        protected string m_version;
        protected bool m_forceVersion;
        protected string m_cName;
        protected string m_source;
        protected string m_fallbackContent;
        protected bool m_useSubDomain;
        protected Dictionary<string, Transformation> m_sourceTransforms;
        protected List<string> m_customParts = new List<string>();
        protected VideoSource[] m_videoSources;
        protected Transformation m_posterTransformation;
        protected string m_posterSource;
        protected Url m_posterUrl;
        protected string[] m_sourceTypes;
        protected string m_action = String.Empty;
        protected string m_resourceType = String.Empty;
        protected Transformation m_transformation;

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

        public Transformation Transformation
        {
            get
            {
                if (m_transformation == null) m_transformation = new Transformation();
                return m_transformation;
            }
        }

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

        public Url VideoSources(params VideoSource[] videoSources)
        {
            if (videoSources != null && videoSources.Length > 0)
                m_videoSources = videoSources;

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

        /// <summary>
        /// Indicates whether to add '/v1/' to the URL when the public ID includes folders and a 'version' value was
        /// not defined.
        /// When no version is explicitly specified and the public id contains folders, a default v1 version
        /// is added to the url. Set this boolean as false to prevent that behaviour.
        /// </summary>
        /// <param name="forceVersion">A boolean parameter indicating whether or not to add the version.</param>
        /// <returns>Url</returns>
        public Url ForceVersion(bool forceVersion = true)
        {
            m_forceVersion = forceVersion;
            return this;
        }

        public Url AuthToken(AuthToken authToken)
        {
            if (m_AuthToken == null)
            {
                m_AuthToken = authToken;
            }

            return this;
        }

        /// <summary>
        /// A Cloudinary public ID or file name or a reference to a resource.
        /// </summary>
        public Url Source(string source)
        {
            m_source = source;
            return this;
        }

        public Url SourceTypes(params string[] sourceTypes)
        {
            m_sourceTypes = sourceTypes;
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

        public Url FallbackContent(string fallbackContent)
        {
            m_fallbackContent = fallbackContent;
            return this;
        }

        public Url Suffix(string suffix)
        {
            m_suffix = suffix;
            return this;
        }

        public Url SourceTransformationFor(string source, Transformation transform)
        {
            if (m_sourceTransforms == null)
                m_sourceTransforms = new Dictionary<string, Transformation>();

            m_sourceTransforms.Add(source, transform);

            return this;
        }

        public Url PosterTransform(Transformation transformation)
        {
            m_posterTransformation = transformation;
            return this;
        }

        public Url PosterSource(string source)
        {
            m_posterSource = source;
            return this;
        }

        public Url PosterUrl(Url url)
        {
            m_posterUrl = url;
            return this;
        }

        public Url Poster(object poster)
        {
            if (poster is string)
                return PosterSource((string)poster);
            else if (poster is Url)
                return PosterUrl((Url)poster);
            else if (poster is Transformation)
                return PosterTransform((Transformation)poster);
            else if (poster == null || (poster is bool && !(bool)poster))
            {
                PosterSource(String.Empty);
                PosterUrl(null);
                PosterTransform(null);
            }

            return this;
        }

        public string BuildSpriteCss(string source)
        {
            m_action = "sprite";
            if (!source.EndsWith(".css")) FormatValue = "css";
            return BuildUrl(source);
        }

        #region BuildImageTag

        /// <summary>
        /// Builds an image tag for embedding in a web view.
        /// </summary>
        /// <param name="source">A Cloudinary public ID or file name or a reference to a resource.</param>
        /// <param name="keyValuePairs">Array of strings in form of "key=value".</param>
        public string BuildImageTag(string source, params string[] keyValuePairs)
        {
            return BuildImageTag(source, new StringDictionary(keyValuePairs));
        }

        /// <summary>
        /// Builds an image tag for embedding in a web view.
        /// </summary>
        /// <param name="source">A Cloudinary public ID or file name or a reference to a resource.</param>
        /// <param name="dict">Additional parameters.</param>
        public string BuildImageTag(string source, StringDictionary dict = null)
        {
            if (dict == null)
                dict = new StringDictionary();

            string url = BuildUrl(source);

            if (!string.IsNullOrEmpty(Transformation.HtmlWidth))
                dict.Add("width", Transformation.HtmlWidth);

            if (!string.IsNullOrEmpty(Transformation.HtmlHeight))
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

            var sb = new StringBuilder();
            sb.Append("<img");
            if (!string.IsNullOrEmpty(url))
                sb.Append(" src=\"").Append(url).Append("\"");

            foreach (var item in dict)
            {
#if NET40
                sb.Append(" ").Append(item.Key).Append("=\"").Append(HttpUtility.HtmlAttributeEncode(item.Value)).Append("\"");
#else
                sb.Append(" ").Append(item.Key).Append("=\"").Append(System.Net.WebUtility.HtmlEncode(item.Value)).Append("\"");
#endif
            }

            sb.Append("/>");

            return sb.ToString();
        }

        #endregion

        #region BuildVideoTag

        /// <summary>
        /// Builds a video tag for embedding in a web view.
        /// </summary>
        /// <param name="source">A Cloudinary public ID or file name or a reference to a resource.</param>
        /// <param name="keyValuePairs">Array of strings in form of "key=value".</param>
        public string BuildVideoTag(string source, params string[] keyValuePairs)
        {
            return BuildVideoTag(source, new StringDictionary(keyValuePairs));
        }

        /// <summary>
        /// Builds a video tag for embedding in a web view.
        /// </summary>
        /// <param name="source">A Cloudinary public ID or file name or a reference to a resource.</param>
        /// <param name="dict">Additional parameters.</param>
        public string BuildVideoTag(string source, StringDictionary dict = null)
        {
            if (dict == null)
                dict = new StringDictionary();

            source = VIDEO_EXTENSION_RE.Replace(source, "", 1);

            if (string.IsNullOrEmpty(m_resourceType))
                m_resourceType = "video";

            string posterUrl = FinalizePosterUrl(source);

            if (!string.IsNullOrEmpty(posterUrl))
                dict.Add("poster", posterUrl);

            List<string> tags = GetVideoSourceTags(source);

            var sb = new StringBuilder("<video");

            bool multiSource = tags.Count > 1;
            if (multiSource)
            {
                BuildUrl(source);
            }
            else
            {
                var sourceTypes = GetSourceTypes();
                string url = BuildUrl(source + "." + sourceTypes[0]);
                dict.Add("src", url);
            }

            if (dict.ContainsKey("html_height"))
                dict["height"] = dict.Remove("html_height");
            else if (Transformation.HtmlHeight != null)
                dict["height"] = Transformation.HtmlHeight;

            if (dict.ContainsKey("html_width"))
                dict["width"] = dict.Remove("html_width");
            else if (Transformation.HtmlWidth != null)
                dict["width"] = Transformation.HtmlWidth;

            bool wasSorted = dict.Sort;
            dict.Sort = true;
            foreach (var item in dict)
            {
                sb.Append(" ").Append(item.Key);
                if (item.Value != null)
                    sb.Append("='").Append(item.Value).Append("'");
            }
            dict.Sort = wasSorted;

            sb.Append(">");

            if (multiSource)
            {
                tags.ForEach(t => sb.Append(t));
            }

            if (!string.IsNullOrEmpty(m_fallbackContent))
            {
                sb.Append(m_fallbackContent);
            }

            sb.Append("</video>");
            return sb.ToString();
        }

        private string[] GetSourceTypes()
        {
            if (m_sourceTypes != null && m_sourceTypes.Length > 0)
            {
                return m_sourceTypes;
            }
            return DEFAULT_VIDEO_SOURCE_TYPES;
        }

        /// <summary>
        /// Helper method for BuildVideoTag, generates video mime type from sourceType and codecs
        /// </summary>
        /// <param name="sourceType">The type of the source</param>
        /// <param name="codecs">Codecs</param>
        /// <returns>Resulting mime type</returns>
        private static string VideoMimeType(string sourceType, params string[] codecs)
        {
            sourceType = sourceType == "ogv" ? "ogg" : sourceType;

            if (string.IsNullOrEmpty(sourceType))
            {
                return string.Empty;
            }

            if (codecs == null || codecs.Length == 0)
            {
                return $"video/{sourceType}";
            }

            var codecsJoined = string.Join(", ", codecs.Where(c => !string.IsNullOrEmpty(c)));
            var codecsStr = !string.IsNullOrEmpty(codecsJoined) ? $"; codecs={codecsJoined}" : string.Empty;

            return $"video/{sourceType}{codecsStr}";
        }

        private static void AppendTransformation(Url url, Transformation transform)
        {
            if (url.m_transformation == null)
            {
                url.Transform(transform);
            }
            else
            {
                url.m_transformation.Chain();
                transform.NestedTransforms.AddRange(url.m_transformation.NestedTransforms);
                url.Transform(transform);
            }
        }

        /// <summary>
        /// Helper method to merge transformation for the URL
        /// </summary>
        /// <param name="url">The URL with transformation to be merged</param>
        /// <param name="transformationSrc">Transformation to merge</param>
        private static void MergeUrlTransformation(Url url, Transformation transformationSrc)
        {
            if (transformationSrc == null)
            {
                return;
            }

            if (url.m_transformation == null)
            {
                url.Transform(transformationSrc);
            }
            else
            {
                foreach (var param in transformationSrc.Params)
                {
                    url.m_transformation.Add(param.Key, param.Value);
                }
            }
        }

        /// <summary>
        /// Helper method for BuildVideoTag, returns source tags from provided options
        ///
        /// Source types and video sources are mutually exclusive, only one of them can be used.
        /// If both are not provided, default source types are used
        /// </summary>
        ///
        /// <param name="source">The public ID of the video</param>
        ///
        /// <returns>Resulting source tags (may be empty)</returns>
        private List<string> GetVideoSourceTags(string source)
        {
            if (m_videoSources != null && m_videoSources.Length > 0)
            {
                return m_videoSources.Select(x => GetSourceTag(source, x.Type, x.Codecs, x.Transformation)).ToList();
            }

            return GetSourceTypes().Select(x => GetSourceTag(source, x)).ToList();
        }

        private string GetSourceTag(string source, string sourceType,
            string[] codecs = null, Transformation transformation = null)
        {
            var sourceUrl = Clone();
            MergeUrlTransformation(sourceUrl, transformation);

            if (m_sourceTransforms != null)
            {
                if (m_sourceTransforms.TryGetValue(sourceType, out var sourceTransformation) &&
                    sourceTransformation != null)
                {
                    AppendTransformation(sourceUrl, sourceTransformation.Clone());
                }
            }

            var src = sourceUrl.Format(sourceType).BuildUrl(source);

            return $"<source src='{src}' type='{VideoMimeType(sourceType, codecs)}'>";
        }

        private string FinalizePosterUrl(string source)
        {
            string posterUrl = null;

            if (m_posterUrl != null)
            {
                posterUrl = m_posterUrl.BuildUrl();
            }
            else if (m_posterTransformation != null)
            {
                posterUrl = Clone().Format("jpg").Transform(m_posterTransformation.Clone()).BuildUrl(source);
            }
            else if (m_posterSource != null)
            {
                if (!String.IsNullOrEmpty(m_posterSource))
                    posterUrl = Clone().Format("jpg").BuildUrl(m_posterSource);
            }
            else
            {
                posterUrl = Clone().Format("jpg").BuildUrl(source);
            }

            return posterUrl;
        }

        #endregion

        #region BuildUrl

        public string BuildUrl()
        {
            return BuildUrl(null);
        }

        public string BuildUrl(string source)
        {
            if (String.IsNullOrEmpty(m_cloudName))
                throw new ArgumentException("cloudName must be specified!");

            if (source == null)
                source = m_source;

            if (source == null)
                source = String.Empty;

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

            if (m_forceVersion &&
                src.SourceToSign.Contains("/") && !Regex.IsMatch(src.SourceToSign, "^v[0-9]+/") &&
                !Regex.IsMatch(src.SourceToSign, "https?:/.*") && string.IsNullOrEmpty(m_version))
            {
                m_version = "1";
            }

            var version = string.IsNullOrEmpty(m_version) ? string.Empty : $"v{m_version}";

            if (m_signed && (m_AuthToken == null && CloudinaryConfiguration.AuthToken == null))
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

            if (m_signed && (m_AuthToken != null || CloudinaryConfiguration.AuthToken != null))
            {
                AuthToken token = m_AuthToken != null ? m_AuthToken : (CloudinaryConfiguration.AuthToken != null ? CloudinaryConfiguration.AuthToken : null);

                if (token != null && !Equals(token, CloudinaryDotNet.AuthToken.NULL_AUTH_TOKEN))
                {
                    var path = new Uri(uriStr).AbsolutePath;
                    var tokenStr = token.Generate(path);
                    uriStr = $"{uriStr}?{tokenStr}";
                }
            }

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
                if (String.IsNullOrEmpty(privateCdn) || Constants.OLD_AKAMAI_SHARED_CDN == privateCdn)
                {
                    privateCdn = m_usePrivateCdn ? m_cloudName + "-res.cloudinary.com" : Constants.SHARED_CDN;
                }
                sharedDomain |= privateCdn == Constants.SHARED_CDN;

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
                else if (m_resourceType == "image" && m_action == "private")
                {
                    m_resourceType = "private_images";
                    m_action = null;
                }
                else if (m_resourceType == "image" && m_action == "authenticated")
                {
                    m_resourceType = "authenticated_images";
                    m_action = null;
                }
                else if (m_resourceType == "video" && m_action == "upload")
                {
                    m_resourceType = "videos";
                    m_action = null;
                }
                else if (m_resourceType == "raw" && m_action == "upload")
                {
                    m_resourceType = "files";
                    m_action = null;
                }
                else
                {
                    throw new NotSupportedException("URL Suffix only supported for image/upload, image/private, " +
                                                    "image/authenticated, video/upload and raw/upload");
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

        #endregion

        #region ICloneable

        /// <summary>
        /// Creates a new object that is a deep copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a deep copy of this instance.
        /// </returns>
        public Url Clone()
        {
            Url newUrl = (Url)this.MemberwiseClone();

            if (m_transformation != null)
                newUrl.m_transformation = this.m_transformation.Clone();

            if (m_posterTransformation != null)
                newUrl.m_posterTransformation = m_posterTransformation.Clone();

            if (m_posterUrl != null)
                newUrl.m_posterUrl = m_posterUrl.Clone();

            if (m_sourceTypes != null)
            {
                newUrl.m_sourceTypes = new string[m_sourceTypes.Length];
                Array.Copy(m_sourceTypes, newUrl.m_sourceTypes, m_sourceTypes.Length);
            }

            if (m_sourceTransforms != null)
            {
                newUrl.m_sourceTransforms = new Dictionary<string, Transformation>();
                foreach (var item in m_sourceTransforms)
                {
                    newUrl.m_sourceTransforms.Add(item.Key, item.Value.Clone());
                }
            }

            newUrl.m_customParts = new List<string>(m_customParts);

            return newUrl;
        }

        /// <summary>
        /// Creates a new object that is a deep copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a deep copy of this instance.
        /// </returns>
        object CloudinaryDotNet.Core.ICloneable.Clone()
        {
            return Clone();
        }

        #endregion
    }

    public class UrlBuilder : UriBuilder
    {
        private StringDictionary queryString = null;

        public StringDictionary QueryString
        {
            get
            {
                if (queryString == null)
                {
                    queryString = new StringDictionary();
                }

                return queryString;
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

        private void PopulateQueryString()
        {
            string query = base.Query;

            if (query == string.Empty || query == null)
            {
                return;
            }

            if (queryString == null)
            {
                queryString = new StringDictionary();
            }

            queryString.Clear();

            query = query.Substring(1); //remove the ?

            string[] pairs = query.Split(new char[] { '&' });
            foreach (string s in pairs)
            {
                string[] pair = s.Split(new char[] { '=' });

                queryString[pair[0]] = (pair.Length > 1) ? pair[1] : string.Empty;
            }
        }

        private void BuildQueryString()
        {
            if (queryString == null) return;

            int count = queryString.Count;

            if (count == 0)
            {
                base.Query = string.Empty;
                return;
            }

            string[] keys = new string[count];
            string[] values = new string[count];
            string[] pairs = new string[count];

            queryString.Keys.CopyTo(keys, 0);
            queryString.Values.CopyTo(values, 0);

            for (int i = 0; i < count; i++)
            {
                pairs[i] = string.Concat(keys[i], "=", values[i]);
            }

            base.Query = string.Join("&", pairs);
        }
    }

    public class VideoSource
    {
        public string Type { get; set; }
        public string[] Codecs { get; set; }
        public Transformation Transformation { get; set; }
    }
}
