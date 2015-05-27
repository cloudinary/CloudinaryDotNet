using System.IO;
using System.Net;
using System.Runtime.Serialization;
//using System.Runtime.Serialization.Json;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Results of tags management
    /// </summary>
    //[DataContract]
    public class MultiResult : BaseResult
    {
        [JsonProperty(PropertyName = "url")]
        public Uri Uri { get; protected set; }

        [JsonProperty(PropertyName = "secure_url")]
        public Uri SecureUri { get; protected set; }

        [JsonProperty(PropertyName = "public_id")]
        public string PublicId { get; protected set; }

        [JsonProperty(PropertyName = "version")]
        public string Version { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static MultiResult Parse(HttpWebResponse response)
        {
            return Parse<MultiResult>(response);
        }
    }
}
