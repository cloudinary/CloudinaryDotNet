using System.Net.Http;
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

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static RestoreResult Parse(HttpResponseMessage response)
        {
            return Parse<RestoreResult>(response);
        }
    }
}
