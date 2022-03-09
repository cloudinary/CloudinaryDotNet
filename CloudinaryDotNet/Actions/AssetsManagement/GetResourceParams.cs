namespace CloudinaryDotNet.Actions
{
    using System;

    /// <summary>
    /// Parameters of the request of resource as well as its derived resources.
    /// </summary>
    public class GetResourceParams : GetResourceParamsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetResourceParams"/> class.
        /// </summary>
        /// <param name="publicId">The public ID of the resource.</param>
        public GetResourceParams(string publicId)
            : base()
        {
            PublicId = publicId;
        }

        /// <summary>
        /// Gets or sets public id assigned to the requested resource.
        /// </summary>
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
