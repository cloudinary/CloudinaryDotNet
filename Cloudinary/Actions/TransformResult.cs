using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
//using System.Runtime.Serialization.Json;
using System.Text;

namespace CloudinaryDotNet.Actions
{
    //[DataContract]
    public class TransformResult : BaseResult
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static TransformResult Parse(HttpWebResponse response)
        {
            return Parse<TransformResult>(response);
        }
    }
}
