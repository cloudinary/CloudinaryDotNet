using CloudinaryDotNet.Actions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CloudinaryDotNet
{
    /// <summary>
    /// Main class of cloudinary .NET API.
    /// </summary>
    public partial class Cloudinary
    {
        public const string CF_SHARED_CDN = "d3jpl91pxevbkh.cloudfront.net";
        public const string OLD_AKAMAI_SHARED_CDN = "cloudinary-a.akamaihd.net";
        public const string AKAMAI_SHARED_CDN = "res.cloudinary.com";
        public const string SHARED_CDN = AKAMAI_SHARED_CDN;
        private const string RESOURCE_TYPE_IMAGE = "image";
        private const string ACTION_GENERATE_ARCHIVE = "generate_archive";


        Api m_api;

        /// <summary>
        /// Default parameterless constructor.
        /// Assumes that environment variable CLOUDINARY_URL is set.
        /// </summary>
        public Cloudinary()
        {
            m_api = new Api();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="cloudinaryUrl">Cloudinary URL</param>
        public Cloudinary(string cloudinaryUrl)
        {
            m_api = new Api(cloudinaryUrl);
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="account">Cloudinary account</param>
        public Cloudinary(Account account)
        {
            m_api = new Api(account);
        }

        /// <summary>
        /// API object that used by this instance
        /// </summary>
        public Api Api
        {
            get { return m_api; }
        }

        /// <summary>
        /// This method can be used to force refresh facebook and twitter profile pictures. The response of this method includes the image's version. Use this version to bypass previously cached CDN copies.
        /// Also it can be used to generate transformed versions of an uploaded image. This is useful when Strict Transformations are allowed for your account and you wish to create custom derived images for already uploaded images. 
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public ExplicitResult Explicit(ExplicitParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.Action("explicit").BuildUrl();

            using (HttpWebResponse response = m_api.Call(HttpMethod.POST, uri, parameters.ToParamsDictionary(), null))
            {
                return ExplicitResult.Parse(response);
            }
        }

        /// <summary>
        /// Creates the upload preset.
        /// Upload presets allow you to define the default behavior for your uploads, instead of receiving these as parameters during the upload request itself. Upload presets have precedence over client-side upload parameters.
        /// </summary>
        /// <param name="parameters">Parameters of the upload preset.</param>
        /// <returns></returns>
        public UploadPresetResult CreateUploadPreset(UploadPresetParams parameters)
        {
            string url = m_api.ApiUrlV.
                Add("upload_presets").
                BuildUrl();

            using (HttpWebResponse response = m_api.Call(HttpMethod.POST, url, parameters.ToParamsDictionary(), null))
            {
                return UploadPresetResult.Parse(response);
            }
        }

        /// <summary>
        /// Updates the upload preset.
        /// Every update overwrites all the preset settings.
        /// </summary>
        /// <param name="parameters">New parameters for upload preset.</param>
        /// <returns></returns>
        public UploadPresetResult UpdateUploadPreset(UploadPresetParams parameters)
        {
            var @params = parameters.ToParamsDictionary();
            @params.Remove("name");

            var url = m_api.ApiUrlV
                .Add("upload_presets")
                .Add(parameters.Name)
                .BuildUrl();

            using (HttpWebResponse response = m_api.Call(HttpMethod.PUT, url, @params, null))
            {
                return UploadPresetResult.Parse(response);
            }
        }

        /// <summary>
        /// Gets the upload preset.
        /// </summary>
        /// <param name="name">Name of the upload preset.</param>
        /// <returns></returns>
        public GetUploadPresetResult GetUploadPreset(string name)
        {
            var url = m_api.ApiUrlV
                .Add("upload_presets")
                .Add(name)
                .BuildUrl();

            using (HttpWebResponse response = m_api.Call(HttpMethod.GET, url, null, null))
            {
                return GetUploadPresetResult.Parse(response);
            }
        }

        /// <summary>
        /// Lists upload presets.
        /// </summary>
        /// <returns></returns>
        public ListUploadPresetsResult ListUploadPresets(string nextCursor = null)
        {
            return ListUploadPresets(new ListUploadPresetsParams() { NextCursor = nextCursor });
        }

        /// <summary>
        /// Lists upload presets.
        /// </summary>
        /// <returns></returns>
        public ListUploadPresetsResult ListUploadPresets(ListUploadPresetsParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV
                .Add("upload_presets")
                .BuildUrl(),
                parameters.ToParamsDictionary());

            using (HttpWebResponse response = m_api.Call(HttpMethod.GET, urlBuilder.ToString(), null, null))
            {
                return ListUploadPresetsResult.Parse(response);
            }
        }

        /// <summary>
        /// Deletes the upload preset.
        /// </summary>
        /// <param name="name">Name of the upload preset.</param>
        /// <returns></returns>
        public DeleteUploadPresetResult DeleteUploadPreset(string name)
        {
            var url = m_api.ApiUrlV
                .Add("upload_presets")
                .Add(name)
                .BuildUrl();

            using (HttpWebResponse response = m_api.Call(HttpMethod.DELETE, url, null, null))
            {
                return DeleteUploadPresetResult.Parse(response);
            }
        }

        /// <summary>
        /// Uploads an image file to cloudinary.
        /// </summary>
        /// <param name="parameters">Parameters of image uploading .</param>
        /// <returns>Results of image uploading.</returns>
        public ImageUploadResult Upload(ImageUploadParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.BuildUrl();

            ResetInternalFileDescription(parameters.File);

            using (HttpWebResponse response = m_api.Call(HttpMethod.POST, uri, parameters.ToParamsDictionary(), parameters.File))
            {
                return ImageUploadResult.Parse(response);
            }
        }

        /// <summary>
        /// Uploads a video file to cloudinary.
        /// </summary>
        /// <param name="parameters">Parameters of video uploading .</param>
        /// <returns>Results of video uploading.</returns>
        public VideoUploadResult Upload(VideoUploadParams parameters)
        {
            string uri = m_api.ApiUrlVideoUpV.BuildUrl();

            ResetInternalFileDescription(parameters.File);

            using (HttpWebResponse response = m_api.Call(HttpMethod.POST, uri, parameters.ToParamsDictionary(), parameters.File))
            {
                return VideoUploadResult.Parse(response);
            }
        }

        /// <summary>
        /// Uploads a file to cloudinary.
        /// </summary>
        /// <param name="resourceType">Resource type ("image", "raw", "video", "auto")</param>
        /// <param name="parameters">Upload parameters.</param>
        /// <param name="fileDescription">File description.</param>
        public RawUploadResult Upload(string resourceType, IDictionary<string, object> parameters, FileDescription fileDescription)
        {
            string uri = m_api.ApiUrlV.Action("upload").ResourceType(resourceType).BuildUrl();

            ResetInternalFileDescription(fileDescription);

            if (parameters == null)
                parameters = new SortedDictionary<string, object>();

            if (!(parameters is SortedDictionary<string, object>))
                parameters = new SortedDictionary<string, object>(parameters);

            using (HttpWebResponse response = m_api.Call(HttpMethod.POST, uri, (SortedDictionary<string, object>)parameters, fileDescription))
            {
                return RawUploadResult.Parse(response);
            }
        }

        /// <summary>
        /// Gets list of folders in the root.
        /// </summary>
        public GetFoldersResult RootFolders()
        {
            using (HttpWebResponse response = m_api.Call(
                HttpMethod.GET, m_api.ApiUrlV.Add("folders").BuildUrl(), null, null))
            {
                return GetFoldersResult.Parse(response);
            }
        }

        /// <summary>
        /// Get list of subfolders in a specified folder.
        /// </summary>
        public GetFoldersResult SubFolders(string folder)
        {
            if (String.IsNullOrEmpty(folder))
                throw new ArgumentException("folder must be set! Please use RootFolders() to get list of folders in root!");

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.GET, m_api.ApiUrlV.Add("folders").Add(folder).BuildUrl(), null, null))
            {
                return GetFoldersResult.Parse(response);
            }
        }

        /// <summary>
        /// Gets URL to download private image
        /// </summary>
        /// <param name="publicId">The image public ID.</param>
        /// <param name="attachment">Whether to download image as attachment (optional).</param>
        /// <param name="format">Format to download (optional).</param>
        /// <param name="type">The type (optional).</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">publicId can't be null</exception>
        public string DownloadPrivate(string publicId, bool? attachment = null, string format = "", string type = "")
        {
            if (String.IsNullOrEmpty(publicId))
                throw new ArgumentException("publicId");

            UrlBuilder urlBuilder = new UrlBuilder(
               m_api.ApiUrlV
               .ResourceType(RESOURCE_TYPE_IMAGE)
               .Action("download")
               .BuildUrl());

            var parameters = new SortedDictionary<string, object>();

            parameters.Add("public_id", publicId);

            if (!String.IsNullOrEmpty(format))
                parameters.Add("format", format);

            if (attachment != null)
                parameters.Add("attachment", (bool)attachment ? "true" : "false");

            if (!String.IsNullOrEmpty(type))
                parameters.Add("type", type);

            return GetDownloadUrl(urlBuilder, parameters);
        }

        /// <summary>
        /// Gets URL to download tag cloud as ZIP package
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

            var parameters = new SortedDictionary<string, object>();

            parameters.Add("tag", tag);

            if (transform != null)
                parameters.Add("transformation", transform.Generate());

            return GetDownloadUrl(urlBuilder, parameters);
        }

        public UsageResult GetUsage()
        {
            string uri = m_api.ApiUrlV.Action("usage").BuildUrl();

            using (HttpWebResponse response = m_api.Call(HttpMethod.GET, uri, null, null))
            {
                return UsageResult.Parse(response);
            }
        }

        /// <summary>
        /// Uploads a file to cloudinary.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="type">The type ("raw" or "auto", last by default).</param>
        public RawUploadResult Upload(RawUploadParams parameters, string type = "auto")
        {
            string uri = m_api.ApiUrlImgUpV.ResourceType(type).BuildUrl();

            ResetInternalFileDescription(parameters.File);

            using (HttpWebResponse response = m_api.Call(HttpMethod.POST, uri, parameters.ToParamsDictionary(), parameters.File))
            {
                return RawUploadResult.Parse(response);
            }
        }

        /// <summary>
        /// Uploads large file to cloudinary by dividing it to chunks.
        /// </summary>
        /// <param name="parameters">Parameters of file uploading.</param>
        /// <param name="bufferSize">Chunk (buffer) size (20 MB by default).</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">
        /// Please use BasicRawUploadParams class for large raw file uploading!
        /// or
        /// The UploadLargeRaw method is intended to be used for large local file uploading and can't be used for remote file uploading!
        /// </exception>
        public RawUploadResult UploadLargeRaw(BasicRawUploadParams parameters, int bufferSize = 20 * 1024 * 1024)
        {
            if (parameters is RawUploadParams)
                throw new ArgumentException("Please use BasicRawUploadParams class for large raw file uploading!");

            parameters.Check();

            if (parameters.File.IsRemote)
                throw new ArgumentException("The UploadLargeRaw method is intended to be used for large local file uploading and can't be used for remote file uploading!");

            string uri = m_api.ApiUrlV.Action("upload_large").ResourceType("raw").BuildUrl();

            ResetInternalFileDescription(parameters.File, bufferSize);

            int partNumber = 1;
            string publicId = null;
            string uploadId = null;

            RawUploadResult result = null;

            while (!parameters.File.EOF)
            {
                var dict = parameters.ToParamsDictionary();

                dict.Add("part_number", partNumber);

                if (partNumber > 1)
                {
                    dict["public_id"] = publicId;
                    dict["upload_id"] = uploadId;
                }

                if (parameters.File.IsLastPart())
                    dict["final"] = true;

                using (HttpWebResponse response = m_api.Call(HttpMethod.POST, uri, dict, parameters.File))
                {
                    var partResult = RawPartUploadResult.Parse(response);
                    result = partResult;

                    if (result.StatusCode != HttpStatusCode.OK)
                        throw new WebException(String.Format(
                            "An error has occured while uploading file (status code: {0}). {1}",
                            partResult.StatusCode,
                            partResult.Error != null ? partResult.Error.Message : "Unknown error"));

                    if (partNumber == 1)
                    {
                        publicId = partResult.PublicId;
                        uploadId = partResult.UploadId;
                    }

                    partNumber++;
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
        /// <returns></returns>
        public RenameResult Rename(string fromPublicId, string toPublicId, bool overwrite = false)
        {
            return Rename(new RenameParams(fromPublicId, toPublicId) { Overwrite = overwrite });
        }

        /// <summary>
        /// Changes public identifier of a file.
        /// </summary>
        /// <param name="parameters">Operation parameters.</param>
        /// <returns></returns>
        public RenameResult Rename(RenameParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.Action("rename").BuildUrl();

            using (HttpWebResponse response = m_api.Call(HttpMethod.POST, uri, parameters.ToParamsDictionary(), null))
            {
                return RenameResult.Parse(response);
            }
        }

        /// <summary>
        /// Delete file from cloudinary
        /// </summary>
        /// <param name="parameters">Parameters for deletion of resource from cloudinary</param>
        /// <returns>Results of deletion</returns>
        public DeletionResult Destroy(DeletionParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.ResourceType(
                Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType)).
                Action("destroy").BuildUrl();

            using (HttpWebResponse response = m_api.Call(HttpMethod.POST, uri, parameters.ToParamsDictionary(), null))
            {
                DeletionResult result = DeletionResult.Parse(response);
                return result;
            }
        }

        /// <summary>
        /// Generate an image of a given textual string
        /// </summary>
        /// <param name="text">Text to draw</param>
        /// <returns>Results of generating an image of a given textual string</returns>
        public TextResult Text(string text)
        {
            return Text(new TextParams(text));
        }

        /// <summary>
        /// Generate an image of a given textual string
        /// </summary>
        /// <param name="parameters">Parameters of generating an image of a given textual string</param>
        /// <returns>Results of generating an image of a given textual string</returns>
        public TextResult Text(TextParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.Action("text").BuildUrl();

            using (HttpWebResponse response = m_api.Call(HttpMethod.POST, uri, parameters.ToParamsDictionary(), null))
            {
                TextResult result = TextResult.Parse(response);
                return result;
            }
        }

        /// <summary>
        /// Manage tag assignments
        /// </summary>
        /// <param name="parameters">Parameters of tag management</param>
        /// <returns>Results of tags management</returns>
        public TagResult Tag(TagParams parameters)
        {
            string uri = m_api.ApiUrlImgUpV.Action("tags").BuildUrl();

            using (HttpWebResponse response = m_api.Call(HttpMethod.POST, uri, parameters.ToParamsDictionary(), null))
            {
                TagResult result = TagResult.Parse(response);
                return result;
            }
        }

        /// <summary>
        /// Lists resource types.
        /// </summary>
        public ListResourceTypesResult ListResourceTypes()
        {
            using (HttpWebResponse response = m_api.Call(
                HttpMethod.GET, m_api.ApiUrlV.Add("resources").BuildUrl(), null, null))
            {
                ListResourceTypesResult result = ListResourceTypesResult.Parse(response);
                return result;
            }
        }

        /// <summary>
        /// Lists resources.
        /// </summary>
        /// <param name="nextCursor">Starting position.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// Lists resources.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public ListResourcesResult ListResources(ListResourcesParams parameters)
        {
            var url = m_api.ApiUrlV.
                ResourceType("resources").
                Add(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType));

            if (parameters is ListResourcesByTagParams)
            {
                var tagParams = (ListResourcesByTagParams)parameters;
                if (!String.IsNullOrEmpty(tagParams.Tag))
                    url.Add("tags").Add(tagParams.Tag);
            }

            if (parameters is ListResourcesByModerationParams)
            {
                var modParams = (ListResourcesByModerationParams)parameters;

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

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.GET, urlBuilder.ToString(), null, null))
            {
                ListResourcesResult result = ListResourcesResult.Parse(response);
                return result;
            }
        }

        public ListTagsResult ListTags()
        {
            return ListTags(new ListTagsParams());
        }

        public ListTagsResult ListTagsByPrefix(string prefix)
        {
            return ListTags(new ListTagsParams() { Prefix = prefix });
        }

        public ListTagsResult ListTags(ListTagsParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                ResourceType("tags").
                Add(Api.GetCloudinaryParam(parameters.ResourceType)).
                BuildUrl(),
                parameters.ToParamsDictionary());

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.GET, urlBuilder.ToString(), null, null))
            {
                ListTagsResult result = ListTagsResult.Parse(response);
                return result;
            }
        }

        public ListTransformsResult ListTransformations()
        {
            return ListTransformations(new ListTransformsParams());
        }

        public ListTransformsResult ListTransformations(ListTransformsParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                ResourceType("transformations").
                BuildUrl(),
                parameters.ToParamsDictionary());

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.GET, urlBuilder.ToString(), null, null))
            {
                ListTransformsResult result = ListTransformsResult.Parse(response);
                return result;
            }
        }

        public GetTransformResult GetTransform(string transform)
        {
            return GetTransform(new GetTransformParams() { Transformation = transform });
        }

        public GetTransformResult GetTransform(GetTransformParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                ResourceType("transformations").
                Add(parameters.Transformation).
                BuildUrl(),
                parameters.ToParamsDictionary());

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.GET, urlBuilder.ToString(), null, null))
            {
                GetTransformResult result = GetTransformResult.Parse(response);
                return result;
            }
        }

        public GetResourceResult UpdateResource(string publicId, ModerationStatus moderationStatus)
        {
            return UpdateResource(new UpdateParams(publicId) { ModerationStatus = moderationStatus });
        }

        public GetResourceResult UpdateResource(UpdateParams parameters)
        {
            var url = m_api.ApiUrlV.
                ResourceType("resources").
                Add(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType)).
                Add(parameters.Type).Add(parameters.PublicId).
                BuildUrl();

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.POST, url, parameters.ToParamsDictionary(), null))
            {
                return GetResourceResult.Parse(response);
            }
        }

        public GetResourceResult GetResource(string publicId)
        {
            return GetResource(new GetResourceParams(publicId));
        }

        public GetResourceResult GetResource(GetResourceParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                ResourceType("resources").
                Add(Api.GetCloudinaryParam(parameters.ResourceType)).
                Add(parameters.Type).Add(parameters.PublicId).
                BuildUrl(),
                parameters.ToParamsDictionary());

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.GET, urlBuilder.ToString(), null, null))
            {
                GetResourceResult result = GetResourceResult.Parse(response);
                return result;
            }
        }

        public DelDerivedResResult DeleteDerivedResources(params string[] ids)
        {
            DelDerivedResParams p = new DelDerivedResParams();
            p.DerivedResources.AddRange(ids);
            return DeleteDerivedResources(p);
        }

        public DelDerivedResResult DeleteDerivedResources(DelDerivedResParams parameters)
        {
            UrlBuilder urlBuilder = new UrlBuilder(
                m_api.ApiUrlV.
                Add("derived_resources").
                BuildUrl(),
                parameters.ToParamsDictionary());

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.DELETE, urlBuilder.ToString(), null, null))
            {
                DelDerivedResResult result = DelDerivedResResult.Parse(response);
                return result;
            }
        }

        public DelResResult DeleteResources(ResourceType type, params string[] publicIds)
        {
            DelResParams p = new DelResParams() { ResourceType = type };
            p.PublicIds.AddRange(publicIds);
            return DeleteResources(p);
        }

        public DelResResult DeleteResources(params string[] publicIds)
        {
            DelResParams p = new DelResParams();
            p.PublicIds.AddRange(publicIds);
            return DeleteResources(p);
        }

        public DelResResult DeleteResourcesByPrefix(string prefix)
        {
            DelResParams p = new DelResParams() { Prefix = prefix };
            return DeleteResources(p);
        }

        public DelResResult DeleteResourcesByPrefix(string prefix, bool keepOriginal, string nextCursor)
        {
            DelResParams p = new DelResParams() { Prefix = prefix, KeepOriginal = keepOriginal, NextCursor = nextCursor };
            return DeleteResources(p);
        }

        public DelResResult DeleteResourcesByTag(string tag)
        {
            DelResParams p = new DelResParams() { Tag = tag };
            return DeleteResources(p);
        }

        public DelResResult DeleteResourcesByTag(string tag, bool keepOriginal, string nextCursor)
        {
            DelResParams p = new DelResParams() { Tag = tag, KeepOriginal = keepOriginal, NextCursor = nextCursor };
            return DeleteResources(p);
        }

        public DelResResult DeleteAllResources()
        {
            DelResParams p = new DelResParams() { All = true };
            return DeleteResources(p);
        }

        public DelResResult DeleteAllResources(bool keepOriginal, string nextCursor)
        {
            DelResParams p = new DelResParams() { All = true, KeepOriginal = keepOriginal, NextCursor = nextCursor };
            return DeleteResources(p);
        }

        public DelResResult DeleteResources(DelResParams parameters)
        {
            Url url = m_api.ApiUrlV.
                Add("resources").
                Add(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType));

            url = string.IsNullOrEmpty(parameters.Tag)
                ? url.Add(parameters.Type)
                : url.Add("tags").Add(parameters.Tag);

            UrlBuilder urlBuilder = new UrlBuilder(url.BuildUrl(), parameters.ToParamsDictionary());

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.DELETE, urlBuilder.ToString(), null, null))
            {
                DelResResult result = DelResResult.Parse(response);
                return result;
            }
        }

        public RestoreResult Restore(params string[] publicIds)
        {
            RestoreParams restoreParams = new RestoreParams();
            restoreParams.PublicIds.AddRange(publicIds);
            return Restore(restoreParams);
        }

        public RestoreResult Restore(RestoreParams parameters)
        {
            var url = m_api.ApiUrlV.
                ResourceType("resources").
                Add(Api.GetCloudinaryParam(parameters.ResourceType)).
                Add("upload").
                Add("restore").BuildUrl();

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.POST, url, parameters.ToParamsDictionary(), null))
            {
                RestoreResult result = RestoreResult.Parse(response);
                return result;
            }
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
        /// Calls an API method and returns a parsed result
        /// </summary>
        private UploadMappingResults CallUploadMappingsAPI(HttpMethod httpMethod, UploadMappingParams parameters)
        {
            string url;
            SortedDictionary<string, object> body = null;
            if (httpMethod == HttpMethod.POST || httpMethod == HttpMethod.PUT)
            {
                url = GetUploadMappingUrl();
                if (parameters != null)
                {
                    body = parameters.ToParamsDictionary();
                }
            }
            else
            {
                url = GetUploadMappingUrl(parameters);
            }
            using (HttpWebResponse response = m_api.Call(httpMethod, url, body, null))
            {
                UploadMappingResults result = UploadMappingResults.Parse(response);
                return result;
            }
        }

        /// <summary>
        /// Returns list of all upload mappings
        /// </summary>
        /// <param name="parameters">Uses only <see cref="MaxResults"/> and <see cref="NextCursor"/> properties. Can be null.</param>
        public UploadMappingResults UploadMappings(UploadMappingParams parameters)
        {
            if (parameters == null)
                parameters = new UploadMappingParams();

            return CallUploadMappingsAPI(HttpMethod.GET, parameters);
        }

        /// <summary>
        /// Returns single upload mapping by <see cref="Folder"/> name.
        /// </summary>
        public UploadMappingResults UploadMapping(string folder)
        {
            if (string.IsNullOrEmpty(folder))
                throw new ArgumentException("Folder must be specified.");

            var parameters = new UploadMappingParams() { Folder = folder };

            return CallUploadMappingsAPI(HttpMethod.GET, parameters);
        }

        /// <summary>
        /// Create a new upload mapping folder and its template (URL).
        /// </summary>
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
        /// Updates existing upload mapping
        /// </summary>
        /// <param name="folder">Existing Folder to be updated</param>
        /// <param name="newTemplate">New value of Template Url</param>
        /// <returns></returns>
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
        /// Deletes all upload mappings 
        /// </summary>
        public UploadMappingResults DeleteUploadMapping()
        {
            return DeleteUploadMapping(string.Empty);
        }

        /// <summary>
        /// Deletes upload mapping by <paramref name="folder"/> name
        /// </summary>
        public UploadMappingResults DeleteUploadMapping(string folder)
        {
            var parameters = new UploadMappingParams();

            if (!string.IsNullOrEmpty(folder))
            {
                parameters.Folder = folder;
            }

            return CallUploadMappingsAPI(HttpMethod.DELETE, parameters);
        }

        public UpdateTransformResult UpdateTransform(UpdateTransformParams parameters)
        {
            var url = m_api.ApiUrlV.
                ResourceType("transformations").
                Add(parameters.Transformation).
                BuildUrl();

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.PUT, url, parameters.ToParamsDictionary(), null))
            {
                UpdateTransformResult result = UpdateTransformResult.Parse(response);
                return result;
            }
        }

        public TransformResult CreateTransform(CreateTransformParams parameters)
        {
            var url = m_api.ApiUrlV.
                ResourceType("transformations").
                Add(parameters.Name).
                BuildUrl();

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.POST, url, parameters.ToParamsDictionary(), null))
            {
                TransformResult result = TransformResult.Parse(response);
                return result;
            }
        }

        public TransformResult DeleteTransform(string transformName)
        {
            var url = m_api.ApiUrlV.
                ResourceType("transformations").
                Add(transformName).
                BuildUrl();

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.DELETE, url, null, null))
            {
                TransformResult result = TransformResult.Parse(response);
                return result;
            }
        }

        /// <summary>
        /// Eagerly generate sprites
        /// </summary>
        /// <param name="parameters">Parameters for sprite generation</param>
        /// <returns>Result of sprite generation</returns>
        public SpriteResult MakeSprite(SpriteParams parameters)
        {
            var url = m_api.ApiUrlImgUpV.
                Action("sprite").
                BuildUrl();

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.POST, url, parameters.ToParamsDictionary(), null))
            {
                SpriteResult result = SpriteResult.Parse(response);
                return result;
            }
        }

        /// <summary>
        /// Allows multi transformation
        /// </summary>
        /// <param name="parameters">Parameters of operation</param>
        /// <returns>Result of operation</returns>
        public MultiResult Multi(MultiParams parameters)
        {
            var url = m_api.ApiUrlImgUpV.
                Action("multi").
                BuildUrl();

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.POST, url, parameters.ToParamsDictionary(), null))
            {
                MultiResult result = MultiResult.Parse(response);
                return result;
            }
        }

        /// <summary>
        /// Explode multipage document to single pages
        /// </summary>
        /// <param name="parameters">Parameters of explosion</param>
        /// <returns>Result of operation</returns>
        public ExplodeResult Explode(ExplodeParams parameters)
        {
            var url = m_api.ApiUrlImgUpV.
                Action("explode").
                BuildUrl();

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.POST, url, parameters.ToParamsDictionary(), null))
            {
                ExplodeResult result = ExplodeResult.Parse(response);
                return result;
            }
        }



        /// <summary>
        /// Create archive and store it as a raw resource in your Cloudinary
        /// </summary>
        /// <param name="parameters">Parameters of new generated archive</param>
        /// <returns>Result of operation</returns>
        public ArchiveResult CreateArchive(ArchiveParams parameters)
        {
            var url = m_api.ApiUrlV.
                ResourceType(RESOURCE_TYPE_IMAGE).
                Action(ACTION_GENERATE_ARCHIVE).
                BuildUrl();

            parameters.Mode(ArchiveCallMode.Create);

            using (HttpWebResponse response = m_api.Call(
                HttpMethod.POST, url, parameters.ToParamsDictionary(), null))
            {
                return ArchiveResult.Parse(response);
            }
        }

        /// <summary>
        ///  Return Url on archive file
        /// </summary>
        /// <param name="parameters">Parameters of generated archive</param>
        /// <returns>Url on archive file</returns>
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
        /// Gets java script that configures Cloudinary JS.
        /// </summary>
        /// <param name="directUpload">Whether to reference additional scripts that are necessary for uploading files directly from browser.</param>
        /// <param name="dir">Override location of js files (default: ~/Scripts).</param>
        /// <returns></returns>
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

        private static void ResetInternalFileDescription(FileDescription file, int bufferSize = Int32.MaxValue)
        {
            file.BufferLength = bufferSize;
            file.EOF = false;
            file.BytesSent = 0;
        }
    }
}
