using System.IO;
using System.Net;
using System.Runtime.Serialization;
//using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    //[DataContract]
    public class UpdateTransformResult : BaseResult
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; protected set; }

        [JsonProperty(PropertyName = "allowed_for_strict")]
        public bool Strict { get; protected set; }

        [JsonProperty(PropertyName = "used")]
        public bool Used { get; protected set; }

        [JsonProperty(PropertyName = "info")]
        public Dictionary<string, string>[] Info { get; protected set; }

        [JsonProperty(PropertyName = "derived")]
        public TransformDerived[] Derived { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static UpdateTransformResult Parse(HttpWebResponse response)
        {
            return Parse<UpdateTransformResult>(response);
        }
    }
}
