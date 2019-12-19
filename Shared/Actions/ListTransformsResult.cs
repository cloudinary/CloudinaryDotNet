namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Parsed list of transformations details.
    /// </summary>
    [DataContract]
    public class ListTransformsResult : BaseResult
    {
        /// <summary>
        /// A listing of transformations specified in your account.
        /// </summary>
        [DataMember(Name = "transformations")]
        public TransformDesc[] Transformations { get; protected set; }

        /// <summary>
        /// When a listing request has more results to return than <see cref="ListTransformsParams.MaxResults"/>,
        /// the <see cref="NextCursor"/> value is returned as part of the response. You can then specify this value as
        /// the <see cref="ListTransformsParams.NextCursor"/> parameter of the following listing request.
        /// </summary>
        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; protected set; }

        /// <inheritdoc/>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            NextCursor = source.ReadValueAsSnakeCase<string>(nameof(NextCursor));
            Transformations = source.ReadList(nameof(Transformations).ToCamelCase(), _ => new TransformDesc(_)).ToArray();
        }
    }

    /// <summary>
    /// Details of a single transformation specified in your account.
    /// </summary>
    [DataContract]
    public class TransformDesc
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransformDesc"/> class.
        /// </summary>
        public TransformDesc()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformDesc"/> class.
        /// </summary>
        /// <param name="source">JSON Token.</param>
        internal TransformDesc(JToken source)
        {
            Name = source.ReadValueAsSnakeCase<string>(nameof(Name));
            Strict = source.ReadValue<bool>("allowed_for_strict");
            Used = source.ReadValueAsSnakeCase<bool>(nameof(Used));
            Named = source.ReadValueAsSnakeCase<bool>(nameof(Named));
        }

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
        /// Indicates whether the transformation is a named transformation.
        /// </summary>
        [DataMember(Name = "named")]
        public bool Named { get; protected set; }
    }
}
