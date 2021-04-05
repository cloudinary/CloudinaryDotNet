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
    public partial class Cloudinary : IAdminApi
    {
        /// <summary>
        /// Resource type 'image'.
        /// </summary>
        protected const string RESOURCE_TYPE_IMAGE = "image";

        /// <summary>
        /// Action 'generate_archive'.
        /// </summary>
        protected const string ACTION_GENERATE_ARCHIVE = "generate_archive";

        /// <summary>
        /// Default chunk (buffer) size for upload large files.
        /// </summary>
        protected const int DEFAULT_CHUNK_SIZE = 20 * 1024 * 1024; // 20 MB

        /// <summary>
        /// Instance of <see cref="Random"/> class.
        /// </summary>
        protected static Random m_random = new Random();

        private Api m_api;

        private Lazy<AdminApi> m_adminApiFactory = new Lazy<AdminApi>(() => new AdminApi());

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
            m_adminApiFactory = new Lazy<AdminApi>(() => new AdminApi(cloudinaryUrl));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cloudinary"/> class with Cloudinary account.
        /// </summary>
        /// <param name="account">Cloudinary account.</param>
        public Cloudinary(Account account)
        {
            m_api = new Api(account);
            m_adminApiFactory = new Lazy<AdminApi>(() => new AdminApi(account));
        }

        /// <summary>
        /// Gets Cloudinary <see cref="IAdminApi"/> object.
        /// </summary>
        public IAdminApi AdminApi => m_adminApiFactory.Value;

        /// <summary>
        /// Gets API object that used by this instance.
        /// </summary>
        internal Api Api => m_api;

        /// <inheritdoc/>
        public void SetApiBaseAddress(string value)
        {
            AdminApi.SetApiBaseAddress(value);
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
        ///  Creates and returns an URL that when invoked creates an archive of a folder.
        /// </summary>
        /// <param name="folderPath">Full path from the root.</param>
        /// <param name="parameters">Optional parameters of generated archive.</param>
        /// <returns>Url for downloading an archive of a folder.</returns>
        public string DownloadFolder(string folderPath, ArchiveParams parameters = null)
        {
            var downloadParameters = parameters ?? new ArchiveParams();

            downloadParameters.Prefixes(new List<string> { folderPath });
            downloadParameters.ResourceType(Constants.RESOURCE_TYPE_ALL);

            return DownloadArchiveUrl(downloadParameters);
        }

        /// <summary>
        /// Creates and returns an URL that allows downloading the backed-up asset
        /// based on the the asset ID and the version ID.
        /// </summary>
        /// <param name="assetId">ID of the asset.</param>
        /// <param name="versionId">Version ID of the asset.</param>
        /// <returns>Url for downloading the backed-up asset.</returns>
        public string DownloadBackedUpAsset(string assetId, string versionId)
        {
            Utils.ShouldNotBeEmpty(() => assetId);
            Utils.ShouldNotBeEmpty(() => versionId);

            var parameters = new SortedDictionary<string, object>
            {
                { "asset_id", assetId },
                { "version_id", versionId },
            };

            var urlBuilder = new UrlBuilder(GetApiUrlV().Action("download_backup").BuildUrl());
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

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateResourceAccessModeByTagAsync method instead.")]
        public Task<UpdateResourceAccessModeResult> UpdateResourceAccessModeByTagAsync(string tag, UpdateResourceAccessModeParams parameters, CancellationToken? cancellationToken = null)
        {
            return AdminApi.UpdateResourceAccessModeByTagAsync(tag, parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateResourceAccessModeByTag method instead.")]
        public UpdateResourceAccessModeResult UpdateResourceAccessModeByTag(string tag, UpdateResourceAccessModeParams parameters)
        {
            return AdminApi.UpdateResourceAccessModeByTag(tag, parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateResourceAccessModeByPrefixAsync method instead.")]
        public Task<UpdateResourceAccessModeResult> UpdateResourceAccessModeByPrefixAsync(string prefix, UpdateResourceAccessModeParams parameters, CancellationToken? cancellationToken = null)
        {
            return AdminApi.UpdateResourceAccessModeByPrefixAsync(prefix, parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateResourceAccessModeByPrefix method instead.")]
        public UpdateResourceAccessModeResult UpdateResourceAccessModeByPrefix(string prefix, UpdateResourceAccessModeParams parameters)
        {
            return AdminApi.UpdateResourceAccessModeByPrefix(prefix, parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateResourceAccessModeByIdsAsync method instead.")]
        public Task<UpdateResourceAccessModeResult> UpdateResourceAccessModeByIdsAsync(UpdateResourceAccessModeParams parameters, CancellationToken? cancellationToken = null)
        {
            return AdminApi.UpdateResourceAccessModeByIdsAsync(parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateResourceAccessModeByIds method instead.")]
        public UpdateResourceAccessModeResult UpdateResourceAccessModeByIds(UpdateResourceAccessModeParams parameters)
        {
            return AdminApi.UpdateResourceAccessModeByIds(parameters);
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

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.CreateUploadPresetAsync method instead.")]
        public Task<UploadPresetResult> CreateUploadPresetAsync(UploadPresetParams parameters, CancellationToken? cancellationToken = null)
        {
            return AdminApi.CreateUploadPresetAsync(parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.CreateUploadPreset method instead.")]
        public UploadPresetResult CreateUploadPreset(UploadPresetParams parameters)
        {
            return AdminApi.CreateUploadPreset(parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateUploadPresetAsync method instead.")]
        public Task<UploadPresetResult> UpdateUploadPresetAsync(UploadPresetParams parameters, CancellationToken? cancellationToken = null)
        {
            return AdminApi.UpdateUploadPresetAsync(parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateUploadPreset method instead.")]
        public UploadPresetResult UpdateUploadPreset(UploadPresetParams parameters)
        {
            return AdminApi.UpdateUploadPreset(parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.GetUploadPresetAsync method instead.")]
        public Task<GetUploadPresetResult> GetUploadPresetAsync(string name, CancellationToken? cancellationToken = null)
        {
            return AdminApi.GetUploadPresetAsync(name, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.GetUploadPreset method instead.")]
        public GetUploadPresetResult GetUploadPreset(string name)
        {
            return AdminApi.GetUploadPreset(name);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListUploadPresetsAsync method instead.")]
        public Task<ListUploadPresetsResult> ListUploadPresetsAsync(string nextCursor = null, CancellationToken? cancellationToken = null)
        {
            return AdminApi.ListUploadPresetsAsync(nextCursor, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListUploadPresets method instead.")]
        public ListUploadPresetsResult ListUploadPresets(string nextCursor = null)
        {
            return AdminApi.ListUploadPresets(nextCursor);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListUploadPresetsAsync method instead.")]
        public Task<ListUploadPresetsResult> ListUploadPresetsAsync(ListUploadPresetsParams parameters, CancellationToken? cancellationToken = null)
        {
            return AdminApi.ListUploadPresetsAsync(parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListUploadPresets method instead.")]
        public ListUploadPresetsResult ListUploadPresets(ListUploadPresetsParams parameters)
        {
            return AdminApi.ListUploadPresets(parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteUploadPresetAsync method instead.")]
        public Task<DeleteUploadPresetResult> DeleteUploadPresetAsync(string name, CancellationToken? cancellationToken = null)
        {
            return AdminApi.DeleteUploadPresetAsync(name, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteUploadPreset method instead.")]
        public DeleteUploadPresetResult DeleteUploadPreset(string name)
        {
            return AdminApi.DeleteUploadPreset(name);
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

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.RootFoldersAsync method instead.")]
        public Task<GetFoldersResult> RootFoldersAsync(GetFoldersParams parameters = null, CancellationToken? cancellationToken = null)
        {
            return AdminApi.RootFoldersAsync(parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.RootFolders method instead.")]
        public GetFoldersResult RootFolders(GetFoldersParams parameters = null)
        {
            return AdminApi.RootFolders(parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.SubFoldersAsync method instead.")]
        public Task<GetFoldersResult> SubFoldersAsync(string folder, CancellationToken? cancellationToken = null)
        {
            return AdminApi.SubFoldersAsync(folder, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.SubFoldersAsync method instead.")]
        public Task<GetFoldersResult> SubFoldersAsync(string folder, GetFoldersParams parameters, CancellationToken? cancellationToken = null)
        {
            return AdminApi.SubFoldersAsync(folder, parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.SubFolders method instead.")]
        public GetFoldersResult SubFolders(string folder, GetFoldersParams parameters = null)
        {
            return AdminApi.SubFolders(folder, parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteFolderAsync method instead.")]
        public Task<DeleteFolderResult> DeleteFolderAsync(string folder, CancellationToken? cancellationToken = null)
        {
            return AdminApi.DeleteFolderAsync(folder, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteFolder method instead.")]
        public DeleteFolderResult DeleteFolder(string folder)
        {
            return AdminApi.DeleteFolder(folder);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.CreateFolder method instead.")]
        public CreateFolderResult CreateFolder(string folder)
        {
            return AdminApi.CreateFolder(folder);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.CreateFolderAsync method instead.")]
        public Task<CreateFolderResult> CreateFolderAsync(string folder, CancellationToken? cancellationToken = null)
        {
            return AdminApi.CreateFolderAsync(folder, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.GetUsageAsync method instead.")]
        public Task<UsageResult> GetUsageAsync(CancellationToken? cancellationToken = null)
        {
            return AdminApi.GetUsageAsync(cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.GetUsageAsync method instead.")]
        public Task<UsageResult> GetUsageAsync(DateTime? date, CancellationToken? cancellationToken = null)
        {
            return AdminApi.GetUsageAsync(date, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.GetUsage method instead.")]
        public UsageResult GetUsage(DateTime? date = null)
        {
            return AdminApi.GetUsage(date);
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
                return await UploadAsync<T>(parameters).ConfigureAwait(false);
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
                    cancellationToken).ConfigureAwait(false);
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

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourceTypesAsync method instead.")]
        public Task<ListResourceTypesResult> ListResourceTypesAsync(CancellationToken? cancellationToken = null)
        {
            return AdminApi.ListResourceTypesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourceTypes method instead.")]
        public ListResourceTypesResult ListResourceTypes()
        {
            return AdminApi.ListResourceTypes();
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourcesAsync method instead.")]
        public Task<ListResourcesResult> ListResourcesAsync(
            string nextCursor = null,
            bool tags = true,
            bool context = true,
            bool moderations = true,
            CancellationToken? cancellationToken = null)
        {
            return AdminApi.ListResourcesAsync(nextCursor, tags, context, moderations, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResources method instead.")]
        public ListResourcesResult ListResources(
            string nextCursor = null,
            bool tags = true,
            bool context = true,
            bool moderations = true)
        {
            return AdminApi.ListResources(nextCursor, tags, context, moderations);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourcesByTypeAsync method instead.")]
        public Task<ListResourcesResult> ListResourcesByTypeAsync(string type, string nextCursor = null, CancellationToken? cancellationToken = null)
        {
            return AdminApi.ListResourcesByTypeAsync(type, nextCursor, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourcesByType method instead.")]
        public ListResourcesResult ListResourcesByType(string type, string nextCursor = null)
        {
            return AdminApi.ListResourcesByType(type, nextCursor);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourcesByPrefixAsync method instead.")]
        public Task<ListResourcesResult> ListResourcesByPrefixAsync(
            string prefix,
            string type = "upload",
            string nextCursor = null,
            CancellationToken? cancellationToken = null)
        {
            return AdminApi.ListResourcesByPrefixAsync(prefix, type, nextCursor, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourcesByPrefix method instead.")]
        public ListResourcesResult ListResourcesByPrefix(string prefix, string type = "upload", string nextCursor = null)
        {
            return AdminApi.ListResourcesByPrefix(prefix, type, nextCursor);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourcesByPrefixAsync method instead.")]
        public Task<ListResourcesResult> ListResourcesByPrefixAsync(
            string prefix,
            bool tags,
            bool context,
            bool moderations,
            string type = "upload",
            string nextCursor = null,
            CancellationToken? cancellationToken = null)
        {
            return AdminApi.ListResourcesByPrefixAsync(prefix, tags, context, moderations, type, nextCursor, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourcesByPrefix method instead.")]
        public ListResourcesResult ListResourcesByPrefix(string prefix, bool tags, bool context, bool moderations, string type = "upload", string nextCursor = null)
        {
            return AdminApi.ListResourcesByPrefix(prefix, tags, context, moderations, type, nextCursor);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourcesByTagAsync method instead.")]
        public Task<ListResourcesResult> ListResourcesByTagAsync(string tag, string nextCursor = null, CancellationToken? cancellationToken = null)
        {
            return AdminApi.ListResourcesByTagAsync(tag, nextCursor, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourcesByTag method instead.")]
        public ListResourcesResult ListResourcesByTag(string tag, string nextCursor = null)
        {
            return AdminApi.ListResourcesByTag(tag, nextCursor);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourcesByPublicIdsAsync method instead.")]
        public Task<ListResourcesResult> ListResourcesByPublicIdsAsync(IEnumerable<string> publicIds, CancellationToken? cancellationToken = null)
        {
            return AdminApi.ListResourcesByPublicIdsAsync(publicIds, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourcesByPublicIds method instead.")]
        public ListResourcesResult ListResourcesByPublicIds(IEnumerable<string> publicIds)
        {
            return AdminApi.ListResourcesByPublicIds(publicIds);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourceByPublicIdsAsync method instead.")]
        public Task<ListResourcesResult> ListResourceByPublicIdsAsync(
            IEnumerable<string> publicIds,
            bool tags,
            bool context,
            bool moderations,
            CancellationToken? cancellationToken = null)
        {
            return AdminApi.ListResourceByPublicIdsAsync(publicIds, tags, context, moderations, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourceByPublicIds method instead.")]
        public ListResourcesResult ListResourceByPublicIds(IEnumerable<string> publicIds, bool tags, bool context, bool moderations)
        {
            return AdminApi.ListResourceByPublicIds(publicIds, tags, context, moderations);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourcesByModerationStatusAsync method instead.")]
        public Task<ListResourcesResult> ListResourcesByModerationStatusAsync(
            string kind,
            ModerationStatus status,
            bool tags = true,
            bool context = true,
            bool moderations = true,
            string nextCursor = null,
            CancellationToken? cancellationToken = null)
        {
            return AdminApi.ListResourcesByModerationStatusAsync(kind, status, tags, context, moderations, nextCursor, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourcesByModerationStatus method instead.")]
        public ListResourcesResult ListResourcesByModerationStatus(
            string kind,
            ModerationStatus status,
            bool tags = true,
            bool context = true,
            bool moderations = true,
            string nextCursor = null)
        {
            return AdminApi.ListResourcesByModerationStatus(kind, status, tags, context, moderations, nextCursor);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourcesByContextAsync method instead.")]
        public Task<ListResourcesResult> ListResourcesByContextAsync(
            string key,
            string value = "",
            bool tags = false,
            bool context = false,
            string nextCursor = null,
            CancellationToken? cancellationToken = null)
        {
            return AdminApi.ListResourcesByContextAsync(key, value, tags, context, nextCursor, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourcesByContext method instead.")]
        public ListResourcesResult ListResourcesByContext(string key, string value = "", bool tags = false, bool context = false, string nextCursor = null)
        {
            return AdminApi.ListResourcesByContext(key, value, tags, context, nextCursor);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResourcesAsync method instead.")]
        public Task<ListResourcesResult> ListResourcesAsync(ListResourcesParams parameters, CancellationToken? cancellationToken = null)
        {
            return AdminApi.ListResourcesAsync(parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListResources method instead.")]
        public ListResourcesResult ListResources(ListResourcesParams parameters)
        {
            return AdminApi.ListResources(parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListTagsAsync method instead.")]
        public Task<ListTagsResult> ListTagsAsync(CancellationToken? cancellationToken = null)
        {
            return AdminApi.ListTagsAsync(cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListTags method instead.")]
        public ListTagsResult ListTags()
        {
            return AdminApi.ListTags();
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListTagsByPrefixAsync method instead.")]
        public Task<ListTagsResult> ListTagsByPrefixAsync(string prefix, CancellationToken? cancellationToken = null)
        {
            return AdminApi.ListTagsByPrefixAsync(prefix, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListTagsByPrefix method instead.")]
        public ListTagsResult ListTagsByPrefix(string prefix)
        {
            return AdminApi.ListTagsByPrefix(prefix);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListTagsAsync method instead.")]
        public Task<ListTagsResult> ListTagsAsync(ListTagsParams parameters, CancellationToken? cancellationToken = null)
        {
            return AdminApi.ListTagsAsync(parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListTags method instead.")]
        public ListTagsResult ListTags(ListTagsParams parameters)
        {
            return AdminApi.ListTags(parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListTransformationsAsync method instead.")]
        public Task<ListTransformsResult> ListTransformationsAsync(CancellationToken? cancellationToken = null)
        {
            return AdminApi.ListTransformationsAsync(cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListTransformations method instead.")]
        public ListTransformsResult ListTransformations()
        {
            return AdminApi.ListTransformations();
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListTransformationsAsync method instead.")]
        public Task<ListTransformsResult> ListTransformationsAsync(ListTransformsParams parameters, CancellationToken? cancellationToken = null)
        {
            return AdminApi.ListTransformationsAsync(parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListTransformations method instead.")]
        public ListTransformsResult ListTransformations(ListTransformsParams parameters)
        {
            return AdminApi.ListTransformations(parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.GetTransformAsync method instead.")]
        public Task<GetTransformResult> GetTransformAsync(string transform, CancellationToken? cancellationToken = null)
        {
            return AdminApi.GetTransformAsync(transform, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.GetTransform method instead.")]
        public GetTransformResult GetTransform(string transform)
        {
            return AdminApi.GetTransform(transform);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.GetTransformAsync method instead.")]
        public Task<GetTransformResult> GetTransformAsync(GetTransformParams parameters, CancellationToken? cancellationToken = null)
        {
            return AdminApi.GetTransformAsync(parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.GetTransform method instead.")]
        public GetTransformResult GetTransform(GetTransformParams parameters)
        {
            return AdminApi.GetTransform(parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateResourceAsync method instead.")]
        public Task<GetResourceResult> UpdateResourceAsync(string publicId, ModerationStatus moderationStatus, CancellationToken? cancellationToken = null)
        {
            return AdminApi.UpdateResourceAsync(publicId, moderationStatus, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateResource method instead.")]
        public GetResourceResult UpdateResource(string publicId, ModerationStatus moderationStatus)
        {
            return AdminApi.UpdateResource(publicId, moderationStatus);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateResourceAsync method instead.")]
        public Task<GetResourceResult> UpdateResourceAsync(UpdateParams parameters, CancellationToken? cancellationToken = null)
        {
            return AdminApi.UpdateResourceAsync(parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateResource method instead.")]
        public GetResourceResult UpdateResource(UpdateParams parameters)
        {
            return AdminApi.UpdateResource(parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.GetResourceAsync method instead.")]
        public Task<GetResourceResult> GetResourceAsync(string publicId, CancellationToken? cancellationToken = null)
        {
            return AdminApi.GetResourceAsync(publicId, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.GetResource method instead.")]
        public GetResourceResult GetResource(string publicId)
        {
            return AdminApi.GetResource(publicId);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.GetResourceAsync method instead.")]
        public Task<GetResourceResult> GetResourceAsync(GetResourceParams parameters, CancellationToken? cancellationToken = null)
        {
            return AdminApi.GetResourceAsync(parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.GetResource method instead.")]
        public GetResourceResult GetResource(GetResourceParams parameters)
        {
            return AdminApi.GetResource(parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteDerivedResourcesAsync method instead.")]
        public Task<DelDerivedResResult> DeleteDerivedResourcesAsync(params string[] ids)
        {
            return AdminApi.DeleteDerivedResourcesAsync(ids);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteDerivedResources method instead.")]
        public DelDerivedResResult DeleteDerivedResources(params string[] ids)
        {
            return AdminApi.DeleteDerivedResources(ids);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteDerivedResourcesAsync method instead.")]
        public Task<DelDerivedResResult> DeleteDerivedResourcesAsync(DelDerivedResParams parameters, CancellationToken? cancellationToken = null)
        {
            return AdminApi.DeleteDerivedResourcesAsync(parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteDerivedResources method instead.")]
        public DelDerivedResResult DeleteDerivedResources(DelDerivedResParams parameters)
        {
            return AdminApi.DeleteDerivedResources(parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteResourcesAsync method instead.")]
        public Task<DelResResult> DeleteResourcesAsync(ResourceType type, params string[] publicIds)
        {
            return AdminApi.DeleteResourcesAsync(type, publicIds);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteResources method instead.")]
        public DelResResult DeleteResources(ResourceType type, params string[] publicIds)
        {
            return AdminApi.DeleteResources(type, publicIds);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteResourcesAsync method instead.")]
        public Task<DelResResult> DeleteResourcesAsync(params string[] publicIds)
        {
            return AdminApi.DeleteResourcesAsync(publicIds);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteResources method instead.")]
        public DelResResult DeleteResources(params string[] publicIds)
        {
            return AdminApi.DeleteResources(publicIds);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteResourcesByPrefixAsync method instead.")]
        public Task<DelResResult> DeleteResourcesByPrefixAsync(string prefix, CancellationToken? cancellationToken = null)
        {
            return AdminApi.DeleteResourcesByPrefixAsync(prefix, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteResourcesByPrefix method instead.")]
        public DelResResult DeleteResourcesByPrefix(string prefix)
        {
            return AdminApi.DeleteResourcesByPrefix(prefix);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteResourcesByPrefixAsync method instead.")]
        public Task<DelResResult> DeleteResourcesByPrefixAsync(string prefix, bool keepOriginal, string nextCursor, CancellationToken? cancellationToken = null)
        {
            return AdminApi.DeleteResourcesByPrefixAsync(prefix, keepOriginal, nextCursor, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteResourcesByPrefix method instead.")]
        public DelResResult DeleteResourcesByPrefix(string prefix, bool keepOriginal, string nextCursor)
        {
            return AdminApi.DeleteResourcesByPrefix(prefix, keepOriginal, nextCursor);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteResourcesByTagAsync method instead.")]
        public Task<DelResResult> DeleteResourcesByTagAsync(string tag, CancellationToken? cancellationToken = null)
        {
            return AdminApi.DeleteResourcesByTagAsync(tag, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteResourcesByTag method instead.")]
        public DelResResult DeleteResourcesByTag(string tag)
        {
            return AdminApi.DeleteResourcesByTag(tag);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteResourcesByTagAsync method instead.")]
        public Task<DelResResult> DeleteResourcesByTagAsync(string tag, bool keepOriginal, string nextCursor, CancellationToken? cancellationToken = null)
        {
            return AdminApi.DeleteResourcesByTagAsync(tag, keepOriginal, nextCursor, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteResourcesByTag method instead.")]
        public DelResResult DeleteResourcesByTag(string tag, bool keepOriginal, string nextCursor)
        {
            return AdminApi.DeleteResourcesByTag(tag, keepOriginal, nextCursor);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteAllResourcesAsync method instead.")]
        public Task<DelResResult> DeleteAllResourcesAsync(CancellationToken? cancellationToken = null)
        {
            return AdminApi.DeleteAllResourcesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteAllResources method instead.")]
        public DelResResult DeleteAllResources()
        {
            return AdminApi.DeleteAllResources();
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteAllResourcesAsync method instead.")]
        public Task<DelResResult> DeleteAllResourcesAsync(bool keepOriginal, string nextCursor, CancellationToken? cancellationToken = null)
        {
            return AdminApi.DeleteAllResourcesAsync(keepOriginal, nextCursor, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteAllResources method instead.")]
        public DelResResult DeleteAllResources(bool keepOriginal, string nextCursor)
        {
            return AdminApi.DeleteAllResources(keepOriginal, nextCursor);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteResourcesAsync method instead.")]
        public Task<DelResResult> DeleteResourcesAsync(DelResParams parameters, CancellationToken? cancellationToken = null)
        {
            return AdminApi.DeleteResourcesAsync(parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteResources method instead.")]
        public DelResResult DeleteResources(DelResParams parameters)
        {
            return AdminApi.DeleteResources(parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.RestoreAsync method instead.")]
        public Task<RestoreResult> RestoreAsync(params string[] publicIds)
        {
            return AdminApi.RestoreAsync(publicIds);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.Restore method instead.")]
        public RestoreResult Restore(params string[] publicIds)
        {
            return AdminApi.Restore(publicIds);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.RestoreAsync method instead.")]
        public Task<RestoreResult> RestoreAsync(RestoreParams parameters, CancellationToken? cancellationToken = null)
        {
            return AdminApi.RestoreAsync(parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.Restore method instead.")]
        public RestoreResult Restore(RestoreParams parameters)
        {
            return AdminApi.Restore(parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UploadMappingsAsync method instead.")]
        public Task<UploadMappingResults> UploadMappingsAsync(UploadMappingParams parameters, CancellationToken? cancellationToken = null)
        {
            return AdminApi.UploadMappingsAsync(parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UploadMappings method instead.")]
        public UploadMappingResults UploadMappings(UploadMappingParams parameters)
        {
            return AdminApi.UploadMappings(parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UploadMappingAsync method instead.")]
        public Task<UploadMappingResults> UploadMappingAsync(string folder, CancellationToken? cancellationToken = null)
        {
            return AdminApi.UploadMappingAsync(folder, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UploadMapping method instead.")]
        public UploadMappingResults UploadMapping(string folder)
        {
            return AdminApi.UploadMapping(folder);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.CreateUploadMappingAsync method instead.")]
        public Task<UploadMappingResults> CreateUploadMappingAsync(string folder, string template, CancellationToken? cancellationToken = null)
        {
            return AdminApi.CreateUploadMappingAsync(folder, template, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.CreateUploadMapping method instead.")]
        public UploadMappingResults CreateUploadMapping(string folder, string template)
        {
            return AdminApi.CreateUploadMapping(folder, template);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateUploadMappingAsync method instead.")]
        public Task<UploadMappingResults> UpdateUploadMappingAsync(string folder, string newTemplate, CancellationToken? cancellationToken = null)
        {
            return AdminApi.UpdateUploadMappingAsync(folder, newTemplate, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateUploadMapping method instead.")]
        public UploadMappingResults UpdateUploadMapping(string folder, string newTemplate)
        {
            return AdminApi.UpdateUploadMapping(folder, newTemplate);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteUploadMappingAsync method instead.")]
        public Task<UploadMappingResults> DeleteUploadMappingAsync(CancellationToken? cancellationToken = null)
        {
            return AdminApi.DeleteUploadMappingAsync(cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteUploadMapping method instead.")]
        public UploadMappingResults DeleteUploadMapping()
        {
            return AdminApi.DeleteUploadMapping();
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteUploadMappingAsync method instead.")]
        public Task<UploadMappingResults> DeleteUploadMappingAsync(string folder, CancellationToken? cancellationToken = null)
        {
            return AdminApi.DeleteUploadMappingAsync(folder, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteUploadMapping method instead.")]
        public UploadMappingResults DeleteUploadMapping(string folder)
        {
            return AdminApi.DeleteUploadMapping(folder);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateTransformAsync method instead.")]
        public Task<UpdateTransformResult> UpdateTransformAsync(UpdateTransformParams parameters, CancellationToken? cancellationToken = null)
        {
            return AdminApi.UpdateTransformAsync(parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateTransform method instead.")]
        public UpdateTransformResult UpdateTransform(UpdateTransformParams parameters)
        {
            return AdminApi.UpdateTransform(parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.CreateTransformAsync method instead.")]
        public Task<TransformResult> CreateTransformAsync(CreateTransformParams parameters, CancellationToken? cancellationToken = null)
        {
            return AdminApi.CreateTransformAsync(parameters, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.CreateTransform method instead.")]
        public TransformResult CreateTransform(CreateTransformParams parameters)
        {
            return AdminApi.CreateTransform(parameters);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteTransformAsync method instead.")]
        public Task<TransformResult> DeleteTransformAsync(string transformName, CancellationToken? cancellationToken = null)
        {
            return AdminApi.DeleteTransformAsync(transformName, cancellationToken);
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteTransform method instead.")]
        public TransformResult DeleteTransform(string transformName)
        {
            return AdminApi.DeleteTransform(transformName);
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
                Action(Constants.ACTION_NAME_SPRITE).
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
                Action(Constants.ACTION_NAME_SPRITE).
                BuildUrl();

            return m_api.CallApi<SpriteResult>(HttpMethod.POST, url, parameters, null);
        }

        /// <summary>
        /// Gets a signed URL to download generated sprite.
        /// </summary>
        /// <param name="parameters">Parameters of Sprite operation.</param>
        /// <returns>Download URL.</returns>
        public string DownloadSprite(SpriteParams parameters)
        {
            parameters.Mode = ArchiveCallMode.Download;
            var urlBuilder = new UrlBuilder(
                m_api.ApiUrlImgUpV.
                    Action(Constants.ACTION_NAME_SPRITE).
                    BuildUrl());

            return GetDownloadUrl(urlBuilder, parameters.ToParamsDictionary());
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
                Action(Constants.ACTION_NAME_MULTI).
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
                Action(Constants.ACTION_NAME_MULTI).
                BuildUrl();

            return m_api.CallApi<MultiResult>(HttpMethod.POST, url, parameters, null);
        }

        /// <summary>
        /// Gets a signed URL to download animated GIF file generated through multi request.
        /// </summary>
        /// <param name="parameters">Parameters of Multi operation.</param>
        /// <returns>Download URL.</returns>
        public string DownloadMulti(MultiParams parameters)
        {
            parameters.Mode = ArchiveCallMode.Download;
            var urlBuilder = new UrlBuilder(
                    m_api.ApiUrlImgUpV.
                    Action(Constants.ACTION_NAME_MULTI).
                    BuildUrl());

            return GetDownloadUrl(urlBuilder, parameters.ToParamsDictionary());
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

        private static SortedDictionary<string, object> NormalizeParameters(IDictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                return new SortedDictionary<string, object>();
            }

            return parameters as SortedDictionary<string, object> ?? new SortedDictionary<string, object>(parameters);
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

        private string GetUploadUrl(string resourceType)
        {
            return GetApiUrlV().Action(Constants.ACTION_NAME_UPLOAD).ResourceType(resourceType).BuildUrl();
        }

        private string GetRenameUrl(RenameParams parameters) =>
            m_api
                .ApiUrlImgUpV
                .ResourceType(ApiShared.GetCloudinaryParam(parameters.ResourceType))
                .Action("rename")
                .BuildUrl();

        private string GetDownloadUrl(UrlBuilder builder, IDictionary<string, object> parameters)
        {
            m_api.FinalizeUploadParameters(parameters);
            builder.SetParameters(parameters);
            return builder.ToString();
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
            private static string GetUploadUrl(BasicRawUploadParams parameters, Api mApi)
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
    }
}
