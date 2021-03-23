using System;
using System.Collections.Generic;
using System.Linq;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.IntegrationTest.AdminApi
{
    public class MetaDataTest : IntegrationTestBase
    {
        private string m_externalIdGeneral;
        private string m_externalIdDate;
        private string m_externalIdEnum2;
        private string m_externalIdSet;
        private string m_externalIdSet2;
        private string m_externalIdSet3;
        private string m_externalIdDelete2;
        private string m_externalIdDateValidation;
        private string m_externalIdDateValidation2;
        private string m_externalIdIntValidation;
        private string m_externalIdIntValidation2;

        private string m_datasourceEntryExternalId;
        private MetadataDataSourceParams m_datasourceSingle;
        private MetadataDataSourceParams m_datasourceMultiple;

        private List<string> m_metaDataFields;

        [OneTimeSetUp]
        public override void Initialize()
        {
            base.Initialize();

            // External IDs for metadata fields that should be created and later deleted
            m_externalIdGeneral = $"metadata_external_id_general_{m_uniqueTestId}";
            m_externalIdDate = $"metadata_external_id_date_{m_uniqueTestId}";
            m_externalIdEnum2 = $"metadata_external_id_enum_2_{m_uniqueTestId}";
            m_externalIdSet = $"metadata_external_id_set_{m_uniqueTestId}";
            m_externalIdSet2 = $"metadata_external_id_set_2_{m_uniqueTestId}";
            m_externalIdSet3 = $"metadata_external_id_set_3_{m_uniqueTestId}";
            m_externalIdDelete2 = $"metadata_deletion_2_{m_uniqueTestId}";
            m_externalIdDateValidation = $"metadata_date_validation_{m_uniqueTestId}";
            m_externalIdDateValidation2 = $"metadata_date_validation_2_{m_uniqueTestId}";
            m_externalIdIntValidation = $"metadata_int_validation_{m_uniqueTestId}";
            m_externalIdIntValidation2 = $"metadata_int_validation_2_{m_uniqueTestId}";
            m_metaDataFields = new List<string>
            {
                m_externalIdGeneral, m_externalIdDate, m_externalIdEnum2, m_externalIdSet, m_externalIdSet2,
                m_externalIdSet3, m_externalIdDelete2, m_externalIdDateValidation, m_externalIdDateValidation2,
                m_externalIdIntValidation, m_externalIdIntValidation2
            };

            // Sample datasource data
            m_datasourceEntryExternalId = $"metadata_datasource_entry_external_id{m_uniqueTestId}";
            var singleEntry = new List<EntryParams>
            {
                new EntryParams("v1", m_datasourceEntryExternalId)
            };
            m_datasourceSingle = new MetadataDataSourceParams(singleEntry);
            var multipleEntries = new List<EntryParams>
            {
                new EntryParams("v2", m_datasourceEntryExternalId),
                new EntryParams("v3"),
                new EntryParams("v4")
            };
            m_datasourceMultiple = new MetadataDataSourceParams(multipleEntries);
            
            CreateMetadataFieldForTest<StringMetadataFieldCreateParams, string>(m_externalIdGeneral);
            CreateMetadataFieldForTest<EnumMetadataFieldCreateParams, string>(m_externalIdEnum2, m_datasourceMultiple);
            CreateMetadataFieldForTest<SetMetadataFieldCreateParams, List<string>>(m_externalIdSet2, m_datasourceMultiple);
            CreateMetadataFieldForTest<SetMetadataFieldCreateParams, List<string>>(m_externalIdSet3, m_datasourceMultiple);
            CreateMetadataFieldForTest<IntMetadataFieldCreateParams, int?>(m_externalIdDelete2);
        }

        [OneTimeTearDown]
        public override void Cleanup()
        {
            base.Cleanup();
            m_metaDataFields.ForEach(p => m_cloudinary.DeleteMetadataField(p));
        }


        /// <summary>
        /// <para>Getting a metadata field by external id.</para>
        ///
        /// <para>Verifies that returned single metadata field definition has expected structure and property values.
        /// See <a href="https://cloudinary.com/documentation/admin_api#get_a_metadata_field_by_external_id">
        /// Get a metadata field by external id.</a></para>
        /// </summary>
        [Test, RetryWithDelay]
        public void TestGetMetadataField()
        {
            var result = m_cloudinary.GetMetadataField(m_externalIdGeneral);

            AssertMetadataField(result, MetadataFieldType.String, m_externalIdGeneral);
        }

        /// <summary>
        /// <para>Creating a date metadata field.</para>
        ///
        /// <para>Verifies that created single metadata field definition has expected structure and property values.
        /// See <a href="https://cloudinary.com/documentation/admin_api#create_a_metadata_field">
        /// Create a metadata field.</a></para>
        /// </summary>
        [Test, RetryWithDelay]
        public void TestCreateDateMetadataField()
        {
            var result = CreateMetadataFieldForTest<DateMetadataFieldCreateParams, DateTime?>(m_externalIdDate);
            
            AssertMetadataField(result, MetadataFieldType.Date, m_externalIdDate, m_externalIdDate);
        }

        /// <summary>
        /// <para>Creating a set metadata field.</para>
        ///
        /// <para>Verifies that created single metadata field definition has expected structure and property values.
        /// See <a href="https://cloudinary.com/documentation/admin_api#create_a_metadata_field">
        /// Create a metadata field.</a></para>
        /// </summary>
        [Test, RetryWithDelay]
        public void TestCreateSetMetadataField()
        {
            var result = CreateMetadataFieldForTest<SetMetadataFieldCreateParams, List<string>>(m_externalIdSet, m_datasourceMultiple);
            
            AssertMetadataField(result, MetadataFieldType.Set, m_externalIdSet, m_externalIdSet);
        }

        /// <summary>
        /// <para>Updating a metadata field by external id.</para>
        ///
        /// <para>Verifies that a metadata field definition was updated partially.
        /// See <a href="https://cloudinary.com/documentation/admin_api#update_a_metadata_field_by_external_id">
        /// Update a metadata field by external id.</a></para>
        /// </summary>
        [Test, RetryWithDelay]
        public void TestUpdateMetadataField()
        {
            var newLabel = $"update_metadata_test_new_label{m_externalIdGeneral}";
            var newDefaultValue = $"update_metadata_test_new_default_value{m_externalIdGeneral}";

            // Call the API to update the metadata field.
            // Will also attempt to update some fields that cannot be updated (external_id and type) which will be ignored.
            var updateParams = new StringMetadataFieldUpdateParams
            {
                ExternalId = m_externalIdSet,
                Label = newLabel,
                Mandatory = true,
                DefaultValue = newDefaultValue
            };

            var result = m_cloudinary.UpdateMetadataField(m_externalIdGeneral, updateParams);

            AssertMetadataField(result, MetadataFieldType.String, newLabel, m_externalIdGeneral, true, newDefaultValue);
        }

        /// <summary>
        /// <para>Updating a metadata field datasource.</para>
        ///
        /// <para>Verifies that a metadata field datasource was updated partially.
        /// See <a href="https://cloudinary.com/documentation/admin_api#update_a_metadata_field_datasource">
        /// Update a metadata field datasource.</a></para>
        /// </summary>
        [Test, RetryWithDelay]
        public void TestUpdateMetadataFieldDatasource()
        {
            var result = m_cloudinary.UpdateMetadataDataSourceEntries(m_externalIdEnum2, m_datasourceSingle);

            AssertMetadataFieldDataSource(result);
            var originalEntry = m_datasourceSingle.Values[0];
            Assert.True(result.Values.Any(entry => entry.ExternalId == originalEntry.ExternalId && entry.Value == originalEntry.Value),
                "The updated metadata field does not contain the updated datasource");
            Assert.AreEqual(m_datasourceMultiple.Values.Count, result.Values.Count);
        }

        /// <summary>
        /// <para>Removing a metadata field definition
        /// then attempting to create a new one with the same external id which should fail.</para>
        ///
        /// <para>Verifies that a metadata field cannot be created with the same external id after removal.
        /// See <a href="https://cloudinary.com/documentation/admin_api#delete_a_metadata_field_by_external_id">
        /// Delete a metadata field by external id.</a></para>
        /// </summary>
        [Test, RetryWithDelay]
        public void TestDeleteMetadataFieldDoesNotReleaseExternalId()
        {
            m_cloudinary.DeleteMetadataField(m_externalIdDelete2);

            var result = CreateMetadataFieldForTest<IntMetadataFieldCreateParams, int?>(m_externalIdDelete2);

            Assert.AreEqual($"external id {m_externalIdDelete2} already exists", result.Error.Message);
        }

        /// <summary>
        /// <para>Removing entries in a metadata field datasource</para>
        ///
        /// <para>Verifies that a datasource entry was deleted (blocked) from a specified metadata field definition.
        /// See <a href="https://cloudinary.com/documentation/admin_api#delete_entries_in_a_metadata_field_datasource">
        /// Delete entries in a metadata field datasource.</a></para>
        /// </summary>
        [Test, RetryWithDelay]
        public void TestDeleteMetadataFieldDataSource()
        {
            var entriesExternalIds = new List<string> {m_datasourceEntryExternalId};

            var result = m_cloudinary.DeleteMetadataDataSourceEntries(m_externalIdSet2, entriesExternalIds);

            AssertMetadataFieldDataSource(result);
            Assert.AreEqual(m_datasourceMultiple.Values.Count - 1, result.Values.Count);
            var values = result.Values.Select(entry => entry.Value).ToList();
            Assert.Contains(m_datasourceMultiple.Values[1].Value, values);
            Assert.Contains(m_datasourceMultiple.Values[2].Value, values);
        }

        /// <summary>
        /// <para>Date field validation.</para>
        ///
        /// <para>Verifies that a metadata field with date validation can be created with valid
        /// default value and cannot be created with invalid one.
        /// See <a href="https://cloudinary.com/documentation/admin_api#validating_data">
        /// Validating data.</a></para>
        /// </summary>
        [Test, RetryWithDelay]
        public void TestDateFieldDefaultValueValidation()
        {
            var todayDate = DateTime.Today.Date;
            var pastDate = todayDate.AddDays(-3);
            var yesterdayDate = todayDate.AddDays(-1);
            var futureDate = todayDate.AddDays(3);
            var validationRules = new List<MetadataValidationParams>
            {
                new DateGreaterThanValidationParams(pastDate),
                new DateLessThanValidationParams(todayDate)
            };
            var lastThreeDaysValidation = new AndValidationParams(validationRules);

            // Test entering a metadata field with date validation and a valid default value.
            var validMetadataField = new DateMetadataFieldCreateParams(m_externalIdDateValidation)
            {
                ExternalId = m_externalIdDateValidation,
                DefaultValue = yesterdayDate,
                Validation = lastThreeDaysValidation
            };

            var validMetadataFieldResult = m_cloudinary.AddMetadataField(validMetadataField);

            Assert.AreEqual(validMetadataField.DefaultValue.Value.ToString("yyyy-MM-dd"), 
                validMetadataFieldResult.DefaultValue);
            Assert.NotNull(validMetadataFieldResult.Validation);
            Assert.AreEqual(validationRules.Count, validMetadataFieldResult.Validation.Rules.Count);

            // Test entering a metadata field with date validation and an invalid default value.
            var invalidMetadataField = new DateMetadataFieldCreateParams(m_externalIdDateValidation2)
            {
                ExternalId = m_externalIdDateValidation2,
                DefaultValue = futureDate,
                Validation = lastThreeDaysValidation
            };

            var invalidMetadataFieldResult = m_cloudinary.AddMetadataField(invalidMetadataField);

            Assert.NotNull(invalidMetadataFieldResult.Error);
        }

        /// <summary>
        /// <para>Integer field validation.</para>
        ///
        /// <para>Verifies that a metadata field with integer validation can be created with valid
        /// default value and cannot be created with invalid one.
        /// See <a href="https://cloudinary.com/documentation/admin_api#validating_data">
        /// Validating data.</a></para>
        /// </summary>
        [Test, RetryWithDelay]
        public void TestIntegerFieldValidation()
        {
            var validation = new IntLessThanValidationParams(5)
            {
                IsEqual = true
            };

            var validMetadataField = new IntMetadataFieldCreateParams(m_externalIdIntValidation)
            {
                ExternalId = m_externalIdIntValidation,
                DefaultValue = 5,
                Validation = validation
            };

            var validMetadataFieldResult = m_cloudinary.AddMetadataField(validMetadataField);

            Assert.AreEqual(validMetadataField.DefaultValue.Value,validMetadataFieldResult.DefaultValue);
            Assert.NotNull(validMetadataFieldResult.Validation);
            Assert.AreEqual(validation.Value, validMetadataFieldResult.Validation.Value);

            var invalidMetadataField = new IntMetadataFieldCreateParams(m_externalIdIntValidation2)
            {
                ExternalId = m_externalIdIntValidation2,
                DefaultValue = 6,
                Validation = validation
            };

            var invalidMetadataFieldResult = m_cloudinary.AddMetadataField(invalidMetadataField);

            Assert.NotNull(invalidMetadataFieldResult.Error);
        }

        /// <summary>
        /// <para>Restoring a deleted entry in a metadata field datasource.</para>
        ///
        /// <para>Verifies that a previously deleted datasource entries can be restored
        /// for a specified metadata field definition.
        /// See <a href="https://cloudinary.com/documentation/admin_api#restore_entries_in_a_metadata_field_datasource">
        /// Restore entries in a metadata field datasource</a></para>
        /// </summary>
        [Test, RetryWithDelay]
        public void TestRestoreMetadataFieldDataSource()
        {
            var entriesExternalIds = new List<string> {m_datasourceEntryExternalId};

            var removalResult = m_cloudinary.DeleteMetadataDataSourceEntries(m_externalIdSet3, entriesExternalIds);
            AssertMetadataFieldDataSource(removalResult);
            Assert.AreEqual(2, removalResult.Values.Count);

            var restoreResult = m_cloudinary.RestoreMetadataDataSourceEntries(m_externalIdSet3, entriesExternalIds);
            AssertMetadataFieldDataSource(restoreResult);
            Assert.AreEqual(3, restoreResult.Values.Count);
        }

        /// <summary>
        /// <para>Asserts that a given object fits the generic structure of a metadata field.</para>
        ///
        /// <para>Asserts that a metadata field has expected structure and property values.
        /// See <a href="https://cloudinary.com/documentation/admin_api#generic_structure_of_a_metadata_field">
        /// Generic structure of a metadata field in API reference.</a></para>
        /// </summary>
        /// <param name="metadataField">The object to test.</param>
        /// <param name="type">The type of metadata field we expect.</param>
        /// <param name="label">The label of the metadata field for display purposes.</param>
        /// <param name="externalId">(Optional) A unique identification string for the metadata field.</param>
        /// <param name="mandatory">(Optional) Whether a value must be given for this field.</param>
        /// <param name="defaultValue">(Optional) The default value for the field.</param>
        private void AssertMetadataField(MetadataFieldResult metadataField, MetadataFieldType type, string label,
            string externalId = null, bool mandatory = false, object defaultValue = null)
        {
            Assert.IsNull(metadataField.Error, metadataField.Error?.Message);
            Assert.AreEqual(ApiShared.GetCloudinaryParam(type), metadataField.Type);

            if (metadataField.Type == ApiShared.GetCloudinaryParam(MetadataFieldType.Enum) ||
                metadataField.Type == ApiShared.GetCloudinaryParam(MetadataFieldType.Set))
            {
                AssertMetadataFieldDataSource(metadataField.DataSource);
            }

            Assert.AreEqual(label, metadataField.Label);
            Assert.AreEqual(mandatory, metadataField.Mandatory);
            if (!string.IsNullOrEmpty(externalId))
                Assert.AreEqual(externalId, metadataField.ExternalId);
            if (defaultValue != null)
                Assert.AreEqual(defaultValue, metadataField.DefaultValue);
        }

        /// <summary>
        /// <para>Asserts that a given object fits the generic structure of a metadata field datasource</para>
        ///
        /// <para>Asserts that a metadata field datasource has expected structure and property values.
        /// See <a href="https://cloudinary.com/documentation/admin_api#datasource_values">
        /// Datasource values in Admin API.</a></para>
        /// </summary>
        /// <param name="dataSource">The object to test.</param>
        private void AssertMetadataFieldDataSource(MetadataDataSourceResult dataSource)
        {
            Assert.IsNotNull(dataSource);
            dataSource.Values.Where(entry => !string.IsNullOrEmpty(entry.State)).ToList()
                .ForEach(entry => Assert.Contains(entry.State, new List<string> {"active", "inactive"}));
        }

        /// <summary>
        /// <para>Private helper method to create metadata fields for this test.</para>
        ///
        /// <para>Creates metadata field of specified type.
        /// See <a href="https://cloudinary.com/documentation/admin_api#create_a_metadata_field">
        /// Create a metadata field.</a></para>
        /// </summary>
        /// <param name="externalId">The ID of the metadata field.</param>
        /// <param name="dataSource">Optional. Data source for a field to be created.</param>
        /// <typeparam name="T">Type of the metadata field.</typeparam>
        /// <typeparam name="TP">Type that can describe the field type.</typeparam>
        /// <returns>Parsed result of the metadata field creating.</returns>
        private MetadataFieldResult CreateMetadataFieldForTest<T, TP>(string externalId, MetadataDataSourceParams dataSource = null)
            where T : MetadataFieldCreateParams<TP>
        {
            var parameters = (T)Activator.CreateInstance(typeof(T), externalId);
            parameters.ExternalId = externalId;
            if (dataSource != null)
                parameters.DataSource = dataSource;

            return m_cloudinary.AddMetadataField(parameters);
        }
    }
}