namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents parameters, required for 'set' metadata field creation.
    /// </summary>
    public class SetMetadataFieldCreateParams : MetadataFieldCreateParams<List<string>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetMetadataFieldCreateParams"/> class.
        /// </summary>
        /// <param name="label">The label of the metadata field.</param>
        public SetMetadataFieldCreateParams(string label)
            : base(label)
        {
            Type = MetadataFieldType.Set;
        }

        /// <summary>
        /// Validates object model.
        /// </summary>
        public override void Check()
        {
            base.Check();

            if (Mandatory)
            {
                Utils.ShouldNotBeEmpty(() => DefaultValue);
            }

            Utils.ShouldBeSpecified(() => DataSource);
            Utils.ShouldNotBeSpecified(() => Validation);
            DataSource.Check();
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            base.AddParamsToDictionary(dict);
            if (DefaultValue != null)
            {
                AddParam(dict, "default_value", DefaultValue);
            }
        }
    }
}