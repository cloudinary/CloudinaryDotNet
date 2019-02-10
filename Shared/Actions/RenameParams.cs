using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters for renaming assets immediately and permanently update in your cloud storage. 
    /// </summary>
    public class RenameParams : BaseParams
    {
        /// <summary>
        /// Instantiates the <see cref="RenameParams"/> object.
        /// </summary>
        /// <param name="fromPublicId">The current identifier of the uploaded asset.</param>
        /// <param name="toPublicId">The new identifier to assign to the uploaded asset.</param>
        public RenameParams(string fromPublicId, string toPublicId)
        {
            FromPublicId = fromPublicId;
            ToPublicId = toPublicId;
            ResourceType = ResourceType.Image;
        }

        /// <summary>
        /// The current identifier of the uploaded asset.
        /// </summary>
        /// <value>
        /// Existing public id.
        /// </value>
        public string FromPublicId { get; set; }

        /// <summary>
        /// The new identifier to assign to the uploaded asset.
        /// </summary>
        /// <value>
        /// Target public id.
        /// </value>
        public string ToPublicId { get; set; }

        /// <summary>
        /// The type of asset to rename. 
        /// Valid values: image, raw, and video. 
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// The specific type of the resource. 
        /// Valid values: upload, private and authenticated.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The new type for the resource. 
        /// Valid values: upload, private and authenticated.
        /// </summary>
        public string ToType { get; set; }

        /// <summary>
        /// Whether to overwrite an existing asset with the target public ID. Default: false.
        /// </summary>
        /// <value>
        ///   <c>true</c> to overwrite; otherwise, <c>false</c>.
        /// </value>
        public bool Overwrite { get; set; }

        /// <summary>
        /// Whether to invalidate CDN cache copies of a previously uploaded image that shares the same public ID. Default: false.
        /// </summary>
        /// <value>
        ///   <c>true</c> to invalidate; otherwise, <c>false</c>.
        /// </value>
        public bool Invalidate { get; set; }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "from_public_id", FromPublicId);
            AddParam(dict, "to_public_id", ToPublicId);
            AddParam(dict, "overwrite", Overwrite);
            AddParam(dict, "type", Type);
            AddParam(dict, "to_type", ToType);
            AddParam(dict, "invalidate", Invalidate);
            return dict;
        }

        /// <summary>
        /// Validate object model.
        /// </summary>
        /// <exception cref="System.ArgumentException">
        /// FromPublicId can't be null!
        /// or
        /// ToPublicId can't be null!
        /// </exception>
        public override void Check()
        {
            if (String.IsNullOrEmpty(FromPublicId))
                throw new ArgumentException("FromPublicId can't be null!");

            if (String.IsNullOrEmpty(ToPublicId))
                throw new ArgumentException("ToPublicId can't be null!");
        }
    }
}
