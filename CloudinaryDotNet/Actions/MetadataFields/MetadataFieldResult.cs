namespace CloudinaryDotNet.Actions
{
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
}
