namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Folder details.
    /// </summary>
    [DataContract]
    public class Folder
    {
        /// <summary>
        /// Gets or sets name of the folder.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets path to the folder.
        /// </summary>
        [DataMember(Name = "path")]
        public string Path { get; set; }
    }
}