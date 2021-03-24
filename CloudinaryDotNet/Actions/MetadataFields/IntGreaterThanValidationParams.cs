namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Greater-than rule for integers.
    /// </summary>
    public class IntGreaterThanValidationParams : ComparisonValidationParams<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntGreaterThanValidationParams"/> class.
        /// </summary>
        /// <param name="value">Value that will be used to compare with.</param>
        public IntGreaterThanValidationParams(int value)
            : base(value)
        {
            Type = MetadataValidationType.GreaterThan;
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            base.AddParamsToDictionary(dict);
            dict.Add("value", Value);
        }
    }
}