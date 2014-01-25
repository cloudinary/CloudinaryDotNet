using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    public class ListResourcesParams : BaseParams
    {
        /// <summary>
        /// Gets or sets the type of the resource.
        /// </summary>
        /// <value>
        /// The type of the resource.
        /// </value>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Find all resources that their public ID starts with the given prefix.
        /// Does not effect if <see cref="Tag"/> or <see cref="PublicIds"/> are set.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Type of resource (upload, facebook, etc).
        /// Does not effect if <see cref="Tag"/> is set.
        /// </summary>
        public string Type { get; set; }

        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets the public identifiers to list.
        /// Does not effect if <see cref="Tag"/> is set.
        /// </summary>
        public List<string> PublicIds { get; set; }

        /// <summary>
        /// Optional. Max number of resources to return. Default=10. Maximum=500.
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// If true, include the list of tag names assigned to each resource.
        /// </summary>
        public bool Tags { get; set; }

        /// <summary>
        /// If true, include context assigned to each resource.
        /// </summary>
        public bool Context { get; set; }

        /// <summary>
        /// Optional.
        /// </summary>
        public string NextCursor { get; set; }

        /// <summary>
        /// Sorting direction (could be asc, desc, 1, -1).
        /// </summary>
        public string Direction { get; set; }

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
            var dict = new SortedDictionary<string, object>();

            if (MaxResults > 0)
                AddParam(dict, "max_results", MaxResults.ToString());

            AddParam(dict, "next_cursor", NextCursor);
            AddParam(dict, "tags", Tags);
            AddParam(dict, "context", Context);

            if (PublicIds == null || PublicIds.Count == 0)
                AddParam(dict, "direction", Direction);

            if (String.IsNullOrEmpty(Tag))
            {
                AddParam(dict, "type", Type);

                if (PublicIds != null && PublicIds.Count > 0)
                {
                    AddParam(dict, "public_ids", PublicIds);
                }
                else
                {
                    AddParam(dict, "prefix", Prefix);
                }
            }

            return dict;
        }
    }
}
