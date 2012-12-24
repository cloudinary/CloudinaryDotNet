using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class ListTagsResult : BaseResult
    {
        [DataMember(Name = "tags")]
        public string[] Tags { get; protected set; }

        [DataMember(Name = "next_cursor")]
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
