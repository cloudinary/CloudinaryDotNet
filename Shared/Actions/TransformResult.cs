using System;
using System.Runtime.Serialization;

namespace CloudinaryDotNet.Actions
{
    [DataContract]
    public class TransformResult : BaseResult
    {
        [DataMember(Name = "message")]
        public string Message { get; protected set; }

        /// <summary>
        /// Parses HTTP response and creates new instance of this class
        /// </summary>
        /// <param name="response">HTTP response</param>
        /// <returns>New instance of this class</returns>
        internal static TransformResult Parse(Object response)
        {
            return Parse<TransformResult>(response);
        }
    }
}
