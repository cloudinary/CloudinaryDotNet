using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Runtime.Serialization;


namespace CloudinaryDotNet.Actions
{
    //[DataContract]
    public class ListTransformsResult : BaseResult
    {
        [JsonProperty(PropertyName = "transformations")]
        public TransformDesc[] Transformations { get; protected set; }

        [JsonProperty(PropertyName = "next_cursor")]
        public string NextCursor { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static ListTransformsResult Parse(HttpWebResponse response)
        {
            return Parse<ListTransformsResult>(response);
        }
    }

    //[DataContract]
    public class TransformDesc
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; protected set; }

        [JsonProperty(PropertyName = "allowed_for_strict")]
        public bool Strict { get; protected set; }

        [JsonProperty(PropertyName = "used")]
        public bool Used { get; protected set; }
    }
}
