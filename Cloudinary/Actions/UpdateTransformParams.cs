using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    public class UpdateTransformParams : BaseParams
    {
        public UpdateTransformParams()
        {
            Transformation = String.Empty;
        }

        public string Transformation { get; set; }

        /// <summary>
        /// Whether this transformation is allowed when Strict Transformations are enabled.
        /// </summary>
        public bool Strict { get; set; }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            if (String.IsNullOrEmpty(Transformation))
                throw new ArgumentException("Transformation must be set!");
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = new SortedDictionary<string, object>();

            AddParam(dict, "allowed_for_strict", Strict ? "true" : "false");

            return dict;
        }
    }
}
