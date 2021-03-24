namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents parameters, required for metadata field creation.
    /// </summary>
    /// <typeparam name="T">Type that can describe the field type.</typeparam>
    public abstract class MetadataFieldCreateParams<T> : MetadataFieldBaseParams<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataFieldCreateParams{T}"/> class.
        /// </summary>
        /// <param name="label">The label of the metadata field.</param>
        protected MetadataFieldCreateParams(string label)
        {
            Label = label;
        }

        /// <summary>
        /// Validates object model.
        /// </summary>
        public override void Check()
        {
            Utils.ShouldBeSpecified(() => Label);

            if (Mandatory)
            {
                Utils.ShouldBeSpecified(() => DefaultValue);
            }
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            base.AddParamsToDictionary(dict);
            AddParam(dict, "label", Label);
        }
    }
}