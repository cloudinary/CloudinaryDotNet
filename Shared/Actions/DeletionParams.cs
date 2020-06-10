namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Parameters for deletion of a single asset from your Cloudinary account.
    /// </summary>
    public class DeletionParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeletionParams"/> class.
        /// </summary>
        /// <param name="publicId">The identifier of the uploaded asset. </param>
        public DeletionParams(string publicId)
        {
            Type = "upload";
            ResourceType = ResourceType.Image;
            PublicId = publicId;
        }

        /// <summary>
        /// Gets or sets the identifier of the uploaded asset.
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// Gets or sets the specific type of the asset. Valid values: upload, private and authenticated. Default: upload.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if true, invalidates CDN cached copies of the asset (and all its transformed versions). Default: false.
        /// Note that it usually takes a few minutes (although it might take up to an hour) for the invalidation to
        /// fully propagate through the CDN.
        /// </summary>
        public bool Invalidate { get; set; }

        /// <summary>
        /// Gets or sets the type of asset to destroy. Valid values: image, raw, and video. Default: image.
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (string.IsNullOrEmpty(PublicId))
            {
                throw new ArgumentException("PublicId must be specified in UploadParams!");
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "public_id", PublicId);
            AddParam(dict, "type", Type);
            AddParam(dict, "invalidate", Invalidate);

            return dict;
        }
    }
}
