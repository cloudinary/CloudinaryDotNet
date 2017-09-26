using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class GetFoldersResult : BaseResult
    {
        [DataMember(Name = "folders")]
        public List<Folder> Folders { get; set; }
        
    }

    [DataContract]
    public class Folder
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "path")]
        public string Path { get; set; }
    }
}
