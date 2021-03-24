namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the values to be applied to custom metadata fields of already uploaded assets.
    /// </summary>
    public class MetadataUpdateParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataUpdateParams"/> class.
        /// </summary>
        public MetadataUpdateParams()
        {
            Type = "upload";
            ResourceType = ResourceType.Image;
        }

        /// <summary>
        /// Gets or sets a list of Public IDs of assets uploaded to Cloudinary.
        /// </summary>
        public List<string> PublicIds { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets a dictionary of custom metadata fields (by external_id) and the values to assign to each of them.
        /// Any metadata-value pairs given are merged with any existing metadata-value pairs
        /// (an empty value for an existing metadata field clears the value).
        /// </summary>
        public StringDictionary Metadata { get; set; } = new StringDictionary();

        /// <summary>
        /// Gets or sets the type of file. Possible values: image, raw, video. Default: image.
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the storage type. Default: upload. Valid values: upload, private, authenticated.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Validates object model.
        /// </summary>
        public override void Check()
        {
            Utils.ShouldNotBeEmpty(() => PublicIds);
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            AddParam(dict, "public_ids", PublicIds);
            AddParam(dict, Constants.METADATA_PARAM_NAME, Utils.SafeJoin("|", Metadata.SafePairs));
            AddParam(dict, "type", Type);
        }
    }
}
