namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Parameters for transformation update.
    /// </summary>
    public class UpdateTransformParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTransformParams"/> class.
        /// </summary>
        public UpdateTransformParams()
        {
            Transformation = string.Empty;
        }

        /// <summary>
        /// Gets or sets transformation represented as string.
        /// </summary>
        public string Transformation { get; set; }

        /// <summary>
        /// Gets or sets unsafe update transformation.
        ///
        /// Optional. Allows updating an existing named transformation without updating all associated derived images
        /// (the new settings of the named transformation only take effect from now on).
        /// </summary>
        /// <value>
        /// New transformation.
        /// </value>
        [Obsolete("Property UnsafeTransform is deprecated, please use UnsafeUpdate instead")]
        public Transformation UnsafeTransform
        {
            get { return UnsafeUpdate; }
            set { UnsafeUpdate = value; }
        }

        /// <summary>
        /// Gets or sets unsafe update transformation.
        ///
        /// Optional. Allows updating an existing named transformation without updating all associated derived images
        /// (the new settings of the named transformation only take effect from now on).
        /// </summary>
        /// <value>
        /// New transformation.
        /// </value>
        public Transformation UnsafeUpdate { get; set; }

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
        /// Gets or sets a value indicating whether this transformation is allowed when Strict Transformations are enabled.
        /// </summary>
        public bool AllowedForStrict { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (string.IsNullOrEmpty(Transformation))
            {
                throw new ArgumentException("Transformation must be set!");
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = base.ToParamsDictionary();

            AddParam(dict, "allowed_for_strict", AllowedForStrict);

            // FIXME: dirty hack to avoid signing of admin api parameters.
            AddParam(dict, "unsigned", "true");
            AddParam(dict, "removeUnsignedParam", "true");

            if (UnsafeUpdate != null)
            {
                AddParam(dict, "unsafe_update", UnsafeUpdate.Generate());
            }

            if (!string.IsNullOrEmpty(Transformation))
            {
                AddParam(dict, "transformation", Transformation);
            }

            return dict;
        }
    }
}
