namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Parameters of list resources request.
    /// </summary>
    public class ListResourcesParams : BaseParams
    {
        /// <summary>
        /// Gets or sets type of resource (image, raw).
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Gets or sets type of resource (upload, facebook, etc).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets max number of resources to return. Default=10. Maximum=500. Optional.
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if true, include list of tag names assigned for each resource.
        /// </summary>
        public bool Tags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if true, include moderation status for each resource.
        /// </summary>
        public bool Moderations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if true, include context assigned to each resource.
        /// </summary>
        public bool Context { get; set; }

        /// <summary>
        /// Gets or sets when a listing request has more results to return than <see cref="ListResourcesParams.MaxResults"/>,
        /// the <see cref="NextCursor"/> value is returned as part of the response. You can then specify this value as
        /// the <see cref="NextCursor"/> parameter of the following listing request.
        /// </summary>
        public string NextCursor { get; set; }

        /// <summary>
        /// Gets or sets sorting direction (could be asc, desc, 1, -1).
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// Gets or sets list resources uploaded later than <see cref="StartAt"/>.
        /// </summary>
        public DateTime StartAt { get; set; }

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

            AddParam(dict, "start_at", StartAt);
            AddParam(dict, "next_cursor", NextCursor);
            AddParam(dict, "tags", Tags);
            AddParam(dict, "moderations", Moderations);
            AddParam(dict, "context", Context);
            AddParam(dict, "direction", Direction);
            AddParam(dict, "type", Type);

            return dict;
        }
    }
}
