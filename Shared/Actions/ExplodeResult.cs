using System;
using System.Runtime.Serialization;

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
        internal static ExplodeResult Parse(Object response)
        {
            return Parse<ExplodeResult>(response);
        }
    }
}
