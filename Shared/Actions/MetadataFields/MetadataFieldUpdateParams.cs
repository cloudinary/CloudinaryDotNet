namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents parameters, required for metadata field update.
    /// </summary>
    /// <typeparam name="T">Type that can describe the field type.</typeparam>
    public abstract class MetadataFieldUpdateParams<T> : MetadataFieldBaseParams<T>
    {
        /// <summary>
        /// Validates object model.
        /// </summary>
        public override void Check()
        {
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            base.AddParamsToDictionary(dict);
            if (!string.IsNullOrEmpty(Label))
            {
                AddParam(dict, "label", Label);
            }
        }
    }
}