using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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

        [DataMember(Name = "moderation")]
        public List<Moderation> Moderation { get; protected set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; protected set; }

        [DataMember(Name = "tags")]
        public string[] Tags { get; protected set; }
        
        [DataMember(Name = "access_control")]
        public List<AccessControlRule> AccessControl { get; protected set; }
        
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
        
    }
}
