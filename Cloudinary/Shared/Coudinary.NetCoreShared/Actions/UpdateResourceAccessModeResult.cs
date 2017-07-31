using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class UpdateResourceAccessModeResult : BaseResult
    {
        [DataMember(Name = "updated")]
        public List<object> Updated { get; protected set; }

        [DataMember(Name = "failed")]
        public List<object> Failed { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static UpdateResourceAccessModeResult Parse(Object response)
        {
            return Parse<UpdateResourceAccessModeResult>(response);
        }
    }
}
