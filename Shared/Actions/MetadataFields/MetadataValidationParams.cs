namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the base class for metadata fields validation mechanisms.
    /// </summary>
    public abstract class MetadataValidationParams : BaseParams
    {
        /// <summary>
        /// Gets or sets the type of value that can be assigned to the metadata field.
        /// </summary>
        public MetadataValidationType Type { get; set; }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            AddParam(dict, "type", Api.GetCloudinaryParam(Type));
        }
    }
}