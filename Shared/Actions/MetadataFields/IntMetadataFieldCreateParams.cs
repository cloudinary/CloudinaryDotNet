namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents parameters, required for 'integer' metadata field creation.
    /// </summary>
    public class IntMetadataFieldCreateParams : MetadataFieldCreateParams<int?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntMetadataFieldCreateParams"/> class.
        /// </summary>
        /// <param name="label">The label of the metadata field.</param>
        public IntMetadataFieldCreateParams(string label)
            : base(label)
        {
            Type = MetadataFieldType.Integer;
        }

        /// <summary>
        /// Validates object model.
        /// </summary>
        public override void Check()
        {
            base.Check();
            var allowedValidationTypes = new List<Type>
            {
                typeof(IntLessThanValidationParams),
                typeof(IntGreaterThanValidationParams),
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
                dict.Add("default_value", DefaultValue.Value);
            }
        }
    }
}