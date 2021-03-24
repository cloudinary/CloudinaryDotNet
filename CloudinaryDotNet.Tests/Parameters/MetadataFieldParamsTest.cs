using System;
using System.Collections.Generic;
using System.Linq;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests.Parameters
{
    public class MetadataFieldParamsTest
    {
        private string emptyValuesTestMessage;
        private string emptyExternalIdsTestMessage;
        private const string label = "field label";
        private const string defaultString = "default value";
        private const string externalId = "external id";
        private const string dataSourceId = "data source id";
        private const string dataSourceValue1 = "blue";
        private const string dataSourceValue2 = "yellow";
        private const string emptyLabelTestMessage = "Must be specified Label";
        private const string emptyDefaultValueTestMessage = "Must be specified DefaultValue";
        private const string nonEmptyDataSourceTestMessage = "DataSource field must not be specified";
        private const string forbiddenValidationTestMessage = "The validation type is forbidden";
        private const string emptyDataSourceTestMessage = "Must be specified DataSource";
        private const string nonEmptyValidationTestMessage = "Must not be specified Validation";

        [Test]
        public void TestIntMetadataFieldCreateParamsCheck()
        {
            var parameters = new IntMetadataFieldCreateParams(null);
            int? defaultValue = 10;

            CheckMainFields(parameters, defaultValue);
            var validationParams = new MetadataValidationParams[]
            {
                new StringLengthValidationParams(),
                new DateGreaterThanValidationParams(DateTime.MinValue),
                new DateLessThanValidationParams(DateTime.MaxValue)
            };
            AssertForbiddenValidations<IntMetadataFieldCreateParams, int?>(parameters, validationParams);
        }

        [Test]
        public void TestStringMetadataFieldCreateParamsCheck()
        {
            var parameters = new StringMetadataFieldCreateParams(null);

            CheckMainFields(parameters, defaultString);
            var validationParams = new MetadataValidationParams[]
            {
                new IntLessThanValidationParams(10),
                new IntGreaterThanValidationParams(10),
                new DateGreaterThanValidationParams(DateTime.MinValue),
                new DateLessThanValidationParams(DateTime.MaxValue)
            };
            AssertForbiddenValidations<StringMetadataFieldCreateParams, string>(parameters, validationParams);
        }

        [Test]
        public void TestDateMetadataFieldCreateParamsCheck()
        {
            var parameters = new DateMetadataFieldCreateParams(null);
            DateTime? defaultValue = DateTime.MinValue;

            CheckMainFields(parameters, defaultValue);
            var validationParams = new MetadataValidationParams[]
            {
                new IntLessThanValidationParams(10),
                new IntGreaterThanValidationParams(10),
                new StringLengthValidationParams()
            };
            AssertForbiddenValidations<DateMetadataFieldCreateParams, DateTime?>(parameters, validationParams);
        }

        [Test]
        public void TestEnumMetadataFieldCreateParamsCheck()
        {
            var parameters = new EnumMetadataFieldCreateParams(null);
            AssertCheck(parameters, emptyLabelTestMessage);

            parameters.Label = label;
            parameters.Mandatory = true;
            AssertCheck(parameters, emptyDefaultValueTestMessage);

            parameters.DefaultValue = defaultString;
            AssertCheck(parameters, emptyDataSourceTestMessage);

            var entryParams = new List<EntryParams>
            {
                new EntryParams(defaultString)
            };
            parameters.DataSource = new MetadataDataSourceParams(entryParams);
            parameters.Validation = new StringLengthValidationParams();
            AssertCheck(parameters, nonEmptyValidationTestMessage);
        }

        [Test]
        public void TestSetMetadataFieldCreateParamsCheck()
        {
            var parameters = new SetMetadataFieldCreateParams(null);
            AssertCheck(parameters, emptyLabelTestMessage);

            parameters.Label = label;
            parameters.Mandatory = true;
            AssertCheck(parameters, emptyDefaultValueTestMessage);

            parameters.DefaultValue = new List<string>();
            AssertCheck(parameters, emptyDefaultValueTestMessage);

            parameters.DefaultValue.Add(defaultString);
            var entryParams = new List<EntryParams>
            {
                new EntryParams(defaultString)
            };
            parameters.DataSource = new MetadataDataSourceParams(entryParams);
            parameters.Validation = new StringLengthValidationParams();
            AssertCheck(parameters, nonEmptyValidationTestMessage);
        }

        [Test]
        public void TestIntMetadataFieldUpdateParamsCheck()
        {
            var parameters = new IntMetadataFieldUpdateParams
            {
                DataSource = new MetadataDataSourceParams(new List<EntryParams>())
            };

            AssertCheck(parameters, nonEmptyDataSourceTestMessage);

            parameters.DataSource = null;
            var validationParams = new MetadataValidationParams[]
            {
                new StringLengthValidationParams(),
                new DateGreaterThanValidationParams(DateTime.MinValue),
                new DateLessThanValidationParams(DateTime.MaxValue)
            };
            AssertForbiddenValidations<IntMetadataFieldUpdateParams, int?>(parameters, validationParams);
        }

        [Test]
        public void TestStringMetadataFieldUpdateParamsCheck()
        {
            var parameters = new StringMetadataFieldUpdateParams
            {
                DataSource = new MetadataDataSourceParams(new List<EntryParams>())
            };
            AssertCheck(parameters, nonEmptyDataSourceTestMessage);

            parameters.DataSource = null;
            var validationParams = new MetadataValidationParams[]
            {
                new IntLessThanValidationParams(10),
                new IntGreaterThanValidationParams(10),
                new DateGreaterThanValidationParams(DateTime.MinValue),
                new DateLessThanValidationParams(DateTime.MaxValue)
            };
            AssertForbiddenValidations<StringMetadataFieldUpdateParams, string>(parameters, validationParams);
        }

        [Test]
        public void TestDateMetadataFieldUpdateParamsCheck()
        {
            var parameters = new DateMetadataFieldUpdateParams
            {
                DataSource = new MetadataDataSourceParams(new List<EntryParams>())
            };
            AssertCheck(parameters, nonEmptyDataSourceTestMessage);

            parameters.DataSource = null;
            var validationParams = new MetadataValidationParams[]
            {
                new IntLessThanValidationParams(10),
                new IntGreaterThanValidationParams(10),
                new StringLengthValidationParams()
            };
            AssertForbiddenValidations<DateMetadataFieldUpdateParams, DateTime?>(parameters, validationParams); ;
        }

        [Test]
        public void TestEnumMetadataFieldUpdateParamsCheck()
        {
            var parameters = new EnumMetadataFieldUpdateParams
            {
                Validation = new StringLengthValidationParams()
            };
            AssertCheck(parameters, nonEmptyValidationTestMessage);
        }

        [Test]
        public void TestSetMetadataFieldUpdateParamsCheck()
        {
            var parameters = new SetMetadataFieldUpdateParams
            {
                Validation = new StringLengthValidationParams()
            };
            AssertCheck(parameters, nonEmptyValidationTestMessage);
        }

        [Test]
        public void TestMetadataDataSourceParamsCheck()
        {
            var parameters = new MetadataDataSourceParams(null);
            emptyValuesTestMessage = "Must be specified Values";
            AssertCheck(parameters, emptyValuesTestMessage);

            parameters.Values = new List<EntryParams>();
            AssertCheck(parameters, emptyValuesTestMessage);

            parameters.Values.Add(new EntryParams(null));
            AssertCheck(parameters, "Must be specified Value");
        }

        [Test]
        public void TestStringLengthValidationParamsCheck()
        {
            var parameters = new StringLengthValidationParams();
            AssertCheck(parameters, "Either min or max must be specified");

            parameters.Min = -5;
            AssertCheck(parameters, "Must be a positive integer Min");

            parameters.Min = 10;
            parameters.Max = -10;
            AssertCheck(parameters, "Must be a positive integer Max");
        }

        [Test]
        public void TestAndValidationParamsCheck()
        {
            var parameters = new AndValidationParams(null);
            AssertCheck(parameters, "Rules list must not be null");

            parameters.Rules = new List<MetadataValidationParams>();
            AssertCheck(parameters, "Rules list must not be empty");
        }

        [Test]
        public void TestDataSourceEntriesParamsCheck()
        {
            var parameters = new DataSourceEntriesParams(new List<string> {"some value"})
            {
                ExternalIds = null
            };
            emptyExternalIdsTestMessage = "Must be specified ExternalIds";
            AssertCheck(parameters, emptyExternalIdsTestMessage);

            parameters.ExternalIds = new List<string>();
            AssertCheck(parameters, emptyExternalIdsTestMessage);
        }

        [Test]
        public void TestIntMetadataFieldCreateParamsDictionary()
        {
            int? defaultValue = 100;
            var parameters = new IntMetadataFieldCreateParams(label)
            {
                Mandatory = true,
                DefaultValue = defaultValue,
                ExternalId = externalId
            };

            CheckParamsDictionary(parameters, "integer", defaultValue);
        }

        [Test]
        public void TestStringMetadataFieldCreateParamsDictionary()
        {
            var parameters = new StringMetadataFieldCreateParams(label)
            {
                Mandatory = true,
                DefaultValue = defaultString,
                ExternalId = externalId
            };

            CheckParamsDictionary(parameters, "string", defaultString);
        }

        [Test]
        public void TestDateMetadataFieldCreateParamsDictionary()
        {
            var parameters = new DateMetadataFieldCreateParams(label)
            {
                Mandatory = true,
                DefaultValue = new DateTime(2018, 11, 5),
                ExternalId = externalId
            };

            var dictionary = CheckParamsDictionary<DateMetadataFieldCreateParams, DateTime?>(parameters,"date", null);
            Assert.AreEqual("2018-11-05", dictionary["default_value"]);
        }

        [Test]
        public void TestEnumMetadataFieldCreateParamsDictionary()
        {
            var entries = new List<EntryParams>
            {
                new EntryParams(dataSourceValue1, dataSourceId),
                new EntryParams(dataSourceValue2)
            };
            var parameters = new EnumMetadataFieldCreateParams(label)
            {
                Mandatory = true,
                DefaultValue = defaultString,
                ExternalId = externalId,
                DataSource = new MetadataDataSourceParams(entries)
            };

            var dictionary = CheckParamsDictionary(parameters, "enum", defaultString);
            Assert.NotNull(dictionary["datasource"]);
        }

        [Test]
        public void TestSetMetadataFieldCreateParamsDictionary()
        {
            var defaultValue = new List<string>{"value1", "value2"};
            var entries = new List<EntryParams>
            {
                new EntryParams(dataSourceValue1, dataSourceId),
                new EntryParams(dataSourceValue2)
            };
            var parameters = new SetMetadataFieldCreateParams(label)
            {
                Mandatory = true,
                DefaultValue = defaultValue,
                ExternalId = externalId,
                DataSource = new MetadataDataSourceParams(entries)
            };

            var dictionary = CheckParamsDictionary(parameters, "set", defaultValue);
            Assert.NotNull(dictionary["datasource"]);
        }

        [Test]
        public void TestIntMetadataFieldUpdateParamsDictionary()
        {
            int? defaultValue = 100;
            var parameters = new IntMetadataFieldUpdateParams
            {
                Mandatory = true,
                DefaultValue = defaultValue,
                ExternalId = externalId,
                Label = label
            };

            CheckParamsDictionary(parameters, "integer", defaultValue);
        }

        [Test]
        public void TestStringMetadataFieldUpdateParamsDictionary()
        {
            var parameters = new StringMetadataFieldUpdateParams
            {
                Mandatory = true,
                DefaultValue = defaultString,
                ExternalId = externalId,
                Label = label
            };

            CheckParamsDictionary(parameters, "string", defaultString);
        }

        [Test]
        public void TestDateMetadataFieldUpdateParamsDictionary()
        {
            var defaultValue = new DateTime(2018, 11, 5);
            var parameters = new DateMetadataFieldUpdateParams
            {
                Mandatory = true,
                DefaultValue = defaultValue,
                ExternalId = externalId,
                Label = label
            };

            var dictionary = CheckParamsDictionary<DateMetadataFieldUpdateParams, DateTime?>(parameters, "date", null);
            Assert.AreEqual("2018-11-05", dictionary["default_value"]);
        }

        [Test]
        public void TestEnumMetadataFieldUpdateParamsDictionary()
        {
            const string defaultValue = "some value";
            var entries = new List<EntryParams>
            {
                new EntryParams(dataSourceValue1, dataSourceId),
                new EntryParams(dataSourceValue2)
            };
            var parameters = new EnumMetadataFieldUpdateParams
            {
                Mandatory = true,
                DefaultValue = defaultValue,
                ExternalId = externalId,
                DataSource = new MetadataDataSourceParams(entries),
                Label = label
            };

            var dictionary = CheckParamsDictionary(parameters, "enum", defaultValue);
            Assert.NotNull(dictionary["datasource"]);
        }

        [Test]
        public void TestSetMetadataFieldUpdateParamsDictionary()
        {
            var defaultValue = new List<string> { "value1", "value2" };
            var entries = new List<EntryParams>
            {
                new EntryParams(dataSourceValue1, dataSourceId),
                new EntryParams(dataSourceValue2)
            };
            var parameters = new SetMetadataFieldUpdateParams
            {
                Mandatory = true,
                DefaultValue = defaultValue,
                ExternalId = externalId,
                DataSource = new MetadataDataSourceParams(entries),
                Label = label
            };

            var dictionary = CheckParamsDictionary(parameters, "set", defaultValue);
            Assert.NotNull(dictionary["datasource"]);
        }

        [Test]
        public void TestMetadataDataSourceParamsDictionary()
        {
            var entries = new List<EntryParams>
            {
                new EntryParams(dataSourceValue1, dataSourceId),
                new EntryParams(dataSourceValue2)
            };
            var parameters = new MetadataDataSourceParams(entries);

            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            var values = (List<SortedDictionary<string, object>>)dictionary["values"];
            Assert.AreEqual(entries.Count, values.Count);
            Assert.AreEqual(dataSourceValue1, values[0]["value"]);
            Assert.AreEqual(dataSourceId, values[0]["external_id"]);
            Assert.AreEqual(dataSourceValue2, values[1]["value"]);
            Assert.False(values[1].ContainsKey("external_id"));
        }

        [Test]
        public void TestStringLengthValidationParamsDictionary()
        {
            const int minValue = 5;
            const int maxValue = 10;
            var parameters = new StringLengthValidationParams
            {
                Min = minValue,
                Max = maxValue
            };

            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual(minValue, dictionary["min"]);
            Assert.AreEqual(maxValue, dictionary["max"]);
            Assert.AreEqual("strlen", dictionary["type"]);
        }

        [Test]
        public void TestIntGreaterThanValidationParamsDictionary()
        {
            const int value = 10;
            var parameters = new IntGreaterThanValidationParams(value)
            {
                IsEqual = true
            };

            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual("greater_than", dictionary["type"]);
            Assert.AreEqual("true", dictionary["equals"]);
            Assert.AreEqual(value, dictionary["value"]);
        }

        [Test]
        public void TestDateGreaterThanValidationParamsDictionary()
        {
            var dateValue = new DateTime(2018, 11, 5);
            var parameters = new DateGreaterThanValidationParams(dateValue)
            {
                IsEqual = true
            };

            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual("greater_than", dictionary["type"]);
            Assert.AreEqual("true", dictionary["equals"]);
            Assert.AreEqual("2018-11-05", dictionary["value"]);
        }

        [Test]
        public void TestIntLessThanValidationParamsDictionary()
        {
            const int value = 10;
            var parameters = new IntLessThanValidationParams(value)
            {
                IsEqual = true
            };

            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual("less_than", dictionary["type"]);
            Assert.AreEqual("true", dictionary["equals"]);
            Assert.AreEqual(value, dictionary["value"]);
        }

        [Test]
        public void TestDateLessThanValidationParamsDictionary()
        {
            var dateValue = new DateTime(2018, 11, 5);
            var parameters = new DateLessThanValidationParams(dateValue)
            {
                IsEqual = true
            };

            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual("less_than", dictionary["type"]);
            Assert.AreEqual("true", dictionary["equals"]);
            Assert.AreEqual("2018-11-05", dictionary["value"]);
        }

        [Test]
        public void TestAndValidationParamsDictionary()
        {
            var strLenParams = new StringLengthValidationParams
            {
                Max = 10
            };
            var intLessThanParams = new IntLessThanValidationParams(30);
            var parameters = new AndValidationParams(new List<MetadataValidationParams>
            {
                strLenParams, intLessThanParams
            });

            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual("and", dictionary["type"]);
            var rules = (List<SortedDictionary<string, object>>)dictionary["rules"];
            Assert.AreEqual(parameters.Rules.Count, rules.Count);
            Assert.AreEqual("strlen", rules[0]["type"]);
            Assert.AreEqual("less_than", rules[1]["type"]);
        }

        [Test]
        public void TestDataSourceEntriesParamsDictionary()
        {
            const string externalId1 = "id 1";
            const string externalId2 = "id 2";
            var externalIds = new List<string> {externalId1, externalId2};
            var parameters = new DataSourceEntriesParams(externalIds);

            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual(externalIds, dictionary["external_ids"]);
        }

        private static void CheckMainFields<T, TP> (T parameters, TP defaultValue)
            where T: MetadataFieldCreateParams<TP>
        {
            AssertCheck(parameters, emptyLabelTestMessage);

            parameters.Label = label;
            parameters.Mandatory = true;
            AssertCheck(parameters, emptyDefaultValueTestMessage);

            parameters.DefaultValue = defaultValue;
            parameters.DataSource = new MetadataDataSourceParams(new List<EntryParams>());
            AssertCheck(parameters, nonEmptyDataSourceTestMessage);

            parameters.DataSource = null;
        }

        private static SortedDictionary<string, object> CheckParamsDictionary<T, TP>(
            T parameters, string fieldType, TP expectedDefaultValue)
            where T : MetadataFieldBaseParams<TP>
        {
            Assert.DoesNotThrow(parameters.Check);

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual(parameters.Label, dictionary["label"]);
            Assert.AreEqual("true", dictionary["mandatory"]);
            Assert.AreEqual(fieldType, dictionary["type"]);
            Assert.AreEqual(parameters.ExternalId, dictionary["external_id"]);
            if (expectedDefaultValue != null)
            {
                Assert.AreEqual(expectedDefaultValue, dictionary["default_value"]);
            }

            return dictionary;
        }

        private static void AssertForbiddenValidations<T, TP>(T metadataParams,
            IEnumerable<MetadataValidationParams> validationParams)
            where T : MetadataFieldBaseParams<TP>
        {
            validationParams.ToList().ForEach(validation =>
            {
                metadataParams.Validation = validation;
                AssertCheck(metadataParams, forbiddenValidationTestMessage);
            });
        }

        private static void AssertCheck<T>(T metadataParams,string expectedMessage)
            where T : BaseParams
        {
            Assert.Throws<ArgumentException>(metadataParams.Check, expectedMessage);
        }
    }
}
