using Cloudinary.NetCoreShared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of tag management
    /// </summary>
    public class TagParams : BaseParams
    {

		public TagParams()
		{
			ResourceType = ResourceType.Image;
		}

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
        /// The tag name to assign or remove.
        /// </summary>
        public string Tag { get; set; }

		/// <summary>
		/// The type of resource to tag
		/// </summary>
		public ResourceType ResourceType { get; set; }

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
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            AddParam(dict, Constants.TAG_PARAM_NAME, Tag);
            AddParam(dict, Constants.PUBLIC_IDS, PublicIds);
            AddParam(dict, Constants.COMMAND, Api.GetCloudinaryParam<TagCommand>(Command));

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
        [EnumMember(Value = "add")]
        Add,
        /// <summary>
        /// Remove the given tag from the resources with the given Public IDs.
        /// </summary>
        [EnumMember(Value = "remove")]
        Remove,
        /// <summary>
        /// Assign the given tag to the resources with the given Public IDs while clearing all other tags assigned to these resources.
        /// </summary>
        [EnumMember(Value = "replace")]
        Replace,
        /// <summary>
        /// Assign the given tag to the resources with the given Public IDs while clearing the given tag from all other resources. This means that only the resources with the given Public IDs will have the given tag.
        /// </summary>
        [EnumMember(Value = "set_exclusive")]
        SetExclusive,
        /// <summary>
        /// Assign the given tag to the resources with the given Public IDs while clearing the given tag from all other resources. This means that only the resources with the given Public IDs will have the given tag.
        /// </summary>
        [EnumMember(Value = "remove_all")]
        RemoveAll

    }
}
