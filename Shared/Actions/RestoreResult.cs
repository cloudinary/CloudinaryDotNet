using System;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Results of restoring resources
    /// </summary>
    [DataContract]
    public class RestoreResult : BaseResult
    {
        [DataMember(Name = "resource_type")]
        protected string m_resourceType;

        public ResourceType ResourceType
        {
            get { return Api.ParseCloudinaryParam<ResourceType>(m_resourceType); }
        }
        
    }
}
