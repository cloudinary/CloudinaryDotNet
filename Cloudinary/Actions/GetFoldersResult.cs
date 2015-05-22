using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

using Newtonsoft.Json;
namespace CloudinaryDotNet.Actions
{
    //[DataContract]
    public class GetFoldersResult : BaseResult
    {
        [JsonProperty(PropertyName = "folders")]
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

    //[DataContract]
    public class Folder
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }
    }
}
