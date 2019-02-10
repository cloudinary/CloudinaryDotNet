using System;
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
    /// <summary>
    /// The builbing blocks for generating an https delivery URL for assets.
    /// </summary>
    public class Url : Core.ICloneable
    {
        /// <summary>
        /// Recommended sources for video tag.
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

        /// <summary>
        /// Blank placeholder image that is displayed until the image is loaded.
        /// </summary>
        protected const string CL_BLANK = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";

        /// <summary>
        /// Default video source types.
        /// </summary>
        protected static readonly string[] DEFAULT_VIDEO_SOURCE_TYPES = { "webm", "mp4", "ogv" };

        /// <summary>
        /// Regular expression to match video source types extensions.
        /// </summary>
        protected static readonly Regex VIDEO_EXTENSION_RE = new Regex("\\.(" + String.Join("|", DEFAULT_VIDEO_SOURCE_TYPES) + ")$", RegexOptions.Compiled);

        /// <summary>
        /// Digital signature provider.
        /// </summary>
        protected ISignProvider m_signProvider;

        /// <summary>
        /// Authentication token for the token-based authentication.
        /// </summary>
        protected AuthToken m_AuthToken;

        /// <summary>
        /// The cloud name from your account details.
        /// </summary>
        protected string m_cloudName;

        /// <summary>
        /// The cloud url address to access the resources.
        /// </summary>
        protected string m_cloudinaryAddr = Api.ADDR_RES;

        /// <summary>
        /// Version of the cloudinary API.
        /// </summary>
        protected string m_apiVersion;

        /// <summary>
        /// Whether to use shorten url when possible.
        /// </summary>
        protected bool m_shorten;

        /// <summary>
        /// Force BuildImageTag to always use HTTPS URLs.
        /// </summary>
        protected bool m_secure;

        /// <summary>
        /// Set this parameter to true if you are an Advanced plan user with a private CDN distribution.
        /// </summary>
        protected bool m_usePrivateCdn;

        /// <summary>
        /// Set this parameter to true to include the signed part to the Url.
        /// </summary>
        protected bool m_signed;

        /// <summary>
        /// With Root Path URL feature set to true, the resource type and type parameters are set to the default values
        /// 'image' and 'upload' respectively, which means that any URL without the resource type and type parameters
        /// will automatically default to using those values.
        /// </summary>
        protected bool m_useRootPath;

        /// <summary>
        /// A descriptive suffix to add to the Public ID in the delivery Url.
        /// </summary>
        protected string m_suffix;

        /// <summary>
        /// Private CDN prefix to be added to the Url.
        /// </summary>
        protected string m_privateCdn;

        /// <summary>
        /// Version for your delivery URL to bypass the cached version on the CDN and force delivery of the latest
        /// resource.
        /// </summary>
        protected string m_version;

        /// <summary>
        /// Custom domain for your Url.
        /// </summary>
        protected string m_cName;

        /// <summary>
        /// Source part of the Url.
        /// </summary>
        protected string m_source;

        /// <summary>
        /// An HTML string to display in the case that the browser does not support any of the video formats included.
        /// </summary>
        protected string m_fallbackContent;

        /// <summary>
        /// Whether to use sub domain.
        /// </summary>
        protected bool m_useSubDomain;

        /// <summary>
        /// Set transformation to override the default transformation instructions for each specific video format.
        /// </summary>
        protected Dictionary<string, Transformation> m_sourceTransforms;

        /// <summary>
        /// Custom parts of the Url.
        /// </summary>
        protected List<string> m_customParts = new List<string>();

        /// <summary>
        /// Sources for video tag.
        /// </summary>
        protected VideoSource[] m_videoSources;

        /// <summary>
        /// The transformations to apply to the default image (you can include the public_id of an uploaded image to
        /// use instead of the default image).
        /// </summary>
        protected Transformation m_posterTransformation;

        /// <summary>
        /// The source of the image to be shown while the video is downloading or until the user hits the play button.
        /// </summary>
        protected string m_posterSource;

        /// <summary>
        /// A URI to an image to be shown while the video is downloading or until the user hits the play button.
        /// </summary>
        protected Url m_posterUrl;

        /// <summary>
        /// An ordered array of the video source types to include in the HTML5 tag, where the type is mapped to the
        /// mime type. Default: ['webm', 'mp4', 'ogv']
        /// </summary>
        protected string[] m_sourceTypes;

        /// <summary>
        /// The action to be added to the Url.
        /// </summary>
        protected string m_action = String.Empty;

        /// <summary>
        /// Type of the resource.
        /// </summary>
        protected string m_resourceType = String.Empty;

        /// <summary>
        /// The transformation to be added to the Url.
        /// </summary>
        protected Transformation m_transformation;

        /// <summary>
        /// Instantiates the <see cref="Url"/> object with cloud name.
        /// </summary>
        /// <param name="cloudName">The name of your cloud.</param>
        public Url(string cloudName)
        {
            m_cloudName = cloudName;
        }

        /// <summary>
        /// Instantiates the <see cref="Url"/> object with cloud name and sign provider.
        /// </summary>
        /// <param name="cloudName">The name of your cloud.</param>
        /// <param name="signProvider">Provider for signing parameters.</param>
        public Url(string cloudName, ISignProvider signProvider)
            : this(cloudName)
        {
            m_signProvider = signProvider;
        }

        /// <summary>
        /// File format of the requested resource.
        /// </summary>
        public string FormatValue { get; set; }

        /// <summary>
        /// The transformation to be added to the Url.
        /// </summary>
        public Transformation Transformation
        {
            get
            {
                if (m_transformation == null) m_transformation = new Transformation();
                return m_transformation;
            }
        }

        /// <summary>
        /// Set whether to use shorten Url when possible.
        /// </summary>
        /// <param name="shorten">True - to use shorten Url.</param>
        public Url Shorten(bool shorten)
        {
            m_shorten = shorten;
            return this;
        }

        /// <summary>
        /// Set the cloudinary Url to access the resources.
        /// </summary>
        /// <param name="cloudinaryAddr">Cloud Url.</param>
        public Url CloudinaryAddr(string cloudinaryAddr)
        {
            m_cloudinaryAddr = cloudinaryAddr;
            return this;
        }

        /// <summary>
        /// Set cloud name from your account details.
        /// </summary>
        /// <param name="cloudName">Cloud name.</param>
        public Url CloudName(string cloudName)
        {
            m_cloudName = cloudName;
            return this;
        }

        /// <summary>
        /// Add custom part to the Url.
        /// </summary>
        /// <param name="part">Custom Url part.</param>
        public Url Add(string part)
        {
            if (!String.IsNullOrEmpty(part))
                m_customParts.Add(part);

            return this;
        }

        /// <summary>
        /// Add sources for video tag.
        /// </summary>
        /// <param name="videoSources">Array of video sources.</param>
        public Url VideoSources(params VideoSource[] videoSources)
        {
            if (videoSources != null && videoSources.Length > 0)
                m_videoSources = videoSources;

            return this;
        }

        /// <summary>
        /// Add action to the Url.
        /// </summary>
        /// <param name="action">The action.</param>
        public Url Action(string action)
        {
            m_action = action;
            return this;
        }

        /// <summary>
        /// Set the version of the cloudinary API.
        /// </summary>
        /// <param name="apiVersion">API version.</param>
        public Url ApiVersion(string apiVersion)
        {
            m_apiVersion = apiVersion;
            return this;
        }

        /// <summary>
        /// Set version for your delivery URL to bypass the cached version on the CDN and force delivery of the latest
        /// resource.
        /// </summary>
        /// <param name="version">The version for your delivery URL.</param>
        public Url Version(string version)
        {
            m_version = version;
            return this;
        }

        /// <summary>
        /// Set authentication token for the token-based authentication.
        /// </summary>
        /// <param name="authToken">The authentication token.</param>
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

        /// <summary>
        /// An ordered array of the video source types to include in the HTML5 tag, where the type is mapped to the
        /// mime type. Default: ['webm', 'mp4', 'ogv']
        /// </summary>
        /// <param name="sourceTypes">An ordered array of the video source types.</param>
        public Url SourceTypes(params string[] sourceTypes)
        {
            m_sourceTypes = sourceTypes;
            return this;
        }

        /// <summary>
        /// When true - add signature part to the Url.
        /// </summary>
        /// <param name="signed">Whether to add signature to the Url.</param>
        public Url Signed(bool signed)
        {
            m_signed = signed;
            return this;
        }

        /// <summary>
        /// Type of the resource.
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public Url ResourceType(string resourceType)
        {
            m_resourceType = resourceType;
            return this;
        }

        /// <summary>
        /// Format of the resource file.
        /// </summary>
        /// <param name="format">File format.</param>
        public Url Format(string format)
        {
            FormatValue = format;
            return this;
        }

        /// <summary>
        /// Set private CDN prefix for the Url.
        /// </summary>
        /// <param name="privateCdn">The prefix of private CDN.</param>
        public Url SecureDistribution(string privateCdn)
        {
            m_privateCdn = privateCdn;
            return this;
        }

        /// <summary>
        /// Set custom domain for the Url.
        /// </summary>
        /// <param name="cName">Custom domain name.</param>
        public Url CName(string cName)
        {
            m_cName = cName;
            return this;
        }

        /// <summary>
        /// Set transformation for the Url.
        /// </summary>
        /// <param name="transformation">The transformation to be addded to the Url.</param>
        public Url Transform(Transformation transformation)
        {
            m_transformation = transformation;
            return this;
        }

        /// <summary>
        /// Force Url builder to use HTTPS urls. Default: true.
        /// </summary>
        /// <param name="secure">Whether to use HTTPS Url.</param>
        public Url Secure(bool secure = true)
        {
            m_secure = secure;
            return this;
        }

        /// <summary>
        /// Set wether to use a private CDN distribution.
        /// </summary>
        /// <param name="usePrivateCdn">Wether to use a private CDN distribution.</param>
        public Url PrivateCdn(bool usePrivateCdn)
        {
            m_usePrivateCdn = usePrivateCdn;
            return this;
        }

        /// <summary>
        /// Set whether to use sub domain.
        /// </summary>
        /// <param name="useSubDomain">Use sub domain.</param>
        public Url CSubDomain(bool useSubDomain)
        {
            m_useSubDomain = useSubDomain;
            return this;
        }

        /// <summary>
        /// Use the resource type and type parameters are set to the default values 'image' and 'upload' respectively, 
        /// which means that any URL without the resource type and type parameters.
        /// </summary>
        /// <param name="useRootPath">Whether to use root path.</param>
        public Url UseRootPath(bool useRootPath)
        {
            m_useRootPath = useRootPath;
            return this;
        }

        /// <summary>
        /// Set HTML string to display in the case that the browser does not support any of the video formats included.
        /// </summary>
        /// <param name="fallbackContent">Fallback content string.</param>
        public Url FallbackContent(string fallbackContent)
        {
            m_fallbackContent = fallbackContent;
            return this;
        }

        /// <summary>
        /// Set descriptive suffix to add to the Public ID in the delivery Url.
        /// </summary>
        /// <param name="suffix">The suffix.</param>
        public Url Suffix(string suffix)
        {
            m_suffix = suffix;
            return this;
        }

        /// <summary>
        /// Set transformation for the specific video format.
        /// </summary>
        /// <param name="source">Video source format.</param>
        /// <param name="transform">Transformation to override the default transformation instructions.</param>
        public Url SourceTransformationFor(string source, Transformation transform)
        {
            if (m_sourceTransforms == null)
                m_sourceTransforms = new Dictionary<string, Transformation>();

            m_sourceTransforms.Add(source, transform);

            return this;
        }

        /// <summary>
        /// Set the transformations to apply to the default image (you can include the public_id of an uploaded image to
        /// use instead of the default image).
        /// </summary>
        /// <param name="transformation">Transformation for the poster.</param>
        public Url PosterTransform(Transformation transformation)
        {
            m_posterTransformation = transformation;
            return this;
        }

        /// <summary>
        /// Set the source of the image to be shown while the video is downloading or until the user hits the play button.
        /// </summary>
        /// <param name="source">The source of the poster image.</param>
        public Url PosterSource(string source)
        {
            m_posterSource = source;
            return this;
        }

        /// <summary>
        /// Set an Url to an image to be shown while the video is downloading or until the user hits the play button.
        /// </summary>
        /// <param name="url">Url to an image.</param>
        public Url PosterUrl(Url url)
        {
            m_posterUrl = url;
            return this;
        }

        /// <summary>
        /// Set a poster to be shown while the video is downloading or until the user hits the play button.
        /// </summary>
        /// <param name="poster">
        /// Poster object. E.g. source string, transformation, Url or null to delete poster options.
        /// </param>
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

        /// <summary>
        /// Build an Url to sprite css file.
        /// </summary>
        /// <param name="source">A Cloudinary public ID or file name or a reference to a resource.</param>
        public string BuildSpriteCss(string source)
        {
            m_action = "sprite";
            if (!source.EndsWith(".css")) FormatValue = "css";
            return BuildUrl(source);
        }

        #region BuildImageTag

        /// <summary>
        /// Build an HTML image tag for embedding in a web view.
        /// </summary>
        /// <param name="source">A Cloudinary public ID or file name or a reference to a resource.</param>
        /// <param name="keyValuePairs">Array of strings in form of "key=value".</param>
        public string BuildImageTag(string source, params string[] keyValuePairs)
        {
            return BuildImageTag(source, new StringDictionary(keyValuePairs));
        }

        /// <summary>
        /// Build an HTML image tag for embedding in a web view.
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
        /// Helper method for BuildVideoTag, generates video mime type from sourceType and codecs.
        /// </summary>
        /// <param name="sourceType">The type of the source.</param>
        /// <param name="codecs">Codecs.</param>
        /// <returns>Resulting mime type.</returns>
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
        /// Helper method to merge transformation for the URL.
        /// </summary>
        /// <param name="url">The URL with transformation to be merged.</param>
        /// <param name="transformationSrc">Transformation to merge.</param>
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
        /// Helper method for BuildVideoTag, returns source tags from provided options.
        ///
        /// Source types and video sources are mutually exclusive, only one of them can be used.
        /// If both are not provided, default source types are used.
        /// </summary>
        ///
        /// <param name="source">The public ID of the video.</param>
        ///
        /// <returns>Resulting source tags (may be empty).</returns>
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

        /// <summary>
        /// Generate a transformation URL directly, without the containing image tag.
        /// </summary>
        /// <returns>The Url without image tag.</returns>
        public string BuildUrl()
        {
            return BuildUrl(null);
        }

        /// <summary>
        /// Generate a transformation URL directly, without the containing image tag.
        /// </summary>
        /// <param name="source">The source part of the Url.</param>
        /// <returns>The Url without image tag.</returns>
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

            if (src.SourceToSign.Contains("/") && !Regex.IsMatch(src.SourceToSign, "^v[0-9]+/") && !Regex.IsMatch(src.SourceToSign, "https?:/.*") && String.IsNullOrEmpty(m_version))
            {
                m_version = "1";
            }

            var version = String.IsNullOrEmpty(m_version) ? String.Empty : String.Format("v{0}", m_version);

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

    /// <summary>
    /// Provides a custom constructor for uniform resource identifiers (URIs) and modifies URIs 
    /// for the <see cref="Url"/> class.
    /// </summary>
    public class UrlBuilder : UriBuilder
    {
        private StringDictionary queryString = null;

        /// <summary>
        /// Gets the query information included in the Url.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a path to the resource referenced by the Url.
        /// </summary>
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

        /// <summary>
        /// Default parameterless constructor. Instantiates the <see cref="UrlBuilder"/> object.
        /// </summary>
        public UrlBuilder()
            : base()
        {
        }

        /// <summary>
        /// Instantiates the <see cref="UrlBuilder"/> object with the specified URI.
        /// </summary>
        /// <param name="uri">A URI string.</param>
        public UrlBuilder(string uri)
            : base(uri)
        {
            PopulateQueryString();
        }

        /// <summary>
        /// Instantiates the <see cref="UrlBuilder"/> object with the specified URI and dictionary with cloudinary 
        /// parameters.
        /// </summary>
        /// <param name="uri">A URI string.</param>
        /// <param name="params">Cloudinary parameters.</param>
        public UrlBuilder(string uri, IDictionary<string, object> @params)
            : base(uri)
        {
            PopulateQueryString();
            SetParameters(@params);
        }

        /// <summary>
        /// Instantiates the <see cref="UrlBuilder"/> object with the specified <see cref="Uri"/> instance.
        /// </summary>
        /// <param name="uri">An instance of the <see cref="Uri"/> class.</param>
        public UrlBuilder(Uri uri)
            : base(uri)
        {
            PopulateQueryString();
        }

        /// <summary>
        /// Instantiates the <see cref="UrlBuilder"/> object with the specified scheme and host.
        /// </summary>
        /// <param name="schemeName">An Internet access protocol.</param>
        /// <param name="hostName">A DNS-style domain name or IP address.</param>
        public UrlBuilder(string schemeName, string hostName)
            : base(schemeName, hostName)
        {
        }

        /// <summary>
        /// Instantiates the <see cref="UrlBuilder"/> object with the specified scheme, host, and port.
        /// </summary>
        /// <param name="scheme">An Internet access protocol.</param>
        /// <param name="host">A DNS-style domain name or IP address.</param>
        /// <param name="portNumber">An IP port number for the service.</param>
        public UrlBuilder(string scheme, string host, int portNumber)
            : base(scheme, host, portNumber)
        {
        }

        /// <summary>
        /// Instantiates the <see cref="UrlBuilder"/> object with the specified scheme, host, port number, and path.
        /// </summary>
        /// <param name="scheme">An Internet access protocol.</param>
        /// <param name="host">A DNS-style domain name or IP address.</param>
        /// <param name="port">An IP port number for the service.</param>
        /// <param name="pathValue">The path to the Internet resource.</param>
        public UrlBuilder(string scheme, string host, int port, string pathValue)
            : base(scheme, host, port, pathValue)
        {
        }

        /// <summary>
        /// Instantiates the <see cref="UrlBuilder"/> object with the specified scheme, host, port number, path and
        /// query string or fragment identifier.
        /// </summary>
        /// <param name="scheme">An Internet access protocol.</param>
        /// <param name="host">A DNS-style domain name or IP address.</param>
        /// <param name="port">An IP port number for the service.</param>
        /// <param name="path">The path to the Internet resource.</param>
        /// <param name="extraValue">A query string or fragment identifier.</param>
        public UrlBuilder(string scheme, string host, int port, string path, string extraValue)
            : base(scheme, host, port, path, extraValue)
        {
        }

        /// <summary>
        /// Set parameters for the Url to be added as query string.
        /// </summary>
        /// <param name="params">Cloudinary parameters.</param>
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

        /// <summary>
        /// Returns a string that represents the current Url.
        /// </summary>
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

    /// <summary>
    /// Source for video tag.
    /// </summary>
    public class VideoSource
    {
        /// <summary>
        /// One of the HTML5 video tag MIME types: video/mp4, video/webm, video/ogg.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// A single value, or a comma-separated list of values identifying the codec(s) that should be used to
        /// generate the video. The codec definition can include additional properties,separated with a dot. 
        /// For example, codecs="avc1.42E01E,mp4a.40.2"
        /// </summary>
        public string[] Codecs { get; set; }

        /// <summary>
        /// Transformation, applied to the <see cref="Type"/> in video tag.
        /// </summary>
        public Transformation Transformation { get; set; }
    }
}
