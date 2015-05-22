using System.IO;
using System.Net;
using System.Runtime.Serialization;

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Results of tags management
    /// </summary>
    //[DataContract]
    public class ExplodeResult : BaseResult
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; protected set; }

        [JsonProperty(PropertyName = "batch_id")]
        public string BatchId { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static ExplodeResult Parse(HttpWebResponse response)
        {
            return Parse<ExplodeResult>(response);
        }
    }
}
