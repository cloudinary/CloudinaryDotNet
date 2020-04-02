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
        /// Gets or sets the transformation for unsafe updating.
        /// </summary>
        /// <value>
        /// New transformation.
        /// </value>
        public Transformation UnsafeTransform { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this transformation is allowed when Strict Transformations are enabled.
        /// </summary>
        public bool Strict { get; set; }

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
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "allowed_for_strict", Strict ? "true" : "false");

            if (UnsafeTransform != null)
            {
                AddParam(dict, "unsafe_update", UnsafeTransform.Generate());
            }

            return dict;
        }
    }
}
