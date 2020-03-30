namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

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

        /// <summary>
        /// When a listing request has more results to return than <see cref="GetFoldersParams.MaxResults"/>,
        /// the <see cref="NextCursor"/> value is returned as part of the response. You can then specify this value as
        /// the <see cref="NextCursor"/> parameter of the following listing request.
        /// </summary>
        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; protected set; }
    }

    /// <summary>
    /// Folder details.
    /// </summary>
    [DataContract]
    public class Folder
    {
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
