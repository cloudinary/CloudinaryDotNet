namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Parameters of the request for transformation details.
    /// </summary>
    public class GetTransformParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetTransformParams"/> class.
        /// </summary>
        public GetTransformParams()
        {
            Transformation = string.Empty;
        }

        /// <summary>
        /// Name of the transformation.
        /// </summary>
        public string Transformation { get; set; }

        /// <summary>
        /// Max number of derived resources to return. Default=10. Maximum=100.
        /// </summary>
        public int MaxResults { get; set; }

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

            if (MaxResults > 0)
            {
                AddParam(dict, "max_results", MaxResults.ToString());
            }

            return dict;
        }
    }
}
