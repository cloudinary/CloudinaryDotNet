namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

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
        [JsonIgnore]
        public ResourceType[] ResourceTypes { get; protected set; }

        /// <summary>
        /// Overrides corresponding method of <see cref="BaseResult"/> class.
        /// Populates additional token fields.
        /// </summary>
        /// <param name="source">JSON token received from the server.</param>
        internal override void SetValues(JToken source)
        {
            base.SetValues(source);
            m_resourceTypes = source.ReadValueAsSnakeCase<string[]>(nameof(ResourceTypes));
            List<ResourceType> types = new List<ResourceType>();
            foreach (var type in m_resourceTypes)
            {
                types.Add(Api.ParseCloudinaryParam<ResourceType>(type));
            }

            ResourceTypes = types.ToArray();
        }
    }
}
