using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json;
using CloudinaryShared.Core;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Technological layer to work with cloudinary API
    /// </summary>
    public class Api : ApiShared
    {
        public Func<string, HttpWebRequest> RequestBuilder = (x) => HttpWebRequest.Create(x) as HttpWebRequest;

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
            var version = Assembly.GetExecutingAssembly().GetName().Version;
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
        public override object Call(HttpMethod method, string url, SortedDictionary<string, object> parameters, FileDescription file, Dictionary<string, string> extraHeaders = null)
        {
#if DEBUG
            Console.WriteLine(String.Format("{0} REQUEST:", method));
            Console.WriteLine(url);
#endif

            HttpWebRequest request = RequestBuilder(url);
            request.Method = Enum.GetName(typeof(HttpMethod), method);
            // Add platform information to the USER_AGENT header
            // This is intended for platform information and not individual applications!
            request.UserAgent = string.IsNullOrEmpty(UserPlatform)
                ? USER_AGENT
                : string.Format("{0} {1}", UserPlatform, USER_AGENT);

            if (Timeout > 0)
            {
                request.Timeout = Timeout;
            }
            byte[] authBytes = Encoding.ASCII.GetBytes(String.Format("{0}:{1}", Account.ApiKey, Account.ApiSecret));
            request.Headers.Add("Authorization", String.Format("Basic {0}", Convert.ToBase64String(authBytes)));

            if (extraHeaders != null)
            {
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

                request.ContentType = "multipart/form-data; boundary=" + HTTP_BOUNDARY;

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

            try
            {
                return (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;
                if (response == null) throw;
                else return response;
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
        /// Builds HTML form
        /// </summary>
        /// <returns>HTML form</returns>
        public IHtmlString BuildUnsignedUploadForm(string field, string preset, string resourceType, SortedDictionary<string, object> parameters = null, Dictionary<string, string> htmlOptions = null)
        {
            return BuildUploadForm(field, resourceType, BuildUnsignedUploadParams(preset, parameters), htmlOptions);
        }

        /// <summary>
        /// Builds HTML form
        /// </summary>
        /// <returns>HTML form</returns>
        public IHtmlString BuildUploadForm(string field, string resourceType, SortedDictionary<string, object> parameters = null, Dictionary<string, string> htmlOptions = null)
        {
            return new HtmlString(BuildUploadFormShared(field, resourceType, parameters, htmlOptions));
        }

        protected override string EncodeApiUrl(string value)
        {
            return HttpUtility.HtmlEncode(value);
        }
    }
}
