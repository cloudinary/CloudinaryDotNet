using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of tag management
    /// </summary>
    public class TagParams : BaseParams
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
        
        public ResourceType ResourceType { get; set; } 
        /// <summary>
        /// The tag name to assign or remove.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The action to perform on image resources using the given tag.
        /// </summary>
        public TagCommand Command { get; set; }

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
            SortedDictionary<string, object> dict = new SortedDictionary<string, object>();

            AddParam(dict, "tag", Tag);
            AddParam(dict, "public_ids", PublicIds);
            AddParam(dict, "resourcetype", ResourceType);
            AddParam(dict, "command", Api.GetCloudinaryParam<TagCommand>(Command));

            return dict;
        }
    }

    /// <summary>
    /// The action to perform on image resources using the given tag.
    /// </summary>
    public enum TagCommand
    {
        /// <summary>
        /// Assign the given tag to the resources with the given Public IDs.
        /// </summary>
        [Description("add")]
        Add,
        /// <summary>
        /// Remove the given tag from the resources with the given Public IDs.
        /// </summary>
        [Description("remove")]
        Remove,
        /// <summary>
        /// Assign the given tag to the resources with the given Public IDs while clearing all other tags assigned to these resources.
        /// </summary>
        [Description("replace")]
        Replace,
        /// <summary>
        /// Assign the given tag to the resources with the given Public IDs while clearing the given tag from all other resources. This means that only the resources with the given Public IDs will have the given tag.
        /// </summary>
        [Description("set_exclusive")]
        SetExclusive
    }
}
