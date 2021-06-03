namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;

    /// <summary>
    /// Main class of Cloudinary .NET API.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    public partial class Cloudinary
    {
        /// <summary>
        /// Action 'generate_archive'.
        /// </summary>
        protected const string ACTION_GENERATE_ARCHIVE = "generate_archive";

        /// <summary>
        /// Default chunk (buffer) size for upload large files.
        /// </summary>
        protected const int DEFAULT_CHUNK_SIZE = 20 * 1024 * 1024; // 20 MB

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

            return CallUploadApiAsync(uri, dict, cancellationToken, fileDescription);
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
            return UploadAsync(resourceType, parameters, fileDescription).GetAwaiter().GetResult();
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

            return CallUploadApiAsync<RawUploadResult>(
                HttpMethod.POST,
                uri,
                parameters,
                cancellationToken,
                parameters.File);
        }

        /// <summary>
        /// Uploads a file to Cloudinary.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="type">The type ("raw" or "auto", last by default).</param>
        /// <returns>Parsed result of the raw file uploading.</returns>
        public RawUploadResult Upload(RawUploadParams parameters, string type = "auto")
        {
            return UploadAsync(parameters, type, null).GetAwaiter().GetResult();
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
                result = await CallUploadApiAsync<T>(
                    HttpMethod.POST,
                    internalParams.Url,
                    parameters,
                    cancellationToken,
                    parameters.File,
                    internalParams.Headers).ConfigureAwait(false);
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
            return UploadLargeAsync<T>(parameters, bufferSize).GetAwaiter().GetResult();
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
            return CallUploadApiAsync<RenameResult>(
                HttpMethod.POST,
                uri,
                parameters,
                cancellationToken);
        }

        /// <summary>
        /// Changes public identifier of a file.
        /// </summary>
        /// <param name="parameters">Operation parameters.</param>
        /// <returns>Result of resource renaming.</returns>
        public RenameResult Rename(RenameParams parameters)
        {
            return RenameAsync(parameters).GetAwaiter().GetResult();
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

            return CallUploadApiAsync<DeletionResult>(HttpMethod.POST, uri, parameters, null);
        }

        /// <summary>
        /// Deletes file from Cloudinary.
        /// </summary>
        /// <param name="parameters">Parameters for deletion of resource from Cloudinary.</param>
        /// <returns>Results of deletion.</returns>
        public DeletionResult Destroy(DeletionParams parameters)
        {
            return DestroyAsync(parameters).GetAwaiter().GetResult();
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

            return CallUploadApiAsync<TagResult>(HttpMethod.POST, uri, parameters, cancellationToken);
        }

        /// <summary>
        /// Manages tag assignments.
        /// </summary>
        /// <param name="parameters">Parameters of tag management.</param>
        /// <returns>Results of tags management.</returns>
        public TagResult Tag(TagParams parameters)
        {
            return TagAsync(parameters).GetAwaiter().GetResult();
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

            return CallUploadApiAsync<ContextResult>(
                HttpMethod.POST,
                uri,
                parameters,
                cancellationToken);
        }

        /// <summary>
        /// Manages context assignments.
        /// </summary>
        /// <param name="parameters">Parameters of context management.</param>
        /// <returns>Results of contexts management.</returns>
        public ContextResult Context(ContextParams parameters)
        {
            return ContextAsync(parameters).GetAwaiter().GetResult();
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

            return CallUploadApiAsync<ExplicitResult>(
                HttpMethod.POST,
                uri,
                parameters,
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
            return ExplicitAsync(parameters).GetAwaiter().GetResult();
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

            return CallUploadApiAsync<ExplodeResult>(
                HttpMethod.POST,
                url,
                parameters,
                cancellationToken);
        }

        /// <summary>
        /// Explodes multipage document to single pages.
        /// </summary>
        /// <param name="parameters">Parameters of explosion operation.</param>
        /// <returns>Parsed response after a call of Explode method.</returns>
        public ExplodeResult Explode(ExplodeParams parameters)
        {
            return ExplodeAsync(parameters).GetAwaiter().GetResult();
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
            return CallUploadApiAsync<ArchiveResult>(
                HttpMethod.POST,
                url.BuildUrl(),
                parameters,
                cancellationToken);
        }

        /// <summary>
        /// Creates archive and stores it as a raw resource in your Cloudinary account.
        /// </summary>
        /// <param name="parameters">Parameters of new generated archive.</param>
        /// <returns>Parsed result of creating the archive.</returns>
        public ArchiveResult CreateArchive(ArchiveParams parameters)
        {
            return CreateArchiveAsync(parameters).GetAwaiter().GetResult();
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

            return CallUploadApiAsync<SpriteResult>(
                HttpMethod.POST,
                url,
                parameters,
                cancellationToken);
        }

        /// <summary>
        /// Eagerly generates sprites.
        /// </summary>
        /// <param name="parameters">Parameters for sprite generation.</param>
        /// <returns>Parsed response with detailed information about the created sprite.</returns>
        public SpriteResult MakeSprite(SpriteParams parameters)
        {
            return MakeSpriteAsync(parameters).GetAwaiter().GetResult();
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

            return CallUploadApiAsync<MultiResult>(
                HttpMethod.POST,
                url,
                parameters,
                cancellationToken);
        }

        /// <summary>
        /// Creates a single animated GIF file from a group of images.
        /// </summary>
        /// <param name="parameters">Parameters of Multi operation.</param>
        /// <returns>Parsed response with detailed information about the created animated GIF.</returns>
        public MultiResult Multi(MultiParams parameters)
        {
            return MultiAsync(parameters).GetAwaiter().GetResult();
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

            return CallUploadApiAsync<TextResult>(
                HttpMethod.POST,
                uri,
                parameters,
                cancellationToken);
        }

        /// <summary>
        /// Generates an image of a given textual string.
        /// </summary>
        /// <param name="parameters">Parameters of generating an image of a given textual string.</param>
        /// <returns>Results of generating an image of a given textual string.</returns>
        public TextResult Text(TextParams parameters)
        {
            return TextAsync(parameters).GetAwaiter().GetResult();
        }

        private static SortedDictionary<string, object> NormalizeParameters(IDictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                return new SortedDictionary<string, object>();
            }

            return parameters as SortedDictionary<string, object> ?? new SortedDictionary<string, object>(parameters);
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

        private static void UpdateContentRange(UploadLargeParams internalParams)
        {
            var fileDescription = internalParams.Parameters.File;
            var fileLength = fileDescription.GetFileLength();
            var startOffset = fileDescription.BytesSent;
            var endOffset = startOffset + Math.Min(internalParams.BufferSize, fileLength - startOffset) - 1;

            internalParams.Headers["Content-Range"] = $"bytes {startOffset}-{endOffset}/{fileLength}";
        }

        private Task<T> CallUploadApiAsync<T>(
            HttpMethod httpMethod,
            string url,
            BaseParams parameters,
            CancellationToken? cancellationToken,
            FileDescription fileDescription = null,
            Dictionary<string, string> extraHeaders = null)
            where T : BaseResult, new()
        {
            return m_api.CallApiAsync<T>(
                            httpMethod,
                            url,
                            parameters,
                            fileDescription,
                            extraHeaders,
                            cancellationToken);
        }

        private Task<RawUploadResult> CallUploadApiAsync(
            string url,
            SortedDictionary<string, object> parameters,
            CancellationToken? cancellationToken,
            FileDescription fileDescription = null)
        {
            return m_api.CallAndParseAsync<RawUploadResult>(
                HttpMethod.POST,
                url,
                parameters,
                fileDescription,
                null,
                cancellationToken);
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
        /// Uploads a resource to Cloudinary.
        /// </summary>
        /// <param name="parameters">Parameters of uploading .</param>
        /// <returns>Results of uploading.</returns>
        private T Upload<T, TP>(TP parameters)
            where T : UploadResult, new()
            where TP : BasicRawUploadParams, new()
        {
            return UploadAsync<T>(parameters, null).GetAwaiter().GetResult();
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

            return CallUploadApiAsync<T>(
                HttpMethod.POST,
                uri,
                parameters,
                cancellationToken,
                parameters.File,
                null);
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
            /// Initializes a new instance of the <see cref="CloudinaryDotNet.Cloudinary.UploadLargeParams"/> class.
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
