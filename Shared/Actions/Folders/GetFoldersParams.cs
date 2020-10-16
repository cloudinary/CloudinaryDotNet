namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Parameters for managing folders list.
    /// </summary>
    public class GetFoldersParams : BaseParams
    {
        /// <summary>
        /// Gets or sets maximum number of results to return (up to 500). Default: 10.
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// Gets or sets when a request has more results to return than <see cref="MaxResults"/>, this value is returned as part of the response.
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
            var dict = base.ToParamsDictionary();

            if (MaxResults > 0)
            {
                AddParam(dict, "max_results", MaxResults.ToString(CultureInfo.InvariantCulture));
            }

            AddParam(dict, "next_cursor", NextCursor);

            return dict;
        }
    }
}
