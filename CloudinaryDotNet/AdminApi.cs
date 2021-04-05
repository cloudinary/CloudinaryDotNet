namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;

    /// <summary>
    /// Main class for accessing Cloudinary Admin .NET API.
    /// </summary>
    internal partial class AdminApi : IAdminApi
    {
        private Api m_api;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminApi"/> class.
        /// Default parameterless constructor. Assumes that environment variable CLOUDINARY_URL is set.
        /// </summary>
        public AdminApi()
        {
            m_api = new Api();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminApi"/> class with Cloudinary URL.
        /// </summary>
        /// <param name="cloudinaryUrl">Cloudinary URL.</param>
        public AdminApi(string cloudinaryUrl)
        {
            m_api = new Api(cloudinaryUrl);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminApi"/> class with Cloudinary account.
        /// </summary>
        /// <param name="account">Cloudinary account.</param>
        public AdminApi(Account account)
        {
            m_api = new Api(account);
        }

        /// <inheritdoc/>
        public void SetApiBaseAddress(string value)
        {
            m_api.ApiBaseAddress = value;
        }

        /// <inheritdoc/>
        public Task<GetFoldersResult> RootFoldersAsync(GetFoldersParams parameters = null, CancellationToken? cancellationToken = null)
        {
            return m_api.CallApiAsync<GetFoldersResult>(
                HttpMethod.GET,
                GetFolderUrl(parameters: parameters),
                null,
                null,
                cancellationToken: cancellationToken);
        }

        /// <inheritdoc/>
        public GetFoldersResult RootFolders(GetFoldersParams parameters = null)
        {
            return m_api.CallApi<GetFoldersResult>(
                HttpMethod.GET,
                GetFolderUrl(parameters: parameters),
                null,
                null);
        }

        /// <inheritdoc/>
        public Task<GetFoldersResult> SubFoldersAsync(string folder, CancellationToken? cancellationToken = null)
        {
            return SubFoldersAsync(folder, null, cancellationToken);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public GetFoldersResult SubFolders(string folder, GetFoldersParams parameters = null)
        {
            CheckFolderParameter(folder);

            return m_api.CallApi<GetFoldersResult>(
                HttpMethod.GET,
                GetFolderUrl(folder, parameters),
                null,
                null);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public DeleteFolderResult DeleteFolder(string folder)
        {
            var uri = GetFolderUrl(folder);
            return m_api.CallApi<DeleteFolderResult>(HttpMethod.DELETE, uri, null, null);
        }

        /// <inheritdoc/>
        public CreateFolderResult CreateFolder(string folder)
        {
            CheckIfNotEmpty(folder);

            return m_api.CallApi<CreateFolderResult>(
                HttpMethod.POST,
                GetFolderUrl(folder),
                null,
                null);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public Task<UploadPresetResult> CreateUploadPresetAsync(UploadPresetParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = GetApiUrlV().
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

        /// <inheritdoc/>
        public UploadPresetResult CreateUploadPreset(UploadPresetParams parameters)
        {
            var url = GetApiUrlV().
                Add("upload_presets").
                BuildUrl();

            return m_api.CallApi<UploadPresetResult>(HttpMethod.POST, url, parameters, null);
        }

        /// <inheritdoc/>
        public Task<UploadPresetResult> UpdateUploadPresetAsync(UploadPresetParams parameters, CancellationToken? cancellationToken = null) =>
            CallApiAsync<UploadPresetResult>(PrepareUploadPresetApiParams(parameters), cancellationToken);

        /// <inheritdoc/>
        public UploadPresetResult UpdateUploadPreset(UploadPresetParams parameters) =>
            CallApi<UploadPresetResult>(PrepareUploadPresetApiParams(parameters));

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public GetUploadPresetResult GetUploadPreset(string name)
        {
            var url = GetApiUrlV()
                .Add("upload_presets")
                .Add(name)
                .BuildUrl();

            return m_api.CallApi<GetUploadPresetResult>(HttpMethod.GET, url, null, null);
        }

        /// <inheritdoc/>
        public Task<ListUploadPresetsResult> ListUploadPresetsAsync(string nextCursor = null, CancellationToken? cancellationToken = null)
        {
            return ListUploadPresetsAsync(new ListUploadPresetsParams() { NextCursor = nextCursor }, cancellationToken);
        }

        /// <inheritdoc/>
        public ListUploadPresetsResult ListUploadPresets(string nextCursor = null)
        {
            return ListUploadPresets(new ListUploadPresetsParams() { NextCursor = nextCursor });
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public ListUploadPresetsResult ListUploadPresets(ListUploadPresetsParams parameters)
        {
            var urlBuilder = new UrlBuilder(
                GetApiUrlV()
                .Add("upload_presets")
                .BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<ListUploadPresetsResult>(HttpMethod.GET, urlBuilder.ToString(), parameters, null);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public DeleteUploadPresetResult DeleteUploadPreset(string name)
        {
            var url = GetApiUrlV()
                .Add("upload_presets")
                .Add(name)
                .BuildUrl();

            return m_api.CallApi<DeleteUploadPresetResult>(HttpMethod.DELETE, url, null, null);
        }

        /// <inheritdoc/>
        public Task<UsageResult> GetUsageAsync(CancellationToken? cancellationToken = null)
        {
            var uri = GetUsageUrl(null);

            return m_api.CallApiAsync<UsageResult>(
                HttpMethod.GET,
                uri,
                null,
                null,
                null,
                cancellationToken);
        }

        /// <inheritdoc/>
        public Task<UsageResult> GetUsageAsync(DateTime? date, CancellationToken? cancellationToken = null)
        {
            var uri = GetUsageUrl(date);

            return m_api.CallApiAsync<UsageResult>(
                HttpMethod.GET,
                uri,
                null,
                null,
                null,
                cancellationToken);
        }

        /// <inheritdoc/>
        public UsageResult GetUsage(DateTime? date = null)
        {
            var uri = GetUsageUrl(date);

            return m_api.CallApi<UsageResult>(HttpMethod.GET, uri, null, null);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public ListResourceTypesResult ListResourceTypes()
        {
            return m_api.CallApi<ListResourceTypesResult>(HttpMethod.GET, GetApiUrlV().Add("resources").BuildUrl(), null, null);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public Task<ListResourcesResult> ListResourcesByTypeAsync(string type, string nextCursor = null, CancellationToken? cancellationToken = null)
        {
            return ListResourcesAsync(new ListResourcesParams() { Type = type, NextCursor = nextCursor }, cancellationToken);
        }

        /// <inheritdoc/>
        public ListResourcesResult ListResourcesByType(string type, string nextCursor = null)
        {
            return ListResources(new ListResourcesParams() { Type = type, NextCursor = nextCursor });
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public ListResourcesResult ListResourcesByPrefix(string prefix, string type = "upload", string nextCursor = null)
        {
            return ListResources(new ListResourcesByPrefixParams()
            {
                Type = type,
                Prefix = prefix,
                NextCursor = nextCursor,
            });
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public Task<ListResourcesResult> ListResourcesByTagAsync(string tag, string nextCursor = null, CancellationToken? cancellationToken = null)
        {
            var listResourcesByTagParams = new ListResourcesByTagParams()
            {
                Tag = tag,
                NextCursor = nextCursor,
            };
            return ListResourcesAsync(listResourcesByTagParams, cancellationToken);
        }

        /// <inheritdoc/>
        public ListResourcesResult ListResourcesByTag(string tag, string nextCursor = null)
        {
            return ListResources(new ListResourcesByTagParams()
            {
                Tag = tag,
                NextCursor = nextCursor,
            });
        }

        /// <inheritdoc/>
        public Task<ListResourcesResult> ListResourcesByPublicIdsAsync(IEnumerable<string> publicIds, CancellationToken? cancellationToken = null)
        {
            var listSpecificResourcesParams = new ListSpecificResourcesParams()
            {
                PublicIds = new List<string>(publicIds),
            };
            return ListResourcesAsync(listSpecificResourcesParams, cancellationToken);
        }

        /// <inheritdoc/>
        public ListResourcesResult ListResourcesByPublicIds(IEnumerable<string> publicIds)
        {
            return ListResources(new ListSpecificResourcesParams()
            {
                PublicIds = new List<string>(publicIds),
            });
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public Task<ListResourcesResult> ListResourcesAsync(ListResourcesParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = GetListResourcesUrl(parameters);
            return m_api.CallApiAsync<ListResourcesResult>(HttpMethod.GET, url, parameters, null, null, cancellationToken);
        }

        /// <inheritdoc/>
        public ListResourcesResult ListResources(ListResourcesParams parameters)
        {
            var url = GetListResourcesUrl(parameters);
            return m_api.CallApi<ListResourcesResult>(HttpMethod.GET, url, parameters, null);
        }

        /// <inheritdoc/>
        public Task<ListTagsResult> ListTagsAsync(CancellationToken? cancellationToken = null)
        {
            return ListTagsAsync(new ListTagsParams(), cancellationToken);
        }

        /// <inheritdoc/>
        public ListTagsResult ListTags()
        {
            return ListTags(new ListTagsParams());
        }

        /// <inheritdoc/>
        public Task<ListTagsResult> ListTagsByPrefixAsync(string prefix, CancellationToken? cancellationToken = null)
        {
            return ListTagsAsync(new ListTagsParams() { Prefix = prefix }, cancellationToken);
        }

        /// <inheritdoc/>
        public ListTagsResult ListTagsByPrefix(string prefix)
        {
            return ListTags(new ListTagsParams() { Prefix = prefix });
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public Task<ListTransformsResult> ListTransformationsAsync(CancellationToken? cancellationToken = null)
        {
            return ListTransformationsAsync(new ListTransformsParams(), cancellationToken);
        }

        /// <inheritdoc/>
        public ListTransformsResult ListTransformations()
        {
            return ListTransformations(new ListTransformsParams());
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public ListTransformsResult ListTransformations(ListTransformsParams parameters)
        {
            var urlBuilder = new UrlBuilder(
                GetApiUrlV().
                ResourceType("transformations").
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<ListTransformsResult>(HttpMethod.GET, urlBuilder.ToString(), parameters, null);
        }

        /// <inheritdoc/>
        public Task<GetTransformResult> GetTransformAsync(string transform, CancellationToken? cancellationToken = null)
        {
            return GetTransformAsync(new GetTransformParams() { Transformation = transform }, cancellationToken);
        }

        /// <inheritdoc/>
        public GetTransformResult GetTransform(string transform)
        {
            return GetTransform(new GetTransformParams() { Transformation = transform });
        }

        /// <inheritdoc/>
        public Task<GetTransformResult> GetTransformAsync(GetTransformParams parameters, CancellationToken? cancellationToken = null)
        {
            var urlBuilder = new UrlBuilder(
                GetApiUrlV().
                ResourceType("transformations").
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

        /// <inheritdoc/>
        public GetTransformResult GetTransform(GetTransformParams parameters)
        {
            var urlBuilder = new UrlBuilder(
                GetApiUrlV().
                ResourceType("transformations").
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<GetTransformResult>(HttpMethod.GET, urlBuilder.ToString(), parameters, null);
        }

        /// <inheritdoc/>
        public Task<GetResourceResult> UpdateResourceAsync(string publicId, ModerationStatus moderationStatus, CancellationToken? cancellationToken = null)
        {
            return UpdateResourceAsync(new UpdateParams(publicId) { ModerationStatus = moderationStatus }, cancellationToken);
        }

        /// <inheritdoc/>
        public GetResourceResult UpdateResource(string publicId, ModerationStatus moderationStatus)
        {
            return UpdateResource(new UpdateParams(publicId) { ModerationStatus = moderationStatus });
        }

        /// <inheritdoc/>
        public Task<GetResourceResult> UpdateResourceAsync(UpdateParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = GetApiUrlV().
                ResourceType("resources").
                Add(ApiShared.GetCloudinaryParam(parameters.ResourceType)).
                Add(parameters.Type).Add(parameters.PublicId).
                BuildUrl();

            return m_api.CallApiAsync<GetResourceResult>(HttpMethod.POST, url, parameters, null, null, cancellationToken);
        }

        /// <inheritdoc/>
        public GetResourceResult UpdateResource(UpdateParams parameters)
        {
            var url = GetApiUrlV().
                ResourceType("resources").
                Add(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType)).
                Add(parameters.Type).Add(parameters.PublicId).
                BuildUrl();

            return m_api.CallApi<GetResourceResult>(HttpMethod.POST, url, parameters, null);
        }

        /// <inheritdoc/>
        public Task<GetResourceResult> GetResourceAsync(string publicId, CancellationToken? cancellationToken = null)
        {
            return GetResourceAsync(new GetResourceParams(publicId), cancellationToken);
        }

        /// <inheritdoc/>
        public GetResourceResult GetResource(string publicId)
        {
            return GetResource(new GetResourceParams(publicId));
        }

        /// <inheritdoc/>
        public Task<GetResourceResult> GetResourceAsync(GetResourceParams parameters, CancellationToken? cancellationToken = null)
        {
            var urlBuilder = new UrlBuilder(
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

        /// <inheritdoc/>
        public GetResourceResult GetResource(GetResourceParams parameters)
        {
            var urlBuilder = new UrlBuilder(
                GetApiUrlV().
                ResourceType("resources").
                Add(Api.GetCloudinaryParam(parameters.ResourceType)).
                Add(parameters.Type).
                Add(parameters.PublicId).
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<GetResourceResult>(HttpMethod.GET, urlBuilder.ToString(), parameters, null);
        }

        /// <inheritdoc/>
        public Task<UpdateResourceAccessModeResult> UpdateResourceAccessModeByTagAsync(
            string tag,
            UpdateResourceAccessModeParams parameters,
            CancellationToken? cancellationToken = null)
        {
            return UpdateResourceAccessModeAsync(Constants.TAG_PARAM_NAME, tag, parameters, cancellationToken);
        }

        /// <inheritdoc/>
        public UpdateResourceAccessModeResult UpdateResourceAccessModeByTag(string tag, UpdateResourceAccessModeParams parameters)
        {
            return UpdateResourceAccessMode(Constants.TAG_PARAM_NAME, tag, parameters);
        }

        /// <inheritdoc/>
        public Task<UpdateResourceAccessModeResult> UpdateResourceAccessModeByPrefixAsync(
            string prefix,
            UpdateResourceAccessModeParams parameters,
            CancellationToken? cancellationToken = null)
        {
            return UpdateResourceAccessModeAsync(Constants.PREFIX_PARAM_NAME, prefix, parameters, cancellationToken);
        }

        /// <inheritdoc/>
        public UpdateResourceAccessModeResult UpdateResourceAccessModeByPrefix(
            string prefix,
            UpdateResourceAccessModeParams parameters)
        {
            return UpdateResourceAccessMode(Constants.PREFIX_PARAM_NAME, prefix, parameters);
        }

        /// <inheritdoc/>
        public Task<UpdateResourceAccessModeResult> UpdateResourceAccessModeByIdsAsync(
            UpdateResourceAccessModeParams parameters, CancellationToken? cancellationToken = null)
        {
            return UpdateResourceAccessModeAsync(string.Empty, string.Empty, parameters, cancellationToken);
        }

        /// <inheritdoc/>
        public UpdateResourceAccessModeResult UpdateResourceAccessModeByIds(UpdateResourceAccessModeParams parameters)
        {
            return UpdateResourceAccessMode(string.Empty, string.Empty, parameters);
        }

        /// <inheritdoc/>
        public Task<DelDerivedResResult> DeleteDerivedResourcesAsync(params string[] ids)
        {
            var p = new DelDerivedResParams();
            p.DerivedResources.AddRange(ids);
            return DeleteDerivedResourcesAsync(p);
        }

        /// <inheritdoc/>
        public DelDerivedResResult DeleteDerivedResources(params string[] ids)
        {
            var p = new DelDerivedResParams();
            p.DerivedResources.AddRange(ids);
            return DeleteDerivedResources(p);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public DelDerivedResResult DeleteDerivedResources(DelDerivedResParams parameters)
        {
            var urlBuilder = new UrlBuilder(
                GetApiUrlV().
                Add("derived_resources").
                BuildUrl(),
                parameters.ToParamsDictionary());

            return m_api.CallApi<DelDerivedResResult>(HttpMethod.DELETE, urlBuilder.ToString(), parameters, null);
        }

        /// <inheritdoc/>
        public Task<DelResResult> DeleteResourcesAsync(ResourceType type, params string[] publicIds)
        {
            var p = new DelResParams() { ResourceType = type };
            p.PublicIds.AddRange(publicIds);
            return DeleteResourcesAsync(p);
        }

        /// <inheritdoc/>
        public DelResResult DeleteResources(ResourceType type, params string[] publicIds)
        {
            var p = new DelResParams() { ResourceType = type };
            p.PublicIds.AddRange(publicIds);
            return DeleteResources(p);
        }

        /// <inheritdoc/>
        public Task<DelResResult> DeleteResourcesAsync(params string[] publicIds)
        {
            var p = new DelResParams();
            p.PublicIds.AddRange(publicIds);
            return DeleteResourcesAsync(p);
        }

        /// <inheritdoc/>
        public DelResResult DeleteResources(params string[] publicIds)
        {
            var p = new DelResParams();
            p.PublicIds.AddRange(publicIds);
            return DeleteResources(p);
        }

        /// <inheritdoc/>
        public Task<DelResResult> DeleteResourcesByPrefixAsync(string prefix, CancellationToken? cancellationToken = null)
        {
            var p = new DelResParams() { Prefix = prefix };
            return DeleteResourcesAsync(p, cancellationToken);
        }

        /// <inheritdoc/>
        public DelResResult DeleteResourcesByPrefix(string prefix)
        {
            var p = new DelResParams() { Prefix = prefix };
            return DeleteResources(p);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public DelResResult DeleteResourcesByPrefix(string prefix, bool keepOriginal, string nextCursor)
        {
            var p = new DelResParams() { Prefix = prefix, KeepOriginal = keepOriginal, NextCursor = nextCursor };
            return DeleteResources(p);
        }

        /// <inheritdoc/>
        public Task<DelResResult> DeleteResourcesByTagAsync(string tag, CancellationToken? cancellationToken = null)
        {
            var p = new DelResParams() { Tag = tag };
            return DeleteResourcesAsync(p, cancellationToken);
        }

        /// <inheritdoc/>
        public DelResResult DeleteResourcesByTag(string tag)
        {
            var p = new DelResParams() { Tag = tag };
            return DeleteResources(p);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public DelResResult DeleteResourcesByTag(string tag, bool keepOriginal, string nextCursor)
        {
            var p = new DelResParams() { Tag = tag, KeepOriginal = keepOriginal, NextCursor = nextCursor };
            return DeleteResources(p);
        }

        /// <inheritdoc/>
        public Task<DelResResult> DeleteAllResourcesAsync(CancellationToken? cancellationToken = null)
        {
            var p = new DelResParams() { All = true };
            return DeleteResourcesAsync(p, cancellationToken);
        }

        /// <inheritdoc/>
        public DelResResult DeleteAllResources()
        {
            var p = new DelResParams() { All = true };
            return DeleteResources(p);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public DelResResult DeleteAllResources(bool keepOriginal, string nextCursor)
        {
            var p = new DelResParams() { All = true, KeepOriginal = keepOriginal, NextCursor = nextCursor };
            return DeleteResources(p);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public DelResResult DeleteResources(DelResParams parameters)
        {
            var url = GetApiUrlV().
                Add("resources").
                Add(Api.GetCloudinaryParam<ResourceType>(parameters.ResourceType));

            url = string.IsNullOrEmpty(parameters.Tag)
                ? url.Add(parameters.Type)
                : url.Add("tags").Add(parameters.Tag);

            var urlBuilder = new UrlBuilder(url.BuildUrl(), parameters.ToParamsDictionary());

            return m_api.CallApi<DelResResult>(HttpMethod.DELETE, urlBuilder.ToString(), parameters, null);
        }

        /// <inheritdoc/>
        public Task<RestoreResult> RestoreAsync(params string[] publicIds)
        {
            var restoreParams = new RestoreParams();
            restoreParams.PublicIds.AddRange(publicIds);

            return RestoreAsync(restoreParams);
        }

        /// <inheritdoc/>
        public RestoreResult Restore(params string[] publicIds)
        {
            var restoreParams = new RestoreParams();
            restoreParams.PublicIds.AddRange(publicIds);

            return Restore(restoreParams);
        }

        /// <inheritdoc/>
        public Task<RestoreResult> RestoreAsync(RestoreParams parameters, CancellationToken? cancellationToken = null)
        {
            var url = GetApiUrlV().
                ResourceType("resources").
                Add(ApiShared.GetCloudinaryParam(parameters.ResourceType)).
                Add("upload").
                Add("restore").BuildUrl();

            return m_api.CallApiAsync<RestoreResult>(HttpMethod.POST, url, parameters, null, null, cancellationToken);
        }

        /// <inheritdoc/>
        public RestoreResult Restore(RestoreParams parameters)
        {
            var url = GetApiUrlV().
                ResourceType("resources").
                Add(Api.GetCloudinaryParam(parameters.ResourceType)).
                Add("upload").
                Add("restore").BuildUrl();

            return m_api.CallApi<RestoreResult>(HttpMethod.POST, url, parameters, null);
        }

        /// <inheritdoc/>
        public Task<UploadMappingResults> UploadMappingsAsync(UploadMappingParams parameters, CancellationToken? cancellationToken = null)
        {
            return CallUploadMappingsApiAsync(HttpMethod.GET, parameters, cancellationToken);
        }

        /// <inheritdoc/>
        public UploadMappingResults UploadMappings(UploadMappingParams parameters)
        {
            return CallUploadMappingsAPI(HttpMethod.GET, parameters);
        }

        /// <inheritdoc/>
        public Task<UploadMappingResults> UploadMappingAsync(string folder, CancellationToken? cancellationToken = null)
        {
            if (string.IsNullOrEmpty(folder))
            {
                throw new ArgumentException("Folder name is required.", nameof(folder));
            }

            var parameters = new UploadMappingParams() { Folder = folder };

            return CallUploadMappingsApiAsync(HttpMethod.GET, parameters, cancellationToken);
        }

        /// <inheritdoc/>
        public UploadMappingResults UploadMapping(string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                throw new ArgumentException("Folder must be specified.");
            }

            var parameters = new UploadMappingParams() { Folder = folder };

            return CallUploadMappingsAPI(HttpMethod.GET, parameters);
        }

        /// <inheritdoc/>
        public Task<UploadMappingResults> CreateUploadMappingAsync(string folder, string template, CancellationToken? cancellationToken = null)
        {
            var parameters = CreateUploadMappingParams(folder, template);
            return CallUploadMappingsApiAsync(HttpMethod.POST, parameters, cancellationToken);
        }

        /// <inheritdoc/>
        public UploadMappingResults CreateUploadMapping(string folder, string template)
        {
            var parameters = CreateUploadMappingParams(folder, template);
            return CallUploadMappingsAPI(HttpMethod.POST, parameters);
        }

        /// <inheritdoc/>
        public Task<UploadMappingResults> UpdateUploadMappingAsync(string folder, string newTemplate, CancellationToken? cancellationToken = null)
        {
            var parameters = CreateUploadMappingParams(folder, newTemplate);
            return CallUploadMappingsApiAsync(HttpMethod.PUT, parameters, cancellationToken);
        }

        /// <inheritdoc/>
        public UploadMappingResults UpdateUploadMapping(string folder, string newTemplate)
        {
            var parameters = CreateUploadMappingParams(folder, newTemplate);
            return CallUploadMappingsAPI(HttpMethod.PUT, parameters);
        }

        /// <inheritdoc/>
        public Task<UploadMappingResults> DeleteUploadMappingAsync(CancellationToken? cancellationToken = null)
        {
            return DeleteUploadMappingAsync(string.Empty, cancellationToken);
        }

        /// <inheritdoc/>
        public UploadMappingResults DeleteUploadMapping()
        {
            return DeleteUploadMapping(string.Empty);
        }

        /// <inheritdoc/>
        public Task<UploadMappingResults> DeleteUploadMappingAsync(string folder, CancellationToken? cancellationToken = null)
        {
            var parameters = new UploadMappingParams { Folder = folder };
            return CallUploadMappingsApiAsync(HttpMethod.DELETE, parameters, cancellationToken);
        }

        /// <inheritdoc/>
        public UploadMappingResults DeleteUploadMapping(string folder)
        {
            var parameters = new UploadMappingParams { Folder = folder };
            return CallUploadMappingsAPI(HttpMethod.DELETE, parameters);
        }

        /// <inheritdoc/>
        public Task<UpdateTransformResult> UpdateTransformAsync(UpdateTransformParams parameters, CancellationToken? cancellationToken = null)
        {
            var httpMethod = HttpMethod.PUT;
            var url = GetTransformationUrl(httpMethod, parameters);

            return m_api.CallApiAsync<UpdateTransformResult>(httpMethod, url, parameters, null, null, cancellationToken);
        }

        /// <inheritdoc/>
        public UpdateTransformResult UpdateTransform(UpdateTransformParams parameters)
        {
            var httpMethod = HttpMethod.PUT;
            var url = GetTransformationUrl(httpMethod, parameters);

            return m_api.CallApi<UpdateTransformResult>(httpMethod, url, parameters, null, null);
        }

        /// <inheritdoc/>
        public Task<TransformResult> CreateTransformAsync(CreateTransformParams parameters, CancellationToken? cancellationToken = null)
        {
            var httpMethod = HttpMethod.POST;
            var url = GetTransformationUrl(httpMethod, parameters);

            return m_api.CallApiAsync<TransformResult>(httpMethod, url, parameters, null, null, cancellationToken);
        }

        /// <inheritdoc/>
        public TransformResult CreateTransform(CreateTransformParams parameters)
        {
            var httpMethod = HttpMethod.POST;
            var url = GetTransformationUrl(httpMethod, parameters);

            return m_api.CallApi<TransformResult>(httpMethod, url, parameters, null, null);
        }

        /// <inheritdoc/>
        public Task<TransformResult> DeleteTransformAsync(string transformName, CancellationToken? cancellationToken = null)
        {
            var httpMethod = HttpMethod.DELETE;
            var url = GetTransformationUrl(httpMethod, new DeleteTransformParams() { Transformation = transformName });
            return m_api.CallApiAsync<TransformResult>(
                httpMethod,
                url,
                null,
                null,
                null,
                cancellationToken);
        }

        /// <inheritdoc/>
        public TransformResult DeleteTransform(string transformName)
        {
            var httpMethod = HttpMethod.DELETE;

            var url = GetTransformationUrl(httpMethod, new DeleteTransformParams() { Transformation = transformName });

            return m_api.CallApi<TransformResult>(httpMethod, url, null, null);
        }

        private static void CheckIfNotEmpty(string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                throw new ArgumentException("Folder must be set.");
            }
        }

        private static void CheckFolderParameter(string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                throw new ArgumentException(
                    "folder must be set. Please use RootFolders() to get list of folders in root.");
            }
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
        /// Get default API URL with version.
        /// </summary>
        /// <returns>URL of the API.</returns>
        private Url GetApiUrlV()
        {
            return m_api.ApiUrlV;
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

        private string GetUsageUrl(DateTime? date)
        {
            var url = GetApiUrlV().Action("usage");

            if (date.HasValue)
            {
                url.Add(date.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture));
            }

            return url.BuildUrl();
        }

        private string GetFolderUrl(string folder = null, GetFoldersParams parameters = null)
        {
            var urlWithoutParams = GetApiUrlV().Add("folders").Add(folder).BuildUrl();

            return (parameters != null) ? new UrlBuilder(urlWithoutParams, parameters.ToParamsDictionary()).ToString() : urlWithoutParams;
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

            var url = GetApiUrlV()
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
            var url = (httpMethod == HttpMethod.POST || httpMethod == HttpMethod.PUT)
                ? GetUploadMappingUrl()
                : GetUploadMappingUrl(parameters);

            return m_api.CallApi<UploadMappingResults>(httpMethod, url, parameters, null);
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
