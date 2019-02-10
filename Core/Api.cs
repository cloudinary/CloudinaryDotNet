using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Technological layer to work with cloudinary API.
    /// </summary>
    public class Api : ApiShared
    {
        static HttpClient client = new HttpClient();

        private Func<string, HttpRequestMessage> RequestBuilder =
            (url) => new HttpRequestMessage {RequestUri = new Uri(url)};

        /// <summary>
        /// Default parameterless constructor. Assumes that environment variable CLOUDINARY_URL is set.
        /// </summary>
        public Api() : base() 
        { 
        }

        /// <summary>
        /// Instantiates the cloudinary <see cref="Api"/> object with cloudinary Url.
        /// </summary>
        /// <param name="cloudinaryUrl">Cloudinary URL.</param>
        public Api(string cloudinaryUrl) : base(cloudinaryUrl)
        {
        }

        /// <summary>
        /// Instantiates the cloudinary <see cref="Api"/> object with initial parameters.
        /// </summary>
        /// <param name="account">Cloudinary account.</param>
        /// <param name="usePrivateCdn">Whether to use private Content Delivery Network.</param>
        /// <param name="privateCdn">Private Content Delivery Network.</param>
        /// <param name="shortenUrl">Whether to use shorten url when possible.</param>
        /// <param name="cSubDomain">Whether to use sub domain.</param>
        public Api(Account account, bool usePrivateCdn, string privateCdn, bool shortenUrl, bool cSubDomain) 
            : base(account, usePrivateCdn, privateCdn, shortenUrl, cSubDomain)
        {
        }

        /// <summary>
        /// Instantiates the cloudinary <see cref="Api"/> object with account.
        /// </summary>
        /// <param name="account">Cloudinary account.</param>
        public Api(Account account) : base(account)
        {
        }

        /// <summary>
        /// Default static parameterless constructor.
        /// </summary>
        static Api()
        {
            var version = typeof(Api).GetTypeInfo().Assembly.GetName().Version;

            var frameworkDescription = RuntimeInformation.FrameworkDescription;

            USER_AGENT = String.Format("CloudinaryDotNet/{0}.{1}.{2} ({3})",
                version.Major, version.Minor, version.Build, frameworkDescription);
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
                    Stream stream;

                    if (file.Stream != null)
                    {
                        stream = file.Stream;
                    }
                    else
                    {
                        stream = File.OpenRead(file.FilePath);
                    }

                    if (extraHeaders != null && extraHeaders.ContainsKey("Content-Range"))
                    {
                        // Unfortunately we don't have ByteRangeStreamContent here,
                        // let's create another stream from the original one
                        stream =  GetRangeFromFile(file, stream);
                    }

                    var streamContent = new StreamContent(stream);

                    streamContent.Headers.Add("Content-Type", "application/octet-stream");
                    streamContent.Headers.Add("Content-Disposition", "form-data; name=\"file\"; filename=\"" + file.FileName + "\"");
                    content.Add(streamContent, "file", file.FileName);
                }
            }

            return content;
        }

        /// <summary>
        /// Check file path for callback url.
        /// </summary>
        /// <param name="path">File path to check.</param>
        /// <returns>Provided path if it matches the callback url format.</returns>
        public override string BuildCallbackUrl(string path = "")
        {
            if (!Regex.IsMatch(path.ToLower(), "^https?:/.*"))
            {
                throw new Exception("Provide an absolute path to file!");
            }
            return path;
        }

        /// <summary>
        /// Builds HTML file input tag for unsigned upload an asset.
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
        /// Encode url to a representation that is unambiguous and universally accepted by web browsers and servers.
        /// </summary>
        /// <param name="value">The url to encode.</param>
        /// <returns>Encoded url.</returns>
        protected override string EncodeApiUrl(string value)
        {
            return HtmlEncoder.Default.Encode(value);
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


        private Stream GetRangeFromFile(FileDescription file, Stream stream)
        {
            MemoryStream memStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(memStream);

            int bytesSent = 0;
            stream.Seek(file.BytesSent, SeekOrigin.Begin);
            file.Eof = ReadBytes(writer, stream, file.BufferLength, file.FileName, out bytesSent);
            file.BytesSent += bytesSent;
            writer.BaseStream.Seek(0, SeekOrigin.Begin);

            return memStream;
        }

        private bool ReadBytes(StreamWriter writer, Stream stream, int length, string fileName, out int bytesSent)
        {

            bytesSent = 0;
            int toSend = 0;
            byte[] buf = new byte[ChunkSize];
            int cnt = 0;

            while ((toSend = length - bytesSent) > 0
                   && (cnt = stream.Read(buf, 0, (toSend > buf.Length ? buf.Length : toSend))) > 0)
            {
                writer.BaseStream.Write(buf, 0, cnt);
                writer.Flush();
                bytesSent += cnt;
            }

            return cnt == 0;
        }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class.
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <returns>New instance of this class.</returns>
        internal static T Parse<T>(Object response) where T : BaseResult, new()
        {
            if (response == null)
                throw new ArgumentNullException("response");

            HttpResponseMessage message = (HttpResponseMessage)response;

            T result;

            var task = message.Content.ReadAsStreamAsync();
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
                    throw new Exception($"Failed to deserialize response with status code: {message.StatusCode}", jex);
                }
            }

            if (message.Headers != null)
                foreach (var header in message.Headers)
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

            result.StatusCode = message.StatusCode;

            return result;
        }

        /// <summary>
        /// Call the Cloudinary API and parse HTTP response.
        /// </summary>
        /// <typeparam name="T">The type of the parsed response.</typeparam>
        /// <param name="method">HTTP method.</param>
        /// <param name="url">A generated Url.</param>
        /// <param name="parameters">A dictionary of parameters in cloudinary notation.</param>
        /// <param name="file">The file to upload.</param>
        /// <param name="extraHeaders">The extra headers to pass into the request.</param>
        /// <returns>Instance of the parsed response from the cloudinary API.</returns>
        public override T CallAndParse<T>(HttpMethod method, string url, SortedDictionary<string, object> parameters, FileDescription file,
            Dictionary<string, string> extraHeaders = null)
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
    }
}
