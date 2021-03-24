namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Base class for comparison validations.
    /// </summary>
    /// <typeparam name="T">Type that can describe the value for validation.</typeparam>
    public abstract class ComparisonValidationParams<T> : MetadataValidationParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComparisonValidationParams{T}"/> class.
        /// </summary>
        /// <param name="value">Value that will be used to compare with.</param>
        protected ComparisonValidationParams(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets or sets the value for validation.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to check if equals.
        /// Default value: false.
        /// </summary>
        public bool IsEqual { get; set; }

        /// <summary>
        /// Validates object model.
        /// </summary>
        public override void Check()
        {
            Utils.ShouldBeSpecified(() => Value);
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            base.AddParamsToDictionary(dict);
            AddParam(dict, "equals", IsEqual);
        }
    }
}