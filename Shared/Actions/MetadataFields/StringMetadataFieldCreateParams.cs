namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents parameters, required for 'string' metadata field creation.
    /// </summary>
    public class StringMetadataFieldCreateParams : MetadataFieldCreateParams<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringMetadataFieldCreateParams"/> class.
        /// </summary>
        /// <param name="label">The label of the metadata field.</param>
        public StringMetadataFieldCreateParams(string label)
            : base(label)
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