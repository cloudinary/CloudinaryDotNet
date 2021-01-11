namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

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
}