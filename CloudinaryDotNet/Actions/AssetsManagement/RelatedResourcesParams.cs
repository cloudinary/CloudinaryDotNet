namespace CloudinaryDotNet.Actions
{
    using System;

    /// <summary>
    /// Base Parameters of RelatedResources requests.
    /// </summary>
    public abstract class RelatedResourcesParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedResourcesParams"/> class.
        /// </summary>
        public RelatedResourcesParams()
        {
            ResourceType = ResourceType.Image;
            Type = "upload";
        }

        /// <summary>
        /// Gets or sets the type of file for which to add related resources. Possible values: image, raw, video.
        /// Default: image. Optional.
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the storage type: upload, private, authenticated, facebook, twitter, gplus, instagram_name,
        /// gravatar, youtube, hulu, vimeo, animoto, worldstarhiphop or dailymotion. Default: upload. Optional.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the public ID of the resource to update.
        /// </summary>s
        public string PublicId { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (string.IsNullOrEmpty(PublicId))
            {
                throw new ArgumentException("PublicId must be set!");
            }
        }
    }
}
