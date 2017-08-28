using CloudinaryDotNet.Actions;
using CloudinaryShared.Core;
using Coudinary.NetCoreShared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Technological layer to work with cloudinary API
    /// </summary>
    public class Api : ApiShared
    {
        public Func<HttpRequestMessage> RequestBuilder = () => new HttpRequestMessage();

        /// <summary>
        /// Default parameterless constructor.
        /// Assumes that environment variable CLOUDINARY_URL is set.
        /// </summary>
        public Api() : base()
        {

        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="cloudinaryUrl">Cloudinary URL</param>
        public Api(string cloudinaryUrl) : base(cloudinaryUrl)
        {
        }

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="account">Cloudinary account</param>
        /// <param name="usePrivateCdn">Whether to use private Content Delivery Network</param>
        /// <param name="privateCdn">Private Content Delivery Network</param>
        /// <param name="shortenUrl">Whether to use shorten url when possible.</param>
        /// <param name="cSubDomain">if set to <c>true</c> [c sub domain].</param>
        public Api(Account account, bool usePrivateCdn, string privateCdn, bool shortenUrl, bool cSubDomain) : base(account, usePrivateCdn, privateCdn, shortenUrl, cSubDomain)
        {
        }

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="account">Cloudinary account</param>
        public Api(Account account) : base(account)
        {
        }

        static Api()
        {
            var version = typeof(Api).GetTypeInfo().Assembly.GetName().Version;
            USER_AGENT = String.Format("CloudinaryDotNet/{0}.{1}.{2}",
                version.Major, version.Minor, version.Build);
        }

        /// <summary>
        /// Custom call to cloudinary API
        /// </summary>
        /// <param name="method">HTTP method of call</param>
        /// <param name="url">URL to call</param>
        /// <param name="parameters">Dictionary of call parameters (can be null)</param>
        /// <param name="file">File to upload (must be null for non-uploading actions)</param>
        /// <returns>HTTP response on call</returns>
        //public HttpWebResponse Call(HttpMethod method, string url, SortedDictionary<string, object> parameters, FileDescription file)
        public override object InternalCall(CloudinaryShared.Core.HttpMethod method, string url, SortedDictionary<string, object> parameters, FileDescription file, Dictionary<string, string> extraHeaders = null)
        {
            using (HttpClient client = new HttpClient())
            {
                if (Timeout > 0)
                {
                    client.Timeout = TimeSpan.FromMilliseconds(Timeout);
                }

                var request = PrepareRequestBody(method, url, parameters, file, extraHeaders);

                var task2 = client.SendAsync(request);
                task2.Wait();

                if (task2.IsCanceled) { }

                if (task2.IsFaulted) { throw task2.Exception; }

                return task2.Result;
            }
        }


        /// <summary>
        /// Custom call to cloudinary API
        /// </summary>
        /// <param name="method">HTTP method of call</param>
        /// <param name="url">URL to call</param>
        /// <param name="parameters">Dictionary of call parameters (can be null)</param>
        /// <param name="file">File to upload (must be null for non-uploading actions)</param>
        /// <returns>HTTP response on call</returns>
        //public HttpWebResponse Call(HttpMethod method, string url, SortedDictionary<string, object> parameters, FileDescription file)
        public  HttpResponseMessage Call(CloudinaryShared.Core.HttpMethod method, string url, SortedDictionary<string, object> parameters, FileDescription file, Dictionary<string, string> extraHeaders = null)
        {
            using (HttpClient client = new HttpClient())
            {
                if (Timeout > 0)
                {
                    client.Timeout = TimeSpan.FromMilliseconds(Timeout);
                }

                var request = PrepareRequestBody(method, url, parameters, file, extraHeaders);

                var task2 = client.SendAsync(request);
                task2.Wait();

                if (task2.IsCanceled) { }

                if (task2.IsFaulted) { throw task2.Exception; }

                return task2.Result;
            }
        }

        public HttpRequestMessage PrepareRequestBody(CloudinaryShared.Core.HttpMethod method, string url, SortedDictionary<string, object> parameters, FileDescription file, Dictionary<string, string> extraHeaders = null)
        {
            var req = RequestBuilder();

            req.RequestUri = new Uri(url);
            switch (method)
            {
                case CloudinaryShared.Core.HttpMethod.DELETE:
                    req.Method = System.Net.Http.HttpMethod.Delete;
                    break;
                case CloudinaryShared.Core.HttpMethod.GET:
                    req.Method = System.Net.Http.HttpMethod.Get;
                    break;
                case CloudinaryShared.Core.HttpMethod.POST:
                    req.Method = System.Net.Http.HttpMethod.Post;
                    break;
                case CloudinaryShared.Core.HttpMethod.PUT:
                    req.Method = System.Net.Http.HttpMethod.Put;
                    break;
                default:
                    req.Method = System.Net.Http.HttpMethod.Get;
                    break;
            }

            req.Headers.Add("User-Agent", string.IsNullOrEmpty(UserPlatform) ? USER_AGENT : string.Format("{0} {1}", UserPlatform, USER_AGENT));

            byte[] _authBytes = Encoding.ASCII.GetBytes(String.Format("{0}:{1}", Account.ApiKey, Account.ApiSecret));
            req.Headers.Add("Authorization", String.Format("Basic {0}", Convert.ToBase64String(_authBytes)));

            if (extraHeaders != null)
            {
                if (extraHeaders.ContainsKey("Content-Type"))
                {
                    req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    extraHeaders.Remove("Content-Type");
                }
                
                
                foreach (var header in extraHeaders)
                {
                    req.Headers.Add(header.Key, header.Value);
                }
            }

            if ((method == CloudinaryShared.Core.HttpMethod.POST || method == CloudinaryShared.Core.HttpMethod.PUT) && parameters != null)
            {
                if (UseChunkedEncoding)
                    req.Headers.Add("Transfer-Encoding", "chunked");

                req.Content = PrepareRequestContent(parameters, file);
            }

            return req;
        }
 

        private MultipartFormDataContent PrepareRequestContent(SortedDictionary<string, object> parameters, FileDescription file)
        {
            var content = new MultipartFormDataContent(HTTP_BOUNDARY);

            if (!parameters.ContainsKey("unsigned") || parameters["unsigned"].ToString() == "false")
                FinalizeUploadParameters(parameters);
            else
            {
                if (parameters.ContainsKey("removeUnsignedParam"))
                {
                    parameters.Remove("unsigned");
                    parameters.Remove("removeUnsignedParam");
                }
            }

            var task1 = content.ReadAsStreamAsync();
            task1.Wait();

            Stream requestStream = task1.Result;
            
            StreamWriter writer = new StreamWriter(requestStream);

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
            
            writer.Flush();

            return content;
        }
        
        public override string BuildCallbackUrl(string path = "")
        {
            if (!Regex.IsMatch(path.ToLower(), "^https?:/.*"))
            {
                throw new Exception("Provide an absolute path to file!");
            }
            return path;
        }

        /// <summary>
        /// Builds HTML form
        /// </summary>
        /// <returns>HTML form</returns>
        public string BuildUnsignedUploadForm(string field, string preset, string resourceType, SortedDictionary<string, object> parameters = null, Dictionary<string, string> htmlOptions = null)
        {
            return BuildUploadForm(field, resourceType, BuildUnsignedUploadParams(preset, parameters), htmlOptions);
        }

        /// <summary>
        /// Builds HTML form
        /// </summary>
        /// <returns>HTML form</returns>
        public string BuildUploadForm(string field, string resourceType, SortedDictionary<string, object> parameters = null, Dictionary<string, string> htmlOptions = null)
        {
            return BuildUploadFormShared(field, resourceType, parameters, htmlOptions);
        }

        protected override string EncodeApiUrl(string value)
        {
            return HtmlEncoder.Default.Encode(value);
        }
    }
}
