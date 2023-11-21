namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// A single access key.
    /// </summary>
    [DataContract]
    public class AccessKeyResult : BaseResult
    {
        /// <summary>
        /// Gets or sets name of the access key.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets API key.
        /// </summary>
        [DataMember(Name = "api_key")]
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the API secret.
        /// </summary>
        [DataMember(Name = "api_secret")]
        public string ApiSecret { get; set; }

        /// <summary>
        /// Gets or sets date when the key was created.
        /// </summary>
        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets date when the key was updated.
        /// </summary>
        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether access key is enabled or not.
        /// </summary>
        [DataMember(Name = "enabled")]
        public bool Enabled { get; set; }
    }
}
