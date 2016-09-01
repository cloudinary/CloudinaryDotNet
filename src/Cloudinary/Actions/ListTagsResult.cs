using System.Net.Http;
using System.Runtime.Serialization;

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
        internal static ListTagsResult Parse(HttpResponseMessage response)
        {
            return Parse<ListTagsResult>(response);
        }
    }
}
