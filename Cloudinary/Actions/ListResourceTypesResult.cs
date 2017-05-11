using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;


namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class ListResourceTypesResult : BaseResult
    {
        [DataMember(Name = "resource_types")]
        protected string[] m_resourceTypes;

        public ResourceType[] ResourceTypes { get; protected set; }

        protected override void OnParse()
        {
            List<ResourceType> types = new List<ResourceType>();

            foreach (var type in m_resourceTypes)
            {
                types.Add(Api.ParseCloudinaryParam<ResourceType>(type));
            }
            ResourceTypes = types.ToArray();
        }

    }
}
