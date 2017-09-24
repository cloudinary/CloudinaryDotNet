using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    public class CreateTransformParams : BaseParams
    {
        public string Name { get; set; }

        /// <summary>
        /// Strict representation of transformation parameters.
        /// </summary>
        public Transformation Transform { get; set; }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentException("Name must be set!");

            if (Transform == null)
                throw new ArgumentException("Transform must be defined!");
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();
            dict.Add("transformation", Transform.Generate());
            return dict;
        }
    }
}
