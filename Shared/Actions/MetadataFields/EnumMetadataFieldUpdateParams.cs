namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents parameters, required for 'enum' metadata field update.
    /// </summary>
    public class EnumMetadataFieldUpdateParams : MetadataFieldUpdateParams<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumMetadataFieldUpdateParams"/> class.
        /// </summary>
        public EnumMetadataFieldUpdateParams()
        {
            Type = MetadataFieldType.Enum;
        }

        /// <summary>
        /// Validate object model.
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