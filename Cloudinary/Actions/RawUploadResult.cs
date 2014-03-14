using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Results of file uploading
    /// </summary>
    [DataContract]
    public class RawUploadResult : UploadResult
    {
        /// <summary>
        /// Signature
        /// </summary>
        [DataMember(Name = "signature")]
        public string Signature { get; protected set; }

        /// <summary>
        /// Resource type
        /// </summary>
        [DataMember(Name = "resource_type")]
        public string ResourceType { get; protected set; }

        /// <summary>
        /// File size (in bytes)
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Length { get; protected set; }

        [DataMember(Name = "moderation")]
        public List<Moderation> Moderation { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static RawUploadResult Parse(HttpWebResponse response)
        {
            return Parse<RawUploadResult>(response);
        }
    }

    /// <summary>
    /// Results of a file's chunk uploading
    /// </summary>
    [DataContract]
    public class RawPartUploadResult : RawUploadResult
    {
        /// <summary>
        /// Signature
        /// </summary>
        [DataMember(Name = "upload_id")]
        public string UploadId { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static new RawPartUploadResult Parse(HttpWebResponse response)
        {
            return Parse<RawPartUploadResult>(response);
        }
    }
}
