namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;
    using Newtonsoft.Json;

    /// <summary>
    /// HTTP method.
    /// </summary>
    public enum HttpMethod
    {
        /// <summary>
        /// DELETE
        /// </summary>
        DELETE,

        /// <summary>
        /// GET
        /// </summary>
        GET,

        /// <summary>
        /// POST
        /// </summary>
        POST,

        /// <summary>
        /// PUT
        /// </summary>
        PUT,
    }

    /// <summary>
    /// Provider for the API calls.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    [SuppressMessage("Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Reviewed.")]
    public partial class ApiShared : ISignProvider
    {
        /// <summary>
        /// URL of the cloudinary API.
        /// </summary>
        public const string ADDR_API = "api.cloudinary.com";

        /// <summary>
        /// URL of the cloudinary shared CDN.
        /// </summary>
        public const string ADDR_RES = "res.cloudinary.com";

        /// <summary>
        /// Version of the API.
        /// </summary>
        public const string API_VERSION = "v1_1";

        /// <summary>
        /// The boundary string to split form-data content.
        /// </summary>
        public const string HTTP_BOUNDARY = "notrandomsequencetouseasboundary";

        /// <summary>
        /// User agent for cloudinary API requests.
        /// </summary>
        public static string USER_AGENT = RuntimeInformation.FrameworkDescription;

        /// <summary>
        /// Whether to use a sub domain.
        /// </summary>
        public bool CSubDomain;

        /// <summary>
        /// Whether to use a shortened URL when possible.
        /// </summary>
        public bool ShortenUrl;

        /// <summary>
        /// Whether to use root path.
        /// </summary>
        public bool UseRootPath;

        /// <summary>
        /// Set this parameter to true if you are an Advanced plan user with a private CDN distribution.
        /// </summary>
        public bool UsePrivateCdn;

        /// <summary>
        /// Whether to use secure URL.
        /// </summary>
        public bool Secure;

        /// <summary>
        /// The private CDN prefix for the URL.
        /// </summary>
        public string PrivateCdn;

        /// <summary>
        /// The descriptive suffix to add to the Public ID in the delivery URL.
        /// </summary>
        public string Suffix;

        /// <summary>
        /// User platform information.
        /// </summary>
        public string UserPlatform;

        /// <summary>
        /// Timeout for the API requests, milliseconds.
        /// </summary>
        public int Timeout;

        /// <summary>
        /// Indicates whether to add '/v1/' to the URL when the public ID includes folders and a 'version' value was
        /// not defined.
        /// When no version is explicitly specified and the public id contains folders, a default v1 version
        /// is added to the URL. Set this boolean as false to prevent that behaviour.
        /// </summary>
        public bool ForceVersion = true;

        /// <summary>
        /// Sets whether to use the use chunked encoding. See http://www.w3.org/Protocols/rfc2616/rfc2616-sec3.html#sec3.6.1 for further info.
        /// Server must support HTTP/1.1 in order to use the chunked encoding.
        /// </summary>
        public bool UseChunkedEncoding = true;

        /// <summary>
        /// Maximum size of chunk when uploading a file.
        /// </summary>
        public int ChunkSize = 65000;

        /// <summary>
        /// Defines authentication signature algorithm.
        /// </summary>
        public SignatureAlgorithm SignatureAlgorithm = SignatureAlgorithm.SHA1;

        /// <summary>
        /// URL of the cloudinary API.
        /// </summary>
        protected string m_apiAddr = "https://" + ADDR_API;

        private readonly Func<string, HttpRequestMessage> requestBuilder =
            (url) => new HttpRequestMessage { RequestUri = new Uri(url) };

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiShared"/> class.
        /// Default parameterless constructor.
        /// </summary>
        public ApiShared()
        {
            Account = new Account();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiShared"/> class.
        /// Parameterized constructor.
        /// </summary>
        /// <param name="cloudinaryUrl">Cloudinary URL.</param>
        public ApiShared(string cloudinaryUrl)
        {
            if (string.IsNullOrEmpty(cloudinaryUrl) || !cloudinaryUrl.StartsWith("cloudinary://", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Invalid CLOUDINARY_URL scheme. Expecting to start with 'cloudinary://'");
            }

            Uri cloudinaryUri = new Uri(cloudinaryUrl);

            if (string.IsNullOrEmpty(cloudinaryUri.Host))
            {
                throw new ArgumentException("Cloud name must be specified as host name in URL!");
            }

            string[] creds = cloudinaryUri.UserInfo.Split(':');
            Account = new Account(cloudinaryUri.Host, creds[0], creds[1]);

            UsePrivateCdn = !string.IsNullOrEmpty(cloudinaryUri.AbsolutePath) &&
                cloudinaryUri.AbsolutePath != "/";

            PrivateCdn = string.Empty;

            if (UsePrivateCdn)
            {
                PrivateCdn = cloudinaryUri.AbsolutePath;
                Secure = true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiShared"/> class.
        /// Parameterized constructor.
        /// </summary>
        /// <param name="account">Cloudinary account.</param>
        /// <param name="usePrivateCdn">Whether to use private Content Delivery Network.</param>
        /// <param name="privateCdn">Private Content Delivery Network.</param>
        /// <param name="shortenUrl">Whether to use shorten URL when possible.</param>
        /// <param name="cSubDomain">Whether to use sub domain.</param>
        public ApiShared(Account account, bool usePrivateCdn, string privateCdn, bool shortenUrl, bool cSubDomain)
            : this(account)
        {
            UsePrivateCdn = usePrivateCdn;
            Secure = usePrivateCdn; // for backwards compatibility
            PrivateCdn = privateCdn;
            ShortenUrl = shortenUrl;
            CSubDomain = cSubDomain;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiShared"/> class.
        /// Parameterized constructor.
        /// </summary>
        /// <param name="account">Cloudinary account.</param>
        public ApiShared(Account account)
        {
            if (account == null)
            {
                throw new ArgumentException("Account can't be null!");
            }

            if (string.IsNullOrEmpty(account.Cloud))
            {
                throw new ArgumentException("Cloud name must be specified in Account!");
            }

            UsePrivateCdn = false;
            Account = account;
        }

        /// <summary>
        /// Gets cloudinary account information.
        /// </summary>
        public Account Account { get; private set; }

        /// <summary>
        /// Gets or sets API base address (https://api.cloudinary.com by default) which is used to build ApiUrl*.
        /// </summary>
        public string ApiBaseAddress
        {
            get { return m_apiAddr; }
            set { m_apiAddr = value; }
        }

        /// <summary>
        /// Gets default URL for working with resources.
        /// </summary>
        public Url Url
        {
            get
            {
                return new Url(Account.Cloud, this as ISignProvider)
                    .CSubDomain(CSubDomain)
                    .Shorten(ShortenUrl)
                    .PrivateCdn(UsePrivateCdn)
                    .Secure(Secure)
                    .ForceVersion(ForceVersion)
                    .SecureDistribution(PrivateCdn);
            }
        }

        /// <summary>
        /// Gets default URL for working with uploaded images.
        /// </summary>
        public Url UrlImgUp
        {
            get
            {
                return Url
                    .ResourceType(Constants.RESOURCE_TYPE_IMAGE)
                    .Action(Constants.ACTION_NAME_UPLOAD)
                    .UseRootPath(UseRootPath)
                    .Suffix(Suffix);
            }
        }

        /// <summary>
        /// Gets default URL for working with fetched images.
        /// </summary>
        public Url UrlImgFetch
        {
            get
            {
                return Url
                    .ResourceType(Constants.RESOURCE_TYPE_IMAGE)
                    .Action(Constants.ACTION_NAME_FETCH)
                    .UseRootPath(UseRootPath)
                    .Suffix(Suffix);
            }
        }

        /// <summary>
        /// Gets default URL for working with uploaded videos.
        /// </summary>
        public Url UrlVideoUp
        {
            get
            {
                return Url
                    .ResourceType(Constants.RESOURCE_TYPE_VIDEO)
                    .Action(Constants.ACTION_NAME_UPLOAD)
                    .UseRootPath(UseRootPath)
                    .Suffix(Suffix);
            }
        }

        /// <summary>
        /// Gets default cloudinary API URL.
        /// </summary>
        public Url ApiUrl
        {
            get
            {
                return Url.
                    CloudinaryAddr(m_apiAddr);
            }
        }

        /// <summary>
        /// Gets default cloudinary API URL for uploading images.
        /// </summary>
        public Url ApiUrlImgUp
        {
            get
            {
                return ApiUrl.
                    Action(Constants.ACTION_NAME_UPLOAD).
                    ResourceType(Constants.RESOURCE_TYPE_IMAGE);
            }
        }

        /// <summary>
        /// Gets default cloudinary API URL with version.
        /// </summary>
        public Url ApiUrlV
        {
            get
            {
                return ApiUrl.
                    ApiVersion(API_VERSION);
            }
        }

        /// <summary>
        /// Gets default cloudinary API URL for streaming profiles.
        /// </summary>
        public Url ApiUrlStreamingProfileV => ApiUrlV.Add(Constants.STREAMING_PROFILE_API_URL);

        /// <summary>
        /// Gets default cloudinary API URL for metadata fields.
        /// </summary>
        public Url ApiUrlMetadataFieldV => ApiUrlV.Add(Constants.METADATA_FIELDS_API_URL);

        /// <summary>
        /// Gets default cloudinary API URL for uploading images with version.
        /// </summary>
        public Url ApiUrlImgUpV
        {
            get
            {
                return ApiUrlV.
                    Action(Constants.ACTION_NAME_UPLOAD).
                    ResourceType(Constants.RESOURCE_TYPE_IMAGE);
            }
        }

        /// <summary>
        /// Gets default cloudinary API URL for uploading video resources with version.
        /// </summary>
        public Url ApiUrlVideoUpV
        {
            get
            {
                return ApiUrlV.
                    Action(Constants.ACTION_NAME_UPLOAD).
                    ResourceType(Constants.RESOURCE_TYPE_VIDEO);
            }
        }

        /// <summary>
        /// Gets cloudinary parameter from enumeration.
        /// </summary>
        /// <typeparam name="T">Enum which fields are decorated with DescriptionAttribute.</typeparam>
        /// <param name="e">Field of enum.</param>
        /// <returns>Cloudinary-compatible parameter.</returns>
        public static string GetCloudinaryParam<T>(T e)
        {
            Type eType = typeof(T);
            FieldInfo fi = eType.GetField(e.ToString());
            EnumMemberAttribute[] attrs = (EnumMemberAttribute[])fi.GetCustomAttributes(
                typeof(EnumMemberAttribute), false);

            if (attrs.Length == 0)
            {
                throw new ArgumentException("Enum fields must be decorated with EnumMemberAttribute!");
            }

            return attrs[0].Value;
        }

        /// <summary>
        /// Parse cloudinary-compatible parameter as enum field.
        /// </summary>
        /// <typeparam name="T">Enum which fields are decorated with DescriptionAttribute.</typeparam>
        /// <param name="s">Field of enum represented as string.</param>
        /// <returns>Field of enum.</returns>
        public static T ParseCloudinaryParam<T>(string s)
        {
            Type eType = typeof(T);
            foreach (var fi in eType.GetFields())
            {
                EnumMemberAttribute[] attrs = (EnumMemberAttribute[])fi.GetCustomAttributes(
                    typeof(EnumMemberAttribute), false);

                if (attrs.Length == 0)
                {
                    continue;
                }

                if (s == attrs[0].Value)
                {
                    return (T)fi.GetValue(null);
                }
            }

            return default(T);
        }

        /// <summary>
        /// Call the Cloudinary API and parse HTTP response asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the response.</typeparam>
        /// <param name="method">HTTP method.</param>
        /// <param name="url">A generated URL.</param>
        /// <param name="parameters">Cloudinary parameters to add to the API call.</param>
        /// <param name="file">(Optional) Add file to the body of the API call.</param>
        /// <param name="extraHeaders">Headers to add to the request.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Instance of the parsed response from the cloudinary API.</returns>
        public async Task<T> CallAndParseAsync<T>(
            HttpMethod method,
            string url,
            SortedDictionary<string, object> parameters,
            FileDescription file,
            Dictionary<string, string> extraHeaders = null,
            CancellationToken? cancellationToken = null)
            where T : BaseResult, new()
        {
            using (var response = await CallAsync(
                method,
                url,
                parameters,
                file,
                extraHeaders,
                cancellationToken).ConfigureAwait(false))
            {
                return await ParseAsync<T>(response).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Call the Cloudinary API and parse HTTP response.
        /// </summary>
        /// <typeparam name="T">Type of the response.</typeparam>
        /// <param name="method">HTTP method.</param>
        /// <param name="url">A generated URL.</param>
        /// <param name="parameters">Cloudinary parameters to add to the API call.</param>
        /// <param name="file">(Optional) Add file to the body of the API call.</param>
        /// <param name="extraHeaders">(Optional) Headers to add to the request.</param>
        /// <returns>Instance of the parsed response from the cloudinary API.</returns>
        public T CallAndParse<T>(
            HttpMethod method,
            string url,
            SortedDictionary<string, object> parameters,
            FileDescription file,
            Dictionary<string, string> extraHeaders = null)
            where T : BaseResult, new()
        {
            using (var response = Call(
                method,
                url,
                parameters,
                file,
                extraHeaders))
            {
                return Parse<T>(response);
            }
        }

        /// <summary>
        /// Makes custom call to Cloudinary API asynchronously.
        /// </summary>
        /// <param name="method">HTTP method of call.</param>
        /// <param name="url">URL to call.</param>
        /// <param name="parameters">Dictionary of call parameters (can be null).</param>
        /// <param name="file">File to upload (must be null for non-uploading actions).</param>
        /// <param name="extraHeaders">Headers to add to the request.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>HTTP response on call.</returns>
        public async Task<HttpResponseMessage> CallAsync(
            HttpMethod method,
            string url,
            SortedDictionary<string, object> parameters,
            FileDescription file,
            Dictionary<string, string> extraHeaders = null,
            CancellationToken? cancellationToken = null)
        {
            using (var request =
                await PrepareRequestBodyAsync(
                    requestBuilder(url),
                    method,
                    parameters,
                    file,
                    extraHeaders,
                    cancellationToken).ConfigureAwait(false))
            {
                var httpCancellationToken = cancellationToken ?? GetDefaultCancellationToken();
                return await Client.SendAsync(request, httpCancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Makes custom call to Cloudinary API.
        /// </summary>
        /// <param name="method">HTTP method of call.</param>
        /// <param name="url">URL to call.</param>
        /// <param name="parameters">Dictionary of call parameters (can be null).</param>
        /// <param name="file">File to upload (must be null for non-uploading actions).</param>
        /// <param name="extraHeaders">Headers to add to the request.</param>
        /// <returns>HTTP response on call.</returns>
        public HttpResponseMessage Call(
            HttpMethod method,
            string url,
            SortedDictionary<string, object> parameters,
            FileDescription file,
            Dictionary<string, string> extraHeaders = null)
        {
            using (var request = requestBuilder(url))
            {
                PrepareRequestBody(request, method, parameters, file, extraHeaders);

                var cancellationToken = GetDefaultCancellationToken();

                return Client
                    .SendAsync(request, cancellationToken)
                    .GetAwaiter()
                    .GetResult();
            }
        }

        /// <summary>
        /// Gets the upload URL for resource.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <returns>
        /// The upload URL.
        /// </returns>
        public string GetUploadUrl(string resourceType = "auto")
        {
            return ApiUrlV.Action("upload").ResourceType(resourceType).BuildUrl();
        }

        /// <summary>
        /// Signs and serializes upload parameters.
        /// </summary>
        /// <param name="parameters">Dictionary of upload parameters.</param>
        /// <returns>JSON representation of upload parameters.</returns>
        public string PrepareUploadParams(IDictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                parameters = new SortedDictionary<string, object>();
            }

            if (!(parameters is SortedDictionary<string, object>))
            {
                parameters = new SortedDictionary<string, object>(parameters);
            }

            foreach (var key in parameters.Keys.ToList())
            {
                var paramValue = parameters[key];
                if (paramValue is IEnumerable<string> value)
                {
                    parameters[key] = Utils.SafeJoin("|", value);
                }
                else if (paramValue is Transformation transformation)
                {
                    parameters[key] = transformation.Generate();
                }
            }

            var path = string.Empty;
            if (parameters.ContainsKey("callback") && parameters["callback"] != null)
            {
                path = parameters["callback"].ToString();
            }

            try
            {
                parameters["callback"] = BuildCallbackUrl(path);
            }
            catch (ArgumentException)
            {
            }

            if (!parameters.ContainsKey("unsigned") || parameters["unsigned"].ToString() == "false")
            {
                FinalizeUploadParameters(parameters);
            }

            return JsonConvert.SerializeObject(parameters);
        }

        /// <summary>
        /// Calculates signature of parameters, based on agreed signature algorithm.
        /// </summary>
        /// <param name="parameters">Parameters to sign.</param>
        /// <returns>Signature of parameters.</returns>
        public string SignParameters(IDictionary<string, object> parameters)
        {
            List<string> excludedSignatureKeys = new List<string>(new string[] { "resource_type", "file", "api_key" });
            StringBuilder signBase = new StringBuilder(string.Join("&", parameters.
                                                                   Where(pair => pair.Value != null && !excludedSignatureKeys.Any(s => pair.Key.Equals(s, StringComparison.Ordinal)))
                .Select(pair =>
                       {
                           var value = pair.Value is IEnumerable<string>
                               ? string.Join(",", ((IEnumerable<string>)pair.Value).ToArray())
                               : pair.Value.ToString();
                           return string.Format(CultureInfo.InvariantCulture, "{0}={1}", pair.Key, value);
                       })
                .ToArray()));

            signBase.Append(Account.ApiSecret);

            var hash = Utils.ComputeHash(signBase.ToString(), SignatureAlgorithm);
            StringBuilder sign = new StringBuilder();
            foreach (byte b in hash)
            {
                sign.Append(b.ToString("x2", CultureInfo.InvariantCulture));
            }

            return sign.ToString();
        }

        /// <summary>
        /// Signs the specified URI part.
        /// </summary>
        /// <param name="uriPart">The URI part.</param>
        /// <param name="isLong">Indicates whether to generate long signature.</param>
        /// <returns>Signature of the URI part.</returns>
        public string SignUriPart(string uriPart, bool isLong = true)
        {
            var extendedUriPart = uriPart + Account.ApiSecret;
            var signatureAlgorithm = isLong ? SignatureAlgorithm.SHA256 : SignatureAlgorithm;
            var hash = Utils.ComputeHash(extendedUriPart, signatureAlgorithm);
            var signatureLength = isLong ? 32 : 8;
            return "s--" + Utils.EncodeUrlSafe(hash).Substring(0, signatureLength) + "--/";
        }

        /// <summary>
        /// Validates API response signature against Cloudinary configuration.
        /// </summary>
        /// <param name="publicId">Public ID of resource.</param>
        /// <param name="version">Version of resource.</param>
        /// <param name="signature">Response signature.</param>
        /// <returns>Boolean result of the validation.</returns>
        public bool VerifyApiResponseSignature(string publicId, string version, string signature)
        {
            var parametersToSign = new SortedDictionary<string, object>()
            {
                { "public_id", publicId },
                { "version", version },
            };
            var signedParameters = SignParameters(parametersToSign);

            return signature.Equals(signedParameters, StringComparison.Ordinal);
        }

        /// <summary>
        /// Validates notification signature against Cloudinary configuration.
        /// </summary>
        /// <param name="body">Request body.</param>
        /// <param name="timestamp">Request timestamp.</param>
        /// <param name="signature">Notification signature.</param>
        /// <param name="validFor">For how long the signature is valid, in seconds.</param>
        /// <returns>Boolean result of the validation.</returns>
        public bool VerifyNotificationSignature(string body, long timestamp, string signature, int validFor = 7200)
        {
            var currentTimestamp = Utils.UnixTimeNowSeconds();
            var isSignatureExpired = timestamp <= currentTimestamp - validFor;
            if (isSignatureExpired)
            {
                return false;
            }

            var payloadHash = Utils.ComputeHexHash($"{body}{timestamp}{Account.ApiSecret}", SignatureAlgorithm);

            return signature.Equals(payloadHash, StringComparison.Ordinal);
        }

        /// <summary>
        /// Virtual build callback URL method. This method should be overridden in child classes.
        /// </summary>
        /// <param name="path">File path to check.</param>
        /// <returns>Callback URL.</returns>
        public virtual string BuildCallbackUrl(string path = "")
        {
            return string.Empty;
        }

        /// <summary>
        /// Builds HTML file input tag for unsigned upload of an asset.
        /// </summary>
        /// <param name="field">The name of an input field in the same form that will be updated post-upload with the asset's metadata.
        /// If no such field exists in your form, a new hidden field with the specified name will be created.</param>
        /// <param name="preset">The name of upload preset.</param>
        /// <param name="resourceType">Type of the uploaded resource.</param>
        /// <param name="parameters">Cloudinary upload parameters to add to the file input tag.</param>
        /// <param name="htmlOptions">Html options to be applied to the file input tag.</param>
        /// <returns>A file input tag, that needs to be added to the form on your HTML page.</returns>
        public string BuildUnsignedUploadForm(string field, string preset, string resourceType, SortedDictionary<string, object> parameters = null, Dictionary<string, string> htmlOptions = null)
        {
            return BuildUploadForm(field, resourceType, BuildUnsignedUploadParams(preset, parameters), htmlOptions);
        }

        /// <summary>
        /// Builds HTML file input tag for upload an asset.
        /// </summary>
        /// <param name="field">The name of an input field in the same form that will be updated post-upload with the asset's metadata.
        /// If no such field exists in your form, a new hidden field with the specified name will be created.</param>
        /// <param name="resourceType">Type of the uploaded resource.</param>
        /// <param name="parameters">Cloudinary upload parameters to add to the file input tag.</param>
        /// <param name="htmlOptions">Html options to be applied to the file input tag.</param>
        /// <returns>A file input tag, that needs to be added to the form on your HTML page.</returns>
        public string BuildUploadForm(string field, string resourceType, SortedDictionary<string, object> parameters = null, Dictionary<string, string> htmlOptions = null)
        {
            return BuildUploadFormShared(field, resourceType, parameters, htmlOptions);
        }

        /// <summary>
        /// Build file input html tag.
        /// </summary>
        /// <param name="field">The name of an input field in the same form that will be updated post-upload with the asset's metadata.
        /// If no such field exists in your form, a new hidden field with the specified name will be created.</param>
        /// <param name="resourceType">Type of the uploaded resource.</param>
        /// <param name="parameters">Cloudinary upload parameters to add to the file input tag.</param>
        /// <param name="htmlOptions">Html options to be applied to the file input tag.</param>
        /// <returns>A file input tag, that needs to be added to the form on your HTML page.</returns>
        public string BuildUploadFormShared(string field, string resourceType, SortedDictionary<string, object> parameters = null, Dictionary<string, string> htmlOptions = null)
        {
            if (htmlOptions == null)
            {
                htmlOptions = new Dictionary<string, string>();
            }

            if (string.IsNullOrEmpty(resourceType))
            {
                resourceType = "auto";
            }

            StringBuilder builder = new StringBuilder();

            builder.
                Append("<input type='file' name='file' data-url='").
                Append(GetUploadUrl(resourceType)).
                Append("' data-form-data='").
                Append(PrepareUploadParams(parameters)).
                Append("' data-cloudinary-field='").
                Append(field).
                Append("' class='cloudinary-fileupload");

            if (htmlOptions.ContainsKey("class"))
            {
                builder.Append(' ').Append(htmlOptions["class"]);
            }

            foreach (var item in htmlOptions)
            {
                if (item.Key == "class")
                {
                    continue;
                }

                builder.
                    Append("' ").
                    Append(item.Key).
                    Append("='").
                    Append(EncodeApiUrl(item.Value));
            }

            builder.Append("'/>");

            return builder.ToString();
        }
    }
}
