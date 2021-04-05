namespace CloudinaryDotNet
{
    using System;
    using System.Collections.Generic;
    using CloudinaryDotNet.Actions;

    /// <summary>
    /// Part of Cloudinary .NET API main class, responsible for metadata fields management.
    /// </summary>
    internal partial class AdminApi
    {
        /// <inheritdoc/>
        /// <typeparam name="T">Type of the metadata field.</typeparam>
        public MetadataFieldResult AddMetadataField<T>(MetadataFieldCreateParams<T> parameters)
        {
            var url = m_api.ApiUrlMetadataFieldV.BuildUrl();
            var result = m_api.CallApi<MetadataFieldResult>(HttpMethod.POST, url, parameters, null, PrepareHeaders());
            return result;
        }

        /// <inheritdoc/>
        public MetadataFieldListResult ListMetadataFields()
        {
            var result = m_api.CallApi<MetadataFieldListResult>(
                HttpMethod.GET, m_api.ApiUrlMetadataFieldV.BuildUrl(), null, null);
            return result;
        }

        /// <inheritdoc/>
        public MetadataFieldResult GetMetadataField(string fieldExternalId)
        {
            var url = m_api.ApiUrlMetadataFieldV.Add(fieldExternalId).BuildUrl();
            var result = m_api.CallApi<MetadataFieldResult>(HttpMethod.GET, url, null, null);
            return result;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public MetadataDataSourceResult DeleteMetadataDataSourceEntries(string fieldExternalId, List<string> entriesExternalIds)
        {
            var url = PrepareUrlForDatasourceOperation(fieldExternalId, entriesExternalIds, Constants.DATASOURCE_MANAGMENT);
            var result = m_api.CallApi<MetadataDataSourceResult>(HttpMethod.DELETE, url, null, null);
            return result;
        }

        /// <inheritdoc/>
        public MetadataDataSourceResult RestoreMetadataDataSourceEntries(string fieldExternalId, List<string> entriesExternalIds)
        {
            var url = PrepareUrlForDatasourceOperation(fieldExternalId, entriesExternalIds, $"{Constants.DATASOURCE_MANAGMENT}_restore");
            var result = m_api.CallApi<MetadataDataSourceResult>(HttpMethod.POST, url, null, null);
            return result;
        }

        /// <inheritdoc/>
        public MetadataUpdateResult UpdateMetadata(MetadataUpdateParams parameters)
        {
            var url = GetApiUrlV().
                Add(Api.GetCloudinaryParam(parameters.ResourceType)).
                Add(Constants.METADATA).
                BuildUrl();
            var result = m_api.CallApi<MetadataUpdateResult>(HttpMethod.POST, url, parameters, null);
            return result;
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
