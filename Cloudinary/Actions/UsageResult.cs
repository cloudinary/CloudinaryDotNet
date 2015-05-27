using Newtonsoft.Json;
using System;
using System.Net;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    //[DataContract]
    public class UsageResult : BaseResult
    {
        [JsonProperty(PropertyName = "plan")]
        public string Plan { get; protected set; }

        [JsonProperty(PropertyName = "last_updated")]
        public DateTime LastUpdated { get; protected set; }

        [JsonProperty(PropertyName = "objects")]
        public Usage Objects { get; protected set; }

        [JsonProperty(PropertyName = "bandwidth")]
        public Usage Bandwidth { get; protected set; }

        [JsonProperty(PropertyName = "storage")]
        public Usage Storage { get; protected set; }

        [JsonProperty(PropertyName = "requests")]
        public int Requests { get; protected set; }

        [JsonProperty(PropertyName = "resources")]
        public int Resources { get; protected set; }

        [JsonProperty(PropertyName = "derived_resources")]
        public int DerivedResources { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static UsageResult Parse(HttpWebResponse response)
        {
            return Parse<UsageResult>(response);
        }
    }

    //[DataContract]
    public class Usage
    {
        [JsonProperty(PropertyName = "usage")]
        public long Used { get; protected set; }

        [JsonProperty(PropertyName = "limit")]
        public long Limit { get; protected set; }

        [JsonProperty(PropertyName = "used_percent")]
        public float UsedPercent { get; protected set; }
    }
}
