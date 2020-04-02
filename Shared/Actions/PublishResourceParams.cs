namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Parameters to publish resource request.
    /// </summary>
    public class PublishResourceParams : BaseParams
    {
        private List<string> m_publicIds = new List<string>();
        private ResourceType m_resourceType = ResourceType.Image;
        private string m_type = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishResourceParams"/> class.
        /// </summary>
        public PublishResourceParams()
        {
        }

        /// <summary>
        /// Gets or sets a value for publishing all resources with the given public IDs.
        /// </summary>
        public List<string> PublicIds
        {
            get { return m_publicIds; }
            set { m_publicIds = value; }
        }

        /// <summary>
        /// Gets or sets a value for publishing resources with the given resource type. Default: "image".
        /// </summary>
        public ResourceType ResourceType
        {
            get { return m_resourceType; }
            set { m_resourceType = value; }
        }

        /// <summary>
        /// Gets or sets privacy mode of the image. Valid values: 'private' and 'authenticated'.
        /// Default: 'authenticated'.
        /// </summary>
        public string Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

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

            if (!string.IsNullOrWhiteSpace(m_type))
            {
                dict.Add("type", m_type);
            }

            return dict;
        }
    }
}
