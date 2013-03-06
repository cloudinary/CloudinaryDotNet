using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System;
using System.Collections.Generic;

namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Results of tags management
    /// </summary>
    [DataContract]
    public class ExplodeResult : BaseResult
    {
        [DataMember(Name = "status")]
        public string Status { get; protected set; }

        [DataMember(Name = "batch_id")]
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
