namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Less-than rule for integers.
    /// </summary>
    public class IntLessThanValidationParams : ComparisonValidationParams<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntLessThanValidationParams"/> class.
        /// </summary>
        /// <param name="value">Value that will be used to compare with.</param>
        public IntLessThanValidationParams(int value)
            : base(value)
        {
            Type = MetadataValidationType.LessThan;
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