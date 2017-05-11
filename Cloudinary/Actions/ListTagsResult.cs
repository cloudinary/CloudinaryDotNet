using System.IO;
using System.Net;
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

    }
}
