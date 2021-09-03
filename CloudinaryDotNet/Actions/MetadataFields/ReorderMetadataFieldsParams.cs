namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the values to be applied to reorder metadata fields.
    /// </summary>
    public class ReorderMetadataFieldsParams : BaseParams
    {
        /// <summary>
        /// Gets or sets criteria for the order. Currently supports only value.
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Gets or sets direction of order (either asc (default) or desc).
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// Validates object model.
        /// </summary>
        public override void Check()
        {
            Utils.ShouldNotBeEmpty(() => OrderBy);
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            AddParam(dict, "order_by", OrderBy);
            if (!string.IsNullOrEmpty(Direction))
            {
                AddParam(dict, "direction", Direction);
            }
        }
    }
}
