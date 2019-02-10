using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;


namespace CloudinaryDotNet
{
    /// <summary>
    /// Technological layer to work with cloudinary API
    /// </summary>
    public class Api : ApiShared
    {
        private Func<string, HttpWebRequest> RequestBuilder = (x) => HttpWebRequest.Create(x) as HttpWebRequest;

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
            var version = new AssemblyName(typeof(Api).Assembly.FullName).Version;

            USER_AGENT = $"CloudinaryDotNet/{version.Major}.{version.Minor}.{version.Build} (.NET Framework 4)";
        }

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

        /// <summary>
        /// Makes custom call to Cloudinary API.
        /// </summary>
        /// <param name="method">HTTP method of call</param>
        /// <param name="url">URL to call</param>
        /// <param name="parameters">Dictionary of call parameters (can be null)</param>
        /// <param name="file">File to upload (must be null for non-uploading actions)</param>
        /// <param name="extraHeaders">Headers to add to the request</param>
        /// <returns>HTTP response on call</returns>
        public HttpWebResponse Call(HttpMethod method, string url, SortedDictionary<string, object> parameters, FileDescription file, Dictionary<string, string> extraHeaders = null)
        {
            HttpWebRequest request = RequestBuilder(url);
            HttpWebResponse response = null;
            if (Timeout > 0)
            {
                request.Timeout = Timeout;
            }

            PrepareRequestBody(request, method, parameters, file, extraHeaders);

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
                if (response == null) throw;
            }
            return response;
        }
        internal HttpWebRequest PrepareRequestBody(HttpWebRequest request, HttpMethod method, SortedDictionary<string, object> parameters, FileDescription file, Dictionary<string, string> extraHeaders = null)
        {
            SetHttpMethod(method, request);

            // Add platform information to the USER_AGENT header
            // This is intended for platform information and not individual applications!
            request.UserAgent = string.IsNullOrEmpty(UserPlatform)
                ? USER_AGENT
                : string.Format("{0} {1}", UserPlatform, USER_AGENT);

            byte[] authBytes = Encoding.ASCII.GetBytes(String.Format("{0}:{1}", Account.ApiKey, Account.ApiSecret));
            request.Headers.Add("Authorization", String.Format("Basic {0}", Convert.ToBase64String(authBytes)));

            if (extraHeaders != null)
            {
                if (extraHeaders.ContainsKey("Content-Type"))
                {
                    request.ContentType = extraHeaders["Content-Type"];
                    extraHeaders.Remove("Content-Type");
                }

                foreach (var header in extraHeaders)
                {
                    request.Headers[header.Key] = header.Value;
                }
            }

            if ((method == HttpMethod.POST || method == HttpMethod.PUT) && parameters != null)
            {
                request.AllowWriteStreamBuffering = false;
                request.AllowAutoRedirect = false;

                if (UseChunkedEncoding)
                    request.SendChunked = true;

                PrepareRequestContent(ref request, parameters, file);
            }

            return request;
        }

        private void PrepareRequestContent(ref HttpWebRequest request, SortedDictionary<string, object> parameters,
            FileDescription file)
        {
            if ( request == null)
            {
                return;
            }

            HandleUnsignedParameters(parameters);

            if (request.ContentType == Constants.CONTENT_TYPE_APPLICATION_JSON)
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(ParamsToJson(parameters));
                }
            }
            else
            {
                PrepareMultipartFormDataContent(request, parameters, file);
            }
        }

        private void PrepareMultipartFormDataContent(HttpWebRequest request, SortedDictionary<string, object> parameters, FileDescription file)
        {
            request.ContentType = "multipart/form-data; boundary=" + HTTP_BOUNDARY;

            using (Stream requestStream = request.GetRequestStream())
            {
                using (StreamWriter writer = new StreamWriter(requestStream))
                {
                    foreach (var param in parameters)
                    {
                        if (param.Value != null)
                        {
                            if (param.Value is IEnumerable<string>)
                            {
                                foreach (var item in (IEnumerable<string>)param.Value)
                                {
                                    WriteParam(writer, param.Key + "[]", item);
                                }
                            }
                            else
                            {
                                WriteParam(writer, param.Key, param.Value.ToString());
                            }
                        }
                    }

                    if (file != null)
                    {
                        WriteFile(writer, file);
                    }

                    writer.Write("--{0}--", HTTP_BOUNDARY);

                }
            }
        }
        public override string BuildCallbackUrl(string path = "")
        {
            if (String.IsNullOrEmpty(path))
                path = "/Content/cloudinary_cors.html";

            if (Regex.IsMatch(path.ToLower(), "^https?:/.*"))
            {
                return path;
            }
            else
            {
                if (HttpContext.Current != null)
                    return new Uri(HttpContext.Current.Request.Url, path).ToString();
                else
                    throw new HttpException("Http context is not set. Either use this method in the right context or provide an absolute path to file!");
            }
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
        public IHtmlString BuildUnsignedUploadForm(string field, string preset, string resourceType, SortedDictionary<string, object> parameters = null, Dictionary<string, string> htmlOptions = null)
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
        public IHtmlString BuildUploadForm(string field, string resourceType, SortedDictionary<string, object> parameters = null, Dictionary<string, string> htmlOptions = null)
        {
            return new HtmlString(BuildUploadFormShared(field, resourceType, parameters, htmlOptions));
        }

        protected override string EncodeApiUrl(string value)
        {
            return HttpUtility.HtmlEncode(value);
        }

        private static void SetHttpMethod(HttpMethod method, HttpWebRequest req)
        {
            req.Method = Enum.GetName(typeof(HttpMethod), method);
        }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static T Parse<T>(Object response) where T : BaseResult, new()
        {
            if (response == null)
                throw new ArgumentNullException("response");

            HttpWebResponse message = (HttpWebResponse)response;

            T result;

            using (Stream stream = message.GetResponseStream())
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
                foreach (var header in message.Headers.AllKeys)
                {
                    if (header.StartsWith("X-FeatureRateLimit"))
                    {
                        long l;
                        DateTime t;

                        if (header.EndsWith("Limit") && long.TryParse(message.Headers[header], out l))
                            result.Limit = l;

                        if (header.EndsWith("Remaining") && long.TryParse(message.Headers[header], out l))
                            result.Remaining = l;

                        if (header.EndsWith("Reset") && DateTime.TryParse(message.Headers[header], out t))
                            result.Reset = t;
                    }
                }

            result.StatusCode = message.StatusCode;

            return result;
        }
    }
}
