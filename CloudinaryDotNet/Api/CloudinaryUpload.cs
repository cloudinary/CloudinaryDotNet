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

    /// <inheritdoc />
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    public class CloudinaryUpload : ICloudinaryUpload
    {
        /// <summary>
        /// Resource type 'image'.
        /// </summary>
        internal const string RESOURCE_TYPE_IMAGE = "image";

        /// <summary>
        /// Default chunk (buffer) size for upload large files.
        /// </summary>
        internal const int DEFAULT_CHUNK_SIZE = 20 * 1024 * 1024; // 20 MB

        /// <summary>
        /// Cloudinary <see cref="CloudinaryDotNet.Api"/> object.
        /// </summary>
        protected internal Api Api;

        /// <summary>
        /// Action 'generate_archive'.
        /// </summary>
        protected const string ACTION_GENERATE_ARCHIVE = "generate_archive";

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudinaryUpload"/> class.
        /// Default parameterless constructor. Assumes that environment variable CLOUDINARY_URL is set.
        /// </summary>
        public CloudinaryUpload()
        {
            Api = new Api();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudinaryUpload"/> class with Cloudinary URL.
        /// </summary>
        /// <param name="cloudinaryUrl">Cloudinary URL.</param>
        public CloudinaryUpload(string cloudinaryUrl)
        {
            Api = new Api(cloudinaryUrl);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudinaryUpload"/> class with Cloudinary account.
        /// </summary>
        /// <param name="account">Cloudinary account.</param>
        public CloudinaryUpload(Account account)
        {
            Api = new Api(account);
        }

        /// <inheritdoc />
        public Task<ImageUploadResult> UploadAsync(ImageUploadParams parameters, CancellationToken? cancellationToken = null)
        {
            return UploadAsync<ImageUploadResult>(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public ImageUploadResult Upload(ImageUploadParams parameters)
        {
            return Upload<ImageUploadResult, ImageUploadParams>(parameters);
        }

        /// <inheritdoc />
        public Task<VideoUploadResult> UploadAsync(VideoUploadParams parameters, CancellationToken? cancellationToken = null)
        {
            return UploadAsync<VideoUploadResult>(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public VideoUploadResult Upload(VideoUploadParams parameters)
        {
            return Upload<VideoUploadResult, VideoUploadParams>(parameters);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public RawUploadResult Upload(string resourceType, IDictionary<string, object> parameters, FileDescription fileDescription)
        {
            return UploadAsync(resourceType, parameters, fileDescription).GetAwaiter().GetResult();
        }

        /// <inheritdoc />
        public Task<RawUploadResult> UploadAsync(RawUploadParams parameters, string type = "auto", CancellationToken? cancellationToken = null)
        {
            string uri = Api.ApiUrlImgUpV.ResourceType(type).BuildUrl();

            parameters.File.Reset();

            return CallUploadApiAsync<RawUploadResult>(
                HttpMethod.POST,
                uri,
                parameters,
                cancellationToken,
                parameters.File);
        }

        /// <inheritdoc />
        public RawUploadResult Upload(RawUploadParams parameters, string type = "auto")
        {
            return UploadAsync(parameters, type, null).GetAwaiter().GetResult();
        }

        /// <inheritdoc />
        public Task<RawUploadResult> UploadLargeRawAsync(
            BasicRawUploadParams parameters,
            int bufferSize = DEFAULT_CHUNK_SIZE,
            CancellationToken? cancellationToken = null)
        {
            return UploadLargeAsync<RawUploadResult>(parameters, bufferSize, cancellationToken);
        }

        /// <inheritdoc />
        public RawUploadResult UploadLargeRaw(BasicRawUploadParams parameters, int bufferSize = DEFAULT_CHUNK_SIZE)
        {
            return UploadLarge<RawUploadResult>(parameters, bufferSize);
        }

        /// <inheritdoc />
        public Task<RawUploadResult> UploadLargeAsync(
            RawUploadParams parameters,
            int bufferSize = DEFAULT_CHUNK_SIZE,
            CancellationToken? cancellationToken = null)
        {
            return UploadLargeAsync<RawUploadResult>(parameters, bufferSize, cancellationToken);
        }

        /// <inheritdoc />
        public RawUploadResult UploadLarge(RawUploadParams parameters, int bufferSize = DEFAULT_CHUNK_SIZE)
        {
            return UploadLarge<RawUploadResult>(parameters, bufferSize);
        }

        /// <inheritdoc />
        public Task<ImageUploadResult> UploadLargeAsync(
            ImageUploadParams parameters,
            int bufferSize = DEFAULT_CHUNK_SIZE,
            CancellationToken? cancellationToken = null)
        {
            return UploadLargeAsync<ImageUploadResult>(parameters, bufferSize, cancellationToken);
        }

        /// <inheritdoc />
        public ImageUploadResult UploadLarge(ImageUploadParams parameters, int bufferSize = DEFAULT_CHUNK_SIZE)
        {
            return UploadLarge<ImageUploadResult>(parameters, bufferSize);
        }

        /// <inheritdoc />
        public Task<VideoUploadResult> UploadLargeAsync(
            VideoUploadParams parameters,
            int bufferSize = DEFAULT_CHUNK_SIZE,
            CancellationToken? cancellationToken = null)
        {
            return UploadLargeAsync<VideoUploadResult>(parameters, bufferSize, cancellationToken);
        }

        /// <inheritdoc />
        public VideoUploadResult UploadLarge(VideoUploadParams parameters, int bufferSize = DEFAULT_CHUNK_SIZE)
        {
            return UploadLarge<VideoUploadResult>(parameters, bufferSize);
        }

        /// <inheritdoc />
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

            var internalParams = new UploadLargeParams(parameters, bufferSize, Api);
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

        /// <inheritdoc />
        public T UploadLarge<T>(BasicRawUploadParams parameters, int bufferSize = DEFAULT_CHUNK_SIZE)
            where T : UploadResult, new()
        {
            return UploadLargeAsync<T>(parameters, bufferSize).GetAwaiter().GetResult();
        }

        /// <inheritdoc />
        public Task<RenameResult> RenameAsync(
            string fromPublicId,
            string toPublicId,
            bool overwrite = false,
            CancellationToken? cancellationToken = null)
        {
            return RenameAsync(
                new RenameParams(fromPublicId, toPublicId)
                {
                    Overwrite = overwrite,
                },
                cancellationToken);
        }

        /// <inheritdoc />
        public RenameResult Rename(string fromPublicId, string toPublicId, bool overwrite = false)
        {
            var renameParams = new RenameParams(fromPublicId, toPublicId)
            {
                Overwrite = overwrite,
            };

            return RenameAsync(renameParams).GetAwaiter().GetResult();
        }

        /// <inheritdoc />
        public Task<RenameResult> RenameAsync(RenameParams parameters, CancellationToken? cancellationToken = null)
        {
            var uri = GetRenameUrl(parameters);
            return CallUploadApiAsync<RenameResult>(
                HttpMethod.POST,
                uri,
                parameters,
                cancellationToken);
        }

        /// <inheritdoc />
        public RenameResult Rename(RenameParams parameters)
        {
            return RenameAsync(parameters).GetAwaiter().GetResult();
        }

        /// <inheritdoc />
        public Task<DeletionResult> DestroyAsync(DeletionParams parameters)
        {
            string uri = Api.ApiUrlImgUpV.ResourceType(
                    ApiShared.GetCloudinaryParam(parameters.ResourceType)).
                Action("destroy").BuildUrl();

            return CallUploadApiAsync<DeletionResult>(HttpMethod.POST, uri, parameters, null);
        }

        /// <inheritdoc />
        public DeletionResult Destroy(DeletionParams parameters)
        {
            return DestroyAsync(parameters).GetAwaiter().GetResult();
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public Task<TagResult> TagAsync(TagParams parameters, CancellationToken? cancellationToken = null)
        {
            string uri = GetApiUrlV()
                .ResourceType(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType))
                .Action(Constants.TAGS_MANGMENT)
                .BuildUrl();

            return CallUploadApiAsync<TagResult>(HttpMethod.POST, uri, parameters, cancellationToken);
        }

        /// <inheritdoc />
        public TagResult Tag(TagParams parameters)
        {
            return TagAsync(parameters).GetAwaiter().GetResult();
        }

        /// <inheritdoc />
        public Task<ContextResult> ContextAsync(ContextParams parameters, CancellationToken? cancellationToken = null)
        {
            string uri = Api.ApiUrlImgUpV.ResourceType(ApiShared.GetCloudinaryParam(parameters.ResourceType)).Action(Constants.CONTEXT_MANAGMENT).BuildUrl();

            return CallUploadApiAsync<ContextResult>(
                HttpMethod.POST,
                uri,
                parameters,
                cancellationToken);
        }

        /// <inheritdoc />
        public ContextResult Context(ContextParams parameters)
        {
            return ContextAsync(parameters).GetAwaiter().GetResult();
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public ExplicitResult Explicit(ExplicitParams parameters)
        {
            return ExplicitAsync(parameters).GetAwaiter().GetResult();
        }

        /// <inheritdoc />
        public Task<ExplodeResult> ExplodeAsync(ExplodeParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = Api.ApiUrlImgUpV.
                Action("explode").
                BuildUrl();

            return CallUploadApiAsync<ExplodeResult>(
                HttpMethod.POST,
                url,
                parameters,
                cancellationToken);
        }

        /// <inheritdoc />
        public ExplodeResult Explode(ExplodeParams parameters)
        {
            return ExplodeAsync(parameters).GetAwaiter().GetResult();
        }

        /// <inheritdoc />
        public Task<ArchiveResult> CreateZipAsync(ArchiveParams parameters, CancellationToken? cancellationToken = null)
        {
            parameters.TargetFormat(ArchiveFormat.Zip);
            return CreateArchiveAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public ArchiveResult CreateZip(ArchiveParams parameters)
        {
            parameters.TargetFormat(ArchiveFormat.Zip);
            return CreateArchive(parameters);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public ArchiveResult CreateArchive(ArchiveParams parameters)
        {
            return CreateArchiveAsync(parameters).GetAwaiter().GetResult();
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public string DownloadFolder(string folderPath, ArchiveParams parameters = null)
        {
            var downloadParameters = parameters ?? new ArchiveParams();

            downloadParameters.Prefixes(new List<string> { folderPath });
            downloadParameters.ResourceType(Constants.RESOURCE_TYPE_ALL);

            return DownloadArchiveUrl(downloadParameters);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public Task<SpriteResult> MakeSpriteAsync(SpriteParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = Api.ApiUrlImgUpV.
                Action(Constants.ACTION_NAME_SPRITE).
                BuildUrl();

            return CallUploadApiAsync<SpriteResult>(
                HttpMethod.POST,
                url,
                parameters,
                cancellationToken);
        }

        /// <inheritdoc />
        public SpriteResult MakeSprite(SpriteParams parameters)
        {
            return MakeSpriteAsync(parameters).GetAwaiter().GetResult();
        }

        /// <inheritdoc />
        public string DownloadSprite(SpriteParams parameters)
        {
            parameters.Mode = ArchiveCallMode.Download;
            var urlBuilder = new UrlBuilder(
                Api.ApiUrlImgUpV.
                    Action(Constants.ACTION_NAME_SPRITE).
                    BuildUrl());

            return GetDownloadUrl(urlBuilder, parameters.ToParamsDictionary());
        }

        /// <inheritdoc />
        public Task<MultiResult> MultiAsync(MultiParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = Api.ApiUrlImgUpV.
                Action(Constants.ACTION_NAME_MULTI).
                BuildUrl();

            return CallUploadApiAsync<MultiResult>(
                HttpMethod.POST,
                url,
                parameters,
                cancellationToken);
        }

        /// <inheritdoc />
        public MultiResult Multi(MultiParams parameters)
        {
            return MultiAsync(parameters).GetAwaiter().GetResult();
        }

        /// <inheritdoc />
        public string DownloadMulti(MultiParams parameters)
        {
            parameters.Mode = ArchiveCallMode.Download;
            var urlBuilder = new UrlBuilder(
                    Api.ApiUrlImgUpV.
                    Action(Constants.ACTION_NAME_MULTI).
                    BuildUrl());

            return GetDownloadUrl(urlBuilder, parameters.ToParamsDictionary());
        }

        /// <inheritdoc />
        public Task<TextResult> TextAsync(string text, CancellationToken? cancellationToken = null)
        {
            return TextAsync(new TextParams(text), cancellationToken);
        }

        /// <inheritdoc />
        public TextResult Text(string text)
        {
            return Text(new TextParams(text));
        }

        /// <inheritdoc />
        public Task<TextResult> TextAsync(TextParams parameters, CancellationToken? cancellationToken = null)
        {
            string uri = Api.ApiUrlImgUpV.Action("text").BuildUrl();

            return CallUploadApiAsync<TextResult>(
                HttpMethod.POST,
                uri,
                parameters,
                cancellationToken);
        }

        /// <inheritdoc />
        public TextResult Text(TextParams parameters)
        {
            return TextAsync(parameters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Creates auto-generated video slideshow.
        /// </summary>
        /// <param name="parameters">Parameters for generating the slideshow.</param>
        /// <returns>The public id of the generated slideshow.</returns>
        public CreateSlideshowResult CreateSlideshow(CreateSlideshowParams parameters)
        {
            return CreateSlideshowAsync(parameters).GetAwaiter().GetResult();
        }

        /// <summary>
        ///  Creates auto-generated video slideshow asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters for generating the slideshow.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>The public id of the generated slideshow.</returns>
        public Task<CreateSlideshowResult> CreateSlideshowAsync(CreateSlideshowParams parameters, CancellationToken? cancellationToken = null)
        {
            string uri = Api.ApiUrlVideoUpV.Action("create_slideshow").BuildUrl();

            return CallUploadApiAsync<CreateSlideshowResult>(
                HttpMethod.POST,
                uri,
                parameters,
                cancellationToken);
        }

        /// <inheritdoc />
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
                    new JProperty("cloud_name", Api.Account.Cloud),
                    new JProperty("api_key", Api.Account.ApiKey),
                    new JProperty("private_cdn", Api.UsePrivateCdn),
                    new JProperty("cdn_subdomain", Api.CSubDomain),
                });

            if (!string.IsNullOrEmpty(Api.PrivateCdn))
            {
                cloudinaryParams.Add("secure_distribution", Api.PrivateCdn);
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
            return Api.CallApiAsync<T>(
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
            return Api.CallAndParseAsync<RawUploadResult>(
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
            Api
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
            Api.FinalizeUploadParameters(parameters);
            builder.SetParameters(parameters);
            return builder.ToString();
        }

        /// <summary>
        /// Get default API URL with version.
        /// </summary>
        /// <returns>URL of the API.</returns>
        private Url GetApiUrlV()
        {
            return Api.ApiUrlV;
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
