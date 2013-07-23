using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Technological layer to work with cloudinary API
    /// </summary>
    public class Api
    {
        public const string ADDR_API = "api.cloudinary.com";
        public const string ADDR_RES = "res.cloudinary.com";
        public const string API_VERSION = "v1_1";
        public const string HTTP_BOUNDARY = "notrandomsequencetouseasboundary";

        bool m_shortenUrl;
        bool m_usePrivateCdn;
        string m_privateCdn;
        string m_apiAddr = "https://" + ADDR_API;
        SHA1 m_hasher;

        /// <summary>
        /// Default parameterless constructor.
        /// Assumes that environment variable CLOUDINARY_URL is set.
        /// </summary>
        public Api()
            : this(Environment.GetEnvironmentVariable("CLOUDINARY_URL")) { }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="cloudinaryUrl">Cloudinary URL</param>
        public Api(string cloudinaryUrl)
        {
            if (String.IsNullOrEmpty(cloudinaryUrl))
                throw new ArgumentException("Valid cloudinary init string must be provided!");

            Uri cloudinaryUri = new Uri(cloudinaryUrl);

            if (String.IsNullOrEmpty(cloudinaryUri.Host))
                throw new ArgumentException("Cloud name must be specified as host name in URL!");

            string[] creds = cloudinaryUri.UserInfo.Split(':');
            Account = new Account(cloudinaryUri.Host, creds[0], creds[1]);

            m_usePrivateCdn = !String.IsNullOrEmpty(cloudinaryUri.AbsolutePath) &&
                cloudinaryUri.AbsolutePath != "/";

            m_privateCdn = String.Empty;

            if (m_usePrivateCdn)
            {
                m_privateCdn = cloudinaryUri.AbsolutePath;
            }

            m_hasher = SHA1.Create();
        }

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="account">Cloudinary account</param>
        /// <param name="usePrivateCdn">Whether to use private Content Delivery Network</param>
        /// <param name="privateCdn">Private Content Delivery Network</param>
        /// <param name="shortenUrl">Whether to use shorten url when possible.</param>
        public Api(Account account, bool usePrivateCdn, string privateCdn, bool shortenUrl)
            : this(account)
        {
            m_usePrivateCdn = usePrivateCdn;
            m_privateCdn = privateCdn;
            m_shortenUrl = shortenUrl;
        }

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="account">Cloudinary account</param>
        public Api(Account account)
        {
            if (account == null)
                throw new ArgumentException("Account can't be null!");

            if (String.IsNullOrEmpty(account.Cloud))
                throw new ArgumentException("Cloud name must be specified in Account!");

            m_usePrivateCdn = false;
            m_hasher = SHA1.Create();
            Account = account;
        }

        /// <summary>
        /// Calculates signature of parameters
        /// </summary>
        /// <param name="parameters">Parameters to sign</param>
        /// <returns>Signature of parameters</returns>
        public string GetSign(SortedDictionary<string, object> parameters)
        {
            StringBuilder signBase = new StringBuilder(String.Join("&", parameters
                .Where(pair => pair.Value != null)
                .Select(pair => String.Format("{0}={1}", pair.Key,
                    pair.Value is IEnumerable<string>
                    ? String.Join(",", ((IEnumerable<string>)pair.Value).ToArray())
                    : pair.Value.ToString()))
                .ToArray()));

            signBase.Append(Account.ApiSecret);

            byte[] hash = m_hasher.ComputeHash(Encoding.UTF8.GetBytes(signBase.ToString()));

            StringBuilder sign = new StringBuilder();
            foreach (byte b in hash) sign.Append(b.ToString("x2"));

            return sign.ToString();
        }

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
        /// Default URL for working with resources
        /// </summary>
        public Url Url
        {
            get
            {
                return new Url(Account.Cloud)
                    .Shorten(m_shortenUrl)
                    .PrivateCdn(m_usePrivateCdn)
                    .Secure(m_usePrivateCdn)
                    .SecureDistribution(m_privateCdn);
            }
        }

        /// <summary>
        /// Default URL for working with uploaded images
        /// </summary>
        public Url UrlImgUp
        {
            get
            {
                return Url.
                    ResourceType("image").
                    Action("upload");
            }
        }

        /// <summary>
        /// Default cloudinary API URL
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
        /// Default cloudinary API URL for uploading images
        /// </summary>
        public Url ApiUrlImgUp
        {
            get
            {
                return ApiUrl.
                    Action("upload").
                    ResourceType("image");
            }
        }

        /// <summary>
        /// Default cloudinary API URL with version
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
        /// Default cloudinary API URL for uploading images with version
        /// </summary>
        public Url ApiUrlImgUpV
        {
            get
            {
                return ApiUrlV.
                    Action("upload").
                    ResourceType("image");
            }
        }

        /// <summary>
        /// Gets cloudinary parameter from enumeration
        /// </summary>
        /// <typeparam name="T">Enum which fields are decorated with DescriptionAttribute</typeparam>
        /// <param name="e">Field of enum</param>
        /// <returns>Cloudinary-compatible parameter</returns>
        public static string GetCloudinaryParam<T>(T e)
        {
            Type eType = typeof(T);
            FieldInfo fi = eType.GetField(e.ToString());
            DescriptionAttribute[] attrs = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attrs.Length == 0)
                throw new ArgumentException("Enum fields must be decorated with DescriptionAttribute!");

            return attrs[0].Description;
        }

        /// <summary>
        /// Parses cloudinary parameters and creates enumeration value
        /// </summary>
        /// <typeparam name="T">Enum which fields are decorated with DescriptionAttribute</typeparam>
        /// <param name="s">String to parse</param>
        /// <returns>API-compatible parameter</returns>
        public static T ParseCloudinaryParam<T>(string s)
        {
            Type eType = typeof(T);
            foreach (var fi in eType.GetFields())
            {
                DescriptionAttribute[] attrs = (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute), false);

                if (attrs.Length == 0)
                    continue;

                if (s == attrs[0].Description)
                    return (T)fi.GetValue(null);
            }

            return default(T);
        }

        /// <summary>
        /// Custom call to cloudinary API
        /// </summary>
        /// <param name="method">HTTP method of call</param>
        /// <param name="url">URL to call</param>
        /// <param name="parameters">Dictionary of call parameters (can be null)</param>
        /// <param name="file">File to upload (must be null for non-uploading actions)</param>
        /// <returns>HTTP response on call</returns>
        public HttpWebResponse Call(HttpMethod method, string url, SortedDictionary<string, object> parameters, FileDescription file)
        {
#if DEBUG
            Console.WriteLine(String.Format("{0} REQUEST:", method));
            Console.WriteLine(url);
#endif

            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = Enum.GetName(typeof(HttpMethod), method);

            if (method == HttpMethod.POST && parameters != null)
            {
                request.ContentType = "multipart/form-data; boundary=" + HTTP_BOUNDARY;
                FinalizeUploadParameters(parameters);

                using (Stream requestStream = request.GetRequestStream())
                {
                    using (StreamWriter writer = new StreamWriter(requestStream) { AutoFlush = true })
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
            else
            {
                byte[] authBytes = Encoding.ASCII.GetBytes(String.Format("{0}:{1}", Account.ApiKey, Account.ApiSecret));
                request.Headers.Add("Authorization", String.Format("Basic {0}", Convert.ToBase64String(authBytes)));
            }

            try
            {
                return request.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                return ex.Response as HttpWebResponse;
            }
        }

        /// <summary>
        /// Builds HTML form
        /// </summary>
        /// <returns>HTML form</returns>
        public string BuildUploadForm(string field, string resourceType)
        {
            return BuildUploadForm(field, resourceType, null, null);
        }

        /// <summary>
        /// Builds HTML form
        /// </summary>
        /// <returns>HTML form</returns>
        public string BuildUploadForm(string field, string resourceType, SortedDictionary<string, object> parameters, Dictionary<string, string> htmlOptions)
        {
            if (parameters == null)
                parameters = new SortedDictionary<string, object>();

            if (htmlOptions == null)
                htmlOptions = new Dictionary<string, string>();

            string url = ApiUrlImgUpV.ResourceType(resourceType).BuildUrl();

            FinalizeUploadParameters(parameters);

            StringBuilder builder = new StringBuilder();

            builder.
                Append("<input type='file' name='file' data-url='").
                Append(url).
                Append("' data-form-data='").
                Append(JsonConvert.SerializeObject(parameters)).
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
                    Append(HttpUtility.HtmlEncode(item.Value));
            }

            builder.Append("'/>");

            return builder.ToString();
        }

        public string PrivateDownload(string publicId, bool? attachment = null, string format = "", string type = "")
        {
            if (String.IsNullOrEmpty(publicId))
                throw new ArgumentException("publicId");

            UrlBuilder urlBuilder = new UrlBuilder(
               ApiUrlV
               .ResourceType("image")
               .Action("download")
               .BuildUrl());

            var parameters = new SortedDictionary<string, object>();

            parameters.Add("public_id", publicId);

            if (!String.IsNullOrEmpty(format))
                parameters.Add("format", format);

            if (attachment != null)
                parameters.Add("attachment", (bool)attachment ? "true" : "false");

            if (!String.IsNullOrEmpty(type))
                parameters.Add("type", type);

            FinalizeUploadParameters(parameters);

            foreach (var param in parameters)
            {
                urlBuilder.QueryString[param.Key] = param.Value.ToString();
            }

            return urlBuilder.ToString();
        }

        /// <summary>
        /// Calculates current UNIX time
        /// </summary>
        /// <returns>Amount of seconds from 1 january 1970</returns>
        private string GetTime()
        {
            return Convert.ToInt64(((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds)).ToString();
        }

        private void FinalizeUploadParameters(SortedDictionary<string, object> parameters)
        {
            parameters.Add("timestamp", GetTime());
            parameters.Add("signature", GetSign(parameters));
            parameters.Add("api_key", Account.ApiKey);
        }

        private void WriteParam(StreamWriter writer, string key, string value)
        {
#if DEBUG
            Console.WriteLine(String.Format("{0}: {1}", key, value));
#endif
            writer.WriteLine("--{0}", HTTP_BOUNDARY);
            writer.WriteLine("Content-Disposition: form-data; name=\"{1}\"{0}{0}{2}",
                Environment.NewLine,
                key,
                value);
        }

        private void WriteFile(StreamWriter writer, FileDescription file)
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
                        WriteFile(writer, stream, file.FileName);
                    }
                }
                else
                {
                    WriteFile(writer, file.Stream, file.FileName);
                }
            }
        }

        private void WriteFile(StreamWriter writer, Stream stream, string fileName)
        {
            writer.WriteLine("--{0}", HTTP_BOUNDARY);
            writer.WriteLine("Content-Disposition: form-data;  name=\"file\"; filename=\"{0}\"", fileName);
            writer.WriteLine("Content-Type: application/octet-stream");
            writer.WriteLine();

            byte[] buf = new byte[4096];
            int cnt, pos = 0;

            while ((cnt = stream.Read(buf, pos, buf.Length)) > 0)
            {
                writer.BaseStream.Write(buf, 0, cnt);
            }
        }
    }

    /// <summary>
    /// HTTP method
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
