namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Metadata field to be ordered by.
    /// </summary>
    public enum MetadataReorderBy
    {
        /// <summary>
        /// Order by label.
        /// </summary>
        [EnumMember(Value = "label")]
        Label,

        /// <summary>
        /// Order by external ID.
        /// </summary>
        [EnumMember(Value = "external_id")]
        ExternalId,

        /// <summary>
        /// Order by created at.
        /// </summary>
        [EnumMember(Value = "created_at")]
        CreatedAt,
    }

    /// <summary>
    /// Metadata field reorder direction.
    /// </summary>
    public enum MetadataReorderDirection
    {
        /// <summary>
        /// Ascending.
        /// </summary>
        [EnumMember(Value = "asc")]
        Asc,

        /// <summary>
        /// Descending.
        /// </summary>
        [EnumMember(Value = "desc")]
        Desc,
    }

    /// <summary>
    /// Metadata field reordering parameters.
    /// </summary>
    public class MetadataReorderParams : BaseParams
    {
        /// <summary>
        /// Gets or sets of sets order by.
        /// </summary>
        public MetadataReorderBy OrderBy { get; set; }

        /// <summary>
        /// Gets or sets of direction (optional).
        /// </summary>
        public MetadataReorderDirection? Direction { get; set; }

        /// <inheritdoc/>
        public override void Check()
        {
        }

        /// <inheritdoc/>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var d = base.ToParamsDictionary();
            AddParam(d, "order_by", ApiShared.GetCloudinaryParam(OrderBy));
            if (Direction.HasValue)
            {
                AddParam(d, "direction", ApiShared.GetCloudinaryParam(Direction.Value));
            }

            return d;
        }
    }
}
