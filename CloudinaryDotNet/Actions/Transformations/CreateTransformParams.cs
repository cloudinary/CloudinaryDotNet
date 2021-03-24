namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Definition of the properties for creation of transformation.
    /// </summary>
    public class CreateTransformParams : BaseParams
    {
        /// <summary>
        /// Gets or sets name of the transformation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets strict representation of transformation parameters.
        /// </summary>
        public Transformation Transform { get; set; }

        /// <summary>
        /// Gets or sets transformation extension. Optional.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new ArgumentException("Name must be set!");
            }

            if (Transform == null)
            {
                throw new ArgumentException("Transform must be defined!");
            }
        }

        /// <summary>
        /// Map object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = base.ToParamsDictionary();

            string transformationStr = Transform.Generate();
            if (Format != null)
            {
                transformationStr += $"/{Format}";
            }

            dict.Add("transformation", transformationStr);
            dict.Add("name", Name);

            return dict;
        }
    }
}
