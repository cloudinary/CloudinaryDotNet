namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Allows to filter resources by specific public identifiers.
    /// </summary>
    public class ListSpecificResourcesParams : ListResourcesParams
    {
        /// <summary>
        /// Gets or sets the public identifiers to list.
        /// When set it overrides usage of <see cref="ListResourcesParams.Direction"/>.
        /// </summary>
        public List<string> PublicIds { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the asset identifiers to list.
        /// </summary>
        public List<string> AssetIds { get; set; } = new List<string>();

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = base.ToParamsDictionary();

            if (PublicIds?.Count > 0)
            {
                AddParam(dict, "public_ids", PublicIds);

                if (dict.ContainsKey("direction"))
                {
                    dict.Remove("direction");
                }
            }

            if (AssetIds?.Count > 0)
            {
                AddParam(dict, "asset_ids", AssetIds);
            }

            return dict;
        }
    }
}
