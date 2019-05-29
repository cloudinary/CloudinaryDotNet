using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Response of transformation update.
    /// </summary>
    [DataContract]
    public class UpdateTransformResult : BaseResult
    {
        /// <summary>
        /// The name of transformation.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; protected set; }

        /// <summary>
        /// Whether this transformation is allowed when Strict Transformations are enabled.
        /// </summary>
        [DataMember(Name = "allowed_for_strict")]
        public bool Strict { get; protected set; }

        /// <summary>
        /// Whether the transformation was ever used.
        /// </summary>
        [DataMember(Name = "used")]
        public bool Used { get; protected set; }

        /// <summary>
        /// Detailed info about the transformation.
        /// </summary>
        [DataMember(Name = "info")]
        public Dictionary<string, string>[] Info { get; protected set; }

        /// <summary>
        /// An array with derived assets settings.
        /// </summary>
        [DataMember(Name = "derived")]
        public TransformDerived[] Derived { get; protected set; }
        
    }
}
