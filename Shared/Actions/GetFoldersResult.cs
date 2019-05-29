using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
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
