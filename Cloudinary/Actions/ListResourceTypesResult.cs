using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;


namespace CloudinaryDotNet.Actions
{
    //[DataContract]
    public class ListResourceTypesResult : BaseResult
    {
        [JsonProperty(PropertyName = "resource_types")]
        protected string[] m_resourceTypes;

        [JsonIgnore]
        public ResourceType[] ResourceTypes { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static ListResourceTypesResult Parse(HttpWebResponse response)
        {
            ListResourceTypesResult result = Parse<ListResourceTypesResult>(response);

            List<ResourceType> types = new List<ResourceType>();
            foreach (var type in result.m_resourceTypes)
            {
                types.Add(Api.ParseCloudinaryParam<ResourceType>(type));
            }

            result.ResourceTypes = types.ToArray();

            return result;
        }
    }
}
