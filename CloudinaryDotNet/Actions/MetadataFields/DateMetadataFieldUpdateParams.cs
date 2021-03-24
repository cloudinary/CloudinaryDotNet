namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Represents parameters, required for 'date' metadata field update.
    /// </summary>
    public class DateMetadataFieldUpdateParams : MetadataFieldUpdateParams<DateTime?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateMetadataFieldUpdateParams"/> class.
        /// </summary>
        public DateMetadataFieldUpdateParams()
        {
            Type = MetadataFieldType.Date;
        }

        /// <summary>
        /// Validates object model.
        /// </summary>
        public override void Check()
        {
            base.Check();
            var allowedValidationTypes = new List<Type>
            {
                typeof(DateGreaterThanValidationParams),
                typeof(DateLessThanValidationParams),
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
                AddParam(dict, "default_value", DefaultValue.Value.ToString(
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture));
            }
        }
    }
}