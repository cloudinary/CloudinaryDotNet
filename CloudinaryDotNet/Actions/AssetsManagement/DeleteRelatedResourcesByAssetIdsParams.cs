namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Parameters of DeleteRelatedResourcesByAssetIds request.
    /// </summary>
    public class DeleteRelatedResourcesByAssetIdsParams : RelatedResourcesByAssetIdsParams
    {
        /// <summary>
        /// Gets or sets the list of up to 10 asset IDs.
        /// </summary>
        public List<string> AssetsToUnrelate { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            base.Check();

            if (AssetsToUnrelate == null || AssetsToUnrelate.Count == 0)
            {
                throw new ArgumentException("AssetsToUnrelate must be specified!");
            }
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = base.ToParamsDictionary();

            AddParam(dict, "assets_to_unrelate", AssetsToUnrelate);

            return dict;
        }
    }
}
