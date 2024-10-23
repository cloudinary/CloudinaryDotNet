namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;

    /// <summary>
    /// Cloudinary Admin API Interface.
    /// </summary>
    public interface ICloudinaryAdminApi
    {
        /// <summary>
        /// Tests the reachability of the Cloudinary API.
        /// </summary>
        /// <returns>Ping result.</returns>
        PingResult Ping();

        /// <summary>
        /// Tests the reachability of the Cloudinary API asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Ping result.</returns>
        Task<PingResult> PingAsync(CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets the Cloudinary account configuration details asynchronously.
        /// </summary>
        /// <param name="configParams">(Optional) Parameters for the configuration request.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>The configuration details of your Cloudinary account.</returns>
        Task<ConfigResult> GetConfigAsync(ConfigParams configParams = null, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets the Cloudinary account configuration details.
        /// </summary>
        /// <param name="configParams">(Optional) Parameters for the configuration request.</param>
        /// <returns>The configuration details of the Cloudinary account.</returns>
        ConfigResult GetConfig(ConfigParams configParams = null);

        /// <summary>
        /// Create a new streaming profile asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of streaming profile creating.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Detailed information about created streaming profile.</returns>
        Task<StreamingProfileResult> CreateStreamingProfileAsync(StreamingProfileCreateParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Create a new streaming profile.
        /// </summary>
        /// <param name="parameters">Parameters of streaming profile creating.</param>
        /// <returns>Detailed information about created streaming profile.</returns>
        StreamingProfileResult CreateStreamingProfile(StreamingProfileCreateParams parameters);

        /// <summary>
        /// Update streaming profile asynchronously.
        /// </summary>
        /// <param name="name">Name to be assigned to a streaming profile.</param>
        /// <param name="parameters">Parameters of streaming profile updating.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <exception cref="ArgumentNullException">parameters can't be null.</exception>
        /// <exception cref="ArgumentException">name can't be null or empty.</exception>
        /// <returns>Result of updating the streaming profile.</returns>
        Task<StreamingProfileResult> UpdateStreamingProfileAsync(
            string name,
            StreamingProfileUpdateParams parameters,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Update streaming profile.
        /// </summary>
        /// <param name="name">Name to be assigned to a streaming profile.</param>
        /// <param name="parameters">Parameters of streaming profile updating.</param>
        /// <exception cref="ArgumentNullException">both arguments can't be null.</exception>
        /// <returns>Result of updating the streaming profile.</returns>
        StreamingProfileResult UpdateStreamingProfile(string name, StreamingProfileUpdateParams parameters);

        /// <summary>
        /// Delete streaming profile asynchronously.
        /// </summary>
        /// <param name="name">The Name of streaming profile.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <exception cref="ArgumentException">name can't be null.</exception>
        /// <returns>Result of removing the streaming profile.</returns>
        Task<StreamingProfileResult> DeleteStreamingProfileAsync(string name, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Delete streaming profile.
        /// </summary>
        /// <param name="name">Streaming profile name to delete.</param>
        /// <exception cref="ArgumentNullException">name can't be null.</exception>
        /// <returns>Result of removing the streaming profile.</returns>
        StreamingProfileResult DeleteStreamingProfile(string name);

        /// <summary>
        /// Retrieve the details of a single streaming profile by name asynchronously.
        /// </summary>
        /// <param name="name">The Name of streaming profile.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <exception cref="ArgumentException">name can't be null.</exception>
        /// <returns>Detailed information about the streaming profile.</returns>
        Task<StreamingProfileResult> GetStreamingProfileAsync(string name, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Retrieve the details of a single streaming profile by name.
        /// </summary>
        /// <param name="name">Streaming profile name.</param>
        /// <exception cref="ArgumentNullException">name can't be null.</exception>
        /// <returns>Detailed information about the streaming profile.</returns>
        StreamingProfileResult GetStreamingProfile(string name);

        /// <summary>
        /// Retrieve the list of streaming profiles, including built-in and custom profiles asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Detailed information about streaming profiles.</returns>
        Task<StreamingProfileListResult> ListStreamingProfilesAsync(CancellationToken? cancellationToken = null);

        /// <summary>
        /// Retrieve the list of streaming profiles, including built-in and custom profiles.
        /// </summary>
        /// <returns>Detailed information about streaming profiles.</returns>
        StreamingProfileListResult ListStreamingProfiles();

        /// <summary>
        /// Create a new metadata field definition.
        /// </summary>
        /// <param name="parameters">Parameters of the metadata field.</param>
        /// <returns>Parsed result of the metadata field creating.</returns>
        /// <typeparam name="T">Type of the metadata field.</typeparam>
        MetadataFieldResult AddMetadataField<T>(MetadataFieldCreateParams<T> parameters);

        /// <summary>
        /// Create a new metadata field definition.
        /// </summary>
        /// <param name="parameters">Parameters of the metadata field.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>Parsed result of the metadata field creating.</returns>
        /// <typeparam name="T">Type of the metadata field.</typeparam>
        Task<MetadataFieldResult> AddMetadataFieldAsync<T>(
            MetadataFieldCreateParams<T> parameters,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Retrieve a list of all metadata field definitions as an array of JSON objects.
        /// </summary>
        /// <returns>Parsed list of metadata field definitions.</returns>
        MetadataFieldListResult ListMetadataFields();

        /// <summary>
        /// Retrieve a list of all metadata field definitions as an array of JSON objects.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>Parsed list of metadata field definitions.</returns>
        Task<MetadataFieldListResult> ListMetadataFieldsAsync(CancellationToken? cancellationToken = null);

        /// <summary>
        /// Retrieve a single metadata field definition.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <returns>Parsed information about metadata field.</returns>
        MetadataFieldResult GetMetadataField(string fieldExternalId);

        /// <summary>
        /// Retrieve a single metadata field definition.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>Parsed information about metadata field.</returns>
        Task<MetadataFieldResult> GetMetadataFieldAsync(
            string fieldExternalId,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Update a metadata field by its external ID.
        /// There is no need to pass the entire object, only properties to be updated.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="parameters">Parameters of the metadata field to be updated.</param>
        /// <returns>Parsed result of the operation.</returns>
        /// <typeparam name="T">Type of the metadata field.</typeparam>
        MetadataFieldResult UpdateMetadataField<T>(string fieldExternalId, MetadataFieldUpdateParams<T> parameters);

        /// <summary>
        /// Update a metadata field by its external ID.
        /// There is no need to pass the entire object, only properties to be updated.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="parameters">Parameters of the metadata field to be updated.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>Parsed result of the operation.</returns>
        /// <typeparam name="T">Type of the metadata field.</typeparam>
        Task<MetadataFieldResult> UpdateMetadataFieldAsync<T>(
            string fieldExternalId,
            MetadataFieldUpdateParams<T> parameters,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Update the datasource of a supported field type (currently only enum and set).
        /// The update is partial: datasource entries with an existing external_id will be updated.
        /// Entries with new external_id’s (or without external_id’s) will be appended.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="parameters">Parameters of the datasource to be updated.</param>
        /// <returns>Parsed result of the operation.</returns>
        MetadataDataSourceResult UpdateMetadataDataSourceEntries(string fieldExternalId, MetadataDataSourceParams parameters);

        /// <summary>
        /// Update the datasource of a supported field type (currently only enum and set).
        /// The update is partial: datasource entries with an existing external_id will be updated.
        /// Entries with new external_id’s (or without external_id’s) will be appended.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="parameters">Parameters of the datasource to be updated.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>Parsed result of the operation.</returns>
        Task<MetadataDataSourceResult> UpdateMetadataDataSourceEntriesAsync(
            string fieldExternalId,
            MetadataDataSourceParams parameters,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Delete a metadata field definition.
        /// The field should no longer be considered a valid candidate for all other endpoints.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <returns>Parsed result of the operation.</returns>
        DelMetadataFieldResult DeleteMetadataField(string fieldExternalId);

        /// <summary>
        /// Delete a metadata field definition.
        /// The field should no longer be considered a valid candidate for all other endpoints.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>Parsed result of the operation.</returns>
        Task<DelMetadataFieldResult> DeleteMetadataFieldAsync(
            string fieldExternalId,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Delete datasource entries for a specified metadata field definition.
        /// This operation sets the state of the entries to inactive.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="entriesExternalIds">An array of IDs of datasource entries to delete.</param>
        /// <returns>Parsed result of the operation.</returns>
        MetadataDataSourceResult DeleteMetadataDataSourceEntries(string fieldExternalId, List<string> entriesExternalIds);

        /// <summary>
        /// Delete datasource entries for a specified metadata field definition.
        /// This operation sets the state of the entries to inactive.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="entriesExternalIds">An array of IDs of datasource entries to delete.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>Parsed result of the operation.</returns>
        Task<MetadataDataSourceResult> DeleteMetadataDataSourceEntriesAsync(
            string fieldExternalId,
            List<string> entriesExternalIds,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Restore (unblock) any previously deleted datasource entries for a specified metadata field definition
        ///  and set the state of the entries to active.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="entriesExternalIds">An array of IDs of datasource entries to restore.</param>
        /// <returns>Parsed result of the operation.</returns>
        MetadataDataSourceResult RestoreMetadataDataSourceEntries(string fieldExternalId, List<string> entriesExternalIds);

        /// <summary>
        /// Restore (unblock) any previously deleted datasource entries for a specified metadata field definition
        ///  and set the state of the entries to active.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="entriesExternalIds">An array of IDs of datasource entries to restore.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>Parsed result of the operation.</returns>
        Task<MetadataDataSourceResult> RestoreMetadataDataSourceEntriesAsync(
            string fieldExternalId,
            List<string> entriesExternalIds,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Populate metadata fields with the given values. Existing values will be overwritten.
        ///
        /// Any metadata-value pairs given are merged with any existing metadata-value pairs
        /// (an empty value for an existing metadata field clears the value).
        /// </summary>
        /// <param name="parameters">Values to be applied to metadata fields of uploaded assets.</param>
        /// <returns>Parsed result of the operation.</returns>
        MetadataUpdateResult UpdateMetadata(MetadataUpdateParams parameters);

        /// <summary>
        /// Populate metadata fields with the given values. Existing values will be overwritten.
        ///
        /// Any metadata-value pairs given are merged with any existing metadata-value pairs
        /// (an empty value for an existing metadata field clears the value).
        /// </summary>
        /// <param name="parameters">Values to be applied to metadata fields of uploaded assets.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>Parsed result of the operation.</returns>
        Task<MetadataUpdateResult> UpdateMetadataAsync(
            MetadataUpdateParams parameters,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Reorders metadata fields.
        /// </summary>
        /// <param name="orderBy">Criteria for the order.</param>
        /// <param name="direction">(Optional) Direction.</param>
        /// <returns>Some result.</returns>
        Task<MetadataFieldListResult> ReorderMetadataFieldsAsync(
            MetadataReorderBy orderBy,
            MetadataReorderDirection? direction = null);

        /// <summary>
        /// Reorders metadata fields.
        /// </summary>
        /// <param name="orderBy">Criteria for the order.</param>
        /// <param name="direction">(Optional) Direction.</param>
        /// <returns>Some result.</returns>
        MetadataFieldListResult ReorderMetadataFields(
            MetadataReorderBy orderBy,
            MetadataReorderDirection? direction = null);

        /// <summary>
        /// Reorders metadata field datasource. Currently, supports only value.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="orderBy">Criteria for the sort. Currently supports only value.</param>
        /// <param name="direction">Optional (gets either asc or desc).</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the operation.</returns>
        Task<MetadataDataSourceResult> ReorderMetadataFieldDatasourceAsync(
            string fieldExternalId,
            string orderBy,
            string direction = null,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Reorders metadata field datasource. Currently, supports only value.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="orderBy">Criteria for the sort. Currently supports only value.</param>
        /// <param name="direction">Optional (gets either asc or desc).</param>
        /// <returns>Parsed result of the operation.</returns>
        MetadataDataSourceResult ReorderMetadataFieldDatasource(
            string fieldExternalId,
            string orderBy,
            string direction = null);

        /// <summary>
        /// Lists resource types asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed list of resource types.</returns>
        Task<ListResourceTypesResult> ListResourceTypesAsync(CancellationToken? cancellationToken = null);

        /// <summary>
        /// Lists resource types.
        /// </summary>
        /// <returns>Parsed list of resource types.</returns>
        ListResourceTypesResult ListResourceTypes();

        /// <summary>
        /// Lists resources asynchronously asynchronously.
        /// </summary>
        /// <param name="nextCursor">Starting position.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        Task<ListResourcesResult> ListResourcesAsync(
            string nextCursor = null,
            bool tags = true,
            bool context = true,
            bool moderations = true,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets a list of resources asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to list resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        Task<ListResourcesResult> ListResourcesAsync(ListResourcesParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Lists resources.
        /// </summary>
        /// <param name="nextCursor">Starting position.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        ListResourcesResult ListResources(
            string nextCursor = null,
            bool tags = true,
            bool context = true,
            bool moderations = true);

        /// <summary>
        /// Gets a list of resources.
        /// </summary>
        /// <param name="parameters">Parameters to list resources.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        ListResourcesResult ListResources(ListResourcesParams parameters);

        /// <summary>
        /// Lists resources of specified type asynchronously.
        /// </summary>
        /// <param name="type">Resource type.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        Task<ListResourcesResult> ListResourcesByTypeAsync(
            string type,
            string nextCursor = null,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Lists resources of specified type.
        /// </summary>
        /// <param name="type">Resource type.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        ListResourcesResult ListResourcesByType(string type, string nextCursor = null);

        /// <summary>
        /// Lists resources by prefix asynchronously.
        /// </summary>
        /// <param name="prefix">Public identifier prefix.</param>
        /// <param name="type">Resource type.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        Task<ListResourcesResult> ListResourcesByPrefixAsync(
            string prefix,
            string type = "upload",
            string nextCursor = null,
            CancellationToken? cancellationToken = null);

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
        Task<ListResourcesResult> ListResourcesByPrefixAsync(
            string prefix,
            bool tags,
            bool context,
            bool moderations,
            string type = "upload",
            string nextCursor = null,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Lists resources by prefix.
        /// </summary>
        /// <param name="prefix">Public identifier prefix.</param>
        /// <param name="type">Resource type.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        ListResourcesResult ListResourcesByPrefix(
            string prefix,
            string type = "upload",
            string nextCursor = null);

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
        ListResourcesResult ListResourcesByPrefix(string prefix, bool tags, bool context, bool moderations, string type = "upload", string nextCursor = null);

        /// <summary>
        /// Lists resources by tag asynchronously.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        Task<ListResourcesResult> ListResourcesByTagAsync(
            string tag,
            string nextCursor = null,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Lists resources by tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="nextCursor">Starting position.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        ListResourcesResult ListResourcesByTag(string tag, string nextCursor = null);

        /// <summary>
        /// Returns resources with specified public identifiers asynchronously.
        /// </summary>
        /// <param name="publicIds">Public identifiers.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        Task<ListResourcesResult> ListResourcesByPublicIdsAsync(
            IEnumerable<string> publicIds,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Returns resources with specified public identifiers.
        /// </summary>
        /// <param name="publicIds">Public identifiers.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        ListResourcesResult ListResourcesByPublicIds(IEnumerable<string> publicIds);

        /// <summary>
        /// Returns resources with specified public identifiers asynchronously.
        /// </summary>
        /// <param name="publicIds">Public identifiers.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        Task<ListResourcesResult> ListResourceByPublicIdsAsync(
            IEnumerable<string> publicIds,
            bool tags,
            bool context,
            bool moderations,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Returns resources with specified public identifiers.
        /// </summary>
        /// <param name="publicIds">Public identifiers.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        ListResourcesResult ListResourceByPublicIds(IEnumerable<string> publicIds, bool tags, bool context, bool moderations);

        /// <summary>
        /// Returns resources with specified asset identifiers asynchronously.
        /// </summary>
        /// <param name="assetIds">Asset identifiers.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        Task<ListResourcesResult> ListResourceByAssetIdsAsync(
            IEnumerable<string> assetIds,
            bool tags,
            bool context,
            bool moderations,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets a list of resources with specified asset identifiers.
        /// </summary>
        /// <param name="assetIds">Asset identifiers.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        ListResourcesResult ListResourcesByAssetIDs(IEnumerable<string> assetIds, bool tags, bool context, bool moderations);

        /// <summary>
        /// Returns resources in the specified asset folder asynchronously.
        /// </summary>
        /// <param name="assetFolder">Asset Folder.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        Task<ListResourcesResult> ListResourceByAssetFolderAsync(
            string assetFolder,
            bool tags,
            bool context,
            bool moderations,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets a list of resources in the specified asset folder.
        /// </summary>
        /// <param name="assetFolder">Asset folder.</param>
        /// <param name="tags">Whether to include tags in result.</param>
        /// <param name="context">Whether to include context in result.</param>
        /// <param name="moderations">Whether to include moderation status in result.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        ListResourcesResult ListResourcesByAssetFolder(string assetFolder, bool tags = false, bool context = false, bool moderations = false);

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
        Task<ListResourcesResult> ListResourcesByModerationStatusAsync(
            string kind,
            ModerationStatus status,
            bool tags = true,
            bool context = true,
            bool moderations = true,
            string nextCursor = null,
            CancellationToken? cancellationToken = null);

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
        ListResourcesResult ListResourcesByModerationStatus(
            string kind,
            ModerationStatus status,
            bool tags = true,
            bool context = true,
            bool moderations = true,
            string nextCursor = null);

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
        Task<ListResourcesResult> ListResourcesByContextAsync(
            string key,
            string value = "",
            bool tags = false,
            bool context = false,
            string nextCursor = null,
            CancellationToken? cancellationToken = null);

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
        ListResourcesResult ListResourcesByContext(
            string key,
            string value = "",
            bool tags = false,
            bool context = false,
            string nextCursor = null);

        /// <summary>
        /// Find images based on their visual content asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters for visual search.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        Task<VisualSearchResult> VisualSearchAsync(VisualSearchParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Find images based on their visual content.
        /// </summary>
        /// <param name="parameters">Parameters for visual search.</param>
        /// <returns>Parsed result of the resources listing.</returns>
        VisualSearchResult VisualSearch(VisualSearchParams parameters);

        /// <summary>
        /// Publishes resources by prefix asynchronously.
        /// </summary>
        /// <param name="prefix">The prefix for publishing resources.</param>
        /// <param name="parameters">Parameters for publishing of resources.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Parsed result of publishing.</returns>
        Task<PublishResourceResult> PublishResourceByPrefixAsync(
            string prefix,
            PublishResourceParams parameters,
            CancellationToken? cancellationToken);

        /// <summary>
        /// Publishes resources by prefix.
        /// </summary>
        /// <param name="prefix">The prefix for publishing resources.</param>
        /// <param name="parameters">Parameters for publishing of resources.</param>
        /// <returns>Parsed result of publishing.</returns>
        PublishResourceResult PublishResourceByPrefix(string prefix, PublishResourceParams parameters);

        /// <summary>
        /// Publishes resources by tag asynchronously.
        /// </summary>
        /// <param name="tag">All resources with the given tag will be published.</param>
        /// <param name="parameters">Parameters for publishing of resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of publishing.</returns>
        Task<PublishResourceResult> PublishResourceByTagAsync(
            string tag,
            PublishResourceParams parameters,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Publishes resources by tag.
        /// </summary>
        /// <param name="tag">All resources with the given tag will be published.</param>
        /// <param name="parameters">Parameters for publishing of resources.</param>
        /// <returns>Parsed result of publishing.</returns>
        PublishResourceResult PublishResourceByTag(string tag, PublishResourceParams parameters);

        /// <summary>
        /// Publishes resource by Id asynchronously.
        /// </summary>
        /// <param name="tag">Not used.</param>
        /// <param name="parameters">Parameters for publishing of resources.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Structure with the results of publishing.</returns>
        Task<PublishResourceResult> PublishResourceByIdsAsync(
            string tag,
            PublishResourceParams parameters,
            CancellationToken? cancellationToken);

        /// <summary>
        /// Publishes resource by Id.
        /// </summary>
        /// <param name="tag">Not used.</param>
        /// <param name="parameters">Parameters for publishing of resources.</param>
        /// <returns>Structure with the results of publishing.</returns>
        PublishResourceResult PublishResourceByIds(string tag, PublishResourceParams parameters);

        /// <summary>
        /// Updates access mode for the resources selected by tag asynchronously.
        /// </summary>
        /// <param name="tag">Update all resources with the given tag (up to a maximum
        /// of 100 matching original resources).</param>
        /// <param name="parameters">Parameters for updating of resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Structure with the results of update.</returns>
        Task<UpdateResourceAccessModeResult> UpdateResourceAccessModeByTagAsync(
            string tag,
            UpdateResourceAccessModeParams parameters,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Updates access mode for the resources selected by tag.
        /// </summary>
        /// <param name="tag">Update all resources with the given tag (up to a maximum
        /// of 100 matching original resources).</param>
        /// <param name="parameters">Parameters for updating of resources.</param>
        /// <returns>Structure with the results of update.</returns>
        UpdateResourceAccessModeResult UpdateResourceAccessModeByTag(string tag, UpdateResourceAccessModeParams parameters);

        /// <summary>
        /// Updates access mode for the resources selected by prefix asynchronously.
        /// </summary>
        /// <param name="prefix">Update all resources where the public ID starts with the given prefix (up to a maximum
        /// of 100 matching original resources).</param>
        /// <param name="parameters">Parameters for updating of resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Structure with the results of update.</returns>
        Task<UpdateResourceAccessModeResult> UpdateResourceAccessModeByPrefixAsync(
            string prefix,
            UpdateResourceAccessModeParams parameters,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Updates access mode for the resources selected by prefix.
        /// </summary>
        /// <param name="prefix">Update all resources where the public ID starts with the given prefix (up to a maximum
        /// of 100 matching original resources).</param>
        /// <param name="parameters">Parameters for updating of resources.</param>
        /// <returns>Structure with the results of update.</returns>
        UpdateResourceAccessModeResult UpdateResourceAccessModeByPrefix(
            string prefix,
            UpdateResourceAccessModeParams parameters);

        /// <summary>
        /// Updates access mode for the resources selected by public ids asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters for updating of resources. Update all resources with the given
        /// public IDs (array of up to 100 public_ids).</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Structure with the results of update.</returns>
        Task<UpdateResourceAccessModeResult> UpdateResourceAccessModeByIdsAsync(
            UpdateResourceAccessModeParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Updates access mode for the resources selected by public ids.
        /// </summary>
        /// <param name="parameters">Parameters for updating of resources. Update all resources with the given
        /// public IDs (array of up to 100 public_ids).</param>
        /// <returns>Structure with the results of update.</returns>
        UpdateResourceAccessModeResult UpdateResourceAccessModeByIds(UpdateResourceAccessModeParams parameters);

        /// <summary>
        /// Deletes derived resources by the given transformation (should be specified in parameters) asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to delete derived resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of deletion derived resources.</returns>
        Task<DelDerivedResResult> DeleteDerivedResourcesByTransformAsync(DelDerivedResParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Deletes derived resources by the given transformation (should be specified in parameters).
        /// </summary>
        /// <param name="parameters">Parameters to delete derived resources.</param>
        /// <returns>Parsed result of deletion derived resources.</returns>
        DelDerivedResResult DeleteDerivedResourcesByTransform(DelDerivedResParams parameters);

        /// <summary>
        /// Async call to get a list of folders in the root asynchronously.
        /// </summary>
        /// <param name="parameters">(optional) Parameters for managing folders list.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of folders listing.</returns>
        Task<GetFoldersResult> RootFoldersAsync(GetFoldersParams parameters = null, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets a list of folders in the root.
        /// </summary>
        /// <param name="parameters">(optional) Parameters for managing folders list.</param>
        /// <returns>Parsed result of folders listing.</returns>
        GetFoldersResult RootFolders(GetFoldersParams parameters = null);

        /// <summary>
        /// Gets a list of subfolders in a specified folder asynchronously.
        /// </summary>
        /// <param name="folder">The folder name.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of folders listing.</returns>
        Task<GetFoldersResult> SubFoldersAsync(string folder, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets a list of subfolders in a specified folder asynchronously.
        /// </summary>
        /// <param name="folder">The folder name.</param>
        /// <param name="parameters">(Optional) Parameters for managing folders list.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of folders listing.</returns>
        Task<GetFoldersResult> SubFoldersAsync(string folder, GetFoldersParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets a list of subfolders in a specified folder.
        /// </summary>
        /// <param name="folder">The folder name.</param>
        /// <param name="parameters">(Optional) Parameters for managing folders list.</param>
        /// <returns>Parsed result of folders listing.</returns>
        GetFoldersResult SubFolders(string folder, GetFoldersParams parameters = null);

        /// <summary>
        /// Deletes folder asynchronously.
        /// </summary>
        /// <param name="folder">Folder name.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of folder deletion.</returns>
        Task<DeleteFolderResult> DeleteFolderAsync(string folder, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Deletes folder.
        /// </summary>
        /// <param name="folder">Folder name.</param>
        /// <returns>Parsed result of folder deletion.</returns>
        DeleteFolderResult DeleteFolder(string folder);

        /// <summary>
        /// Create a new empty folder.
        /// </summary>
        /// <param name="folder">The full path of the new folder to create.</param>
        /// <returns>Parsed result of folder creation.</returns>
        CreateFolderResult CreateFolder(string folder);

        /// <summary>
        /// Create a new empty folder.
        /// </summary>
        /// <param name="folder">The full path of the new folder to create.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of folder creation.</returns>
        Task<CreateFolderResult> CreateFolderAsync(string folder, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Renames an existing asset folder.
        /// </summary>
        /// <param name="fromPath">The full path of the existing folder.</param>
        /// <param name="toPath">The full path of the new folder.</param>
        /// <returns>Parsed result of folder rename operation.</returns>
        public RenameFolderResult RenameFolder(string fromPath, string toPath);

        /// <summary>
        /// Renames an existing asset folder asynchronously.
        /// </summary>
        /// <param name="fromPath">The full path of the existing folder.</param>
        /// <param name="toPath">The full path of the new folder.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of folder creation.</returns>
        public Task<RenameFolderResult> RenameFolderAsync(string fromPath, string toPath, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Creates the upload preset.
        /// Upload presets allow you to define the default behavior for your uploads, instead of
        /// receiving these as parameters during the upload request itself. Upload presets have
        /// precedence over client-side upload parameters asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of the upload preset.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after manipulation of upload presets.</returns>
        Task<UploadPresetResult> CreateUploadPresetAsync(UploadPresetParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Creates the upload preset.
        /// Upload presets allow you to define the default behavior for your uploads, instead of receiving these as parameters during the upload request itself. Upload presets have precedence over client-side upload parameters.
        /// </summary>
        /// <param name="parameters">Parameters of the upload preset.</param>
        /// <returns>Parsed response after manipulation of upload presets.</returns>
        UploadPresetResult CreateUploadPreset(UploadPresetParams parameters);

        /// <summary>
        /// Updates the upload preset.
        /// Every update overwrites all the preset settings asynchronously.
        /// File specified as null because it's non-uploading action.
        /// </summary>
        /// <param name="parameters">New parameters for upload preset.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after manipulation of upload presets.</returns>
        Task<UploadPresetResult> UpdateUploadPresetAsync(UploadPresetParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Updates the upload preset.
        /// Every update overwrites all the preset settings.
        /// File specified as null because it's non-uploading action.
        /// </summary>
        /// <param name="parameters">New parameters for upload preset.</param>
        /// <returns>Parsed response after manipulation of upload presets.</returns>
        UploadPresetResult UpdateUploadPreset(UploadPresetParams parameters);

        /// <summary>
        /// Gets the upload preset asynchronously.
        /// </summary>
        /// <param name="name">Name of the upload preset.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Upload preset details.</returns>
        Task<GetUploadPresetResult> GetUploadPresetAsync(string name, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets the upload preset.
        /// </summary>
        /// <param name="name">Name of the upload preset.</param>
        /// <returns>Upload preset details.</returns>
        GetUploadPresetResult GetUploadPreset(string name);

        /// <summary>
        /// Lists upload presets asynchronously.
        /// </summary>
        /// <param name="nextCursor">Next cursor.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of upload presets listing.</returns>
        Task<ListUploadPresetsResult> ListUploadPresetsAsync(string nextCursor = null, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Lists upload presets asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to list upload presets.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of upload presets listing.</returns>
        Task<ListUploadPresetsResult> ListUploadPresetsAsync(ListUploadPresetsParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Lists upload presets.
        /// </summary>
        /// <param name="nextCursor">(Optional) Starting position.</param>
        /// <returns>Parsed result of upload presets listing.</returns>
        ListUploadPresetsResult ListUploadPresets(string nextCursor = null);

        /// <summary>
        /// Lists upload presets.
        /// </summary>
        /// <param name="parameters">Parameters to list upload presets.</param>
        /// <returns>Parsed result of upload presets listing.</returns>
        ListUploadPresetsResult ListUploadPresets(ListUploadPresetsParams parameters);

        /// <summary>
        /// Deletes the upload preset asynchronously.
        /// </summary>
        /// <param name="name">Name of the upload preset.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Result of upload preset deletion.</returns>
        Task<DeleteUploadPresetResult> DeleteUploadPresetAsync(string name, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Deletes the upload preset.
        /// </summary>
        /// <param name="name">Name of the upload preset.</param>
        /// <returns>Result of upload preset deletion.</returns>
        DeleteUploadPresetResult DeleteUploadPreset(string name);

        /// <summary>
        /// Gets the Cloudinary account usage details asynchronously.
        /// </summary>
        /// <param name="date">(Optional) The date for the usage report. Must be within the last 3 months.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>The report on the status of your Cloudinary account usage details.</returns>
        Task<UsageResult> GetUsageAsync(DateTime? date, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets the Cloudinary account usage details asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>The report on the status of your Cloudinary account usage details.</returns>
        Task<UsageResult> GetUsageAsync(CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets the Cloudinary account usage details.
        /// </summary>
        /// <param name="date">(Optional) The date for the usage report. Must be within the last 3 months.</param>
        /// <returns>The report on the status of your Cloudinary account usage details.</returns>
        UsageResult GetUsage(DateTime? date = null);

        /// <summary>
        /// Gets a list of tags asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed list of tags.</returns>
        Task<ListTagsResult> ListTagsAsync(CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets a list of tags asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of the request.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed list of tags.</returns>
        Task<ListTagsResult> ListTagsAsync(ListTagsParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets a list of all tags.
        /// </summary>
        /// <returns>Parsed list of tags.</returns>
        ListTagsResult ListTags();

        /// <summary>
        /// Gets a list of tags.
        /// </summary>
        /// <param name="parameters">Parameters of the request.</param>
        /// <returns>Parsed list of tags.</returns>
        ListTagsResult ListTags(ListTagsParams parameters);

        /// <summary>
        /// Finds all tags that start with the given prefix asynchronously.
        /// </summary>
        /// <param name="prefix">The tag prefix.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed list of tags.</returns>
        Task<ListTagsResult> ListTagsByPrefixAsync(string prefix, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Finds all tags that start with the given prefix.
        /// </summary>
        /// <param name="prefix">The tag prefix.</param>
        /// <returns>Parsed list of tags.</returns>
        ListTagsResult ListTagsByPrefix(string prefix);

        /// <summary>
        /// Gets a list of transformations asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed list of transformations details.</returns>
        Task<ListTransformsResult> ListTransformationsAsync(CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets a list of transformations asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of the request for a list of transformation.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed list of transformations details.</returns>
        Task<ListTransformsResult> ListTransformationsAsync(ListTransformsParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets a list of transformations.
        /// </summary>
        /// <returns>Parsed list of transformations details.</returns>
        ListTransformsResult ListTransformations();

        /// <summary>
        /// Gets a list of transformations.
        /// </summary>
        /// <param name="parameters">Parameters of the request for a list of transformation.</param>
        /// <returns>Parsed list of transformations details.</returns>
        ListTransformsResult ListTransformations(ListTransformsParams parameters);

        /// <summary>
        /// Gets details of a single transformation asynchronously.
        /// </summary>
        /// <param name="transform">Name of the transformation.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed details of a single transformation.</returns>
        Task<GetTransformResult> GetTransformAsync(string transform, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets details of a single transformation asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of the request of transformation details.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed details of a single transformation.</returns>
        Task<GetTransformResult> GetTransformAsync(GetTransformParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets details of a single transformation by name.
        /// </summary>
        /// <param name="transform">Name of the transformation.</param>
        /// <returns>Parsed details of a single transformation.</returns>
        GetTransformResult GetTransform(string transform);

        /// <summary>
        /// Gets details of a single transformation.
        /// </summary>
        /// <param name="parameters">Parameters of the request of transformation details.</param>
        /// <returns>Parsed details of a single transformation.</returns>
        GetTransformResult GetTransform(GetTransformParams parameters);

        /// <summary>
        ///  Analyzes an asset with the requested analysis type asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of analysis.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Detailed analysis information.</returns>
        Task<AnalyzeResult> AnalyzeAsync(AnalyzeParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        ///  Analyzes an asset with the requested analysis type.
        /// </summary>
        /// <param name="parameters">Parameters of analysis.</param>
        /// <returns>Detailed analysis information .</returns>
        AnalyzeResult Analyze(AnalyzeParams parameters);

        /// <summary>
        /// Updates details of an existing resource asynchronously.
        /// </summary>
        /// <param name="publicId">The public ID of the resource to update.</param>
        /// <param name="moderationStatus">The image moderation status.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response of the detailed resource information.</returns>
        Task<GetResourceResult> UpdateResourceAsync(string publicId, ModerationStatus moderationStatus, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Updates details of an existing resource asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to update details of an existing resource.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response of the detailed resource information.</returns>
        Task<GetResourceResult> UpdateResourceAsync(UpdateParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Updates details of an existing resource.
        /// </summary>
        /// <param name="publicId">The public ID of the resource to update.</param>
        /// <param name="moderationStatus">The image moderation status.</param>
        /// <returns>Parsed response of the detailed resource information.</returns>
        GetResourceResult UpdateResource(string publicId, ModerationStatus moderationStatus);

        /// <summary>
        /// Updates details of an existing resource.
        /// </summary>
        /// <param name="parameters">Parameters to update details of an existing resource.</param>
        /// <returns>Parsed response of the detailed resource information.</returns>
        GetResourceResult UpdateResource(UpdateParams parameters);

        /// <summary>
        /// Relates a resource to other resources by public IDs asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to relate resource to other resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<AddRelatedResourcesResult> AddRelatedResourcesAsync(
            AddRelatedResourcesParams parameters,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Relates a resource to other resources by public IDs.
        /// </summary>
        /// <param name="parameters">Parameters to relate resource to other resources.</param>
        /// <returns>Parsed response of the detailed resource information.</returns>
        AddRelatedResourcesResult AddRelatedResources(AddRelatedResourcesParams parameters);

        /// <summary>
        /// Relates a resource to other resources by asset IDs asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to relate resource to other resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<AddRelatedResourcesResult> AddRelatedResourcesByAssetIdsAsync(
            AddRelatedResourcesByAssetIdsParams parameters,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Relates a resource to other resources by asset IDs.
        /// </summary>
        /// <param name="parameters">Parameters to relate resource to other resources.</param>
        /// <returns>Parsed response of the detailed resource information.</returns>
        AddRelatedResourcesResult AddRelatedResourcesByAssetIds(AddRelatedResourcesByAssetIdsParams parameters);

        /// <summary>
        /// Unrelates a resource from other resources by public IDs asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to unrelate resource from other resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<DeleteRelatedResourcesResult> DeleteRelatedResourcesAsync(
            DeleteRelatedResourcesParams parameters,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Unrelates a resource from other resources by public IDs.
        /// </summary>
        /// <param name="parameters">Parameters to unrelate resource from other resources.</param>
        /// <returns>Parsed response of the detailed resource information.</returns>
        DeleteRelatedResourcesResult DeleteRelatedResources(DeleteRelatedResourcesParams parameters);

        /// <summary>
        /// Unrelates a resource from other resources by asset IDs asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to relate resource to other resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<DeleteRelatedResourcesResult> DeleteRelatedResourcesByAssetIdsAsync(
            DeleteRelatedResourcesByAssetIdsParams parameters,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Unrelates a resource from other resources by asset IDs.
        /// </summary>
        /// <param name="parameters">Parameters to relate resource to other resources.</param>
        /// <returns>Parsed response of the detailed resource information.</returns>
        DeleteRelatedResourcesResult DeleteRelatedResourcesByAssetIds(DeleteRelatedResourcesByAssetIdsParams parameters);

        /// <summary>
        /// Gets details of a single resource as well as all its derived resources by its public ID asynchronously.
        /// </summary>
        /// <param name="assetId">The asset ID of the resource.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response with the detailed resource information.</returns>
        Task<GetResourceResult> GetResourceByAssetIdAsync(string assetId, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets details of a single resource as well as all its derived resources by its public ID.
        /// </summary>
        /// <param name="assetId">The asset ID of the resource.</param>
        /// <returns>Parsed response with the detailed resource information.</returns>
        GetResourceResult GetResourceByAssetId(string assetId);

        /// <summary>
        /// Gets details of a single resource as well as all its derived resources by its public ID asynchronously.
        /// </summary>
        /// <param name="publicId">The public ID of the resource.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response with the detailed resource information.</returns>
        Task<GetResourceResult> GetResourceAsync(string publicId, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets details of the requested resource as well as all its derived resources asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of the request of resource.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response with the detailed resource information.</returns>
        Task<GetResourceResult> GetResourceAsync(GetResourceParamsBase parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets details of a single resource as well as all its derived resources by its public ID.
        /// </summary>
        /// <param name="publicId">The public ID of the resource.</param>
        /// <returns>Parsed response with the detailed resource information.</returns>
        GetResourceResult GetResource(string publicId);

        /// <summary>
        /// Gets details of the requested resource as well as all its derived resources.
        /// </summary>
        /// <param name="parameters">Parameters of the request of resource.</param>
        /// <returns>Parsed response with the detailed resource information.</returns>
        GetResourceResult GetResource(GetResourceParams parameters);

        /// <summary>
        /// Deletes all derived resources with the given IDs asynchronously.
        /// </summary>
        /// <param name="ids">An array of up to 100 derived_resource_ids.</param>
        /// <returns>Parsed result of deletion derived resources.</returns>
        Task<DelDerivedResResult> DeleteDerivedResourcesAsync(params string[] ids);

        /// <summary>
        /// Deletes all derived resources with the given parameters asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to delete derived resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of deletion derived resources.</returns>
        Task<DelDerivedResResult> DeleteDerivedResourcesAsync(DelDerivedResParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Deletes all derived resources with the given IDs.
        /// </summary>
        /// <param name="ids">An array of up to 100 derived_resource_ids.</param>
        /// <returns>Parsed result of deletion derived resources.</returns>
        DelDerivedResResult DeleteDerivedResources(params string[] ids);

        /// <summary>
        /// Deletes all derived resources with the given parameters.
        /// </summary>
        /// <param name="parameters">Parameters to delete derived resources.</param>
        /// <returns>Parsed result of deletion derived resources.</returns>
        DelDerivedResResult DeleteDerivedResources(DelDerivedResParams parameters);

        /// <summary>
        /// Deletes all resources of the given resource type and with the given public IDs asynchronously.
        /// </summary>
        /// <param name="type">The type of file to delete. Default: image.</param>
        /// <param name="publicIds">Array of up to 100 public_ids.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        Task<DelResResult> DeleteResourcesAsync(ResourceType type, params string[] publicIds);

        /// <summary>
        /// Deletes all resources with the given public IDs asynchronously.
        /// </summary>
        /// <param name="publicIds">Array of up to 100 public_ids.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        Task<DelResResult> DeleteResourcesAsync(params string[] publicIds);

        /// <summary>
        /// Deletes all resources with parameters asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters for deletion resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        Task<DelResResult> DeleteResourcesAsync(DelResParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Deletes all resources of the given resource type and with the given public IDs.
        /// </summary>
        /// <param name="type">The type of file to delete. Default: image.</param>
        /// <param name="publicIds">Array of up to 100 public_ids.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        DelResResult DeleteResources(ResourceType type, params string[] publicIds);

        /// <summary>
        /// Deletes all resources with the given public IDs.
        /// </summary>
        /// <param name="publicIds">Array of up to 100 public_ids.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        DelResResult DeleteResources(params string[] publicIds);

        /// <summary>
        /// Deletes all resources with parameters.
        /// </summary>
        /// <param name="parameters">Parameters for deletion resources.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        DelResResult DeleteResources(DelResParams parameters);

        /// <summary>
        /// Deletes all resources, including derived resources, where the public ID starts with the given prefix (up to
        /// a maximum of 1000 original resources) asynchronously.
        /// </summary>
        /// <param name="prefix">Delete all resources where the public ID starts with the given prefix. </param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        Task<DelResResult> DeleteResourcesByPrefixAsync(string prefix, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Deletes all resources, including derived resources, where the public ID starts with the given prefix (up to
        /// a maximum of 1000 original resources) asynchronously.
        /// </summary>
        /// <param name="prefix">Delete all resources where the public ID starts with the given prefix. </param>
        /// <param name="keepOriginal">If true, delete only the derived images of the matching resources.</param>
        /// <param name="nextCursor">Continue deletion from the given cursor.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        Task<DelResResult> DeleteResourcesByPrefixAsync(string prefix, bool keepOriginal, string nextCursor, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Deletes all resources, including derived resources, where the public ID starts with the given prefix (up to
        /// a maximum of 1000 original resources).
        /// </summary>
        /// <param name="prefix">Delete all resources where the public ID starts with the given prefix. </param>
        /// <returns>Parsed result of deletion resources.</returns>
        DelResResult DeleteResourcesByPrefix(string prefix);

        /// <summary>
        /// Deletes all resources, including derived resources, where the public ID starts with the given prefix (up to
        /// a maximum of 1000 original resources).
        /// </summary>
        /// <param name="prefix">Delete all resources where the public ID starts with the given prefix. </param>
        /// <param name="keepOriginal">If true, delete only the derived images of the matching resources.</param>
        /// <param name="nextCursor">Continue deletion from the given cursor.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        DelResResult DeleteResourcesByPrefix(string prefix, bool keepOriginal, string nextCursor);

        /// <summary>
        /// Deletes resources by the given tag name asynchronously.
        /// </summary>
        /// <param name="tag">
        /// Delete all resources (and their derivatives) with the given tag name (up to a maximum of
        /// 1000 original resources).
        /// </param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        Task<DelResResult> DeleteResourcesByTagAsync(string tag, CancellationToken? cancellationToken = null);

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
        Task<DelResResult> DeleteResourcesByTagAsync(string tag, bool keepOriginal, string nextCursor, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Deletes resources by the given tag name.
        /// </summary>
        /// <param name="tag">
        /// Delete all resources (and their derivatives) with the given tag name (up to a maximum of
        /// 1000 original resources).
        /// </param>
        /// <returns>Parsed result of deletion resources.</returns>
        DelResResult DeleteResourcesByTag(string tag);

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
        DelResResult DeleteResourcesByTag(string tag, bool keepOriginal, string nextCursor);

        /// <summary>
        /// Deletes all resources asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        Task<DelResResult> DeleteAllResourcesAsync(CancellationToken? cancellationToken = null);

        /// <summary>
        /// Deletes all resources with conditions asynchronously.
        /// </summary>
        /// <param name="keepOriginal">If true, delete only the derived resources.</param>
        /// <param name="nextCursor">
        /// Value of the <see cref="DelResResult.NextCursor"/> to continue delete from.
        /// </param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of deletion resources.</returns>
        Task<DelResResult> DeleteAllResourcesAsync(bool keepOriginal, string nextCursor, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Deletes all resources.
        /// </summary>
        /// <returns>Parsed result of deletion resources.</returns>
        DelResResult DeleteAllResources();

        /// <summary>
        /// Deletes all resources with conditions.
        /// </summary>
        /// <param name="keepOriginal">If true, delete only the derived resources.</param>
        /// <param name="nextCursor">
        /// Value of the <see cref="DelResResult.NextCursor"/> to continue delete from.
        /// </param>
        /// <returns>Parsed result of deletion resources.</returns>
        DelResResult DeleteAllResources(bool keepOriginal, string nextCursor);

        /// <summary>
        /// Restores a deleted resources by array of public ids asynchronously.
        /// </summary>
        /// <param name="publicIds">The public IDs of (deleted or existing) backed up resources to restore.</param>
        /// <returns>Parsed result of restoring resources.</returns>
        Task<RestoreResult> RestoreAsync(params string[] publicIds);

        /// <summary>
        /// Restores a deleted resources asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters to restore a deleted resources.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of restoring resources.</returns>
        Task<RestoreResult> RestoreAsync(RestoreParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Restores a deleted resources by array of public ids.
        /// </summary>
        /// <param name="publicIds">The public IDs of (deleted or existing) backed up resources to restore.</param>
        /// <returns>Parsed result of restoring resources.</returns>
        RestoreResult Restore(params string[] publicIds);

        /// <summary>
        /// Restores a deleted resources.
        /// </summary>
        /// <param name="parameters">Parameters to restore a deleted resources.</param>
        /// <returns>Parsed result of restoring resources.</returns>
        RestoreResult Restore(RestoreParams parameters);

        /// <summary>
        /// Returns list of all upload mappings asynchronously.
        /// </summary>
        /// <param name="parameters">
        /// Uses only <see cref="UploadMappingParams.MaxResults"/> and <see cref="UploadMappingParams.NextCursor"/>
        /// properties. Can be null.
        /// </param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        Task<UploadMappingResults> UploadMappingsAsync(UploadMappingParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Returns list of all upload mappings.
        /// </summary>
        /// <param name="parameters">
        /// Uses only <see cref="UploadMappingParams.MaxResults"/> and <see cref="UploadMappingParams.NextCursor"/>
        /// properties. Can be null.
        /// </param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        UploadMappingResults UploadMappings(UploadMappingParams parameters);

        /// <summary>
        /// Returns single upload mapping by <see cref="Folder"/> name asynchronously.
        /// </summary>
        /// <param name="folder">Folder name.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        Task<UploadMappingResults> UploadMappingAsync(string folder, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Returns single upload mapping by <see cref="Folder"/> name.
        /// </summary>
        /// <param name="folder">Folder name.</param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        UploadMappingResults UploadMapping(string folder);

        /// <summary>
        /// Creates a new upload mapping folder and its template (URL) asynchronously.
        /// </summary>
        /// <param name="folder">Folder name to create.</param>
        /// <param name="template">URL template for mapping to the <paramref name="folder"/>.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        Task<UploadMappingResults> CreateUploadMappingAsync(string folder, string template, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Creates a new upload mapping folder and its template (URL).
        /// </summary>
        /// <param name="folder">Folder name to create.</param>
        /// <param name="template">URL template for mapping to the <paramref name="folder"/>.</param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        UploadMappingResults CreateUploadMapping(string folder, string template);

        /// <summary>
        /// Updates existing upload mapping asynchronously.
        /// </summary>
        /// <param name="folder">Existing Folder to be updated.</param>
        /// <param name="newTemplate">New value of Template URL.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after Upload mappings update.</returns>
        Task<UploadMappingResults> UpdateUploadMappingAsync(string folder, string newTemplate, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Updates existing upload mapping.
        /// </summary>
        /// <param name="folder">Existing Folder to be updated.</param>
        /// <param name="newTemplate">New value of Template URL.</param>
        /// <returns>Parsed response after Upload mappings update.</returns>
        UploadMappingResults UpdateUploadMapping(string folder, string newTemplate);

        /// <summary>
        /// Deletes all upload mappings asynchronously.
        /// </summary>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after Upload mappings delete.</returns>
        Task<UploadMappingResults> DeleteUploadMappingAsync(CancellationToken? cancellationToken = null);

        /// <summary>
        /// Deletes upload mapping by <paramref name="folder"/> name asynchronously.
        /// </summary>
        /// <param name="folder">Folder name.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        Task<UploadMappingResults> DeleteUploadMappingAsync(string folder, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Deletes all upload mappings.
        /// </summary>
        /// <returns>Parsed response after Upload mappings delete.</returns>
        UploadMappingResults DeleteUploadMapping();

        /// <summary>
        /// Deletes upload mapping by <paramref name="folder"/> name.
        /// </summary>
        /// <param name="folder">Folder name.</param>
        /// <returns>Parsed response after Upload mappings manipulation.</returns>
        UploadMappingResults DeleteUploadMapping(string folder);

        /// <summary>
        /// Updates Cloudinary transformation resource asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters for transformation update.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        Task<UpdateTransformResult> UpdateTransformAsync(UpdateTransformParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Updates Cloudinary transformation resource.
        /// </summary>
        /// <param name="parameters">Parameters for transformation update.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        UpdateTransformResult UpdateTransform(UpdateTransformParams parameters);

        /// <summary>
        /// Creates Cloudinary transformation resource asynchronously.
        /// </summary>
        /// <param name="parameters">Parameters of the new transformation.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        Task<TransformResult> CreateTransformAsync(CreateTransformParams parameters, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Creates Cloudinary transformation resource.
        /// </summary>
        /// <param name="parameters">Parameters of the new transformation.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        TransformResult CreateTransform(CreateTransformParams parameters);

        /// <summary>
        /// Deletes transformation by name asynchronously.
        /// </summary>
        /// <param name="transformName">The name of transformation to delete.</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        Task<TransformResult> DeleteTransformAsync(string transformName, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Deletes transformation by name.
        /// </summary>
        /// <param name="transformName">The name of transformation to delete.</param>
        /// <returns>Parsed response after transformation manipulation.</returns>
        TransformResult DeleteTransform(string transformName);
    }
}
