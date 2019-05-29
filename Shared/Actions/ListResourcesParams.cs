using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of list resources request.
    /// </summary>
    public class ListResourcesParams : BaseParams
    {
        /// <summary>
        /// Type of resource (image, raw).
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Type of resource (upload, facebook, etc).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Optional. Max number of resources to return. Default=10. Maximum=500.
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// If true, include list of tag names assigned for each resource.
        /// </summary>
        public bool Tags { get; set; }

        /// <summary>
        /// If true, include moderation status for each resource. 
        /// </summary>
        public bool Moderations { get; set; }

        /// <summary>
        /// If true, include context assigned to each resource.
        /// </summary>
        public bool Context { get; set; }

        /// <summary>
        /// When a listing request has more results to return than <see cref="ListResourcesParams.MaxResults"/>, 
        /// the <see cref="NextCursor"/> value is returned as part of the response. You can then specify this value as
        /// the <see cref="NextCursor"/> parameter of the following listing request.
        /// </summary>
        public string NextCursor { get; set; }

        /// <summary>
        /// Sorting direction (could be asc, desc, 1, -1).
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// List resources uploaded later than <see cref="StartAt"/>.
        /// </summary>
        public DateTime StartAt { get; set; }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            // ok
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            if (MaxResults > 0)
                AddParam(dict, "max_results", MaxResults.ToString());

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

    /// <summary>
    /// Allows to filter resources by specific public identifiers.
    /// </summary>
    public class ListSpecificResourcesParams : ListResourcesParams
    {
        /// <summary>
        /// Instantiates the <see cref="ListSpecificResourcesParams"/> object.
        /// </summary>
        public ListSpecificResourcesParams()
        {
            PublicIds = new List<string>();
        }

        /// <summary>
        /// Gets or sets the public identifiers to list.
        /// When set it overrides usage of <see cref="ListResourcesParams.Direction"/>.
        /// </summary>
        public List<string> PublicIds { get; set; }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            if (PublicIds != null && PublicIds.Count > 0)
            {
                AddParam(dict, "public_ids", PublicIds);

                if (dict.ContainsKey("direction"))
                    dict.Remove("direction");
            }

            return dict;
        }
    }

    /// <summary>
    /// Allow to filter resources by prefix.
    /// </summary>
    public class ListResourcesByPrefixParams : ListResourcesParams
    {
        /// <summary>
        /// Find all resources that their public ID starts with the given prefix.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "prefix", Prefix);

            return dict;
        }
    }

    /// <summary>
    /// Allows to filter resources by tag.
    /// </summary>
    public class ListResourcesByTagParams : ListResourcesParams
    {
        /// <summary>
        /// Gets or sets the tag to filter resources.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        /// <exception cref="System.ArgumentException">Tag must be set to list resource by tag!</exception>
        public override void Check()
        {
            base.Check();

            if (String.IsNullOrEmpty(Tag))
                throw new ArgumentException("Tag must be set to filter resources by tag!");
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            if (dict.ContainsKey("type"))
                dict.Remove("type");

            return dict;
        }
    }

    /// <summary>
    /// Allows to filter resources by moderation kind/status.
    /// </summary>
    public class ListResourcesByModerationParams : ListResourcesParams
    {
        /// <summary>
        /// Gets or sets the kind of the moderation (manual, etc.).
        /// </summary>
        public string ModerationKind { get; set; }

        /// <summary>
        /// Gets or sets the moderation status.
        /// </summary>
        public ModerationStatus ModerationStatus { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            base.Check();

            if (String.IsNullOrEmpty(ModerationKind))
                throw new ArgumentException("ModerationKind must be set to filter resources by moderation kind/status!");
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            if (dict.ContainsKey("type"))
                dict.Remove("type");

            return dict;
        }
    }
}
