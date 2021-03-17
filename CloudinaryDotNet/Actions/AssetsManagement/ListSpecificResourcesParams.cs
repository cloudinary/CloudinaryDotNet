namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Allows to filter resources by specific public identifiers.
    /// </summary>
    public class ListSpecificResourcesParams : ListResourcesParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListSpecificResourcesParams"/> class.
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
                {
                    dict.Remove("direction");
                }
            }

            return dict;
        }
    }
}