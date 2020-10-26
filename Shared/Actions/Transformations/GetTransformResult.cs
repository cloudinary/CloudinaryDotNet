namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
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
}
