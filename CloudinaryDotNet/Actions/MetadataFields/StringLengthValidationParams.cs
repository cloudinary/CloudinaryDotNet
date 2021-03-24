namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Strlen validation, relevant to 'string' field types only.
    /// </summary>
    public class StringLengthValidationParams : MetadataValidationParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringLengthValidationParams"/> class.
        /// </summary>
        public StringLengthValidationParams()
        {
            Type = MetadataValidationType.StringLength;
        }

        /// <summary>
        /// Gets or sets the minimum string length, represented by a positive integer.
        /// Default value: 0.
        /// </summary>
        public int? Min { get; set; }

        /// <summary>
        /// Gets or sets the maximum string length, represented by a positive integer.
        /// Default value: 1024.
        /// </summary>
        public int? Max { get; set; }

        /// <summary>
        /// Validates object model.
        /// Either min or max must be given, supplying both is optional.
        /// </summary>
        public override void Check()
        {
            if (Min == null && Max == null)
            {
                throw new ArgumentException("Either Min or Max must be specified");
            }

            if (Min != null && Min.Value < 0)
            {
                throw new ArgumentException("Min must be a positive integer");
            }

            if (Max != null && Max.Value < 0)
            {
                throw new ArgumentException("Max must be a positive integer");
            }
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            base.AddParamsToDictionary(dict);
            if (Min != null)
            {
                dict.Add("min", Min.Value);
            }

            if (Max != null)
            {
                dict.Add("max", Max.Value);
            }
        }
    }
}