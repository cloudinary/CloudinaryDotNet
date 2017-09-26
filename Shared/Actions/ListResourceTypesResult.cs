using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class ListResourceTypesResult : BaseResult
    {
        [DataMember(Name = "resource_types")]
        protected string[] m_resourceTypes;

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
