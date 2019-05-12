using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters for deletion of a single asset from your Cloudinary account.
    /// </summary>
    public class DeletionParams : BaseParams
    {
        /// <summary>
        /// Instantiates the <see cref="DeletionParams"/> object.
        /// </summary>
        /// <param name="publicId">The identifier of the uploaded asset. </param>
        public DeletionParams(string publicId)
        {
            Type = "upload";
            ResourceType = ResourceType.Image;
            PublicId = publicId;
        }

        /// <summary>
        /// The identifier of the uploaded asset. 
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// The specific type of the asset. Valid values: upload, private and authenticated. Default: upload.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// If true, invalidates CDN cached copies of the asset (and all its transformed versions). Default: false.
        /// Note that it usually takes a few minutes (although it might take up to an hour) for the invalidation to
        /// fully propagate through the CDN.
        /// </summary>
        public bool Invalidate { get; set; }

        /// <summary>
        /// The type of asset to destroy. Valid values: image, raw, and video. Default: image. 
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            if (String.IsNullOrEmpty(PublicId))
                throw new ArgumentException("PublicId must be specified in UploadParams!");
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
