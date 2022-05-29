namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;

    /// <summary>
    /// Part of Cloudinary .NET API main class, responsible for metadata fields management.
    /// </summary>
    public partial class Cloudinary
    {
        /// <summary>
        /// Create a new metadata field definition.
        /// </summary>
        /// <param name="parameters">Parameters of the metadata field.</param>
        /// <returns>Parsed result of the metadata field creating.</returns>
        /// <typeparam name="T">Type of the metadata field.</typeparam>
        public MetadataFieldResult AddMetadataField<T>(MetadataFieldCreateParams<T> parameters) =>
            AddMetadataFieldAsync(parameters).GetAwaiter().GetResult();

        /// <summary>
        /// Create a new metadata field definition.
        /// </summary>
        /// <param name="parameters">Parameters of the metadata field.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>Parsed result of the metadata field creating.</returns>
        /// <typeparam name="T">Type of the metadata field.</typeparam>
        public Task<MetadataFieldResult> AddMetadataFieldAsync<T>(
                MetadataFieldCreateParams<T> parameters,
                CancellationToken? cancellationToken = null)
        {
            var url = m_api.ApiUrlMetadataFieldV.BuildUrl();
            return CallAdminApiAsync<MetadataFieldResult>(HttpMethod.POST, url, parameters, cancellationToken, PrepareHeaders());
        }

        /// <summary>
        /// Retrieve a list of all metadata field definitions as an array of JSON objects.
        /// </summary>
        /// <returns>Parsed list of metadata field definitions.</returns>
        public MetadataFieldListResult ListMetadataFields() =>
            ListMetadataFieldsAsync().GetAwaiter().GetResult();

        /// <summary>
        /// Retrieve a list of all metadata field definitions as an array of JSON objects.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>Parsed list of metadata field definitions.</returns>
        public Task<MetadataFieldListResult> ListMetadataFieldsAsync(CancellationToken? cancellationToken = null)
        {
            return CallAdminApiAsync<MetadataFieldListResult>(
                HttpMethod.GET, m_api.ApiUrlMetadataFieldV.BuildUrl(), null, cancellationToken);
        }

        /// <summary>
        /// Retrieve a single metadata field definition.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <returns>Parsed information about metadata field.</returns>
        public MetadataFieldResult GetMetadataField(string fieldExternalId) =>
            GetMetadataFieldAsync(fieldExternalId).GetAwaiter().GetResult();

        /// <summary>
        /// Retrieve a single metadata field definition.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>Parsed information about metadata field.</returns>
        public Task<MetadataFieldResult> GetMetadataFieldAsync(
                string fieldExternalId,
                CancellationToken? cancellationToken = null)
        {
            var url = m_api.ApiUrlMetadataFieldV.Add(fieldExternalId).BuildUrl();
            return CallAdminApiAsync<MetadataFieldResult>(HttpMethod.GET, url, null, cancellationToken);
        }

        /// <summary>
        /// Update a metadata field by its external ID.
        /// There is no need to pass the entire object, only properties to be updated.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="parameters">Parameters of the metadata field to be updated.</param>
        /// <returns>Parsed result of the operation.</returns>
        /// <typeparam name="T">Type of the metadata field.</typeparam>
        public MetadataFieldResult UpdateMetadataField<T>(string fieldExternalId, MetadataFieldUpdateParams<T> parameters) =>
            UpdateMetadataFieldAsync(fieldExternalId, parameters).GetAwaiter().GetResult();

        /// <summary>
        /// Update a metadata field by its external ID.
        /// There is no need to pass the entire object, only properties to be updated.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="parameters">Parameters of the metadata field to be updated.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>Parsed result of the operation.</returns>
        /// <typeparam name="T">Type of the metadata field.</typeparam>
        public Task<MetadataFieldResult> UpdateMetadataFieldAsync<T>(
                string fieldExternalId,
                MetadataFieldUpdateParams<T> parameters,
                CancellationToken? cancellationToken = null)
        {
            if (string.IsNullOrEmpty(fieldExternalId))
            {
                throw new ArgumentNullException(nameof(fieldExternalId));
            }

            var url = m_api.ApiUrlMetadataFieldV.Add(fieldExternalId).BuildUrl();
            return CallAdminApiAsync<MetadataFieldResult>(
                    HttpMethod.PUT, url, parameters, cancellationToken, PrepareHeaders());
        }

        /// <summary>
        /// Update the datasource of a supported field type (currently only enum and set).
        /// The update is partial: datasource entries with an existing external_id will be updated.
        /// Entries with new external_id’s (or without external_id’s) will be appended.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="parameters">Parameters of the datasource to be updated.</param>
        /// <returns>Parsed result of the operation.</returns>
        public MetadataDataSourceResult UpdateMetadataDataSourceEntries(string fieldExternalId, MetadataDataSourceParams parameters) =>
            UpdateMetadataDataSourceEntriesAsync(fieldExternalId, parameters).GetAwaiter().GetResult();

        /// <summary>
        /// Update the datasource of a supported field type (currently only enum and set).
        /// The update is partial: datasource entries with an existing external_id will be updated.
        /// Entries with new external_id’s (or without external_id’s) will be appended.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="parameters">Parameters of the datasource to be updated.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>Parsed result of the operation.</returns>
        public Task<MetadataDataSourceResult> UpdateMetadataDataSourceEntriesAsync(
                string fieldExternalId,
                MetadataDataSourceParams parameters,
                CancellationToken? cancellationToken = null)
        {
            if (string.IsNullOrEmpty(fieldExternalId))
            {
                throw new ArgumentNullException(nameof(fieldExternalId));
            }

            var url = m_api.ApiUrlMetadataFieldV.Add(fieldExternalId).Add(Constants.DATASOURCE_MANAGMENT).BuildUrl();

            return CallAdminApiAsync<MetadataDataSourceResult>(
                    HttpMethod.PUT, url, parameters, cancellationToken, PrepareHeaders());
        }

        /// <summary>
        /// Delete a metadata field definition.
        /// The field should no longer be considered a valid candidate for all other endpoints.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <returns>Parsed result of the operation.</returns>
        public DelMetadataFieldResult DeleteMetadataField(string fieldExternalId) =>
            DeleteMetadataFieldAsync(fieldExternalId).GetAwaiter().GetResult();

        /// <summary>
        /// Delete a metadata field definition.
        /// The field should no longer be considered a valid candidate for all other endpoints.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>Parsed result of the operation.</returns>
        public Task<DelMetadataFieldResult> DeleteMetadataFieldAsync(
                string fieldExternalId,
                CancellationToken? cancellationToken = null)
        {
            if (string.IsNullOrEmpty(fieldExternalId))
            {
                throw new ArgumentNullException(nameof(fieldExternalId));
            }

            var url = m_api.ApiUrlMetadataFieldV.Add(fieldExternalId).BuildUrl();
            return CallAdminApiAsync<DelMetadataFieldResult>(HttpMethod.DELETE, url, null, cancellationToken);
        }

        /// <summary>
        /// Delete datasource entries for a specified metadata field definition.
        /// This operation sets the state of the entries to inactive.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="entriesExternalIds">An array of IDs of datasource entries to delete.</param>
        /// <returns>Parsed result of the operation.</returns>
        public MetadataDataSourceResult DeleteMetadataDataSourceEntries(string fieldExternalId, List<string> entriesExternalIds) =>
            DeleteMetadataDataSourceEntriesAsync(fieldExternalId, entriesExternalIds).GetAwaiter().GetResult();

        /// <summary>
        /// Delete datasource entries for a specified metadata field definition.
        /// This operation sets the state of the entries to inactive.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="entriesExternalIds">An array of IDs of datasource entries to delete.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>Parsed result of the operation.</returns>
        public Task<MetadataDataSourceResult> DeleteMetadataDataSourceEntriesAsync(
                string fieldExternalId,
                List<string> entriesExternalIds,
                CancellationToken? cancellationToken = null)
        {
            var url = PrepareUrlForDatasourceOperation(fieldExternalId, entriesExternalIds, Constants.DATASOURCE_MANAGMENT);
            return CallAdminApiAsync<MetadataDataSourceResult>(HttpMethod.DELETE, url, null, cancellationToken);
        }

        /// <summary>
        /// Restore (unblock) any previously deleted datasource entries for a specified metadata field definition
        ///  and set the state of the entries to active.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="entriesExternalIds">An array of IDs of datasource entries to restore.</param>
        /// <returns>Parsed result of the operation.</returns>
        public MetadataDataSourceResult RestoreMetadataDataSourceEntries(string fieldExternalId, List<string> entriesExternalIds) =>
            RestoreMetadataDataSourceEntriesAsync(fieldExternalId, entriesExternalIds)
                .GetAwaiter().GetResult();

        /// <summary>
        /// Restore (unblock) any previously deleted datasource entries for a specified metadata field definition
        ///  and set the state of the entries to active.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="entriesExternalIds">An array of IDs of datasource entries to restore.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>Parsed result of the operation.</returns>
        public Task<MetadataDataSourceResult> RestoreMetadataDataSourceEntriesAsync(
                string fieldExternalId,
                List<string> entriesExternalIds,
                CancellationToken? cancellationToken = null)
        {
            var url = PrepareUrlForDatasourceOperation(fieldExternalId, entriesExternalIds, $"{Constants.DATASOURCE_MANAGMENT}_restore");
            return CallAdminApiAsync<MetadataDataSourceResult>(HttpMethod.POST, url, null, cancellationToken);
        }

        /// <summary>
        /// Populate metadata fields with the given values. Existing values will be overwritten.
        ///
        /// Any metadata-value pairs given are merged with any existing metadata-value pairs
        /// (an empty value for an existing metadata field clears the value).
        /// </summary>
        /// <param name="parameters">Values to be applied to metadata fields of uploaded assets.</param>
        /// <returns>Parsed result of the operation.</returns>
        public MetadataUpdateResult UpdateMetadata(MetadataUpdateParams parameters) =>
            UpdateMetadataAsync(parameters).GetAwaiter().GetResult();

        /// <summary>
        /// Populate metadata fields with the given values. Existing values will be overwritten.
        ///
        /// Any metadata-value pairs given are merged with any existing metadata-value pairs
        /// (an empty value for an existing metadata field clears the value).
        /// </summary>
        /// <param name="parameters">Values to be applied to metadata fields of uploaded assets.</param>
        /// <param name="cancellationToken">The cancellation token (optional).</param>
        /// <returns>Parsed result of the operation.</returns>
        public Task<MetadataUpdateResult> UpdateMetadataAsync(
                MetadataUpdateParams parameters,
                CancellationToken? cancellationToken = null)
        {
            var url = GetApiUrlV().
                Add(Api.GetCloudinaryParam(parameters.ResourceType)).
                Add(Constants.METADATA).
                BuildUrl();
            return CallAdminApiAsync<MetadataUpdateResult>(HttpMethod.POST, url, parameters, cancellationToken);
        }

        /// <summary>
        /// Reorders metadata fields.
        /// </summary>
        /// <param name="orderBy">Criteria for the order.</param>
        /// <param name="direction">(Optional) Direction.</param>
        /// <returns>Some result.</returns>
        public Task<MetadataFieldListResult> ReorderMetadataFieldsAsync(
                MetadataReorderBy orderBy,
                MetadataReorderDirection? direction = null)
        {
            var url = m_api.ApiUrlMetadataFieldV.Add("order").BuildUrl();
            var parameters = new MetadataReorderParams { OrderBy = orderBy, Direction = direction };
            return CallAdminApiAsync<MetadataFieldListResult>(HttpMethod.PUT, url, parameters, null);
        }

        /// <summary>
        /// Reorders metadata fields.
        /// </summary>
        /// <param name="orderBy">Criteria for the order.</param>
        /// <param name="direction">(Optional) Direction.</param>
        /// <returns>Some result.</returns>
        public MetadataFieldListResult ReorderMetadataFields(
                MetadataReorderBy orderBy,
                MetadataReorderDirection? direction = null)
        {
            return ReorderMetadataFieldsAsync(orderBy, direction).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Reorders metadata field datasource. Currently, supports only value.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="orderBy">Criteria for the sort. Currently supports only value.</param>
        /// <param name="direction">Optional (gets either asc or desc).</param>
        /// <param name="cancellationToken">(Optional) Cancellation token.</param>
        /// <returns>Parsed result of the operation.</returns>
        public Task<MetadataDataSourceResult> ReorderMetadataFieldDatasourceAsync(
            string fieldExternalId,
            string orderBy,
            string direction = null,
            CancellationToken? cancellationToken = null)
        {
            var url = m_api.ApiUrlMetadataFieldV
                .Add(fieldExternalId)
                .Add(Constants.DATASOURCE_MANAGMENT)
                .Add("order")
                .BuildUrl();

            var parameters = new ReorderMetadataFieldsParams()
            {
                OrderBy = orderBy,
                Direction = direction,
            };

            return CallAdminApiAsync<MetadataDataSourceResult>(HttpMethod.POST, url, parameters, cancellationToken);
        }

        /// <summary>
        /// Reorders metadata field datasource. Currently, supports only value.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="orderBy">Criteria for the sort. Currently supports only value.</param>
        /// <param name="direction">Optional (gets either asc or desc).</param>
        /// <returns>Parsed result of the operation.</returns>
        public MetadataDataSourceResult ReorderMetadataFieldDatasource(
            string fieldExternalId,
            string orderBy,
            string direction = null) =>
            ReorderMetadataFieldDatasourceAsync(fieldExternalId, orderBy, direction).GetAwaiter().GetResult();

        private static Dictionary<string, string> PrepareHeaders()
        {
            var extraHeaders = new Dictionary<string, string>
            {
                {
                    Constants.HEADER_CONTENT_TYPE,
                    Constants.CONTENT_TYPE_APPLICATION_JSON
                },
            };

            return extraHeaders;
        }

        private string PrepareUrlForDatasourceOperation(string fieldExternalId, List<string> entriesExternalIds, string actionName)
        {
            if (string.IsNullOrEmpty(fieldExternalId))
            {
                throw new ArgumentNullException(nameof(fieldExternalId));
            }

            var parameters = new DataSourceEntriesParams(entriesExternalIds);
            var urlBuilder = new UrlBuilder(
                m_api.ApiUrlMetadataFieldV.Add(fieldExternalId).Add(actionName).BuildUrl(),
                parameters.ToParamsDictionary());
            var url = urlBuilder.ToString();
            return url;
        }
    }
}
