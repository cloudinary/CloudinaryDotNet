namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Main class of Cloudinary .NET API.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    public partial class Cloudinary
    {
        /// <summary>
        /// Resource type 'image'.
        /// </summary>
        protected const string RESOURCE_TYPE_IMAGE = "image";

        /// <summary>
        /// Instance of <see cref="Random"/> class.
        /// </summary>
        protected static Random m_random = new Random();

        /// <summary>
        /// Cloudinary <see cref="Api"/> object.
        /// </summary>
        protected Api m_api;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cloudinary"/> class.
        /// Default parameterless constructor. Assumes that environment variable CLOUDINARY_URL is set.
        /// </summary>
        public Cloudinary()
        {
            m_api = new Api();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cloudinary"/> class with Cloudinary URL.
        /// </summary>
        /// <param name="cloudinaryUrl">Cloudinary URL.</param>
        public Cloudinary(string cloudinaryUrl)
        {
            m_api = new Api(cloudinaryUrl);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cloudinary"/> class with Cloudinary account.
        /// </summary>
        /// <param name="account">Cloudinary account.</param>
        public Cloudinary(Account account)
        {
            m_api = new Api(account);
        }

        /// <summary>
        /// Gets API object that used by this instance.
        /// </summary>
        public Api Api
        {
            get { return m_api; }
        }

        /// <summary>
        /// Gets the advanced search provider used by the Cloudinary instance.
        /// </summary>
        /// <returns>Instance of the <see cref="Search"/> class.</returns>
        public Search Search()
        {
            return new Search(m_api);
        }

        /// <summary>
        /// Gets URL to download private image.
        /// </summary>
        /// <param name="publicId">The image public ID.</param>
        /// <param name="attachment">Whether to download image as attachment (optional).</param>
        /// <param name="format">Format to download (optional).</param>
        /// <param name="type">The type (optional).</param>
        /// <param name="expiresAt">The date (UNIX time in seconds) for the URL expiration. (optional).</param>
        /// <param name="resourceType">Resource type (image, video or raw) of files to include in the archive (optional).</param>
        /// <returns>Download URL.</returns>
        /// <exception cref="System.ArgumentException">publicId can't be null.</exception>
        public string DownloadPrivate(
            string publicId,
            bool? attachment = null,
            string format = "",
            string type = "",
            long? expiresAt = null,
            string resourceType = RESOURCE_TYPE_IMAGE)
        {
            if (string.IsNullOrEmpty(publicId))
            {
                throw new ArgumentException("The image public ID is missing.");
            }

            var urlBuilder = new UrlBuilder(
               GetApiUrlV()
               .ResourceType(resourceType)
               .Action("download")
               .BuildUrl());

            var parameters = new SortedDictionary<string, object>
            {
                { "public_id", publicId },
            };

            if (!string.IsNullOrEmpty(format))
            {
                parameters.Add("format", format);
            }

            if (attachment != null)
            {
                parameters.Add("attachment", (bool)attachment ? "true" : "false");
            }

            if (!string.IsNullOrEmpty(type))
            {
                parameters.Add("type", type);
            }

            if (expiresAt != null)
            {
                parameters.Add("expires_at", expiresAt);
            }

            return GetDownloadUrl(urlBuilder, parameters);
        }

        /// <summary>
        /// Gets URL to download tag cloud as ZIP package.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="transform">The transformation.</param>
        /// <returns>Download URL.</returns>
        /// <exception cref="System.ArgumentException">Tag should be specified.</exception>
        /// <param name="resourceType">Resource type (image, video or raw) of files to include in the archive (optional).</param>
        public string DownloadZip(string tag, Transformation transform, string resourceType = RESOURCE_TYPE_IMAGE)
        {
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentException("Tag should be specified!");
            }

            var urlBuilder = new UrlBuilder(
               GetApiUrlV()
               .ResourceType(resourceType)
               .Action("download_tag.zip")
               .BuildUrl());

            var parameters = new SortedDictionary<string, object>
            {
                { "tag", tag },
            };

            if (transform != null)
            {
                parameters.Add("transformation", transform.Generate());
            }

            return GetDownloadUrl(urlBuilder, parameters);
        }

        /// <summary>
        /// Publishes resources by prefix asynchronously.
        /// </summary>
        /// <param name="prefix">The prefix for publishing resources.</param>
        /// <param name="parameters">Parameters for publishing of resources.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Parsed result of publishing.</returns>
        public Task<PublishResourceResult> PublishResourceByPrefixAsync(
            string prefix,
            PublishResourceParams parameters,
            CancellationToken? cancellationToken)
        {
            return PublishResourceAsync("prefix", prefix, parameters, cancellationToken);
        }

        /// <summary>
        /// Publishes resources by prefix.
        /// </summary>
        /// <param name="prefix">The prefix for publishing resources.</param>
        /// <param name="parameters">Parameters for publishing of resources.</param>
        /// <returns>Parsed result of publishing.</returns>
        public PublishResourceResult PublishResourceByPrefix(string prefix, PublishResourceParams parameters)
        {
            return PublishResource("prefix", prefix, parameters);
        }

        /// <summary>
        /// Publishes resources by tag asynchronously.
        /// </summary>
        /// <param name="tag">All resources with the given tag will be published.</param>
        /// <param name="parameters">Parameters for publishing of resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of publishing.</returns>
        public Task<PublishResourceResult> PublishResourceByTagAsync(
            string tag,
            PublishResourceParams parameters,
            CancellationToken? cancellationToken = null)
        {
            return PublishResourceAsync("tag", tag, parameters, cancellationToken);
        }

        /// <summary>
        /// Publishes resources by tag.
        /// </summary>
        /// <param name="tag">All resources with the given tag will be published.</param>
        /// <param name="parameters">Parameters for publishing of resources.</param>
        /// <returns>Parsed result of publishing.</returns>
        public PublishResourceResult PublishResourceByTag(string tag, PublishResourceParams parameters)
        {
            return PublishResource("tag", tag, parameters);
        }

        /// <summary>
        /// Publishes resource by Id asynchronously.
        /// </summary>
        /// <param name="tag">Not used.</param>
        /// <param name="parameters">Parameters for publishing of resources.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Structure with the results of publishing.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801: Review unused parameters", Justification = "Reviewed.")]
        public Task<PublishResourceResult> PublishResourceByIdsAsync(
            string tag,
            PublishResourceParams parameters,
            CancellationToken? cancellationToken)
        {
            return PublishResourceAsync(string.Empty, string.Empty, parameters, cancellationToken);
        }

        /// <summary>
        /// Publishes resource by Id.
        /// </summary>
        /// <param name="tag">Not used.</param>
        /// <param name="parameters">Parameters for publishing of resources.</param>
        /// <returns>Structure with the results of publishing.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801: Review unused parameters", Justification = "Reviewed.")]
        public PublishResourceResult PublishResourceByIds(string tag, PublishResourceParams parameters)
        {
            return PublishResource(string.Empty, string.Empty, parameters);
        }

        /// <summary>
        /// Updates access mode for the resources selected by tag asynchronously.
        /// </summary>
        /// <param name="tag">Update all resources with the given tag (up to a maximum
        /// of 100 matching original resources).</param>
        /// <param name="parameters">Parameters for updating of resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Structure with the results of update.</returns>
        public Task<UpdateResourceAccessModeResult> UpdateResourceAccessModeByTagAsync(
            string tag,
            UpdateResourceAccessModeParams parameters,
            CancellationToken? cancellationToken = null)
        {
            return UpdateResourceAccessModeAsync(Constants.TAG_PARAM_NAME, tag, parameters, cancellationToken);
        }

        /// <summary>
        /// Updates access mode for the resources selected by tag.
        /// </summary>
        /// <param name="tag">Update all resources with the given tag (up to a maximum
        /// of 100 matching original resources).</param>
        /// <param name="parameters">Parameters for updating of resources.</param>
        /// <returns>Structure with the results of update.</returns>
        public UpdateResourceAccessModeResult UpdateResourceAccessModeByTag(string tag, UpdateResourceAccessModeParams parameters)
        {
            return UpdateResourceAccessMode(Constants.TAG_PARAM_NAME, tag, parameters);
        }

        /// <summary>
        /// Updates access mode for the resources selected by prefix asynchronously.
        /// </summary>
        /// <param name="prefix">Update all resources where the public ID starts with the given prefix (up to a maximum
        /// of 100 matching original resources).</param>
        /// <param name="parameters">Parameters for updating of resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Structure with the results of update.</returns>
        public Task<UpdateResourceAccessModeResult> UpdateResourceAccessModeByPrefixAsync(
            string prefix,
            UpdateResourceAccessModeParams parameters,
            CancellationToken? cancellationToken = null)
        {
            return UpdateResourceAccessModeAsync(Constants.PREFIX_PARAM_NAME, prefix, parameters, cancellationToken);
        }

        /// <summary>
        /// Updates access mode for the resources selected by prefix.
        /// </summary>
        /// <param name="prefix">Update all resources where the public ID starts with the given prefix (up to a maximum
        /// of 100 matching original resources).</param>
        /// <param name="parameters">Parameters for updating of resources.</param>
        /// <returns>Structure with the results of update.</returns>
        public UpdateResourceAccessModeResult UpdateResourceAccessModeByPrefix(
            string prefix,
            UpdateResourceAccessModeParams parameters)
        {
            return UpdateResourceAccessMode(Constants.PREFIX_PARAM_NAME, prefix, parameters);
        }

        /// <summary>
        /// Updates access mode for the resources selected by public ids asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters for updating of resources. Update all resources with the given
        /// public IDs (array of up to 100 public_ids).</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Structure with the results of update.</returns>
        public Task<UpdateResourceAccessModeResult> UpdateResourceAccessModeByIdsAsync(
            UpdateResourceAccessModeParams parameters, CancellationToken? cancellationToken = null)
        {
            return UpdateResourceAccessModeAsync(string.Empty, string.Empty, parameters, cancellationToken);
        }

        /// <summary>
        /// Updates access mode for the resources selected by public ids.
        /// </summary>
        /// <param name="parameters">Parameters for updating of resources. Update all resources with the given
        /// public IDs (array of up to 100 public_ids).</param>
        /// <returns>Structure with the results of update.</returns>
        public UpdateResourceAccessModeResult UpdateResourceAccessModeByIds(UpdateResourceAccessModeParams parameters)
        {
            return UpdateResourceAccessMode(string.Empty, string.Empty, parameters);
        }

        /// <summary>
        /// Deletes derived resources by the given transformation (should be specified in parameters) asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to delete derived resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of deletion derived resources.</returns>
        public Task<DelDerivedResResult> DeleteDerivedResourcesByTransformAsync(DelDerivedResParams parameters, CancellationToken? cancellationToken = null)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                GetApiUrlV().
                Add("derived_resources").
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApiAsync<DelDerivedResResult>(
                HttpMethod.DELETE,
                urlBuilder.ToString(),
                parameters,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Deletes derived resources by the given transformation (should be specified in parameters).
        /// </summary>
        /// <param name="parameters">Parameters to delete derived resources.</param>
        /// <returns>Parsed result of deletion derived resources.</returns>
        public DelDerivedResResult DeleteDerivedResourcesByTransform(DelDerivedResParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                GetApiUrlV().
                Add("derived_resources").
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<DelDerivedResResult>(HttpMethod.DELETE, urlBuilder.ToString(), parameters, null);
        }

        /// <summary>
        /// Gets java script that configures Cloudinary JS.
        /// </summary>
        /// <param name="directUpload">Whether to reference additional scripts that are necessary for uploading files directly from browser.</param>
        /// <param name="dir">Override location of js files (default: ~/Scripts).</param>
        /// <returns>HTML script tag with Cloudinary JS configuration.</returns>
        public string GetCloudinaryJsConfig(bool directUpload = false, string dir = "")
        {
            if (string.IsNullOrEmpty(dir))
            {
                dir = "/Scripts";
            }

            StringBuilder sb = new StringBuilder(1000);

            AppendScriptLine(sb, dir, "jquery.ui.widget.js");
            AppendScriptLine(sb, dir, "jquery.iframe-transport.js");
            AppendScriptLine(sb, dir, "jquery.fileupload.js");
            AppendScriptLine(sb, dir, "jquery.cloudinary.js");

            if (directUpload)
            {
                AppendScriptLine(sb, dir, "canvas-to-blob.min.js");
                AppendScriptLine(sb, dir, "jquery.fileupload-image.js");
                AppendScriptLine(sb, dir, "jquery.fileupload-process.js");
                AppendScriptLine(sb, dir, "jquery.fileupload-validate.js");
                AppendScriptLine(sb, dir, "load-image.min.js");
            }

            var cloudinaryParams = new JObject(
                new JProperty[]
                {
                    new JProperty("cloud_name", m_api.Account.Cloud),
                    new JProperty("api_key", m_api.Account.ApiKey),
                    new JProperty("private_cdn", m_api.UsePrivateCdn),
                    new JProperty("cdn_subdomain", m_api.CSubDomain),
                });

            if (!string.IsNullOrEmpty(m_api.PrivateCdn))
            {
                cloudinaryParams.Add("secure_distribution", m_api.PrivateCdn);
            }

            sb.AppendLine("<script type='text/javascript'>");
            sb.Append("$.cloudinary.config(");
            sb.Append(cloudinaryParams.ToString());
            sb.AppendLine(");");
            sb.AppendLine("</script>");

            return sb.ToString();
        }

        private static void AppendScriptLine(StringBuilder sb, string dir, string script)
        {
            sb.Append("<script src=\"");
            sb.Append(dir);

            if (!dir.EndsWith("/", StringComparison.Ordinal) && !dir.EndsWith("\\", StringComparison.Ordinal))
            {
                sb.Append('/');
            }

            sb.Append(script);

            sb.AppendLine("\"></script>");
        }

        /// <summary>
        /// Get default API URL with version.
        /// </summary>
        /// <returns>URL of the API.</returns>
        private Url GetApiUrlV()
        {
            return m_api.ApiUrlV;
        }

        private string GetUploadMappingUrl()
        {
            return GetApiUrlV().
                ResourceType("upload_mappings").
                BuildUrl();
        }

        private string GetUploadMappingUrl(UploadMappingParams parameters)
        {
            var uri = GetUploadMappingUrl();
            return (parameters == null) ? uri : new UrlBuilder(uri, parameters.ToParamsDictionary()).ToString();
        }

        /// <summary>
        /// Call api with specified parameters.
        /// </summary>
        /// <param name="apiParams">New parameters for upload preset.</param>
        private T CallApi<T>(UploadPresetApiParams apiParams)
            where T : BaseResult, new() =>
            m_api.CallApi<T>(apiParams.HttpMethod, apiParams.Url, apiParams.ParamsCopy, null);

        /// <summary>
        /// Call api with specified parameters asynchronously.
        /// </summary>
        /// <param name="apiParams">New parameters for upload preset.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        private Task<T> CallApiAsync<T>(UploadPresetApiParams apiParams, CancellationToken? cancellationToken = null)
            where T : BaseResult, new() =>
            m_api.CallApiAsync<T>(apiParams.HttpMethod, apiParams.Url, apiParams.ParamsCopy, null, null, cancellationToken);

        private Task<PublishResourceResult> PublishResourceAsync(
            string byKey,
            string value,
            PublishResourceParams parameters,
            CancellationToken? cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(byKey) && !string.IsNullOrWhiteSpace(value))
            {
                parameters.AddCustomParam(byKey, value);
            }

            Url url = GetApiUrlV()
                .Add("resources")
                .Add(parameters.ResourceType.ToString().ToLowerInvariant())
                .Add("publish_resources");

            return m_api.CallApiAsync<PublishResourceResult>(HttpMethod.POST, url.BuildUrl(), parameters, null, null, cancellationToken);
        }

        private PublishResourceResult PublishResource(string byKey, string value, PublishResourceParams parameters)
        {
            if (!string.IsNullOrWhiteSpace(byKey) && !string.IsNullOrWhiteSpace(value))
            {
                parameters.AddCustomParam(byKey, value);
            }

            Url url = GetApiUrlV()
                .Add("resources")
                .Add(parameters.ResourceType.ToString().ToLowerInvariant())
                .Add("publish_resources");

            return m_api.CallApi<PublishResourceResult>(HttpMethod.POST, url.BuildUrl(), parameters, null);
        }

        private Task<UpdateResourceAccessModeResult> UpdateResourceAccessModeAsync(
            string byKey,
            string value,
            UpdateResourceAccessModeParams parameters,
            CancellationToken? cancellationToken = null)
        {
            if (!string.IsNullOrWhiteSpace(byKey) && !string.IsNullOrWhiteSpace(value))
            {
                parameters.AddCustomParam(byKey, value);
            }

            var url = GetApiUrlV()
                 .Add(Constants.RESOURCES_API_URL)
                 .Add(parameters.ResourceType.ToString().ToLowerInvariant())
                 .Add(parameters.Type)
                 .Add(Constants.UPDATE_ACESS_MODE);

            return m_api.CallApiAsync<UpdateResourceAccessModeResult>(
                HttpMethod.POST,
                url.BuildUrl(),
                parameters,
                null,
                null,
                cancellationToken);
        }

        private UpdateResourceAccessModeResult UpdateResourceAccessMode(string byKey, string value, UpdateResourceAccessModeParams parameters)
        {
            if (!string.IsNullOrWhiteSpace(byKey) && !string.IsNullOrWhiteSpace(value))
            {
                parameters.AddCustomParam(byKey, value);
            }

            Url url = GetApiUrlV()
                 .Add(Constants.RESOURCES_API_URL)
                 .Add(parameters.ResourceType.ToString().ToLowerInvariant())
                 .Add(parameters.Type)
                 .Add(Constants.UPDATE_ACESS_MODE);

            return m_api.CallApi<UpdateResourceAccessModeResult>(HttpMethod.POST, url.BuildUrl(), parameters, null);
        }

        /// <summary>
        /// Calls an upload mappings API asynchronously.
        /// </summary>
        /// <param name="httpMethod">HTTP method.</param>
        /// <param name="parameters">Parameters for Mapping of folders to URL prefixes for dynamic image fetching from
        /// existing online locations.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        private Task<UploadMappingResults> CallUploadMappingsApiAsync(HttpMethod httpMethod, UploadMappingParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = (httpMethod == HttpMethod.POST || httpMethod == HttpMethod.PUT)
                ? GetUploadMappingUrl()
                : GetUploadMappingUrl(parameters);

            return m_api.CallApiAsync<UploadMappingResults>(
                httpMethod,
                url,
                parameters,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Calls an upload mappings API.
        /// </summary>
        /// <param name="httpMethod">HTTP method.</param>
        /// <param name="parameters">Parameters for Mapping of folders to URL prefixes for dynamic image fetching from
        /// existing online locations.</param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        private UploadMappingResults CallUploadMappingsAPI(HttpMethod httpMethod, UploadMappingParams parameters)
        {
            string url = (httpMethod == HttpMethod.POST || httpMethod == HttpMethod.PUT)
                ? GetUploadMappingUrl()
                : GetUploadMappingUrl(parameters);

            return m_api.CallApi<UploadMappingResults>(httpMethod, url, parameters, null);
        }

        private string GetTransformationUrl(HttpMethod httpMethod, BaseParams parameters)
        {
            var url = GetApiUrlV().
                        ResourceType("transformations").
                        BuildUrl();

            if (parameters != null && (httpMethod == HttpMethod.GET || httpMethod == HttpMethod.DELETE))
            {
                url = new UrlBuilder(url, parameters.ToParamsDictionary()).ToString();
            }

            return url;
        }

        private string GetDownloadUrl(UrlBuilder builder, IDictionary<string, object> parameters)
        {
            m_api.FinalizeUploadParameters(parameters);
            builder.SetParameters(parameters);
            return builder.ToString();
        }

        /// <summary>
        /// Private helper class for specifying parameters for upload preset api call.
        /// </summary>
        private class UploadPresetApiParams
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="UploadPresetApiParams"/> class.
            /// </summary>
            /// <param name="httpMethod">Http request method.</param>
            /// <param name="url">Url for api call.</param>
            /// <param name="paramsCopy">Parameters of the upload preset.</param>
            public UploadPresetApiParams(
                HttpMethod httpMethod,
                string url,
                UploadPresetParams paramsCopy)
            {
                Url = url;
                ParamsCopy = paramsCopy;
                HttpMethod = httpMethod;
            }

            /// <summary>
            /// Gets url for api call.
            /// </summary>
            public string Url { get; private set; }

            /// <summary>
            /// Gets parameters of the upload preset.
            /// </summary>
            public UploadPresetParams ParamsCopy { get; private set; }

            /// <summary>
            /// Gets http request method.
            /// </summary>
            public HttpMethod HttpMethod { get; private set; }
        }
    }
}
