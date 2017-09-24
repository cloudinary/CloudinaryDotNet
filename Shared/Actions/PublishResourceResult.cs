using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class PublishResourceResult : BaseResult
    {
        [DataMember(Name = "published")]
        public List<object> Published { get; protected set; }

        [DataMember(Name = "failed")]
        public List<object> Failed { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static PublishResourceResult Parse(Object response)
        {
            return Parse<PublishResourceResult>(response);
        }
    }
}
