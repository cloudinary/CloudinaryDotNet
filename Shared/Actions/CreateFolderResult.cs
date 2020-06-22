namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Result of folder creation.
    /// </summary>
    [DataContract]
    public class CreateFolderResult : BaseResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether folder creation was successful.
        /// </summary>
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets path to the folder.
        /// </summary>
        [DataMember(Name = "path")]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets name of the folder.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
