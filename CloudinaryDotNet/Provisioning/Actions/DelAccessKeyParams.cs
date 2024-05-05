namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Parameters of delete access key request.
    /// </summary>
    public class DelAccessKeyParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelAccessKeyParams"/> class.
        /// </summary>
        /// <param name="subAccountId">The ID of the sub-account.</param>
        public DelAccessKeyParams(string subAccountId)
        {
            SubAccountId = subAccountId;
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
        }
    }
}
