namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Access keys field to be ordered by.
    /// </summary>
    public enum AccessKeysSortBy
    {
        /// <summary>
        /// Sort by api_key.
        /// </summary>
        [EnumMember(Value = "api_key")]
        ApiKey,

        /// <summary>
        /// Sort by created at.
        /// </summary>
        [EnumMember(Value = "created_at")]
        CreatedAt,

        /// <summary>
        /// Sort by name.
        /// </summary>
        [EnumMember(Value = "name")]
        Name,

        /// <summary>
        /// Sort by created at.
        /// </summary>
        [EnumMember(Value = "enabled")]
        Enabled,
    }

    /// <summary>
    /// Access keys field sort order.
    /// </summary>
    public enum AccessKeysSortOrder
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
    /// Parameters of list access keys request.
    /// </summary>
    public class ListAccessKeysParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListAccessKeysParams"/> class.
        /// </summary>
        /// <param name="subAccountId">The ID of the sub-account.</param>
        public ListAccessKeysParams(string subAccountId)
        {
            SubAccountId = subAccountId;
        }

        /// <summary>
        ///  Gets or sets the ID of the sub-account.
        /// </summary>
        public string SubAccountId { get; set; }

        /// <summary>
        ///  Gets or sets how many entries to display on each page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///  Gets or sets which page to return (maximum pages: 100). **Default**: All pages are returned.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets response parameter to sort by. **Possible values**: `api_key`, `created_at`, `name`, `enabled`.
        /// </summary>
        public AccessKeysSortBy? SortBy { get; set; }

        /// <summary>
        /// Gets or sets the order of returned keys. **Possible values**: `desc` (default), `asc`.
        /// </summary>
        public AccessKeysSortOrder? SortOrder { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            Utils.ShouldNotBeEmpty(() => SubAccountId);
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            if (PageSize > 0)
            {
                AddParam(dict, "page_size", PageSize);
            }

            if (Page > 0)
            {
                AddParam(dict, "page", PageSize);
            }

            if (SortBy.HasValue)
            {
                AddParam(dict, "sort_by", ApiShared.GetCloudinaryParam(SortBy.Value));
            }

            if (SortOrder.HasValue)
            {
                AddParam(dict, "sort_order", ApiShared.GetCloudinaryParam(SortOrder.Value));
            }
        }
    }
}
