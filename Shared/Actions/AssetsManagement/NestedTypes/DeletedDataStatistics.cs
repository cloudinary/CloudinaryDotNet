namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed result of statistics of deleted resource.
    /// </summary>
    [DataContract]
    public class DeletedDataStatistics
    {
        /// <summary>
        /// Gets or sets count of original resources deleted.
        /// </summary>
        [DataMember(Name = "original")]
        public int Original { get; set; }

        /// <summary>
        /// Gets or sets count of derived resources deleted.
        /// </summary>
        [DataMember(Name = "derived")]
        public int Derived { get; set; }
    }
}