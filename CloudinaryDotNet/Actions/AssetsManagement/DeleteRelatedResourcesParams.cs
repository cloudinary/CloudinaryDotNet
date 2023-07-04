namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Parameters of DeleteRelatedResourcesParams request.
    /// </summary>
    public class DeleteRelatedResourcesParams : RelatedResourcesParams
    {
        /// <summary>
        /// Gets or sets the list of up to 10 fully_qualified_public_ids given as resource_type/type/public_id.
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
