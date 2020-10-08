namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Stores a set of parameters for updating the access_mode of resources.
    /// </summary>
    public class UpdateResourceAccessModeParams : BaseParams
    {
        private List<string> m_publicIds = new List<string>();
        private ResourceType m_resourceType = ResourceType.Image;
        private string m_accessMode = "public";
        private string m_type = "upload";

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateResourceAccessModeParams"/> class.
        /// </summary>
        public UpdateResourceAccessModeParams()
        {
        }

        /// <summary>
        /// Gets or sets a possibility to update all resources with the given public IDs (array of up to 100 public_ids).
        /// </summary>
        public List<string> PublicIds
        {
            get { return m_publicIds; }
            set { m_publicIds = value; }
        }

        /// <summary>
        /// Gets or sets the new access mode ("public" or "authenticated").
        /// </summary>
        public string AccessMode
        {
            get { return m_accessMode; }
            set { m_accessMode = value; }
        }

        /// <summary>
        /// Gets or sets a possibility to update resources with the given resource type. Default resource type: "image".
        /// </summary>
        public ResourceType ResourceType
        {
            get { return m_resourceType; }
            set { m_resourceType = value; }
        }

        /// <summary>
        /// Gets or sets a possibility to update resources with the given type. Default resource type: "upload".
        /// </summary>
        public string Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        /// <summary>
        /// Gets or sets prefix.
        ///
        /// Update all assets where the public ID starts with the given
        /// prefix (up to a maximum of 100 matching original assets).
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Gets or sets tag.
        ///
        /// Update all assets with the given tag (up to a maximum of 100 matching original assets).
        /// </summary>
        public string Tag { get; set; }

        private bool PublicIdsExist
        {
            get { return PublicIds != null && PublicIds.Count > 0; }
        }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation.
        /// </summary>
        /// <returns>Sorted dictionary of parameters.</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            if (PublicIdsExist)
            {
                dict.Add("public_ids", PublicIds);
            }
            else if (!string.IsNullOrEmpty(Prefix))
            {
                dict.Add("prefix", Prefix);
            }
            else if (!string.IsNullOrEmpty(Tag))
            {
                dict.Add("tag", Tag);
            }

            dict.Add("access_mode", m_accessMode);

            return dict;
        }
    }
}
