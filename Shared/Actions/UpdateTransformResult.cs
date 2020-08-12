namespace CloudinaryDotNet.Actions
{
    using System;
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
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this transformation is allowed when Strict Transformations are enabled.
        /// </summary>
        [DataMember(Name = "allowed_for_strict")]
        public bool AllowedForStrict { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the transformation was ever used.
        /// </summary>
        [DataMember(Name = "used")]
        public bool Used { get; set; }

        /// <summary>
        /// Gets or sets detailed info about the transformation.
        /// </summary>
        [DataMember(Name = "info")]
        public Dictionary<string, string>[] Info { get; set; }

        /// <summary>
        /// Gets or sets an array with derived assets settings.
        /// </summary>
        [DataMember(Name = "derived")]
        public TransformDerived[] Derived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this transformation is allowed when Strict Transformations are enabled.
        /// </summary>
        [Obsolete("Property Strict is deprecated, please use AllowedForStrict instead")]
        public bool Strict
        {
            get { return AllowedForStrict; }
            set { AllowedForStrict = value; }
        }

        /// <summary>
        /// Gets or sets result message.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
