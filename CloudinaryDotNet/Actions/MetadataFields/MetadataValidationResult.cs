namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

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