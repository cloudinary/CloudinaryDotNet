namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Detailed information about sub-account.
    /// </summary>
    [DataContract]
    public class SubAccountResult : BaseResult
    {
        /// <summary>
        /// Gets or sets an auto-generated unique identifier of the sub-account.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the cloud, provided by user or auto-generated.
        /// </summary>
        [DataMember(Name = "cloud_name")]
        public string CloudName { get; set; }

        /// <summary>
        /// Gets or sets the name of the sub-account.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether sub-accounts should always be created as enabled.
        /// Disabled accounts cannot perform any new API operations,
        /// i.e.no image upload, no new transformations.
        /// </summary>
        [DataMember(Name = "enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets list of ​enabled​ access keys for this account, sorted by ascending order
        /// of creation.On creation there will only be one.
        /// </summary>
        [DataMember(Name = "api_access_keys")]
        public ApiAccessKey[] ApiAccessKeys { get; set; }

        /// <summary>
        /// Gets or sets date when the sub-account was created.
        /// </summary>
        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets a list of ​enabled​ access keys for this account, sorted by ascending order
        /// of creation.On creation there will only be one.
        /// </summary>
        [DataMember(Name = "custom_attributes")]
        public StringDictionary CustomAttributes { get; set; }
    }
}
