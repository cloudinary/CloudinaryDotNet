using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class SearchResult : BaseResult
    {
        [DataMember(Name = "total_count")]
        public int TotalCount { get; protected set; }

        [DataMember(Name = "time")]
        public long Time { get; protected set; }

        [DataMember(Name = "resources")]
        public List<object> Resources { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static SearchResult Parse(Object response)
        {
            return Parse<SearchResult>(response);
        }
    }
}
