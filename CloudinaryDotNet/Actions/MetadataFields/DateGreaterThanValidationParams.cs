namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Greater-than rule for dates.
    /// </summary>
    public class DateGreaterThanValidationParams : ComparisonValidationParams<DateTime>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateGreaterThanValidationParams"/> class.
        /// </summary>
        /// <param name="value">Value that will be used to compare with.</param>
        public DateGreaterThanValidationParams(DateTime value)
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
            dict.Add("value", Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        }
    }
}