namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Parsed result of folders listing.
    /// </summary>
    [DataContract]
    public class GetFoldersResult : BaseResult
    {
        /// <summary>
        /// List of folders.
        /// </summary>
        [DataMember(Name = "folders")]
        public List<Folder> Folders { get; set; }

        /// <inheritdoc/>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            Folders = source.ReadList(nameof(Folders).ToSnakeCase(), _ => new Folder(_));
        }
    }

    /// <summary>
    /// Folder details.
    /// </summary>
    [DataContract]
    public class Folder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        public Folder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        /// <param name="source">JSON Token.</param>
        internal Folder(JToken source)
        {
            Name = source.ReadValueAsSnakeCase<string>(nameof(Name));
            Path = source.ReadValueAsSnakeCase<string>(nameof(Path));
        }

        /// <summary>
        /// Name of the folder.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Path to the folder.
        /// </summary>
        [DataMember(Name = "path")]
        public string Path { get; set; }
    }
}
