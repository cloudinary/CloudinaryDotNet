
using CloudinaryDotNet.Actions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json.Converters;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Provider for the API calls.
    /// </summary>
    public class ApiShared : ISignProvider
    {
        /// <summary>
        /// Url of the cloudinary API.
        /// </summary>
        public const string ADDR_API = "api.cloudinary.com";

        /// <summary>
        /// Url of the cloudinary shared CDN.
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
        /// Whether to use sub domain.
        /// </summary>
        public bool CSubDomain;

        /// <summary>
        /// Whether to use shorten url when possible.
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
        /// Whether to use secure Url.
        /// </summary>
        public bool Secure;

        /// <summary>
        /// The private CDN prefix for the Url.
        /// </summary>
        public string PrivateCdn;

        /// <summary>
        /// The descriptive suffix to add to the Public ID in the delivery Url.
        /// </summary>
        public string Suffix;

        /// <summary>
        /// User platform information.
        /// </summary>
        public string UserPlatform;

        /// <summary>
        /// Timeout for the API requests,
        /// </summary>
        public int Timeout = 0;

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

        /// <summary>
        /// Virtual method to call the cloudinary API. This method should be overriden in child classes.
        /// </summary>
        /// <param name="method">Http request method.</param>
        /// <param name="url">API Url.</param>
        /// <param name="parameters">Cloudinary parameters to add to the API call.</param>
        /// <param name="file">(Optional) Add file to the body of the API call.</param>
        /// <param name="extraHeaders">(Optional) Add file to the body of the API call.</param>
        /// <returns>Parsed response from the cloudinary API.</returns>
        public virtual object InternalCall(HttpMethod method, string url, SortedDictionary<string, object> parameters, FileDescription file, Dictionary<string, string> extraHeaders = null)
        {
            throw new Exception("Please call overriden method");
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
        /// Virtual method to call the cloudinary API and return the parsed response. This method should be overriden
        /// in child classes.
        /// </summary>
        /// <typeparam name="T">Type of the response.</typeparam>
        /// <param name="method">Http request method.</param>
        /// <param name="url">API Url.</param>
        /// <param name="parameters">Cloudinary parameters to add to the API call.</param>
        /// <param name="file">(Optional) Add file to the body of the API call.</param>
        /// <param name="extraHeaders">(Optional) Add file to the body of the API call.</param>
        /// <returns>Parsed response from the cloudinary API.</returns>
        public virtual T CallAndParse<T>(HttpMethod method, string url, SortedDictionary<string, object> parameters, FileDescription file, Dictionary<string, string> extraHeaders = null) where T : BaseResult, new()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        /// <summary>
        /// Parametrized constructor.
        /// </summary>
        /// <param name="account">Cloudinary account.</param>
        /// <param name="usePrivateCdn">Whether to use private Content Delivery Network.</param>
        /// <param name="privateCdn">Private Content Delivery Network.</param>
        /// <param name="shortenUrl">Whether to use shorten url when possible.</param>
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
        /// Parametrized constructor.
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
        /// Calculates current UNIX time.
        /// </summary>
        /// <returns>Amount of seconds from 1 january 1970.</returns>
        private string GetTime()
        {
            return Convert.ToInt64(((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds)).ToString();
        }

        /// <summary>
        /// Virtual build callback url method. This method should be overriden in child classes.
        /// </summary>
        /// <returns>Callback url.</returns>
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
        /// Virtual encode API Url method. This method should be overriden in child classes.
        /// </summary>
        /// <param name="value">Url to be encoded.</param>
        /// <returns>Encoded Url.</returns>
        protected virtual string EncodeApiUrl(string value)
        {
            return string.Empty;
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
                int bytesSent = 0;
                if (file.Stream == null)
                {
                    using (FileStream stream = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        stream.Seek(file.BytesSent, SeekOrigin.Begin);
                        file.Eof = WriteFile(writer, stream, file.BufferLength, file.FileName, out bytesSent);
                        file.BytesSent += bytesSent;
                    }
                }
                else
                {
                    file.Eof = WriteFile(writer, file.Stream, file.BufferLength, file.FileName, out bytesSent);
                    file.BytesSent += bytesSent;
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
        /// <param name="bytesSent">Amount of sent bytes.</param>
        /// <returns>
        /// true for EOF.
        /// </returns>
        private bool WriteFile(StreamWriter writer, Stream stream, int length, string fileName, out int bytesSent)
        {
            WriteLine(writer, "--{0}", HTTP_BOUNDARY);
            WriteLine(writer, "Content-Disposition: form-data;  name=\"file\"; filename=\"{0}\"", fileName);
            WriteLine(writer, "Content-Type: application/octet-stream");
            WriteLine(writer);

            writer.Flush();

            bytesSent = 0;
            int toSend = 0;
            byte[] buf = new byte[ChunkSize];
            int cnt = 0;

            while ((toSend = length - bytesSent) > 0
                && (cnt = stream.Read(buf, 0, (toSend > buf.Length ? buf.Length : toSend))) > 0)
            {
                writer.BaseStream.Write(buf, 0, cnt);
                bytesSent += cnt;
            }

            return cnt == 0;
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

