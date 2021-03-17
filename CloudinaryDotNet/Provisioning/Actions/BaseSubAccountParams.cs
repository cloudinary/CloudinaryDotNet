namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Base parameters for sub-account modification requests.
    /// </summary>
    public abstract class BaseSubAccountParams : BaseParams
    {
        /// <summary>
        /// Gets or sets the display name as shown in the management console.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a case-insensitive cloud name comprised of alphanumeric and underscore characters.
        /// Generates an error if the specified cloud name is not unique across all Cloudinary accounts.
        /// Note: Once created, the name can only be changed for accounts with fewer than 1000 assets.
        /// </summary>
        public string CloudName { get; set; }

        /// <summary>
        /// Gets or sets any custom attributes you want to associate with the sub-account, as a map/hash of key/value pairs.
        /// </summary>
        public StringDictionary CustomAttributes { get; set; }

        /// <summary>
        /// Gets or sets whether the sub-account is enabled. Default: true.
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            if (Enabled.HasValue)
            {
                AddParam(dict, "enabled", Enabled.Value);
            }

            if (!string.IsNullOrEmpty(Name))
            {
                AddParam(dict, "name", Name);
            }

            if (!string.IsNullOrEmpty(CloudName))
            {
                AddParam(dict, "cloud_name", CloudName);
            }

            if (CustomAttributes != null)
            {
                dict.Add("custom_attributes", Utils.SafeJoin("|", CustomAttributes.SafePairs));
            }
        }
    }
}
