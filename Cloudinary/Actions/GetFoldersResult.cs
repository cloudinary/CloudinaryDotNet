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

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static GetFoldersResult Parse(HttpWebResponse response)
        {
            return Parse<GetFoldersResult>(response);
        }
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
