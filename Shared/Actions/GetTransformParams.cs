namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

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
        /// Gets or sets name of the transformation.
        /// </summary>
        public string Transformation { get; set; }

        /// <summary>
        /// Gets or sets max number of derived resources to return. Default=10. Maximum=100.
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// Optional. When a request has more results to return than max_results,
        /// the next_cursor value is returned as part of the response.
        /// You can then specify this value as the next_cursor parameter of a following request.
        /// </summary>
        public string NextCursor { get; set; }

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
                AddParam(dict, "max_results", MaxResults.ToString(CultureInfo.InvariantCulture));
            }

            AddParam(dict, "next_cursor", NextCursor);

            return dict;
        }
    }
}
