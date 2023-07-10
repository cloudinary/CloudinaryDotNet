namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Params for visual search.
    /// </summary>
    public class VisualSearchParams : BaseParams
    {
        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the image Asset Id.
        /// </summary>
        public string ImageAssetId { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (string.IsNullOrEmpty(ImageUrl) && string.IsNullOrEmpty(ImageAssetId) && string.IsNullOrEmpty(Text))
            {
                throw new ArgumentException("At least one of the VisualSearchParams must be specified!");
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = base.ToParamsDictionary();

            AddParam(dict, "image_url", ImageUrl);
            AddParam(dict, "image_asset_id", ImageAssetId);
            AddParam(dict, "text", Text);

            return dict;
        }
    }
}
