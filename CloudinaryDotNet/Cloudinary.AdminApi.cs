namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
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
        /// Lists resource types asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed list of resource types.</returns>
        public Task<ListResourceTypesResult> ListResourceTypesAsync(CancellationToken? cancellationToken = null)
        {
            return CallAdminApiAsync<ListResourceTypesResult>(HttpMethod.GET, GetResourcesUrl().BuildUrl(), null, cancellationToken);
        }

        /// <summary>
        /// Lists resource types.
        /// </summary>
        /// <returns>Parsed list of resource types.</returns>
        public ListResourceTypesResult ListResourceTypes()
        {
            return ListResourceTypesAsync().GetAwaiter().GetResult();
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
            return ListResourcesAsync(nextCursor, tags, context, moderations)
                .GetAwaiter().GetResult();
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
            return ListResourcesByTypeAsync(type, nextCursor).GetAwaiter().GetResult();
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
            return ListResourcesByPrefixAsync(prefix, type, nextCursor)
                .GetAwaiter().GetResult();
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
            return ListResourcesByPrefixAsync(prefix, tags, context, moderations, type, nextCursor)
                .GetAwaiter().GetResult();
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
            return ListResourcesByTagAsync(tag, nextCursor).GetAwaiter().GetResult();
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
            return ListResourcesByPublicIdsAsync(publicIds)
                .GetAwaiter().GetResult();
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
            return ListResourceByPublicIdsAsync(publicIds, tags, context, moderations)
                .GetAwaiter().GetResult();
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
            return ListResourcesByModerationStatusAsync(kind, status, tags, context, moderations, nextCursor)
                .GetAwaiter().GetResult();
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
            return ListResourcesByContextAsync(key, value, tags, context, nextCursor).GetAwaiter().GetResult();
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
            return CallAdminApiAsync<ListResourcesResult>(HttpMethod.GET, url, parameters, cancellationToken);
        }

        /// <summary>
        /// Gets a list of resources.
        /// </summary>
        /// <param name="parameters">Parameters to list resources.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        public ListResourcesResult ListResources(ListResourcesParams parameters)
        {
            return ListResourcesAsync(parameters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Async call to get a list of folders in the root asynchronously.
        /// </summary>
        /// <param name="parameters">(optional) Parameters for managing folders list.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of folders listing.</returns>
        public Task<GetFoldersResult> RootFoldersAsync(GetFoldersParams parameters = null, CancellationToken? cancellationToken = null)
        {
            return CallAdminApiAsync<GetFoldersResult>(HttpMethod.GET, GetFolderUrl(parameters: parameters), parameters, cancellationToken);
        }

        /// <summary>
        /// Gets a list of folders in the root.
        /// </summary>
        /// <param name="parameters">(optional) Parameters for managing folders list.</param>
        /// <returns>Parsed result of folders listing.</returns>
        public GetFoldersResult RootFolders(GetFoldersParams parameters = null)
        {
            return RootFoldersAsync(parameters, null).GetAwaiter().GetResult();
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

            return CallAdminApiAsync<GetFoldersResult>(
                HttpMethod.GET,
                GetFolderUrl(folder, parameters),
                null,
                cancellationToken);
        }

        /// <summary>
        /// Gets a list of subfolders in a specified folder.
        /// </summary>
        /// <param name="folder">The folder name.</param>
        /// <param name="parameters">(Optional) Parameters for managing folders list.</param>
        /// <returns>Parsed result of folders listing.</returns>
        public GetFoldersResult SubFolders(string folder, GetFoldersParams parameters = null)
        {
            return SubFoldersAsync(
                folder,
                parameters,
                null).GetAwaiter().GetResult();
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
            return CallAdminApiAsync<DeleteFolderResult>(
                HttpMethod.DELETE,
                uri,
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
            return DeleteFolderAsync(folder, null).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Create a new empty folder.
        /// </summary>
        /// <param name="folder">The full path of the new folder to create.</param>
        /// <returns>Parsed result of folder creation.</returns>
        public CreateFolderResult CreateFolder(string folder)
        {
            return CreateFolderAsync(
                folder,
                null).GetAwaiter().GetResult();
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

            return CallAdminApiAsync<CreateFolderResult>(
                HttpMethod.POST,
                GetFolderUrl(folder),
                null,
                cancellationToken);
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

            return CallAdminApiAsync<UploadPresetResult>(
                HttpMethod.POST,
                url,
                parameters,
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
            return CreateUploadPresetAsync(parameters, null).GetAwaiter().GetResult();
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

            return CallAdminApiAsync<GetUploadPresetResult>(
                HttpMethod.GET,
                url,
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
            return GetUploadPresetAsync(name, null).GetAwaiter().GetResult();
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

            return CallAdminApiAsync<ListUploadPresetsResult>(
                HttpMethod.GET,
                urlBuilder.ToString(),
                parameters,
                cancellationToken);
        }

        /// <summary>
        /// Lists upload presets.
        /// </summary>
        /// <param name="parameters">Parameters to list upload presets.</param>
        /// <returns>Parsed result of upload presets listing.</returns>
        public ListUploadPresetsResult ListUploadPresets(ListUploadPresetsParams parameters)
        {
            return ListUploadPresetsAsync(parameters, null).GetAwaiter().GetResult();
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

            return CallAdminApiAsync<DeleteUploadPresetResult>(
                HttpMethod.DELETE,
                url,
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
            return DeleteUploadPresetAsync(name, null).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets the Cloudinary account usage details asynchronously.
        /// </summary>
        /// <param name="date">(Optional) The date for the usage report. Must be within the last 3 months.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>The report on the status of your Cloudinary account usage details.</returns>
        public Task<UsageResult> GetUsageAsync(DateTime? date, CancellationToken? cancellationToken = null)
        {
            string uri = GetUsageUrl(date);

            return CallAdminApiAsync<UsageResult>(
                HttpMethod.GET,
                uri,
                null,
                cancellationToken);
        }

        /// <summary>
        /// Gets the Cloudinary account usage details.
        /// </summary>
        /// <param name="date">(Optional) The date for the usage report. Must be within the last 3 months.</param>
        /// <returns>The report on the status of your Cloudinary account usage details.</returns>
        public UsageResult GetUsage(DateTime? date = null)
        {
            return GetUsageAsync(date, null).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets the Cloudinary account usage details asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>The report on the status of your Cloudinary account usage details.</returns>
        public Task<UsageResult> GetUsageAsync(CancellationToken? cancellationToken = null)
        {
            string uri = GetUsageUrl(null);

            return CallAdminApiAsync<UsageResult>(
                HttpMethod.GET,
                uri,
                null,
                cancellationToken);
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

            return CallAdminApiAsync<ListTagsResult>(HttpMethod.GET, urlBuilder.ToString(), parameters, cancellationToken);
        }

        /// <summary>
        /// Gets a list of tags.
        /// </summary>
        /// <param name="parameters">Parameters of the request.</param>
        /// <returns>Parsed list of tags.</returns>
        public ListTagsResult ListTags(ListTagsParams parameters)
        {
            return ListTagsAsync(parameters).GetAwaiter().GetResult();
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

            return CallAdminApiAsync<ListTransformsResult>(
                HttpMethod.GET,
                urlBuilder.ToString(),
                parameters,
                cancellationToken);
        }

        /// <summary>
        /// Gets a list of transformations.
        /// </summary>
        /// <param name="parameters">Parameters of the request for a list of transformation.</param>
        /// <returns>Parsed list of transformations details.</returns>
        public ListTransformsResult ListTransformations(ListTransformsParams parameters)
        {
            return ListTransformationsAsync(parameters, null).GetAwaiter().GetResult();
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
                BuildUrl(),
                parameters.ToParamsDictionary());

            return CallAdminApiAsync<GetTransformResult>(
                HttpMethod.GET,
                urlBuilder.ToString(),
                parameters,
                cancellationToken);
        }

        /// <summary>
        /// Gets details of a single transformation.
        /// </summary>
        /// <param name="parameters">Parameters of the request of transformation details.</param>
        /// <returns>Parsed details of a single transformation.</returns>
        public GetTransformResult GetTransform(GetTransformParams parameters)
        {
            return GetTransformAsync(parameters, null).GetAwaiter().GetResult();
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

            return CallAdminApiAsync<GetResourceResult>(HttpMethod.POST, url, parameters, cancellationToken);
        }

        /// <summary>
        /// Updates details of an existing resource.
        /// </summary>
        /// <param name="parameters">Parameters to update details of an existing resource.</param>
        /// <returns>Parsed response of the detailed resource information.</returns>
        public GetResourceResult UpdateResource(UpdateParams parameters)
        {
            return UpdateResourceAsync(parameters, null).GetAwaiter().GetResult();
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

            return CallAdminApiAsync<GetResourceResult>(
                HttpMethod.GET,
                urlBuilder.ToString(),
                parameters,
                cancellationToken);
        }

        /// <summary>
        /// Gets details of the requested resource as well as all its derived resources.
        /// </summary>
        /// <param name="parameters">Parameters of the request of resource.</param>
        /// <returns>Parsed response with the detailed resource information.</returns>
        public GetResourceResult GetResource(GetResourceParams parameters)
        {
            return GetResourceAsync(parameters, null).GetAwaiter().GetResult();
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

            return CallAdminApiAsync<DelDerivedResResult>(
                HttpMethod.DELETE,
                urlBuilder.ToString(),
                parameters,
                cancellationToken);
        }

        /// <summary>
        /// Deletes all derived resources with the given parameters.
        /// </summary>
        /// <param name="parameters">Parameters to delete derived resources.</param>
        /// <returns>Parsed result of deletion derived resources.</returns>
        public DelDerivedResResult DeleteDerivedResources(DelDerivedResParams parameters)
        {
            return DeleteDerivedResourcesAsync(parameters, null).GetAwaiter().GetResult();
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

            return CallAdminApiAsync<DelResResult>(
                HttpMethod.DELETE,
                urlBuilder.ToString(),
                parameters,
                cancellationToken);
        }

        /// <summary>
        /// Deletes all resources with parameters.
        /// </summary>
        /// <param name="parameters">Parameters for deletion resources.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        public DelResResult DeleteResources(DelResParams parameters)
        {
            return DeleteResourcesAsync(parameters, null).GetAwaiter().GetResult();
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

            return CallAdminApiAsync<RestoreResult>(HttpMethod.POST, url, parameters, cancellationToken);
        }

        /// <summary>
        /// Restores a deleted resources.
        /// </summary>
        /// <param name="parameters">Parameters to restore a deleted resources.</param>
        /// <returns>Parsed result of restoring resources.</returns>
        public RestoreResult Restore(RestoreParams parameters)
        {
            return RestoreAsync(parameters, null).GetAwaiter().GetResult();
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
            var httpMethod = HttpMethod.PUT;
            var url = GetTransformationUrl(httpMethod, parameters);

            return CallAdminApiAsync<UpdateTransformResult>(httpMethod, url, parameters, cancellationToken);
        }

        /// <summary>
        /// Updates Cloudinary transformation resource.
        /// </summary>
        /// <param name="parameters">Parameters for transformation update.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        public UpdateTransformResult UpdateTransform(UpdateTransformParams parameters)
        {
            return UpdateTransformAsync(parameters, null).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Creates Cloudinary transformation resource asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of the new transformation.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        public Task<TransformResult> CreateTransformAsync(CreateTransformParams parameters, CancellationToken? cancellationToken = null)
        {
            var httpMethod = HttpMethod.POST;
            var url = GetTransformationUrl(httpMethod, parameters);

            return CallAdminApiAsync<TransformResult>(httpMethod, url, parameters, cancellationToken);
        }

        /// <summary>
        /// Creates Cloudinary transformation resource.
        /// </summary>
        /// <param name="parameters">Parameters of the new transformation.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        public TransformResult CreateTransform(CreateTransformParams parameters)
        {
            return CreateTransformAsync(parameters, null).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Deletes transformation by name asynchronously.
        /// </summary>
        /// <param name="transformName">The name of transformation to delete.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        public Task<TransformResult> DeleteTransformAsync(string transformName, CancellationToken? cancellationToken = null)
        {
            var httpMethod = HttpMethod.DELETE;
            var url = GetTransformationUrl(httpMethod, new DeleteTransformParams() { Transformation = transformName });

            return CallAdminApiAsync<TransformResult>(
                httpMethod,
                url,
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
            return DeleteTransformAsync(transformName, null).GetAwaiter().GetResult();
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

        private string GetFolderUrl(string folder = null, GetFoldersParams parameters = null)
        {
            var urlWithoutParams = GetApiUrlV().Add("folders").Add(folder).BuildUrl();

            return (parameters != null) ? new UrlBuilder(urlWithoutParams, parameters.ToParamsDictionary()).ToString() : urlWithoutParams;
        }

        private string GetUsageUrl(DateTime? date)
        {
            var url = GetApiUrlV().Action("usage");

            if (date.HasValue)
            {
                url.Add(date.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture));
            }

            return url.BuildUrl();
        }

        private string GetListResourcesUrl(ListResourcesParams parameters)
        {
            var url = GetResourcesUrl().Add(ApiShared.GetCloudinaryParam(parameters.ResourceType));

            switch (parameters)
            {
                case ListResourcesByTagParams tagParams when !string.IsNullOrEmpty(tagParams.Tag):
                    url
                        .Add("tags")
                        .Add(tagParams.Tag);

                    break;
                case ListResourcesByModerationParams modParams when !string.IsNullOrEmpty(modParams.ModerationKind):
                    url
                        .Add("moderations")
                        .Add(modParams.ModerationKind)
                        .Add(Api.GetCloudinaryParam(modParams.ModerationStatus));

                    break;
                case ListResourcesByContextParams _:
                    url.Add("context");

                    break;
            }

            var urlBuilder = new UrlBuilder(
                url.BuildUrl(),
                parameters.ToParamsDictionary());

            var s = urlBuilder.ToString();
            return s;
        }

        private Url GetResourcesUrl() => GetApiUrlV().ResourceType("resources");

        private Task<T> CallAdminApiAsync<T>(
            HttpMethod httpMethod,
            string url,
            BaseParams parameters,
            CancellationToken? cancellationToken,
            Dictionary<string, string> extraHeaders = null)
            where T : BaseResult, new()
        {
            return m_api.CallApiAsync<T>(
                            httpMethod,
                            url,
                            parameters,
                            null,
                            extraHeaders,
                            cancellationToken);
        }
    }
}
