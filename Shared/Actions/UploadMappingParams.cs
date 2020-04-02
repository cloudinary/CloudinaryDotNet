namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Parameters for Mapping of folders to URL prefixes for dynamic image fetching from existing online locations.
    /// </summary>
    public class UploadMappingParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UploadMappingParams"/> class.
        /// Default constructor.
        /// </summary>
        public UploadMappingParams()
        {
        }

        /// <summary>
        /// Gets or sets a value for a situation when a listing request has more results to return than <see cref="MaxResults"/>, the
        /// <see cref="NextCursor"/> value is returned as part of the response. You can then specify this value as the
        /// <see cref="NextCursor"/> parameter of the following listing request. Optional.
        /// </summary>
        public string NextCursor { get; set; }

        /// <summary>
        /// Gets or sets max number of upload mappings to return. Default=10. Maximum=500. Optional.
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// Gets or sets the name for the Folder to map.
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// Gets or sets the URL to be mapped to the <see cref="Folder"/>.
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (MaxResults > 500)
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture, "The maximal count of folders to return is 500, but {0} given!", MaxResults));
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();
            AddParam(dict, "folder", Folder);
            AddParam(dict, "template", Template);
            if (MaxResults > 0)
            {
                AddParam(dict, "max_results", MaxResults);
            }

            if (!string.IsNullOrEmpty(NextCursor))
            {
                AddParam(dict, "next_cursor", NextCursor);
            }

            return dict;
        }
    }
}
