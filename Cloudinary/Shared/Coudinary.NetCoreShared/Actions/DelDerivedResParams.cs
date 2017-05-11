using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    public class DelDerivedResParams : BaseParams
    {
        public DelDerivedResParams()
        {
            DerivedResources = new List<string>();
        }

        /// <summary>
        /// Delete all derived resources with the given IDs
        /// </summary>
        public List<string> DerivedResources { get; set; }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            if (DerivedResources == null)
                throw new ArgumentException("DerivedResources can't be null!");

            if (DerivedResources.Count == 0)
                throw new ArgumentException("At least one derived resource must be specified!");
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            if (DerivedResources != null && DerivedResources.Count > 0)
                dict.Add("derived_resource_ids", DerivedResources);

            return dict;
        }
    }
}
