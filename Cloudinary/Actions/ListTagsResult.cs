using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Runtime.Serialization;


namespace CloudinaryDotNet.Actions
{
    //[DataContract]
    public class ListTagsResult : BaseResult
    {
        [JsonProperty(PropertyName = "tags")]
        public string[] Tags { get; protected set; }

        [JsonProperty(PropertyName = "next_cursor")]
        public string NextCursor { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static ListTagsResult Parse(HttpWebResponse response)
        {
            return Parse<ListTagsResult>(response);
        }
    }
}
