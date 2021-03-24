namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Access key, enabled for the account.
    /// </summary>
    [DataContract]
    public class ApiAccessKey
    {
        /// <summary>
        /// Gets or sets account API key.
        /// </summary>
        [DataMember(Name = "key")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the account API secret.
        /// </summary>
        [DataMember(Name = "secret")]
        public string Secret { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether account is enabled or not.
        /// </summary>
        [DataMember(Name = "enabled")]
        public bool Enabled { get; set; }
    }
}
