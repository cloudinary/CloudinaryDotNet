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
        /// Gets or sets a unique immutable identification string for the metadata field.
        /// </summary>
        [DataMember(Name = "external_id")]
        public string ExternalId { get; set; }

        /// <summary>
        ///  Gets or sets the type of value that can be assigned to the metadata field.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        ///  Gets or sets the label of the metadata field for display purposes.
        /// </summary>
        [DataMember(Name = "label")]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a value must be given for this field.
        /// </summary>
        [DataMember(Name = "mandatory")]
        public bool Mandatory { get; set; }

        /// <summary>
        /// Gets or sets the default value for the field.
        /// </summary>
        [DataMember(Name = "default_value")]
        public object DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets any validation rules to apply when values are entered (or updated) for this field.
        /// </summary>
        [DataMember(Name = "validation")]
        public MetadataValidationResult Validation { get; set; }

        /// <summary>
        ///  Gets or sets the predefined list of values, referenced by external_ids, available for this field.
        /// </summary>
        [DataMember(Name = "datasource")]
        public MetadataDataSourceResult DataSource { get; set; }
    }

    /// <summary>
    /// Result of removing the metadata field.
    /// </summary>
    [DataContract]
    public class DelMetadataFieldResult : BaseResult
    {
        /// <summary>
        /// Gets or sets an API message.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }

    /// <summary>
    /// Result of metadata fields listing.
    /// </summary>
    [DataContract]
    public class MetadataFieldListResult : BaseResult
    {
        /// <summary>
        ///  Gets or sets a list of basic details of listed metadata fields.
        /// </summary>
        [DataMember(Name = "metadata_fields")]
        public IEnumerable<MetadataFieldResult> MetadataFields { get; set; }
    }

    /// <summary>
    /// Represents a data source for a given field.
    /// </summary>
    [DataContract]
    public class MetadataDataSourceResult : BaseResult
    {
        /// <summary>
        ///  Gets or sets  a list of datasource values.
        /// </summary>
        [DataMember(Name = "values")]
        public List<EntryResult> Values { get; set; }
    }

    /// <summary>
    /// Defines a single possible value for the field.
    /// </summary>
    [DataContract]
    public class EntryResult
    {
        /// <summary>
        /// Gets or sets a unique immutable identification string for the datasource entry.
        /// </summary>
        [DataMember(Name = "external_id")]
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets the value for this datasource.
        /// </summary>
        [DataMember(Name = "value")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the state.
        ///
        /// Only given as part of a response - ignored on requests.
        /// It is immutable unless changed via DELETE of a datasource entry.
        /// Possible values: 'active' and 'inactive'.
        /// </summary>
        [DataMember(Name = "state")]
        public string State { get; set; }
    }

    /// <summary>
    /// Details on metadata field validation.
    /// </summary>
    [DataContract]
    public class MetadataValidationResult : BaseResult
    {
        /// <summary>
        /// Gets or sets validation type.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the value for validation.
        /// </summary>
        [DataMember(Name = "value")]
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets a value to indicate whether to check for equality.
        /// </summary>
        [DataMember(Name = "equals")]
        public bool? IsEqual { get; set; }

        /// <summary>
        /// Gets or sets the minimum string length.
        /// </summary>
        [DataMember(Name = "min")]
        public int? Min { get; set; }

        /// <summary>
        /// Gets or sets the maximum string length.
        /// </summary>
        [DataMember(Name = "max")]
        public int? Max { get; set; }

        /// <summary>
        /// Gets or sets rules combined with an 'AND' logic relation between them.
        /// </summary>
        [DataMember(Name = "rules")]
        public List<MetadataValidationResult> Rules { get; set; }
    }
}
