namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed result of the access keys listing request.
    /// </summary>
    [DataContract]
    public class ListAccessKeysResult : BaseResult
    {
        /// <summary>
        /// Gets or sets a list of the access keys matching the request conditions.
        /// </summary>
        [DataMember(Name = "access_keys")]
        public AccessKeyResult[] AccessKeys { get; set; }

        /// <summary>
        /// Gets or sets a number of total access keys.
        /// </summary>
        [DataMember(Name = "total")]
        public int Total { get; set; }
    }
}
