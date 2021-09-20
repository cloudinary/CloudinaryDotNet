using System.Collections.Generic;
using SystemHttp = System.Net.Http;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests
{
    public class MetaDataTest
    {
        private const string externalIdString = "metadata_external_id_string";
        private const string externalIdInt = "metadata_external_id_int";
        private const string externalIdEnum = "metadata_external_id_enum";
        private const string externalIdDelete = "metadata_deletion";
        private const string datasourceEntryExternalId = "metadata_datasource_entry_external_id";
        private MockedCloudinaryAdmin mockedCloudinary;

        [SetUp]
        public void SetUp()
        {
            mockedCloudinary = new MockedCloudinaryAdmin();
        }

        /// <summary>
        /// <para>Getting a list of all metadata fields.</para>
        ///
        /// <para>Verifies the API endpoint was called with correct Uri and parameters.
        /// See <a href="https://cloudinary.com/documentation/admin_api#get_metadata_fields">
        /// Get metadata fields.</a></para>
        /// </summary>
        [Test]
        public void TestListMetadataFields()
        {
            mockedCloudinary.ListMetadataFields();

            mockedCloudinary.AssertHttpCall(SystemHttp.HttpMethod.Get, "metadata_fields");
        }


        /// <summary>
        /// <para>Creating a string metadata field.</para>
        ///
        /// <para>Verifies that the API endpoint was called with correct Uri and parameters.
        /// See <a href="https://cloudinary.com/documentation/admin_api#create_a_metadata_field">
        /// Create a metadata field.</a></para>
        /// </summary>
        [Test]
        public void TestCreateStringMetadataField()
        {
            var parameters = new StringMetadataFieldCreateParams(externalIdString)
            {
                ExternalId = externalIdString
            };

            mockedCloudinary.AddMetadataField(parameters);

            mockedCloudinary.AssertHttpCall(SystemHttp.HttpMethod.Post, "metadata_fields");
            AssertEncodedRequestFields(mockedCloudinary, "string", externalIdString);
        }

        /// <summary>
        /// <para>Creating an integer metadata field.</para>
        ///
        /// <para>Verifies that the API endpoint was called with correct Uri and parameters.
        /// See <a href="https://cloudinary.com/documentation/admin_api#create_a_metadata_field">
        /// Create a metadata field.</a></para>
        /// </summary>
        [Test]
        public void TestCreateIntMetadataField()
        {
            var parameters = new IntMetadataFieldCreateParams(externalIdInt)
            {
                ExternalId = externalIdInt
            };

            mockedCloudinary.AddMetadataField(parameters);

            mockedCloudinary.AssertHttpCall(SystemHttp.HttpMethod.Post, "metadata_fields");
            AssertEncodedRequestFields(mockedCloudinary, "integer", externalIdInt);
        }

        /// <summary>
        /// <para>Creating an enum metadata field.</para>
        ///
        /// <para>Verifies that the API endpoint was called with correct Uri and parameters.
        /// See <a href="https://cloudinary.com/documentation/admin_api#create_a_metadata_field">
        /// Create a metadata field.</a></para>
        /// </summary>
        [Test]
        public void TestCreateEnumMetadataField()
        {
            var singleEntry = new List<EntryParams>
            {
                new EntryParams("v1", datasourceEntryExternalId)
            };
            var datasourceSingle = new MetadataDataSourceParams(singleEntry);
            var parameters = new EnumMetadataFieldCreateParams(externalIdEnum)
            {
                ExternalId = externalIdEnum,
                DataSource = datasourceSingle
            };

            mockedCloudinary.AddMetadataField(parameters);

            mockedCloudinary.AssertHttpCall(SystemHttp.HttpMethod.Post, "metadata_fields");
            AssertEncodedRequestFields(mockedCloudinary, "enum", externalIdEnum, singleEntry[0]);
        }

        /// <summary>
        /// <para>Deleting a metadata field definition by its external id.</para>
        ///
        /// <para>Verifies that the API endpoint was called with correct Uri and parameters.
        /// See <a href="https://cloudinary.com/documentation/admin_api#delete_a_metadata_field_by_external_id">
        /// Delete a metadata field by external id</a></para>
        /// </summary>
        [Test]
        public void TestDeleteMetadataField()
        {
            mockedCloudinary.DeleteMetadataField(externalIdDelete);

            mockedCloudinary.AssertHttpCall(SystemHttp.HttpMethod.Delete, $"metadata_fields/{externalIdDelete}");
        }

        /// <summary>
        /// <para>Asserts that a given HTTP call was sent with expected JSON content.</para>
        /// </summary>
        ///
        /// <para>Asserts that a metadata field has expected structure and property values.
        /// See <a href="https://cloudinary.com/documentation/admin_api#generic_structure_of_a_metadata_field">
        /// Generic structure of a metadata field in API reference.</a></para>
        /// <param name="mockedCloudinary">An instance of the <see cref="Cloudinary"/> class with mocked HttpClient.</param>
        /// <param name="type"> The type of value that can be assigned to the metadata field.</param>
        /// <param name="externalId">A unique identification string for the metadata field.</param>
        /// <param name="dataSourceEntry">(Optional) Data source for a given field.</param>
        private static void AssertEncodedRequestFields(MockedCloudinaryAdmin mockedCloudinary, string type, string externalId,
            EntryParams dataSourceEntry = null)
        {
            var requestJson = JToken.Parse(mockedCloudinary.HttpRequestContent);
            Assert.AreEqual(type, requestJson["type"].Value<string>());
            Assert.AreEqual(externalId, requestJson["label"].Value<string>());
            Assert.AreEqual(externalId, requestJson["external_id"].Value<string>());

            if (dataSourceEntry == null) return;

            var firstDataSource = requestJson["datasource"]["values"].First;
            Assert.AreEqual(dataSourceEntry.Value, firstDataSource["value"].Value<string>());
            Assert.AreEqual(dataSourceEntry.ExternalId, firstDataSource["external_id"].Value<string>());
        }
    }
}
