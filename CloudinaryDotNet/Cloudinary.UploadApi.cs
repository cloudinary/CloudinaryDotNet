namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;

    /// <summary>
    /// Main class of Cloudinary .NET API.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
    public partial class Cloudinary : ICloudinaryUpload
    {
        private readonly ICloudinaryUpload cloudinaryUpload;

        /// <inheritdoc />
        public Task<ImageUploadResult> UploadAsync(ImageUploadParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.UploadAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public Task<VideoUploadResult> UploadAsync(VideoUploadParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.UploadAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public Task<RawUploadResult> UploadAsync(string resourceType, IDictionary<string, object> parameters, FileDescription fileDescription, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.UploadAsync(resourceType, parameters, fileDescription, cancellationToken);
        }

        /// <inheritdoc />
        public Task<RawUploadResult> UploadAsync(RawUploadParams parameters, string type = "auto", CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.UploadAsync(parameters, type, cancellationToken);
        }

        /// <inheritdoc />
        public ImageUploadResult Upload(ImageUploadParams parameters)
        {
            return cloudinaryUpload.Upload(parameters);
        }

        /// <inheritdoc />
        public VideoUploadResult Upload(VideoUploadParams parameters)
        {
            return cloudinaryUpload.Upload(parameters);
        }

        /// <inheritdoc />
        public RawUploadResult Upload(string resourceType, IDictionary<string, object> parameters, FileDescription fileDescription)
        {
            return cloudinaryUpload.Upload(resourceType, parameters, fileDescription);
        }

        /// <inheritdoc />
        public RawUploadResult Upload(RawUploadParams parameters, string type = "auto")
        {
            return cloudinaryUpload.Upload(parameters, type);
        }

        /// <inheritdoc />
        public Task<RawUploadResult> UploadLargeRawAsync(BasicRawUploadParams parameters, int bufferSize = CloudinaryUpload.DEFAULT_CHUNK_SIZE, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.UploadLargeRawAsync(parameters, bufferSize, cancellationToken);
        }

        /// <inheritdoc />
        public RawUploadResult UploadLargeRaw(BasicRawUploadParams parameters, int bufferSize = CloudinaryUpload.DEFAULT_CHUNK_SIZE)
        {
            return cloudinaryUpload.UploadLargeRaw(parameters, bufferSize);
        }

        /// <inheritdoc />
        public Task<RawUploadResult> UploadLargeAsync(RawUploadParams parameters, int bufferSize = CloudinaryUpload.DEFAULT_CHUNK_SIZE, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.UploadLargeAsync(parameters, bufferSize, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ImageUploadResult> UploadLargeAsync(ImageUploadParams parameters, int bufferSize = CloudinaryUpload.DEFAULT_CHUNK_SIZE, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.UploadLargeAsync(parameters, bufferSize, cancellationToken);
        }

        /// <inheritdoc />
        public Task<VideoUploadResult> UploadLargeAsync(VideoUploadParams parameters, int bufferSize = CloudinaryUpload.DEFAULT_CHUNK_SIZE, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.UploadLargeAsync(parameters, bufferSize, cancellationToken);
        }

        /// <inheritdoc />
        public Task<T> UploadLargeAsync<T>(BasicRawUploadParams parameters, int bufferSize = CloudinaryUpload.DEFAULT_CHUNK_SIZE, CancellationToken? cancellationToken = null)
            where T : UploadResult, new()
        {
            return cloudinaryUpload.UploadLargeAsync<T>(parameters, bufferSize, cancellationToken);
        }

        /// <inheritdoc />
        public RawUploadResult UploadLarge(RawUploadParams parameters, int bufferSize = CloudinaryUpload.DEFAULT_CHUNK_SIZE)
        {
            return cloudinaryUpload.UploadLarge(parameters, bufferSize);
        }

        /// <inheritdoc />
        public ImageUploadResult UploadLarge(ImageUploadParams parameters, int bufferSize = CloudinaryUpload.DEFAULT_CHUNK_SIZE)
        {
            return cloudinaryUpload.UploadLarge(parameters, bufferSize);
        }

        /// <inheritdoc />
        public VideoUploadResult UploadLarge(VideoUploadParams parameters, int bufferSize = CloudinaryUpload.DEFAULT_CHUNK_SIZE)
        {
            return cloudinaryUpload.UploadLarge(parameters, bufferSize);
        }

        /// <summary>
        /// Uploads large resources to Cloudinary by dividing it to chunks.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <param name="isRaw">Whether the file is raw.</param>
        /// <returns>Parsed result of uploading.</returns>
        [Obsolete("Use UploadLarge<T>(parameters, bufferSize) instead.")]
        public UploadResult UploadLarge(BasicRawUploadParams parameters, int bufferSize = CloudinaryUpload.DEFAULT_CHUNK_SIZE, bool isRaw = false)
        {
            return isRaw
                ? UploadLarge<RawUploadResult>(parameters, bufferSize)
                : UploadLarge<ImageUploadResult>(parameters, bufferSize);
        }

        /// <inheritdoc />
        public T UploadLarge<T>(BasicRawUploadParams parameters, int bufferSize = CloudinaryUpload.DEFAULT_CHUNK_SIZE)
            where T : UploadResult, new()
        {
            return cloudinaryUpload.UploadLarge<T>(parameters, bufferSize);
        }

        /// <inheritdoc />
        public Task<RenameResult> RenameAsync(string fromPublicId, string toPublicId, bool overwrite = false, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.RenameAsync(fromPublicId, toPublicId, overwrite, cancellationToken);
        }

        /// <inheritdoc />
        public Task<RenameResult> RenameAsync(RenameParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.RenameAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public RenameResult Rename(string fromPublicId, string toPublicId, bool overwrite = false)
        {
            return cloudinaryUpload.Rename(fromPublicId, toPublicId, overwrite);
        }

        /// <inheritdoc />
        public RenameResult Rename(RenameParams parameters)
        {
            return cloudinaryUpload.Rename(parameters);
        }

        /// <inheritdoc />
        public Task<DeletionResult> DestroyAsync(DeletionParams parameters)
        {
            return cloudinaryUpload.DestroyAsync(parameters);
        }

        /// <inheritdoc />
        public DeletionResult Destroy(DeletionParams parameters)
        {
            return cloudinaryUpload.Destroy(parameters);
        }

        /// <inheritdoc />
        public string DownloadBackedUpAsset(string assetId, string versionId)
        {
            return cloudinaryUpload.DownloadBackedUpAsset(assetId, versionId);
        }

        /// <inheritdoc />
        public Task<TagResult> TagAsync(TagParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.TagAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public TagResult Tag(TagParams parameters)
        {
            return cloudinaryUpload.Tag(parameters);
        }

        /// <inheritdoc />
        public Task<ContextResult> ContextAsync(ContextParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.ContextAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public ContextResult Context(ContextParams parameters)
        {
            return cloudinaryUpload.Context(parameters);
        }

        /// <inheritdoc />
        public Task<ExplicitResult> ExplicitAsync(ExplicitParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.ExplicitAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public ExplicitResult Explicit(ExplicitParams parameters)
        {
            return cloudinaryUpload.Explicit(parameters);
        }

        /// <inheritdoc />
        public Task<ExplodeResult> ExplodeAsync(ExplodeParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.ExplodeAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public ExplodeResult Explode(ExplodeParams parameters)
        {
            return cloudinaryUpload.Explode(parameters);
        }

        /// <inheritdoc />
        public Task<ArchiveResult> CreateZipAsync(ArchiveParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.CreateZipAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public ArchiveResult CreateZip(ArchiveParams parameters)
        {
            return cloudinaryUpload.CreateZip(parameters);
        }

        /// <inheritdoc />
        public Task<ArchiveResult> CreateArchiveAsync(ArchiveParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.CreateArchiveAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public ArchiveResult CreateArchive(ArchiveParams parameters)
        {
            return cloudinaryUpload.CreateArchive(parameters);
        }

        /// <inheritdoc />
        public string DownloadArchiveUrl(ArchiveParams parameters)
        {
            return cloudinaryUpload.DownloadArchiveUrl(parameters);
        }

        /// <inheritdoc />
        public string DownloadFolder(string folderPath, ArchiveParams parameters = null)
        {
            return cloudinaryUpload.DownloadFolder(folderPath, parameters);
        }

        /// <inheritdoc />
        public string DownloadZip(string tag, Transformation transform, string resourceType = CloudinaryUpload.RESOURCE_TYPE_IMAGE)
        {
            return cloudinaryUpload.DownloadZip(tag, transform, resourceType);
        }

        /// <inheritdoc />
        public string DownloadPrivate(string publicId, bool? attachment = null, string format = "", string type = "", long? expiresAt = null, string resourceType = CloudinaryUpload.RESOURCE_TYPE_IMAGE)
        {
            return cloudinaryUpload.DownloadPrivate(publicId, attachment, format, type, expiresAt, resourceType);
        }

        /// <inheritdoc />
        public Task<SpriteResult> MakeSpriteAsync(SpriteParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.MakeSpriteAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public SpriteResult MakeSprite(SpriteParams parameters)
        {
            return cloudinaryUpload.MakeSprite(parameters);
        }

        /// <inheritdoc />
        public string DownloadSprite(SpriteParams parameters)
        {
            return cloudinaryUpload.DownloadSprite(parameters);
        }

        /// <inheritdoc />
        public Task<MultiResult> MultiAsync(MultiParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.MultiAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public MultiResult Multi(MultiParams parameters)
        {
            return cloudinaryUpload.Multi(parameters);
        }

        /// <inheritdoc />
        public string DownloadMulti(MultiParams parameters)
        {
            return cloudinaryUpload.DownloadMulti(parameters);
        }

        /// <inheritdoc />
        public Task<TextResult> TextAsync(string text, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.TextAsync(text, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TextResult> TextAsync(TextParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.TextAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public TextResult Text(string text)
        {
            return cloudinaryUpload.Text(text);
        }

        /// <inheritdoc />
        public TextResult Text(TextParams parameters)
        {
            return cloudinaryUpload.Text(parameters);
        }

        /// <inheritdoc />
        public CreateSlideshowResult CreateSlideshow(CreateSlideshowParams parameters)
        {
            return cloudinaryUpload.CreateSlideshow(parameters);
        }

        /// <inheritdoc />
        public Task<CreateSlideshowResult> CreateSlideshowAsync(CreateSlideshowParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinaryUpload.CreateSlideshowAsync(parameters, cancellationToken);
        }

        /// <inheritdoc />
        public string GetCloudinaryJsConfig(bool directUpload = false, string dir = "")
        {
            return cloudinaryUpload.GetCloudinaryJsConfig(directUpload, dir);
        }
    }
}
