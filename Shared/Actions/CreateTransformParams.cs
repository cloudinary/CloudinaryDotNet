using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Definition of the properties for creation of transformation.
    /// </summary>
    public class CreateTransformParams : BaseParams
    {
        /// <summary>
        /// Name of the transformation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Strict representation of transformation parameters.
        /// </summary>
        public Transformation Transform { get; set; }

        /// <summary>
        /// [optional] The transformation's extension. 
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentException("Name must be set!");

            if (Transform == null)
                throw new ArgumentException("Transform must be defined!");
        }

        /// <summary>
        /// Map object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            string transformationStr = Transform.Generate();

            if (Format != null)
            {
                transformationStr += $"/{ Format}";
            }
           
            dict.Add("transformation", transformationStr);
            dict.Add("name", Name);

            return dict;
        }
    }
}
