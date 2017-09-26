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
        
    }
}
