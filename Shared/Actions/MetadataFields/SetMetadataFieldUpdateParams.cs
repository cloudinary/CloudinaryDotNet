namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents parameters, required for 'set' metadata field update.
    /// </summary>
    public class SetMetadataFieldUpdateParams : MetadataFieldUpdateParams<List<string>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetMetadataFieldUpdateParams"/> class.
        /// </summary>
        public SetMetadataFieldUpdateParams()
        {
            Type = MetadataFieldType.Set;
        }

        /// <summary>
        /// Validates object model.
        /// </summary>
        public override void Check()
        {
            base.Check();
            DataSource?.Check();
            Utils.ShouldNotBeSpecified(() => Validation);
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