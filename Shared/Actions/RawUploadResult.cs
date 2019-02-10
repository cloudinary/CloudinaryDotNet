using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Results of raw file upload.
    /// </summary>
    [DataContract]
    public class RawUploadResult : UploadResult
    {
        /// <summary>
        /// The signature for verifying the response is a valid response from Cloudinary.
        /// </summary>
        [DataMember(Name = "signature")]
        public string Signature { get; protected set; }

        /// <summary>
        /// Type of the uploaded asset.
        /// </summary>
        [DataMember(Name = "resource_type")]
        public string ResourceType { get; protected set; }

        /// <summary>
        /// The array of data received from moderation service.
        /// </summary>
        [DataMember(Name = "moderation")]
        public List<Moderation> Moderation { get; protected set; }

        /// <summary>
        /// Date when the asset was uploaded.
        /// </summary>
        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; protected set; }

        /// <summary>
        /// The list of tags currently assigned to the asset.
        /// </summary>
        [DataMember(Name = "tags")]
        public string[] Tags { get; protected set; }

        /// <summary>
        /// An array of access types for the asset. The anonymous access type should also include 'start' and 'end'
        /// dates (in ISO 8601 format) defining when the resource is publicly available.
        /// </summary>
        [DataMember(Name = "access_control")]
        public List<AccessControlRule> AccessControl { get; protected set; }
        
    }

    /// <summary>
    /// Results of a file's chunk uploading.
    /// </summary>
    [DataContract]
    public class RawPartUploadResult : RawUploadResult
    {
        /// <summary>
        /// Id of the uploaded chunk of the asset.
        /// </summary>
        [DataMember(Name = "upload_id")]
        public string UploadId { get; protected set; }
        
    }
}
