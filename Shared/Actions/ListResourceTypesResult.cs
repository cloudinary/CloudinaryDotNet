using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parsed list of resource types.
    /// </summary>
    [DataContract]
    public class ListResourceTypesResult : BaseResult
    {
        /// <summary>
        /// An array of the resource types.
        /// </summary>
        [DataMember(Name = "resource_types")]
        protected string[] m_resourceTypes;

        /// <summary>
        /// An array of the resource types.
        /// </summary>
        public ResourceType[] ResourceTypes { get; protected set; }
        
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            List<ResourceType> types = new List<ResourceType>();
            foreach (var type in m_resourceTypes)
            {
                types.Add(Api.ParseCloudinaryParam<ResourceType>(type));
            }

            ResourceTypes = types.ToArray();
        }
    }
}
