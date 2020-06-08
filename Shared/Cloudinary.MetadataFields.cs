namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;
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
        public MetadataFieldResult AddMetadataField<T>(MetadataFieldCreateParams<T> parameters)
        {
            var url = m_api.ApiUrlMetadataFieldV.BuildUrl();
            var result = m_api.CallApi<MetadataFieldResult>(HttpMethod.POST, url, parameters, null, PrepareHeaders());
            return result;
        }

        /// <summary>
        /// Retrieve a list of all metadata field definitions as an array of JSON objects.
        /// </summary>
        /// <returns>Parsed list of metadata field definitions.</returns>
        public MetadataFieldListResult ListMetadataFields()
        {
            var result = m_api.CallApi<MetadataFieldListResult>(
                HttpMethod.GET, m_api.ApiUrlMetadataFieldV.BuildUrl(), null, null);
            return result;
        }

        /// <summary>
        /// Retrieve a single metadata field definition.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <returns>Parsed information about metadata field.</returns>
        public MetadataFieldResult GetMetadataField(string fieldExternalId)
        {
            var url = m_api.ApiUrlMetadataFieldV.Add(fieldExternalId).BuildUrl();
            var result = m_api.CallApi<MetadataFieldResult>(HttpMethod.GET, url, null, null);
            return result;
        }

        /// <summary>
        /// Update a metadata field by its external ID.
        /// There is no need to pass the entire object, only properties to be updated.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="parameters">Parameters of the metadata field to be updated.</param>
        /// <returns>Parsed result of the operation.</returns>
        /// <typeparam name="T">Type of the metadata field.</typeparam>
        public MetadataFieldResult UpdateMetadataField<T>(string fieldExternalId, MetadataFieldUpdateParams<T> parameters)
        {
            if (string.IsNullOrEmpty(fieldExternalId))
            {
                throw new ArgumentNullException(nameof(fieldExternalId));
            }

            var url = m_api.ApiUrlMetadataFieldV.Add(fieldExternalId).BuildUrl();
            var result = m_api.CallApi<MetadataFieldResult>(HttpMethod.PUT, url, parameters, null, PrepareHeaders());
            return result;
        }

        /// <summary>
        /// Update the datasource of a supported field type (currently only enum and set).
        /// The update is partial: datasource entries with an existing external_id will be updated.
        /// Entries with new external_id’s (or without external_id’s) will be appended.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="parameters">Parameters of the datasource to be updated.</param>
        /// <returns>Parsed result of the operation.</returns>
        public MetadataDataSourceResult UpdateMetadataDataSourceEntries(string fieldExternalId, MetadataDataSourceParams parameters)
        {
            if (string.IsNullOrEmpty(fieldExternalId))
            {
                throw new ArgumentNullException(nameof(fieldExternalId));
            }

            var url = m_api.ApiUrlMetadataFieldV.Add(fieldExternalId).Add(Constants.DATASOURCE_MANAGMENT).BuildUrl();
            var result = m_api.CallApi<MetadataDataSourceResult>(HttpMethod.PUT, url, parameters, null, PrepareHeaders());
            return result;
        }

        /// <summary>
        /// Delete a metadata field definition.
        /// The field should no longer be considered a valid candidate for all other endpoints.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <returns>Parsed result of the operation.</returns>
        public DelMetadataFieldResult DeleteMetadataField(string fieldExternalId)
        {
            if (string.IsNullOrEmpty(fieldExternalId))
            {
                throw new ArgumentNullException(nameof(fieldExternalId));
            }

            var url = m_api.ApiUrlMetadataFieldV.Add(fieldExternalId).BuildUrl();
            var result = m_api.CallApi<DelMetadataFieldResult>(HttpMethod.DELETE, url, null, null);
            return result;
        }

        /// <summary>
        /// Delete datasource entries for a specified metadata field definition.
        /// This operation sets the state of the entries to inactive.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="entriesExternalIds">An array of IDs of datasource entries to delete.</param>
        /// <returns>Parsed result of the operation.</returns>
        public MetadataDataSourceResult DeleteMetadataDataSourceEntries(string fieldExternalId, List<string> entriesExternalIds)
        {
            var url = PrepareUrlForDatasourceOperation(fieldExternalId, entriesExternalIds, Constants.DATASOURCE_MANAGMENT);
            var result = m_api.CallApi<MetadataDataSourceResult>(HttpMethod.DELETE, url, null, null);
            return result;
        }

        /// <summary>
        /// Restore (unblock) any previously deleted datasource entries for a specified metadata field definition
        ///  and set the state of the entries to active.
        /// </summary>
        /// <param name="fieldExternalId">The ID of the metadata field.</param>
        /// <param name="entriesExternalIds">An array of IDs of datasource entries to restore.</param>
        /// <returns>Parsed result of the operation.</returns>
        public MetadataDataSourceResult RestoreMetadataDataSourceEntries(string fieldExternalId, List<string> entriesExternalIds)
        {
            var url = PrepareUrlForDatasourceOperation(fieldExternalId, entriesExternalIds, $"{Constants.DATASOURCE_MANAGMENT}_restore");
            var result = m_api.CallApi<MetadataDataSourceResult>(HttpMethod.POST, url, null, null);
            return result;
        }

        /// <summary>
        /// Populate metadata fields with the given values. Existing values will be overwritten.
        ///
        /// Any metadata-value pairs given are merged with any existing metadata-value pairs
        /// (an empty value for an existing metadata field clears the value).
        /// </summary>
        /// <param name="parameters">Values to be applied to metadata fields of uploaded assets.</param>
        /// <returns>Parsed result of the operation.</returns>
        public MetadataUpdateResult UpdateMetadata(MetadataUpdateParams parameters)
        {
            var url = GetApiUrlV().
                Add(Api.GetCloudinaryParam(parameters.ResourceType)).
                Add(Constants.METADATA).
                BuildUrl();
            var result = m_api.CallApi<MetadataUpdateResult>(HttpMethod.POST, url, parameters, null);
            return result;
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
    }
}
