using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parsed details of a single transformation.
    /// </summary>
    [DataContract]
    public class GetTransformResult : BaseResult
    {
        /// <summary>
        /// The name of a named transformation (e.g., t_trans1) or the transformation itself as expressed in a dynamic
        /// URL (e.g., w_110,h_100,c_fill).
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; protected set; }

        /// <summary>
        /// Indicates whether the transformation can be used when strict transformations are enabled.
        /// </summary>
        [DataMember(Name = "allowed_for_strict")]
        public bool Strict { get; protected set; }

        /// <summary>
        /// Indicates whether the transformation has been used to create a derived asset.
        /// </summary>
        [DataMember(Name = "used")]
        public bool Used { get; protected set; }

        /// <summary>
        /// Any requested information from executing one of the Cloudinary Add-ons on the media asset.
        /// </summary>
        [DataMember(Name = "info")]
        public Dictionary<string, object>[] Info { get; protected set; }

        /// <summary>
        /// The list of derived assets generated (and cached) from the original media asset.
        /// </summary>
        [DataMember(Name = "derived")]
        public TransformDerived[] Derived { get; protected set; }
        
    }

    /// <summary>
    /// Settings of derived assets generated (and cached) from the original media asset.
    /// </summary>
    [DataContract]
    public class TransformDerived
    {
        /// <summary>
        /// The public identifier that is used for accessing the media asset.
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; protected set; }

        /// <summary>
        /// The type of media asset: image, raw, or video.
        /// </summary>
        [DataMember(Name = "resource_type")]
        public string m_resourceType;

        /// <summary>
        /// The type of media asset: image, raw, or video.
        /// </summary>
        public ResourceType ResourceType
        {
            get { return Api.ParseCloudinaryParam<ResourceType>(m_resourceType); }
        }

        /// <summary>
        /// The accessibility type of the media asset: upload, private or authenticated.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; protected set; }

        /// <summary>
        /// The format of the media asset.
        /// </summary>
        [DataMember(Name = "format")]
        public string Format { get; protected set; }

        /// <summary>
        /// URL for accessing the derived media asset.
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; protected set; }

        /// <summary>
        /// The HTTPS URL for securely accessing the derived media asset.
        /// </summary>
        [DataMember(Name = "secure_url")]
        public string SecureUrl { get; protected set; }

        /// <summary>
        /// The size of the media asset in bytes.
        /// </summary>
        [DataMember(Name = "bytes")]
        public long Length { get; protected set; }

        /// <summary>
        /// Id of the generated derived resource.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; protected set; }
    }
}
