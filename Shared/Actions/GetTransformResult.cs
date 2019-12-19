namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

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

        /// <summary>
        /// Indicates whether the transformation is a named transformation.
        /// </summary>
        [DataMember(Name = "named")]
        public bool Named { get; protected set; }

        /// <inheritdoc/>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            Name = source.ReadValueAsSnakeCase<string>(nameof(Name));
            Strict = source.ReadValue<bool>("allowed_for_strict");
            Used = source.ReadValueAsSnakeCase<bool>(nameof(Used));
            Derived = source.ReadList(nameof(Derived).ToSnakeCase(), _ => new TransformDerived(_)).ToArray();
            Info = source.ReadValueAsSnakeCase<Dictionary<string, object>[]>(nameof(Info));
            Named = source.ReadValueAsSnakeCase<bool>(nameof(Named));
        }
    }

    /// <summary>
    /// Settings of derived assets generated (and cached) from the original media asset.
    /// </summary>
    [DataContract]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
    public class TransformDerived
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransformDerived"/> class.
        /// </summary>
        public TransformDerived()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformDerived"/> class.
        /// </summary>
        /// <param name="source">JSON Token.</param>
        internal TransformDerived(JToken source)
        {
            m_resourceType = source.ReadValue<string>("resource_type");
            PublicId = source.ReadValueAsSnakeCase<string>(nameof(PublicId));
            Type = source.ReadValueAsSnakeCase<string>(nameof(Type));
            Format = source.ReadValueAsSnakeCase<string>(nameof(Format));
            Url = source.ReadValueAsSnakeCase<string>(nameof(Url));
            SecureUrl = source.ReadValueAsSnakeCase<string>(nameof(SecureUrl));
            Length = source.ReadValue<long>("bytes");
            Id = source.ReadValueAsSnakeCase<string>(nameof(Id));
        }

        /// <summary>
        /// The type of media asset: image, raw, or video.
        /// </summary>
        [DataMember(Name = "resource_type")]
        public string m_resourceType;

        /// <summary>
        /// The public identifier that is used for accessing the media asset.
        /// </summary>
        [DataMember(Name = "public_id")]
        public string PublicId { get; protected set; }

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
