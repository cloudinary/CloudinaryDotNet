namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents the settings of your Cloudinary account.
    /// </summary>
    [DataContract]
    public class AccountSettings
    {
        /// <summary>
        /// Gets or sets the folder mode.
        /// </summary>
        [DataMember(Name = "folder_mode")]
        public string FolderMode { get; set; }
    }
}
