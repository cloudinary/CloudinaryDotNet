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
        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.AddMetadataField method instead.")]
        public MetadataFieldResult AddMetadataField<T>(MetadataFieldCreateParams<T> parameters)
        {
            var result = AdminApi.AddMetadataField(parameters);
            return result;
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.ListMetadataFields method instead.")]
        public MetadataFieldListResult ListMetadataFields()
        {
            var result = AdminApi.ListMetadataFields();
            return result;
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.GetMetadataField method instead.")]
        public MetadataFieldResult GetMetadataField(string fieldExternalId)
        {
            var result = AdminApi.GetMetadataField(fieldExternalId);
            return result;
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateMetadataField method instead.")]
        public MetadataFieldResult UpdateMetadataField<T>(string fieldExternalId, MetadataFieldUpdateParams<T> parameters)
        {
            var result = AdminApi.UpdateMetadataField(fieldExternalId, parameters);
            return result;
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateMetadataDataSourceEntries method instead.")]
        public MetadataDataSourceResult UpdateMetadataDataSourceEntries(string fieldExternalId, MetadataDataSourceParams parameters)
        {
            var result = AdminApi.UpdateMetadataDataSourceEntries(fieldExternalId, parameters);
            return result;
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteMetadataField method instead.")]
        public DelMetadataFieldResult DeleteMetadataField(string fieldExternalId)
        {
            var result = AdminApi.DeleteMetadataField(fieldExternalId);
            return result;
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.DeleteMetadataDataSourceEntries method instead.")]
        public MetadataDataSourceResult DeleteMetadataDataSourceEntries(string fieldExternalId, List<string> entriesExternalIds)
        {
            var result = AdminApi.DeleteMetadataDataSourceEntries(fieldExternalId, entriesExternalIds);
            return result;
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.RestoreMetadataDataSourceEntries method instead.")]
        public MetadataDataSourceResult RestoreMetadataDataSourceEntries(string fieldExternalId, List<string> entriesExternalIds)
        {
            var result = AdminApi.RestoreMetadataDataSourceEntries(fieldExternalId, entriesExternalIds);
            return result;
        }

        /// <inheritdoc/>
        [Obsolete("This method is deprecated and will be removed in future versions of the SDK. Use Cloudinary.AdminApi.UpdateMetadata method instead.")]
        public MetadataUpdateResult UpdateMetadata(MetadataUpdateParams parameters)
        {
            var result = AdminApi.UpdateMetadata(parameters);
            return result;
        }
    }
}
