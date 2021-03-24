namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Definition of the properties for deleting a transformation.
    /// </summary>
    public class DeleteTransformParams : BaseParams
    {
        /// <summary>
        /// Gets or sets name of the transformation.
        /// </summary>
        public string Transformation { get; set; }

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
        /// Map object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = base.ToParamsDictionary();
            AddParam(dict, "transformation", Transformation);

            return dict;
        }
    }
}
