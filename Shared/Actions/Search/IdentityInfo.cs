namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents indetity data.
    /// </summary>
    [DataContract]
    public class IdentityInfo
    {
        /// <summary>
        /// Gets or sets identity access key.
        /// </summary>
        [DataMember(Name = "access_key")]
        public string AccessKey { get; set; }
    }
}