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
        /// <inheritdoc />
        public MetadataFieldResult AddMetadataField<T>(MetadataFieldCreateParams<T> parameters)
        {
            return cloudinaryAdmin.AddMetadataField<T>(parameters);
        }

        /// <inheritdoc />
        public MetadataFieldListResult ListMetadataFields()
        {
            return cloudinaryAdmin.ListMetadataFields();
        }

        /// <inheritdoc />
        public MetadataFieldResult GetMetadataField(string fieldExternalId)
        {
            return cloudinaryAdmin.GetMetadataField(fieldExternalId);
        }

        /// <inheritdoc />
        public MetadataFieldResult UpdateMetadataField<T>(string fieldExternalId, MetadataFieldUpdateParams<T> parameters)
        {
            return cloudinaryAdmin.UpdateMetadataField<T>(fieldExternalId, parameters);
        }

        /// <inheritdoc />
        public MetadataDataSourceResult UpdateMetadataDataSourceEntries(string fieldExternalId, MetadataDataSourceParams parameters)
        {
            return cloudinaryAdmin.UpdateMetadataDataSourceEntries(fieldExternalId, parameters);
        }

        /// <inheritdoc />
        public DelMetadataFieldResult DeleteMetadataField(string fieldExternalId)
        {
            return cloudinaryAdmin.DeleteMetadataField(fieldExternalId);
        }

        /// <inheritdoc />
        public MetadataDataSourceResult DeleteMetadataDataSourceEntries(string fieldExternalId, List<string> entriesExternalIds)
        {
            return cloudinaryAdmin.DeleteMetadataDataSourceEntries(fieldExternalId, entriesExternalIds);
        }

        /// <inheritdoc />
        public MetadataDataSourceResult RestoreMetadataDataSourceEntries(string fieldExternalId, List<string> entriesExternalIds)
        {
            return cloudinaryAdmin.RestoreMetadataDataSourceEntries(fieldExternalId, entriesExternalIds);
        }

        /// <inheritdoc />
        public MetadataUpdateResult UpdateMetadata(MetadataUpdateParams parameters)
        {
            return cloudinaryAdmin.UpdateMetadata(parameters);
        }
    }
}
