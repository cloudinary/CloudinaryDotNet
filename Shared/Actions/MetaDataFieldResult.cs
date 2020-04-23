namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Detailed information about metadata field.
    /// </summary>
    [DataContract]
    public class MetadataFieldResult : BaseResult
    {
        /// <summary>
        /// A unique immutable identification string for the metadata field.
        /// </summary>
        [DataMember(Name = "external_id")]
        public string ExternalId { get; protected set; }

        /// <summary>
        /// The type of value that can be assigned to the metadata field.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; protected set; }

        /// <summary>
        /// The label of the metadata field for display purposes.
        /// </summary>
        [DataMember(Name = "label")]
        public string Label { get; protected set; }

        /// <summary>
        /// Whether a value must be given for this field.
        /// </summary>
        [DataMember(Name = "mandatory")]
        public bool Mandatory { get; protected set; }

        /// <summary>
        /// The default value for the field.
        /// </summary>
        [DataMember(Name = "default_value")]
        public object DefaultValue { get; protected set; }

        /// <summary>
        /// Any validation rules to apply when values are entered (or updated) for this field.
        /// </summary>
        [DataMember(Name = "validation")]
        public MetadataValidationResult Validation { get; protected set; }

        /// <summary>
        /// The predefined list of values, referenced by external_ids, available for this field.
        /// </summary>
        [DataMember(Name = "datasource")]
        public MetadataDataSourceResult DataSource { get; protected set; }
    }

    /// <summary>
    /// Result of removing the metadata field.
    /// </summary>
    [DataContract]
    public class DelMetadataFieldResult : BaseResult
    {
        /// <summary>
        /// An API message.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; protected set; }
    }

    /// <summary>
    /// Result of metadata fields listing.
    /// </summary>
    [DataContract]
    public class MetadataFieldListResult : BaseResult
    {
        /// <summary>
        /// List of basic details of listed metadata fields.
        /// </summary>
        [DataMember(Name = "metadata_fields")]
        public IEnumerable<MetadataFieldResult> MetadataFields { get; protected set; }
    }

    /// <summary>
    /// Represents a data source for a given field.
    /// </summary>
    [DataContract]
    public class MetadataDataSourceResult : BaseResult
    {
        /// <summary>
        /// A list of datasource values.
        /// </summary>
        [DataMember(Name = "values")]
        public List<EntryResult> Values { get; protected set; }
    }

    /// <summary>
    /// Defines a single possible value for the field.
    /// </summary>
    [DataContract]
    public class EntryResult
    {
        /// <summary>
        /// A unique immutable identification string for the datasource entry.
        /// </summary>
        [DataMember(Name = "external_id")]
        public string ExternalId { get; protected set; }

        /// <summary>
        /// The value for this datasource.
        /// </summary>
        [DataMember(Name = "value")]
        public string Value { get; protected set; }

        /// <summary>
        /// Only given as part of a response - ignored on requests.
        /// It is immutable unless changed via DELETE of a datasource entry.
        /// Possible values: 'active' and 'inactive'.
        /// </summary>
        [DataMember(Name = "state")]
        public string State { get; protected set; }
    }

    /// <summary>
    /// Details on metadata field validation.
    /// </summary>
    [DataContract]
    public class MetadataValidationResult : BaseResult
    {
        /// <summary>
        /// Validation type.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; protected set; }

        /// <summary>
        /// The value for validation.
        /// </summary>
        [DataMember(Name = "value")]
        public object Value { get; protected set; }

        /// <summary>
        /// Whether to check for equality.
        /// </summary>
        [DataMember(Name = "equals")]
        public bool? IsEqual { get; protected set; }

        /// <summary>
        /// The minimum string length.
        /// </summary>
        [DataMember(Name = "min")]
        public int? Min { get; protected set; }

        /// <summary>
        /// The maximum string length.
        /// </summary>
        [DataMember(Name = "max")]
        public int? Max { get; protected set; }

        /// <summary>
        /// Rules combined with an 'AND' logic relation between them.
        /// </summary>
        [DataMember(Name = "rules")]
        public List<MetadataValidationResult> Rules { get; protected set; }
    }
}
