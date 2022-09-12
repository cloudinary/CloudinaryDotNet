namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Allows to filter resources by asset folder.
    /// </summary>
    public class ListResourcesByAssetFolderParams : ListResourcesParams
    {
        /// <summary>
        /// Gets or sets the asset folder to filter resources.
        /// </summary>
        public string AssetFolder { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        /// <exception cref="System.ArgumentException">Tag must be set to list resource by tag.</exception>
        public override void Check()
        {
            base.Check();

            if (string.IsNullOrEmpty(AssetFolder))
            {
                throw new ArgumentException("AssetFolder must be set to filter resources by AssetFolder!");
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            if (dict.ContainsKey("type"))
            {
                dict.Remove("type");
            }

            AddParam(dict, "asset_folder", AssetFolder);

            return dict;
        }
    }
}
