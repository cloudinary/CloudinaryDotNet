using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    public class PublishResourceParams : BaseParams
    {
        List<string> m_publicIds = new List<string>();
        ResourceType m_resourceType = ResourceType.Image;
        string m_toType = string.Empty;

        public PublishResourceParams() { }

        /// <summary>
        /// Restore all resources with the given public IDs
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
        /// Restore resources with the given resource type. Default resource type: "image"
        /// </summary>
        public ResourceType ResourceType
        {
            get { return m_resourceType; }
            set { m_resourceType = value; }
        }

        public string ToType
        {
            get { return m_toType; }
            set { m_toType = value; }
        }

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            
        }

        /// <summary>
        /// Maps object model to dictionary of parameters in cloudinary notation
        /// </summary>
        /// <returns>Sorted dictionary of parameters</returns>
        public override SortedDictionary<string, object> ToParamsDictionary()
        {
            SortedDictionary<string, object> dict = base.ToParamsDictionary();

            if (PublicIdsExist)
            {
                dict.Add("public_ids", PublicIds);
            }
            dict.Add("resource_type", this.m_resourceType);
            if (!string.IsNullOrWhiteSpace(this.m_toType))
                dict.Add("to_type", this.m_toType);

            return dict;
        }
    }
}
