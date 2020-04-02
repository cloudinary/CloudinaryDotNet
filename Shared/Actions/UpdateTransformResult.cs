﻿namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Response of transformation update.
    /// </summary>
    [DataContract]
    public class UpdateTransformResult : BaseResult
    {
        /// <summary>
        /// Gets or sets the name of transformation.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this transformation is allowed when Strict Transformations are enabled.
        /// </summary>
        [DataMember(Name = "allowed_for_strict")]
        public bool Strict { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the transformation was ever used.
        /// </summary>
        [DataMember(Name = "used")]
        public bool Used { get; protected set; }

        /// <summary>
        /// Gets or sets detailed info about the transformation.
        /// </summary>
        [DataMember(Name = "info")]
        public Dictionary<string, string>[] Info { get; protected set; }

        /// <summary>
        /// Gets or sets an array with derived assets settings.
        /// </summary>
        [DataMember(Name = "derived")]
        public TransformDerived[] Derived { get; protected set; }
    }
}
