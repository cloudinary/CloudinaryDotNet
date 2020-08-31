namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed details of a single transformation.
    /// </summary>
    [DataContract]
    public class GetTransformResult : BaseResult
    {
        /// <summary>
        /// Gets or sets the name of a named transformation (e.g., t_trans1) or the transformation itself as expressed in a dynamic
        /// URL (e.g., w_110,h_100,c_fill).
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether indicates whether the transformation can be used when strict transformations are enabled.
        /// </summary>
        [Obsolete("Property Strict is deprecated, please use AllowedForStrict instead")]
        public bool Strict
        {
            get { return AllowedForStrict; }
            set { AllowedForStrict = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the transformation can be used when strict transformations are enabled.
        /// </summary>
        [DataMember(Name = "allowed_for_strict")]
        public bool AllowedForStrict { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether indicates whether the transformation has been used to create a derived asset.
        /// </summary>
        [DataMember(Name = "used")]
        public bool Used { get; set; }

        /// <summary>
        /// Gets or sets any requested information from executing one of the Cloudinary Add-ons on the media asset.
        /// </summary>
        [DataMember(Name = "info")]
        public Dictionary<string, object>[] Info { get; set; }

        /// <summary>
        /// Gets or sets the list of derived assets generated (and cached) from the original media asset.
        /// </summary>
        [DataMember(Name = "derived")]
        public TransformDerived[] Derived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether indicates whether the transformation is a named transformation.
        /// </summary>
        [DataMember(Name = "named")]
        public bool Named { get; set; }
    }

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
