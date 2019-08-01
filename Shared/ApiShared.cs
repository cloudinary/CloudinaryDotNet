
using CloudinaryDotNet.Actions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Provider for the API calls.
    /// </summary>
    public class ApiShared : ISignProvider
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
        public static string USER_AGENT;

        string m_apiAddr = "https://" + ADDR_API;

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
        public int Timeout = 0;

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

        private static readonly HttpClient client = new HttpClient();

        private readonly Func<string, HttpRequestMessage> RequestBuilder =
            (url) => new HttpRequestMessage { RequestUri = new Uri(url) };

        /// <summary>
        /// Default static parameterless constructor.
        /// </summary>
        static ApiShared()
        {
            var version = typeof(Api).GetTypeInfo().Assembly.GetName().Version;

            var frameworkDescription = RuntimeInformation.FrameworkDescription;

            USER_AGENT = String.Format("CloudinaryDotNet/{0}.{1}.{2} ({3})",
                version.Major, version.Minor, version.Build, frameworkDescription);
        }

        /// <summary>
        /// Default parameterless constructor.
        /// Assumes that environment variable CLOUDINARY_URL is set.
        /// </summary>
        public ApiShared()
            : this(Environment.GetEnvironmentVariable("CLOUDINARY_URL")) { }

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="cloudinaryUrl">Cloudinary URL.</param>
        public ApiShared(string cloudinaryUrl)
        {
            if (String.IsNullOrEmpty(cloudinaryUrl))
                throw new ArgumentException("Valid cloudinary init string must be provided!");

            Uri cloudinaryUri = new Uri(cloudinaryUrl);

            if (String.IsNullOrEmpty(cloudinaryUri.Host))
                throw new ArgumentException("Cloud name must be specified as host name in URL!");

            string[] creds = cloudinaryUri.UserInfo.Split(':');
            Account = new Account(cloudinaryUri.Host, creds[0], creds[1]);

            UsePrivateCdn = !String.IsNullOrEmpty(cloudinaryUri.AbsolutePath) &&
                cloudinaryUri.AbsolutePath != "/";

            PrivateCdn = String.Empty;

            if (UsePrivateCdn)
            {
                PrivateCdn = cloudinaryUri.AbsolutePath;
                Secure = true;
            }

        }

        /// <inheritdoc />
        /// <summary>
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
        /// Parameterized constructor.
        /// </summary>
        /// <param name="account">Cloudinary account.</param>
        public ApiShared(Account account)
        {
            if (account == null)
                throw new ArgumentException("Account can't be null!");

            if (String.IsNullOrEmpty(account.Cloud))
                throw new ArgumentException("Cloud name must be specified in Account!");

            UsePrivateCdn = false;
            Account = account;
        }

        /// <summary>
        /// Cloudinary account information.
        /// </summary>
        public Account Account { get; private set; }

        /// <summary>
        /// Gets or sets API base address (https://api.cloudinary.com by default) which is used to build ApiUrl*
        /// </summary>
        public string ApiBaseAddress
        {
            get { return m_apiAddr; }
            set { m_apiAddr = value; }
        }

        /// <summary>
        /// Default URL for working with resources.
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
        /// Default URL for working with uploaded images.
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
        /// Default URL for working with fetched images.
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
        /// Default URL for working with uploaded videos.
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
        /// Default cloudinary API URL.
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
        /// Default cloudinary API URL for uploading images.
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
        /// Default cloudinary API URL with version.
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
        /// Default cloudinary API URL for streaming profiles.
        /// </summary>
        public Url ApiUrlStreamingProfileV => ApiUrlV.Add(Constants.STREAMING_PROFILE_API_URL);

        /// <summary>
        /// Default cloudinary API URL for uploading images with version.
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
        /// Default cloudinary API URL for uploading video resources with version.
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
        /// Virtual method to call the cloudinary API. This method should be overridden in child classes.
        /// </summary>
        /// <param name="method">Http request method.</param>
        /// <param name="url">API URL.</param>
        /// <param name="parameters">Cloudinary parameters to add to the API call.</param>
        /// <param name="file">(Optional) Add file to the body of the API call.</param>
        /// <param name="extraHeaders">(Optional) Add file to the body of the API call.</param>
        /// <returns>Parsed response from the cloudinary API.</returns>
        public virtual object InternalCall(HttpMethod method, string url, SortedDictionary<string, object> parameters, FileDescription file, Dictionary<string, string> extraHeaders = null)
        {
            throw new Exception("Please call overriden method");
        }

        internal virtual Task<T> CallApiAsync<T>(HttpMethod method, string url, BaseParams parameters, FileDescription file, Dictionary<string, string> extraHeaders = null) where T : BaseResult, new()
        {
            parameters?.Check();

            var callParams = (method == HttpMethod.PUT || method == HttpMethod.POST)
                ? parameters?.ToParamsDictionary()
                : null;

            return CallAndParseAsync<T>(method, url, callParams, file, extraHeaders);
        }

        internal virtual T CallApi<T>(HttpMethod method, string url, BaseParams parameters, FileDescription file, Dictionary<string, string> extraHeaders = null) where T : BaseResult, new()
        {
            parameters?.Check();

            return CallAndParse<T>(method,
                                   url,
                                   (method == HttpMethod.PUT || method == HttpMethod.POST) ? parameters?.ToParamsDictionary() : null,
                                   file,
                                   extraHeaders);
        }

        /// <summary>
        /// Call the Cloudinary API and parse HTTP response.
        /// </summary>
        /// <typeparam name="T">Type of the response.</typeparam>
        /// <param name="method">HTTP method.</param>
        /// <param name="url">A generated URL.</param>
        /// <param name="parameters">Cloudinary parameters to add to the API call.</param>
        /// <param name="file">(Optional) Add file to the body of the API call.</param>
        /// <param name="extraHeaders">(Optional) Add file to the body of the API call.</param>
        /// <returns>Instance of the parsed response from the cloudinary API.</returns>
        public async Task<T> CallAndParseAsync<T>(
            HttpMethod method,
            string url,
            SortedDictionary<string, object> parameters,
            FileDescription file,
            Dictionary<string, string> extraHeaders = null) where T : BaseResult, new()
        {
            using (var response = await CallAsync(method,
                url,
                parameters,
                file,
                extraHeaders))
            {
                return await ParseAsync<T>(response);
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
        /// <param name="extraHeaders">(Optional) Add file to the body of the API call.</param>
        /// <returns>Instance of the parsed response from the cloudinary API.</returns>
        public T CallAndParse<T>(
            HttpMethod method,
            string url,
            SortedDictionary<string, object> parameters,
            FileDescription file,
            Dictionary<string, string> extraHeaders = null) where T : BaseResult, new()
        {
            using (var response = Call(method,
                url,
                parameters,
                file,
                extraHeaders))
            {
                return Parse<T>(response);
            }
        }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class.
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <returns>New instance of this class.</returns>
        internal static async Task<T> ParseAsync<T>(HttpResponseMessage response) where T : BaseResult
        {
            if (response == null)
                throw new ArgumentNullException("response");

            T result;

            using (Stream stream = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(stream))
            {
                string s = await reader.ReadToEndAsync();
                try
                {
                    result = JsonConvert.DeserializeObject<T>(s);
                    result.JsonObj = JToken.Parse(s);
                }
                catch (JsonException jex)
                {
                    throw new Exception($"Failed to deserialize response with status code: {response.StatusCode}", jex);
                }
            }

            if (response.Headers != null)
                foreach (var header in response.Headers)
                {
                    if (header.Key.StartsWith("X-FeatureRateLimit"))
                    {

                        if (header.Key.EndsWith("Limit") && long.TryParse(header.Value.First(), out long l))
                            result.Limit = l;

                        if (header.Key.EndsWith("Remaining") && long.TryParse(header.Value.First(), out l))
                            result.Remaining = l;

                        if (header.Key.EndsWith("Reset") && DateTime.TryParse(header.Value.First(), out DateTime t))
                            result.Reset = t;
                    }
                }

            result.StatusCode = response.StatusCode;

            return result;
        }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class.
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <returns>New instance of this class.</returns>
        internal static T Parse<T>(HttpResponseMessage response) where T : BaseResult
        {
            if (response == null)
                throw new ArgumentNullException("response");

            T result;

            var task = response.Content.ReadAsStreamAsync();
            task.Wait();
            using (Stream stream = task.Result)
            using (StreamReader reader = new StreamReader(stream))
            {
                string s = reader.ReadToEnd();
                try
                {
                    result = JsonConvert.DeserializeObject<T>(s);
                    result.JsonObj = JToken.Parse(s);
                }
                catch (JsonException jex)
                {
                    throw new Exception($"Failed to deserialize response with status code: {response.StatusCode}", jex);
                }
            }

            if (response.Headers != null)
                foreach (var header in response.Headers)
                {
                    if (header.Key.StartsWith("X-FeatureRateLimit"))
                    {
                        long l;
                        DateTime t;

                        if (header.Key.EndsWith("Limit") && long.TryParse(header.Value.First(), out l))
                            result.Limit = l;

                        if (header.Key.EndsWith("Remaining") && long.TryParse(header.Value.First(), out l))
                            result.Remaining = l;

                        if (header.Key.EndsWith("Reset") && DateTime.TryParse(header.Value.First(), out t))
                            result.Reset = t;
                    }
                }

            result.StatusCode = response.StatusCode;

            return result;
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
        public async Task<HttpResponseMessage> CallAsync(
            HttpMethod method,
            string url,
            SortedDictionary<string, object> parameters,
            FileDescription file,
            Dictionary<string, string> extraHeaders = null)
        {
            using (var requestPrepared =
                await PrepareRequestBodyAsync(RequestBuilder(url), method, parameters, file, extraHeaders))
            {
                return await (Timeout > 0
                            ? client.SendAsync(requestPrepared, new CancellationTokenSource(Timeout).Token)
                            : client.SendAsync(requestPrepared));
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
        public HttpResponseMessage Call(HttpMethod method, string url, SortedDictionary<string, object> parameters, FileDescription file, Dictionary<string, string> extraHeaders = null)
        {
            var request = RequestBuilder(url);
            HttpResponseMessage response = null;
            using (request)
            {
                PrepareRequestBody(request, method, parameters, file, extraHeaders);

                System.Threading.Tasks.Task<HttpResponseMessage> task2;

                if (Timeout > 0)
                {
                    var cancellationTokenSource = new CancellationTokenSource(Timeout);
                    task2 = client.SendAsync(request, cancellationTokenSource.Token);
                }
                else
                {
                    task2 = client.SendAsync(request);
                }

                task2.Wait();
                if (task2.IsFaulted) { throw task2.Exception; }
                response = task2.Result;

                if (task2.IsCanceled) { }
                if (task2.IsFaulted) { throw task2.Exception; }

                return response;
            }

        }

        internal async Task<HttpRequestMessage> PrepareRequestBodyAsync(
            HttpRequestMessage request,
            HttpMethod method,
            SortedDictionary<string, object> parameters,
            FileDescription file,
            Dictionary<string, string> extraHeaders = null)
        {
            SetHttpMethod(method, request);

            // Add platform information to the USER_AGENT header
            // This is intended for platform information and not individual applications!
            request.Headers.Add("User-Agent", string.IsNullOrEmpty(UserPlatform)
                ? USER_AGENT
                : string.Format("{0} {1}", UserPlatform, USER_AGENT));

            byte[] authBytes = Encoding.ASCII.GetBytes(String.Format("{0}:{1}", Account.ApiKey, Account.ApiSecret));
            request.Headers.Add("Authorization", String.Format("Basic {0}", Convert.ToBase64String(authBytes)));

            if (extraHeaders != null)
            {
                if (extraHeaders.ContainsKey("Accept"))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(extraHeaders["Accept"]));
                    extraHeaders.Remove("Accept");
                }

                foreach (var header in extraHeaders)
                {
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            if ((method == HttpMethod.POST || method == HttpMethod.PUT) && parameters != null)
            {
                if (UseChunkedEncoding)
                    request.Headers.Add("Transfer-Encoding", "chunked");

                await PrepareRequestContentAsync(request, parameters, file, extraHeaders);
            }

            return request;
        }

        internal HttpRequestMessage PrepareRequestBody(HttpRequestMessage request, HttpMethod method, SortedDictionary<string, object> parameters, FileDescription file, Dictionary<string, string> extraHeaders = null)
        {
            SetHttpMethod(method, request);

            // Add platform information to the USER_AGENT header
            // This is intended for platform information and not individual applications!
            request.Headers.Add("User-Agent", string.IsNullOrEmpty(UserPlatform)
                ? USER_AGENT
                : string.Format("{0} {1}", UserPlatform, USER_AGENT));

            byte[] authBytes = Encoding.ASCII.GetBytes(String.Format("{0}:{1}", Account.ApiKey, Account.ApiSecret));
            request.Headers.Add("Authorization", String.Format("Basic {0}", Convert.ToBase64String(authBytes)));

            if (extraHeaders != null)
            {
                if (extraHeaders.ContainsKey("Accept"))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(extraHeaders["Accept"]));
                    extraHeaders.Remove("Accept");
                }

                foreach (var header in extraHeaders)
                {
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            if ((method == HttpMethod.POST || method == HttpMethod.PUT) && parameters != null)
            {
                if (UseChunkedEncoding)
                    request.Headers.Add("Transfer-Encoding", "chunked");

                PrepareRequestContent(request, parameters, file, extraHeaders);
            }

            return request;
        }

        private async Task PrepareRequestContentAsync(HttpRequestMessage request, SortedDictionary<string, object> parameters, FileDescription file, Dictionary<string, string> extraHeaders = null)
        {
            HandleUnsignedParameters(parameters);

            HttpContent content = extraHeaders != null &&
                                  extraHeaders.ContainsKey(Constants.HEADER_CONTENT_TYPE) &&
                                  extraHeaders[Constants.HEADER_CONTENT_TYPE] == Constants.CONTENT_TYPE_APPLICATION_JSON
                ? new StringContent(ParamsToJson(parameters), Encoding.UTF8, Constants.CONTENT_TYPE_APPLICATION_JSON)
                : await PrepareMultipartFormDataContentAsync(parameters, file, extraHeaders);

            if (extraHeaders != null)
            {
                foreach (var header in extraHeaders)
                {
                    content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            request.Content = content;
        }

        private void PrepareRequestContent(HttpRequestMessage request, SortedDictionary<string, object> parameters, FileDescription file, Dictionary<string, string> extraHeaders = null)
        {
            HandleUnsignedParameters(parameters);

            HttpContent content = extraHeaders != null &&
                                  extraHeaders.ContainsKey(Constants.HEADER_CONTENT_TYPE) &&
                                  extraHeaders[Constants.HEADER_CONTENT_TYPE] == Constants.CONTENT_TYPE_APPLICATION_JSON
                ? new StringContent(ParamsToJson(parameters), Encoding.UTF8, Constants.CONTENT_TYPE_APPLICATION_JSON)
                : PrepareMultipartFormDataContent(parameters, file, extraHeaders);

            if (extraHeaders != null)
            {
                foreach (var header in extraHeaders)
                {
                    content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            request.Content = content;
        }

        private async Task<HttpContent> PrepareMultipartFormDataContentAsync(
            SortedDictionary<string, object> parameters,
            FileDescription file,
            Dictionary<string, string> extraHeaders = null)
        {
            var content = new MultipartFormDataContent(HTTP_BOUNDARY);
            foreach (var param in parameters)
            {
                if (param.Value != null)
                {
                    if (param.Value is IEnumerable<string>)
                    {
                        foreach (var item in (IEnumerable<string>)param.Value)
                        {
                            content.Add(new StringContent(item), string.Format("\"{0}\"", string.Concat(param.Key, "[]")));
                        }
                    }
                    else
                    {
                        content.Add(new StringContent(param.Value.ToString()), string.Format("\"{0}\"", param.Key));
                    }
                }
            }

            if (file != null)
            {
                if (file.IsRemote)
                {
                    StringContent strContent = new StringContent(file.FilePath);
                    strContent.Headers.Add("Content-Disposition", string.Format("form-data; name=\"{0}\"", "file"));
                    content.Add(strContent);
                }
                else
                {
                    Stream stream = file.Stream ?? File.OpenRead(file.FilePath);

                    if (extraHeaders != null && extraHeaders.ContainsKey("Content-Range"))
                    {
                        // Unfortunately we don't have ByteRangeStreamContent here,
                        // let's create another stream from the original one
                        stream = await GetRangeFromFileAsync(file, stream);
                    }

                    var streamContent = new StreamContent(stream);

                    streamContent.Headers.Add("Content-Type", "application/octet-stream");
                    streamContent.Headers.Add("Content-Disposition", "form-data; name=\"file\"; filename=\"" + file.FileName + "\"");
                    content.Add(streamContent, "file", file.FileName);
                }
            }

            return content;
        }

        private HttpContent PrepareMultipartFormDataContent(SortedDictionary<string, object> parameters,
            FileDescription file, Dictionary<string, string> extraHeaders = null)
        {
            var content = new MultipartFormDataContent(HTTP_BOUNDARY);
            foreach (var param in parameters)
            {
                if (param.Value != null)
                {
                    if (param.Value is IEnumerable<string>)
                    {
                        foreach (var item in (IEnumerable<string>)param.Value)
                        {
                            content.Add(new StringContent(item), String.Format("\"{0}\"", string.Concat(param.Key, "[]")));
                        }
                    }
                    else
                    {
                        content.Add(new StringContent(param.Value.ToString()), String.Format("\"{0}\"", param.Key));
                    }
                }
            }

            if (file != null)
            {
                if (file.IsRemote)
                {
                    StringContent strContent = new StringContent(file.FilePath);
                    strContent.Headers.Add("Content-Disposition", string.Format("form-data; name=\"{0}\"", "file"));
                    content.Add(strContent);
                }
                else
                {
                    Stream stream = file.Stream ?? File.OpenRead(file.FilePath);

                    if (extraHeaders != null && extraHeaders.ContainsKey("Content-Range"))
                    {
                        // Unfortunately we don't have ByteRangeStreamContent here,
                        // let's create another stream from the original one
                        stream = GetRangeFromFile(file, stream);
                    }

                    var streamContent = new StreamContent(stream);

                    streamContent.Headers.Add("Content-Type", "application/octet-stream");
                    streamContent.Headers.Add("Content-Disposition", "form-data; name=\"file\"; filename=\"" + file.FileName + "\"");
                    content.Add(streamContent, "file", file.FileName);
                }
            }

            return content;
        }

        private async Task<Stream> GetRangeFromFileAsync(FileDescription file, Stream stream)
        {
            var memStream = new MemoryStream();
            var writer = new StreamWriter(memStream);

            stream.Seek(file.BytesSent, SeekOrigin.Begin);
            file.BytesSent += await ReadBytesAsync(writer, stream, file.BufferLength);
            writer.BaseStream.Seek(0, SeekOrigin.Begin);

            return memStream;
        }

        private Stream GetRangeFromFile(FileDescription file, Stream stream)
        {
            var memStream = new MemoryStream();
            var writer = new StreamWriter(memStream);

            stream.Seek(file.BytesSent, SeekOrigin.Begin);
            file.BytesSent += ReadBytes(writer, stream, file.BufferLength);
            writer.BaseStream.Seek(0, SeekOrigin.Begin);

            return memStream;
        }

        private async Task<int> ReadBytesAsync(StreamWriter writer, Stream stream, int length)
        {
            int bytesSent = 0;
            byte[] buf = new byte[ChunkSize];
            int toSend;
            int cnt;
            while ((toSend = length - bytesSent) > 0
                && (cnt = stream.Read(buf, 0, (toSend > buf.Length ? buf.Length : toSend))) > 0)
            {
                await writer.BaseStream.WriteAsync(buf, 0, cnt);
                await writer.FlushAsync();
                bytesSent += cnt;
            }

            return bytesSent;
        }

        private int ReadBytes(StreamWriter writer, Stream stream, int length)
        {

            int bytesSent = 0;
            byte[] buf = new byte[ChunkSize];
            int toSend;
            int cnt;
            while ((toSend = length - bytesSent) > 0
                && (cnt = stream.Read(buf, 0, (toSend > buf.Length ? buf.Length : toSend))) > 0)
            {
                writer.BaseStream.Write(buf, 0, cnt);
                writer.Flush();
                bytesSent += cnt;
            }

            return bytesSent;
        }

        private static void SetHttpMethod(HttpMethod method, HttpRequestMessage req)
        {
            switch (method)
            {
                case HttpMethod.DELETE:
                    req.Method = System.Net.Http.HttpMethod.Delete;
                    break;
                case HttpMethod.GET:
                    req.Method = System.Net.Http.HttpMethod.Get;
                    break;
                case HttpMethod.POST:
                    req.Method = System.Net.Http.HttpMethod.Post;
                    break;
                case HttpMethod.PUT:
                    req.Method = System.Net.Http.HttpMethod.Put;
                    break;
                default:
                    req.Method = System.Net.Http.HttpMethod.Get;
                    break;
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
                throw new ArgumentException("Enum fields must be decorated with EnumMemberAttribute!");

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
                    continue;

                if (s == attrs[0].Value)
                    return (T)fi.GetValue(null);
            }

            return default(T);
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
                parameters = new SortedDictionary<string, object>();

            if (!(parameters is SortedDictionary<string, object>))
                parameters = new SortedDictionary<string, object>(parameters);

            string path = "";
            if (parameters.ContainsKey("callback") && parameters["callback"] != null)
                path = parameters["callback"].ToString();

            try
            {
                parameters["callback"] = BuildCallbackUrl(path);
            }
            catch (Exception)
            {

            }

            if (!parameters.ContainsKey("unsigned") || parameters["unsigned"].ToString() == "false")
                FinalizeUploadParameters(parameters);

            return JsonConvert.SerializeObject(parameters);
        }

        /// <summary>
        /// Calculates signature of parameters.
        /// </summary>
        /// <param name="parameters">Parameters to sign.</param>
        /// <returns>Signature of parameters.</returns>
        public string SignParameters(IDictionary<string, object> parameters)
        {
            List<string> excludedSignatureKeys = new List<string>(new string[] { "resource_type", "file", "api_key" });
            StringBuilder signBase = new StringBuilder(String.Join("&", parameters.
                                                                   Where(pair => pair.Value != null && !excludedSignatureKeys.Any(s => pair.Key.Equals(s)))
                .Select(pair => String.Format("{0}={1}", pair.Key,
                    pair.Value is IEnumerable<string>
                    ? String.Join(",", ((IEnumerable<string>)pair.Value).ToArray())
                    : pair.Value.ToString()))
                .ToArray()));

            signBase.Append(Account.ApiSecret);

            var hash = Utils.ComputeHash(signBase.ToString());
            StringBuilder sign = new StringBuilder();
            foreach (byte b in hash) sign.Append(b.ToString("x2"));

            return sign.ToString();
        }

        /// <summary>
        /// Signs the specified URI part.
        /// </summary>
        /// <param name="uriPart">The URI part.</param>
        /// <returns>Signature of the URI part.</returns>
        public string SignUriPart(string uriPart)
        {
            var hash = Utils.ComputeHash(uriPart + Account.ApiSecret);
            return "s--" + Utils.EncodeUrlSafe(hash).Substring(0, 8) + "--/";
        }

        /// <summary>
        /// Validates API response signature against Cloudinary configuration
        /// </summary>
        /// <param name="publicId">Public ID of resource</param>
        /// <param name="version">Version of resource</param>
        /// <param name="signature">Response signature</param>
        /// <returns>Boolean result of the validation</returns>
        public bool VerifyApiResponseSignature(string publicId, string version, string signature)
        {
            var parametersToSign = new SortedDictionary<string, object>() {
                { "public_id", publicId},
                { "version", version}
            };
            var signedParameters = SignParameters(parametersToSign);

            return signature.Equals(signedParameters);
        }

        /// <summary>
        /// Validates notification signature against Cloudinary configuration
        /// </summary>
        /// <param name="body">Request body</param>
        /// <param name="timestamp">Request timestamp</param>
        /// <param name="signature">Notification signature</param>
        /// <param name="validFor">For how long the signature is valid, in seconds</param>
        /// <returns>Boolean result of the validation</returns>
        public bool VerifyNotificationSignature(string body, long timestamp, string signature, int validFor = 7200)
        {
            var currentTimestamp = Utils.UnixTimeNowSeconds();
            var isSignatureExpired = timestamp <= currentTimestamp - validFor;
            if (isSignatureExpired)
                return false;

            var payloadHash = Utils.ComputeHexHash($"{body}{timestamp}{Account.ApiSecret}");

            return signature.Equals(payloadHash);
        }

        /// <summary>
        /// Calculates current UNIX time.
        /// </summary>
        /// <returns>Amount of seconds from 1 january 1970.</returns>
        private string GetTime()
        {
            return Convert.ToInt64(((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds)).ToString();
        }

        /// <summary>
        /// Virtual build callback URL method. This method should be overridden in child classes.
        /// </summary>
        /// <returns>Callback URL.</returns>
        public virtual string BuildCallbackUrl(string path = "")
        {
            return string.Empty;
        }

        /// <summary>
        /// Build unsigned upload params with defined preset.
        /// </summary>
        /// <param name="preset">The name of an upload preset defined for your Cloudinary account.</param>
        /// <param name="parameters">Cloudinary upload parameters.</param>
        /// <returns>Unsigned cloudinary parameters with upload preset included.</returns>
        protected SortedDictionary<string, object> BuildUnsignedUploadParams(string preset, SortedDictionary<string, object> parameters = null)
        {
            if (parameters == null)
                parameters = new SortedDictionary<string, object>();

            parameters.Add("upload_preset", preset);
            parameters.Add("unsigned", true);

            return parameters;
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
                htmlOptions = new Dictionary<string, string>();

            if (String.IsNullOrEmpty(resourceType))
                resourceType = "auto";

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
                builder.Append(" ").Append(htmlOptions["class"]);
            }

            foreach (var item in htmlOptions)
            {
                if (item.Key == "class") continue;

                builder.
                    Append("' ").
                    Append(item.Key).
                    Append("='").
                    Append(EncodeApiUrl(item.Value));
            }

            builder.Append("'/>");

            return builder.ToString();
        }

        /// <summary>
        /// Virtual encode API URL method. This method should be overridden in child classes.
        /// </summary>
        /// <param name="value">URL to be encoded.</param>
        /// <returns>Encoded URL.</returns>
        protected string EncodeApiUrl(string value)
        {
            return HtmlEncoder.Default.Encode(value);
        }

        /// <summary>
        /// Check 'unsigned' parameter value and add signature into parameters if unsigned=false or not specified.
        /// </summary>
        /// <param name="parameters">Parameters to check signature.</param>
        protected void HandleUnsignedParameters(IDictionary<string, object> parameters)
        {
            if (!parameters.ContainsKey("unsigned") || parameters["unsigned"].ToString() == "false")
                FinalizeUploadParameters(parameters);
            else if (parameters.ContainsKey("removeUnsignedParam"))
            {
                parameters.Remove("unsigned");
                parameters.Remove("removeUnsignedParam");
            }
        }

        /// <summary>
        /// Serialize the cloudinary parameters to JSON.
        /// </summary>
        /// <param name="parameters">Parameters to serialize.</param>
        /// <returns>Serialized parameters as JSON string.</returns>
        protected string ParamsToJson(SortedDictionary<string, object> parameters)
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            var sb = new StringBuilder();
            var sw = new StringWriter(sb);

            using (var writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, parameters);
            }

            return sb.ToString();
        }

        internal void FinalizeUploadParameters(IDictionary<string, object> parameters)
        {
            parameters.Add("timestamp", GetTime());
            parameters.Add("signature", SignParameters(parameters));
            parameters.Add("api_key", Account.ApiKey);
        }

        /// <summary>
        /// Write cloudinary parameter to the request stream.
        /// </summary>
        /// <param name="writer">Stream writer.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The parameter value.</param>
        protected void WriteParam(StreamWriter writer, string key, string value)
        {
#if DEBUG
            Console.WriteLine(String.Format("{0}: {1}", key, value));
#endif
            WriteLine(writer, "--{0}", HTTP_BOUNDARY);
            WriteLine(writer, "Content-Disposition: form-data; name=\"{0}\"", key);
            WriteLine(writer);
            WriteLine(writer, value);
        }

        /// <summary>
        /// Write cloudinary parameter to the request stream.
        /// </summary>
        /// <param name="writer">Stream writer.</param>
        /// <param name="file">File to be written to the stream.</param>
        protected void WriteFile(StreamWriter writer, FileDescription file)
        {
            if (file.IsRemote)
            {
                WriteParam(writer, "file", file.FilePath);
            }
            else
            {
                if (file.Stream == null)
                {
                    using (FileStream stream = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        stream.Seek(file.BytesSent, SeekOrigin.Begin);
                        file.BytesSent += WriteFile(writer, stream, file.BufferLength, file.FileName);
                    }
                }
                else
                {
                    file.BytesSent += WriteFile(writer, file.Stream, file.BufferLength, file.FileName);
                }
            }
        }

        /// <summary>
        /// Writes one chunk of file to stream.
        /// </summary>
        /// <param name="writer">Output writer.</param>
        /// <param name="fileName">Name of file.</param>
        /// <param name="stream">Input stream.</param>
        /// <param name="length">Maximum amount of bytes to send.</param>
        /// <returns>Amount of sent bytes.</returns>
        private int WriteFile(StreamWriter writer, Stream stream, int length, string fileName)
        {
            WriteLine(writer, "--{0}", HTTP_BOUNDARY);
            WriteLine(writer, "Content-Disposition: form-data;  name=\"file\"; filename=\"{0}\"", fileName);
            WriteLine(writer, "Content-Type: application/octet-stream");
            WriteLine(writer);

            writer.Flush();

            int bytesSent = 0;
            byte[] buf = new byte[ChunkSize];
            int toSend;
            int cnt;
            while ((toSend = length - bytesSent) > 0
                && (cnt = stream.Read(buf, 0, (toSend > buf.Length ? buf.Length : toSend))) > 0)
            {
                writer.BaseStream.Write(buf, 0, cnt);
                bytesSent += cnt;
            }

            return bytesSent;
        }

        private void WriteLine(StreamWriter writer)
        {
            writer.Write("\r\n");
        }

        private void WriteLine(StreamWriter writer, string format)
        {
            writer.Write(format);
            writer.Write("\r\n");
        }

        private void WriteLine(StreamWriter writer, string format, Object val)
        {
            writer.Write(format, val);
            writer.Write("\r\n");
        }
    }

    /// <summary>
    /// Digital signature provider.
    /// </summary>
    public interface ISignProvider
    {
        /// <summary>
        /// Generate digital signature for parameters.
        /// </summary>
        /// <param name="parameters">The parameters to sign.</param>
        /// <returns>Generated signature.</returns>
        string SignParameters(IDictionary<string, object> parameters);

        /// <summary>
        /// Generate digital signature for part of an URI.
        /// </summary>
        /// <param name="uriPart">The part of an URI to sign.</param>
        /// <returns>Generated signature.</returns>
        string SignUriPart(string uriPart);
    }

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
        PUT
    }
}

