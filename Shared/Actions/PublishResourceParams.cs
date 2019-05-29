using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters to publish resource request.
    /// </summary>
    public class PublishResourceParams : BaseParams
    {
        List<string> m_publicIds = new List<string>();
        ResourceType m_resourceType = ResourceType.Image;
        string m_type = string.Empty;

        /// <summary>
        /// Instantiates the <see cref="PublishResourceParams"/> object.
        /// </summary>
        public PublishResourceParams() { }

        /// <summary>
        /// Publish all resources with the given public IDs.
        /// </summary>
        public List<string> PublicIds
        {
            get { return m_publicIds; }
            set { m_publicIds = value; }
        }

        private bool PublicIdsExist
        {
            get { return PublicIds != null && PublicIds.Count > 0; }
        }

        /// <summary>
        /// Publish resources with the given resource type. Default: "image".
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
                dict.Add("public_ids", PublicIds);

            if (!string.IsNullOrWhiteSpace(m_type))
                dict.Add("type", m_type);

            return dict;
        }
    }
}
