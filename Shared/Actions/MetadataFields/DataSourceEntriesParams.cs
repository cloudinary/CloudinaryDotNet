namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents delete datasource entries operation.
    /// </summary>
    public class DataSourceEntriesParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceEntriesParams"/> class.
        /// </summary>
        /// <param name="externalIds">IDs of datasource entries to delete.</param>
        public DataSourceEntriesParams(List<string> externalIds)
        {
            ExternalIds = externalIds;
        }

        /// <summary>
        /// Gets or sets an array of IDs of datasource entries to delete.
        /// </summary>
        public List<string> ExternalIds { get; set; }

        /// <summary>
        /// Validates object model.
        /// </summary>
        public override void Check()
        {
            Utils.ShouldNotBeEmpty(() => ExternalIds);
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            base.AddParamsToDictionary(dict);
            AddParam(dict, "external_ids", ExternalIds);
        }
    }
}