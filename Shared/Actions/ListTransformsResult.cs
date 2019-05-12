using System;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
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
        
    }

    /// <summary>
    /// Details of a single transformation specified in your account.
    /// </summary>
    [DataContract]
    public class TransformDesc
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
    }
}
