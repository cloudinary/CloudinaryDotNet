namespace CloudinaryDotNet.Actions
{
    using System;

    /// <summary>
    /// Parameters of the request of resource as well as its derived resources.
    /// </summary>
    public class GetResourceParamsAssetId : GetResourceParamsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetResourceParamsAssetId"/> class.
        /// </summary>
        /// <param name="assetId">The asset ID of the resource.</param>
        public GetResourceParamsAssetId(string assetId)
            : base()
        {
            AssetId = assetId;
        }

        /// <summary>
        /// Gets or sets asset id assigned to the requested resource.
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
