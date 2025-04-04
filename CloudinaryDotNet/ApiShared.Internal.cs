﻿namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;
    using CloudinaryDotNet.Core;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Provider for the API calls.
    /// </summary>
    [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Reviewed.")]
    public partial class ApiShared : ISignProvider
    {
        /// <summary>
        /// Parses HTTP response and creates new instance of this class asynchronously.
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <returns>New instance of this class.</returns>
        /// <typeparam name="T">Type of the parsed response.</typeparam>
        internal static async Task<T> ParseAsync<T>(HttpResponseMessage response)
            where T : BaseResult
        {
            using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return CreateResult<T>(response, stream);
        }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class.
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <returns>New instance of this class.</returns>
        /// <typeparam name="T">Type of the parsed response.</typeparam>
        internal static T Parse<T>(HttpResponseMessage response)
            where T : BaseResult
        {
            using var stream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
            return CreateResult<T>(response, stream);
        }

        /// <summary>
        /// Serialize the cloudinary parameters to JSON.
        /// </summary>
        /// <param name="parameters">Parameters to serialize.</param>
        /// <returns>Serialized parameters as JSON string.</returns>
        internal static string ParamsToJson(SortedDictionary<string, object> parameters)
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

        /// <summary>
        /// Call api asynchronous and return response of specified type asynchronously.
        /// </summary>
        /// <param name="method">HTTP method.</param>
        /// <param name="url">Url for api call.</param>
        /// <param name="parameters">Parameters for api call.</param>
        /// <param name="extraHeaders">Extra headers.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Return response of specified type.</returns>
        /// <typeparam name="T">Type of the parsed response.</typeparam>
        internal virtual Task<T> CallApiAsync<T>(
            HttpMethod method,
            string url,
            BaseParams parameters,
            Dictionary<string, string> extraHeaders = null,
            CancellationToken? cancellationToken = null)
            where T : BaseResult, new()
        {
            var callParams = GetCallParams(parameters);

            return CallAndParseAsync<T>(method, url, callParams, extraHeaders, cancellationToken);
        }

        /// <summary>
        /// Call api synchronous and return response of specified type.
        /// </summary>
        /// <param name="method">HTTP method.</param>
        /// <param name="url">Url for api call.</param>
        /// <param name="parameters">Parameters for api call.</param>
        /// <param name="extraHeaders">Extra headers.</param>
        /// <returns>Return response of specified type.</returns>
        /// <typeparam name="T">Type of the parsed response.</typeparam>
        internal virtual T CallApi<T>(HttpMethod method, string url, BaseParams parameters, Dictionary<string, string> extraHeaders = null)
            where T : BaseResult, new()
        {
            var callParams = GetCallParams(parameters);

            return CallAndParse<T>(
                method,
                url,
                callParams,
                extraHeaders);
        }

        /// <summary>
        /// Prepares request body to be sent on custom asynchronous call to Cloudinary API.
        /// </summary>
        /// <param name="request">HTTP request to alter.</param>
        /// <param name="method">HTTP method of call.</param>
        /// <param name="parameters">Dictionary of call parameters.</param>
        /// <param name="extraHeaders">(Optional) Headers to add to the request.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Prepared HTTP request.</returns>
        internal async Task<HttpRequestMessage> PrepareRequestAsync(
            HttpRequestMessage request,
            HttpMethod method,
            SortedDictionary<string, object> parameters,
            Dictionary<string, string> extraHeaders = null,
            CancellationToken? cancellationToken = null)
        {
            SetHttpMethod(method, request);
            SetRequestHeaders(request, extraHeaders);

            if (!ShouldPrepareContent(method, parameters))
            {
                return request;
            }

            SetChunkedEncoding(request);

            await SetRequestContentAsync(request, parameters, extraHeaders, cancellationToken)
                .ConfigureAwait(false);

            return request;
        }

        /// <summary>
        /// Extends Cloudinary upload parameters with additional attributes.
        /// </summary>
        /// <param name="parameters">Cloudinary upload parameters.</param>
        internal void FinalizeUploadParameters(IDictionary<string, object> parameters)
        {
            if (!parameters.ContainsKey("timestamp"))
            {
                parameters.Add("timestamp", Utils.UnixTimeNowSeconds());
            }

            if (!parameters.ContainsKey("signature") && string.IsNullOrEmpty(Account.OAuthToken))
            {
                parameters.Add("signature", SignParameters(parameters));
            }

            parameters.Add("api_key", Account.ApiKey);
        }

        /// <summary>
        /// Virtual encode API URL method. This method should be overridden in child classes.
        /// </summary>
        /// <param name="value">URL to be encoded.</param>
        /// <returns>Encoded URL.</returns>
        protected static string EncodeApiUrl(string value)
        {
            return WebUtility.UrlEncode(value);
        }

        /// <summary>
        /// Build unsigned upload params with defined preset.
        /// </summary>
        /// <param name="preset">The name of an upload preset defined for your Cloudinary account.</param>
        /// <param name="parameters">Cloudinary upload parameters.</param>
        /// <returns>Unsigned cloudinary parameters with upload preset included.</returns>
        protected static SortedDictionary<string, object> BuildUnsignedUploadParams(string preset, SortedDictionary<string, object> parameters = null)
        {
            parameters ??= new SortedDictionary<string, object>();

            parameters.Add("upload_preset", preset);
            parameters.Add("unsigned", true);

            return parameters;
        }

        /// <summary>
        /// Gets authentication credentials.
        /// </summary>
        /// <returns>Credentials string for authentication.</returns>
        protected virtual string GetApiCredentials()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}:{1}", Account.ApiKey, Account.ApiSecret);
        }

        /// <summary>
        /// Check 'unsigned' parameter value and add signature into parameters if unsigned=false or not specified.
        /// </summary>
        /// <param name="parameters">Parameters to check signature.</param>
        protected void HandleUnsignedParameters(IDictionary<string, object> parameters)
        {
            if (!parameters.ContainsKey("unsigned") || parameters["unsigned"].ToString() == "false")
            {
                FinalizeUploadParameters(parameters);
            }
            else if (parameters.ContainsKey("removeUnsignedParam"))
            {
                parameters.Remove("unsigned");
                parameters.Remove("removeUnsignedParam");
            }
        }

        /// <summary>
        /// Prepares request Url to be sent on custom call to Cloudinary API.
        /// </summary>
        /// <param name="method">HTTP method of call.</param>
        /// <param name="url">The existing URL.</param>
        /// <param name="parameters">Dictionary of call parameters.</param>
        /// <returns>Prepared HTTP request.</returns>
        private static string PrepareRequestUrl(
            HttpMethod method,
            string url,
            IDictionary<string, object> parameters)
        {
            return ShouldPrepareContent(method, parameters) ? url : new UrlBuilder(url, parameters).ToString();
        }

        private static SortedDictionary<string, object> GetCallParams(BaseParams parameters)
        {
            parameters?.Check();
            return parameters?.ToParamsDictionary();
        }

        private static T CreateResult<T>(HttpResponseMessage response, Stream s)
            where T : BaseResult
        {
            var result = CreateResultFromStream<T>(s, response.StatusCode);
            UpdateResultFromResponse(response, result);
            return result;
        }

        private static T CreateResultFromStream<T>(Stream s, HttpStatusCode statusCode)
            where T : BaseResult
        {
            try
            {
                using var streamReader = new StreamReader(s);
                using var jsonReader = new JsonTextReader(streamReader);
                var jsonObj = JToken.Load(jsonReader);
                var serializer = new JsonSerializer();
                serializer.Converters.Add(new SafeBooleanConverter());
                var result = jsonObj.ToObject<T>(serializer);
                result.JsonObj = jsonObj;

                return result;
            }
            catch (JsonException jex)
            {
                throw new Exception($"Failed to deserialize response with status code: {statusCode}", jex);
            }
        }

        private static void UpdateResultFromResponse<T>(HttpResponseMessage response, T result)
            where T : BaseResult
        {
            if (response == null)
            {
                return;
            }

            response.Headers
                .Where(p => p.Key.StartsWith("X-FeatureRateLimit", StringComparison.OrdinalIgnoreCase))
                .ToList()
                .ForEach(header =>
                {
                    var value = header.Value.First();
                    var key = header.Key;
                    if (key.EndsWith("Limit", StringComparison.OrdinalIgnoreCase) && long.TryParse(value, out long l))
                    {
                        result.Limit = l;
                    }

                    if (key.EndsWith("Remaining", StringComparison.OrdinalIgnoreCase) && long.TryParse(value, out l))
                    {
                        result.Remaining = l;
                    }

                    if (key.EndsWith("Reset", StringComparison.OrdinalIgnoreCase) && DateTime.TryParse(value, out DateTime t))
                    {
                        result.Reset = t;
                    }
                });

            result.StatusCode = response.StatusCode;
        }

        private static bool ShouldPrepareContent(HttpMethod method, object parameters) =>
            method is HttpMethod.POST or HttpMethod.PUT or HttpMethod.DELETE && parameters != null;

        private static bool IsChunkedUpload(Dictionary<string, string> extraHeaders) =>
            extraHeaders != null && extraHeaders.ContainsKey("X-Unique-Upload-Id");

        private static void SetStreamContent(string fieldName, FileDescription file, Stream stream, MultipartFormDataContent content)
        {
            var streamContent = new StreamContent(stream);

            streamContent.Headers.Add("Content-Type", "application/octet-stream");
            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = fieldName,
                FileNameStar = file.FileName,
            };
            content.Add(streamContent, fieldName, file.FileName);
        }

        private static void SetContentForRemoteFile(string fieldName, FileDescription file, MultipartFormDataContent content)
        {
            var strContent = new StringContent(file.FilePath);
            strContent.Headers.Add("Content-Disposition", string.Format(CultureInfo.InvariantCulture, "form-data; name=\"{0}\"", fieldName));
            content.Add(strContent);
        }

        private static void SetChunkContent(ChunkData chunk, MultipartFormDataContent content)
        {
            content.Headers.TryAddWithoutValidation("Content-Range", $"bytes {chunk.StartByte}-{chunk.EndByte}/{chunk.TotalBytes}");
        }

        private static StringContent CreateJsonContent(SortedDictionary<string, object> parameters) =>
            new (ParamsToJson(parameters), Encoding.UTF8, Constants.CONTENT_TYPE_APPLICATION_JSON);

        private static bool IsJsonContent(IReadOnlyDictionary<string, string> extraHeaders) =>
            extraHeaders != null &&
            extraHeaders.TryGetValue(Constants.HEADER_CONTENT_TYPE, out var value) &&
            value == Constants.CONTENT_TYPE_APPLICATION_JSON;

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

        private static async Task<HttpContent> CreateMultipartContentAsync(
            SortedDictionary<string, object> parameters,
            Dictionary<string, string> extraHeaders = null,
            CancellationToken? cancellationToken = null)
        {
            var content = new MultipartFormDataContent(HTTP_BOUNDARY);
            foreach (var param in parameters.Where(param => param.Value != null))
            {
                switch (param.Value)
                {
                    case FileDescription { IsRemote: true } file:
                        SetContentForRemoteFile(param.Key, file, content);
                        break;
                    case FileDescription file:
                    {
                        Stream stream;

                        if (IsChunkedUpload(extraHeaders))
                        {
                            var chunk = await file.GetNextChunkAsync(cancellationToken).ConfigureAwait(false);

                            SetChunkContent(chunk, content);

                            stream = chunk.Chunk;
                        }
                        else
                        {
                            stream = file.GetFileStream();
                        }

                        SetStreamContent(param.Key, file, stream, content);
                        break;
                    }

                    case IEnumerable<string> value:
                    {
                        foreach (var item in value)
                        {
                            content.Add(new StringContent(item), string.Format(CultureInfo.InvariantCulture, "\"{0}\"", string.Concat(param.Key, "[]")));
                        }

                        break;
                    }

                    default:
                        content.Add(new StringContent(param.Value.ToString()), string.Format(CultureInfo.InvariantCulture, "\"{0}\"", param.Key));
                        break;
                }
            }

            return content;
        }

        private CancellationToken GetDefaultCancellationToken() =>
            Timeout > 0
                ? new CancellationTokenSource(Timeout).Token
                : CancellationToken.None;

        private void SetChunkedEncoding(HttpRequestMessage request)
        {
            if (UseChunkedEncoding)
            {
                request.Headers.Add("Transfer-Encoding", "chunked");
            }
        }

        private AuthenticationHeaderValue GetAuthorizationHeaderValue()
        {
            return !string.IsNullOrEmpty(Account.OAuthToken)
                ? new AuthenticationHeaderValue("Bearer", Account.OAuthToken)
                : new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(GetApiCredentials())));
        }

        private void SetRequestHeaders(
            HttpRequestMessage request,
            Dictionary<string, string> headers)
        {
            // Add platform information to the USER_AGENT header
            // This is intended for platform information and not individual applications!
            var userPlatform = string.IsNullOrEmpty(UserPlatform)
                ? USER_AGENT
                : string.Format(CultureInfo.InvariantCulture, "{0} {1}", UserPlatform, USER_AGENT);
            request.Headers.Add("User-Agent", userPlatform);

            request.Headers.Authorization = GetAuthorizationHeaderValue();

            if (headers == null)
            {
                return;
            }

            foreach (var header in headers)
            {
                if (header.Key == "Accept")
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(headers["Accept"]));
                    continue;
                }

                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        private async Task SetRequestContentAsync(
            HttpRequestMessage request,
            SortedDictionary<string, object> parameters,
            Dictionary<string, string> extraHeaders = null,
            CancellationToken? cancellationToken = null)
        {
            HandleUnsignedParameters(parameters);

            var content = IsJsonContent(extraHeaders)
                ? CreateJsonContent(parameters)
                : await CreateMultipartContentAsync(parameters, extraHeaders, cancellationToken).ConfigureAwait(false);

            request.Content = content;
        }
    }
}
