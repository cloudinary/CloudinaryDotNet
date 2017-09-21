using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Cloudinary.NetCoreShared;
using CloudinaryShared.Core;
using HttpMethod = CloudinaryShared.Core.HttpMethod;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Technological layer to work with cloudinary API
    /// </summary>
    public class Api : ApiShared
    {
        static HttpClient client = new HttpClient();

        public Func<string, HttpRequestMessage> RequestBuilder =
            (url) => new HttpRequestMessage {RequestUri = new Uri(url)};
        
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

        /// <summary>
        /// Initializes the <see cref="Api"/> class.
        /// </summary>
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
        public override object InternalCall(HttpMethod method, string url, SortedDictionary<string, object> parameters, FileDescription file, Dictionary<string, string> extraHeaders = null)
        {
            var request = PrepareRequestBody(method, url, parameters, file, extraHeaders);
                
            if (Timeout > 0)
            {
                client.Timeout = TimeSpan.FromMilliseconds(Timeout);
            }

                
            var task2 = client.SendAsync(request);
            task2.Wait();

            if (task2.IsCanceled) { }

            if (task2.IsFaulted) { throw task2.Exception; }

            return task2.Result;
        }

        /// <summary>
        /// Custom call to cloudinary API
        /// </summary>
        /// <param name="method">HTTP method of call</param>
        /// <param name="url">URL to call</param>
        /// <param name="parameters">Dictionary of call parameters (can be null)</param>
        /// <param name="file">File to upload (must be null for non-uploading actions)</param>
        /// <returns>HTTP response on call</returns>
        public HttpResponseMessage Call(HttpMethod method, string url, SortedDictionary<string, object> parameters, FileDescription file, Dictionary<string, string> extraHeaders = null)
        {
            var request = PrepareRequestBody(method, url, parameters, file, extraHeaders);
            
            if (Timeout > 0)
            {
                client.Timeout = TimeSpan.FromMilliseconds(Timeout);
            }

            
            var task = client.SendAsync(request);
            task.Wait();

            if (task.IsCanceled) { }
            if (task.IsFaulted) { throw task.Exception; }

            return task.Result;
        
        }

        public HttpRequestMessage PrepareRequestBody(HttpMethod method, string url, SortedDictionary<string, object> parameters, FileDescription file, Dictionary<string, string> extraHeaders = null)
        {
            var req = RequestBuilder(url);

            SetHttpMethod(method, req);

            // Add platform information to the USER_AGENT header
            // This is intended for platform information and not individual applications!
            req.Headers.Add("User-Agent", string.IsNullOrEmpty(UserPlatform)
                ? USER_AGENT
                : string.Format("{0} {1}", UserPlatform, USER_AGENT));

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

            if ((method == HttpMethod.POST || method == HttpMethod.PUT) && parameters != null)
            {
                if (UseChunkedEncoding)
                    req.Headers.Add("Transfer-Encoding", "chunked");

                PrepareRequestContent(ref req, parameters, file);
            }
            
            return req;
        }

        private void PrepareRequestContent(ref HttpRequestMessage request, SortedDictionary<string, object> parameters, FileDescription file)
        {
            HttpRequestMessage req = (HttpRequestMessage) request;
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
                if (file.IsRemote && file.Stream == null)
                {
                    StringContent strContent = new StringContent(file.FilePath);
                    strContent.Headers.Add("Content-Disposition", string.Format("form-data; name=\"{0}\"", "file"));
                    content.Add(strContent);
                }
                else
                {
                    Stream stream = null;

                    if (file.Stream != null)
                    {
                        stream = file.Stream;
                    }
                    else
                    {
                        stream = File.OpenRead(file.FilePath);
                    }

                    string fileName = string.IsNullOrWhiteSpace(file.FilePath) ? file.FileName : Path.GetFileName(file.FilePath);

                    var streamContent = new StreamContent(stream);
                    streamContent.Headers.Add("Content-Type", "application/octet-stream");
                    streamContent.Headers.Add("Content-Disposition", "form-data; name=\"file\"; filename=\"" + fileName + "\"");
                    content.Add(streamContent, "file", fileName);
                }
            }

            req.Content = content;
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


        private byte[] GetRangeFromFile(FileDescription file, Stream stream)
        {
            MemoryStream memStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(memStream);

            int bytesSent = 0;
            stream.Seek(file.BytesSent, SeekOrigin.Begin);
            file.EOF = ReadBytes(writer, stream, file.BufferLength, file.FileName, out bytesSent);
            file.BytesSent += bytesSent;
            byte[] buff = new byte[bytesSent];
            writer.BaseStream.Seek(0, SeekOrigin.Begin);
            writer.BaseStream.Read(buff, 0, bytesSent);

            return buff;
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


    }
    
}
