namespace CloudinaryDotNet
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;

    /// <summary>
    /// Cloudinary Upload Api Interface.
    /// </summary>
    public interface ICloudinaryUploadApi
    {
        /// <summary>
        /// Uploads an image file to Cloudinary asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of image uploading .</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Results of image uploading.</returns>
        Task<ImageUploadResult> UploadAsync(ImageUploadParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Uploads a video file to Cloudinary asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of video uploading.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Results of video uploading.</returns>
        Task<VideoUploadResult> UploadAsync(VideoUploadParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Uploads a file to Cloudinary asynchronously.
        /// </summary>
        /// <param name="resourceType">Resource type ("image", "raw", "video", "auto").</param>
        /// <param name="parameters">Upload parameters.</param>
        /// <param name="fileDescription">File description.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Results of the raw file uploading.</returns>
        Task<RawUploadResult> UploadAsync(
            string resourceType,
            IDictionary<string, object> parameters,
            FileDescription fileDescription,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Uploads a file to Cloudinary asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="type">The type ("raw" or "auto", last by default).</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the raw file uploading.</returns>
        Task<RawUploadResult> UploadAsync(RawUploadParams parameters, string type = "auto", CancellationToken? cancellationToken = null);

        /// <summary>
        /// Uploads an image file to Cloudinary.
        /// </summary>
        /// <param name="parameters">Parameters of image uploading .</param>
        /// <returns>Results of image uploading.</returns>
        ImageUploadResult Upload(ImageUploadParams parameters);

        /// <summary>
        /// Uploads a video file to Cloudinary.
        /// </summary>
        /// <param name="parameters">Parameters of video uploading.</param>
        /// <returns>Results of video uploading.</returns>
        VideoUploadResult Upload(VideoUploadParams parameters);

        /// <summary>
        /// Uploads a file to Cloudinary.
        /// </summary>
        /// <param name="resourceType">Resource type ("image", "raw", "video", "auto").</param>
        /// <param name="parameters">Upload parameters.</param>
        /// <param name="fileDescription">File description.</param>
        /// <returns>Results of the raw file uploading.</returns>
        RawUploadResult Upload(string resourceType, IDictionary<string, object> parameters, FileDescription fileDescription);

        /// <summary>
        /// Uploads a file to Cloudinary.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="type">The type ("raw" or "auto", last by default).</param>
        /// <returns>Parsed result of the raw file uploading.</returns>
        RawUploadResult Upload(RawUploadParams parameters, string type = "auto");

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
        Task<RawUploadResult> UploadLargeRawAsync(
            BasicRawUploadParams parameters,
            int bufferSize = 0,
            CancellationToken? cancellationToken = null);

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
        RawUploadResult UploadLargeRaw(BasicRawUploadParams parameters, int bufferSize = 0);

        /// <summary>
        /// Uploads large raw file to Cloudinary by dividing it to chunks asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of uploading.</returns>
        Task<RawUploadResult> UploadLargeAsync(
            RawUploadParams parameters,
            int bufferSize = 0,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Uploads large image file to Cloudinary by dividing it to chunks asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of uploading.</returns>
        Task<ImageUploadResult> UploadLargeAsync(
            ImageUploadParams parameters,
            int bufferSize = 0,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Uploads large video file to Cloudinary by dividing it to chunks asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of uploading.</returns>
        Task<VideoUploadResult> UploadLargeAsync(
            VideoUploadParams parameters,
            int bufferSize = 0,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Uploads large file to Cloudinary by dividing it to chunks asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of uploading.</returns>
        Task<RawUploadResult> UploadLargeAsync(
            AutoUploadParams parameters,
            int bufferSize = 0,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Uploads large resources to Cloudinary by dividing it to chunks asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of result of upload.</typeparam>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of uploading.</returns>
        Task<T> UploadLargeAsync<T>(
            BasicRawUploadParams parameters,
            int bufferSize = 0,
            CancellationToken? cancellationToken = null)
            where T : UploadResult, new();

        /// <summary>
        /// Uploads large raw file to Cloudinary by dividing it to chunks.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <returns>Parsed result of uploading.</returns>
        RawUploadResult UploadLarge(RawUploadParams parameters, int bufferSize = 0);

        /// <summary>
        /// Uploads large image file to Cloudinary by dividing it to chunks.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <returns>Parsed result of uploading.</returns>
        ImageUploadResult UploadLarge(ImageUploadParams parameters, int bufferSize = 0);

        /// <summary>
        /// Uploads large video file to Cloudinary by dividing it to chunks.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <returns>Parsed result of uploading.</returns>
        VideoUploadResult UploadLarge(VideoUploadParams parameters, int bufferSize = 0);

        /// <summary>
        /// Uploads large file to Cloudinary by dividing it to chunks.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <returns>Parsed result of uploading.</returns>
        RawUploadResult UploadLarge(AutoUploadParams parameters, int bufferSize = 0);

        /// <summary>
        /// Uploads large resources to Cloudinary by dividing it to chunks.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <param name="isRaw">Whether the file is raw.</param>
        /// <returns>Parsed result of uploading.</returns>
        UploadResult UploadLarge(BasicRawUploadParams parameters, int bufferSize = 0, bool isRaw = false);

        /// <summary>
        /// Uploads large resources to Cloudinary by dividing it to chunks.
        /// </summary>
        /// <typeparam name="T">The type of result of upload.</typeparam>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <returns>Parsed result of uploading.</returns>
        T UploadLarge<T>(BasicRawUploadParams parameters, int bufferSize = 0)
            where T : UploadResult, new();

        /// <summary>
        /// Changes public identifier of a file asynchronously.
        /// </summary>
        /// <param name="fromPublicId">Old identifier.</param>
        /// <param name="toPublicId">New identifier.</param>
        /// <param name="overwrite">Overwrite a file with the same identifier as new if such file exists.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Result of resource renaming.</returns>
        Task<RenameResult> RenameAsync(
            string fromPublicId,
            string toPublicId,
            bool overwrite = false,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Changes public identifier of a file asynchronously.
        /// </summary>
        /// <param name="parameters">Operation parameters.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Result of resource renaming.</returns>
        Task<RenameResult> RenameAsync(RenameParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Changes public identifier of a file.
        /// </summary>
        /// <param name="fromPublicId">Old identifier.</param>
        /// <param name="toPublicId">New identifier.</param>
        /// <param name="overwrite">Overwrite a file with the same identifier as new if such file exists.</param>
        /// <returns>Result of resource renaming.</returns>
        RenameResult Rename(string fromPublicId, string toPublicId, bool overwrite = false);

        /// <summary>
        /// Changes public identifier of a file.
        /// </summary>
        /// <param name="parameters">Operation parameters.</param>
        /// <returns>Result of resource renaming.</returns>
        RenameResult Rename(RenameParams parameters);

        /// <summary>
        /// Delete file from Cloudinary asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters for deletion of resource from Cloudinary.</param>
        /// <returns>Results of deletion.</returns>
        Task<DeletionResult> DestroyAsync(DeletionParams parameters);

        /// <summary>
        /// Deletes file from Cloudinary.
        /// </summary>
        /// <param name="parameters">Parameters for deletion of resource from Cloudinary.</param>
        /// <returns>Results of deletion.</returns>
        DeletionResult Destroy(DeletionParams parameters);

        /// <summary>
        /// Creates and returns an URL that allows downloading the backed-up asset
        /// based on the the asset ID and the version ID.
        /// </summary>
        /// <param name="assetId">ID of the asset.</param>
        /// <param name="versionId">Version ID of the asset.</param>
        /// <returns>Url for downloading the backed-up asset.</returns>
        string DownloadBackedUpAsset(string assetId, string versionId);

        /// <summary>
        /// Manage tag assignments asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of tag management.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Results of tags management.</returns>
        Task<TagResult> TagAsync(TagParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Manages tag assignments.
        /// </summary>
        /// <param name="parameters">Parameters of tag management.</param>
        /// <returns>Results of tags management.</returns>
        TagResult Tag(TagParams parameters);

        /// <summary>
        /// Manages context assignments asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of context management.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Results of contexts management.</returns>
        Task<ContextResult> ContextAsync(ContextParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Manages context assignments.
        /// </summary>
        /// <param name="parameters">Parameters of context management.</param>
        /// <returns>Results of contexts management.</returns>
        ContextResult Context(ContextParams parameters);

        /// <summary>
        /// This method can be used to force refresh facebook and twitter profile pictures. The response of this method
        /// includes the image's version. Use this version to bypass previously cached CDN copies. Also it can be used
        /// to generate transformed versions of an uploaded image. This is useful when Strict Transformations are
        /// allowed for your account and you wish to create custom derived images for already uploaded images asynchronously.
        /// </summary>
        /// <param name="parameters">The parameters for explicit method.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after a call of Explicit method.</returns>
        Task<ExplicitResult> ExplicitAsync(ExplicitParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// This method can be used to force refresh facebook and twitter profile pictures. The response of this method
        /// includes the image's version. Use this version to bypass previously cached CDN copies. Also it can be used
        /// to generate transformed versions of an uploaded image. This is useful when Strict Transformations are
        /// allowed for your account and you wish to create custom derived images for already uploaded images.
        /// </summary>
        /// <param name="parameters">The parameters for explicit method.</param>
        /// <returns>Parsed response after a call of Explicit method.</returns>
        ExplicitResult Explicit(ExplicitParams parameters);

        /// <summary>
        /// Explodes multi page document to single pages asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of explosion operation.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after a call of Explode method.</returns>
        Task<ExplodeResult> ExplodeAsync(ExplodeParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Explodes multi page document to single pages.
        /// </summary>
        /// <param name="parameters">Parameters of explosion operation.</param>
        /// <returns>Parsed response after a call of Explode method.</returns>
        ExplodeResult Explode(ExplodeParams parameters);

        /// <summary>
        /// Creates a zip archive and stores it as a raw resource in your Cloudinary account asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of the new generated zip archive.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of creating the archive.</returns>
        Task<ArchiveResult> CreateZipAsync(ArchiveParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Creates a zip archive and stores it as a raw resource in your Cloudinary account.
        /// </summary>
        /// <param name="parameters">Parameters of the new generated zip archive.</param>
        /// <returns>Parsed result of creating the archive.</returns>
        ArchiveResult CreateZip(ArchiveParams parameters);

        /// <summary>
        /// Creates archive and stores it as a raw resource in your Cloudinary account asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of new generated archive.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of creating the archive.</returns>
        Task<ArchiveResult> CreateArchiveAsync(ArchiveParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Creates archive and stores it as a raw resource in your Cloudinary account.
        /// </summary>
        /// <param name="parameters">Parameters of new generated archive.</param>
        /// <returns>Parsed result of creating the archive.</returns>
        ArchiveResult CreateArchive(ArchiveParams parameters);

        /// <summary>
        ///  Returns URL on archive file.
        /// </summary>
        /// <param name="parameters">Parameters of generated archive.</param>
        /// <returns>URL on archive file.</returns>
        string DownloadArchiveUrl(ArchiveParams parameters);

        /// <summary>
        ///  Creates and returns an URL that when invoked creates an archive of a folder.
        /// </summary>
        /// <param name="folderPath">Full path from the root.</param>
        /// <param name="parameters">Optional parameters of generated archive.</param>
        /// <returns>Url for downloading an archive of a folder.</returns>
        string DownloadFolder(string folderPath, ArchiveParams parameters = null);

        /// <summary>
        /// Gets URL to download tag cloud as ZIP package.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="transform">The transformation.</param>
        /// <returns>Download URL.</returns>
        /// <exception cref="System.ArgumentException">Tag should be specified.</exception>
        /// <param name="resourceType">Resource type (image, video or raw) of files to include in the archive (optional).</param>
        string DownloadZip(string tag, Transformation transform, string resourceType = "image");

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
        string DownloadPrivate(
            string publicId,
            bool? attachment = null,
            string format = "",
            string type = "",
            long? expiresAt = null,
            string resourceType = "image");

        /// <summary>
        /// Eagerly generate sprites asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters for sprite generation.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response with detailed information about the created sprite.</returns>
        Task<SpriteResult> MakeSpriteAsync(SpriteParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Eagerly generates sprites.
        /// </summary>
        /// <param name="parameters">Parameters for sprite generation.</param>
        /// <returns>Parsed response with detailed information about the created sprite.</returns>
        SpriteResult MakeSprite(SpriteParams parameters);

        /// <summary>
        /// Gets a signed URL to download generated sprite.
        /// </summary>
        /// <param name="parameters">Parameters of Sprite operation.</param>
        /// <returns>Download URL.</returns>
        string DownloadSprite(SpriteParams parameters);

        /// <summary>
        /// Creates a single animated GIF file from a group of images asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of Multi operation.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response with detailed information about the created animated GIF.</returns>
        Task<MultiResult> MultiAsync(MultiParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Creates a single animated GIF file from a group of images.
        /// </summary>
        /// <param name="parameters">Parameters of Multi operation.</param>
        /// <returns>Parsed response with detailed information about the created animated GIF.</returns>
        MultiResult Multi(MultiParams parameters);

        /// <summary>
        /// Gets a signed URL to download animated GIF file generated through multi request.
        /// </summary>
        /// <param name="parameters">Parameters of Multi operation.</param>
        /// <returns>Download URL.</returns>
        string DownloadMulti(MultiParams parameters);

        /// <summary>
        /// Generate an image of a given textual string asynchronously.
        /// </summary>
        /// <param name="text">Text to draw.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Results of generating an image of a given textual string.</returns>
        Task<TextResult> TextAsync(string text, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Generates an image of a given textual string asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of generating an image of a given textual string.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Results of generating an image of a given textual string.</returns>
        Task<TextResult> TextAsync(TextParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Generates an image of a given textual string.
        /// </summary>
        /// <param name="text">Text to draw.</param>
        /// <returns>Results of generating an image of a given textual string.</returns>
        TextResult Text(string text);

        /// <summary>
        /// Generates an image of a given textual string.
        /// </summary>
        /// <param name="parameters">Parameters of generating an image of a given textual string.</param>
        /// <returns>Results of generating an image of a given textual string.</returns>
        TextResult Text(TextParams parameters);

        /// <summary>
        /// Creates auto-generated video slideshow.
        /// </summary>
        /// <param name="parameters">Parameters for generating the slideshow.</param>
        /// <returns>The public id of the generated slideshow.</returns>
        CreateSlideshowResult CreateSlideshow(CreateSlideshowParams parameters);

        /// <summary>
        ///  Creates auto-generated video slideshow asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters for generating the slideshow.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>The public id of the generated slideshow.</returns>
        Task<CreateSlideshowResult> CreateSlideshowAsync(CreateSlideshowParams parameters, CancellationToken? cancellationToken = null);
    }
}
