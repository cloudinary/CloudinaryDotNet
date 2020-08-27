namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Parameters of create sub-account request.
    /// </summary>
    public class CreateSubAccountParams : BaseSubAccountParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSubAccountParams"/> class.
        /// </summary>
        /// <param name="subAccountName">The name of the sub-account.</param>
        public CreateSubAccountParams(string subAccountName)
        {
            Name = subAccountName;
            Enabled = true;
        }

        /// <summary>
        /// Gets or sets he ID of another sub-account, from which to copy all of
        /// the following  settings:Size limits, Timed limits, and Flags.
        /// </summary>
        public string BaseSubAccountId { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            Utils.ShouldNotBeEmpty(() => Name);
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            base.AddParamsToDictionary(dict);
            if (!string.IsNullOrEmpty(BaseSubAccountId))
            {
                AddParam(dict, "base_sub_account_id", BaseSubAccountId);
            }
        }
    }
}
