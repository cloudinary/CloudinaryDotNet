namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Net;
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

        /// <summary>
        /// Resource type 'image'.
        /// </summary>
        protected const string RESOURCE_TYPE_IMAGE = "image";

        /// <summary>
        /// Action 'generate_archive'.
        /// </summary>
        protected const string ACTION_GENERATE_ARCHIVE = "generate_archive";

        /// <summary>
        /// Instance of <see cref="Random"/> class.
        /// </summary>
        protected static Random m_random = new Random();

        /// <summary>
        /// Default chunk (buffer) size for upload large files.
        /// </summary>
        protected const int DEFAULT_CHUNK_SIZE = 20 * 1024 * 1024; // 20 MB

        /// <summary>
        /// Cloudinary <see cref="Api"/> object.
        /// </summary>
        protected Api m_api;

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
        /// Get default API URL with version.
        /// </summary>
        /// <returns>URL of the API.</returns>
        private Url GetApiUrlV()
        {
            return m_api.ApiUrlV;
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
        ///  Returns URL on archive file.
        /// </summary>
        /// <param name="parameters">Parameters of generated archive.</param>
        /// <returns>URL on archive file.</returns>
        public string DownloadArchiveUrl(ArchiveParams parameters)
        {
            parameters.Mode(ArchiveCallMode.Download);

            UrlBuilder urlBuilder = new UrlBuilder(
                GetApiUrlV().
                ResourceType(parameters.ResourceType()).
                Action(ACTION_GENERATE_ARCHIVE).
                BuildUrl());

            return GetDownloadUrl(urlBuilder, parameters.ToParamsDictionary());
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
        public PublishResourceResult PublishResourceByIds(string tag, PublishResourceParams parameters)
        {
            return PublishResource(string.Empty, string.Empty, parameters);
        }

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
        /// Manage tag assignments asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of tag management.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Results of tags management.</returns>
        public Task<TagResult> TagAsync(TagParams parameters, CancellationToken? cancellationToken = null)
        {
            string uri = GetApiUrlV()
                .ResourceType(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType))
                .Action(Constants.TAGS_MANGMENT)
                .BuildUrl();

            return m_api.CallApiAsync<TagResult>(HttpMethod.POST, uri, parameters, null, null, cancellationToken);
        }

        /// <summary>
        /// Manages tag assignments.
        /// </summary>
        /// <param name="parameters">Parameters of tag management.</param>
        /// <returns>Results of tags management.</returns>
        public TagResult Tag(TagParams parameters)
        {
            string uri = GetApiUrlV()
                .ResourceType(ApiShared.GetCloudinaryParam(parameters.ResourceType))
                .Action(Constants.TAGS_MANGMENT)
                .BuildUrl();

            return m_api.CallApi<TagResult>(HttpMethod.POST, uri, parameters, null);
        }

        /// <summary>
        /// Manages context assignments asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of context management.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Results of contexts management.</returns>
        public Task<ContextResult> ContextAsync(ContextParams parameters, CancellationToken? cancellationToken = null)
        {
            string uri = m_api.ApiUrlImgUpV.Action(Constants.CONTEXT_MANAGMENT).BuildUrl();

            return m_api.CallApiAsync<ContextResult>(
                HttpMethod.POST,
                uri,
                parameters,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Manages context assignments.
        /// </summary>
        /// <param name="parameters">Parameters of context management.</param>
        /// <returns>Results of contexts management.</returns>
        public ContextResult Context(ContextParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.Action(Constants.CONTEXT_MANAGMENT).BuildUrl();

            return m_api.CallApi<ContextResult>(HttpMethod.POST, uri, parameters, null);
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
        /// Creates archive and stores it as a raw resource in your Cloudinary account asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of new generated archive.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of creating the archive.</returns>
        public Task<ArchiveResult> CreateArchiveAsync(ArchiveParams parameters, CancellationToken? cancellationToken = null)
        {
            Url url = GetApiUrlV().ResourceType(RESOURCE_TYPE_IMAGE).Action(ACTION_GENERATE_ARCHIVE);

            if (!string.IsNullOrEmpty(parameters.ResourceType()))
            {
                url.ResourceType(parameters.ResourceType());
            }

            parameters.Mode(ArchiveCallMode.Create);
            return m_api.CallApiAsync<ArchiveResult>(
                HttpMethod.POST,
                url.BuildUrl(),
                parameters,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Creates archive and stores it as a raw resource in your Cloudinary account.
        /// </summary>
        /// <param name="parameters">Parameters of new generated archive.</param>
        /// <returns>Parsed result of creating the archive.</returns>
        public ArchiveResult CreateArchive(ArchiveParams parameters)
        {
            var url = m_api.ApiUrlV.ResourceType(parameters.ResourceType()).Action(ACTION_GENERATE_ARCHIVE);

            parameters.Mode(ArchiveCallMode.Create);
            return m_api.CallApi<ArchiveResult>(HttpMethod.POST, url.BuildUrl(), parameters, null);
        }

        /// <summary>
        /// Creates a zip archive and stores it as a raw resource in your Cloudinary account asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of the new generated zip archive.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of creating the archive.</returns>
        public Task<ArchiveResult> CreateZipAsync(ArchiveParams parameters, CancellationToken? cancellationToken = null)
        {
            parameters.TargetFormat(ArchiveFormat.Zip);
            return CreateArchiveAsync(parameters, cancellationToken);
        }

        /// <summary>
        /// Creates a zip archive and stores it as a raw resource in your Cloudinary account.
        /// </summary>
        /// <param name="parameters">Parameters of the new generated zip archive.</param>
        /// <returns>Parsed result of creating the archive.</returns>
        public ArchiveResult CreateZip(ArchiveParams parameters)
        {
            parameters.TargetFormat(ArchiveFormat.Zip);
            return CreateArchive(parameters);
        }

        /// <summary>
        /// This method can be used to force refresh facebook and twitter profile pictures. The response of this method
        /// includes the image's version. Use this version to bypass previously cached CDN copies. Also it can be used
        /// to generate transformed versions of an uploaded image. This is useful when Strict Transformations are
        /// allowed for your account and you wish to create custom derived images for already uploaded images asynchronously.
        /// </summary>
        /// <param name="parameters">The parameters for explicit method.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after a call of Explicit method.</returns>
        public Task<ExplicitResult> ExplicitAsync(ExplicitParams parameters, CancellationToken? cancellationToken = null)
        {
            string uri = GetApiUrlV()
                .ResourceType(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType))
                .Action("explicit")
                .BuildUrl();

            return m_api.CallApiAsync<ExplicitResult>(
                HttpMethod.POST,
                uri,
                parameters,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// This method can be used to force refresh facebook and twitter profile pictures. The response of this method
        /// includes the image's version. Use this version to bypass previously cached CDN copies. Also it can be used
        /// to generate transformed versions of an uploaded image. This is useful when Strict Transformations are
        /// allowed for your account and you wish to create custom derived images for already uploaded images.
        /// </summary>
        /// <param name="parameters">The parameters for explicit method.</param>
        /// <returns>Parsed response after a call of Explicit method.</returns>
        public ExplicitResult Explicit(ExplicitParams parameters)
        {
            string uri = GetApiUrlV()
                .ResourceType(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType))
                .Action("explicit")
                .BuildUrl();

            return m_api.CallApi<ExplicitResult>(HttpMethod.POST, uri, parameters, null);
        }

        /// <summary>
        /// Creates the upload preset.
        /// Upload presets allow you to define the default behavior for your uploads, instead of
        /// receiving these as parameters during the upload request itself. Upload presets have
        /// precedence over client-side upload parameters asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of the upload preset.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after manipulation of upload presets.</returns>
        public Task<UploadPresetResult> CreateUploadPresetAsync(UploadPresetParams parameters, CancellationToken? cancellationToken = null)
        {
            string url = GetApiUrlV().
                Add("upload_presets").
                BuildUrl();

            return m_api.CallApiAsync<UploadPresetResult>(
                HttpMethod.POST,
                url,
                parameters,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Creates the upload preset.
        /// Upload presets allow you to define the default behavior for your uploads, instead of receiving these as parameters during the upload request itself. Upload presets have precedence over client-side upload parameters.
        /// </summary>
        /// <param name="parameters">Parameters of the upload preset.</param>
        /// <returns>Parsed response after manipulation of upload presets.</returns>
        public UploadPresetResult CreateUploadPreset(UploadPresetParams parameters)
        {
            var url = GetApiUrlV().
                Add("upload_presets").
                BuildUrl();

            return m_api.CallApi<UploadPresetResult>(HttpMethod.POST, url, parameters, null);
        }

        /// <summary>
        /// Updates the upload preset.
        /// Every update overwrites all the preset settings asynchronously.
        /// File specified as null because it's non-uploading action.
        /// </summary>
        /// <param name="parameters">New parameters for upload preset.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after manipulation of upload presets.</returns>
        public Task<UploadPresetResult> UpdateUploadPresetAsync(UploadPresetParams parameters, CancellationToken? cancellationToken = null) =>
            CallApiAsync<UploadPresetResult>(PrepareUploadPresetApiParams(parameters), cancellationToken);

        /// <summary>
        /// Updates the upload preset.
        /// Every update overwrites all the preset settings.
        /// File specified as null because it's non-uploading action.
        /// </summary>
        /// <param name="parameters">New parameters for upload preset.</param>
        /// <returns>Parsed response after manipulation of upload presets.</returns>
        public UploadPresetResult UpdateUploadPreset(UploadPresetParams parameters) =>
            CallApi<UploadPresetResult>(PrepareUploadPresetApiParams(parameters));

        private UploadPresetApiParams PrepareUploadPresetApiParams(UploadPresetParams parameters)
        {
            var paramsCopy = (UploadPresetParams)parameters.Copy();
            paramsCopy.Name = null;

            var url = GetApiUrlV()
                .Add("upload_presets")
                .Add(parameters.Name)
                .BuildUrl();

            return new UploadPresetApiParams(HttpMethod.PUT, url, paramsCopy);
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

        /// <summary>
        /// Gets the upload preset asynchronously.
        /// </summary>
        /// <param name="name">Name of the upload preset.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Upload preset details.</returns>
        public Task<GetUploadPresetResult> GetUploadPresetAsync(string name, CancellationToken? cancellationToken = null)
        {
            var url = GetApiUrlV()
                .Add("upload_presets")
                .Add(name)
                .BuildUrl();

            return m_api.CallApiAsync<GetUploadPresetResult>(
                HttpMethod.GET,
                url,
                null,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Gets the upload preset.
        /// </summary>
        /// <param name="name">Name of the upload preset.</param>
        /// <returns>Upload preset details.</returns>
        public GetUploadPresetResult GetUploadPreset(string name)
        {
            var url = GetApiUrlV()
                .Add("upload_presets")
                .Add(name)
                .BuildUrl();

            return m_api.CallApi<GetUploadPresetResult>(HttpMethod.GET, url, null, null);
        }

        /// <summary>
        /// Lists upload presets asynchronously.
        /// </summary>
        /// <param name="nextCursor">Next cursor.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of upload presets listing.</returns>
        public Task<ListUploadPresetsResult> ListUploadPresetsAsync(string nextCursor = null, CancellationToken? cancellationToken = null)
        {
            return ListUploadPresetsAsync(new ListUploadPresetsParams() { NextCursor = nextCursor }, cancellationToken);
        }

        /// <summary>
        /// Lists upload presets.
        /// </summary>
        /// <param name="nextCursor">(Optional) Starting position.</param>
        /// <returns>Parsed result of upload presets listing.</returns>
        public ListUploadPresetsResult ListUploadPresets(string nextCursor = null)
        {
            return ListUploadPresets(new ListUploadPresetsParams() { NextCursor = nextCursor });
        }

        /// <summary>
        /// Lists upload presets asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to list upload presets.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of upload presets listing.</returns>
        public Task<ListUploadPresetsResult> ListUploadPresetsAsync(ListUploadPresetsParams parameters, CancellationToken? cancellationToken = null)
        {
            var urlBuilder = new UrlBuilder(
                GetApiUrlV()
                .Add("upload_presets")
                .BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApiAsync<ListUploadPresetsResult>(
                HttpMethod.GET,
                urlBuilder.ToString(),
                parameters,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Lists upload presets.
        /// </summary>
        /// <param name="parameters">Parameters to list upload presets.</param>
        /// <returns>Parsed result of upload presets listing.</returns>
        public ListUploadPresetsResult ListUploadPresets(ListUploadPresetsParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                GetApiUrlV()
                .Add("upload_presets")
                .BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<ListUploadPresetsResult>(HttpMethod.GET, urlBuilder.ToString(), parameters, null);
        }

        /// <summary>
        /// Deletes the upload preset asynchronously.
        /// </summary>
        /// <param name="name">Name of the upload preset.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Result of upload preset deletion.</returns>
        public Task<DeleteUploadPresetResult> DeleteUploadPresetAsync(string name, CancellationToken? cancellationToken = null)
        {
            var url = GetApiUrlV()
                .Add("upload_presets")
                .Add(name)
                .BuildUrl();

            return m_api.CallApiAsync<DeleteUploadPresetResult>(
                HttpMethod.DELETE,
                url,
                null,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Deletes the upload preset.
        /// </summary>
        /// <param name="name">Name of the upload preset.</param>
        /// <returns>Result of upload preset deletion.</returns>
        public DeleteUploadPresetResult DeleteUploadPreset(string name)
        {
            var url = GetApiUrlV()
                .Add("upload_presets")
                .Add(name)
                .BuildUrl();

            return m_api.CallApi<DeleteUploadPresetResult>(HttpMethod.DELETE, url, null, null);
        }

        /// <summary>
        /// Uploads a resource to Cloudinary asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of uploading .</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Results of uploading.</returns>
        private Task<T> UploadAsync<T>(BasicRawUploadParams parameters, CancellationToken? cancellationToken = null)
            where T : UploadResult, new()
        {
            var uri = CheckUploadParametersAndGetUploadUrl(parameters);

            return m_api.CallApiAsync<T>(
                HttpMethod.POST,
                uri,
                parameters,
                parameters.File,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Uploads a resource to Cloudinary.
        /// </summary>
        /// <param name="parameters">Parameters of uploading .</param>
        /// <returns>Results of uploading.</returns>
        private T Upload<T, TP>(TP parameters)
            where T : UploadResult, new()
            where TP : BasicRawUploadParams, new()
        {
            var uri = CheckUploadParametersAndGetUploadUrl(parameters);

            return m_api.CallApi<T>(HttpMethod.POST, uri, parameters, parameters.File);
        }

        private string CheckUploadParametersAndGetUploadUrl(BasicRawUploadParams parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters), "Upload parameters should be defined");
            }

            string uri = GetApiUrlV()
                .Action(Constants.ACTION_NAME_UPLOAD)
                .ResourceType(ApiShared.GetCloudinaryParam(parameters.ResourceType))
                .BuildUrl();

            parameters.File.Reset();
            return uri;
        }

        /// <summary>
        /// Uploads an image file to Cloudinary asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of image uploading .</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Results of image uploading.</returns>
        public Task<ImageUploadResult> UploadAsync(ImageUploadParams parameters, CancellationToken? cancellationToken = null)
        {
            return UploadAsync<ImageUploadResult>(parameters, cancellationToken);
        }

        /// <summary>
        /// Uploads an image file to Cloudinary.
        /// </summary>
        /// <param name="parameters">Parameters of image uploading .</param>
        /// <returns>Results of image uploading.</returns>
        public ImageUploadResult Upload(ImageUploadParams parameters)
        {
            return Upload<ImageUploadResult, ImageUploadParams>(parameters);
        }

        /// <summary>
        /// Uploads a video file to Cloudinary asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of video uploading.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Results of video uploading.</returns>
        public Task<VideoUploadResult> UploadAsync(VideoUploadParams parameters, CancellationToken? cancellationToken = null)
        {
            return UploadAsync<VideoUploadResult>(parameters, cancellationToken);
        }

        /// <summary>
        /// Uploads a video file to Cloudinary.
        /// </summary>
        /// <param name="parameters">Parameters of video uploading.</param>
        /// <returns>Results of video uploading.</returns>
        public VideoUploadResult Upload(VideoUploadParams parameters)
        {
            return Upload<VideoUploadResult, VideoUploadParams>(parameters);
        }

        /// <summary>
        /// Uploads a file to Cloudinary asynchronously.
        /// </summary>
        /// <param name="resourceType">Resource type ("image", "raw", "video", "auto").</param>
        /// <param name="parameters">Upload parameters.</param>
        /// <param name="fileDescription">File description.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Results of the raw file uploading.</returns>
        public Task<RawUploadResult> UploadAsync(
            string resourceType,
            IDictionary<string, object> parameters,
            FileDescription fileDescription,
            CancellationToken? cancellationToken = null)
        {
            var uri = GetUploadUrl(resourceType);

            fileDescription.Reset();

            var dict = NormalizeParameters(parameters);

            return m_api.CallAndParseAsync<RawUploadResult>(
                HttpMethod.POST,
                uri,
                dict,
                fileDescription,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Uploads a file to Cloudinary.
        /// </summary>
        /// <param name="resourceType">Resource type ("image", "raw", "video", "auto").</param>
        /// <param name="parameters">Upload parameters.</param>
        /// <param name="fileDescription">File description.</param>
        /// <returns>Results of the raw file uploading.</returns>
        public RawUploadResult Upload(string resourceType, IDictionary<string, object> parameters, FileDescription fileDescription)
        {
            var uri = GetUploadUrl(resourceType);

            fileDescription.Reset();

            var dict = NormalizeParameters(parameters);

            return m_api.CallAndParse<RawUploadResult>(
                HttpMethod.POST,
                uri,
                dict,
                fileDescription);
        }

        private static SortedDictionary<string, object> NormalizeParameters(IDictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                return new SortedDictionary<string, object>();
            }

            return parameters as SortedDictionary<string, object> ?? new SortedDictionary<string, object>(parameters);
        }

        private string GetUploadUrl(string resourceType)
        {
            return GetApiUrlV().Action(Constants.ACTION_NAME_UPLOAD).ResourceType(resourceType).BuildUrl();
        }

        /// <summary>
        /// Async call to get a list of folders in the root asynchronously.
        /// </summary>
        /// <param name="parameters">(optional) Parameters for managing folders list.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of folders listing.</returns>
        public Task<GetFoldersResult> RootFoldersAsync(GetFoldersParams parameters = null, CancellationToken? cancellationToken = null)
        {
            return m_api.CallApiAsync<GetFoldersResult>(
                HttpMethod.GET,
                GetFolderUrl(parameters: parameters),
                null,
                null,
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Gets a list of folders in the root.
        /// </summary>
        /// <param name="parameters">(optional) Parameters for managing folders list.</param>
        /// <returns>Parsed result of folders listing.</returns>
        public GetFoldersResult RootFolders(GetFoldersParams parameters = null)
        {
            return m_api.CallApi<GetFoldersResult>(
                HttpMethod.GET,
                GetFolderUrl(parameters: parameters),
                null,
                null);
        }

        /// <summary>
        /// Gets a list of subfolders in a specified folder asynchronously.
        /// </summary>
        /// <param name="folder">The folder name.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of folders listing.</returns>
        public Task<GetFoldersResult> SubFoldersAsync(string folder, CancellationToken? cancellationToken = null)
        {
            return SubFoldersAsync(folder, null, cancellationToken);
        }

        /// <summary>
        /// Gets a list of subfolders in a specified folder asynchronously.
        /// </summary>
        /// <param name="folder">The folder name.</param>
        /// <param name="parameters">(Optional) Parameters for managing folders list.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of folders listing.</returns>
        public Task<GetFoldersResult> SubFoldersAsync(string folder, GetFoldersParams parameters, CancellationToken? cancellationToken = null)
        {
            CheckFolderParameter(folder);

            return m_api.CallApiAsync<GetFoldersResult>(
                HttpMethod.GET,
                GetFolderUrl(folder, parameters),
                null,
                null,
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Gets a list of subfolders in a specified folder.
        /// </summary>
        /// <param name="folder">The folder name.</param>
        /// <param name="parameters">(Optional) Parameters for managing folders list.</param>
        /// <returns>Parsed result of folders listing.</returns>
        public GetFoldersResult SubFolders(string folder, GetFoldersParams parameters = null)
        {
            CheckFolderParameter(folder);

            return m_api.CallApi<GetFoldersResult>(
                HttpMethod.GET,
                GetFolderUrl(folder, parameters),
                null,
                null);
        }

        private string GetFolderUrl(string folder = null, GetFoldersParams parameters = null)
        {
            var urlWithoutParams = GetApiUrlV().Add("folders").Add(folder).BuildUrl();

            return (parameters != null) ? new UrlBuilder(urlWithoutParams, parameters.ToParamsDictionary()).ToString() : urlWithoutParams;
        }

        private static void CheckFolderParameter(string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                throw new ArgumentException(
                    "folder must be set. Please use RootFolders() to get list of folders in root.");
            }
        }

        /// <summary>
        /// Deletes folder asynchronously.
        /// </summary>
        /// <param name="folder">Folder name.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of folder deletion.</returns>
        public Task<DeleteFolderResult> DeleteFolderAsync(string folder, CancellationToken? cancellationToken = null)
        {
            var uri = GetFolderUrl(folder);
            return m_api.CallApiAsync<DeleteFolderResult>(
                HttpMethod.DELETE,
                uri,
                null,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Deletes folder.
        /// </summary>
        /// <param name="folder">Folder name.</param>
        /// <returns>Parsed result of folder deletion.</returns>
        public DeleteFolderResult DeleteFolder(string folder)
        {
            var uri = GetFolderUrl(folder);
            return m_api.CallApi<DeleteFolderResult>(HttpMethod.DELETE, uri, null, null);
        }

        /// <summary>
        /// Create a new empty folder.
        /// </summary>
        /// <param name="folder">The full path of the new folder to create.</param>
        /// <returns>Parsed result of folder creation.</returns>
        public CreateFolderResult CreateFolder(string folder)
        {
            CheckIfNotEmpty(folder);

            return m_api.CallApi<CreateFolderResult>(
                HttpMethod.POST,
                GetFolderUrl(folder),
                null,
                null);
        }

        /// <summary>
        /// Create a new empty folder.
        /// </summary>
        /// <param name="folder">The full path of the new folder to create.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of folder creation.</returns>
        public Task<CreateFolderResult> CreateFolderAsync(string folder, CancellationToken? cancellationToken = null)
        {
            CheckIfNotEmpty(folder);

            return m_api.CallApiAsync<CreateFolderResult>(
                HttpMethod.POST,
                GetFolderUrl(folder),
                null,
                null,
                null,
                cancellationToken);
        }

        private static void CheckIfNotEmpty(string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                throw new ArgumentException("Folder must be set.");
            }
        }

        /// <summary>
        /// Gets the Cloudinary account usage details asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>The report on the status of your Cloudinary account usage details.</returns>
        public Task<UsageResult> GetUsageAsync(CancellationToken? cancellationToken = null)
        {
            string uri = GetApiUrlV().Action("usage").BuildUrl();

            return m_api.CallApiAsync<UsageResult>(
                HttpMethod.GET,
                uri,
                null,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Gets the Cloudinary account usage details.
        /// </summary>
        /// <returns>The report on the status of your Cloudinary account usage details.</returns>
        public UsageResult GetUsage()
        {
            string uri = GetApiUrlV().Action("usage").BuildUrl();

            return m_api.CallApi<UsageResult>(HttpMethod.GET, uri, null, null);
        }

        /// <summary>
        /// Uploads a file to Cloudinary asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="type">The type ("raw" or "auto", last by default).</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the raw file uploading.</returns>
        public Task<RawUploadResult> UploadAsync(RawUploadParams parameters, string type = "auto", CancellationToken? cancellationToken = null)
        {
            string uri = m_api.ApiUrlImgUpV.ResourceType(type).BuildUrl();

            parameters.File.Reset();

            return m_api.CallApiAsync<RawUploadResult>(
                HttpMethod.POST,
                uri,
                parameters,
                parameters.File,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Uploads a file to Cloudinary.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="type">The type ("raw" or "auto", last by default).</param>
        /// <returns>Parsed result of the raw file uploading.</returns>
        public RawUploadResult Upload(RawUploadParams parameters, string type = "auto")
        {
            string uri = m_api.ApiUrlImgUpV.ResourceType(type).BuildUrl();

            parameters.File.Reset();

            return m_api.CallApi<RawUploadResult>(HttpMethod.POST, uri, parameters, parameters.File);
        }

        /// <summary>
        /// Uploads large file by dividing it to chunks asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the large file uploading.</returns>
        /// <exception cref="System.ArgumentException">
        /// Please use BasicRawUploadParams class for large raw file uploading!
        /// or
        /// The UploadLargeRaw method is intended to be used for large local file uploading and can't be used for
        /// remote file uploading.
        /// </exception>
        public Task<RawUploadResult> UploadLargeRawAsync(
            BasicRawUploadParams parameters,
            int bufferSize = DEFAULT_CHUNK_SIZE,
            CancellationToken? cancellationToken = null)
        {
            return UploadLargeAsync<RawUploadResult>(parameters, bufferSize, cancellationToken);
        }

        /// <summary>
        /// Uploads large file to Cloudinary by dividing it to chunks.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <returns>Parsed result of the large file uploading.</returns>
        /// <exception cref="System.ArgumentException">
        /// Please use BasicRawUploadParams class for large raw file uploading!
        /// or
        /// The UploadLargeRaw method is intended to be used for large local file uploading and can't be used for remote file uploading.
        /// </exception>
        public RawUploadResult UploadLargeRaw(BasicRawUploadParams parameters, int bufferSize = DEFAULT_CHUNK_SIZE)
        {
            return UploadLarge<RawUploadResult>(parameters, bufferSize);
        }

        /// <summary>
        /// Uploads large raw file to Cloudinary by dividing it to chunks asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of uploading.</returns>
        public Task<RawUploadResult> UploadLargeAsync(
            RawUploadParams parameters,
            int bufferSize = DEFAULT_CHUNK_SIZE,
            CancellationToken? cancellationToken = null)
        {
            return UploadLargeAsync<RawUploadResult>(parameters, bufferSize, cancellationToken);
        }

        /// <summary>
        /// Uploads large raw file to Cloudinary by dividing it to chunks.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <returns>Parsed result of uploading.</returns>
        public RawUploadResult UploadLarge(RawUploadParams parameters, int bufferSize = DEFAULT_CHUNK_SIZE)
        {
            return UploadLarge<RawUploadResult>(parameters, bufferSize);
        }

        /// <summary>
        /// Uploads large image file to Cloudinary by dividing it to chunks asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of uploading.</returns>
        public Task<ImageUploadResult> UploadLargeAsync(
            ImageUploadParams parameters,
            int bufferSize = DEFAULT_CHUNK_SIZE,
            CancellationToken? cancellationToken = null)
        {
            return UploadLargeAsync<ImageUploadResult>(parameters, bufferSize, cancellationToken);
        }

        /// <summary>
        /// Uploads large image file to Cloudinary by dividing it to chunks.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <returns>Parsed result of uploading.</returns>
        public ImageUploadResult UploadLarge(ImageUploadParams parameters, int bufferSize = DEFAULT_CHUNK_SIZE)
        {
            return UploadLarge<ImageUploadResult>(parameters, bufferSize);
        }

        /// <summary>
        /// Uploads large video file to Cloudinary by dividing it to chunks asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of uploading.</returns>
        public Task<VideoUploadResult> UploadLargeAsync(
            VideoUploadParams parameters,
            int bufferSize = DEFAULT_CHUNK_SIZE,
            CancellationToken? cancellationToken = null)
        {
            return UploadLargeAsync<VideoUploadResult>(parameters, bufferSize, cancellationToken);
        }

        /// <summary>
        /// Uploads large video file to Cloudinary by dividing it to chunks.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <returns>Parsed result of uploading.</returns>
        public VideoUploadResult UploadLarge(VideoUploadParams parameters, int bufferSize = DEFAULT_CHUNK_SIZE)
        {
            return UploadLarge<VideoUploadResult>(parameters, bufferSize);
        }

        /// <summary>
        /// Uploads large resources to Cloudinary by dividing it to chunks.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <param name="isRaw">Whether the file is raw.</param>
        /// <returns>Parsed result of uploading.</returns>
        [Obsolete("Use UploadLarge(parameters, bufferSize) instead.")]
        public UploadResult UploadLarge(BasicRawUploadParams parameters, int bufferSize = DEFAULT_CHUNK_SIZE, bool isRaw = false)
        {
            if (isRaw)
            {
                return UploadLarge<RawUploadResult>(parameters, bufferSize);
            }
            else
            {
                return UploadLarge<ImageUploadResult>(parameters, bufferSize);
            }
        }

        /// <summary>
        /// Uploads large resources to Cloudinary by dividing it to chunks asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of result of upload.</typeparam>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of uploading.</returns>
        public async Task<T> UploadLargeAsync<T>(
            BasicRawUploadParams parameters,
            int bufferSize = DEFAULT_CHUNK_SIZE,
            CancellationToken? cancellationToken = null)
            where T : UploadResult, new()
        {
            CheckUploadParameters(parameters);

            if (parameters.File.IsRemote)
            {
                return await UploadAsync<T>(parameters);
            }

            var internalParams = new UploadLargeParams(parameters, bufferSize, m_api);
            T result = null;

            while (!parameters.File.Eof)
            {
                UpdateContentRange(internalParams);
                result = await m_api.CallApiAsync<T>(
                    HttpMethod.POST,
                    internalParams.Url,
                    parameters,
                    parameters.File,
                    internalParams.Headers,
                    cancellationToken);
                CheckUploadResult(result);
            }

            return result;
        }

        /// <summary>
        /// Uploads large resources to Cloudinary by dividing it to chunks.
        /// </summary>
        /// <typeparam name="T">The type of result of upload.</typeparam>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <returns>Parsed result of uploading.</returns>
        public T UploadLarge<T>(BasicRawUploadParams parameters, int bufferSize = DEFAULT_CHUNK_SIZE)
            where T : UploadResult, new()
        {
            CheckUploadParameters(parameters);

            if (parameters.File.IsRemote)
            {
                return Upload<T, BasicRawUploadParams>(parameters);
            }

            var internalParams = new UploadLargeParams(parameters, bufferSize, m_api);
            T result = null;

            while (!parameters.File.Eof)
            {
                UpdateContentRange(internalParams);
                result = m_api.CallApi<T>(HttpMethod.POST, internalParams.Url, parameters, parameters.File, internalParams.Headers);
                CheckUploadResult(result);
            }

            return result;
        }

        /// <summary>
        /// Upload large file parameters.
        /// </summary>
        internal class UploadLargeParams
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="UploadLargeParams"/> class.
            /// </summary>
            /// <param name="parameters">Basic raw upload parameters.</param>
            /// <param name="bufferSize">Buffer size.</param>
            /// <param name="api">Technological layer to work with cloudinary API.</param>
            public UploadLargeParams(BasicRawUploadParams parameters, int bufferSize, Api api)
            {
                parameters.File.Reset(bufferSize);
                this.Parameters = parameters;
                this.Url = GetUploadUrl(parameters, api);
                this.BufferSize = bufferSize;
            }

            /// <summary>
            /// Gets buffer size.
            /// </summary>
            public int BufferSize { get; }

            /// <summary>
            /// Gets url.
            /// </summary>
            public string Url { get; }

            /// <summary>
            /// Gets basic raw upload parameters.
            /// </summary>
            public BasicRawUploadParams Parameters { get; }

            /// <summary>
            /// Gets request headers.
            /// </summary>
            public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>
            {
                ["X-Unique-Upload-Id"] = RandomPublicId(),
            };

            /// <summary>
            /// Generate random PublicId.
            /// </summary>
            /// <returns>Randomly generated PublicId.</returns>
            private static string RandomPublicId()
            {
                var buffer = new byte[8];
                new Random().NextBytes(buffer);
                return string.Concat(buffer.Select(x => x.ToString("X2", CultureInfo.InvariantCulture)).ToArray());
            }

            /// <summary>
            /// A convenient method for uploading an image before testing.
            /// </summary>
            /// <param name="parameters">Parameters of type BasicRawUploadParams.</param>
            /// <param name="mApi">Action to set custom upload parameters.</param>
            /// <returns>The upload url.</returns>
            private string GetUploadUrl(BasicRawUploadParams parameters, Api mApi)
            {
                var url = mApi.ApiUrlImgUpV;
                var name = Enum.GetName(typeof(ResourceType), parameters.ResourceType);
                if (name != null)
                {
                    url.ResourceType(name.ToLowerInvariant());
                }

                return url.BuildUrl();
            }
        }

        private static void UpdateContentRange(UploadLargeParams internalParams)
        {
            var fileDescription = internalParams.Parameters.File;
            var fileLength = fileDescription.GetFileLength();
            var startOffset = fileDescription.BytesSent;
            var endOffset = startOffset + Math.Min(internalParams.BufferSize, fileLength - startOffset) - 1;

            internalParams.Headers["Content-Range"] = $"bytes {startOffset}-{endOffset}/{fileLength}";
        }

        private static void CheckUploadResult<T>(T result)
            where T : UploadResult, new()
        {
            if (result.StatusCode != HttpStatusCode.OK)
            {
                var error = result.Error != null ? result.Error.Message : "Unknown error";
                throw new Exception(
                    $"An error has occured while uploading file (status code: {result.StatusCode}). {error}");
            }
        }

        private static void CheckUploadParameters(BasicRawUploadParams parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters), "Upload parameters should be defined");
            }

            if (parameters.File == null)
            {
                throw new ArgumentException("Parameters.File parameter should be defined");
            }
        }

        /// <summary>
        /// Changes public identifier of a file asynchronously.
        /// </summary>
        /// <param name="fromPublicId">Old identifier.</param>
        /// <param name="toPublicId">New identifier.</param>
        /// <param name="overwrite">Overwrite a file with the same identifier as new if such file exists.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Result of resource renaming.</returns>
        public Task<RenameResult> RenameAsync(string fromPublicId, string toPublicId, bool overwrite = false, CancellationToken? cancellationToken = null)
        {
            return RenameAsync(
                new RenameParams(fromPublicId, toPublicId)
                {
                    Overwrite = overwrite,
                },
                cancellationToken);
        }

        /// <summary>
        /// Changes public identifier of a file.
        /// </summary>
        /// <param name="fromPublicId">Old identifier.</param>
        /// <param name="toPublicId">New identifier.</param>
        /// <param name="overwrite">Overwrite a file with the same identifier as new if such file exists.</param>
        /// <returns>Result of resource renaming.</returns>
        public RenameResult Rename(string fromPublicId, string toPublicId, bool overwrite = false)
        {
            return Rename(
                new RenameParams(fromPublicId, toPublicId)
                {
                    Overwrite = overwrite,
                });
        }

        /// <summary>
        /// Changes public identifier of a file asynchronously.
        /// </summary>
        /// <param name="parameters">Operation parameters.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Result of resource renaming.</returns>
        public Task<RenameResult> RenameAsync(RenameParams parameters, CancellationToken? cancellationToken = null)
        {
            var uri = GetRenameUrl(parameters);
            return m_api.CallApiAsync<RenameResult>(
                HttpMethod.POST,
                uri,
                parameters,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Changes public identifier of a file.
        /// </summary>
        /// <param name="parameters">Operation parameters.</param>
        /// <returns>Result of resource renaming.</returns>
        public RenameResult Rename(RenameParams parameters)
        {
            var uri = GetRenameUrl(parameters);
            return m_api.CallApi<RenameResult>(HttpMethod.POST, uri, parameters, null);
        }

        private string GetRenameUrl(RenameParams parameters) =>
            m_api
                .ApiUrlImgUpV
                .ResourceType(ApiShared.GetCloudinaryParam(parameters.ResourceType))
                .Action("rename")
                .BuildUrl();

        /// <summary>
        /// Delete file from Cloudinary asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters for deletion of resource from Cloudinary.</param>
        /// <returns>Results of deletion.</returns>
        public Task<DeletionResult> DestroyAsync(DeletionParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.ResourceType(
                ApiShared.GetCloudinaryParam(parameters.ResourceType)).
                Action("destroy").BuildUrl();

            return m_api.CallApiAsync<DeletionResult>(HttpMethod.POST, uri, parameters, null);
        }

        /// <summary>
        /// Deletes file from Cloudinary.
        /// </summary>
        /// <param name="parameters">Parameters for deletion of resource from Cloudinary.</param>
        /// <returns>Results of deletion.</returns>
        public DeletionResult Destroy(DeletionParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.ResourceType(
                Api.GetCloudinaryParam(parameters.ResourceType)).
                Action("destroy").BuildUrl();

            return m_api.CallApi<DeletionResult>(HttpMethod.POST, uri, parameters, null);
        }

        /// <summary>
        /// Generate an image of a given textual string asynchronously.
        /// </summary>
        /// <param name="text">Text to draw.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Results of generating an image of a given textual string.</returns>
        public Task<TextResult> TextAsync(string text, CancellationToken? cancellationToken = null)
        {
            return TextAsync(new TextParams(text), cancellationToken);
        }

        /// <summary>
        /// Generates an image of a given textual string.
        /// </summary>
        /// <param name="text">Text to draw.</param>
        /// <returns>Results of generating an image of a given textual string.</returns>
        public TextResult Text(string text)
        {
            return Text(new TextParams(text));
        }

        /// <summary>
        /// Generates an image of a given textual string asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of generating an image of a given textual string.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Results of generating an image of a given textual string.</returns>
        public Task<TextResult> TextAsync(TextParams parameters, CancellationToken? cancellationToken = null)
        {
            string uri = m_api.ApiUrlImgUpV.Action("text").BuildUrl();

            return m_api.CallApiAsync<TextResult>(
                HttpMethod.POST,
                uri,
                parameters,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Generates an image of a given textual string.
        /// </summary>
        /// <param name="parameters">Parameters of generating an image of a given textual string.</param>
        /// <returns>Results of generating an image of a given textual string.</returns>
        public TextResult Text(TextParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.Action("text").BuildUrl();

            return m_api.CallApi<TextResult>(HttpMethod.POST, uri, parameters, null);
        }

        /// <summary>
        /// Lists resource types asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed list of resource types.</returns>
        public Task<ListResourceTypesResult> ListResourceTypesAsync(CancellationToken? cancellationToken = null)
        {
            return m_api.CallApiAsync<ListResourceTypesResult>(
                HttpMethod.GET,
                GetApiUrlV().Add("resources").BuildUrl(),
                null,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Lists resource types.
        /// </summary>
        /// <returns>Parsed list of resource types.</returns>
        public ListResourceTypesResult ListResourceTypes()
        {
            return m_api.CallApi<ListResourceTypesResult>(HttpMethod.GET, GetApiUrlV().Add("resources").BuildUrl(), null, null);
        }

        /// <summary>
        /// Lists resources asynchronously asynchronously.
        /// </summary>
        /// <param name="nextCursor">Starting position.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourcesAsync(
            string nextCursor = null,
            bool tags = true,
            bool context = true,
            bool moderations = true,
            CancellationToken? cancellationToken = null)
        {
            var listResourcesParams = new ListResourcesParams()
            {
                NextCursor = nextCursor,
                Tags = tags,
                Context = context,
                Moderations = moderations,
            };
            return ListResourcesAsync(listResourcesParams, cancellationToken);
        }

        /// <summary>
        /// Lists resources.
        /// </summary>
        /// <param name="nextCursor">Starting position.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResources(
            string nextCursor = null,
            bool tags = true,
            bool context = true,
            bool moderations = true)
        {
            return ListResources(new ListResourcesParams()
            {
                NextCursor = nextCursor,
                Tags = tags,
                Context = context,
                Moderations = moderations,
            });
        }

        /// <summary>
        /// Lists resources of specified type asynchronously.
        /// </summary>
        /// <param name="type">Resource type.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourcesByTypeAsync(string type, string nextCursor = null, CancellationToken? cancellationToken = null)
        {
            return ListResourcesAsync(new ListResourcesParams() { Type = type, NextCursor = nextCursor }, cancellationToken);
        }

        /// <summary>
        /// Lists resources of specified type.
        /// </summary>
        /// <param name="type">Resource type.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResourcesByType(string type, string nextCursor = null)
        {
            return ListResources(new ListResourcesParams() { Type = type, NextCursor = nextCursor });
        }

        /// <summary>
        /// Lists resources by prefix asynchronously.
        /// </summary>
        /// <param name="prefix">Public identifier prefix.</param>
        /// <param name="type">Resource type.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourcesByPrefixAsync(
            string prefix,
            string type = "upload",
            string nextCursor = null,
            CancellationToken? cancellationToken = null)
        {
            var listResourcesByPrefixParams = new ListResourcesByPrefixParams()
            {
                Type = type,
                Prefix = prefix,
                NextCursor = nextCursor,
            };
            return ListResourcesAsync(listResourcesByPrefixParams, cancellationToken);
        }

        /// <summary>
        /// Lists resources by prefix.
        /// </summary>
        /// <param name="prefix">Public identifier prefix.</param>
        /// <param name="type">Resource type.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResourcesByPrefix(string prefix, string type = "upload", string nextCursor = null)
        {
            return ListResources(new ListResourcesByPrefixParams()
            {
                Type = type,
                Prefix = prefix,
                NextCursor = nextCursor,
            });
        }

        /// <summary>
        /// Lists resources by prefix asynchronously.
        /// </summary>
        /// <param name="prefix">Public identifier prefix.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">If true, include moderation status for each resource.</param>
        /// <param name="type">Resource type.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourcesByPrefixAsync(
            string prefix,
            bool tags,
            bool context,
            bool moderations,
            string type = "upload",
            string nextCursor = null,
            CancellationToken? cancellationToken = null)
        {
            var listResourcesByPrefixParams = new ListResourcesByPrefixParams()
            {
                Tags = tags,
                Context = context,
                Moderations = moderations,
                Type = type,
                Prefix = prefix,
                NextCursor = nextCursor,
            };
            return ListResourcesAsync(listResourcesByPrefixParams, cancellationToken);
        }

        /// <summary>
        /// Lists resources by prefix.
        /// </summary>
        /// <param name="prefix">Public identifier prefix.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">If true, include moderation status for each resource.</param>
        /// <param name="type">Resource type.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResourcesByPrefix(string prefix, bool tags, bool context, bool moderations, string type = "upload", string nextCursor = null)
        {
            return ListResources(new ListResourcesByPrefixParams()
            {
                Tags = tags,
                Context = context,
                Moderations = moderations,
                Type = type,
                Prefix = prefix,
                NextCursor = nextCursor,
            });
        }

        /// <summary>
        /// Lists resources by tag asynchronously.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourcesByTagAsync(string tag, string nextCursor = null, CancellationToken? cancellationToken = null)
        {
            var listResourcesByTagParams = new ListResourcesByTagParams()
            {
                Tag = tag,
                NextCursor = nextCursor,
            };
            return ListResourcesAsync(listResourcesByTagParams, cancellationToken);
        }

        /// <summary>
        /// Lists resources by tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResourcesByTag(string tag, string nextCursor = null)
        {
            return ListResources(new ListResourcesByTagParams()
            {
                Tag = tag,
                NextCursor = nextCursor,
            });
        }

        /// <summary>
        /// Returns resources with specified public identifiers asynchronously.
        /// </summary>
        /// <param name="publicIds">Public identifiers.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourcesByPublicIdsAsync(IEnumerable<string> publicIds, CancellationToken? cancellationToken = null)
        {
            var listSpecificResourcesParams = new ListSpecificResourcesParams()
            {
                PublicIds = new List<string>(publicIds),
            };
            return ListResourcesAsync(listSpecificResourcesParams, cancellationToken);
        }

        /// <summary>
        /// Returns resources with specified public identifiers.
        /// </summary>
        /// <param name="publicIds">Public identifiers.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResourcesByPublicIds(IEnumerable<string> publicIds)
        {
            return ListResources(new ListSpecificResourcesParams()
            {
                PublicIds = new List<string>(publicIds),
            });
        }

        /// <summary>
        /// Returns resources with specified public identifiers asynchronously.
        /// </summary>
        /// <param name="publicIds">Public identifiers.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourceByPublicIdsAsync(
            IEnumerable<string> publicIds,
            bool tags,
            bool context,
            bool moderations,
            CancellationToken? cancellationToken = null)
        {
            var listSpecificResourcesParams = new ListSpecificResourcesParams()
            {
                PublicIds = new List<string>(publicIds),
                Tags = tags,
                Context = context,
                Moderations = moderations,
            };
            return ListResourcesAsync(listSpecificResourcesParams, cancellationToken);
        }

        /// <summary>
        /// Returns resources with specified public identifiers.
        /// </summary>
        /// <param name="publicIds">Public identifiers.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResourceByPublicIds(IEnumerable<string> publicIds, bool tags, bool context, bool moderations)
        {
            return ListResources(new ListSpecificResourcesParams()
            {
                PublicIds = new List<string>(publicIds),
                Tags = tags,
                Context = context,
                Moderations = moderations,
            });
        }

        /// <summary>
        /// Lists resources by moderation status asynchronously.
        /// </summary>
        /// <param name="kind">The moderation kind.</param>
        /// <param name="status">The moderation status.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <param name="nextCursor">The next cursor.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourcesByModerationStatusAsync(
            string kind,
            ModerationStatus status,
            bool tags = true,
            bool context = true,
            bool moderations = true,
            string nextCursor = null,
            CancellationToken? cancellationToken = null)
        {
            var listResourcesByModerationParams = new ListResourcesByModerationParams()
            {
                ModerationKind = kind,
                ModerationStatus = status,
                Tags = tags,
                Context = context,
                Moderations = moderations,
                NextCursor = nextCursor,
            };
            return ListResourcesAsync(listResourcesByModerationParams, cancellationToken);
        }

        /// <summary>
        /// Lists resources by moderation status.
        /// </summary>
        /// <param name="kind">The moderation kind.</param>
        /// <param name="status">The moderation status.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <param name="nextCursor">The next cursor.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResourcesByModerationStatus(
            string kind,
            ModerationStatus status,
            bool tags = true,
            bool context = true,
            bool moderations = true,
            string nextCursor = null)
        {
            return ListResources(new ListResourcesByModerationParams()
            {
                ModerationKind = kind,
                ModerationStatus = status,
                Tags = tags,
                Context = context,
                Moderations = moderations,
                NextCursor = nextCursor,
            });
        }

        /// <summary>
        /// List resources by context metadata keys and values asynchronously.
        /// </summary>
        /// <param name="key">Only resources with the given key should be returned.</param>
        /// <param name="value">When provided should only return resources with this given value for the context key.
        /// When not provided, return all resources for which the context key exists.</param>
        /// <param name="tags">If true, include list of tag names assigned for each resource.</param>
        /// <param name="context">If true, include context assigned to each resource.</param>
        /// <param name="nextCursor">The next cursor.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourcesByContextAsync(
            string key,
            string value = "",
            bool tags = false,
            bool context = false,
            string nextCursor = null,
            CancellationToken? cancellationToken = null)
        {
            var listResourcesByContextParams = new ListResourcesByContextParams()
            {
                Key = key,
                Value = value,
                Tags = tags,
                Context = context,
                NextCursor = nextCursor,
            };
            return ListResourcesAsync(listResourcesByContextParams, cancellationToken);
        }

        /// <summary>
        /// List resources by context metadata keys and values.
        /// </summary>
        /// <param name="key">Only resources with the given key should be returned.</param>
        /// <param name="value">When provided should only return resources with this given value for the context key.
        /// When not provided, return all resources for which the context key exists.</param>
        /// <param name="tags">If true, include list of tag names assigned for each resource.</param>
        /// <param name="context">If true, include context assigned to each resource.</param>
        /// <param name="nextCursor">The next cursor.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResourcesByContext(string key, string value = "", bool tags = false, bool context = false, string nextCursor = null)
        {
            return ListResources(new ListResourcesByContextParams()
            {
                Key = key,
                Value = value,
                Tags = tags,
                Context = context,
                NextCursor = nextCursor,
            });
        }

        /// <summary>
        /// Gets a list of resources asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to list resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public Task<ListResourcesResult> ListResourcesAsync(ListResourcesParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = GetListResourcesUrl(parameters);
            return m_api.CallApiAsync<ListResourcesResult>(HttpMethod.GET, url, parameters, null, null, cancellationToken);
        }

        /// <summary>
        /// Gets a list of resources.
        /// </summary>
        /// <param name="parameters">Parameters to list resources.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResources(ListResourcesParams parameters)
        {
            var url = GetListResourcesUrl(parameters);
            return m_api.CallApi<ListResourcesResult>(HttpMethod.GET, url, parameters, null);
        }

        private string GetListResourcesUrl(ListResourcesParams parameters)
        {
            var url = GetApiUrlV().ResourceType("resources").Add(ApiShared.GetCloudinaryParam(parameters.ResourceType));

            switch (parameters)
            {
                case ListResourcesByTagParams tagParams:
                    if (!string.IsNullOrEmpty(tagParams.Tag))
                    {
                        url.Add("tags").Add(tagParams.Tag);
                    }

                    break;
                case ListResourcesByModerationParams modParams:
                    if (!string.IsNullOrEmpty(modParams.ModerationKind))
                    {
                        url.Add("moderations")
                            .Add(modParams.ModerationKind)
                            .Add(Api.GetCloudinaryParam(modParams.ModerationStatus));
                    }

                    break;
                case ListResourcesByContextParams _:
                    {
                        url.Add("context");
                    }

                    break;
            }

            var urlBuilder = new UrlBuilder(
                url.BuildUrl(),
                parameters.ToParamsDictionary());

            var s = urlBuilder.ToString();
            return s;
        }

        /// <summary>
        /// Gets a list of tags asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed list of tags.</returns>
        public Task<ListTagsResult> ListTagsAsync(CancellationToken? cancellationToken = null)
        {
            return ListTagsAsync(new ListTagsParams(), cancellationToken);
        }

        /// <summary>
        /// Gets a list of all tags.
        /// </summary>
        /// <returns>Parsed list of tags.</returns>
        public ListTagsResult ListTags()
        {
            return ListTags(new ListTagsParams());
        }

        /// <summary>
        /// Finds all tags that start with the given prefix asynchronously.
        /// </summary>
        /// <param name="prefix">The tag prefix.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed list of tags.</returns>
        public Task<ListTagsResult> ListTagsByPrefixAsync(string prefix, CancellationToken? cancellationToken = null)
        {
            return ListTagsAsync(new ListTagsParams() { Prefix = prefix }, cancellationToken);
        }

        /// <summary>
        /// Finds all tags that start with the given prefix.
        /// </summary>
        /// <param name="prefix">The tag prefix.</param>
        /// <returns>Parsed list of tags.</returns>
        public ListTagsResult ListTagsByPrefix(string prefix)
        {
            return ListTags(new ListTagsParams() { Prefix = prefix });
        }

        /// <summary>
        /// Gets a list of tags asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of the request.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed list of tags.</returns>
        public Task<ListTagsResult> ListTagsAsync(ListTagsParams parameters, CancellationToken? cancellationToken = null)
        {
            var urlBuilder = new UrlBuilder(
                GetApiUrlV().
                ResourceType("tags").
                Add(ApiShared.GetCloudinaryParam(parameters.ResourceType)).
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApiAsync<ListTagsResult>(HttpMethod.GET, urlBuilder.ToString(), parameters, null, null, cancellationToken);
        }

        /// <summary>
        /// Gets a list of tags.
        /// </summary>
        /// <param name="parameters">Parameters of the request.</param>
        /// <returns>Parsed list of tags.</returns>
        public ListTagsResult ListTags(ListTagsParams parameters)
        {
            var urlBuilder = new UrlBuilder(
                GetApiUrlV().
                ResourceType("tags").
                Add(ApiShared.GetCloudinaryParam(parameters.ResourceType)).
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<ListTagsResult>(HttpMethod.GET, urlBuilder.ToString(), parameters, null);
        }

        /// <summary>
        /// Gets a list of transformations asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed list of transformations details.</returns>
        public Task<ListTransformsResult> ListTransformationsAsync(CancellationToken? cancellationToken = null)
        {
            return ListTransformationsAsync(new ListTransformsParams(), cancellationToken);
        }

        /// <summary>
        /// Gets a list of transformations.
        /// </summary>
        /// <returns>Parsed list of transformations details.</returns>
        public ListTransformsResult ListTransformations()
        {
            return ListTransformations(new ListTransformsParams());
        }

        /// <summary>
        /// Gets a list of transformations asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of the request for a list of transformation.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed list of transformations details.</returns>
        public Task<ListTransformsResult> ListTransformationsAsync(ListTransformsParams parameters, CancellationToken? cancellationToken = null)
        {
            var urlBuilder = new UrlBuilder(
                GetApiUrlV().
                ResourceType("transformations").
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApiAsync<ListTransformsResult>(
                HttpMethod.GET,
                urlBuilder.ToString(),
                parameters,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Gets a list of transformations.
        /// </summary>
        /// <param name="parameters">Parameters of the request for a list of transformation.</param>
        /// <returns>Parsed list of transformations details.</returns>
        public ListTransformsResult ListTransformations(ListTransformsParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                GetApiUrlV().
                ResourceType("transformations").
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<ListTransformsResult>(HttpMethod.GET, urlBuilder.ToString(), parameters, null);
        }

        /// <summary>
        /// Gets details of a single transformation asynchronously.
        /// </summary>
        /// <param name="transform">Name of the transformation.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed details of a single transformation.</returns>
        public Task<GetTransformResult> GetTransformAsync(string transform, CancellationToken? cancellationToken = null)
        {
            return GetTransformAsync(new GetTransformParams() { Transformation = transform }, cancellationToken);
        }

        /// <summary>
        /// Gets details of a single transformation by name.
        /// </summary>
        /// <param name="transform">Name of the transformation.</param>
        /// <returns>Parsed details of a single transformation.</returns>
        public GetTransformResult GetTransform(string transform)
        {
            return GetTransform(new GetTransformParams() { Transformation = transform });
        }

        /// <summary>
        /// Gets details of a single transformation asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of the request of transformation details.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed details of a single transformation.</returns>
        public Task<GetTransformResult> GetTransformAsync(GetTransformParams parameters, CancellationToken? cancellationToken = null)
        {
            var urlBuilder = new UrlBuilder(
                GetApiUrlV().
                ResourceType("transformations").
                Add(parameters.Transformation).
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApiAsync<GetTransformResult>(
                HttpMethod.GET,
                urlBuilder.ToString(),
                parameters,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Gets details of a single transformation.
        /// </summary>
        /// <param name="parameters">Parameters of the request of transformation details.</param>
        /// <returns>Parsed details of a single transformation.</returns>
        public GetTransformResult GetTransform(GetTransformParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                GetApiUrlV().
                ResourceType("transformations").
                Add(parameters.Transformation).
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<GetTransformResult>(HttpMethod.GET, urlBuilder.ToString(), parameters, null);
        }

        /// <summary>
        /// Updates details of an existing resource asynchronously.
        /// </summary>
        /// <param name="publicId">The public ID of the resource to update.</param>
        /// <param name="moderationStatus">The image moderation status.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response of the detailed resource information.</returns>
        public Task<GetResourceResult> UpdateResourceAsync(string publicId, ModerationStatus moderationStatus, CancellationToken? cancellationToken = null)
        {
            return UpdateResourceAsync(new UpdateParams(publicId) { ModerationStatus = moderationStatus }, cancellationToken);
        }

        /// <summary>
        /// Updates details of an existing resource.
        /// </summary>
        /// <param name="publicId">The public ID of the resource to update.</param>
        /// <param name="moderationStatus">The image moderation status.</param>
        /// <returns>Parsed response of the detailed resource information.</returns>
        public GetResourceResult UpdateResource(string publicId, ModerationStatus moderationStatus)
        {
            return UpdateResource(new UpdateParams(publicId) { ModerationStatus = moderationStatus });
        }

        /// <summary>
        /// Updates details of an existing resource asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to update details of an existing resource.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response of the detailed resource information.</returns>
        public Task<GetResourceResult> UpdateResourceAsync(UpdateParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = GetApiUrlV().
                ResourceType("resources").
                Add(ApiShared.GetCloudinaryParam(parameters.ResourceType)).
                Add(parameters.Type).Add(parameters.PublicId).
                BuildUrl();

            return m_api.CallApiAsync<GetResourceResult>(HttpMethod.POST, url, parameters, null, null, cancellationToken);
        }

        /// <summary>
        /// Updates details of an existing resource.
        /// </summary>
        /// <param name="parameters">Parameters to update details of an existing resource.</param>
        /// <returns>Parsed response of the detailed resource information.</returns>
        public GetResourceResult UpdateResource(UpdateParams parameters)
        {
            var url = GetApiUrlV().
                ResourceType("resources").
                Add(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType)).
                Add(parameters.Type).Add(parameters.PublicId).
                BuildUrl();

            return m_api.CallApi<GetResourceResult>(HttpMethod.POST, url, parameters, null);
        }

        /// <summary>
        /// Gets details of a single resource as well as all its derived resources by its public ID asynchronously.
        /// </summary>
        /// <param name="publicId">The public ID of the resource.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response with the detailed resource information.</returns>
        public Task<GetResourceResult> GetResourceAsync(string publicId, CancellationToken? cancellationToken = null)
        {
            return GetResourceAsync(new GetResourceParams(publicId), cancellationToken);
        }

        /// <summary>
        /// Gets details of a single resource as well as all its derived resources by its public ID.
        /// </summary>
        /// <param name="publicId">The public ID of the resource.</param>
        /// <returns>Parsed response with the detailed resource information.</returns>
        public GetResourceResult GetResource(string publicId)
        {
            return GetResource(new GetResourceParams(publicId));
        }

        /// <summary>
        /// Gets details of the requested resource as well as all its derived resources asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of the request of resource.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response with the detailed resource information.</returns>
        public Task<GetResourceResult> GetResourceAsync(GetResourceParams parameters, CancellationToken? cancellationToken = null)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                GetApiUrlV().
                ResourceType("resources").
                Add(ApiShared.GetCloudinaryParam(parameters.ResourceType)).
                Add(parameters.Type).
                Add(parameters.PublicId).
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApiAsync<GetResourceResult>(
                HttpMethod.GET,
                urlBuilder.ToString(),
                parameters,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Gets details of the requested resource as well as all its derived resources.
        /// </summary>
        /// <param name="parameters">Parameters of the request of resource.</param>
        /// <returns>Parsed response with the detailed resource information.</returns>
        public GetResourceResult GetResource(GetResourceParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                GetApiUrlV().
                ResourceType("resources").
                Add(Api.GetCloudinaryParam(parameters.ResourceType)).
                Add(parameters.Type).
                Add(parameters.PublicId).
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<GetResourceResult>(HttpMethod.GET, urlBuilder.ToString(), parameters, null);
        }

        /// <summary>
        /// Deletes all derived resources with the given IDs asynchronously.
        /// </summary>
        /// <param name="ids">An array of up to 100 derived_resource_ids.</param>
        /// <returns>Parsed result of deletion derived resources.</returns>
        public Task<DelDerivedResResult> DeleteDerivedResourcesAsync(params string[] ids)
        {
            var p = new DelDerivedResParams();
            p.DerivedResources.AddRange(ids);
            return DeleteDerivedResourcesAsync(p);
        }

        /// <summary>
        /// Deletes all derived resources with the given IDs.
        /// </summary>
        /// <param name="ids">An array of up to 100 derived_resource_ids.</param>
        /// <returns>Parsed result of deletion derived resources.</returns>
        public DelDerivedResResult DeleteDerivedResources(params string[] ids)
        {
            DelDerivedResParams p = new DelDerivedResParams();
            p.DerivedResources.AddRange(ids);
            return DeleteDerivedResources(p);
        }

        /// <summary>
        /// Deletes all derived resources with the given parameters asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to delete derived resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of deletion derived resources.</returns>
        public Task<DelDerivedResResult> DeleteDerivedResourcesAsync(DelDerivedResParams parameters, CancellationToken? cancellationToken = null)
        {
            var urlBuilder = new UrlBuilder(
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
        /// Deletes all derived resources with the given parameters.
        /// </summary>
        /// <param name="parameters">Parameters to delete derived resources.</param>
        /// <returns>Parsed result of deletion derived resources.</returns>
        public DelDerivedResResult DeleteDerivedResources(DelDerivedResParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                GetApiUrlV().
                Add("derived_resources").
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<DelDerivedResResult>(HttpMethod.DELETE, urlBuilder.ToString(), parameters, null);
        }

        /// <summary>
        /// Deletes all resources of the given resource type and with the given public IDs asynchronously.
        /// </summary>
        /// <param name="type">The type of file to delete. Default: image.</param>
        /// <param name="publicIds">Array of up to 100 public_ids.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        public Task<DelResResult> DeleteResourcesAsync(ResourceType type, params string[] publicIds)
        {
            var p = new DelResParams() { ResourceType = type };
            p.PublicIds.AddRange(publicIds);
            return DeleteResourcesAsync(p);
        }

        /// <summary>
        /// Deletes all resources of the given resource type and with the given public IDs.
        /// </summary>
        /// <param name="type">The type of file to delete. Default: image.</param>
        /// <param name="publicIds">Array of up to 100 public_ids.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        public DelResResult DeleteResources(ResourceType type, params string[] publicIds)
        {
            DelResParams p = new DelResParams() { ResourceType = type };
            p.PublicIds.AddRange(publicIds);
            return DeleteResources(p);
        }

        /// <summary>
        /// Deletes all resources with the given public IDs asynchronously.
        /// </summary>
        /// <param name="publicIds">Array of up to 100 public_ids.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        public Task<DelResResult> DeleteResourcesAsync(params string[] publicIds)
        {
            var p = new DelResParams();
            p.PublicIds.AddRange(publicIds);
            return DeleteResourcesAsync(p);
        }

        /// <summary>
        /// Deletes all resources with the given public IDs.
        /// </summary>
        /// <param name="publicIds">Array of up to 100 public_ids.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        public DelResResult DeleteResources(params string[] publicIds)
        {
            DelResParams p = new DelResParams();
            p.PublicIds.AddRange(publicIds);
            return DeleteResources(p);
        }

        /// <summary>
        /// Deletes all resources, including derived resources, where the public ID starts with the given prefix (up to
        /// a maximum of 1000 original resources) asynchronously.
        /// </summary>
        /// <param name="prefix">Delete all resources where the public ID starts with the given prefix. </param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        public Task<DelResResult> DeleteResourcesByPrefixAsync(string prefix, CancellationToken? cancellationToken = null)
        {
            var p = new DelResParams() { Prefix = prefix };
            return DeleteResourcesAsync(p, cancellationToken);
        }

        /// <summary>
        /// Deletes all resources, including derived resources, where the public ID starts with the given prefix (up to
        /// a maximum of 1000 original resources).
        /// </summary>
        /// <param name="prefix">Delete all resources where the public ID starts with the given prefix. </param>
        /// <returns>Parsed result of deletion resources.</returns>
        public DelResResult DeleteResourcesByPrefix(string prefix)
        {
            DelResParams p = new DelResParams() { Prefix = prefix };
            return DeleteResources(p);
        }

        /// <summary>
        /// Deletes all resources, including derived resources, where the public ID starts with the given prefix (up to
        /// a maximum of 1000 original resources) asynchronously.
        /// </summary>
        /// <param name="prefix">Delete all resources where the public ID starts with the given prefix. </param>
        /// <param name="keepOriginal">If true, delete only the derived images of the matching resources.</param>
        /// <param name="nextCursor">Continue deletion from the given cursor.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        public Task<DelResResult> DeleteResourcesByPrefixAsync(string prefix, bool keepOriginal, string nextCursor, CancellationToken? cancellationToken = null)
        {
            var p = new DelResParams()
            {
                Prefix = prefix,
                KeepOriginal = keepOriginal,
                NextCursor = nextCursor,
            };
            return DeleteResourcesAsync(p, cancellationToken);
        }

        /// <summary>
        /// Deletes all resources, including derived resources, where the public ID starts with the given prefix (up to
        /// a maximum of 1000 original resources).
        /// </summary>
        /// <param name="prefix">Delete all resources where the public ID starts with the given prefix. </param>
        /// <param name="keepOriginal">If true, delete only the derived images of the matching resources.</param>
        /// <param name="nextCursor">Continue deletion from the given cursor.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        public DelResResult DeleteResourcesByPrefix(string prefix, bool keepOriginal, string nextCursor)
        {
            DelResParams p = new DelResParams() { Prefix = prefix, KeepOriginal = keepOriginal, NextCursor = nextCursor };
            return DeleteResources(p);
        }

        /// <summary>
        /// Deletes resources by the given tag name asynchronously.
        /// </summary>
        /// <param name="tag">
        /// Delete all resources (and their derivatives) with the given tag name (up to a maximum of
        /// 1000 original resources).
        /// </param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        public Task<DelResResult> DeleteResourcesByTagAsync(string tag, CancellationToken? cancellationToken = null)
        {
            var p = new DelResParams() { Tag = tag };
            return DeleteResourcesAsync(p, cancellationToken);
        }

        /// <summary>
        /// Deletes resources by the given tag name.
        /// </summary>
        /// <param name="tag">
        /// Delete all resources (and their derivatives) with the given tag name (up to a maximum of
        /// 1000 original resources).
        /// </param>
        /// <returns>Parsed result of deletion resources.</returns>
        public DelResResult DeleteResourcesByTag(string tag)
        {
            DelResParams p = new DelResParams() { Tag = tag };
            return DeleteResources(p);
        }

        /// <summary>
        /// Deletes resources by the given tag name asynchronously.
        /// </summary>
        /// <param name="tag">
        /// Delete all resources (and their derivatives) with the given tag name (up to a maximum of
        /// 1000 original resources).
        /// </param>
        /// <param name="keepOriginal">If true, delete only the derived images of the matching resources.</param>
        /// <param name="nextCursor">Continue deletion from the given cursor.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        public Task<DelResResult> DeleteResourcesByTagAsync(string tag, bool keepOriginal, string nextCursor, CancellationToken? cancellationToken = null)
        {
            var p = new DelResParams()
            {
                Tag = tag,
                KeepOriginal = keepOriginal,
                NextCursor = nextCursor,
            };
            return DeleteResourcesAsync(p, cancellationToken);
        }

        /// <summary>
        /// Deletes resources by the given tag name.
        /// </summary>
        /// <param name="tag">
        /// Delete all resources (and their derivatives) with the given tag name (up to a maximum of
        /// 1000 original resources).
        /// </param>
        /// <param name="keepOriginal">If true, delete only the derived images of the matching resources.</param>
        /// <param name="nextCursor">Continue deletion from the given cursor.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        public DelResResult DeleteResourcesByTag(string tag, bool keepOriginal, string nextCursor)
        {
            DelResParams p = new DelResParams() { Tag = tag, KeepOriginal = keepOriginal, NextCursor = nextCursor };
            return DeleteResources(p);
        }

        /// <summary>
        /// Deletes all resources asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        public Task<DelResResult> DeleteAllResourcesAsync(CancellationToken? cancellationToken = null)
        {
            var p = new DelResParams() { All = true };
            return DeleteResourcesAsync(p, cancellationToken);
        }

        /// <summary>
        /// Deletes all resources.
        /// </summary>
        /// <returns>Parsed result of deletion resources.</returns>
        public DelResResult DeleteAllResources()
        {
            DelResParams p = new DelResParams() { All = true };
            return DeleteResources(p);
        }

        /// <summary>
        /// Deletes all resources with conditions asynchronously.
        /// </summary>
        /// <param name="keepOriginal">If true, delete only the derived resources.</param>
        /// <param name="nextCursor">
        /// Value of the <see cref="DelResResult.NextCursor"/> to continue delete from.
        /// </param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        public Task<DelResResult> DeleteAllResourcesAsync(bool keepOriginal, string nextCursor, CancellationToken? cancellationToken = null)
        {
            var p = new DelResParams()
            {
                All = true,
                KeepOriginal = keepOriginal,
                NextCursor = nextCursor,
            };
            return DeleteResourcesAsync(p, cancellationToken);
        }

        /// <summary>
        /// Deletes all resources with conditions.
        /// </summary>
        /// <param name="keepOriginal">If true, delete only the derived resources.</param>
        /// <param name="nextCursor">
        /// Value of the <see cref="DelResResult.NextCursor"/> to continue delete from.
        /// </param>
        /// <returns>Parsed result of deletion resources.</returns>
        public DelResResult DeleteAllResources(bool keepOriginal, string nextCursor)
        {
            DelResParams p = new DelResParams() { All = true, KeepOriginal = keepOriginal, NextCursor = nextCursor };
            return DeleteResources(p);
        }

        /// <summary>
        /// Deletes all resources with parameters asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters for deletion resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        public Task<DelResResult> DeleteResourcesAsync(DelResParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = GetApiUrlV().
                Add("resources").
                Add(ApiShared.GetCloudinaryParam(parameters.ResourceType));

            url = string.IsNullOrEmpty(parameters.Tag)
                ? url.Add(parameters.Type)
                : url.Add("tags").Add(parameters.Tag);

            var urlBuilder = new UrlBuilder(url.BuildUrl(), parameters.ToParamsDictionary());

            return m_api.CallApiAsync<DelResResult>(
                HttpMethod.DELETE,
                urlBuilder.ToString(),
                parameters,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Deletes all resources with parameters.
        /// </summary>
        /// <param name="parameters">Parameters for deletion resources.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        public DelResResult DeleteResources(DelResParams parameters)
        {
            Url url = GetApiUrlV().
                Add("resources").
                Add(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType));

            url = string.IsNullOrEmpty(parameters.Tag)
                ? url.Add(parameters.Type)
                : url.Add("tags").Add(parameters.Tag);

            UrlBuilder urlBuilder = new UrlBuilder(url.BuildUrl(), parameters.ToParamsDictionary());

            return m_api.CallApi<DelResResult>(HttpMethod.DELETE, urlBuilder.ToString(), parameters, null);
        }

        /// <summary>
        /// Restores a deleted resources by array of public ids asynchronously.
        /// </summary>
        /// <param name="publicIds">The public IDs of (deleted or existing) backed up resources to restore.</param>
        /// <returns>Parsed result of restoring resources.</returns>
        public Task<RestoreResult> RestoreAsync(params string[] publicIds)
        {
            var restoreParams = new RestoreParams();
            restoreParams.PublicIds.AddRange(publicIds);

            return RestoreAsync(restoreParams);
        }

        /// <summary>
        /// Restores a deleted resources by array of public ids.
        /// </summary>
        /// <param name="publicIds">The public IDs of (deleted or existing) backed up resources to restore.</param>
        /// <returns>Parsed result of restoring resources.</returns>
        public RestoreResult Restore(params string[] publicIds)
        {
            RestoreParams restoreParams = new RestoreParams();
            restoreParams.PublicIds.AddRange(publicIds);

            return Restore(restoreParams);
        }

        /// <summary>
        /// Restores a deleted resources asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to restore a deleted resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of restoring resources.</returns>
        public Task<RestoreResult> RestoreAsync(RestoreParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = GetApiUrlV().
                ResourceType("resources").
                Add(ApiShared.GetCloudinaryParam(parameters.ResourceType)).
                Add("upload").
                Add("restore").BuildUrl();

            return m_api.CallApiAsync<RestoreResult>(HttpMethod.POST, url, parameters, null, null, cancellationToken);
        }

        /// <summary>
        /// Restores a deleted resources.
        /// </summary>
        /// <param name="parameters">Parameters to restore a deleted resources.</param>
        /// <returns>Parsed result of restoring resources.</returns>
        public RestoreResult Restore(RestoreParams parameters)
        {
            var url = GetApiUrlV().
                ResourceType("resources").
                Add(Api.GetCloudinaryParam(parameters.ResourceType)).
                Add("upload").
                Add("restore").BuildUrl();

            return m_api.CallApi<RestoreResult>(HttpMethod.POST, url, parameters, null);
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

        /// <summary>
        /// Returns list of all upload mappings asynchronously.
        /// </summary>
        /// <param name="parameters">
        /// Uses only <see cref="UploadMappingParams.MaxResults"/> and <see cref="UploadMappingParams.NextCursor"/>
        /// properties. Can be null.
        /// </param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        public Task<UploadMappingResults> UploadMappingsAsync(UploadMappingParams parameters, CancellationToken? cancellationToken = null)
        {
            return CallUploadMappingsApiAsync(HttpMethod.GET, parameters, cancellationToken);
        }

        /// <summary>
        /// Returns list of all upload mappings.
        /// </summary>
        /// <param name="parameters">
        /// Uses only <see cref="UploadMappingParams.MaxResults"/> and <see cref="UploadMappingParams.NextCursor"/>
        /// properties. Can be null.
        /// </param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        public UploadMappingResults UploadMappings(UploadMappingParams parameters)
        {
            return CallUploadMappingsAPI(HttpMethod.GET, parameters);
        }

        /// <summary>
        /// Returns single upload mapping by <see cref="Folder"/> name asynchronously.
        /// </summary>
        /// <param name="folder">Folder name.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        public Task<UploadMappingResults> UploadMappingAsync(string folder, CancellationToken? cancellationToken = null)
        {
            if (string.IsNullOrEmpty(folder))
            {
                throw new ArgumentException("Folder name is required.", nameof(folder));
            }

            var parameters = new UploadMappingParams() { Folder = folder };

            return CallUploadMappingsApiAsync(HttpMethod.GET, parameters, cancellationToken);
        }

        /// <summary>
        /// Returns single upload mapping by <see cref="Folder"/> name.
        /// </summary>
        /// <param name="folder">Folder name.</param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        public UploadMappingResults UploadMapping(string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                throw new ArgumentException("Folder must be specified.");
            }

            var parameters = new UploadMappingParams() { Folder = folder };

            return CallUploadMappingsAPI(HttpMethod.GET, parameters);
        }

        /// <summary>
        /// Creates a new upload mapping folder and its template (URL) asynchronously.
        /// </summary>
        /// <param name="folder">Folder name to create.</param>
        /// <param name="template">URL template for mapping to the <paramref name="folder"/>.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        public Task<UploadMappingResults> CreateUploadMappingAsync(string folder, string template, CancellationToken? cancellationToken = null)
        {
            var parameters = CreateUploadMappingParams(folder, template);
            return CallUploadMappingsApiAsync(HttpMethod.POST, parameters, cancellationToken);
        }

        /// <summary>
        /// Creates a new upload mapping folder and its template (URL).
        /// </summary>
        /// <param name="folder">Folder name to create.</param>
        /// <param name="template">URL template for mapping to the <paramref name="folder"/>.</param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        public UploadMappingResults CreateUploadMapping(string folder, string template)
        {
            var parameters = CreateUploadMappingParams(folder, template);
            return CallUploadMappingsAPI(HttpMethod.POST, parameters);
        }

        /// <summary>
        /// Updates existing upload mapping asynchronously.
        /// </summary>
        /// <param name="folder">Existing Folder to be updated.</param>
        /// <param name="newTemplate">New value of Template URL.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after Upload mappings update.</returns>
        public Task<UploadMappingResults> UpdateUploadMappingAsync(string folder, string newTemplate, CancellationToken? cancellationToken = null)
        {
            var parameters = CreateUploadMappingParams(folder, newTemplate);
            return CallUploadMappingsApiAsync(HttpMethod.PUT, parameters, cancellationToken);
        }

        /// <summary>
        /// Updates existing upload mapping.
        /// </summary>
        /// <param name="folder">Existing Folder to be updated.</param>
        /// <param name="newTemplate">New value of Template URL.</param>
        /// <returns>Parsed response after Upload mappings update.</returns>
        public UploadMappingResults UpdateUploadMapping(string folder, string newTemplate)
        {
            var parameters = CreateUploadMappingParams(folder, newTemplate);
            return CallUploadMappingsAPI(HttpMethod.PUT, parameters);
        }

        private static UploadMappingParams CreateUploadMappingParams(string folder, string template)
        {
            if (string.IsNullOrEmpty(folder))
            {
                throw new ArgumentException("Folder property must be specified.");
            }

            if (string.IsNullOrEmpty(template))
            {
                throw new ArgumentException("Template must be specified.");
            }

            var parameters = new UploadMappingParams()
            {
                Folder = folder,
                Template = template,
            };
            return parameters;
        }

        /// <summary>
        /// Deletes all upload mappings asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after Upload mappings delete.</returns>
        public Task<UploadMappingResults> DeleteUploadMappingAsync(CancellationToken? cancellationToken = null)
        {
            return DeleteUploadMappingAsync(string.Empty, cancellationToken);
        }

        /// <summary>
        /// Deletes all upload mappings.
        /// </summary>
        /// <returns>Parsed response after Upload mappings delete.</returns>
        public UploadMappingResults DeleteUploadMapping()
        {
            return DeleteUploadMapping(string.Empty);
        }

        /// <summary>
        /// Deletes upload mapping by <paramref name="folder"/> name asynchronously.
        /// </summary>
        /// <param name="folder">Folder name.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        public Task<UploadMappingResults> DeleteUploadMappingAsync(string folder, CancellationToken? cancellationToken = null)
        {
            var parameters = new UploadMappingParams { Folder = folder };
            return CallUploadMappingsApiAsync(HttpMethod.DELETE, parameters, cancellationToken);
        }

        /// <summary>
        /// Deletes upload mapping by <paramref name="folder"/> name.
        /// </summary>
        /// <param name="folder">Folder name.</param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        public UploadMappingResults DeleteUploadMapping(string folder)
        {
            var parameters = new UploadMappingParams { Folder = folder };
            return CallUploadMappingsAPI(HttpMethod.DELETE, parameters);
        }

        /// <summary>
        /// Updates Cloudinary transformation resource asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters for transformation update.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        public Task<UpdateTransformResult> UpdateTransformAsync(UpdateTransformParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = GetTransformationUrl(parameters.Transformation);
            return m_api.CallApiAsync<UpdateTransformResult>(HttpMethod.PUT, url, parameters, null, null, cancellationToken);
        }

        /// <summary>
        /// Updates Cloudinary transformation resource.
        /// </summary>
        /// <param name="parameters">Parameters for transformation update.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        public UpdateTransformResult UpdateTransform(UpdateTransformParams parameters)
        {
            var url = GetTransformationUrl(parameters.Transformation);
            return m_api.CallApi<UpdateTransformResult>(HttpMethod.PUT, url, parameters, null);
        }

        /// <summary>
        /// Creates Cloudinary transformation resource asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of the new transformation.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        public Task<TransformResult> CreateTransformAsync(CreateTransformParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = GetTransformationUrl(parameters.Name);
            return m_api.CallApiAsync<TransformResult>(HttpMethod.POST, url, parameters, null, null, cancellationToken);
        }

        private string GetTransformationUrl(string transformationName) =>
            GetApiUrlV().
                ResourceType("transformations").
                Add(transformationName).
                BuildUrl();

        /// <summary>
        /// Creates Cloudinary transformation resource.
        /// </summary>
        /// <param name="parameters">Parameters of the new transformation.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        public TransformResult CreateTransform(CreateTransformParams parameters)
        {
            var url = GetTransformationUrl(parameters.Name);
            return m_api.CallApi<TransformResult>(HttpMethod.POST, url, parameters, null);
        }

        /// <summary>
        /// Deletes transformation by name asynchronously.
        /// </summary>
        /// <param name="transformName">The name of transformation to delete.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        public Task<TransformResult> DeleteTransformAsync(string transformName, CancellationToken? cancellationToken = null)
        {
            var url = GetTransformationUrl(transformName);
            return m_api.CallApiAsync<TransformResult>(
                HttpMethod.DELETE,
                url,
                null,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Deletes transformation by name.
        /// </summary>
        /// <param name="transformName">The name of transformation to delete.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        public TransformResult DeleteTransform(string transformName)
        {
            var url = GetTransformationUrl(transformName);
            return m_api.CallApi<TransformResult>(HttpMethod.DELETE, url, null, null);
        }

        /// <summary>
        /// Eagerly generate sprites asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters for sprite generation.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response with detailed information about the created sprite.</returns>
        public Task<SpriteResult> MakeSpriteAsync(SpriteParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = m_api.ApiUrlImgUpV.
                Action("sprite").
                BuildUrl();

            return m_api.CallApiAsync<SpriteResult>(
                HttpMethod.POST,
                url,
                parameters,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Eagerly generates sprites.
        /// </summary>
        /// <param name="parameters">Parameters for sprite generation.</param>
        /// <returns>Parsed response with detailed information about the created sprite.</returns>
        public SpriteResult MakeSprite(SpriteParams parameters)
        {
            var url = m_api.ApiUrlImgUpV.
                Action("sprite").
                BuildUrl();

            return m_api.CallApi<SpriteResult>(HttpMethod.POST, url, parameters, null);
        }

        /// <summary>
        /// Creates a single animated GIF file from a group of images asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of Multi operation.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response with detailed information about the created animated GIF.</returns>
        public Task<MultiResult> MultiAsync(MultiParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = m_api.ApiUrlImgUpV.
                Action("multi").
                BuildUrl();

            return m_api.CallApiAsync<MultiResult>(
                HttpMethod.POST,
                url,
                parameters,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Creates a single animated GIF file from a group of images.
        /// </summary>
        /// <param name="parameters">Parameters of Multi operation.</param>
        /// <returns>Parsed response with detailed information about the created animated GIF.</returns>
        public MultiResult Multi(MultiParams parameters)
        {
            var url = m_api.ApiUrlImgUpV.
                Action("multi").
                BuildUrl();

            return m_api.CallApi<MultiResult>(HttpMethod.POST, url, parameters, null);
        }

        /// <summary>
        /// Explodes multipage document to single pages asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of explosion operation.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after a call of Explode method.</returns>
        public Task<ExplodeResult> ExplodeAsync(ExplodeParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = m_api.ApiUrlImgUpV.
                Action("explode").
                BuildUrl();

            return m_api.CallApiAsync<ExplodeResult>(
                HttpMethod.POST,
                url,
                parameters,
                null,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Explodes multipage document to single pages.
        /// </summary>
        /// <param name="parameters">Parameters of explosion operation.</param>
        /// <returns>Parsed response after a call of Explode method.</returns>
        public ExplodeResult Explode(ExplodeParams parameters)
        {
            var url = m_api.ApiUrlImgUpV.
                Action("explode").
                BuildUrl();

            return m_api.CallApi<ExplodeResult>(HttpMethod.POST, url, parameters, null);
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
                sb.Append("/");
            }

            sb.Append(script);

            sb.AppendLine("\"></script>");
        }

        private string GetDownloadUrl(UrlBuilder builder, IDictionary<string, object> parameters)
        {
            m_api.FinalizeUploadParameters(parameters);
            builder.SetParameters(parameters);
            return builder.ToString();
        }
    }
}
