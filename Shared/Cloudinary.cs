using CloudinaryDotNet.Actions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Main class of Cloudinary .NET API.
    /// </summary>
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
        /// API object that used by this instance.
        /// </summary>
        public Api Api
        {
            get { return m_api; }
        }

        /// <summary>
        /// Gets the advanced search provider used by the Cloudinary instance.
        /// </summary>
        /// <returns></returns>
        public Search Search()
        {
            return new Search(m_api);
        }

        /// <summary>
        /// Default parameterless constructor. Assumes that environment variable CLOUDINARY_URL is set.
        /// </summary>
        public Cloudinary()
        {
            m_api = new Api();
        }

        /// <summary>
        /// Instantiates the <see cref="Cloudinary"/> object with Cloudinary Url.
        /// </summary>
        /// <param name="cloudinaryUrl">Cloudinary URL.</param>
        public Cloudinary(string cloudinaryUrl)
        {
            m_api = new Api(cloudinaryUrl);
        }

        /// <summary>
        /// Instantiates the <see cref="Cloudinary"/> object with Cloudinary account.
        /// </summary>
        /// <param name="account">Cloudinary account.</param>
        public Cloudinary(Account account)
        {
            m_api = new Api(account);
        }

        /// <summary>
        /// Gets URL to download private image.
        /// </summary>
        /// <param name="publicId">The image public ID.</param>
        /// <param name="attachment">Whether to download image as attachment (optional).</param>
        /// <param name="format">Format to download (optional).</param>
        /// <param name="type">The type (optional).</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">publicId can't be null.</exception>
        public string DownloadPrivate(string publicId, bool? attachment = null, string format = "", string type = "")
        {
            if (String.IsNullOrEmpty(publicId))
                throw new ArgumentException("publicId");

            UrlBuilder urlBuilder = new UrlBuilder(
               m_api.ApiUrlV
               .ResourceType(RESOURCE_TYPE_IMAGE)
               .Action("download")
               .BuildUrl());

            var parameters = new SortedDictionary<string, object>
            {
                { "public_id", publicId }
            };

            if (!String.IsNullOrEmpty(format))
                parameters.Add("format", format);

            if (attachment != null)
                parameters.Add("attachment", (bool)attachment ? "true" : "false");

            if (!String.IsNullOrEmpty(type))
                parameters.Add("type", type);

            return GetDownloadUrl(urlBuilder, parameters);
        }

        /// <summary>
        /// Gets URL to download tag cloud as ZIP package.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="transform">The transformation.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Tag should be specified!</exception>
        public string DownloadZip(string tag, Transformation transform)
        {
            if (String.IsNullOrEmpty(tag))
                throw new ArgumentException("Tag should be specified!");

            UrlBuilder urlBuilder = new UrlBuilder(
               m_api.ApiUrlV
               .ResourceType(RESOURCE_TYPE_IMAGE)
               .Action("download_tag.zip")
               .BuildUrl());

            var parameters = new SortedDictionary<string, object>
            {
                { "tag", tag }
            };

            if (transform != null)
                parameters.Add("transformation", transform.Generate());

            return GetDownloadUrl(urlBuilder, parameters);
        }
        private string GetUploadMappingUrl()
        {
            return m_api.ApiUrlV.
                ResourceType("upload_mappings").
                BuildUrl();
        }

        private string GetUploadMappingUrl(UploadMappingParams parameters)
        {
            var uri = GetUploadMappingUrl();
            return new UrlBuilder(uri, parameters.ToParamsDictionary()).ToString();
        }


        /// <summary>
        ///  Returns Url on archive file.
        /// </summary>
        /// <param name="parameters">Parameters of generated archive.</param>
        /// <returns>Url on archive file.</returns>
        public string DownloadArchiveUrl(ArchiveParams parameters)
        {
            parameters.Mode(ArchiveCallMode.Download);

            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                ResourceType(RESOURCE_TYPE_IMAGE).
                Action(ACTION_GENERATE_ARCHIVE).
                BuildUrl());

            return GetDownloadUrl(urlBuilder, parameters.ToParamsDictionary());
        }

        /// <summary>
        /// Publishes resources by prefix.
        /// </summary>
        /// <param name="prefix">The prefix for publishing resources.</param>
        /// <param name="parameters">Parameters for publishing of resources.</param>
        /// <returns></returns>
        public PublishResourceResult PublishResourceByPrefix(string prefix, PublishResourceParams parameters)
        {
            return PublishResource("prefix", prefix, parameters);
        }

        /// <summary>
        /// Publishes resources by tag.
        /// </summary>
        /// <param name="tag">All resources with the given tag will be published.</param>
        /// <param name="parameters">Parameters for publishing of resources.</param>
        /// <returns></returns>
        public PublishResourceResult PublishResourceByTag(string tag, PublishResourceParams parameters)
        {
            return PublishResource("tag", tag, parameters);
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

        private PublishResourceResult PublishResource(string byKey, string value, PublishResourceParams parameters)
        {
            if (!string.IsNullOrWhiteSpace(byKey) && !string.IsNullOrWhiteSpace(value))
                parameters.AddCustomParam(byKey, value);

            Url url = m_api.ApiUrlV
                .Add("resources")
                .Add(parameters.ResourceType.ToString().ToLower())
                .Add("publish_resources");

            return m_api.CallApi<PublishResourceResult>(HttpMethod.POST, url.BuildUrl(), parameters, null);

        }

        private UpdateResourceAccessModeResult UpdateResourceAccessMode(string byKey, string value, UpdateResourceAccessModeParams parameters)
        {
            if (!string.IsNullOrWhiteSpace(byKey) && !string.IsNullOrWhiteSpace(value))
                parameters.AddCustomParam(byKey, value);

            Url url = m_api.ApiUrlV
                 .Add(Constants.RESOURCES_API_URL)
                 .Add(parameters.ResourceType.ToString().ToLower())
                 .Add(parameters.Type)
                 .Add(Constants.UPDATE_ACESS_MODE);


            return m_api.CallApi<UpdateResourceAccessModeResult>(HttpMethod.POST, url.BuildUrl(), parameters, null);
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
        /// Updates access mode for the resources selected by prefix.
        /// </summary>
        /// <param name="prefix">Update all resources where the public ID starts with the given prefix (up to a maximum
        /// of 100 matching original resources).</param>
        /// <param name="parameters">Parameters for updating of resources.</param>
        /// <returns>Structure with the results of update.</returns>
        public UpdateResourceAccessModeResult UpdateResourceAccessModeByPrefix(string prefix, UpdateResourceAccessModeParams parameters)
        {
            return UpdateResourceAccessMode(Constants.PREFIX_PARAM_NAME, prefix, parameters);
        }

        /// <summary>
        /// Updates access mode for the resources selected by public ids.
        /// </summary>
        /// <param name="parameters">Parameters for updating of resources. Update all resources with the given public IDs 
        /// (array of up to 100 public_ids).</param>
        /// <returns>Structure with the results of update.</returns>
        public UpdateResourceAccessModeResult UpdateResourceAccessModeByIds(UpdateResourceAccessModeParams parameters)
        {
            return UpdateResourceAccessMode(string.Empty, string.Empty, parameters);
        }

        /// <summary>
        /// Manages tag assignments.
        /// </summary>
        /// <param name="parameters">Parameters of tag management.</param>
        /// <returns>Results of tags management.</returns>
        public TagResult Tag(TagParams parameters)
        {
            string uri = m_api.ApiUrlV
                .ResourceType(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType))
                .Action(Constants.TAGS_MANGMENT)
                .BuildUrl();

            return m_api.CallApi<TagResult>(HttpMethod.POST, uri, parameters, null);
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
        /// Deletes derived resources by the given transformation (should be specified in parameters).
        /// </summary>
        /// <param name="parameters">Parameters to delete derived resources.</param>
        /// <returns>Parsed result of deletion derived resources.</returns>
        public DelDerivedResResult DeleteDerivedResourcesByTransform(DelDerivedResParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                Add("derived_resources").
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<DelDerivedResResult>(HttpMethod.DELETE, urlBuilder.ToString(), parameters, null);
        }

        /// <summary>
        /// Creates archive and stores it as a raw resource in your Cloudinary account.
        /// </summary>
        /// <param name="parameters">Parameters of new generated archive.</param>
        /// <returns>Parsed result of creating the archive.</returns>
        public ArchiveResult CreateArchive(ArchiveParams parameters)
        {
            Url url = m_api.ApiUrlV.ResourceType(RESOURCE_TYPE_IMAGE).Action(ACTION_GENERATE_ARCHIVE);

            if (!String.IsNullOrEmpty(parameters.ResourceType()))
                url.ResourceType(parameters.ResourceType());

            parameters.Mode(ArchiveCallMode.Create);
            return m_api.CallApi<ArchiveResult>(HttpMethod.POST, url.BuildUrl(), parameters, null);
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
        /// allowed for your account and you wish to create custom derived images for already uploaded images.
        /// </summary>
        /// <param name="parameters">The parameters for explicit method.</param>
        /// <returns>Parsed response after a call of Explicit method.</returns>
        public ExplicitResult Explicit(ExplicitParams parameters)
        {
            string uri = m_api.ApiUrlV
                .ResourceType(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType))
                .Action("explicit")
                .BuildUrl();

            return m_api.CallApi<ExplicitResult>(HttpMethod.POST, uri, parameters, null);
        }

        /// <summary>
        /// Creates the upload preset.
        /// Upload presets allow you to define the default behavior for your uploads, instead of receiving these as parameters during the upload request itself. Upload presets have precedence over client-side upload parameters.
        /// </summary>
        /// <param name="parameters">Parameters of the upload preset.</param>
        /// <returns>Parsed response after manipulation of upload presets.</returns>
        public UploadPresetResult CreateUploadPreset(UploadPresetParams parameters)
        {
            string url = m_api.ApiUrlV.
                Add("upload_presets").
                BuildUrl();

            return m_api.CallApi<UploadPresetResult>(HttpMethod.POST, url, parameters, null);
        }

        /// <summary>
        /// Updates the upload preset.
        /// Every update overwrites all the preset settings.
        /// </summary>
        /// <param name="parameters">New parameters for upload preset.</param>
        /// <returns>Parsed response after manipulation of upload presets.</returns>
        public UploadPresetResult UpdateUploadPreset(UploadPresetParams parameters)
        {
            var paramsCopy = (UploadPresetParams)parameters.Copy();
            paramsCopy.Name = null;

            var url = m_api.ApiUrlV
                .Add("upload_presets")
                .Add(parameters.Name)
                .BuildUrl();

            return m_api.CallApi<UploadPresetResult>(HttpMethod.PUT, url, paramsCopy, null);
        }

        /// <summary>
        /// Gets the upload preset.
        /// </summary>
        /// <param name="name">Name of the upload preset.</param>
        /// <returns>Upload preset details.</returns>
        public GetUploadPresetResult GetUploadPreset(string name)
        {
            var url = m_api.ApiUrlV
                .Add("upload_presets")
                .Add(name)
                .BuildUrl();

            return m_api.CallApi<GetUploadPresetResult>(HttpMethod.GET, url, null, null);
        }

        /// <summary>
        /// Lists upload presets.
        /// </summary>
        /// <returns>Parsed result of upload presets listing.</returns>
        public ListUploadPresetsResult ListUploadPresets(string nextCursor = null)
        {
            return ListUploadPresets(new ListUploadPresetsParams() { NextCursor = nextCursor });
        }

        /// <summary>
        /// Lists upload presets.
        /// </summary>
        /// <param name="parameters">Parameters to list upload presets.</param>
        /// <returns>Parsed result of upload presets listing.</returns>
        public ListUploadPresetsResult ListUploadPresets(ListUploadPresetsParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV
                .Add("upload_presets")
                .BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<ListUploadPresetsResult>(HttpMethod.GET, urlBuilder.ToString(), parameters, null);
        }

        /// <summary>
        /// Deletes the upload preset.
        /// </summary>
        /// <param name="name">Name of the upload preset.</param>
        /// <returns>Result of upload preset deletion.</returns>
        public DeleteUploadPresetResult DeleteUploadPreset(string name)
        {
            var url = m_api.ApiUrlV
                .Add("upload_presets")
                .Add(name)
                .BuildUrl();

            return m_api.CallApi<DeleteUploadPresetResult>(HttpMethod.DELETE, url, null, null);
        }

        /// <summary>
        /// Uploads a resource to Cloudinary.
        /// </summary>
        /// <param name="parameters">Parameters of uploading .</param>
        /// <returns>Results of uploading.</returns>
        private T Upload<T, P>(P parameters) where T : UploadResult, new()
                                             where P : BasicRawUploadParams, new()
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters", "Upload parameters should be defined");

            string uri = m_api.ApiUrlV
                .Action(Constants.ACTION_NAME_UPLOAD)
                .ResourceType(ApiShared.GetCloudinaryParam(parameters.ResourceType))
                .BuildUrl();

            parameters.File.Reset();

            return m_api.CallApi<T>(HttpMethod.POST, uri, parameters, parameters.File);
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
        /// Uploads a video file to Cloudinary.
        /// </summary>
        /// <param name="parameters">Parameters of video uploading.</param>
        /// <returns>Results of video uploading.</returns>
        public VideoUploadResult Upload(VideoUploadParams parameters)
        {
            return Upload<VideoUploadResult, VideoUploadParams>(parameters);
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
            string uri = m_api.ApiUrlV.Action(Constants.ACTION_NAME_UPLOAD).ResourceType(resourceType).BuildUrl();

            fileDescription.Reset();

            if (parameters == null)
                parameters = new SortedDictionary<string, object>();

            if (!(parameters is SortedDictionary<string, object>))
                parameters = new SortedDictionary<string, object>(parameters);

            return m_api.CallAndParse<RawUploadResult>(HttpMethod.POST, uri, (SortedDictionary<string, object>)parameters, fileDescription);
        }

        /// <summary>
        /// Gets a list of folders in the root.
        /// </summary>
        /// <returns>Parsed result of folders listing.</returns>
        public GetFoldersResult RootFolders()
        {
            return m_api.CallApi<GetFoldersResult>(HttpMethod.GET, m_api.ApiUrlV.Add("folders").BuildUrl(), null, null);
        }

        /// <summary>
        /// Gets a list of subfolders in a specified folder.
        /// </summary>
        /// <param name="folder">The folder name.</param>
        /// <returns>Parsed result of folders listing.</returns>
        public GetFoldersResult SubFolders(string folder)
        {
            if (String.IsNullOrEmpty(folder))
                throw new ArgumentException("folder must be set! Please use RootFolders() to get list of folders in root!");

            return m_api.CallApi<GetFoldersResult>(HttpMethod.GET, m_api.ApiUrlV.Add("folders").Add(folder).BuildUrl(), null, null);
        }

        /// <summary>
        /// Gets the Cloudinary account usage details.
        /// </summary>
        /// <returns>The report on the status of your Cloudinary account usage details.</returns>
        public UsageResult GetUsage()
        {
            string uri = m_api.ApiUrlV.Action("usage").BuildUrl();

            return m_api.CallApi<UsageResult>(HttpMethod.GET, uri, null, null);
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

        private string RandomPublicId()
        {
            byte[] buffer = new byte[8];
            m_random.NextBytes(buffer);
            return string.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
        }

        /// <summary>
        /// Uploads large file to Cloudinary by dividing it to chunks.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">
        /// Please use BasicRawUploadParams class for large raw file uploading!
        /// or
        /// The UploadLargeRaw method is intended to be used for large local file uploading and can't be used for remote file uploading!
        /// </exception>
        public RawUploadResult UploadLargeRaw(BasicRawUploadParams parameters, int bufferSize = DEFAULT_CHUNK_SIZE)
        {
            return UploadLarge<RawUploadResult>(parameters, bufferSize);
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
        /// Uploads large resources to Cloudinary by dividing it to chunks.
        /// </summary>
        /// <typeparam name="T">The type of result of upload.</typeparam>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <returns>Parsed result of uploading.</returns>
        public T UploadLarge<T>(BasicRawUploadParams parameters, int bufferSize = DEFAULT_CHUNK_SIZE) where T : UploadResult, new()
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters), "Upload parameters should be defined");

            if (parameters.File == null)
                throw new ArgumentNullException(nameof(parameters.File), "File parameter should be defined");

            if(parameters.File.IsRemote)
                return Upload<T, BasicRawUploadParams>(parameters);

            Url url = m_api.ApiUrlImgUpV;
            var name = Enum.GetName(typeof(ResourceType), parameters.ResourceType);
            if (name != null)
                url.ResourceType(name.ToLower());

            string uri = url.BuildUrl();

            parameters.File.Reset(bufferSize);

            var extraHeaders = new Dictionary<string, string>
            {
                ["X-Unique-Upload-Id"] = RandomPublicId()
            };


            var fileLength = parameters.File.GetFileLength();

            T result = null;

            while (!parameters.File.Eof)
            {
                var startOffset = parameters.File.BytesSent;
                var endOffset = startOffset + Math.Min(bufferSize, fileLength - startOffset) - 1;

                extraHeaders["Content-Range"] = $"bytes {startOffset}-{endOffset}/{fileLength}";

                result = m_api.CallApi<T>(HttpMethod.POST, uri, parameters, parameters.File, extraHeaders);

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    var error = result.Error != null ? result.Error.Message : "Unknown error";
                    throw new Exception(
                        $"An error has occured while uploading file (status code: {result.StatusCode}). {error}");
                }
            }

            return result;
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
            return Rename(new RenameParams(fromPublicId, toPublicId) { Overwrite = overwrite });
        }

        /// <summary>
        /// Changes public identifier of a file.
        /// </summary>
        /// <param name="parameters">Operation parameters.</param>
        /// <returns>Result of resource renaming.</returns>
        public RenameResult Rename(RenameParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.ResourceType(
                    Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType)).
                    Action("rename").BuildUrl();
            return m_api.CallApi<RenameResult>(HttpMethod.POST, uri, parameters, null);
        }

        /// <summary>
        /// Deletes file from Cloudinary.
        /// </summary>
        /// <param name="parameters">Parameters for deletion of resource from Cloudinary.</param>
        /// <returns>Results of deletion.</returns>
        public DeletionResult Destroy(DeletionParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.ResourceType(
                Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType)).
                Action("destroy").BuildUrl();

            return m_api.CallApi<DeletionResult>(HttpMethod.POST, uri, parameters, null);
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
        /// Lists resource types.
        /// </summary>
        /// <returns>Parsed list of resource types.</returns>
        public ListResourceTypesResult ListResourceTypes()
        {
            return m_api.CallApi<ListResourceTypesResult>(HttpMethod.GET, m_api.ApiUrlV.Add("resources").BuildUrl(), null, null);
        }

        /// <summary>
        /// Lists resources.
        /// </summary>
        /// <param name="nextCursor">Starting position.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResources(string nextCursor = null, bool tags = true, bool context = true, bool moderations = true)
        {
            return ListResources(new ListResourcesParams()
            {
                NextCursor = nextCursor,
                Tags = tags,
                Context = context,
                Moderations = moderations
            });
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
                NextCursor = nextCursor
            });
        }

        /// <summary>
        /// Lists resources by prefix.
        /// </summary>
        /// <param name="prefix">Public identifier prefix.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="type">Resource type.</param>
        /// <param name="moderations">If true, include moderation status for each resource.</param>
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
                NextCursor = nextCursor
            });
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
                NextCursor = nextCursor
            });
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
                PublicIds = new List<string>(publicIds)
            });
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
                Moderations = moderations
            });
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
        public ListResourcesResult ListResourcesByModerationStatus(string kind, ModerationStatus status, bool tags = true, bool context = true, bool moderations = true, string nextCursor = null)
        {
            return ListResources(new ListResourcesByModerationParams()
            {
                ModerationKind = kind,
                ModerationStatus = status,
                Tags = tags,
                Context = context,
                Moderations = moderations,
                NextCursor = nextCursor
            });
        }

        /// <summary>
        /// Gets a list of resources.
        /// </summary>
        /// <param name="parameters">Parameters to list resources.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResources(ListResourcesParams parameters)
        {
            var url = m_api.ApiUrlV.
                ResourceType("resources").
                Add(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType));

            if (parameters is ListResourcesByTagParams tagParams)
            {
                if (!String.IsNullOrEmpty(tagParams.Tag))
                    url.Add("tags").Add(tagParams.Tag);
            }

            if (parameters is ListResourcesByModerationParams modParams)
            {
                if (!String.IsNullOrEmpty(modParams.ModerationKind))
                {
                    url
                        .Add("moderations")
                        .Add(modParams.ModerationKind)
                        .Add(Api.GetCloudinaryParam<ModerationStatus>(modParams.ModerationStatus));
                }
            }

            UrlBuilder urlBuilder = new UrlBuilder(
                url.BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<ListResourcesResult>(HttpMethod.GET, urlBuilder.ToString(), parameters, null);
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
        /// Finds all tags that start with the given prefix.
        /// </summary>
        /// <param name="prefix">The tag prefix.</param>
        /// <returns>Parsed list of tags.</returns>
        public ListTagsResult ListTagsByPrefix(string prefix)
        {
            return ListTags(new ListTagsParams() { Prefix = prefix });
        }

        /// <summary>
        /// Gets a list of tags.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ListTagsResult ListTags(ListTagsParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                ResourceType("tags").
                Add(Api.GetCloudinaryParam(parameters.ResourceType)).
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<ListTagsResult>(HttpMethod.GET, urlBuilder.ToString(), parameters, null);
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
        /// Gets a list of transformations.
        /// </summary>
        /// <param name="parameters">Parameters of the request for a list of transformation.</param>
        /// <returns>Parsed list of transformations details.</returns>
        public ListTransformsResult ListTransformations(ListTransformsParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                ResourceType("transformations").
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<ListTransformsResult>(HttpMethod.GET, urlBuilder.ToString(), parameters, null);
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
        /// Gets details of a single transformation.
        /// </summary>
        /// <param name="parameters">Parameters of the request of transformation details.</param>
        /// <returns>Parsed details of a single transformation.</returns>
        public GetTransformResult GetTransform(GetTransformParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                ResourceType("transformations").
                Add(parameters.Transformation).
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<GetTransformResult>(HttpMethod.GET, urlBuilder.ToString(), parameters, null);
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
        /// Updates details of an existing resource.
        /// </summary>
        /// <param name="parameters">Parameters to update details of an existing resource.</param>
        /// <returns>Parsed response of the detailed resource information.</returns>
        public GetResourceResult UpdateResource(UpdateParams parameters)
        {
            var url = m_api.ApiUrlV.
                ResourceType("resources").
                Add(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType)).
                Add(parameters.Type).Add(parameters.PublicId).
                BuildUrl();

            return m_api.CallApi<GetResourceResult>(HttpMethod.POST, url, parameters, null);
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
        /// Gets details of the requested resource as well as all its derived resources.
        /// </summary>
        /// <param name="parameters">Parameters of the request of resource.</param>
        /// <returns>Parsed response with the detailed resource information.</returns>
        public GetResourceResult GetResource(GetResourceParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                ResourceType("resources").
                Add(Api.GetCloudinaryParam(parameters.ResourceType)).
                Add(parameters.Type).
                Add(parameters.PublicId).
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<GetResourceResult>(HttpMethod.GET, urlBuilder.ToString(), parameters, null);
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
        /// Deletes all derived resources with the given parameters.
        /// </summary>
        /// <param name="parameters">Parameters to delete derived resources.</param>
        /// <returns>Parsed result of deletion derived resources.</returns>
        public DelDerivedResResult DeleteDerivedResources(DelDerivedResParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                Add("derived_resources").
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<DelDerivedResResult>(HttpMethod.DELETE, urlBuilder.ToString(), parameters, null);
        }

        /// <summary>
        /// Deletes all resources of the given resource type and with the given public IDs.
        /// </summary>
        ///<param name="type">The type of file to delete. Default: image.</param>
        /// <param name="publicIds">Array of up to 100 public_ids.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        public DelResResult DeleteResources(ResourceType type, params string[] publicIds)
        {
            DelResParams p = new DelResParams() { ResourceType = type };
            p.PublicIds.AddRange(publicIds);
            return DeleteResources(p);
        }

        /// <summary>
        /// Deletes all resources with the given public IDs.
        /// </summary>
        /// <param name="publicIds">Array of up to 100 public_ids</param>
        /// <returns>Parsed result of deletion resources.</returns>
        public DelResResult DeleteResources(params string[] publicIds)
        {
            DelResParams p = new DelResParams();
            p.PublicIds.AddRange(publicIds);
            return DeleteResources(p);
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
        /// Deletes all resources.
        /// </summary>
        /// <returns>Parsed result of deletion resources.</returns>
        public DelResResult DeleteAllResources()
        {
            DelResParams p = new DelResParams() { All = true };
            return DeleteResources(p);
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
        /// Deletes all resources with parameters.
        /// </summary>
        /// <param name="parameters">Parameters for deletion resources.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        public DelResResult DeleteResources(DelResParams parameters)
        {
            Url url = m_api.ApiUrlV.
                Add("resources").
                Add(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType));

            url = string.IsNullOrEmpty(parameters.Tag)
                ? url.Add(parameters.Type)
                : url.Add("tags").Add(parameters.Tag);

            UrlBuilder urlBuilder = new UrlBuilder(url.BuildUrl(), parameters.ToParamsDictionary());

            return m_api.CallApi<DelResResult>(HttpMethod.DELETE, urlBuilder.ToString(), parameters, null);
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
        /// Restores a deleted resources.
        /// </summary>
        /// <param name="parameters">Parameters to restore a deleted resources.</param>
        /// <returns>Parsed result of restoring resources.</returns>
        public RestoreResult Restore(RestoreParams parameters)
        {
            var url = m_api.ApiUrlV.
                ResourceType("resources").
                Add(Api.GetCloudinaryParam(parameters.ResourceType)).
                Add("upload").
                Add("restore").BuildUrl();

            return m_api.CallApi<RestoreResult>(HttpMethod.POST, url, parameters, null);
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
        /// Returns list of all upload mappings.
        /// </summary>
        /// <param name="parameters">
        /// Uses only <see cref="UploadMappingParams.MaxResults"/> and <see cref="UploadMappingParams.NextCursor"/>
        /// properties. Can be null.
        /// </param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        public UploadMappingResults UploadMappings(UploadMappingParams parameters)
        {
            if (parameters == null)
                parameters = new UploadMappingParams();

            return CallUploadMappingsAPI(HttpMethod.GET, parameters);
        }

        /// <summary>
        /// Returns single upload mapping by <see cref="Folder"/> name.
        /// </summary>
        /// <param name="folder">Folder name.</param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        public UploadMappingResults UploadMapping(string folder)
        {
            if (string.IsNullOrEmpty(folder))
                throw new ArgumentException("Folder must be specified.");

            var parameters = new UploadMappingParams() { Folder = folder };

            return CallUploadMappingsAPI(HttpMethod.GET, parameters);
        }

        /// <summary>
        /// Creates a new upload mapping folder and its template (URL).
        /// </summary>
        /// <param name="folder">Folder name to create.</param>
        /// <param name="template">Url template for mapping to the <paramref name="folder"/>.</param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        public UploadMappingResults CreateUploadMapping(string folder, string template)
        {
            if (string.IsNullOrEmpty(folder))
                throw new ArgumentException("Folder property must be specified.");

            if (string.IsNullOrEmpty(template))
                throw new ArgumentException("Template must be specified.");

            var parameters = new UploadMappingParams()
            {
                Folder = folder,
                Template = template,
            };
            return CallUploadMappingsAPI(HttpMethod.POST, parameters);
        }

        /// <summary>
        /// Updates existing upload mapping.
        /// </summary>
        /// <param name="folder">Existing Folder to be updated.</param>
        /// <param name="newTemplate">New value of Template Url.</param>
        /// <returns>Parsed response after Upload mappings update.</returns>
        public UploadMappingResults UpdateUploadMapping(string folder, string newTemplate)
        {
            if (string.IsNullOrEmpty(folder))
                throw new ArgumentException("Folder must be specified.");

            if (string.IsNullOrEmpty(newTemplate))
                throw new ArgumentException("New Template name must be specified.");

            var parameters = new UploadMappingParams() { Folder = folder, Template = newTemplate };

            return CallUploadMappingsAPI(HttpMethod.PUT, parameters);
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
        /// Deletes upload mapping by <paramref name="folder"/> name.
        /// </summary>
        /// <param name="folder">Folder name.</param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        public UploadMappingResults DeleteUploadMapping(string folder)
        {
            var parameters = new UploadMappingParams();

            if (!string.IsNullOrEmpty(folder))
            {
                parameters.Folder = folder;
            }

            return CallUploadMappingsAPI(HttpMethod.DELETE, parameters);
        }

        /// <summary>
        /// Updates Cloudinary transformation resource.
        /// </summary>
        /// <param name="parameters">Parameters for transformation update.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        public UpdateTransformResult UpdateTransform(UpdateTransformParams parameters)
        {
            var url = m_api.ApiUrlV.
                ResourceType("transformations").
                Add(parameters.Transformation).
                BuildUrl();

            return m_api.CallApi<UpdateTransformResult>(HttpMethod.PUT, url, parameters, null);
        }

        /// <summary>
        /// Creates Cloudinary transformation resource.
        /// </summary>
        /// <param name="parameters">Parameters of the new transformation.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        public TransformResult CreateTransform(CreateTransformParams parameters)
        {
            var url = m_api.ApiUrlV.
                ResourceType("transformations").
                Add(parameters.Name).
                BuildUrl();

            return m_api.CallApi<TransformResult>(HttpMethod.POST, url, parameters, null);
        }

        /// <summary>
        /// Deletes transformation by name.
        /// </summary>
        /// <param name="transformName">The name of transformation to delete.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        public TransformResult DeleteTransform(string transformName)
        {
            var url = m_api.ApiUrlV.
                ResourceType("transformations").
                Add(transformName).
                BuildUrl();

            return m_api.CallApi<TransformResult>(HttpMethod.DELETE, url, null, null);
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
#if NET40
        public System.Web.IHtmlString GetCloudinaryJsConfig(bool directUpload = false, string dir = "")
#else
        public string GetCloudinaryJsConfig(bool directUpload = false, string dir = "")
#endif
        {
            if (String.IsNullOrEmpty(dir))
                dir = "/Scripts";

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
                    new JProperty("cloud_name",m_api.Account.Cloud),
                    new JProperty("api_key",m_api.Account.ApiKey),
                    new JProperty("private_cdn",m_api.UsePrivateCdn),
                    new JProperty("cdn_subdomain",m_api.CSubDomain)
                });

            if (!String.IsNullOrEmpty(m_api.PrivateCdn))
                cloudinaryParams.Add("secure_distribution", m_api.PrivateCdn);

            sb.AppendLine("<script type='text/javascript'>");
            sb.Append("$.cloudinary.config(");
            sb.Append(cloudinaryParams.ToString());
            sb.AppendLine(");");
            sb.AppendLine("</script>");

#if NET40
            return new System.Web.HtmlString(sb.ToString());
#else
            return sb.ToString();
#endif
        }

        private static void AppendScriptLine(StringBuilder sb, string dir, string script)
        {
            sb.Append("<script src=\"");
            sb.Append(dir);

            if (!dir.EndsWith("/") && !dir.EndsWith("\\"))
                sb.Append("/");

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
