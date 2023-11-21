namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Parameters of generate access key request.
    /// </summary>
    public class GenerateAccessKeyParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateAccessKeyParams"/> class.
        /// </summary>
        /// <param name="subAccountId">The ID of the sub-account.</param>
        public GenerateAccessKeyParams(string subAccountId)
        {
            SubAccountId = subAccountId;
        }

        /// <summary>
        ///  Gets or sets the ID of the sub-account.
        /// </summary>
        public string SubAccountId { get; set; }

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
