namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Parameters of list sub-accounts request.
    /// </summary>
    public class ListSubAccountsParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListSubAccountsParams"/> class.
        /// </summary>
        public ListSubAccountsParams()
        {
            Ids = new List<string>();
        }

        /// <summary>
        /// Gets or sets whether to return enabled sub-accounts only (true) or disabled accounts (false).
        /// Default: all accounts are returned(both enabled and disabled).
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        /// Gets or sets the list of up to 100 sub-account IDs. When provided, other parameters are ignored.
        /// </summary>
        public List<string> Ids { get; set; }

        /// <summary>
        /// Gets or sets accounts where the name begins with the specified case-insensitive string.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            // ok
        }

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

            if (Ids != null && Ids.Any())
            {
                AddParam(dict, "ids", Ids);
            }

            if (!string.IsNullOrEmpty(Prefix))
            {
                AddParam(dict, "prefix", Prefix);
            }
        }
    }
}
