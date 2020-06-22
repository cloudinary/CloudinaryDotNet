namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Parameters for the Explode method.
    /// </summary>
    public class ExplodeParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExplodeParams"/> class.
        /// </summary>
        /// <param name="publicId">The identifier of the uploaded multi-page file (PDF or animated GIF).
        /// Note: The public ID for images does not include a file extension.</param>
        /// <param name="transformation">A transformation to run on all the pages before storing them as derived
        /// images.</param>
        public ExplodeParams(string publicId, Transformation transformation)
        {
            PublicId = publicId;
            Transformation = transformation;
        }

        /// <summary>
        /// Gets or sets the identifier of the uploaded multi-page file (PDF or animated GIF).
        /// Note: The public ID for images does not include a file extension.
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// Gets or sets a transformation to run on all the pages before storing them as derived images.
        /// </summary>
        public Transformation Transformation { get; set; }

        /// <summary>
        /// Gets or sets an HTTP or HTTPS URL to notify your application (a webhook) when the process has completed. Optional.
        /// </summary>
        public string NotificationUrl { get; set; }

        /// <summary>
        /// Gets or sets an optional format to convert the images before storing them in your Cloudinary account. Optional.
        /// Default: png.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the specific type of the asset.
        /// Valid values: upload, private and authenticated. Default: upload.
        /// </summary>
        public AssetType Type { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (string.IsNullOrEmpty(PublicId))
            {
                throw new ArgumentException("PublicId must be set!");
            }

            if (Transformation == null)
            {
                throw new ArgumentException("Transformation must be set!");
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = base.ToParamsDictionary();

            AddParam(dict, "public_id", PublicId);
            AddParam(dict, "notification_url", NotificationUrl);
            AddParam(dict, "format", Format);
            AddParam(dict, "type", Api.GetCloudinaryParam<AssetType>(Type));

            if (Transformation != null)
            {
                AddParam(dict, "transformation", Transformation.Generate());
            }

            return dict;
        }
    }
}
