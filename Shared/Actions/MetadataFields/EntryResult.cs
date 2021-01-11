namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

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
}