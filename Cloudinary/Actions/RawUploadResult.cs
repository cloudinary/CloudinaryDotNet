using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
//using System.Runtime.Serialization.Json;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Results of file uploading
    /// </summary>
    //[DataContract]
    public class RawUploadResult : UploadResult
    {
        /// <summary>
        /// Signature
        /// </summary>
        [JsonProperty(PropertyName = "signature")]
        public string Signature { get; protected set; }

        /// <summary>
        /// Resource type
        /// </summary>
        [JsonProperty(PropertyName = "resource_type")]
        public string ResourceType { get; protected set; }

        /// <summary>
        /// File size (in bytes)
        /// </summary>
        [JsonProperty(PropertyName = "bytes")]
        public long Length { get; protected set; }

        [JsonProperty(PropertyName = "moderation")]
        public List<Moderation> Moderation { get; protected set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; protected set; }

        [JsonProperty(PropertyName = "tags")]
        public string[] Tags { get; protected set; }

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
    //[DataContract]
    public class RawPartUploadResult : RawUploadResult
    {
        /// <summary>
        /// Signature
        /// </summary>
        [JsonProperty(PropertyName = "upload_id")]
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
