﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Net;
using Newtonsoft.Json;

namespace CloudinaryDotNet.Actions
{
    //[DataContract]
    public class ExplicitResult : RawUploadResult
    {
        [JsonProperty(PropertyName = "format")]
        public string Format { get; protected set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; protected set; }

        [JsonProperty(PropertyName = "eager")]
        public Eager[] Eager { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal new static ExplicitResult Parse(HttpWebResponse response)
        {
            return Parse<ExplicitResult>(response);
        }
    }

    //[DataContract]
    public class Eager
    {
        [JsonProperty(PropertyName = "url")]
        public Uri Uri { get; protected set; }

        [JsonProperty(PropertyName = "secure_url")]
        public Uri SecureUri { get; protected set; }
    }
}
