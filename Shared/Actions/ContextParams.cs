using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of context management.
    /// </summary>
    public class ContextParams : BaseParams
    {
        List<string> m_publicIds = new List<string>();

        /// <summary>
        /// A list of Public IDs of assets uploaded to Cloudinary.
        /// </summary>
        public List<string> PublicIds
        {
            get { return m_publicIds; }
            set { m_publicIds = value; }
        }

        /// <summary>
        /// The context name to assign or remove.
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// (Only relevant when adding context) A list of the key-value pairs of general textual context metadata to
        /// add to the asset.
        /// </summary>
        public StringDictionary ContextDict { get; set; }

        /// <summary>
        /// (Optional) The specific type of the asset. 
        /// Valid values: upload, private and authenticated. Default: upload.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The action to perform on assets using the given context.
        /// </summary>
        public ContextCommand Command { get; set; }

        /// <summary>
        /// Validate object model
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

            return dict;
        }
    }

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
        RemoveAll
    }
}
