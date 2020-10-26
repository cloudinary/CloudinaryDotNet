namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The action to perform on image resources using the given context.
    /// </summary>
    public enum ContextCommand
    {
        /// <summary>
        /// Assign the given context to the resources with the given Public IDs.
        /// </summary>
        [EnumMember(Value = "add")]
        Add,

        /// <summary>
        /// Remove all contexts from resources with the given Public IDs.
        /// </summary>
        [EnumMember(Value = "remove_all")]
        RemoveAll,
    }

    /// <summary>
    /// Parameters for context management.
    /// </summary>
    public class ContextParams : BaseParams
    {
        /// <summary>
        /// Gets or sets a list of Public IDs of assets uploaded to Cloudinary.
        /// </summary>
        public List<string> PublicIds { get; set; }

        /// <summary>
        /// Gets or sets the context name to assign or remove.
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// Gets or sets a list of the key-value pairs of general textual context metadata to
        /// add to the asset. Only relevant when adding context.
        /// </summary>
        public StringDictionary ContextDict { get; set; }

        /// <summary>
        /// Gets or sets the specific type of the asset.
        /// Valid values: upload, private and authenticated. Default: upload. Optional.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the action to perform on assets using the given context.
        /// </summary>
        public ContextCommand Command { get; set; }

        /// <summary>
        /// Gets or sets (Optional) The type of asset.
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            // ok
        }

        /// <inheritdoc />
        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            var dict = base.ToParamsDictionary();

            var contextPairs = new List<string>();

            if (ContextDict?.SafePairs != null)
            {
                contextPairs.AddRange(ContextDict.SafePairs);
            }

            if (!string.IsNullOrEmpty(Context))
            {
                contextPairs.Add(Context);
            }

            if (contextPairs.Count > 0)
            {
                AddParam(dict, Constants.CONTEXT_PARAM_NAME, Utils.SafeJoin("|", contextPairs));
            }

            AddParam(dict, Constants.PUBLIC_IDS, PublicIds);
            AddParam(dict, Constants.COMMAND, ApiShared.GetCloudinaryParam(Command));
            AddParam(dict, Constants.RESOURCE_TYPE, ApiShared.GetCloudinaryParam(ResourceType));

            return dict;
        }
    }
}
