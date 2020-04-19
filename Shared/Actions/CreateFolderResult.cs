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
        /// Determine whether folder creation was successful.
        /// </summary>
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        /// <summary>
        /// Path to the folder.
        /// </summary>
        [DataMember(Name = "path")]
        public string Path { get; set; }

        /// <summary>
        /// Name of the folder.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
