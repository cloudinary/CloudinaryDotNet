namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Parameters of update access key request.
    /// </summary>
    public class UpdateAccessKeyParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAccessKeyParams"/> class.
        /// </summary>
        /// <param name="subAccountId">The ID of the sub-account.</param>
        /// <param name="apiKey">The Api Key.</param>
        public UpdateAccessKeyParams(string subAccountId, string apiKey)
        {
            SubAccountId = subAccountId;
            ApiKey = apiKey;
        }

        /// <summary>
        ///  Gets or sets the ID of the sub-account.
        /// </summary>
        public string SubAccountId { get; set; }

        /// <summary>
        ///  Gets or sets the ID of the sub-account.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the access key.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets whether the access key is enabled.
        /// </summary>
        public bool? Enabled { get; set; }

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
            if (!string.IsNullOrEmpty(Name))
            {
                AddParam(dict, "name", Name);
            }

            if (Enabled.HasValue)
            {
                AddParam(dict, "enabled", Enabled);
            }
        }
    }
}
