using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Definition of the properties for deleting a transformation.
    /// </summary>
    public class DeleteTransformParams : BaseParams
    {
        /// <summary>
        /// The transformation object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (Name == null)
                throw new ArgumentException("Name must be set!");

        }

        /// <summary>
        /// Map object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();
            dict.Add("transformation", Name);
            return dict;
        }
    }
}