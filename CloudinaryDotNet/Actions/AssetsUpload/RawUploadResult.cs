namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Results of raw file upload.
    /// </summary>
    [DataContract]
    public class RawUploadResult : UploadResult
    {
        /// <summary>
        /// Gets or sets the signature for verifying the response is a valid response from Cloudinary.
        /// </summary>
        [DataMember(Name = "signature")]
        public string Signature { get; set; }

        /// <summary>
        /// Gets or sets storage type.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets type of the uploaded asset.
        /// </summary>
        [DataMember(Name = "resource_type")]
        public string ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the array of data received from moderation service.
        /// </summary>
        [DataMember(Name = "moderation")]
        public List<Moderation> Moderation { get; set; }

        /// <summary>
        /// Gets or sets date when the asset was uploaded.
        /// </summary>
        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the list of tags currently assigned to the asset.
        /// </summary>
        [DataMember(Name = "tags")]
        public string[] Tags { get; set; }

        /// <summary>
        /// Gets or sets an array of access types for the asset. The anonymous access type should also include 'start' and 'end'
        /// dates (in ISO 8601 format) defining when the resource is publicly available.
        /// </summary>
        [DataMember(Name = "access_control")]
        public List<AccessControlRule> AccessControl { get; set; }

        /// <summary>
        /// Gets the Fully Qualified Public ID.
        /// </summary>
        public string FullyQualifiedPublicId => $"{ResourceType}/{Type}/{PublicId}";

        /// <summary>
        /// Gets or sets the accessibility mode of the media asset: public, or authenticated.
        /// </summary>
        [DataMember(Name = "access_mode")]
        public string AccessMode { get; set; }

        /// <summary>
        /// Gets or sets etag.
        ///
        /// Used to determine whether two versions of an asset are identical.
        /// </summary>
        [DataMember(Name = "etag")]
        public string Etag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a placeholder (default image) is currently used instead of
        /// displaying the image (due to moderation).
        /// </summary>
        [DataMember(Name = "placeholder")]
        public bool Placeholder { get; set; }

        /// <summary>
        /// Gets or sets the name of the media asset when originally uploaded. Relevant when delivering assets
        /// as attachments (setting the flag transformation parameter to attachment).
        /// </summary>
        [DataMember(Name = "original_filename")]
        public string OriginalFilename { get; set; }
    }
}
