namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// And validation, relevant for all field types.
    /// Allows to include more than one validation rule to be evaluated.
    /// </summary>
    public class AndValidationParams : MetadataValidationParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndValidationParams"/> class.
        /// </summary>
        /// <param name="rules">List of combined rules.</param>
        public AndValidationParams(List<MetadataValidationParams> rules)
        {
            Type = MetadataValidationType.And;
            Rules = rules;
        }

        /// <summary>
        /// Gets or sets rules combined with an 'AND' logic relation between them.
        /// </summary>
        public List<MetadataValidationParams> Rules { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            Utils.ShouldNotBeEmpty(() => Rules);
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            base.AddParamsToDictionary(dict);
            var rulesList = Rules.Select(entry => entry.ToParamsDictionary()).ToList();
            dict.Add("rules", rulesList);
        }
    }
}