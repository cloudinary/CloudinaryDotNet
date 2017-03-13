﻿using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    public class RestoreParams : BaseParams
    {
        List<string> m_publicIds = new List<string>();
        ResourceType m_resourceType = ResourceType.Image;

        public RestoreParams() { }

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

        /// <summary>
        /// Validate object model
        /// </summary>
        public override void Check()
        {
            if (!PublicIdsExist)
            {
                throw new ArgumentException("At least one PublicId must be specified!");
            }
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
            return dict;
        }
    }
}
