namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed result of folders listing.
    /// </summary>
    [DataContract]
    public class RenameFolderResult : BaseResult
    {
        /// <summary>
        /// Gets or sets original folder.
        /// </summary>
        [DataMember(Name = "from")]
        public Folder From { get; set; }

        /// <summary>
        /// Gets or sets new folder.
        /// </summary>
        [DataMember(Name = "to")]
        public Folder To { get; set; }
    }
}
