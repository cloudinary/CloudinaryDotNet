namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

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
}