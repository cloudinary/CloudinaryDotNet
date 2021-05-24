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

        /// <summary>
        /// Gets the Cloudinary account usage details asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>The report on the status of your Cloudinary account usage details.</returns>
        public Task<UsageResult> GetUsageAsync(CancellationToken? cancellationToken = null)
        {
            string uri = GetUsageUrl(null);

            return m_api.CallApiAsync<UsageResult>(
                HttpMethod.GET,
                uri,
                null,
                null,
                null,
                cancellationToken);
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
