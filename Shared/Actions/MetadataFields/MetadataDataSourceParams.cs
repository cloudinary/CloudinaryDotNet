namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a data source for a given field. This is used in both 'Set' and 'Enum' field types.
    /// The datasource holds a list of the valid values to be used with the corresponding metadata field.
    /// </summary>
    public class MetadataDataSourceParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataDataSourceParams"/> class.
        /// </summary>
        /// <param name="entries">Datasource values.</param>
        public MetadataDataSourceParams(List<EntryParams> entries)
        {
            Values = entries;
        }

        /// <summary>
        /// Gets or sets a list of datasource values.
        /// </summary>
        public List<EntryParams> Values { get; set; }

        /// <summary>
        /// Validates object model.
        /// </summary>
        public override void Check()
        {
            Utils.ShouldNotBeEmpty(() => Values);
            Values.ForEach(value => value.Check());
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            base.AddParamsToDictionary(dict);
            var valuesList = Values.Select(entry => entry.ToParamsDictionary()).ToList();
            dict.Add("values", valuesList);
        }
    }
}