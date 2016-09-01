using System.Net.Http;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class ListTransformsResult : BaseResult
    {
        [DataMember(Name = "transformations")]
        public TransformDesc[] Transformations { get; protected set; }

        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static ListTransformsResult Parse(HttpResponseMessage response)
        {
            return Parse<ListTransformsResult>(response);
        }
    }

    [DataContract]
    public class TransformDesc
    {
        [DataMember(Name = "name")]
        public string Name { get; protected set; }

        [DataMember(Name = "allowed_for_strict")]
        public bool Strict { get; protected set; }

        [DataMember(Name = "used")]
        public bool Used { get; protected set; }
    }
}
