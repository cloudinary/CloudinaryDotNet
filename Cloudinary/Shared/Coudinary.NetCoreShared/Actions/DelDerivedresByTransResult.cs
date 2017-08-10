using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class DelDerivedresByTransResult : BaseResult
    {
        [DataMember(Name = "deleted")]
        public Dictionary<string, string> Deleted { get; protected set; }

        [DataMember(Name = "next_cursor")]
        public string NextCursor { get; protected set; }

        [DataMember(Name = "partial")]
        public bool Partial { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static DelDerivedresByTransResult Parse(Object response)
        {
            return Parse<DelDerivedresByTransResult>(response);
        }
    }
}
