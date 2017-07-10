using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of tag management
    /// </summary>
    public class ContextParams : BaseParams
    {
        List<string> m_publicIds = new List<string>();

        /// <summary>
        /// A list of Public IDs of images uploaded to Cloudinary.
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
        /// Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The action to perform on image resources using the given tag.
        /// </summary>
        public ContextCommand Command { get; set; }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            // ok
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, "context", Context);
            AddParam(dict, "public_ids", PublicIds);
            AddParam(dict, "command", Api.GetCloudinaryParam<ContextCommand>(Command));

            return dict;
        }
    }

    /// <summary>
    /// The action to perform on image resources using the given tag.
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
