namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    /// <summary>
    /// Settings of derived assets generated (and cached) from the original media asset.
    /// </summary>
    [DataContract]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
    public class TransformDerived
    {
        /// <summary>
        /// The type of media asset: image, raw, or video.
        /// </summary>
        [DataMember(Name = "resource_type")]
        public string m_resourceType;

        /// <summary>
        /// Gets or sets the public identifier that is used for accessing the media asset.
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; set; }

        /// <summary>
        /// Gets the type of media asset: image, raw, or video.
        /// </summary>
        public ResourceType ResourceType
        {
            get { return Api.ParseCloudinaryParam<ResourceType>(m_resourceType); }
        }

        /// <summary>
        /// Gets or sets the accessibility type of the media asset: upload, private or authenticated.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the format of the media asset.
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets URL for accessing the derived media asset.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the HTTPS URL for securely accessing the derived media asset.
        /// </summary>
        [DataMember(Name = "secure_url")]
        public string SecureUrl { get; set; }

        /// <summary>
        /// Gets or sets the size of the media asset in bytes.
        /// </summary>
        [Obsolete("Property Length is deprecated, please use Bytes instead")]
        public long Length
        {
            get { return Bytes; }
            set { Bytes = value; }
        }

        /// <summary>
        /// Gets or sets the size of the media asset in bytes.
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Bytes { get; set; }

        /// <summary>
        /// Gets or sets id of the generated derived resource.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }
    }
}