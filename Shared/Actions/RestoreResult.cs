using System;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parsed results of resources restore.
    /// </summary>
    [DataContract]
    public class RestoreResult : BaseResult
    {
        /// <summary>
        /// The type of file. Possible values: image, raw, video. Default: image.
        /// </summary>
        [DataMember(Name = "resource_type")]
        protected string m_resourceType;

        /// <summary>
        /// Get the cloudinary resource type.
        /// </summary>
        public ResourceType ResourceType
        {
            get { return Api.ParseCloudinaryParam<ResourceType>(m_resourceType); }
        }
        
    }
}
