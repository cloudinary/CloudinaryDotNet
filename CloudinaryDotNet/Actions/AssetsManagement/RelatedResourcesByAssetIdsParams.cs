namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Parameters of RelatedResourcesByAssetIds requests.
    /// </summary>
    public abstract class RelatedResourcesByAssetIdsParams : BaseParams
    {
        /// <summary>
        /// Gets or sets the Asset ID of the resource to update.
        /// </summary>
        public string AssetId { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (string.IsNullOrEmpty(AssetId))
            {
                throw new ArgumentException("AssetId must be set!");
            }
        }
    }
}
