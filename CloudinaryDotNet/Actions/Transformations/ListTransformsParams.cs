namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Parameters of the request for a list of transformation.
    /// </summary>
    public class ListTransformsParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListTransformsParams"/> class.
        /// </summary>
        public ListTransformsParams()
        {
            NextCursor = string.Empty;
        }

        /// <summary>
        /// Gets or sets max number of transformations to return. Default=10. Maximum=500.
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// Gets or sets whether to return named transformations or not.
        /// </summary>
        public bool? Named { get; set; }

        /// <summary>
        /// Gets or sets a value for a situation when a listing request has more results to return than <see cref="MaxResults"/>,
        /// the <see cref="ListTransformsResult.NextCursor"/> value is returned as part of the response. You can then
        /// specify this value as the <see cref="NextCursor"/> parameter of the following listing request.
        /// </summary>
        public string NextCursor { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            // ok
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

            if (Named.HasValue)
            {
                AddParam(dict, "named", string.Format(CultureInfo.InvariantCulture, "{0}", Named.Value));
            }

            if (!string.IsNullOrWhiteSpace(NextCursor))
            {
                AddParam(dict, "next_cursor", NextCursor);
            }

            return dict;
        }
    }
}
