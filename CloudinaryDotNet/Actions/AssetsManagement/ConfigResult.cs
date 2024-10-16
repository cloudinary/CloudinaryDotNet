namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents the configuration details of your Cloudinary account.
    /// </summary>
    [DataContract]
    public class ConfigResult : BaseResult
    {
        /// <summary>
        /// Gets or sets the cloud name associated with your account.
        /// </summary>
        [DataMember(Name = "cloud_name")]
        public string CloudName { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the account.
        /// </summary>
        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the settings associated with your account.
        /// </summary>
        [DataMember(Name = "settings")]
        public AccountSettings Settings { get; set; }
    }
}
