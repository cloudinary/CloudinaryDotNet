using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters for deletion of resource from cloudinary
    /// </summary>
    public class DeletionParams : BaseParams
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DeletionParams(string publicId)
        {
            Type = "upload";
            ResourceType = ResourceType.Image;
            PublicId = publicId;
        }

        /// <summary>
        /// The identifier of the uploaded image
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// The type of the image you want to delete. Default: "upload".
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Whether to invalidate CDN cache copies of a previously uploaded image that shares the same public ID. Default: false.
        /// </summary>
        /// <value>
        ///   <c>true</c> to invalidate; otherwise, <c>false</c>.
        /// </value>
        public bool Invalidate { get; set; }

        /// <summary>
        /// The type of resource to delete
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            if (String.IsNullOrEmpty(PublicId))
                throw new ArgumentException("PublicId must be specified in UploadParams!");
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
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
