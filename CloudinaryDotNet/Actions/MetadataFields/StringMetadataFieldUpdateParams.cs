namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents parameters, required for 'string' metadata field update.
    /// </summary>
    public class StringMetadataFieldUpdateParams : MetadataFieldUpdateParams<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringMetadataFieldUpdateParams"/> class.
        /// </summary>
        public StringMetadataFieldUpdateParams()
        {
            Type = MetadataFieldType.String;
        }

        /// <summary>
        /// Validates object model.
        /// </summary>
        public override void Check()
        {
            base.Check();
            var allowedValidationTypes = new List<Type>
            {
                typeof(StringLengthValidationParams),
                typeof(AndValidationParams),
            };
            CheckScalarDataModel(allowedValidationTypes);
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
                dict.Add("default_value", DefaultValue);
            }
        }
    }
}